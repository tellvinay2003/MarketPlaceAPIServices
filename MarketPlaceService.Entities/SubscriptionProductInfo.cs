using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class SubscriptionProductInfo
    {
        public string PublishedBy { get; set; }
        public DateTime FirstSubscriptionDate { get; set; }
        public IEnumerable<HistoryDetail> SubscriptionHistoryDetails { get; set; }
        public string SubscriptionJsonString { get; set;}
        public string MarketplaceJsonString { get; set; }
    }
}
