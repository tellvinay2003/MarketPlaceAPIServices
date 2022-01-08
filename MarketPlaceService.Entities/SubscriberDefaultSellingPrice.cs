using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class SubscriberDefaultSellingPrice
    {
        public int RuleId {get;set;}
        public Guid SubscriberId {get;set;}
        public int? RegionId {get;set;}
        public List<int> ServiceTypes {get;set;}
        public bool? AllServiceTypesSelected {get;set;}
        public short? Sequence { get; set; }
        public List<SubscriberDefaultSellingPricePolicy> SubscriberDefaultSellingPricePolicy {get;set;}

    }
}
