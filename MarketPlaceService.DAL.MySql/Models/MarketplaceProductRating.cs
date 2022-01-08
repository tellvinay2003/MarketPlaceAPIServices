using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MarketplaceProductRating
    {
        public Guid Marketplaceproductratingid { get; set; }
        public Guid Marketplaceproductid { get; set; }
        public int Ratingid { get; set; }

        public virtual MarketplaceProduct Marketplaceproduct { get; set; }
        public virtual MasterData Rating { get; set; }
    }
}
