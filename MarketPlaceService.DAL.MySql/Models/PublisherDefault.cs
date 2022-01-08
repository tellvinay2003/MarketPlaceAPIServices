using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublisherDefault
    {
        public Guid PublisherDefaultId { get; set; }
        public Guid PublisherId { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? PackagePriceStartDate { get; set; }

        public virtual Publisher Publisher { get; set; }
    }
}
