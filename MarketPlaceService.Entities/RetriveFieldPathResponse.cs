using System;
using System.Linq;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class RetriveFieldPathResponse
    {
     public string FieldPath { get; set; }

     public List<string> Value {get; set;}
            
    }
}
