using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceService.Entities
{
    public class MarketplaceDataModel
    {
        public IEnumerable<PublisherDataModel> PublisherCollection { get; set; }

        public bool OperationResult { get; set; }

        public IEnumerable<ServiceTypeDataModel> ServiceTypeCollection { get; set; }


        public IEnumerable<RegionDataModel> RegionCollection { get; set; }

        public IEnumerable<PublishedStatus> PublishedStatusCollection { get; set; }

        public IEnumerable<ChangeHistory> ChangeHistoryCollection { get; set; }

        public IEnumerable<MasterDataConfig> MasterDataConfigCollection { get; set; }

        public IEnumerable<MasterData> MasterDataCollection{get;set;}

        public IEnumerable<MasterDataGeolocation> MasterGeolocationCollection{get;set;}

        public IEnumerable<MasterDataRating> MasterDataRatingCollection { get; set; }
        public IEnumerable<MasterDataServiceType> MasterDataServiceTypeCollection { get; set; }
        public Exception Error { get; set; }
        public IEnumerable<MappingDataConfig> MappingDataConfigCollection { get; set; }
        public IEnumerable<DataMap> MappingDataCollection { get; set; }

        public IEnumerable<MasterDataPackage> MasterDataPackageCollection { get; set; }
        public IEnumerable<PackageTypeDataModel> PackageTypeCollection { get; set; }    }
}
