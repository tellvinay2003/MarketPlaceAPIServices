using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberDefaultSellingPricePolicies
    {
        public int Subscriberdefaultsellingpricepolicyid { get; set; }
        public int Subscriberdefaultsellingpriceid { get; set; }
        public int Pricetypeid { get; set; }
        public int Bookingtypeid { get; set; }
        public int? Taxid { get; set; }

        public virtual SubscriberDefaultSellingPrices Subscriberdefaultsellingprice { get; set; }
    }
}
