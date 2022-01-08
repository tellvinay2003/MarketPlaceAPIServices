using System;

namespace MarketPlaceService.Entities.Booking
{
    public class QueueBookingUpdateFromPublisherRequest
    {
       public int SubscriberbookingId {get;set;}
       public string SubscriberBookingRef {get;set;}
       public int BookingVersion {get;set;}
       public Guid SiteBookingId {get;set;}
       public string BookingData {get;set;}
       public string JobNote{get;set;}
      
    }
}
