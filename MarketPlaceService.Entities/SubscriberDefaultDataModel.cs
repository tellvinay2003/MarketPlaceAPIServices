using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class SubscriberDefaultDataModel
    {
        public Guid SubscriberDefaultID {get;set;}
        public Guid SubscriberId {get;set;}	 
        public int? SeasonTypeID {get;set;}	
        public int? BuyPriceTypeID {get;set;}
        public int? BuyBookingTypeID {get;set;}
        public int? StartDateOffsetDays { get; set; }
        public int? EndDateOffsetDays { get; set; }
    }
}
