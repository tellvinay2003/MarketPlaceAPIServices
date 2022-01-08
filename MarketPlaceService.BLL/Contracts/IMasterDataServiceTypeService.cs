using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IMasterDataServiceTypeService
    {
         Task<IEnumerable<MasterDataServiceType>> GetMasterDataServiceTypes();
        Task<MasterDataServiceType> GetMasterDataServiceType(int id);
        Task<MasterDataServiceType> InsertMasterDataServiceType(MasterDataServiceType data);
        Task<MasterDataServiceType> UpdateMasterDataServiceType(int id, MasterDataServiceType data);
        Task<bool> DeleteMasterDataServiceType(int id);
        Task<bool> CheckIfMappedToImportedProduct(int id);
        Guid TraceId { get; set; }
        Guid UserId { get; set; }
    }
}
