using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CommonUtilities;

namespace MarketPlaceService.BLL
{
    public class MappingDataConfigService : IMappingDataConfigService
    {
        private readonly ILogger<MappingDataConfigService> _logger;
        private readonly IMappingDataConfigRepository _mappingDataConfigRepository;

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

        public MappingDataConfigService(IMappingDataConfigRepository mappingDataConfigRepository, ILogger<MappingDataConfigService> logger)
        {
            _logger = logger;
            _mappingDataConfigRepository = mappingDataConfigRepository;
        }
        public async Task<IEnumerable<MappingDataConfig>> GetMappingDataConfig(Entities.MappingDirection direction, Guid site)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappingDataConfig", "MappingDataConfigService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataConfigRepository.GetMappingDataConfig(direction, site);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMappingDataConfig", "MappingDataConfigRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMappingDataConfig", "MappingDataConfigService", TraceId);
            return result;
        }

        public async Task<MappingDataConfig> GetMappingDataConfig(Entities.MappingDirection direction, ushort dataTypeId, Guid site)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappingDataConfig", "MappingDataConfigService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataConfigRepository.GetMappingDataConfig(direction, dataTypeId, site);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMappingDataConfig", "MappingDataConfigRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMappingDataConfig", "MappingDataConfigService", TraceId);
            return result;
        }
    }
}
