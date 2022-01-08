using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MarketPlaceService.BLL.UtilityService;
using System.Net.Http;
using Newtonsoft.Json;
using CommonUtilities;

namespace MarketPlaceService.BLL
{
    public class SupplierStatusesService : ISupplierStatusesService
    {
       private readonly ICommonRepository _commonRepository;
       private readonly ISupplierStatusesRepository _supplierStatusesRepository;

        private readonly ILogger<SupplierStatusesService> _logger;
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
            }
        }

        public SupplierStatusesService(ICommonRepository commonRepository, ISupplierStatusesRepository supplierStatusesRepository, ILogger<SupplierStatusesService> logger, IAPIManagerService apiManagerService)
        {
            _commonRepository = commonRepository;
            _supplierStatusesRepository = supplierStatusesRepository;
            _logger = logger;
            _apiManagerService = apiManagerService;
        }

        public async Task<string> GetAllSupplierStatusesAsync(Guid entityId,EntityType entityType)
        {
            var result = string.Empty;
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllSupplierStatusesAsync", "SupplierStatusesService", TraceId);
            var watch = Stopwatch.StartNew();
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/supplierStatuses", url));
            result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.SupplierStatuses,"",null,null,entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllSupplierStatusesAsync", "SupplierStatusesService", TraceId);
            return result;
        }

        public async Task<string> GetSupplierStatusByIdAsync(Guid entityId,EntityType entityType, int supplierStatusId)
        {
            var result = string.Empty;
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierStatusByIdAsync", "SupplierStatusesService", TraceId);
            var watch = Stopwatch.StartNew();
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);            
            // result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/supplierStatuses/{1}", url, supplierStatusId)); 
            var mandatoryParams = new List<APIParam>{
                new APIParam{ Value= supplierStatusId.ToString()}
            };
             result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.SupplierStatuses,"", mandatoryParams,null,entityType ,entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierStatusByIdAsync", "SupplierStatusesService", TraceId);
            return result;
        }

        
    }
}