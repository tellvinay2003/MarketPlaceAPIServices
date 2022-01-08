using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublisherServiceStatus
    {
        public Guid PublisherServiceStatusId { get; set; }
        public Guid PublisherId { get; set; }
        public int ServiceStatusId { get; set; }        
        public int? PackageStatusId { get; set; }
        public virtual Publisher Publisher { get; set; }
    }
}
