using System;

namespace MarketPlaceService.Entities
{
    public class ChangeHistory
    {
        public int DataTypeId { get; set; }
        public HistoryOrigin Origin { get; set; }
        public Guid Site { get; set; }
        public DateTime When {get;set;}
        public string Who { get; set; }
        public HistoryAction Action { get; set; }
        public string Details { get; set; }
        public Guid? UserId { get; set; }
    }
}
