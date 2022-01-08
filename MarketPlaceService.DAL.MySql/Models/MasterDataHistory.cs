using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MasterDataHistory
    {
        public Guid Masterdatahistoryid { get; set; }
        public int? Masterdataid { get; set; }
        public string Masterdataname { get; set; }
        public int? Datatypeid { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Username { get; set; }
        public byte? Action { get; set; }
    }
}
