using System;

namespace MarketPlaceService.Entities.Job
{
    public class JobRecord
    {
        public Guid JobId{get;set;}

        public Guid SiteId{get;set;}

        public string SiteName{get;set;}

        public int ProcessQueueId{get;set;}

        public string ProcessQueueName{get;set;}

        public short DbJobTypeId{get;set;}


        public short JobTypeId{get;set;}
        public string JobTypeName{get;set;}

        public DateTime Created{get;set;}

         public DateTime? Started{get;set;}

         public double Duration {get;set;}

         public int StatusId{get;set;}
         
         public string StatusName{get;set;}

         public DbJobQueue QueueTable {get;set;}

         public bool IsHistory{get;set;}

         


         






    }
}
