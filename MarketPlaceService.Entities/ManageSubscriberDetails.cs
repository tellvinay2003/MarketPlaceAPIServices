using System;

namespace MarketPlaceService.Entities
{
    public class ManageSubscriberDetails
    {
        public Guid SubscriberId{get;set;}
        public string SubscriberName {get;set;}
        public Guid PublishedProductId {get;set;}
        public Guid SubscriberProductId {get;set;}
    }
}
