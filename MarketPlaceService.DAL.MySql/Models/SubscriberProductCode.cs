using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberProductCode
    {
        public SubscriberProductCode()
        {
            SubscriberProductCodeServiceType = new HashSet<SubscriberProductCodeServiceType>();
        }

        public Guid SubscriberProductCodeId { get; set; }
        public Guid SubscriberId { get; set; }
        public int? RegionId { get; set; }
        public int? ProductCodeId { get; set; }
        public bool? ApplytoOptions { get; set; }
        public bool? ApplytoExtras { get; set; }
        public bool? Allservicetypes { get; set; }

        public virtual Subscriber Subscriber { get; set; }
        public virtual ICollection<SubscriberProductCodeServiceType> SubscriberProductCodeServiceType { get; set; }
    }
}
