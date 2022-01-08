using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class MarketPlaceProduct
    {
        public Guid MarketPlaceProductId {get;set;}

        public Guid PublishedProductId {get;set;}

        public string ProductData {get;set;}

        public DateTime  ProcessedOn {get;set;}

        public string ProcessedBy {get;set;}

        public int ServiceTypeId {get;set;}

        public int RegionId {get;set;}
        
        public string ProductLongName {get;set;}

        public string ProductShortName {get;set;}

        public string RegionName {get;set;}

        [JsonIgnore]
        public Guid SubscriberProductId { get; set; }

        public bool IsSubscribed { get; set; }
        public int MessageTypeId { get; set; }
        public short ProcessingStatusId { get; set; }
        public string ProcessingStatusName { get; set; }

        public bool IsSubscribable { get; set; }
        public string UnSubscribableReason { get; set; }
        public bool ProductDisabled { get; set; }

        public string ErrorMessage { get; set; }

        public short? ProductSubStatusId { get; set; }
        public string ProductSubStatusName { get; set; }
        public Guid? TraceId { get; set; }
    }
}
