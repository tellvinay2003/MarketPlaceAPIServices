using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IMasterDataConfigService
    {
        Task<IEnumerable<MasterDataConfig>> GetMasterDataConfig();
        Task<MasterDataConfig> GetMasterDataConfig(int id);

        Guid TraceId { get; set; }
    }
}
