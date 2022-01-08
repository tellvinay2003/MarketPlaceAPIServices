using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
   public interface ISubscriberService
    {
        Task<IEnumerable<SubscriberDataModel>> GetSubscribersListAsync();

        Task<SubscriberDataModel> DeleteSubscriber(Guid id);

        Task<SubscriberDataModel> EditSubscriber(Guid id);

        Task<SubscriberDataModel> AddNewSubscriber(SubscriberDataModel SubscriberItem);

        Task<SubscriberDataModel> UpdateSubscriber(SubscriberDataModel SubscriberItem);
        Task<IEnumerable<SubscriberDataModel>> GetEnabledSubscribersListAsync();

        Task<IEnumerable<SubscriberSupplierDataModel>> GetSupplierMapSubscriberByIdAsync(Guid SubscriberId);

        Task<SubscriberSupplierDataModel> GetSupplierMapSubscriberByIdandPubIdAsync(Guid SubscriberId,Guid PublisherId);

        Task<SubscriberSupplierDataModel> AddNewSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId,int SupplierId);

        Task<SubscriberSupplierDataModel> UpdateSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId,int SupplierId);

        Task<bool> DeleteSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId);
         Task<SubscriberChargingPolicyDataModel> GetDefaultSubscriberById(Guid subscriberId);
        Task<SubscriberChargingPolicyDataModel> UpdateDefaultSubscriber(SubscriberChargingPolicyDataModel model);
        Task<SubscriberDefaultDataModel> GetContractDefaultSubscriberById(Guid subscriberId); 
        Task<SubscriberDefaultDataModel> UpdateContractDefaultSubscriber(SubscriberDefaultDataModel request);

        Task<IEnumerable<SubscriberDefaultsProductCode>> GetSubscriberDefaultsProductCodes(Guid subscriberId);
        Task<SubscriberDefaultsProductCode> GetSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId);
        Task<SubscriberDefaultsProductCode> InsertSubscriberDefaultsProductCodes(Guid subscriberId, SubscriberDefaultsProductCode request);
        Task<SubscriberDefaultsProductCode> UpdateSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId, SubscriberDefaultsProductCode request);
        Task<bool> DeleteSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId);

        Task<IEnumerable<SubscriberDefaultSellingPrice>> GetSubscriberDefaultSellPrices(Guid subscriberId);
        Task<SubscriberDefaultSellingPrice> GetSubscriberDefaultSellPriceById(Guid subscriberId, int ruleId);
        Task<SubscriberDefaultSellingPrice> InsertSubscriberDefaultsSellPrice(Guid subscriberId, SubscriberDefaultSellingPrice request);
        Task<SubscriberDefaultSellingPrice> UpdateSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId, SubscriberDefaultSellingPrice request);
        Task<bool> DeleteSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId);

        Guid TraceId { get; set; }
    }
}
