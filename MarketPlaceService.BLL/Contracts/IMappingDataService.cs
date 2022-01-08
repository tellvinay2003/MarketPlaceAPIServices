using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL.Contracts
{
    public interface IMappingDataService
    {
        Task<IEnumerable<DataMapResponse>> GetMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId);
        Task<DataMapResponse> GetMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId);
        Task<DataMapResponse> InsertMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId, DataMap request);
        Task<DataMapResponse> UpdateMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId, DataMap request);
        Task<bool> DeleteMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId);
        Guid TraceId { get; set; }
        Guid UserId { get; set; }
    }
}
