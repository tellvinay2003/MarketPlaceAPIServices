using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublisherProductSubStatus
    {
        public PublisherProductSubStatus()
        {
            PublishedProducts = new HashSet<PublishedProducts>();
            PublishedProductsHistory = new HashSet<PublishedProductsHistory>();
            PublishedProductsQueueHistory = new HashSet<PublishedProductsQueueHistory>();
        }

        public short Productsubstatusid { get; set; }
        public string Productsubstatusname { get; set; }

        public virtual ICollection<PublishedProducts> PublishedProducts { get; set; }
        public virtual ICollection<PublishedProductsHistory> PublishedProductsHistory { get; set; }
        public virtual ICollection<PublishedProductsQueueHistory> PublishedProductsQueueHistory { get; set; }
    }
}
