using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;

namespace MarketPlaceService.BLL
{
    public class MappingDataService : IMappingDataService
    {
        private readonly ILogger<MappingDataService> _logger;
        private readonly IMappingDataRepository _mappingDataRepository;

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
          private Guid _userId;
        public Guid UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
                _mappingDataRepository.UserId = value;
            }
        }

        public MappingDataService(ILogger<MappingDataService> logger, IMappingDataRepository mappingDataRepository)
        {
            _logger =   logger;
            _mappingDataRepository = mappingDataRepository;
        }
        public async Task<bool> DeleteMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMappedData", "MappingDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRepository.DeleteMappedData(direction,dataMappingTypeId,siteId, sourceId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetAllPriceTypesAsync", "MappingDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMappedData", "MappingDataService", TraceId);
            return result;
        }

        public async Task<IEnumerable<DataMapResponse>> GetMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedData", "MappingDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRepository.GetMappedData(direction,dataMappingTypeId,siteId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMappedData", "MappingDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedData", "MappingDataService", TraceId);
            return result;
        }

        public async Task<DataMapResponse> GetMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedData", "MappingDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRepository.GetMappedData(direction,dataMappingTypeId,siteId,sourceId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMappedData", "MappingDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedData", "MappingDataService", TraceId);
            return result;
        }

        public async Task<DataMapResponse> InsertMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId, DataMap request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMappedData", "MappingDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRepository.InsertMappedData(direction,dataMappingTypeId,siteId,sourceId, request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertMappedData", "MappingDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertMappedData", "MappingDataService", TraceId);
            return result;
        }

        public async Task<DataMapResponse> UpdateMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId, DataMap request)
        {
            
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMappedData", "MappingDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRepository.UpdateMappedData(direction,dataMappingTypeId,siteId,sourceId,request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateMappedData", "MappingDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMappedData", "MappingDataService", TraceId);
            return result;
        }
    }
}
