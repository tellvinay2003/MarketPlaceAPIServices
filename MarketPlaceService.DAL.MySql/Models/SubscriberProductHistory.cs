using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscriberProductHistory
    {
        public Guid Subscriberproducthistoryid { get; set; }
        public Guid Subscriberproductid { get; set; }
        public Guid Marketplaceproductid { get; set; }
        public Guid Subscriberid { get; set; }
        public int Productversion { get; set; }
        public string Productdata { get; set; }
        public short Productstatusid { get; set; }
        public DateTime Processedon { get; set; }
        public string Productstatusnote { get; set; }
        public string Processedby { get; set; }
        public DateTime Subscribedon { get; set; }
        public Guid? CreatedBy { get; set; }
        public int Messagetypeid { get; set; }
        public short? Productsubstatusid { get; set; }
        public Guid? Traceid { get; set; }
        public Guid? Subscriberproductqueueid { get; set; }
        public string Productdatadiff { get; set; }

        public virtual MarketplaceProduct Marketplaceproduct { get; set; }
        public virtual MessageTypes Messagetype { get; set; }
        public virtual SubscribeProductStatus Productstatus { get; set; }
        public virtual SubscribeProductSubStatus Productsubstatus { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public virtual SubscriberProduct Subscriberproduct { get; set; }
    }
}
