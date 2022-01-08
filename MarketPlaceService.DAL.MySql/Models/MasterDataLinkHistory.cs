using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterDataLinkHistory
    {
        public Guid Masterdatalinkhistoryid { get; set; }
        public int? Parentmasterdataid { get; set; }
        public int? Masterdataid { get; set; }
        public byte? Action { get; set; }

        public virtual MasterData Masterdata { get; set; }
    }
}
