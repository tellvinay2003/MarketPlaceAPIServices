using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class BookingUpdateFromPublisherQueueHistory
    {
        public Guid Bookingupdatefrompublisherqueuehistoryid { get; set; }
        public Guid? Bookingupdatefrompublisherqueueid { get; set; }
        public int? Publisherbookingid { get; set; }
        public Guid? Publishersiteid { get; set; }
        public short Retrycount { get; set; }
        public short Jobstatusid { get; set; }
        public short? Jobtypeid { get; set; }
        public string Jobnote { get; set; }
        public DateTime Jobcreationdatetime { get; set; }
        public DateTime? Jobstartdatetime { get; set; }
        public DateTime? Jobenddatetime { get; set; }
        public Guid? Traceid { get; set; }
        public Guid? Sitebookingid { get; set; }
        public string Bookingdata { get; set; }

        public virtual JobStatus Jobstatus { get; set; }
        public virtual Site Publishersite { get; set; }
    }
}
