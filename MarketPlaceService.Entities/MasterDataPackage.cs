using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class MasterDataPackage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MasterData ServiceLink { get; set; }
    }
}
