using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class PublishedProductAllowedSubscriber
    {
        public Guid Publishedproductallowedsubscriberid { get; set; }
        public Guid Publishedproductid { get; set; }
        public Guid Subscriberid { get; set; }

        public virtual PublishedProducts Publishedproduct { get; set; }
        public virtual Subscriber Subscriber { get; set; }
    }
}
