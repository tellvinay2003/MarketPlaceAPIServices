using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Booking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MarketPlaceService.Entities.Booking;

namespace MarketPlaceService.DAL.Contract
{
    public interface IBookingRepository
    {
        Task<List<SiteServiceData>> GetSiteServiceData(List<Guid> marketplaceProductIds, Guid subscriberId);
        Task<bool> QueueSubscriberBooking(Guid subscriberSiteId, int bookingId, Guid traceId, bool insertmarketplaceBookingRecord);
        Task<MarketplaceBookingPushQueueData> GetMarketplaceBookingPushQueueRecord(int bookingId, Guid subscriberSiteId);
        void QueueSiteBookingRecord(Guid publisherSiteId, Guid subscriberSiteId, BookingInfoResponse bookinginfo, Guid traceId, Guid marketplaceBookingPushQueueId, Guid siteBookingId);
        Task<bool> UpdateSiteBookingStatus(ProcessSiteBookingRequest request, int bookingStatusId);
        Task<bool> UpdateBookingStatusOnSubscriberCallback(ProcessSubscriberCallbackRequest request);
        Task<string> GetSavedPublisherBookingInfo(ProcessBookingUpdateFromPublisherRequest request);
        Task UpdatePublisherInfoAndDiff(string publisherBookingInfoStr, string bookingInfoDiff, Guid siteBookingId, string bookingName, string bookingReference, string jobNote);
        Task<bool> UpdateSubscriberBookingInfoAndDiff(string bookingInfo, string bookingInfoDiff, Guid siteBookingId);
        Task UpdateSiteBookingStatusId(Guid siteBookingId, int bookingStatusId);
        Task<List<MappingData>> GetBookedOptionStatusMapping(Guid publisherSiteId, int direction);
        Task<List<MappingData>> GetSubscriberBookedOptionStatusMapping(Guid siteBookingId, int direction);
        Task<SiteBooking> GetSiteBookingData(Guid siteBookingId);
        Task<MarketplaceBooking> GetMarketplaceBookingData(Guid marketplacebookingid);
        Task<Guid> GetMarketplaceBookingId(int bookingId, Guid subscriberSiteId);
        Task<Guid?> GetSiteBookingId(Guid marketplaceBookingId, Guid publisherSiteId);
        Task<Guid> InsertIntoSiteBooking(string bookingData, Guid publisherSiteId, Guid marketplaceBookingId);

        Task<IEnumerable<string>> GetBookingReference(string bookingReference, int limit);

        Task<BookingSearchResponse> BookingSearch(BookingSearchRequest request);

        Task<GetBookingStatusResponse> GetBookingStatus();
        Task<GetMpBookingInfoResponse> GetMpSubscriberBookingInfo(GetMpBookingInfoRequest request);

        Task<GetMpBookingInfoResponse> GetMpPublisherBookingInfo(GetMpBookingInfoRequest request);
        Task<GetMpBookingHistoryInfoResponse> GetMpSubscriberBookingHistoryInfo(GetMpBookingHistoryInfoRequest request);

        Task<GetMpBookingHistoryInfoResponse> GetMpPublisherBookingHistoryInfo(GetMpBookingHistoryInfoRequest request);
        Task<GetMpBookingJsonDataResponse> GetMpSubscriberBookingJsonData(GetMpBookingJsonDataRequest request);
        Task<GetMpBookingJsonDataResponse> GetMpPublisherBookingJsonData(GetMpBookingJsonDataRequest request);
        Task<GetMpBookingJsonDataResponse> GetLatestMpSubscriberBookingJsonData(GetMpBookingJsonDataRequest request);
        Task<GetMpBookingJsonDataResponse> GetLatestMpPublisherBookingJsonData(GetMpBookingJsonDataRequest request);
        Task UpdateMarketPlaceBooking(Guid marketplaceBookingId, BookingInfoResponse bookingInfo, Guid subscriberId);
        bool InsertSiteBookingPublisherData(Guid siteBookingId, List<SiteServiceData> siteService);
    }
}
