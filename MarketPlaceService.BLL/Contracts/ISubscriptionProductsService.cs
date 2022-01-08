using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface ISubscriptionProductsService
    {
        Task<SubscriptionProduct> SubscriptionProduct(SubscriptionProduct subscriptionProductDataModel);

        Task<SubscriptionProduct> UpdateSubscriptionProduct(SubscriptionProduct subscriptionProductDataModel);

        Task<SubscriptionProduct> InsertSubscriberProductHistory(QueuedSubscription queuedSubscriberDataModel);

        Task<IEnumerable<SubscriptionStatus>> GetSubscriptionStatus();

        Task<IEnumerable<MarketPlaceProduct>> SearchSubscriptionProduct(MarketPlaceProductsSearch marketPlaceProductSearchDataModel);

        Task<IEnumerable<QueuedSubscription>> GetQueuedSubscriptions(int limit, string serverName);

        Task<QueuedSubscription> GetQueuedSubscriptionsById(Guid subScriberProductQueuedId);

        Task DeleteSubscriptionProductQueue(Guid subScriberProductQueuedId);

        Task<QueuedSubscription> UpdateSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel);

        Task<SubscriberProductDataResponse> ProcessSubscriberProduct(SubscriberProductDataRequest request);

        Task<SubscriptionProductInfo> GetSubscriptionProductInfo(Guid marketplaceProductId, Guid subscriberId);

        Guid TraceId { get; set; }
        Task<QueuedSubscription> InsertSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel);
        Task<SubscriptionProduct> UnsubscribeProduct(SubscriptionProduct subscriptionProductDataModel);
        Task<string> GetSubscriptionProductHistoryJson(Guid subscriberProductHistoryId);
        Guid UserId { get; set; }
    }
}
