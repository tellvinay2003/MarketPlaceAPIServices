using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MessageTypes
    {
        public MessageTypes()
        {
            MarketplaceProduct = new HashSet<MarketplaceProduct>();
            MarketplaceProductHistory = new HashSet<MarketplaceProductHistory>();
            MessageFields = new HashSet<MessageFields>();
            PublishedProducts = new HashSet<PublishedProducts>();
            PublishedProductsHistory = new HashSet<PublishedProductsHistory>();
            SubscriberProduct = new HashSet<SubscriberProduct>();
            SubscriberProductHistory = new HashSet<SubscriberProductHistory>();
            SubscriberProductQueue = new HashSet<SubscriberProductQueue>();
            SubscriberProductQueueHistory = new HashSet<SubscriberProductQueueHistory>();
            SubscriberProductTsUpdateQueue = new HashSet<SubscriberProductTsUpdateQueue>();
            SubscriberProductTsUpdateQueueHistory = new HashSet<SubscriberProductTsUpdateQueueHistory>();
        }

        public int Messagetypeid { get; set; }
        public string Messagetypename { get; set; }

        public virtual ICollection<MarketplaceProduct> MarketplaceProduct { get; set; }
        public virtual ICollection<MarketplaceProductHistory> MarketplaceProductHistory { get; set; }
        public virtual ICollection<MessageFields> MessageFields { get; set; }
        public virtual ICollection<PublishedProducts> PublishedProducts { get; set; }
        public virtual ICollection<PublishedProductsHistory> PublishedProductsHistory { get; set; }
        public virtual ICollection<SubscriberProduct> SubscriberProduct { get; set; }
        public virtual ICollection<SubscriberProductHistory> SubscriberProductHistory { get; set; }
        public virtual ICollection<SubscriberProductQueue> SubscriberProductQueue { get; set; }
        public virtual ICollection<SubscriberProductQueueHistory> SubscriberProductQueueHistory { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueue> SubscriberProductTsUpdateQueue { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueueHistory> SubscriberProductTsUpdateQueueHistory { get; set; }
    }
}
