using MarketPlaceService.DAL.Contract;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarketPlaceService.Entities;
using System.Linq;
using MarketPlaceService.DAL.Models;

namespace MarketPlaceService.DAL
{
    public class SiteRepository : BaseRepository, ISiteRepository, IHealthCheck
    {

        public SiteRepository(MarketplaceDbContext context) : base(context)
        {

        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public async Task<SiteDataModel> AddNewSite(SiteDataModel siteData)
        {          
          var siteItem = new Site
          {
              //SiteId= Guid.NewGuid(),
              SiteName = siteData.SiteName,
              Url = siteData.Url,
              Enabled = true,
          };

          _context.Site.Add(siteItem);              
          _context.SaveChanges();
          
          siteData.SiteId = siteItem.SiteId;
          siteData.Enabled = siteItem.Enabled;
          siteData.IsValid=true;
                
          return await Task.FromResult(siteData);
        }

        public async Task<SiteDataModel> GetSiteById(Guid id)
        {         
           var SiteModel=(from s in _context.Site where s.SiteId==id select new SiteDataModel{
                        SiteId=s.SiteId,
                        SiteName=s.SiteName,
                        Url=s.Url,
                        Enabled=s.Enabled
                    }).FirstOrDefault();
            return await Task.FromResult(SiteModel);
        }

        public async Task<SiteDataModel> DeleteSiteById(Guid id)
        {
           SiteDataModel response=new SiteDataModel();
            var  DeleteSite=_context.Site.Where(s=>s.SiteId==id).FirstOrDefault();
            if(DeleteSite == null)
              return null;

            
              if(DeleteSite.Enabled==true)
              {   
              var checkPublisher=_context.Publisher.Where(s=>s.SiteId==id && s.Enabled==true).Select(a=>a).FirstOrDefault();
              if(checkPublisher==null)
              {
                var checksubscriber=_context.Subscriber.Where(s=>s.SiteId==id && s.Enabled==true).Select(a=>a).FirstOrDefault();
                if(checksubscriber==null)
                {
                    DeleteSite.Enabled=false;
                    response.Message="Site has been disabled successfully";
                    response.IsValid=true;
                }
                else
                {
                    response.Message="This site cannot be disabled as at least one enabled Publisher & Subscriber exists";
                    response.IsValid=false;
                }
              }
              else
              {
                  response.Message="This site cannot be disabled as at least one enabled Publisher & Subscriber exists";
                    response.IsValid=false;
              }
              }
            else
            {
              DeleteSite.Enabled=true;
              response.Message="Site has been enable successfully";
              response.IsValid=true;
            }
              var test= _context.SaveChanges();              

            return await Task.FromResult(response);
        }

        public async Task<SiteDataModel> UpdateSite(SiteDataModel siteData)
        {
            SiteDataModel response=new SiteDataModel();
            
            var UpdateSite=_context.Site.Where(s=>s.SiteId==siteData.SiteId).FirstOrDefault();

            if(UpdateSite == null)
              return null;
                       
            UpdateSite.SiteName=siteData.SiteName;
            UpdateSite.Url=siteData.Url;
            UpdateSite.Enabled=siteData.Enabled;
            
            _context.Site.Update(UpdateSite);
            _context.SaveChanges(); 

            siteData.IsValid = true;

            return await Task.FromResult(siteData);
        }

        public async Task<IEnumerable<SiteDataModel>> GetRegisteredSitesAsync()
        {
           IList<SiteDataModel> GetSiteList=(from s in _context.Site 
           //where s.Enabled==true
           orderby s.SiteName
            select new SiteDataModel{
              SiteId=s.SiteId,
              SiteName=s.SiteName,
              Url=s.Url,
              Enabled=s.Enabled,
            }).ToList();
            return await Task.FromResult(GetSiteList);
        }

        public async Task<IEnumerable<SiteDataModel>> GetEnableSites()
        {
           IList<SiteDataModel> GetEnabledSiteList=(from s in _context.Site 
           where s.Enabled==true
           orderby s.SiteName
            select new SiteDataModel{
              SiteId=s.SiteId,
              SiteName=s.SiteName,
              Url=s.Url,
              Enabled=s.Enabled,
            }).ToList();
            return await Task.FromResult(GetEnabledSiteList);
        }
    }
}
