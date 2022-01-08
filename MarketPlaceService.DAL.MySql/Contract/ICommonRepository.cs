using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace MarketPlaceService.DAL.Contract
{
    public interface ICommonRepository
    {
        Task<string> GetSiteUrlByPublisherId(Guid publisherId);
        Task<string> GetSiteUrl(Guid entityId, EntityType entityType);
        Task<string> GetSiteUrlBySiteId(Guid publisherId);
        Task<string> GetSiteUrlBySubscriberId(Guid siteId);
        Task<int> GetMappingDirectionId(Entities.MappingDirection direction);
        Task<int> GetDataTypeIdFromName(string datatype);
        Task<HistoryOrigin> GetHistoryOriginFromMappingDirection(Entities.MappingDirection direction);

        Task<int> GetMessageTypeId(Guid MarketPlaceProductId);
        Error GetErrorDetails(Error error);
        Task<List<int>> GetOrganisationIdsUsedForSite(EntityType entityType, Guid entityId, Guid siteId);
        Task<bool> DuplicateMasterDataNameExists(int dataTypeId, string dataTypeName,int dataId, string dataName, int ratingTypeId);
        Task<short> GetProductTypeId(Guid marketplaceProductId);
    }
}
