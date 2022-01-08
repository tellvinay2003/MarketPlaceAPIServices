using System;

namespace MarketPlaceService.Entities
{
    public class PackageDataModel
    {
        public int PackageId { get; set; }
        public string PackageLongname { get; set; }
        public string PackageShortname { get; set; }      
        public int PackageTypeId { get; set; }
        public int PackageStatusId { get; set; }
        public string RegionName { get; set; }
        public string PackageTypeName { get; set; }
        public string PackageStatusName { get; set; }
        public int? OrganisationId { get; set; }
    }
}
