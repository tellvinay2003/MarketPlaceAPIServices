using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class SupplierStatusDataModel
    {
        public int SupplierStatusId { get; set; }
        public string SupplierStatusName { get; set; }
        public Guid PublisherId {get; set;}
    }
}
