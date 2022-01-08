using System;

namespace MarketPlaceService.Entities
{
    public class MasterDataRating
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public MasterDataRatingType Ratingtype { get; set; }
    }

    public class MasterDataRatingType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
