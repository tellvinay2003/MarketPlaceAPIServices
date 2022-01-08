using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublishedProducts
    {
        public PublishedProducts()
        {
            MarketplaceProduct = new HashSet<MarketplaceProduct>();
            MarketplaceProductHistory = new HashSet<MarketplaceProductHistory>();
            PublishedProductAllowedSubscriber = new HashSet<PublishedProductAllowedSubscriber>();
            PublishedProductsQueue = new HashSet<PublishedProductsQueue>();
        }

        public Guid PublishedProductId { get; set; }
        public Guid PublisherId { get; set; }
        public short ProductTypeId { get; set; }
        public int ProductId { get; set; }
        public int ProductVersion { get; set; }
        public string ProductData { get; set; }
        public short? PublishedStatusId { get; set; }
        public DateTime ProcessedOn { get; set; }
        public string ProcessingNote { get; set; }
        public string ProcessedBy { get; set; }
        public Guid? PublishedBy { get; set; }
        public DateTime PublishedOn { get; set; }
        public short? Productsubstatusid { get; set; }
        public int? Messagetypeid { get; set; }
        public short? Publisherproductstatusid { get; set; }
        public Guid? Traceid { get; set; }
        public string Productdatadiff { get; set; }

        public virtual MessageTypes Messagetype { get; set; }
        public virtual PublisherProductSubStatus Productsubstatus { get; set; }
        public virtual PublishedStatus PublishedStatus { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual PublisherProductStatus Publisherproductstatus { get; set; }
        public virtual ICollection<MarketplaceProduct> MarketplaceProduct { get; set; }
        public virtual ICollection<MarketplaceProductHistory> MarketplaceProductHistory { get; set; }
        public virtual ICollection<PublishedProductAllowedSubscriber> PublishedProductAllowedSubscriber { get; set; }
        public virtual ICollection<PublishedProductsQueue> PublishedProductsQueue { get; set; }
    }
}
