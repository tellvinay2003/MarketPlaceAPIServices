using System;
using System.Collections.Generic;

namespace MarketPlaceService.Entities
{
    public class PublisherDefaultDataModel
    {
        public List<int> SupplierStatuses { get; set; }
        public List<int> ServiceStatuses { get; set; }
        public List<int> PackageStatuses { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? PackagePriceStartDate {get;set;}
    }
}
