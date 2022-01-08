using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using MarketPlaceService.Entities;
using MarketPlaceService.DAL.MySql.Models;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Contract
{
    public interface ISubscriptionProductsRepository
    {
        Task<SubscriptionProduct> SubscriptionProduct(SubscriptionProduct subscriptionProductDataModel);

        Task<SubscriptionProduct> UpdateSubscriptionProduct(SubscriptionProduct subscriptionProductDataModel);
        Task<IEnumerable<SubscriptionStatus>> GetSubscriptionStatus();

        Task<SubscriptionProduct> InsertSubscriberProductHistory(QueuedSubscription queuedSubscriberDataModel);

        Task<IEnumerable<MarketPlaceProduct>> SearchSubscriptionProduct(MarketPlaceProductsSearch marketPlaceProductSearchDataModel);


        Task<IEnumerable<QueuedSubscription>> GetQueuedSubscriptions(int limit, string serverName);

        Task<QueuedSubscription> GetQueuedSubscriptionsById(Guid subScriberProductQueuedId);

        Task DeleteSubscriptionProductQueue(Guid subScriberProductQueuedId);

        Task<QueuedSubscription> UpdateSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel);

        Task<IEnumerable<Entities.Status>> GetSubscriberProductStatus();

        Task<int> GetMessageTypeIdAsync(Guid subscriberProductId);

        Task<int> GetMappingDirection();

        Task<string> GetMarketplaceJsonString(Guid marketplaceProductId);

        Task<string> GetSubscriptionJsonString(Guid marketplaceProductId, Guid subscriberId);

        Task<IEnumerable<HistoryDetail>> GetSubscriptionHistoryDetails(Guid marketplaceProductId, Guid subscriberId);

        Task<string> GetPublishedByName(Guid marketplaceProductId);
        Task<DateTime> GetFirstSubscriptionDate(Guid marketplaceProductId, Guid subscriberId);

        SubscriberDefaultsModelForSubscription GetSubscriberDefaults(Guid SubscriberId, Guid marketplaceProductId);
        SiteDataModel GetSiteDetailsFromMarketplaceProduct(Guid marketplaceProductId);

        Task<QueuedSubscription> InsertSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel);

        SubscriptionProduct GetExistingSubscriptionProduct(Guid marketplaceProductId, Guid subscriberId);
        List<Guid> GetSubscriberIdsForSubscribedProduct(Guid marketplaceProductId);
        Task<bool> UnsubscribeProduct(Guid subscriberProductId, Guid marketplaceProductId);
        Task<string> GetSubscriptionProductHistoryJson(Guid subscriberProductHistoryId);
        Guid UserId { get; set; }
    }
}
