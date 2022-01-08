using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterDataTypes
    {
        public MasterDataTypes()
        {
            MappingData = new HashSet<MappingData>();
            MasterData = new HashSet<MasterData>();
            MasterDataTypesApplicable = new HashSet<MasterDataTypesApplicable>();
            MessageFields = new HashSet<MessageFields>();
        }

        public int Datatypeid { get; set; }
        public string Datatypename { get; set; }
        public int Masteruiformat { get; set; }
        public int Mappinguiformat { get; set; }

        public virtual DataFormat MappinguiformatNavigation { get; set; }
        public virtual DataFormat MasteruiformatNavigation { get; set; }
        public virtual ICollection<MappingData> MappingData { get; set; }
        public virtual ICollection<MasterData> MasterData { get; set; }
        public virtual ICollection<MasterDataTypesApplicable> MasterDataTypesApplicable { get; set; }
        public virtual ICollection<MessageFields> MessageFields { get; set; }
    }
}
