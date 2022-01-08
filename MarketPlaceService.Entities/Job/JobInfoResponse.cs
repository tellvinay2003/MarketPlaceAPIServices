using System;

namespace MarketPlaceService.Entities.Job
{
    public class JobInfoResponse
    {
        public Guid JobId{get;set;}
        public String JobNote{get;set;} 
         public DateTime? JobCreatedDate{get;set;}  
        public DateTime? JobStartDate{get;set;}
        public DateTime? JobEndDate{get;set;}
        public string ProcessedBy{get;set;}
        public int? RetryCount{get;set;}

        public int? ProductId{get;set;}

        public int? BookingId{get;set;}

        public string JobStatus{get;set;}


       

    }
}
