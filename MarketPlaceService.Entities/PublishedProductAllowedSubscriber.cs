using System;

namespace MarketPlaceService.Entities
{
    public class PublishedProductAllowedSubscriber
    {
        public Guid PublishedProductAllowedSubscriberId {get;set;}

        public Guid PublisherProductId {get;set;}

        public Guid SubscriberId {get;set;}

        public Guid? SubscriberProductId { get; set; }
    }
}
