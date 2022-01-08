using System;

namespace MarketPlaceService.Entities
{
    public class CurrentSubscriber
    {
        public Guid SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}
