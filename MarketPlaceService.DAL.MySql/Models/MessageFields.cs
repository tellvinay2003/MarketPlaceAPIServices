using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Models
{
    public partial class MessageFields
    {
        public int Fieldid { get; set; }
        public string Fieldname { get; set; }
        public string Fieldpath { get; set; }
        public int Messagetypeid { get; set; }
        public int Mappingdatatype { get; set; }
        public bool Ismappingmandatory { get; set; }
        public string Removetag { get; set; }

        public virtual MasterDataTypes MappingdatatypeNavigation { get; set; }
        public virtual MessageTypes Messagetype { get; set; }
    }
}
