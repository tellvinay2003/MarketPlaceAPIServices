using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class PublishedProductInfo
    {
        public DateTime FirstPublicationDate { get; set; }
        public IEnumerable<HistoryDetail> PublicationHistoryDetails { get; set;}
        public IEnumerable<CurrentSubscriber> CurrentSubscribers { get; set; }
        public string SiteJsonString { get; set;}
        public string MarketplaceJsonString { get; set; }
    }
}
