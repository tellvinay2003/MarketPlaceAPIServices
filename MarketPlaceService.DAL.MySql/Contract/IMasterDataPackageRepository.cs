using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMasterDataPackageRepository
    {
        Task<IEnumerable<MasterDataPackage>> GetMasterDataPackage(int id);
        Task<MasterDataPackage> InsertMasterDataPackage(int masterDataTypeId,MasterDataPackage data);
        Task<MasterDataPackage> UpdateMasterDataPackage(int masterDataTypeId,int id, MasterDataPackage data);
        Guid UserId { get; set; }

        Task<bool> DeleteMasterDataPackage(int masterDataTypeId,int id);

    }
}
