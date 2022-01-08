using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SiteBookingPushQueue
    {
        public Guid Sitebookingpushqueueid { get; set; }
        public Guid? Marketplacebookingpushqueueid { get; set; }
        public Guid? Publishersiteid { get; set; }
        public Guid? Subscribersiteid { get; set; }
        public int? Publisherbookingid { get; set; }
        public string Bookingdata { get; set; }
        public DateTime? Jobcreateddatetime { get; set; }
        public DateTime? Jobstartdatetime { get; set; }
        public DateTime? Jobenddatetime { get; set; }
        public string Processingnote { get; set; }
        public short? Jobstatusid { get; set; }
        public Guid? Traceid { get; set; }
        public short? Jobtypeid { get; set; }
        public int? Retrycount { get; set; }
        public Guid? Sitebookingid { get; set; }

        public virtual Site Publishersite { get; set; }
        public virtual SiteBooking Sitebooking { get; set; }
        public virtual Site Subscribersite { get; set; }
    }
}
