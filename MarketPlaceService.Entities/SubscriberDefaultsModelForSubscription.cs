using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class SubscriberDefaultsModelForSubscription
    {
        public SubscriberDefaultDataModel Defaults { get; set; }
        public IEnumerable<SubscriberDefaultsProductCode> ProductCodeRules { get; set; }
        public IEnumerable<SubscriberSupplierDataModel> Suppliers { get; set; }
        public SubscriberChargingPolicyDataModel ChargingPolicy { get; set; }
        public IEnumerable<SubscriberDefaultSellingPrice> SellingPrices { get; set; }
    }
}
