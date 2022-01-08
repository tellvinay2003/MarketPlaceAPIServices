using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class BookingPushQueueHistory
    {
        public Guid Bookingpushqueuehistoryid { get; set; }
        public Guid? Marketplacebookingpushqueueid { get; set; }
        public Guid? Publishersiteid { get; set; }
        public Guid? Subscribersiteid { get; set; }
        public int? Bookingid { get; set; }
        public string Bookingdata { get; set; }
        public DateTime? Jobcreateddatetime { get; set; }
        public DateTime? Jobstartdatetime { get; set; }
        public DateTime? Jobenddatetime { get; set; }
        public string Processingnote { get; set; }
        public short? Jobstatusid { get; set; }
        public Guid? Traceid { get; set; }
        public short? Jobtypeid { get; set; }
        public int? Retrycount { get; set; }

        public virtual Site Publishersite { get; set; }
        public virtual Site Subscribersite { get; set; }
    }
}
