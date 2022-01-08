using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class SubscriberSupplierDataModel
    {
        public Guid SubscriberSupplierId {get;set;}
        public Guid SubscriberId {get;set;}
        public Guid PublisherId {get;set;}
        public int SupplierId {get;set;}
        public bool Enabled {get;set;}

    }
}
