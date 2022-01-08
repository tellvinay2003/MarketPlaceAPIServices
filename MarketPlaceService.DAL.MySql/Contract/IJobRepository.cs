using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities.Job;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace MarketPlaceService.DAL.Contract
{
    public interface IJobRepository
    {

        Task<JobSearchResponse> SearchPublishedProductsQueue(JobSearchRequest request);
        Task<JobSearchResponse> SearchPublishedProductsQueueHistory(JobSearchRequest request);
        Task<JobSearchResponse> SearchSubscriberProductsQueue(JobSearchRequest request);
        Task<JobSearchResponse> SearchSubscriberProductsQueueHistory(JobSearchRequest request);
        Task<JobSearchResponse> SearchSubscriberProductTsUpdateQueue(JobSearchRequest request);
        Task<JobSearchResponse> SearchSubscriberProductTsUpdateQueueHistory(JobSearchRequest request);
        Task<JobSearchResponse> SearchMarketplaceBookingPushQueue(JobSearchRequest request);
        Task<JobSearchResponse> SearchMarketplaceBookingPushQueueHistory(JobSearchRequest request);
        Task<JobSearchResponse> SearchBookingUpdateFromPublisherQueue(JobSearchRequest request);
        Task<JobSearchResponse> SearchBookingUpdateFromPublisherQueueHistory(JobSearchRequest request);
        Task<JobSearchResponse> SearchSiteBookingPushQueue(JobSearchRequest request);
        Task<JobSearchResponse> SearchSiteBookingPushQueueHistory(JobSearchRequest request);
        bool SaveChanges();
        BookingUpdateFromPublisherQueue UpdateBookingUpdateFromPublisherQueueData(Guid queueId);
        MarketplaceBookingPushQueue UpdateMarketplaceBookingPushQueueData(Guid queueId);
        PublishedProductsQueue UpdatePublishedProductsQueueData(Guid queueId);
        SiteBookingPushQueue UpdateSiteBookingPushQueueData(Guid queueId);
        SubscriberProductQueue UpdateSubscriberProductQueueData(Guid queueId);
        Models.SubscriberProductTsUpdateQueue UpdateSubscriberProductTsUpdateQueueData(Guid queueId);
        BookingUpdateFromPublisherQueue InsertBookingUpdateFromPublisherQueueData(Guid queueHistoryId, Guid traceId);
        MarketplaceBookingPushQueue InsertMarketplaceBookingPushQueueData(Guid queueHistoryId, Guid traceId);
        PublishedProductsQueue InsertPublishedProductsQueueData(Guid queueHistoryId, Guid traceId);
        SiteBookingPushQueue InsertSiteBookingPushQueueData(Guid queueHistoryId, Guid traceId);
        SubscriberProductQueue InsertSubscriberProductQueueData(Guid queueHistoryId, Guid traceId);
        Models.SubscriberProductTsUpdateQueue InsertSubscriberProductTsUpdateQueueData(Guid queueHistoryId, Guid traceId);
        Task<JobInfoResponse> GetPublishedProductsQueueRecord(JobInfoRequest request);        
        Task<JobInfoResponse> GetPublishedProductsQueueHistoryRecord(JobInfoRequest request);
     
        Task<JobInfoResponse> GetSubscriberProductsQueueRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetSubscriberProductsQueueHistoryRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetSubscriberProductTsUpdateQueueRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetSubscriberProductTsUpdateQueueHistoryRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetMarketplaceBookingPushQueueRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetMarketplaceBookingPushQueueHistoryRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetBookingUpdateFromPublisherQueueRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetBookingUpdateFromPublisherQueueHistoryRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetSiteBookingPushQueueRecord(JobInfoRequest request);

        Task<JobInfoResponse> GetSiteBookingPushQueueHistoryRecord(JobInfoRequest request);


        PublishedProductsQueueHistory GetPublishedProductQueueHistoryItem(Guid historyId);
        List<PublishedProductsQueue> GetPublishedProductQueueItems();
        SubscriberProductQueueHistory getSubscriberProductQueueHistoryItem(Guid historyId);
        List<SubscriberProductQueue> GetSubscriberProductQueueItems();




    }
}
