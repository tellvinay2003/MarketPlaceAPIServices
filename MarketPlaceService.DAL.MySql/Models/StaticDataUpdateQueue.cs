using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class StaticDataUpdateQueue
    {
        public Guid Staticdataupdatequeueid { get; set; }
        public short Staticdatatypeid { get; set; }
        public int Staticdataid { get; set; }
        public string Serviceid { get; set; }
        public Guid Siteid { get; set; }
        public short Jobstatusid { get; set; }
        public string Processingnote { get; set; }
        public short Retrycount { get; set; }
        public DateTime Jobcreationdatetime { get; set; }
        public DateTime? Jobstartdatetime { get; set; }
        public DateTime? Jobenddatetime { get; set; }
        public string Packageid { get; set; }

        public virtual JobStatus Jobstatus { get; set; }
        public virtual Site Site { get; set; }
        public virtual StaticDataType Staticdatatype { get; set; }
    }
}
