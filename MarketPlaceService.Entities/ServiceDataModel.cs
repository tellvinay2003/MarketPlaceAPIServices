using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    // Vinay
     public class ServiceDataModel
    {
        public int Serviceid { get; set; }
        public string Servicelongname { get; set; }
        public string Serviceshortname { get; set; }
        public string Servicedescription { get; set; }
        public int ServiceTypeId { get; set; }
        public string RegionName { get; set; }
        public string ServiceTypeName { get; set; }
        public string ServiceStatusName { get; set; }
        public string SupplierName { get; set; }
    }
}
