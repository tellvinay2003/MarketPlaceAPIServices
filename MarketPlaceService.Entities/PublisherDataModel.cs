using System;
using System.ComponentModel.DataAnnotations;

namespace MarketPlaceService.Entities
{
    public class InsertPublisherDataModel
    {        
        [Required]
        public Guid SiteId { get; set; }
        public int? OrganizationId { get; set; }
        [Required]
        public string PublisherName { get; set; }        
        public bool Enabled { get; set; }
    }

    public class PublisherDataModel : InsertPublisherDataModel
    {
        [Required]
        public Guid PublisherId { get; set; }
        public string SiteName { get; set; }
    }


}
