using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class ChangeHistory
    {
        public Guid Masterdatahistoryid { get; set; }
        public int? Datatypeid { get; set; }
        public byte? Origin { get; set; }
        public Guid? Siteid { get; set; }
        public DateTime? Modifieddate { get; set; }
        public Guid? Modifiedby { get; set; }
        public byte? Action { get; set; }
        public string Details { get; set; }
    }
}
