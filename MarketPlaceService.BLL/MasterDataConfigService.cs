using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.DAL;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlaceService.BLL
{
    public class MasterDataConfigService : IMasterDataConfigService
    {
        private readonly ILogger<MasterDataConfigService> _logger;
        private readonly IMasterDataConfigRepository _masterDataConfigRepository;

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

        public MasterDataConfigService(ILogger<MasterDataConfigService> logger, IMasterDataConfigRepository masterDataConfigRepository)
        {
            _logger = logger;
            _masterDataConfigRepository = masterDataConfigRepository;
        }
        public async Task<IEnumerable<MasterDataConfig>> GetMasterDataConfig()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataConfig", "MasterDataConfigService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataConfigRepository.GetMasterDataConfig();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataConfig", "MasterDataConfigRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataConfig", "MasterDataConfigService", TraceId);
            return result;
        }

        public async Task<MasterDataConfig> GetMasterDataConfig(int id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataConfig", "MasterDataConfigService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataConfigRepository.GetMasterDataConfig(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataConfig", "MasterDataConfigRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataConfig", "MasterDataConfigService", TraceId);
            return result;
        }
    }
}
