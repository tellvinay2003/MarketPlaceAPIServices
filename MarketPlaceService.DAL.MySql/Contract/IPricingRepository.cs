using MarketPlaceService.Entities;
using MarketPlaceService.Entities.TSv2ApiEntities;
using System;
using System.Collections.Generic;

namespace MarketPlaceService.DAL.Contract
{
    public interface IPricingRepository
    {
        List<SiteServiceData> GetSiteServiceData(List<Guid> marketplaceProductIds, Guid subscriberId);

        Guid GetSubscriberIdBySiteAndOrganisation(Guid siteId, int organisationId);
        bool CheckIfHasAccess(Guid marketplaceProductId, Guid subscriberId);
        List<SubscriberDataModel> GetSubscribersBySite(Guid siteId);
    }
}
