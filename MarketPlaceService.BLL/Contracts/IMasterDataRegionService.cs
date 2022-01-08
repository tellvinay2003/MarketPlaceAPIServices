using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IMasterDataRegionService
    {
        Task<IEnumerable<MasterDataGeolocation>> GetMasterDataGeolocations(string name);
        Task<MasterDataGeolocation> GetMasterDataGeolocations(int id);
        Task<MasterDataGeolocation> InsertMasterDataGeolocations(int parentId, MasterDataGeolocation data);
        Task<MasterDataGeolocation> UpdateMasterDataGeolocations(int id, MasterDataGeolocation data);
        Task<bool> DeleteMasterDataGeolocations(int id);
        Guid TraceId { get; set; }
        Guid UserId { get; set; }
        Task<bool> CheckIfMappedToImportedProduct(int regionId);
    }
}
