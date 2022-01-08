using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface ICommonService
    {
        Task<Tuple<bool, string>> CanOrganisationBeUsed(int organisationId, EntityType entityType, Guid entityId, Guid siteId);
        
        Task<bool> DuplicateMasterDataNameExists(int dataTypeId, string dataTypeName, int dataId, string dataName, int ratingTypeId);
        string GetJsonDifference(string originalJson, string changedJson, JsonType type);
    }
}
