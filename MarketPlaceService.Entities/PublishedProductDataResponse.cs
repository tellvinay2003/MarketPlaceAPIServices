using System;

namespace MarketPlaceService.Entities
{
    public class PublishedProductDataResponse:ResponseBase
    {
        public Guid MarketplaceProductId { get; set; }
        public string Data { get; set; }
    }
}
