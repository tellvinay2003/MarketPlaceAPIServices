using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class QueuedSubscription
    {
         public Guid JobId {get;set;}
        public Guid SubscribeProductQueueId { get; set; }
        public Guid MarketplaceProductId { get; set; }
        public Guid SubscriberId { get; set; }
        public short JobStatusId { get; set; }
        public int MessageTypeId { get; set; }
        public short RetryCount { get; set; }
        public string JobNote { get; set; }
        public DateTime JobCreationDateTime { get; set; }
        public DateTime? JobStartDateTime { get; set; }
        public DateTime? JobEndDateTime { get; set; }
        public Guid SubscribeProductQueueHistoryid { get; set; }
        public short JobHistoryStatusId { get; set; }
        public Guid SubscriberProductId { get; set; }
        public string ProcessedBy { get; set; }
        public bool InsertSubscriptionHistory { get; set; }
        public List<Error> Errors{get;set;}
        public Guid TraceId {get;set;}
        public short? JobTypeId { get; set; }
    }
}
