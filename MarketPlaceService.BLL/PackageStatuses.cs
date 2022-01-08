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
    public class PackageStatuses : IPackageStatuses
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
        public PackageStatuses(ICommonRepository commonRepository, IAPIManagerService apiManagerService,ILogger<ServiceStatusesService> logger)
        {
            _commonRepository = commonRepository;
            _apiManagerService = apiManagerService;
            _logger = logger;
        }
        public async Task<string> GetPackageStatusAsync(Guid entityId, EntityType entityType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPackageStatusAsync", "PackageStatusesService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.PackageStatuses,"",null,null, entityType, entityId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPackageStatusAsync", "PackageStatusesService", TraceId);
            return result;
        }
    }
}
