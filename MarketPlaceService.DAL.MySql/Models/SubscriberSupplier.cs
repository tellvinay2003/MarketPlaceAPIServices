using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberSupplier
    {
        public Guid SubscriberSupplierId { get; set; }
        public Guid SubscriberId { get; set; }
        public Guid PublisherId { get; set; }
        public int SupplierId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual Subscriber Subscriber { get; set; }
    }
}
