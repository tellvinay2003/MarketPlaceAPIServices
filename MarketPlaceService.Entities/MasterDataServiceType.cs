using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class MasterDataServiceType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<MasterData> Ratings { get; set; }
        public bool UsePublishedAddress{get;set;}=true;
    }
}
