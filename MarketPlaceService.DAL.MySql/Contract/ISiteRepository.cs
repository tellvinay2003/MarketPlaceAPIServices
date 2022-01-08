using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using MarketPlaceService.Entities;
using MarketPlaceService.DAL.MySql.Models;
using System.Threading.Tasks;

namespace MarketPlaceService.DAL.Contract
{
    public interface ISiteRepository
    {
           Task<SiteDataModel> AddNewSite(SiteDataModel siteData);

           Task<SiteDataModel> GetSiteById(Guid id);

           Task<SiteDataModel> DeleteSiteById(Guid id);

           Task<SiteDataModel> UpdateSite(SiteDataModel siteData);

           Task<IEnumerable<SiteDataModel>> GetRegisteredSitesAsync();

           Task<IEnumerable<SiteDataModel>> GetEnableSites();
    }
}
