using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class ProcessJSONResponse : ResponseBase
    {
        public string JsonString { get; set; } 
        public MappingErrorType MappingError { get; set; }
        public List<string> Description {get; set;}
    }

   
}
