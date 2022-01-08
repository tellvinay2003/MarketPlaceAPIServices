using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Entities;

namespace MarketPlaceService.DAL.Contract
{
    public interface IMappingDataRatingRepository
    {
        Task<IEnumerable<DataMapResponse>> GetMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId);
        Task<DataMapResponse> GetMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId);
        Task<DataMapResponse> InsertMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId, DataMap data);
        Task<DataMapResponse> UpdateMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId, DataMap data);
        Task<bool> DeleteMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId);
        Guid UserId { get; set; }
    }
}
