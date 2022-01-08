using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class QueuedPublication
    {
        public Guid JobId {get;set;}
        public Guid PublishedProductId { get; set; }
        public  int ProductId {get;set;}
        public Guid PublisherId {get;set;}
        public int RetryCount {get;set;}   
        public short JobStatusId{get;set;}        
        public DateTime? JobStartTime{get;set;}
        public DateTime? JobEndTime { get; set; }
        public string ProcessingNote { get; set; }
        public string ProcessedBy {get;set;}
        public bool IsProcessing { get; set; }
        public int PublishStatus { get; set; }        
        public bool InsertPublishHistory { get; set; }
        public List<Error> Errors { get; set; }
        public Guid TraceId {get;set;}
        public short? JobTypeId { get; set; }
        public Guid SiteId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
