using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IProfileService
    {
        Task<PublisherDataModel> GetPublisherById(Guid id);
        Task<PublisherDataModel> AddNewPublisher(PublisherDataModel publisherItem);
        Task<IEnumerable<PublisherDataModel>> GetPublishersListAsync(int pageNumber, int pageSize);
        Task<IEnumerable<PublisherDataModel>> GetPublishersListAsync();
        Task<PublisherDataModel> UpdatePublisher(PublisherDataModel publisherItem);
        Task<bool> DeletePublisher(Guid id);
        // Task<GeoTreeDataModel> GetGeoTree();
        // Task<IEnumerable<ServiceTypeDataModel>> GetServiceTypesAsync();
        // Task<IEnumerable<RegionDataModel>> GetLocationsAsync();
        // Task<IEnumerable<PublishedProductsDataModel>> SearchUnpublishedProductAsync();        
        Task<PublishedProductsDataModel> PublishProduct(PublishedProductsDataModel publishedProductsDataModel);
        Task<IEnumerable<QueuedPublication>> GetQueuedPublications(int limit, string serverName);
        Task<QueuedPublication> UpdatePublicationQueue(QueuedPublication queuedPublicationDataModel);
        Task<IEnumerable<PublishedStatus>> GetPublishStatus();
        //Task<IEnumerable<ProcessingStatus>> GetProcessingStatus();
        Task<PublishedProductsDataModel> UpdatePublishedProduct(PublishedProductsDataModel publishedProductsDataModel);
        Task<QueuedPublication> InsertPublicationHistory(QueuedPublication queuedPublicationDataModel);
        Task<QueuedPublication> InsertPublicationQueue(QueuedPublication queuedPublicationDataModel);
        Task DeletePublishedProductQueue(Guid publishedProductQueueId);

        Task<PublishedProductDataResponse> ProcessPublishedProduct(ProductDataRequest request);

        Task<string> GetPublishedProductDataString(Guid publishedProductId);
        Task<PublishedProductInfo> GetPublishedProductInfo(Guid publishedProductId);
        
       
        

        Task<string> SearchProductAsync(int serviceTypeId, int RegionId, string productName, Guid publisherId);       

        // Task<string> GetLocationsAsync(Guid publisherId);

        Task<GetPublishProductResponse> GetPublishProduct(int productId, Guid publisherId);

         Task<IEnumerable<PublisherAgentMap>> GetPublisherAgentMaps(Guid publisherId); //get publisher subscriber agent map
        Task<PublisherAgentMap> GetPublisherAgentMaps(Guid publisherId, Guid subscriberId);
        Task<PublisherAgentMap> InsertPublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId);
        Task<PublisherAgentMap> UpdatePublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId);
        Task<bool> DeletePublisherAgentMaps(Guid publisherId, Guid subscriberId);
        Task<PublisherDefaultDataModel> GetPublisherDefaults(Guid publisherId);
        Task<PublisherDefaultDataModel> InsertUpdatePublisherDefaults(Guid publisherId, PublisherDefaultDataModel request);

        Task<IEnumerable<PublishedProductsDataModel>> GetPublishedUnpublishedProducts(Guid publisherId, int productType, IEnumerable<ServiceDataModel> products, int? publishedStatus, IEnumerable<PackageDataModel> packages, ProductType productTypeId);

        Guid TraceId { get; set; }

        Task<IEnumerable<PublishedProductAllowedSubscriber>> GetAllowedSubscribers(List<Guid> productIds);

        Task<PublishedProductAllowedSubscriber> AllowedSubscriber(Guid publishedproductId,Guid subscriberId);

        Task<bool> DeleteAllowedSubscriber(Guid publishedProductId,Guid subscriberId);
        Task<IEnumerable<ManageSubscribersResponse>> SearchManageSubscribers(ManageSubscribersSearch manageSubscribersDataModel);

        Task<QueuedPublication> InsertStaticDataUpdateQueue(StaticDataUpdateQueueRequest insertStaticDataUpdateQueueRequest);
        Task<PublishedProductsDataModel> UnpublishProduct(PublishedProductsDataModel publishedProductsDataModel);
        Task<List<PublishedStatus>> GetPublishedStatus();
        Task<string> GetPublishedProductHistoryJsonData(Guid publishedProductHistoryId);
        Guid UserId { get; set; }

        Task<string> SearchPackageProductAsync(int packageTypeId, int regionId, string productName, Guid publisherId);
    }
}
