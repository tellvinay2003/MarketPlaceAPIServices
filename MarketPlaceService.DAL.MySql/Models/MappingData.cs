using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MappingData
    {
        public MappingData()
        {
            MappingDataLink = new HashSet<MappingDataLink>();
        }

        public Guid Mappingdataid { get; set; }
        public Guid Siteid { get; set; }
        public int Datatypeid { get; set; }
        public int Sourceid { get; set; }
        public int Targetid { get; set; }
        public int Mappingdirectionid { get; set; }
        public string Sourcename { get; set; }
        public string Targetname { get; set; }

        public virtual MasterDataTypes Datatype { get; set; }
        public virtual MappingDirection Mappingdirection { get; set; }
        public virtual Site Site { get; set; }
        public virtual ICollection<MappingDataLink> MappingDataLink { get; set; }
    }
}
