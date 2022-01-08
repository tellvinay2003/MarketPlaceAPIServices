using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterRegions
    {
        public MasterRegions()
        {
            MarketplaceProduct = new HashSet<MarketplaceProduct>();
            MarketplaceProductHistory = new HashSet<MarketplaceProductHistory>();
        }

        public int Regionid { get; set; }
        public string Regionname { get; set; }
        public short Level { get; set; }
        public int? Parentregionid { get; set; }

        public virtual ICollection<MarketplaceProduct> MarketplaceProduct { get; set; }
        public virtual ICollection<MarketplaceProductHistory> MarketplaceProductHistory { get; set; }
    }
}
