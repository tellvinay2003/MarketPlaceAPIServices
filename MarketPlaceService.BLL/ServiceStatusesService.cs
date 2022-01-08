using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.UtilityService;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlaceService.BLL
{
    public class ServiceStatusesService : IServiceStatusesService
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IAPIManagerService _apiManagerService;
        private readonly ILogger<ServiceStatusesService> _logger;

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
        public ServiceStatusesService(ICommonRepository commonRepository, IAPIManagerService apiManagerService,ILogger<ServiceStatusesService> logger)
        {
            _commonRepository = commonRepository;
            _apiManagerService = apiManagerService;
            _logger = logger;
        }
        public async Task<string> GetServiceStatusAsync(Guid entityId, EntityType entityType)
        {
            // var url = await _commonRepository.GetSiteUrl(entityId, entityType);
            // var result = await APIManagerService.GetResponseAsync(string.Format("{0}api/v1/serviceStatuses", url));
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceStatusAsync", "ServiceStatusesService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.ServiceStatuses,"",null,null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceStatusAsync", "ServiceStatusesService", TraceId);
            return result;
        }
    }
}
