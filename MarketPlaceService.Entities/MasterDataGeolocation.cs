using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class MasterDataGeolocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Level { get; set; }
        public int? ParentId { get; set; }
        public List<MasterDataGeolocation> ChildRegions { get; set; }
        
    }
}
