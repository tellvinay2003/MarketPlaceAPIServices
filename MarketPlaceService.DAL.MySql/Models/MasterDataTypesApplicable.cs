using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterDataTypesApplicable
    {
        public int Masterdatatypeapplicableid { get; set; }
        public int Datatypeid { get; set; }
        public int Mappingdirectionid { get; set; }
        public bool Issubscriber { get; set; }
        public bool Ispublisher { get; set; }

        public virtual MasterDataTypes Datatype { get; set; }
        public virtual MappingDirection Mappingdirection { get; set; }
    }
}
