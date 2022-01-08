using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MappingDataLink
    {
        public Guid Mappingdatalinkid { get; set; }
        public int Parentdataid { get; set; }
        public Guid Mappingdataid { get; set; }

        public virtual MappingData Mappingdata { get; set; }
    }
}
