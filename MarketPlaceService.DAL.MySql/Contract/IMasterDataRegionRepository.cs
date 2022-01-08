using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMasterDataRegionRepository
    {
        Task<IEnumerable<MasterDataGeolocation>> GetMasterDataGeolocations(string name);
        Task<MasterDataGeolocation> GetMasterDataGeolocations(int id);
        Task<MasterDataGeolocation> InsertMasterDataGeolocations(int? parentId, MasterDataGeolocation data);
        Task<MasterDataGeolocation> UpdateMasterDataGeolocations(int id, MasterDataGeolocation data);
        Task<bool> DeleteMasterDataGeolocations(int id);
        Guid UserId { get; set; }
        Task<bool> CheckIfMappedToImportedProduct(int regionId);
    }
}
