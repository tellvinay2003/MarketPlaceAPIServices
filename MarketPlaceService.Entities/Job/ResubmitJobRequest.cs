using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities.Job
{
    public class ResubmitJobRequest
    {
        public DbJobQueue QueueTable { get; set; }
        public Guid SiteId { get; set; }
        public string JobId { get; set; }
        public bool IsHistory { get; set; }
    }
}
