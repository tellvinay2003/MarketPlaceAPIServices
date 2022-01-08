using MarketPlaceService.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlaceService.BLL.Contracts
{
    public interface ISiteService
    {
        Guid TraceId { get;  set; }
        Task<SiteDataModel> GetSiteById(Guid id);

        Task<SiteDataModel> AddNewSite(SiteDataModel siteData);

        Task<IEnumerable<SiteDataModel>> GetRegisteredSitesAsync();

        Task<SiteDataModel> DeleteSiteById(Guid id);

        Task<SiteDataModel> UpdateSite(SiteDataModel siteData);

        Task<bool> ValidateSite(string url);

        Task<IEnumerable<SiteDataModel>> GetEnableSites();
    }
}
