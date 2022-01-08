using System;
using System.Collections.Generic;
namespace MarketPlaceService.Entities.Job
{
    public class JobSearchRequest
    {
        public BusinessProcess BusinessProcess { get; set; }
        public Guid? SiteId { get; set; }
        public int? ProcessQueueId { get; set; } = 0;
        public bool CurrentJobsOnly { get; set; } = true;

        // public JobHistoryStatus? JobHistoryStatus{get;set;}       
        // public JobStatus? JobStatus{get;set;}

        public int? JobStatusId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<Guid> AllowedSites { get; set; }


    }
}
