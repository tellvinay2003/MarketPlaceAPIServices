using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class Subscriber
    {
        public Subscriber()
        {
            MarketplaceBooking = new HashSet<MarketplaceBooking>();
            MarketplaceBookingHistory = new HashSet<MarketplaceBookingHistory>();
            PublishedProductAllowedSubscriber = new HashSet<PublishedProductAllowedSubscriber>();
            PublisherAgent = new HashSet<PublisherAgent>();
            SubscriberChargingPolicy = new HashSet<SubscriberChargingPolicy>();
            SubscriberDefault = new HashSet<SubscriberDefault>();
            SubscriberDefaultSellingPrices = new HashSet<SubscriberDefaultSellingPrices>();
            SubscriberProduct = new HashSet<SubscriberProduct>();
            SubscriberProductCode = new HashSet<SubscriberProductCode>();
            SubscriberProductHistory = new HashSet<SubscriberProductHistory>();
            SubscriberProductQueue = new HashSet<SubscriberProductQueue>();
            SubscriberProductQueueHistory = new HashSet<SubscriberProductQueueHistory>();
            SubscriberProductTsUpdateQueue = new HashSet<SubscriberProductTsUpdateQueue>();
            SubscriberProductTsUpdateQueueHistory = new HashSet<SubscriberProductTsUpdateQueueHistory>();
            SubscriberSupplier = new HashSet<SubscriberSupplier>();
        }

        public Guid SubscriberId { get; set; }
        public Guid SiteId { get; set; }
        public int? OrganizationId { get; set; }
        public string SubscriberName { get; set; }
        public bool Enabled { get; set; }

        public virtual Site Site { get; set; }
        public virtual ICollection<MarketplaceBooking> MarketplaceBooking { get; set; }
        public virtual ICollection<MarketplaceBookingHistory> MarketplaceBookingHistory { get; set; }
        public virtual ICollection<PublishedProductAllowedSubscriber> PublishedProductAllowedSubscriber { get; set; }
        public virtual ICollection<PublisherAgent> PublisherAgent { get; set; }
        public virtual ICollection<SubscriberChargingPolicy> SubscriberChargingPolicy { get; set; }
        public virtual ICollection<SubscriberDefault> SubscriberDefault { get; set; }
        public virtual ICollection<SubscriberDefaultSellingPrices> SubscriberDefaultSellingPrices { get; set; }
        public virtual ICollection<SubscriberProduct> SubscriberProduct { get; set; }
        public virtual ICollection<SubscriberProductCode> SubscriberProductCode { get; set; }
        public virtual ICollection<SubscriberProductHistory> SubscriberProductHistory { get; set; }
        public virtual ICollection<SubscriberProductQueue> SubscriberProductQueue { get; set; }
        public virtual ICollection<SubscriberProductQueueHistory> SubscriberProductQueueHistory { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueue> SubscriberProductTsUpdateQueue { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueueHistory> SubscriberProductTsUpdateQueueHistory { get; set; }
        public virtual ICollection<SubscriberSupplier> SubscriberSupplier { get; set; }
    }
}
