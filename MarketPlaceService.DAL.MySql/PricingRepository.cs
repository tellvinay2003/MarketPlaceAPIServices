using MarketPlaceService.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarketPlaceService.Entities;
using System.Linq;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities.TSv2ApiEntities;

namespace MarketPlaceService.DAL
{
    public class PricingRepository : BaseRepository, IPricingRepository
    {
        public PricingRepository(MarketplaceDbContext context): base(context)
        {
           
        }

        public List<SiteServiceData> GetSiteServiceData(List<Guid> marketplaceProductIds, Guid subscriberId)
        {
            var data = (from mp in _context.MarketplaceProduct
                        join pp in _context.PublishedProducts on mp.Publishedproductid equals pp.PublishedProductId
                        join p in _context.Publisher on pp.PublisherId equals p.PublisherId
                        join pa in _context.PublisherAgent on p.PublisherId equals pa.PublisherId
                        where marketplaceProductIds.Contains(mp.Marketplaceproductid)
                        && pa.SubscriberD == subscriberId
                        select new SiteServiceData
                        {
                            MarketplaceProductId = mp.Marketplaceproductid,
                            SiteId = p.SiteId,
                            MPServiceId = pp.ProductId,
                            AgentId = pa.AgentId,
                            BookingPrefixId = pa.BookingPrefixId,
                            BookingOwnerId = pa.UserId,
                            PublisherId = p.PublisherId
                        }).ToList();

            return data;
        }

        public Guid GetSubscriberIdBySiteAndOrganisation(Guid siteId, int organisationId)
        {            
            return _context.Subscriber.FirstOrDefault(a => a.SiteId == siteId && a.OrganizationId == organisationId).SubscriberId;
        }
        public bool CheckIfHasAccess(Guid marketplaceProductId, Guid subscriberId)
        {
           return (from ppsa in _context.PublishedProductAllowedSubscriber
              join  mp in _context.MarketplaceProduct  on ppsa.Publishedproductid equals mp.Publishedproductid
              where ppsa.Subscriberid == subscriberId && mp.Marketplaceproductid == marketplaceProductId
              select(1)).Any();          
        }

        public List<SubscriberDataModel> GetSubscribersBySite(Guid siteId)
        {
            return _context.Subscriber.Where(a => a.SiteId == siteId).Select(q => new SubscriberDataModel
            {
                SiteId = siteId,
                SubscriberId = q.SubscriberId,
                SubscriberName = q.SubscriberName,
                Enabled = q.Enabled,
                OrganizationId = q.OrganizationId
            }).ToList() ;
        }
    }
}