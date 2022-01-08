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
    public class MasterDataServiceTypeService : IMasterDataServiceTypeService
    {
        private readonly ILogger<MasterDataServiceTypeService> _logger;
        private readonly IMasterDataServiceTypeRepository _masterDataServiceTypeRepository;

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
                _masterDataServiceTypeRepository.UserId = value;
            }
        }

        public MasterDataServiceTypeService(ILogger<MasterDataServiceTypeService> logger, IMasterDataServiceTypeRepository masterDataServiceTypeRepository)
        {
            _logger = logger;
            _masterDataServiceTypeRepository = masterDataServiceTypeRepository;
        }

        public async Task<bool> DeleteMasterDataServiceType(int id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataServiceTypeRepository.DeleteMasterDataServiceType(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteMasterDataServiceType", "MasterDataServiceTypeRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
            return result;
        }

        public async Task<Entities.MasterDataServiceType> GetMasterDataServiceType(int id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataServiceTypeRepository.GetMasterDataServiceType(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataServiceType", "MasterDataServiceTypeRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
            return result;
        }

        public async Task<IEnumerable<Entities.MasterDataServiceType>> GetMasterDataServiceTypes()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataServiceTypes", "MasterDataServiceTypeService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataServiceTypeRepository.GetMasterDataServiceTypes();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataServiceTypes", "MasterDataServiceTypeRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataServiceTypes", "MasterDataServiceTypeService", TraceId);
            return result;
        }

        public async Task<Entities.MasterDataServiceType> InsertMasterDataServiceType(Entities.MasterDataServiceType data)
        {
            try
            {                
                LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataServiceTypeRepository.InsertMasterDataServiceType(data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertMasterDataServiceType", "MasterDataServiceTypeRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<Entities.MasterDataServiceType> UpdateMasterDataServiceType(int id, Entities.MasterDataServiceType data)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataServiceTypeRepository.UpdateMasterDataServiceType(id,data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateMasterDataServiceType", "MasterDataServiceTypeRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataServiceType", "MasterDataServiceTypeService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> CheckIfMappedToImportedProduct(int id)
        {
            try
            {
                _logger.LogInformation("Repository call for CheckIfMappedToImportedProduct started");
                var watch = Stopwatch.StartNew();
                var result = await _masterDataServiceTypeRepository.CheckIfMappedToImportedProduct(id);
                watch.Stop();
                _logger.LogInformation("Execution Time of CheckIfMappedToImportedProduct repository call is: {duration}ms", watch.ElapsedMilliseconds);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
