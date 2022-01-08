using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class MarketPlaceProductsSearch
    {
        public Guid SubscriberId {get;set;}

        public Guid PublisherId {get;set;}

        public int ProductTypeId {get;set;}

        public int RegionId {get;set;}

        public string ProductLongName {get;set;}

        public string ProductShortName {get;set;}

        public List<int> Ratings {get;set;}

        public int SubscriptionStatusId {get;set;}
    }
}
