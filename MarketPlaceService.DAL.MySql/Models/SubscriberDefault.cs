using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberDefault
    {
        public Guid SubscriberDefaultId { get; set; }
        public Guid SubscriberId { get; set; }
        public int? CommunicationTypeId { get; set; }
        public int? SeasonTypeId { get; set; }
        public int? BuyPriceTypeId { get; set; }
        public int? BuyBookingTypeId { get; set; }
        public int? Startdateoffsetdays { get; set; }
        public int? Enddateoffsetdays { get; set; }

        public virtual Subscriber Subscriber { get; set; }
    }
}
