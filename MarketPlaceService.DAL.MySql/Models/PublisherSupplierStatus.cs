using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublisherSupplierStatus
    {
        public Guid PublisherSupplierStatusId { get; set; }
        public Guid PublisherId { get; set; }
        public int SupplierStatusId { get; set; }

        public virtual Publisher Publisher { get; set; }
    }
}
