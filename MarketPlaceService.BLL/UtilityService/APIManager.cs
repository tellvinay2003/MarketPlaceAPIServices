using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using MarketPlaceService.Entities;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL;
using MarketPlaceService.DAL.Models;

namespace MarketPlaceService.BLL.UtilityService
{
    public interface IAPIManagerService
    {
        Guid TraceId { get; set; }
        Task<string> GetResponseAsync(TravelStudioControllers controllers, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId);
        Task<string> GetResponseAsync(TravelStudioControllers controllers, string url);
        Task<string> PostResponseAsync(object objRequest, TravelStudioControllers controllers, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId);
        Task<string> PutResponseAsync(object objRequest, TravelStudioControllers controllers, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId);
    }

    public class APIManagerService : IAPIManagerService
    {
        private readonly IAPIManagerHelperService _apiManagerHelperService;
        private Guid _traceId;
        public Guid TraceId
        {
            get
            {
                if (_traceId == Guid.Empty)
                    _traceId = Guid.NewGuid();
                return _traceId;
            }
            set
            {
                _traceId = value;
            }
        }
        public APIManagerService(IAPIManagerHelperService apiManagerHelperService)
        {
            _apiManagerHelperService = apiManagerHelperService;
        }
        static HttpClient client = new HttpClient();

        public async Task<string> GetResponseAsync(TravelStudioControllers controllers, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId)
        {
            var url = _apiManagerHelperService.GetUrl(controllers, additionalRoute, routeParameters, optionalParameters, entityType, entityId);

            if (string.IsNullOrEmpty(url))
                return null;

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get
                };
                request.Headers.Add("TraceId", TraceId.ToString());
                response = await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception($"GetResponseAsync (Error = {ex.Message})");
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> PostResponseAsync(object objRequest, TravelStudioControllers controllers, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId)
        {
            var url = _apiManagerHelperService.GetUrl(controllers, additionalRoute, routeParameters, optionalParameters, entityType, entityId);

            if (string.IsNullOrEmpty(url))
                return null;

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var jsonObject = JsonConvert.SerializeObject(objRequest);
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                response = await client.PostAsync(url, content);
            }
            catch (Exception ex)
            {
                throw new Exception($"PostResponseAsync (Error = {ex.Message})");
            }

            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> PutResponseAsync(object objRequest, TravelStudioControllers controllers, string additionalRoute, List<APIParam> routeParameters, List<APIParam> optionalParameters, EntityType entityType, Guid entityId)
        {
            var url = _apiManagerHelperService.GetUrl(controllers, additionalRoute, routeParameters, optionalParameters, entityType, entityId);

            if (string.IsNullOrEmpty(url))
                return null;

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var jsonObject = JsonConvert.SerializeObject(objRequest);
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                response = await client.PutAsync(url, content);
            }
            catch (Exception ex)
            {
                throw new Exception($"PutResponseAsync (Error = {ex.Message})");
                //TrackLog.WriteLog(ex, -999);
            }
            //$"api/products/{id}");
            return response.Content.ReadAsStringAsync().Result;
        }

        public async Task<string> GetResponseAsync(TravelStudioControllers controllers, string url)
        {
            var urlValue = _apiManagerHelperService.GetUrl(controllers, url);

            if (string.IsNullOrEmpty(url))
                return null;

            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await client.GetAsync(urlValue);
            }
            catch (Exception ex)
            {
                throw new Exception($"GetResponseAsync (Error = {ex.Message})");
            }

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}