using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMasterDataConfigRepository
    {
        Task<IEnumerable<MasterDataConfig>> GetMasterDataConfig();
        Task<MasterDataConfig> GetMasterDataConfig(int id);
    }
}
