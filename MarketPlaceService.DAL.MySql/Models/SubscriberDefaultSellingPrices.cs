using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberDefaultSellingPrices
    {
        public SubscriberDefaultSellingPrices()
        {
            SubscriberDefaultSellingPricePolicies = new HashSet<SubscriberDefaultSellingPricePolicies>();
            SubscriberDefaultSellingPriceServiceType = new HashSet<SubscriberDefaultSellingPriceServiceType>();
        }

        public int Subscriberdefaultsellingpriceid { get; set; }
        public Guid Subscriberid { get; set; }
        public int? Regionid { get; set; }
        public short? Sequence { get; set; }
        public bool? Allservicetypes { get; set; }

        public virtual Subscriber Subscriber { get; set; }
        public virtual ICollection<SubscriberDefaultSellingPricePolicies> SubscriberDefaultSellingPricePolicies { get; set; }
        public virtual ICollection<SubscriberDefaultSellingPriceServiceType> SubscriberDefaultSellingPriceServiceType { get; set; }
    }
}
