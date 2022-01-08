using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MarketplaceBooking
    {
        public MarketplaceBooking()
        {
            MarketplaceBookingHistory = new HashSet<MarketplaceBookingHistory>();
            SiteBookingHistory = new HashSet<SiteBookingHistory>();
        }

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
        public int? Statusid { get; set; }
        public string Bookingname { get; set; }

        public virtual Subscriber Subscriber { get; set; }
        public virtual Site Subscribersite { get; set; }
        public virtual ICollection<MarketplaceBookingHistory> MarketplaceBookingHistory { get; set; }
        public virtual ICollection<SiteBookingHistory> SiteBookingHistory { get; set; }
    }
}
