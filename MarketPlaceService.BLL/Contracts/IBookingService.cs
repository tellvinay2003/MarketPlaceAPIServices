using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Booking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IBookingService
    {
        Guid TraceId { get; set; }
        Task<QueueSubscriberBookingResponse> QueueSubscriberBooking(QueueSubscriberBookingRequest request);
        Task<ProcessSubscriberBookingResponse> ProcessSubscriberBooking(ProcessSubscriberBookingRequest request);
        Task<ProcessSiteBookingResponse> ProcessSiteBooking(ProcessSiteBookingRequest request);
        Task<ProcessSubscriberCallbackResponse> ProcessSubscriberCallback(ProcessSubscriberCallbackRequest request);
        Task<ProcessBookingUpdateFromPublisherResponse> ProcessBookingUpdateFromPublisher(ProcessBookingUpdateFromPublisherRequest request);        
        Task<IEnumerable<string>> GetBookingReference(string bookingReference,int limit);
        Task<BookingSearchResponse> BookingSearch(BookingSearchRequest request);        

        Task<GetBookingStatusResponse> GetBookingStatus();
        
        Task<GetMpBookingInfoResponse> GetMpSubscriberBookingInfo(GetMpBookingInfoRequest request);
        Task<GetMpBookingInfoResponse> GetMpPublisherBookingInfo(GetMpBookingInfoRequest request);
        Task<GetMpBookingHistoryInfoResponse> GetMpSubscriberBookingHistoryInfo(GetMpBookingHistoryInfoRequest request);

        Task<GetMpBookingHistoryInfoResponse> GetMpPublisherBookingHistoryInfo(GetMpBookingHistoryInfoRequest request);

        Task<GetMpBookingJsonDataResponse> GetMpBookingJsonData(GetMpBookingJsonDataRequest request);

        Task<ApplicableRulesResponse> GetServiceApplicableRules(ApplicableRulesRequest request);
        Task<ServiceRuleResponse> GetServiceRules(ServiceRuleRequest request);
    }
}
