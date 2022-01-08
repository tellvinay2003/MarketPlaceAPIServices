using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class SubscriberProductTsUpdateQueue
    {
       public Guid SubscriberProductTsUpdateQueueId {get;set;}
       public Guid MarketPlaceProductId{get;set;}       
       public Guid SubscriberId {get;set;}
       public short JobStatusId {get;set;}
       public int MessageTypeId {get;set;}
       public short RetryCount {get;set;}
       public string JobNote {get;set;}
       public DateTime JobCreationDateTime {get;set;}
       public DateTime JobStartDateTime {get;set;}
       public DateTime JobEndDateTime {get;set;}       
       public int? TsId {get;set;} //PublisherBookingId
       public List<Error> Errors {get;set;}
       public Guid SubscriberProductId { get; set; }
       public Guid TraceId {get;set;}

       public short CallBackJobTypeId {get;set;}
       public Guid SiteBookingId { get; set; }
    }
}
