using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System.Linq;

namespace MarketPlaceService.DAL
{
    public class MappingDataConfigRepository : BaseRepository, IMappingDataConfigRepository
    {
        private readonly ICommonRepository _commonRepository;

        public MappingDataConfigRepository(MarketplaceDbContext context, ICommonRepository commonRepository) : base(context)
        {
            _commonRepository = commonRepository;
        }

        public async Task<MappingDataConfig> GetMappingDataConfig(Entities.MappingDirection direction, ushort dataTypeId, Guid site)
        {     
             var mappingDataConfig = new MappingDataConfig();       
            var directionId =  await _commonRepository.GetMappingDirectionId(direction);  

            int subscriberSite =  (from s in _context.Site 
             join sub in _context.Subscriber on s.SiteId equals sub.SiteId
             where s.SiteId == site && sub.Enabled select s).Count();
                 

             int publisherSite =  (from s in _context.Site 
             join pub in _context.Publisher on s.SiteId equals pub.SiteId
             where s.SiteId == site && pub.Enabled select s).Count();

             if((subscriberSite > 0 && publisherSite > 0) || (subscriberSite > 0 && directionId == 2) || (publisherSite > 0 && directionId == 1))
             {
                mappingDataConfig = (from mdt in _context.MasterDataTypes 
                //join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
                join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
                where mdt.Datatypeid == dataTypeId
                select new MappingDataConfig
                {
                    Id = mdt.Datatypeid,
                    Name = mdt.Datatypename,
                    Style = df.Formatname
                }).FirstOrDefault();            
             }
             else if(subscriberSite > 0  && directionId == 1)
             {
                    mappingDataConfig = (from mdt in _context.MasterDataTypes 
                    join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
                    join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
                    where mdta.Mappingdirectionid == directionId  && mdt.Datatypeid == dataTypeId
                    && mdta.Issubscriber == true
                    select new MappingDataConfig
                    {
                        Id = mdt.Datatypeid,
                        Name = mdt.Datatypename,
                        Style = df.Formatname
                    }).FirstOrDefault();
             }
             else if(publisherSite > 0  && directionId == 2)
             {
                    mappingDataConfig = (from mdt in _context.MasterDataTypes 
                    join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
                    join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
                    where mdta.Mappingdirectionid == directionId  && mdt.Datatypeid == dataTypeId
                    && mdta.Ispublisher == true
                    select new MappingDataConfig
                    {
                        Id = mdt.Datatypeid,
                        Name = mdt.Datatypename,
                        Style = df.Formatname
                    }).FirstOrDefault();
             }          
            
            // var mappingDataConfig = (from mdt in _context.MasterDataTypes 
            // join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
            // join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
            // where mdt.Datatypeid == dataTypeId && mdta.Mappingdirectionid == directionId  && mdt.SiteId == site
            // select new MappingDataConfig
            // {
            //     Id = mdt.Datatypeid,
            //     Name = mdt.Datatypename,
            //     Style = df.Formatname
            // }).FirstOrDefault();
            
            return mappingDataConfig;
        }

        public async Task<IEnumerable<MappingDataConfig>> GetMappingDataConfig(Entities.MappingDirection direction, Guid site)
        {
            var mappingDataConfig = new List<MappingDataConfig>();
            var directionId =  await _commonRepository.GetMappingDirectionId(direction); 
             int subscriberSite =  (from s in _context.Site 
             join sub in _context.Subscriber on s.SiteId equals sub.SiteId
             where s.SiteId == site && sub.Enabled select s).Count();
                 

             int publisherSite =  (from s in _context.Site 
             join pub in _context.Publisher on s.SiteId equals pub.SiteId
             where s.SiteId == site && pub.Enabled select s).Count();

             if((subscriberSite > 0 && publisherSite > 0) || (subscriberSite > 0 && directionId == 2) || (publisherSite > 0 && directionId == 1))
             {
                mappingDataConfig = (from mdt in _context.MasterDataTypes 
                //join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
                join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
                //where mdta.Mappingdirectionid == directionId 
                select new MappingDataConfig
                {
                    Id = mdt.Datatypeid,
                    Name = mdt.Datatypename,
                    Style = df.Formatname
                }).ToList();    
                if(directionId == 2 && subscriberSite > 0)
                mappingDataConfig = mappingDataConfig.Where(m => m.Id != 15 && m.Id != 16 && m.Id != 17).ToList();        
             }
             else if(subscriberSite > 0  && directionId == 1)
             {
                    mappingDataConfig = (from mdt in _context.MasterDataTypes 
                    join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
                    join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
                    where mdta.Mappingdirectionid == directionId 
                    && mdta.Issubscriber == true
                    select new MappingDataConfig
                    {
                        Id = mdt.Datatypeid,
                        Name = mdt.Datatypename,
                        Style = df.Formatname
                    }).ToList();
             }
             else if(publisherSite > 0  && directionId == 2)
             {
                    mappingDataConfig = (from mdt in _context.MasterDataTypes 
                    join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
                    join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
                    where mdta.Mappingdirectionid == directionId 
                    && mdta.Ispublisher == true
                    select new MappingDataConfig
                    {
                        Id = mdt.Datatypeid,
                        Name = mdt.Datatypename,
                        Style = df.Formatname
                    }).ToList();
             }
            
            // mappingDataConfig = (from mdt in _context.MasterDataTypes 
            // join mdta in _context.MasterDataTypesApplicable on mdt.Datatypeid equals mdta.Datatypeid
            // join df in _context.DataFormat on mdt.Mappinguiformat equals df.Formatid
            // where mdta.Mappingdirectionid == directionId 
            // select new MappingDataConfig
            // {
            //     Id = mdt.Datatypeid,
            //     Name = mdt.Datatypename,
            //     Style = df.Formatname
            // }).ToList();
            
            return mappingDataConfig;
        }
    }
}
