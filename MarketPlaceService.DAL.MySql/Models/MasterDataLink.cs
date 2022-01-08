using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterDataLink
    {
        public int Masterdatalinkid { get; set; }
        public int Parentmasterdataid { get; set; }
        public int Masterdataid { get; set; }

        public virtual MasterData Masterdata { get; set; }
        public virtual MasterData Parentmasterdata { get; set; }
    }
}
