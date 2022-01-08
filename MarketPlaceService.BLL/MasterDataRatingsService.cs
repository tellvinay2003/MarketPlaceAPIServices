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
    public class MasterDataRatingsService : IMasterDataRatingsService
    {
        private readonly ILogger<MasterDataRatingsService> _logger;
        private readonly IMasterDataRatingsRepository _masterDataRatingsRepository;

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
                _masterDataRatingsRepository.UserId = value;
            }
        }

        public MasterDataRatingsService(ILogger<MasterDataRatingsService> logger, IMasterDataRatingsRepository masterDataRatingsRepository)
        {
            _logger = logger;
            _masterDataRatingsRepository = masterDataRatingsRepository;
        }
        public async Task<bool> DeleteMasterDataRatings(int ratingType, int ratingId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataRatings", "MasterDataRegionService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRatingsRepository.DeleteMasterDataRatings(ratingType, ratingId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteMasterDataRatings", "MasterDataRatingsRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataRatings", "MasterDataRegionService", TraceId);
            return result;
        }

        public async Task<IEnumerable<MasterDataRating>> GetMasterDataRatings(int ratingType)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataRatings", "MasterDataRegionService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRatingsRepository.GetMasterDataRatings(ratingType);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataRatings", "MasterDataRatingsRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataRatings", "MasterDataRegionService", TraceId);
            return result;
        }

        public async Task<MasterDataRating> GetMasterDataRatings(int ratingType, int ratingId)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataRatings", "MasterDataRegionService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRatingsRepository.GetMasterDataRatings(ratingType, ratingId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataRatings", "MasterDataRatingsRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataRatings", "MasterDataRegionService", TraceId);
            return result;
        }

        public async Task<MasterDataRating> InsertMasterDataRatings(int ratingType, MasterDataRating data)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataRatings", "MasterDataRegionService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataRatingsRepository.InsertMasterDataRatings(ratingType, data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertMasterDataRatings", "MasterDataRatingsRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataRatings", "MasterDataRegionService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<MasterDataRating> UpdateMasterDataRatings(int ratingType, int ratingId, MasterDataRating data)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataRatings", "MasterDataRegionService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataRatingsRepository.UpdateMasterDataRatings(ratingType, ratingId, data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateMasterDataRatings", "MasterDataRatingsRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataRatings", "MasterDataRegionService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> CheckIfMappedToImportedProduct(int ratingId)
        {
            _logger.LogInformation("Repository call for CheckIfMappedToImportedProduct started");
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRatingsRepository.CheckIfMappedToImportedProduct(ratingId);
            watch.Stop();
            _logger.LogInformation("Execution Time of CheckIfMappedToImportedProduct repository call is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }
    }
}
