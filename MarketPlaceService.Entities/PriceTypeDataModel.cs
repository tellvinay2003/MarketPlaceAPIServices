using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class PriceTypeDataModel
    {
        public int PriceTypeId  {get;set;}

        public string PriceTypeName {get;set;}

        public bool IsSellPriceType {get;set;}
    }
}
