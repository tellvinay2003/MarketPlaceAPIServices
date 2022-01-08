using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class ProcessSiteBookingRequest
    {
        public BookingInfoResponse BookingDetails { get; set; }
        public Guid PublisherSiteId { get; set; }
        public Guid SiteBookingId { get; set; }
    }
}
