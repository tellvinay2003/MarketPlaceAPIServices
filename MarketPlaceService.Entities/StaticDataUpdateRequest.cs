using System;

namespace MarketPlaceService.Entities
{
    public class StaticDataUpdateQueueRequest
    {
        public int ProductId { get; set; }
        public short JobTypeId { get; set; }
        public string ServiceId { get; set; }
        public Guid SiteId { get; set;}
        public string PackageId { get; set; }
    }

    public enum StaticDataType{
        ServiceTypeOption,
        ServiceTypeExtra,
        ServiceTypeFacility
    }
}
