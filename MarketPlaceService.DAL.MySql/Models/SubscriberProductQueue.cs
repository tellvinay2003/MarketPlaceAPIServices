using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberProductQueue
    {
        public Guid Subscriberproductqueueid { get; set; }
        public Guid Subscriberproductid { get; set; }
        public Guid Marketplaceproductid { get; set; }
        public Guid Subscriberid { get; set; }
        public short Jobstatusid { get; set; }
        public int Messagetypeid { get; set; }
        public short Retrycount { get; set; }
        public string Jobnote { get; set; }
        public DateTime Jobcreationdatetime { get; set; }
        public DateTime? Jobstartdatetime { get; set; }
        public DateTime? Jobenddatetime { get; set; }
        public string Processedby { get; set; }
        public Guid? Traceid { get; set; }
        public short? Jobtypeid { get; set; }

        public virtual JobStatus Jobstatus { get; set; }
        public virtual JobType Jobtype { get; set; }
        public virtual MarketplaceProduct Marketplaceproduct { get; set; }
        public virtual MessageTypes Messagetype { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public virtual SubscriberProduct Subscriberproduct { get; set; }
    }
}
