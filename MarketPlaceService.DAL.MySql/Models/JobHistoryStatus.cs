using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class JobHistoryStatus
    {
        public JobHistoryStatus()
        {
            PublishedProductsQueueHistory = new HashSet<PublishedProductsQueueHistory>();
            StaticDataUpdateQueueHistory = new HashSet<StaticDataUpdateQueueHistory>();
            SubscriberProductQueueHistory = new HashSet<SubscriberProductQueueHistory>();
            SubscriberProductTsUpdateQueueHistory = new HashSet<SubscriberProductTsUpdateQueueHistory>();
        }

        public short Jobstatusid { get; set; }
        public string Jobstatusname { get; set; }

        public virtual ICollection<PublishedProductsQueueHistory> PublishedProductsQueueHistory { get; set; }
        public virtual ICollection<StaticDataUpdateQueueHistory> StaticDataUpdateQueueHistory { get; set; }
        public virtual ICollection<SubscriberProductQueueHistory> SubscriberProductQueueHistory { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueueHistory> SubscriberProductTsUpdateQueueHistory { get; set; }
    }
}
