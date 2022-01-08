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
    public class MasterDataPackageService : IMasterDataPackageService
    {
        private readonly ILogger<MasterDataPackageService> _logger;
        private readonly IMasterDataPackageRepository _masterDataPackageRepository;

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
                _masterDataPackageRepository.UserId = value;
            }
        }


        public MasterDataPackageService(ILogger<MasterDataPackageService> logger, IMasterDataPackageRepository masterDataPackageRepository)
        {
            _logger = logger;
            _masterDataPackageRepository = masterDataPackageRepository;
        }

        public async Task<IEnumerable<MasterDataPackage>> GetMasterDataPackage(int id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataPackage", "MasterDataPackageService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataPackageRepository.GetMasterDataPackage(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMasterDataPackage", "MasterDataPackageRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataPackage", "MasterDataPackageService", TraceId);
            return result;
        }

        public async Task<MasterDataPackage> InsertMasterDataPackage(int masterDataTypeId, MasterDataPackage data)
        {
             try
            {                
                LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataPackage", "MasterDataPackageService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataPackageRepository.InsertMasterDataPackage(masterDataTypeId,data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertMasterDataPackage", "MasterDataPackageRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataPackage", "MasterDataPackageService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<MasterDataPackage> UpdateMasterDataPackage(int masterDataTypeId,int id, MasterDataPackage data)
        {
             try
            {                
                LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataPackage", "MasterDataPackageService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _masterDataPackageRepository.UpdateMasterDataPackage(masterDataTypeId,id,data);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateMasterDataPackage", "MasterDataPackageRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataPackage", "MasterDataPackageService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        } 

         public async Task<bool> DeleteMasterDataPackage(int masterDataTypeId,int id)
        {
            try
            {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataPackage", "MasterDataPackageService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _masterDataPackageRepository.DeleteMasterDataPackage(masterDataTypeId,id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteMasterDataPackage", "MasterDataPackageRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataPackage", "MasterDataPackageService", TraceId);
            return result;
          
        }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
