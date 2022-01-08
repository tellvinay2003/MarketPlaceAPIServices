using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class StaticDataType
    {
        public StaticDataType()
        {
            StaticDataUpdateQueue = new HashSet<StaticDataUpdateQueue>();
            StaticDataUpdateQueueHistory = new HashSet<StaticDataUpdateQueueHistory>();
        }

        public short Staticdatatypeid { get; set; }
        public string Staticdatatypename { get; set; }

        public virtual ICollection<StaticDataUpdateQueue> StaticDataUpdateQueue { get; set; }
        public virtual ICollection<StaticDataUpdateQueueHistory> StaticDataUpdateQueueHistory { get; set; }
    }
}
