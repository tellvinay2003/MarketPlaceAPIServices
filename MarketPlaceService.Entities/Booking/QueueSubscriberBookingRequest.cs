using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Booking
{
    public class QueueSubscriberBookingRequest
    {
        public Guid SubscriberSiteId { get; set; }
        public int BookingId { get; set; }
    }
}
