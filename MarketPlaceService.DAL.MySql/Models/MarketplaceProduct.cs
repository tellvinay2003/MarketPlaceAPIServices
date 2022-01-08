using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MarketplaceProduct
    {
        public MarketplaceProduct()
        {
            MarketplaceProductRating = new HashSet<MarketplaceProductRating>();
            SubscriberProduct = new HashSet<SubscriberProduct>();
            SubscriberProductHistory = new HashSet<SubscriberProductHistory>();
            SubscriberProductQueue = new HashSet<SubscriberProductQueue>();
            SubscriberProductQueueHistory = new HashSet<SubscriberProductQueueHistory>();
            SubscriberProductTsUpdateQueue = new HashSet<SubscriberProductTsUpdateQueue>();
            SubscriberProductTsUpdateQueueHistory = new HashSet<SubscriberProductTsUpdateQueueHistory>();
        }

        public Guid Marketplaceproductid { get; set; }
        public Guid Publishedproductid { get; set; }
        public string Productdata { get; set; }
        public DateTime Processedon { get; set; }
        public string Processedby { get; set; }
        public int Servicetypeid { get; set; }
        public int Regionid { get; set; }
        public string Productlongname { get; set; }
        public string Productshortname { get; set; }
        public int Messagetypeid { get; set; }

        public virtual MessageTypes Messagetype { get; set; }
        public virtual PublishedProducts Publishedproduct { get; set; }
        public virtual MasterRegions Region { get; set; }
        public virtual MasterData Servicetype { get; set; }
        public virtual ICollection<MarketplaceProductRating> MarketplaceProductRating { get; set; }
        public virtual ICollection<SubscriberProduct> SubscriberProduct { get; set; }
        public virtual ICollection<SubscriberProductHistory> SubscriberProductHistory { get; set; }
        public virtual ICollection<SubscriberProductQueue> SubscriberProductQueue { get; set; }
        public virtual ICollection<SubscriberProductQueueHistory> SubscriberProductQueueHistory { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueue> SubscriberProductTsUpdateQueue { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueueHistory> SubscriberProductTsUpdateQueueHistory { get; set; }
    }
}
