using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.BLL
{
    public interface IMappingDataRatingService
    {
        Task<IEnumerable<DataMapResponse>> GetMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId);
        Task<DataMapResponse> GetMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId);
        Task<DataMapResponse> InsertMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId, DataMap data);
        Task<DataMapResponse> UpdateMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId, DataMap data);
        Task<bool> DeleteMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId);
        Guid TraceId { get; set; }
        Guid UserId { get; set; }
    }
}
