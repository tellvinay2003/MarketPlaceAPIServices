using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IMappingDataConfigService
    {
        Task<IEnumerable<MappingDataConfig>> GetMappingDataConfig(Entities.MappingDirection direction, Guid site);
        Task<MappingDataConfig> GetMappingDataConfig(Entities.MappingDirection direction, ushort dataTypeId, Guid site);
        Guid TraceId { get; set; }
    }
}
