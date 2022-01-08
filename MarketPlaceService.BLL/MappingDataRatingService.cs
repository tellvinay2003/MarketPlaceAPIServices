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
    public class MappingDataRatingService : IMappingDataRatingService
    {
        private readonly ILogger<MappingDataRatingService> _logger;
        private readonly IMappingDataRatingRepository _mappingDataRatingRepository;
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
                _mappingDataRatingRepository.UserId = value;
            }
        }

        public MappingDataRatingService(ILogger<MappingDataRatingService> logger, IMappingDataRatingRepository mappingDataRatingRepository)
        {
            _logger =   logger;
            _mappingDataRatingRepository = mappingDataRatingRepository;
        }
        public async Task<bool> DeleteMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId,int sourceId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMappedData", "MappingDataRatingService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRatingRepository.DeleteMappedData(direction,siteId,ratingTypeId,sourceId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteMappedData", "MappingDataRatingRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMappedData", "MappingDataRatingService", TraceId);
            return result;
        }

        public async Task<IEnumerable<DataMapResponse>> GetMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedData", "MappingDataRatingService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRatingRepository.GetMappedData(direction,siteId,ratingTypeId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMappedData", "PriceTypeRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedData", "PriceTypeService", TraceId);
            return result;
        }

        public async Task<DataMapResponse> GetMappedData(Entities.MappingDirection direction, Guid siteId,int ratingTypeId, int sourceId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedData", "MappingDataRatingService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRatingRepository.GetMappedData(direction,siteId,ratingTypeId,sourceId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMappedData", "PriceTypeRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedData", "PriceTypeService", TraceId);
            return result;
        }

        public async Task<DataMapResponse> InsertMappedData(Entities.MappingDirection direction, Guid siteId,int ratingTypeId, int sourceId, DataMap data)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMappedData", "MappingDataRatingService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRatingRepository.InsertMappedData(direction,siteId,ratingTypeId,sourceId, data);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertMappedData", "PriceTypeRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertMappedData", "PriceTypeService", TraceId);
            return result;
        }

        public async Task<DataMapResponse> UpdateMappedData(Entities.MappingDirection direction, Guid siteId,int ratingTypeId, int sourceId, DataMap data)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMappedData", "MappingDataRatingService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _mappingDataRatingRepository.UpdateMappedData(direction,siteId,ratingTypeId,sourceId, data);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateMappedData", "PriceTypeRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMappedData", "PriceTypeService", TraceId);
            return result;
        }

    }
}
