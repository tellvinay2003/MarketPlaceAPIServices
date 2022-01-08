using System;

namespace MarketPlaceService.Entities
{
    public class ProductDataRequest
    {
        public Guid PublisherId { get; set; }
        public int ProductId { get; set; }
        public ProductType ProductType { get; set; }
        public Guid PublishedProductId { get; set; }
        public short? JobTypeId { get; set; }
    }
}
