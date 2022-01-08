using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class SiteServiceData
    {
        public Guid MarketplaceProductId { get; set; }
        public Guid SiteId { get; set; }
        public int MPServiceId { get; set; }
        public int TSServiceId { get; set; }
        public int AgentId { get; set; }
        public int? BookingPrefixId { get; set; }
        public int? BookingOwnerId { get; set; }
        public Guid PublisherId { get; set; }
    }
}
