using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMasterDataRepository
    {
        Task<IEnumerable<MasterData>> GetMasterDataGeneric(int masterdatatypeid);
        Task<MasterData> GetMasterDataGeneric(int masterDataTypeId, int itemId);

        Task<MasterData> InsertMasterDataGeneric(int masterDataTypeId, MasterData item);
        Task<MasterData> UpdateMasterDataGeneric(int masterDataTypeId, int itemId, MasterData item);

        Task<bool> DeleteMasterDataGeneric(int masterDataTypeId, int itemId);
        Task<bool> CheckIfMappedToImportedProduct(int ratingTypeId);
        Guid UserId { get; set; }

    }
}
