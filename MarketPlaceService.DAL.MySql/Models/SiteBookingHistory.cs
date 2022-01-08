using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SiteBookingHistory
    {
        public Guid Sitebookinghistoryid { get; set; }
        public Guid Sitebookingid { get; set; }
        public Guid? Marketplacebookingpushqueueid { get; set; }
        public string PublisherBookingRef { get; set; }
        public Guid? Publisherid { get; set; }
        public Guid? Publishersiteid { get; set; }
        public string Bookingdata { get; set; }
        public int? Bookingversion { get; set; }
        public DateTime? Createdon { get; set; }
        public DateTime? Processedon { get; set; }
        public string Processingnote { get; set; }
        public int? Statusid { get; set; }
        public string Bookingdatadiff { get; set; }
        public Guid Marketplacebookingid { get; set; }
        public string Subscriberbookinginfodiff { get; set; }
        public string Publisherbookinginfo { get; set; }
        public string Publisherbookinginfodiff { get; set; }
        public string Bookingname { get; set; }
        public int? Publisherbookingid { get; set; }

        public virtual MarketplaceBooking Marketplacebooking { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Site Publishersite { get; set; }
        public virtual SiteBooking Sitebooking { get; set; }
    }
}
