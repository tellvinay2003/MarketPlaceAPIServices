using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;


namespace MarketPlaceService.BLL.Contracts
{
    public interface IMasterDataService
    {
        Task<IEnumerable<MasterData>> GetMasterDataGeneric(int masterDataTypeId);
        Task<MasterData> GetMasterDataGeneric(int masterDataTypeId, int itemId);
        Task<MasterData> InsertMasterDataGeneric(int masterDataTypeId, MasterData item);
        Task<MasterData> UpdateMasterDataGeneric(int masterDataTypeId, int itemId, MasterData item);
        Task<bool> DeleteMasterDataGeneric(int masterDataTypeId, int itemId);
        Task<bool> CheckIfMappedToImportedProduct(int ratingTypeId);

        Guid TraceId { get; set; }
        Guid UserId { get; set; }
    }
}
