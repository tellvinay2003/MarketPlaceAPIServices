using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MappingDirection
    {
        public MappingDirection()
        {
            MappingData = new HashSet<MappingData>();
            MasterDataTypesApplicable = new HashSet<MasterDataTypesApplicable>();
        }

        public int Mappingdirectionid { get; set; }
        public string Mappingdirectionname { get; set; }

        public virtual ICollection<MappingData> MappingData { get; set; }
        public virtual ICollection<MasterDataTypesApplicable> MasterDataTypesApplicable { get; set; }
    }
}
