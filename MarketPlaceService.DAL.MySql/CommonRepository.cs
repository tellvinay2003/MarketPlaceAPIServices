using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MarketPlaceService.DAL.Models;

namespace MarketPlaceService.DAL
{
    public class CommonRepository : BaseRepository, ICommonRepository
    {
        public CommonRepository(MarketplaceDbContext context) : base(context)
        {

        }

        public async Task<string> GetSiteUrl(Guid entityId, EntityType entityType)
        {
            var response = "";
            switch(entityType)
            {
                case EntityType.Publisher:
                    response= await GetSiteUrlByPublisherId(entityId);
                    break;
                case EntityType.Subscriber:
                    response= await GetSiteUrlBySubscriberId(entityId);
                    break;
                case EntityType.Site:
                    response= await GetSiteUrlBySiteId(entityId);
                    break;
            }
            return response;
        }

        public async Task<string> GetSiteUrlByPublisherId(Guid publisherId)
        {
            var siteUrl = (from publisher in _context.Publisher
                            join site in _context.Site on publisher.SiteId equals site.SiteId
                            where publisher.PublisherId == publisherId
                            select site.Url).FirstOrDefault();

           return await Task.FromResult(siteUrl);
        }

        public async Task<string> GetSiteUrlBySubscriberId(Guid subscriberId)
        {
            var siteUrl = (from subscriber in _context.Subscriber
                            join site in _context.Site on subscriber.SiteId equals site.SiteId
                            where subscriber.SubscriberId == subscriberId
                            select site.Url).FirstOrDefault();

           return await Task.FromResult(siteUrl);
        }

         public async Task<string> GetSiteUrlBySiteId(Guid siteId)
        {
            var siteUrl = (from site in _context.Site
                            where site.SiteId == siteId
                            select site.Url).FirstOrDefault();

           return await Task.FromResult(siteUrl);
        }

        public async Task<int> GetMappingDirectionId(Entities.MappingDirection direction)
        {
            Models.MappingDirection mappingDirection = null;
            switch(direction)
            {
                case Entities.MappingDirection.MpToSite:
                    mappingDirection= _context.MappingDirection.FirstOrDefault(md => md.Mappingdirectionname.ToLower().Equals("mp-site"));
                    break;
                case Entities.MappingDirection.SiteToMp:
                    mappingDirection = _context.MappingDirection.FirstOrDefault(md => md.Mappingdirectionname.ToLower().Equals("site-mp"));
                    break;
            }
            
            if(mappingDirection != null)
                return mappingDirection.Mappingdirectionid;

            return 0;
        }

        public async Task<int> GetDataTypeIdFromName(string datatype)
        {
            return _context.MasterDataTypes.FirstOrDefault(a=> a.Datatypename.ToLower().Equals(datatype)).Datatypeid;
        }

        public async Task<int> GetMessageTypeId(Guid MarketPlaceProductId)
        {
           var MessageTypeId= _context.MarketplaceProduct.FirstOrDefault(a=>a.Marketplaceproductid==MarketPlaceProductId).Messagetypeid;
           return await Task.FromResult(MessageTypeId);
        }

        public async Task<HistoryOrigin> GetHistoryOriginFromMappingDirection(Entities.MappingDirection direction)
        {
            switch(direction)
            {
                case Entities.MappingDirection.MpToSite:
                    return HistoryOrigin.MpToSite;
                case Entities.MappingDirection.SiteToMp:
                    return HistoryOrigin.SiteToMp;
            }

            return HistoryOrigin.SiteToMp;
        }

        public Error GetErrorDetails(Error error)
        {
            //hardcoded ids now. please get from the db
            switch(error.Type)
            {
                case ErrorType.DataMappingIntegrity:
                    error.ErrorId = 1;
                    break;
                case ErrorType.ReadingMapping:
                    error.ErrorId = 2;
                    break;
                case ErrorType.FetchingProductData:
                case ErrorType.SaveData:
                case ErrorType.Unknown:
                case ErrorType.SubscriptionPost:
                    error.ErrorId=3;
                    break;
            }

            return error;
        }

        public async Task<List<int>> GetOrganisationIdsUsedForSite(EntityType entityType, Guid entityId, Guid siteId)
        {
            List<int> response = new List<int>();
            switch(entityType)
            {
                case EntityType.Publisher:
                    response = _context.Publisher.Where(a => a.SiteId == siteId && a.PublisherId != entityId).Select(q => (int)q.OrganizationId).ToList();
                    break;
                case EntityType.Subscriber:
                    response = _context.Subscriber.Where(a => a.SiteId == siteId && a.SubscriberId != entityId).Select(q => (int)q.OrganizationId).ToList();
                    break;
            }

            return response;
        }

        public async Task<bool> DuplicateMasterDataNameExists(int dataTypeId, string dataTypeName, int dataId, string dataName, int parentTypeId)
        {
            var dataExists = false;
            if(!string.IsNullOrEmpty(dataName))
            {
            if(dataTypeId == 0)
            {
                dataTypeId = _context.MasterDataTypes.FirstOrDefault(a=>a.Datatypename.ToLower().Equals(dataTypeName)).Datatypeid;
            }
            
            if(parentTypeId > 0)
            {
                 dataExists = (from masterData in _context.MasterData
                    join masterDataLink in _context.MasterDataLink on masterData.Masterdataid equals masterDataLink.Parentmasterdataid
                    join masterData2 in _context.MasterData on masterDataLink.Masterdataid equals masterData2.Masterdataid
                    where masterData.Masterdataid==parentTypeId
                    select new MasterDataRating
                    {
                        Id = masterData2.Masterdataid,
                        Name = masterData2.Masterdataname
                    }).ToList().Any(a => a.Name.ToLower() == dataName.ToLower());
            }
            else
            {
                if(dataId > 0)
                {
                    dataExists = (from masterData in _context.MasterData
                                        where masterData.Datatypeid == dataTypeId                                    
                                        select masterData).Any(a => a.Masterdataname.ToLower() == dataName.ToLower() && a.Masterdataid != dataId);
                }
                else
                {
                    dataExists = (from masterData in _context.MasterData
                                        where masterData.Datatypeid == dataTypeId                                    
                                        select masterData).Any(a => a.Masterdataname.ToLower() == dataName.ToLower());
                }
            }
            }
           
           return  dataExists;
        }

       public async  Task<short> GetProductTypeId(Guid marketplaceProductId)
       {
            return (from mp in _context.MarketplaceProduct
                                join pp in _context.PublishedProducts 
                                on mp.Publishedproductid equals pp.PublishedProductId
                                where mp.Marketplaceproductid == marketplaceProductId
                                select pp.ProductTypeId).FirstOrDefault();
       }
        
    }
}
