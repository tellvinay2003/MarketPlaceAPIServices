using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterData
    {
        public MasterData()
        {
            MarketplaceProduct = new HashSet<MarketplaceProduct>();
            MarketplaceProductHistory = new HashSet<MarketplaceProductHistory>();
            MarketplaceProductRating = new HashSet<MarketplaceProductRating>();
            MasterDataLinkHistory = new HashSet<MasterDataLinkHistory>();
            MasterDataLinkMasterdata = new HashSet<MasterDataLink>();
            MasterDataLinkParentmasterdata = new HashSet<MasterDataLink>();
        }

        public int Masterdataid { get; set; }
        public string Masterdataname { get; set; }
        public int Datatypeid { get; set; }
        public bool? Servicetypepublishermainaddress { get; set; }

        public virtual MasterDataTypes Datatype { get; set; }
        public virtual ICollection<MarketplaceProduct> MarketplaceProduct { get; set; }
        public virtual ICollection<MarketplaceProductHistory> MarketplaceProductHistory { get; set; }
        public virtual ICollection<MarketplaceProductRating> MarketplaceProductRating { get; set; }
        public virtual ICollection<MasterDataLinkHistory> MasterDataLinkHistory { get; set; }
        public virtual ICollection<MasterDataLink> MasterDataLinkMasterdata { get; set; }
        public virtual ICollection<MasterDataLink> MasterDataLinkParentmasterdata { get; set; }
    }
}
