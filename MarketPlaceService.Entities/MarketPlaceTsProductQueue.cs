using System;

namespace MarketPlaceService.Entities
{
    public class MarketPlaceTsProductQueue
    {
        
        public Guid MarketplaceProductId { get; set; }
        public short MessageTypeId { get; set; }       
        public Guid SubscriberId { get; set; }
        public Guid MarketPlaceTsProductId {get;set;}
        public Guid SubscriberProductId { get; set; }
        public int? OrganisationId { get; set; }
        public int ProductVersion { get; set; }
        public string ProductData { get; set; }
        public string ProductDefaults { get; set; }
        public short ProductStatusId { get; set; }
        public DateTime ImportedOn { get; set; }
        public string ProductStatusNote { get; set; }
        public string ImportedBy { get; set; }
        public DateTime SubscribedOn { get; set; }
        public string SubscribedBy { get; set; }
        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
        public string MarketplaceApiUrl { get; set; }
        public string ProductUpdateDifferenceData { get; set; }
        public short? JobTypeId { get; set; }
		public short ProductTypeId{get;set;}
    }
}
