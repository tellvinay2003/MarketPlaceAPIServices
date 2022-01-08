using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class ProcessJSONRequest
    {
        public int MessageTypeId { get; set; }
        public int MappingDirection { get; set; } 
        public Guid SiteId { get; set; }
        public string JsonString { get; set; } 
        public SubscriberDefaultsModelForSubscription subscriberDefaults { get; set; }
        public List<RegionData> AllRegions { get; set; }     
        public short? ProductTypeId { get; set; } 
    }
}
