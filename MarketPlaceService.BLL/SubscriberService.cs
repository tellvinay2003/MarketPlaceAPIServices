using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MarketPlaceService.DAL.Contract;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using MarketPlaceService.BLL.UtilityService;
using CommonUtilities;

namespace MarketPlaceService.BLL
{
    public class SubscriberService : ISubscriberService
    {
       private readonly ISubscriberRepository _subscriberRepository;

       private readonly ISiteRepository _siteRepository;

        private readonly ILogger<SubscriberService> _logger;

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

        public SubscriberService(ISubscriberRepository subscriberRepository,ISiteRepository siteRepository, ILogger<SubscriberService> logger)
        {
            _subscriberRepository = subscriberRepository;
            _siteRepository = siteRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<SubscriberDataModel>> GetSubscribersListAsync()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscribersListAsync", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetSubscribersListAsync();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSubscribersListAsync", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscribersListAsync", "SubscriberService", TraceId);
            return result;
        }

         public async Task<SubscriberDataModel> DeleteSubscriber(Guid id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriber", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.DeleteSubscriber(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteSubscriber", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriber", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberDataModel> EditSubscriber(Guid id)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "EditSubscriber", "SubscriberService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _subscriberRepository.EditSubscriber(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "EditSubscriber", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "EditSubscriber", "SubscriberService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<SubscriberDataModel> AddNewSubscriber(SubscriberDataModel SubscriberItem)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "AddNewSubscriber", "SubscriberService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _subscriberRepository.AddNewSubscriber(SubscriberItem);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "AddNewSubscriber", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "AddNewSubscriber", "SubscriberService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<SubscriberDataModel> UpdateSubscriber(SubscriberDataModel SubscriberItem)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriber", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.UpdateSubscriber(SubscriberItem);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateSubscriber", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriber", "SubscriberService", TraceId);
            return result;
        }


        public async Task<IEnumerable<SubscriberDataModel>> GetEnabledSubscribersListAsync()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetEnabledSubscribersListAsync", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetEnabledSubscribersListAsync();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetEnabledSubscribersListAsync", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetEnabledSubscribersListAsync", "SubscriberService", TraceId);
            return result;
        }

