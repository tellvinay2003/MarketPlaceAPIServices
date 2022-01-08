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
    public class MasterDataService : IMasterDataService
    {
        private readonly ILogger<MasterDataService> _logger;
        private readonly IMasterDataRepository _masterDataRepository;

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
                _masterDataRepository.UserId = value;
            }
        }

        public MasterDataService(ILogger<MasterDataService> logger, IMasterDataRepository masterDataRepository)
        {
            _logger = logger;
            _masterDataRepository = masterDataRepository;
        }
        public async Task<bool> DeleteMasterDataGeneric(int masterDataTypeId, int itemId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataGeneric", "MasterDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRepository.DeleteMasterDataGeneric(masterDataTypeId, itemId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteMasterDataGeneric", "MasterDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataGeneric", "MasterDataService", TraceId);
            return result;
        }

        public async Task<IEnumerable<MasterData>> GetMasterDataGeneric(int masterdatatypeid)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGeneric", "MasterDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRepository.GetMasterDataGeneric(masterdatatypeid);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataGeneric", "MasterDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGeneric", "MasterDataService", TraceId);
            return result;
        }

        public async Task<MasterData> GetMasterDataGeneric(int masterDataTypeId, int itemId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGeneric", "MasterDataService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRepository.GetMasterDataGeneric(masterDataTypeId, itemId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataGeneric", "MasterDataRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGeneric", "MasterDataService", TraceId);
            return result;
        }

        public async Task<MasterData> InsertMasterDataGeneric(int masterDataTypeId, MasterData item)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataGeneric", "MasterDataService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataRepository.InsertMasterDataGeneric(masterDataTypeId, item);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertMasterDataGeneric", "MasterDataRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataGeneric", "MasterDataService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<MasterData> UpdateMasterDataGeneric(int masterDataTypeId, int itemId, MasterData item)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataGeneric", "MasterDataService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataRepository.UpdateMasterDataGeneric(masterDataTypeId, itemId, item);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateMasterDataGeneric", "MasterDataRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataGeneric", "MasterDataService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfMappedToImportedProduct(int ratingTypeId)
        {
            _logger.LogInformation("Repository call for CheckIfMappedToImportedProduct started");
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRepository.CheckIfMappedToImportedProduct(ratingTypeId);
            watch.Stop();
            _logger.LogInformation("Execution Time of CheckIfMappedToImportedProduct repository call is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }
    }
}
