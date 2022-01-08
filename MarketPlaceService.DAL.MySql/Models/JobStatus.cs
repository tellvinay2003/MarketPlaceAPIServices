using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class JobStatus
    {
        public JobStatus()
        {
            BookingUpdateFromPublisherQueue = new HashSet<BookingUpdateFromPublisherQueue>();
            BookingUpdateFromPublisherQueueHistory = new HashSet<BookingUpdateFromPublisherQueueHistory>();
            PublishedProductsQueue = new HashSet<PublishedProductsQueue>();
            StaticDataUpdateQueue = new HashSet<StaticDataUpdateQueue>();
            SubscriberProductQueue = new HashSet<SubscriberProductQueue>();
            SubscriberProductTsUpdateQueue = new HashSet<SubscriberProductTsUpdateQueue>();
        }

        public short Jobstatusid { get; set; }
        public string Jobstatusname { get; set; }

        public virtual ICollection<BookingUpdateFromPublisherQueue> BookingUpdateFromPublisherQueue { get; set; }
        public virtual ICollection<BookingUpdateFromPublisherQueueHistory> BookingUpdateFromPublisherQueueHistory { get; set; }
        public virtual ICollection<PublishedProductsQueue> PublishedProductsQueue { get; set; }
        public virtual ICollection<StaticDataUpdateQueue> StaticDataUpdateQueue { get; set; }
        public virtual ICollection<SubscriberProductQueue> SubscriberProductQueue { get; set; }
        public virtual ICollection<SubscriberProductTsUpdateQueue> SubscriberProductTsUpdateQueue { get; set; }
    }
}
