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
    public class MasterDataRegionService : IMasterDataRegionService
    {
        private readonly ILogger<MasterDataRegionService> _logger;
        private readonly IMasterDataRegionRepository _masterDataRegionRepository;

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
                _masterDataRegionRepository.UserId = value;
            }
        }

        public MasterDataRegionService(ILogger<MasterDataRegionService> logger, IMasterDataRegionRepository masterDataRegionRepository)
        {
            _logger = logger;
            _masterDataRegionRepository = masterDataRegionRepository;
        }
        public async Task<bool> DeleteMasterDataGeolocations(int id)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataGeolocations", "MasterDataRegionService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRegionRepository.DeleteMasterDataGeolocations(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteMasterDataGeolocations", "MasterDataRegionRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataGeolocations", "MasterDataRegionService", TraceId);
            return result;
        }

        public async Task<IEnumerable<MasterDataGeolocation>> GetMasterDataGeolocations(string name)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGeolocations", "MasterDataRegionService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRegionRepository.GetMasterDataGeolocations(name);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataGeolocations", "MasterDataRegionRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGeolocations", "MasterDataRegionService", TraceId);
            return result;
        }

        public async Task<MasterDataGeolocation> GetMasterDataGeolocations(int id)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGeolocations", "MasterDataRegionService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRegionRepository.GetMasterDataGeolocations(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataGeolocations", "MasterDataRegionRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGeolocations", "MasterDataRegionService", TraceId);
            return result;
        }

        public async Task<MasterDataGeolocation> InsertMasterDataGeolocations(int parentId, MasterDataGeolocation data)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataGeolocations", "MasterDataRegionService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataRegionRepository.InsertMasterDataGeolocations(parentId,data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertMasterDataGeolocations", "MasterDataRegionRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataGeolocations", "MasterDataRegionService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<MasterDataGeolocation> UpdateMasterDataGeolocations(int id, MasterDataGeolocation data)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataGeolocations", "MasterDataRegionService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataRegionRepository.UpdateMasterDataGeolocations(id, data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateMasterDataGeolocations", "MasterDataRegionRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataGeolocations", "MasterDataRegionService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckIfMappedToImportedProduct(int regionId)
        {
            _logger.LogInformation("Repository call for CheckIfMappedToImportedProduct started");
            var watch = Stopwatch.StartNew();
            var result = await _masterDataRegionRepository.CheckIfMappedToImportedProduct(regionId);
            watch.Stop();
            _logger.LogInformation("Execution Time of CheckIfMappedToImportedProduct repository call is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }
    }
}
