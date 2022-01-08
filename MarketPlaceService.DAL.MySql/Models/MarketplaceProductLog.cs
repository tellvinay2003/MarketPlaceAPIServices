using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MarketplaceProductLog
    {
        public Guid Marketplaceproductlogid { get; set; }
        public Guid Publishedproductid { get; set; }
        public string Productdata { get; set; }
        public DateTime Processedon { get; set; }
        public string Processedby { get; set; }
        public int Servicetypeid { get; set; }
        public int Regionid { get; set; }
        public string Productlongname { get; set; }
        public string Productshortname { get; set; }
        public int Messagetypeid { get; set; }

        public virtual MessageTypes Messagetype { get; set; }
        public virtual PublishedProducts Publishedproduct { get; set; }
        public virtual MasterRegions Region { get; set; }
        public virtual MasterData Servicetype { get; set; }
    }
}
