using System;

namespace MarketPlaceService.Entities
{
    public class StaticDataUpdateQueue
    {
        public Guid StaticDataUpdateQueueId { get; set; }
        public int StaticDataId { get; set; }
        public short StaticDataTypeId { get; set; }
        public string ServiceIds { get; set; }
        public Guid SiteId { get; set; }
        public short JobStatusId { get; set; }
        public string ProcessingNote { get; set; }
        public short RetryCount { get; set; }
        public DateTime JobCreationTime { get; set; }
        public DateTime JobStartTime { get; set; }
        public DateTime JobEndTime { get; set; }
    }
}
