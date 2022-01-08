using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class SubscribeProduct
    {
        public SubscribeProduct()
        {
            SubscriberProductHistory = new HashSet<SubscriberProductHistory>();
        }

        public Guid Subscribeproductid { get; set; }
        public Guid Marketplaceproductid { get; set; }
        public Guid Subscriberid { get; set; }
        public int Productversion { get; set; }
        public string Productdata { get; set; }
        public short Productstatusid { get; set; }
        public DateTime Processedon { get; set; }
        public string Productstatusnote { get; set; }
        public string Processedby { get; set; }
        public DateTime Subscribedon { get; set; }
        public string Subscribedby { get; set; }
        public int Messagetypeid { get; set; }
        public short? Productsubstatusid { get; set; }

        public virtual MarketplaceProduct Marketplaceproduct { get; set; }
        public virtual MessageTypes Messagetype { get; set; }
        public virtual SubscribeProductStatus Productstatus { get; set; }
        public virtual SubscribeProductSubStatus Productsubstatus { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public virtual ICollection<SubscriberProductHistory> SubscriberProductHistory { get; set; }
    }
}
