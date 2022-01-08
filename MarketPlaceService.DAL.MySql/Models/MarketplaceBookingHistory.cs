using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MarketplaceBookingHistory
    {
        public Guid Marketplacebookinghistoryid { get; set; }
        public Guid Marketplacebookingid { get; set; }
        public string SubscriberBookingRef { get; set; }
        public int? SubscriberBookingId { get; set; }
        public Guid? Subscriberid { get; set; }
        public Guid? Subscribersiteid { get; set; }
        public string Bookingdata { get; set; }
        public int? Bookingversion { get; set; }
        public DateTime? Createdon { get; set; }
        public DateTime? Processedon { get; set; }
        public string Processingnote { get; set; }
        public string Bookingdatadiff { get; set; }
        public int? Statusid { get; set; }
        public string Bookingname { get; set; }

        public virtual MarketplaceBooking Marketplacebooking { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public virtual Site Subscribersite { get; set; }
    }
}
