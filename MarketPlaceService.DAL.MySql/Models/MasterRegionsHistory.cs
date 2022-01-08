using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterRegionsHistory
    {
        public Guid Masterregionshistoryid { get; set; }
        public int? Regionid { get; set; }
        public string Regionname { get; set; }
        public short? Level { get; set; }
        public int? Parentregionid { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Username { get; set; }
        public byte? Action { get; set; }
    }
}
