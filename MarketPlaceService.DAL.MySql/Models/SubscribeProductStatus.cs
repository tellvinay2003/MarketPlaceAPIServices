using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscribeProductStatus
    {
        public SubscribeProductStatus()
        {
            SubscriberProduct = new HashSet<SubscriberProduct>();
            SubscriberProductHistory = new HashSet<SubscriberProductHistory>();
        }

        public short Productstatusid { get; set; }
        public string Productstatusname { get; set; }

        public virtual ICollection<SubscriberProduct> SubscriberProduct { get; set; }
        public virtual ICollection<SubscriberProductHistory> SubscriberProductHistory { get; set; }
    }
}