        public async Task<IEnumerable<SubscriberSupplierDataModel>> GetSupplierMapSubscriberByIdAsync(Guid SubscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierMapSubscriberByIdAsync", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetSupplierMapSubscriberByIdAsync(SubscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSupplierMapSubscriberByIdAsync", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierMapSubscriberByIdAsync", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberSupplierDataModel> GetSupplierMapSubscriberByIdandPubIdAsync(Guid SubscriberId,Guid PublisherId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierMapSubscriberByIdandPubIdAsync", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetSupplierMapSubscriberByIdandPubIdAsync(SubscriberId,PublisherId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSupplierMapSubscriberByIdandPubIdAsync", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierMapSubscriberByIdandPubIdAsync", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberSupplierDataModel> AddNewSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId,int SupplierId)
        {
           LoggingHelper.LogInfo(_logger, LogType.Start, "AddNewSubscriberSupplierMap", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.AddNewSubscriberSupplierMap(SubscriberId,PublisherId,SupplierId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "AddNewSubscriberSupplierMap", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "AddNewSubscriberSupplierMap", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberSupplierDataModel> UpdateSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId,int SupplierId)
        {
           LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriberSupplierMap", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.UpdateSubscriberSupplierMap(SubscriberId,PublisherId,SupplierId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateSubscriberSupplierMap", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriberSupplierMap", "SubscriberService", TraceId);
            return result;
        }

        public async Task<bool> DeleteSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriberSupplierMap", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.DeleteSubscriberSupplierMap(SubscriberId,PublisherId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteSubscriberSupplierMap", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriberSupplierMap", "SubscriberService", TraceId);
            return result;
        }
        
        public async Task<SubscriberChargingPolicyDataModel> GetDefaultSubscriberById(Guid SubscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetDefaultSubscriberById", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetDefaultSubscriberById(SubscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetDefaultSubscriberById", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetDefaultSubscriberById", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberChargingPolicyDataModel> UpdateDefaultSubscriber(SubscriberChargingPolicyDataModel request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateDefaultSubscriber", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.UpdateDefaultSubscriber(request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateDefaultSubscriber", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateDefaultSubscriber", "SubscriberService", TraceId);
            return result;
        }



        public async Task<SubscriberDefaultDataModel> GetContractDefaultSubscriberById(Guid subscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetContractDefaultSubscriberById", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetContractDefaultSubscriberById(subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetContractDefaultSubscriberById", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetContractDefaultSubscriberById", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberDefaultDataModel> UpdateContractDefaultSubscriber(SubscriberDefaultDataModel request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateContractDefaultSubscriber", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.UpdateContractDefaultSubscriber(request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateContractDefaultSubscriber", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateContractDefaultSubscriber", "SubscriberService", TraceId);
            return result;
        }

        public async Task<IEnumerable<SubscriberDefaultsProductCode>> GetSubscriberDefaultsProductCodes(Guid subscriberId)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetSubscriberDefaultsProductCodes(subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSubscriberDefaultsProductCodes", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberDefaultsProductCode> GetSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetSubscriberDefaultsProductCodes(subscriberId, ruleId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSubscriberDefaultsProductCodes", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberDefaultsProductCode> InsertSubscriberDefaultsProductCodes(Guid subscriberId, SubscriberDefaultsProductCode request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.InsertSubscriberDefaultsProductCodes(subscriberId, request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertSubscriberDefaultsProductCodes", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberDefaultsProductCode> UpdateSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId, SubscriberDefaultsProductCode request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.UpdateSubscriberDefaultsProductCodes(subscriberId, ruleId, request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateSubscriberDefaultsProductCodes", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            return result;
        }

        public async Task<bool> DeleteSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.DeleteSubscriberDefaultsProductCodes(subscriberId, ruleId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteSubscriberDefaultsProductCodes", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriberDefaultsProductCodes", "SubscriberService", TraceId);
            return result;
        }

        public async Task<IEnumerable<SubscriberDefaultSellingPrice>> GetSubscriberDefaultSellPrices(Guid subscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultSellPrices", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetSubscriberDefaultSellPrices(subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSubscriberDefaultSellPrices", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultSellPrices", "SubscriberService", TraceId);
            return result;
        }

        public async Task<SubscriberDefaultSellingPrice> GetSubscriberDefaultSellPriceById(Guid subscriberId, int ruleId)
        {
          LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultSellPriceById", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.GetSubscriberDefaultSellPriceById(subscriberId,ruleId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSubscriberDefaultSellPriceById", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultSellPriceById", "SubscriberService", TraceId);
            return result;  
        }

        public async Task<SubscriberDefaultSellingPrice> InsertSubscriberDefaultsSellPrice(Guid subscriberId, SubscriberDefaultSellingPrice request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriberDefaultsSellPrice", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.InsertSubscriberDefaultsSellPrice(subscriberId,request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertSubscriberDefaultsSellPrice", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriberDefaultsSellPrice", "SubscriberService", TraceId);
            return result; 
        }

         public async Task<SubscriberDefaultSellingPrice> UpdateSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId, SubscriberDefaultSellingPrice request)
         {
             LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriberDefaultsSellPrice", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.UpdateSubscriberDefaultsSellPrice(subscriberId,ruleId,request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateSubscriberDefaultsSellPrice", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriberDefaultsSellPrice", "SubscriberService", TraceId);
            return result; 
         }

         public async Task<bool> DeleteSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId)
         {
             LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriberDefaultsSellPrice", "SubscriberService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriberRepository.DeleteSubscriberDefaultsSellPrice(subscriberId,ruleId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteSubscriberDefaultsSellPrice", "SubscriberRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriberDefaultsSellPrice", "SubscriberService", TraceId);
            return result; 
         }

    }
}
