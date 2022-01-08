using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class MarketplaceBookingPushQueueData
    {
        public Guid MarketplaceBookingPushQueueId { get; set; }
        public Guid SubscriberSiteId { get; set; }
        public int BookingId { get; set; }
    }
}
