using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberProductCodeServiceType
    {
        public Guid SubscriberProductCodeServiceTypeId { get; set; }
        public Guid SubscriberProductCodeId { get; set; }
        public int ServiceTypeId { get; set; }

        public virtual SubscriberProductCode SubscriberProductCode { get; set; }
    }
}
