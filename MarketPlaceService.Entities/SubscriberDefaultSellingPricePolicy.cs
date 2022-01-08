using System;

namespace MarketPlaceService.Entities
{
    public class SubscriberDefaultSellingPricePolicy
    {
        public int SubscriberDefaultSellingPricePolicyId {get;set;}
        public int PriceTypeId {get;set;}
        public int BookingTypeId {get;set;}
        public int? TaxId {get;set;}
    }
}
