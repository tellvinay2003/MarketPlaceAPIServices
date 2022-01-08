using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscribeProductSubStatus
    {
        public SubscribeProductSubStatus()
        {
            SubscriberProduct = new HashSet<SubscriberProduct>();
            SubscriberProductHistory = new HashSet<SubscriberProductHistory>();
            SubscriberProductQueueHistory = new HashSet<SubscriberProductQueueHistory>();
        }

        public short Productsubstatusid { get; set; }
        public string Productsubstatusname { get; set; }

        public virtual ICollection<SubscriberProduct> SubscriberProduct { get; set; }
        public virtual ICollection<SubscriberProductHistory> SubscriberProductHistory { get; set; }
        public virtual ICollection<SubscriberProductQueueHistory> SubscriberProductQueueHistory { get; set; }
    }
}
