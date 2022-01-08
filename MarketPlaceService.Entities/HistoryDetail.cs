using System;

namespace MarketPlaceService.Entities
{
    public class HistoryDetail
    {
        public DateTime Date { get; set; }
        public string Event { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public short? SubStatusId { get; set; }
        public string SubStatusName { get; set; }
        public Guid? TraceId { get; set; }
        public Guid HistoryId { get; set; }
        public bool JsonExists { get; set; }
    }
}
