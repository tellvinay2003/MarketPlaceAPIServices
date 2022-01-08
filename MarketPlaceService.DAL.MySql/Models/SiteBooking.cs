using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SiteBooking
    {
        public SiteBooking()
        {
            SiteBookingHistory = new HashSet<SiteBookingHistory>();
            SiteBookingPublisher = new HashSet<SiteBookingPublisher>();
            SiteBookingPushQueue = new HashSet<SiteBookingPushQueue>();
            SiteBookingPushQueueHistory = new HashSet<SiteBookingPushQueueHistory>();
        }

        public Guid Sitebookingid { get; set; }
        public Guid? Marketplacebookingid { get; set; }
        public string PublisherBookingRef { get; set; }
        public Guid? Publisherid { get; set; }
        public Guid? Publishersiteid { get; set; }
        public string Bookingdata { get; set; }
        public int? Bookingversion { get; set; }
        public DateTime? Createdon { get; set; }
        public DateTime? Processedon { get; set; }
        public string Processingnote { get; set; }
        public int? Statusid { get; set; }
        public int? Publisherbookingid { get; set; }
        public string Subscriberbookinginfodiff { get; set; }
        public string Publisherbookinginfo { get; set; }
        public string Publisherbookinginfodiff { get; set; }
        public string Bookingname { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual Site Publishersite { get; set; }
        public virtual ICollection<SiteBookingHistory> SiteBookingHistory { get; set; }
        public virtual ICollection<SiteBookingPublisher> SiteBookingPublisher { get; set; }
        public virtual ICollection<SiteBookingPushQueue> SiteBookingPushQueue { get; set; }
        public virtual ICollection<SiteBookingPushQueueHistory> SiteBookingPushQueueHistory { get; set; }
    }
}
