using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MarketPlaceService.Entities.Booking
{
    public class ProcessSubscriberBookingRequest
    {       
        public Guid SubscriberSiteId { get; set; }
        public int BookingId { get; set; }
    }
}
