using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class SubscriberDefaultsProductCode
    {
        public Guid RuleId { get; set; }
        public Guid SubscriberId { get; set; }
        public int? Region { get; set; }
        public List<int> ServiceTypes { get; set; }
        public int? ProductCodeId { get; set; }
        public bool ApplyToOptions { get; set; }
        public bool ApplyToExtras { get; set; }        
        public bool? AllServiceTypesSelected {get;set;}
    }
}
