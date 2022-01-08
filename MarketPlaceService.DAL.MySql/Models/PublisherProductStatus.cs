using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublisherProductStatus
    {
        public PublisherProductStatus()
        {
            PublishedProducts = new HashSet<PublishedProducts>();
            PublishedProductsHistory = new HashSet<PublishedProductsHistory>();
        }

        public short Productstatusid { get; set; }
        public string Productstatusname { get; set; }

        public virtual ICollection<PublishedProducts> PublishedProducts { get; set; }
        public virtual ICollection<PublishedProductsHistory> PublishedProductsHistory { get; set; }
    }
}
