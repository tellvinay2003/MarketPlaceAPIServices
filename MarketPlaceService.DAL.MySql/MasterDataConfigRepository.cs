using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System.Linq;

namespace MarketPlaceService.DAL
{
    public class MasterDataConfigRepository : BaseRepository, IMasterDataConfigRepository
    {
        public MasterDataConfigRepository(MarketplaceDbContext context) : base(context)
        {
            
        }
        public async Task<IEnumerable<MasterDataConfig>> GetMasterDataConfig()
        {
            return _context.MasterDataTypes.Select(a=> new MasterDataConfig
            {
                Id = a.Datatypeid,
                Name = a.Datatypename,
                Style = a.MasteruiformatNavigation.Formatname
            }).ToList();
        }

        public async Task<MasterDataConfig> GetMasterDataConfig(int id)
        {
            return _context.MasterDataTypes.Where(q=> q.Datatypeid ==id).Select(a=> new MasterDataConfig
            {
                Id = a.Datatypeid,
                Name = a.Datatypename,
                Style = a.MasteruiformatNavigation.Formatname
            }).FirstOrDefault();
        }
    }
}
