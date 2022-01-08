using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberChargingPolicy
    {
        public Guid SubscriberChargingPolicyId { get; set; }
        public Guid SubscriberId { get; set; }
        public int ServiceTypeTypeId { get; set; }
        public int? OptionChargingPolicyId { get; set; }
        public int? ExtraChargingPolicyId { get; set; }

        public virtual Subscriber Subscriber { get; set; }
    }
}
