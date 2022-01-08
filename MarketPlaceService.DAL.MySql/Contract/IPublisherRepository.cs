using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Contract
{
    public interface IPublisherRepository
    {
        Task<PublisherDataModel> AddNewPublisher(PublisherDataModel publisher);
        Task<bool> DeletePublisher(Guid id);
        Task<PublisherDataModel> UpdatePublisher(PublisherDataModel publisher);
        Task<PublisherDataModel> GetPublisherByIdAsync(Guid id);        
        // Task<IEnumerable<PublisherDataModel>> GetPublishersListAsync(int pageNumber, int pageSize);
        Task<IEnumerable<PublisherDataModel>> GetPublishersListAsync();

        // Task<GeoTreeDataModel> GetGeoTree();

        // Task<IEnumerable<ServiceTypeDataModel>> GetServiceTypesAsync();
        // Task<IEnumerable<RegionDataModel>> GetLocationsAsync();
        // Task<IEnumerable<PublishedProductsDataModel>> SearchUnpublishedProductAsync();
        Task<PublishedProductsDataModel> PublishProduct(PublishedProductsDataModel publishedProductsDataModel);
        Task<IEnumerable<QueuedPublication>> GetQueuedPublications(int limit, string serverName);
        Task<QueuedPublication> UpdatePublicationQueue(QueuedPublication queuedPublicationDataModel);
        Task<IEnumerable<Entities.PublishedStatus>> GetPublishStatus();
        //Task<IEnumerable<Entities.ProcessingStatus>> GetProcessingStatus();
        Task<PublishedProductsDataModel> UpdatePublishedProduct(PublishedProductsDataModel publishedProductsDataModel);
        Task<QueuedPublication> InsertPublicationHistory(QueuedPublication queuedPublicationDataModel);
        Task<QueuedPublication> InsertPublicationQueue(QueuedPublication queuedPublicationDataModel);
        Task DeletePublishedProductQueue(Guid publishedProductQueueId);
        Task<string> GetPublishedProductDataString(Guid publishedProductId);
        Task<IEnumerable<CurrentSubscriber>> GetCurrentSubscribers(Guid publishedProductId);
        Task<string> GetPublishedProductDataMarketplaceString(Guid publishedProductId);
         
         Task<string> GetSiteUrlBasedOnSiteId(Guid publishedProductQueueId);

        Task<GetPublishProductResponse> GetPublishProduct(int productId, Guid publisherId);

        Task<IEnumerable<PublisherAgentMap>> GetPublisherAgentMaps(Guid publisherId); //get publisher subscriber agent map
        Task<PublisherAgentMap> GetPublisherAgentMaps(Guid publisherId, Guid subscriberId);
        Task<PublisherAgentMap> InsertPublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId);
        Task<PublisherAgentMap> UpdatePublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId);
        Task<bool> DeletePublisherAgentMaps(Guid publisherId, Guid subscriberId);   

        Task<PublisherDefaultDataModel> GetPublisherDefaults(Guid publisherId);
        Task<PublisherDefaultDataModel> InsertUpdatePublisherDefaults(Guid publisherId, PublisherDefaultDataModel request);

        Task<IEnumerable<PublishedProductsDataModel>> GetPublishedUnpublishedProducts(Guid publisherId, int productType, IEnumerable<ServiceDataModel> products, int? publishedStatus, IEnumerable<PackageDataModel> packages, Entities.ProductType productTypeId);
        Task<IEnumerable<Entities.Status>> GetPublisherProductStatus();

        Task<int> GetMessageTypeIdAsync(Guid publishedProductId);
        
        Task<PublishedProductDataResponse> InsertUpdateMarketPlaceProductData(IEnumerable<RetriveFieldPathResponse> fieldList, Guid publishedProductId, string json, int messageTypeId);
        Task<IEnumerable<HistoryDetail>> GetPublicationHistoryDetails(Guid publishedProductId);
        Task<DateTime> GetFirstPublicationDate(Guid publishedProductId);

        Task<IEnumerable<Entities.PublishedProductAllowedSubscriber>> GetAllowedSubscribers(List<Guid> productIds);

        Task<Entities.PublishedProductAllowedSubscriber> AllowedSubscriber(Guid publishedproductId,Guid subscriberId);

        Task<bool> DeleteAllowedSubscriber(Guid publishedProductId,Guid subscriberId, int calledFrom);

        Task<IEnumerable<ManageSubscribersResponse>> SearchManageSubscribers(ManageSubscribersSearch manageSubscribersDataModel);
        PublishedProductsDataModel GetExistingPublishedProduct(int productId, Guid publisherId, Entities.ProductType productType);
        Task<bool> InsertStaticDataUpdateQueue(StaticDataUpdateQueueRequest insertStaticDataUpdateQueueRequest);
        Task<bool> UpdatePublishedProductStatus(Guid publishedProductId, string status);
        Task<List<Entities.PublishedProductAllowedSubscriber>> GetPublishedProductSubscriberList(Guid publishedProductId);
        Task<List<Entities.PublishedStatus>> GetPublishedStatus();
        Task InsertPublicationHistoryForUnpublishProduct(Guid publishedProductId, Guid? subscriberId, int calledFrom);
        void InsertSubscribeProductQueue(Guid marketplaceProductId,Guid subscriberProductId,Guid subscriberId, short jobTypeId);
        Task InsertSubscriptionHistoryForUnsubscribeProduct(Guid? subscriberProductId, int calledFrom, Guid? userId);
        Task<string> GetPublishedProductHistoryJsonData(Guid publishedProductHistoryId);
        Task<bool> IsImportSupplierAddress(int serviceTypeId);
        Guid UserId { get; set; }
        Task<PublisherDefaultDataModel> GetPublisherDefaultsForPackage(Guid publisherId);
        int GetPackageToServiceMapForType(Guid siteId, int dataTypeId, int sourceId);
        Task<IEnumerable<Entities.MessageType>> GetMessageTypes();
        string GetMasterDataName(int masterDataId);
    }
}
