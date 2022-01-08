using System;

namespace MarketPlaceService.Entities
{
    public class RegionData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public int? ContainRegionid { get; set; }
    }
}
