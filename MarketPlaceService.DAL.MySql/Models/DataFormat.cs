using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class DataFormat
    {
        public DataFormat()
        {
            MasterDataTypesMappinguiformatNavigation = new HashSet<MasterDataTypes>();
            MasterDataTypesMasteruiformatNavigation = new HashSet<MasterDataTypes>();
        }

        public int Formatid { get; set; }
        public string Formatname { get; set; }

        public virtual ICollection<MasterDataTypes> MasterDataTypesMappinguiformatNavigation { get; set; }
        public virtual ICollection<MasterDataTypes> MasterDataTypesMasteruiformatNavigation { get; set; }
    }
}
