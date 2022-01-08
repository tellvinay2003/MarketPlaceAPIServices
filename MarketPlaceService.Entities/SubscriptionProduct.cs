using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class SubscriptionProduct
    {
        public Guid SubscribeProductId { get ; set;}
        public Guid MarketPlaceProductId{ get ; set;}
        public Guid SubscriberId { get ; set;}
        public int ProductVersion { get ; set;}
        public string ProductData { get ; set;}
        public short ProductStatusId { get ; set;}
        public DateTime ProcessedOn { get ; set;}
        public string  ProductStatusNote { get ; set;}
        public string ProcessedBy { get ; set;}
        public DateTime  SubscribedOn {get;set;}
        public int  MessageTypeId {get;set;}
        public short  ProductSubStatusId {get;set;}

        public Guid SubscribeProductQueueId {get;set;}

        public short JobStatusId {get;set;}

        public string JobNote {get;set;}

        public Guid? SubscribedBy {get;set;}

        public List<Error> Errors { get; set; }
        public int? TsId { get; set; }
        public bool UpdateProductHistory { get; set; }
        public Guid TraceId {get;set;}
        public string ProductUpdateDifferenceData { get; set; }

    }
}
