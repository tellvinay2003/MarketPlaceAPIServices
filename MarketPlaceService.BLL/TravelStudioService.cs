using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Job;
using System;
using System.Collections;
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
using System.Web;

namespace MarketPlaceService.BLL
{
    public class TravelStudioService : ITravelStudioService
    {
        private readonly ILogger<TravelStudioService> _logger;
        private readonly ISiteRepository _siteRepository;
        private readonly ISubscriberRepository _SubscriberRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IAPIManagerService _apiManagerService;
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
                _apiManagerService.TraceId = _traceId;

            }
        }

        public TravelStudioService(ILogger<TravelStudioService> logger, ISiteRepository siteRepository, ISubscriberRepository SubscriberRepository, ICommonRepository commonRepository, IAPIManagerService apiManagerService)
        {
            _logger = logger;
            _siteRepository = siteRepository;
            _SubscriberRepository = SubscriberRepository;
            _commonRepository = commonRepository;
            _apiManagerService = apiManagerService;
        }
        // public async Task<SubscriberDataModel> GetSubscriberById(Guid id)
        // {
        //     _logger.LogInformation("Repository call for GetSubscriberById started");
        //     var watch = Stopwatch.StartNew();
        //     var result = await _SubscriberRepository.GetSubscriberById(id);
        //     watch.Stop();
        //     _logger.LogInformation("Execution Time of GetSubscriberById repository call is: {duration}ms", watch.ElapsedMilliseconds);
        //     return result;
        // }

        public async Task<string> GetAllBookingTypesAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllBookingTypesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            //    var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            //     var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/BookingTypes", url));
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.BookingTypes, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllBookingTypesAsync", "TravelStudioService", TraceId);
            return result;
        }

        public async Task<string> GetAllPriceTypesAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllPriceTypesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            //    var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            //     var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/PriceTypes", url));
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.PriceTypes, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllPriceTypesAsync", "TravelStudioService", TraceId);
            return result;
        }


        public async Task<string> GetAllCommunicationTypesAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllCommunicationTypesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/CommunicationTypes", url)); 
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.CommunicationTypes, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllCommunicationTypesAsync", "TravelStudioService", TraceId);
            return result;
        }


        public async Task<string> GetAllSeasonTypesAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllSeasonTypesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            //    var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            //     var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/SeasonTypes", url));
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.SeasonTypes, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllSeasonTypesAsync", "TravelStudioService", TraceId);
            return result;
        }


        public async Task<string> GetAllChargingPolicyAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllChargingPolicyAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            //    var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            //     var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/ChargingPolicies", url));
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.ChargingPolicies, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllChargingPolicyAsync", "TravelStudioService", TraceId);
            return result;
        }

        public async Task<string> GetSuppliersAsync(Guid entityId, EntityType entityType, string SupplierName)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSuppliersAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/Suppliers?SupplierName={1}", url, SupplierName));
            var optionalParams = new List<APIParam>{
                new APIParam{Name = "SupplierName", Value = HttpUtility.UrlEncode(SupplierName)}
            };
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Suppliers, "", null, optionalParams, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSuppliersAsync", "TravelStudioService", TraceId);
            return result;
        }

        public async Task<string> GetSupplierNameAsync(Guid entityId, EntityType entityType, int supplierId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierNameAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/Suppliers/{1}", url, supplierId));
            var mandatoryParams = new List<APIParam>{
                new APIParam{Value = supplierId.ToString()}
            };
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Suppliers, "", mandatoryParams, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierNameAsync", "TravelStudioService", TraceId);
            return result;
        }
        public async Task<string> GetProductCodesAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetProductCodesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/ProductCodes", url));
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.ProductCodes, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetProductCodesAsync", "TravelStudioService", TraceId);
            return result;
        }

        public async Task<string> GetRegions(Guid entityId, EntityType entityType, string name)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegions", "TravelStudioService", TraceId);
            //_logger.LogInformation("[TraceId:{traceId}]  TSAPI call for GetRegions started", TraceId);
            var watch = Stopwatch.StartNew();
            //var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            //var result = await APIManager.GetResponseAsync(string.Format("{0}api/v1/regions?name={1}", url, name));
            List<APIParam> optionalParameters = new List<APIParam>
            {
                new APIParam{ Name = "name", Value = HttpUtility.UrlEncode(name)}
            };
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Regions, "", null, optionalParameters, entityType, entityId);
            watch.Stop();
            //_logger.LogInformation("[TraceId:{traceId}]  Execution Time of GetRegions TSAPI call is: {duration}ms", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetRegions", "TravelStudioService", TraceId);
            return result;
        }

        public async Task<string> GetServiceTypesAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceTypesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            //var siteurl =await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await APIManager.GetResponseAsync(string.Format("{0}api/v1/controller/{1}", siteurl, "GetServiceTypes"));
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.ServiceTypes, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceTypesAsync", "TravelStudioService", TraceId);
            return result;
        }

        public async Task<string> GetRegionById(Guid entityId, EntityType entityType, int id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegionById", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/regions/{1}", url, id));
            var mandatoryParams = new List<APIParam>{
                new APIParam{Value = id.ToString()}
            };
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Regions, "", mandatoryParams, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetRegionById", "TravelStudioService", TraceId);
            return result;
        }


        public async Task<string> GetOrganization(Guid entityId, EntityType entityType)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "GetOrganization", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Organisations, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetOrganization", "TravelStudioService", TraceId);

            return result;
        }

        public async Task<string> GetServiceTypeTypes(Guid entityId, EntityType entityType)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceTypeTypes", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.ServiceTypes, "servicetypetypes", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceTypeTypes", "TravelStudioService", TraceId);

            return result;
        }

        public async Task<string> GetMappingTypesSiteData(Guid entityId, EntityType entityType, int dataTypeid, int? filterId)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappingTypesSiteData", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";

            var mandatoryParams = new List<APIParam>{
                new APIParam{ Value = dataTypeid.ToString()}
            };

            List<APIParam> optionalParameters = new List<APIParam>
            {
                new APIParam{ Name = "filterId", Value = (filterId == null ? string.Empty : filterId.ToString())}
            };
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.MappingDataTypes, "", mandatoryParams, optionalParameters, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMappingTypesSiteData", "TravelStudioService", TraceId);

            return result;
        }

        public async Task<string> GetAllTaxesAsync(Guid entityId, EntityType entityType)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllTaxesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Taxes, "", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllTaxesAsync", "TravelStudioService", TraceId);
            return result;
        }


        public async Task<string> GetOrganizationsPrefix(Guid entityId, EntityType entityType, int organisationId)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "GetOrganizationsPrefix", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";
               var optionalParams = new List<APIParam>{
                new APIParam { Name = "organisationId", Value = organisationId.ToString()}
            };    
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Organisations, "GetPrefix", null, optionalParams, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetOrganizationsPrefix", "TravelStudioService", TraceId);

            return result;
        }



         public async Task<string> GetUsers( Guid entityId, EntityType entityType,string name,int limit)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "GetUsers", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";
            var optionalParams = new List<APIParam>{
                new APIParam { Name= "name", Value = HttpUtility.UrlEncode(name)},
                new APIParam { Name = "limit", Value = limit.ToString()}
            };    
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Users, "GetUsers", null, optionalParams, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetUsers", "TravelStudioService", TraceId);

            return result;
        }


         public async Task<string> GetUserById( Guid entityId, EntityType entityType,int userid)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "GetUserById", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";
            var routeParams = new List<APIParam>{
                new APIParam { Name= "userid", Value = userid.ToString()}
              
            };    
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Users, "GetUserById", routeParams, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetUserById", "TravelStudioService", TraceId);

            return result;
        }


        public async Task<string> GetBookingStatus( Guid entityId, EntityType entityType)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingStatus", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";
           
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Booking, "GetBookingStatus", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingStatus", "TravelStudioService", TraceId);

            return result;
        }


        public async Task<string> SearchJob( Guid entityId, EntityType entityType,BusinessProcess businessProcess ,Enum queue ,Enum jobType)
        {

            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchJob", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = "";
            var optionalParams = new List<APIParam>{
                new APIParam { Name= "businessProcess", Value = businessProcess.ToString()},
                new APIParam { Name = "queue", Value = queue.ToString()},
                new APIParam { Name = "jobType", Value = jobType.ToString()}
            };    
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Job, "SearchJob", null, optionalParams, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "SearchJob", "TravelStudioService", TraceId);

            return result;
        }
        
        public async Task<string> GetPackageTypesAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPackageTypesAsync", "TravelStudioService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.PackageTypes, "PackageTypes", null, null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "GetResponseAsync", "APIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPackageTypesAsync", "TravelStudioService", TraceId);
            return result;
        }
    }
}
