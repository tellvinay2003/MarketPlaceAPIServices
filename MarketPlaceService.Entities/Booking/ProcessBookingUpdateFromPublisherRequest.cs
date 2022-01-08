using System;

namespace MarketPlaceService.Entities.Booking
{
    public class ProcessBookingUpdateFromPublisherRequest
    {
        public int PublisherBookingId { get; set; }
        public Guid SiteBookingId { get; set; }
        public Guid PublisherSiteId { get; set; }
        public Int16 JobTypeId { get; set; }
        public string JobNote{get;set;}
    }
}
