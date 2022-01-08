using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class JobType
    {
        public JobType()
        {
            PublishedProductsQueue = new HashSet<PublishedProductsQueue>();
            PublishedProductsQueueHistory = new HashSet<PublishedProductsQueueHistory>();
            SubscriberProductQueue = new HashSet<SubscriberProductQueue>();
            SubscriberProductQueueHistory = new HashSet<SubscriberProductQueueHistory>();
        }

        public short Jobtypeid { get; set; }
        public string Jobtypename { get; set; }

        public virtual ICollection<PublishedProductsQueue> PublishedProductsQueue { get; set; }
        public virtual ICollection<PublishedProductsQueueHistory> PublishedProductsQueueHistory { get; set; }
        public virtual ICollection<SubscriberProductQueue> SubscriberProductQueue { get; set; }
        public virtual ICollection<SubscriberProductQueueHistory> SubscriberProductQueueHistory { get; set; }
    }
}
