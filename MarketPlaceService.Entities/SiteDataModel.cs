using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace MarketPlaceService.Entities
{
    public class SiteDataModel
    {
        public Guid SiteId { get; set; }
        public string SiteName { get; set; }
        public string Url { get; set; }

        public bool Enabled { get; set; }

        public string Message {get;set;}

        //[IgnoreDataMember]    
        public bool responseMessage{get;set;}

        public bool IsValid {get;set;}
    }
}
