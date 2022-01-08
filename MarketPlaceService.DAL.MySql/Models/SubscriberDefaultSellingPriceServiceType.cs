using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberDefaultSellingPriceServiceType
    {
        public int Subscriberdefaultsellingpriceservicetypeid { get; set; }
        public int Subscriberdefaultsellingpriceid { get; set; }
        public int Servicetypeid { get; set; }

        public virtual SubscriberDefaultSellingPrices Subscriberdefaultsellingprice { get; set; }
    }
}
