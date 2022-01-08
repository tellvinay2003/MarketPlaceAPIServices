using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class ManageSubscribersResponse
    {
       public Guid MarketPlaceProductId {get;set;}
        public Guid PublishedProductId {get;set;}
        public int ServiceTypeId {get;set;}
        public int RegionId {get;set;}        
        public string ProductLongName {get;set;}
        public string ProductShortName {get;set;}
        public string RegionName {get;set;}  
        public Guid? TraceId { get; set; }
        public int MessageTypeId { get; set; }
        public List<ManageSubscriberDetails> Subscribers{get;set;}
        public short ProductTypeId{get;set;}
    }
}
