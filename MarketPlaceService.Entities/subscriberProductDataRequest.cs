using System;

namespace MarketPlaceService.Entities
{
    public class SubscriberProductDataRequest
    {
        public Guid SubscriberId { get; set; }
        public Guid MarketPlaceProductId { get; set; }
        public int ProductTypeId { get; set; }
        public Guid SubscriberProductId { get; set; }
        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
        public short? JobTypeId { get; set; }
    }
}
