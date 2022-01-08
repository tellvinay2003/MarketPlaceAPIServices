using System;

namespace MarketPlaceService.Entities.Job
{
    public class JobInfoRequest
    {        
        public Guid JobId{get;set;}

        public Guid SiteId{get;set;}

        public BusinessProcess BusinessProcess{get;set;}

        public int ProcessQueueId{get;set;}     

        public short JobTypeId{get;set;}

        public bool CurrentJobsOnly{get;set;}

        public DbJobQueue QueueTable { get; set; }       
        public bool IsHistory { get; set; }



    }
}
