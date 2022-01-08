using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using MarketPlaceService.DAL.Contract;
using System.Net.Http;
using Newtonsoft.Json;
using MarketPlaceService.BLL.UtilityService;
using System.IO;
using System.Runtime.Serialization.Json;
using CommonUtilities;
using Microsoft.Extensions.Configuration;

namespace MarketPlaceService.BLL
{
    public class SiteService : ISiteService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ILogger<SiteService> _logger;
        private readonly IAPIManagerService _apiManagerService;
        private readonly IConfiguration _configuration;
        private Guid _traceId;
        public SiteService(ISiteRepository siteRepository, ILogger<SiteService> logger, IAPIManagerService apiManagerService,IConfiguration configuration)
        {
            _siteRepository = siteRepository;
            _logger = logger;
            _apiManagerService = apiManagerService;
            _configuration = configuration;
        }
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
                _apiManagerService.TraceId = _traceId;

            }
        }

        public async Task<SiteDataModel> AddNewSite(SiteDataModel siteData)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "AddNewSite", "SiteService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _siteRepository.AddNewSite(siteData);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "AddNewSite", "SiteRepository", TraceId, watch.ElapsedMilliseconds);
                string mpURL =_configuration["AppSettings:MarketplaceApiUrl"];
                var optionalParams = new MarketplaceConnector
               {
                    MpURL = mpURL,
                    SiteId = result.SiteId
               };
                var saveSiteIdInTs=await _apiManagerService.PostResponseAsync(optionalParams,TravelStudioControllers.Sites,"",null,null,EntityType.Site, result.SiteId); 
                LoggingHelper.LogInfo(_logger, LogType.End, "AddNewSite", "SiteService", TraceId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SiteDataModel>> GetRegisteredSitesAsync()
        {
            //_logger.LogInformation("[TraceId:{traceId}]  Repository call for GetRegisteredSitesAsync started", TraceId);
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegisteredSitesAsync", "SiteService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _siteRepository.GetRegisteredSitesAsync();
            watch.Stop();
            //_logger.LogInformation("[TraceId:{traceId}]  Execution Time of GetRegisteredSitesAsync repository call is: {duration}ms", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetRegisteredSitesAsync", "SiteRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetRegisteredSitesAsync", "SiteService", TraceId);
            return result;
        }

        public async Task<SiteDataModel> GetSiteById(Guid id)
        {          
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSiteById", "SiteService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _siteRepository.GetSiteById(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSiteById", "SiteRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSiteById", "SiteService", TraceId);
            return result;
        }

        public async Task<SiteDataModel> DeleteSiteById(Guid id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSiteById", "SiteService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _siteRepository.DeleteSiteById(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteSiteById", "SiteRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSiteById", "SiteService", TraceId);
            return result;
        }


        public async Task<SiteDataModel> UpdateSite(SiteDataModel siteData)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSite", "SiteService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _siteRepository.UpdateSite(siteData);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateSite", "SiteRepository", TraceId, watch.ElapsedMilliseconds);
                string mpURL =_configuration["AppSettings:MarketplaceApiUrl"];
                var optionalParams = new MarketplaceConnector
               {
                    MpURL = mpURL,
                    SiteId = siteData.SiteId
               };
                var saveSiteIdInTs=await _apiManagerService.PostResponseAsync(optionalParams,TravelStudioControllers.Sites,"",null,null,EntityType.Site, siteData.SiteId); 
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSite", "SiteService", TraceId);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ValidateSite(string url)
        {
            try
            {
                bool sucess;
                LoggingHelper.LogInfo(_logger, LogType.Start, "ValidateSite", "SiteService", TraceId);
                var watch = Stopwatch.StartNew();
                //var result = await _siteRepository.ValidateSiteAsync(url);
                string TSAPIURL = url + "api/v1/site/ValidateSite";
                //var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/site/{1}", url, ""));
                var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Sites, url);
                var response = JsonConvert.DeserializeObject<SiteDataModel>(result);
                sucess = response.responseMessage;
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "ValidateSite", "APIManager", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "ValidateSite", "SiteService", TraceId);
                return sucess;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<SiteDataModel>> GetEnableSites()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetEnableSites", "SiteService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _siteRepository.GetEnableSites();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetEnableSites", "SiteRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetEnableSites", "SiteService", TraceId);
            return result;
        }
    }
}
