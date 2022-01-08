using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System.Linq;
using MarketPlaceService.DAL.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceService.DAL
{
    public class MasterDataRegionRepository : BaseRepository, IMasterDataRegionRepository
    {
        private readonly IChangeHistoryHelper _changeHistoryHelper;
        private readonly IChangeHistoryRepository _changeHistoryRepository;
        private readonly int _masterTypeId;
         private Guid _userId;
        public Guid UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                
                _userId = value;
            }
        }

        public MasterDataRegionRepository(MarketplaceDbContext context, IEnumerable<IChangeHistoryHelper> changeHistoryHelpers, IChangeHistoryRepository changeHistoryRepository) : base(context)
        {
            _changeHistoryHelper = changeHistoryHelpers.FirstOrDefault(a => a is MasterDataRegionChangeHistoryHelper);
            _changeHistoryRepository = changeHistoryRepository;
            _masterTypeId = context.MasterDataTypes.FirstOrDefault(a => a.Datatypename.ToLower().Equals("region")).Datatypeid;
        }

        public async Task<IEnumerable<MasterDataGeolocation>> GetMasterDataGeolocations(string name)
        {
            var regions = _context.MasterRegions.Where(a => a.Regionname.ToLower().Contains(name.ToLower())).Select(q => new MasterDataGeolocation
            {
                Id = q.Regionid,
                Name = q.Regionname,
                Level = q.Level,
                ParentId = q.Parentregionid
            });
            if (!string.IsNullOrWhiteSpace(name))
            {
                regions.Where(a => a.Name.ToLower().Contains(name.ToLower()));
            }
            regions = regions.OrderBy(a => a.Name);
            return await regions.ToListAsync();
        }

        public async Task<MasterDataGeolocation> GetMasterDataGeolocations(int id)
        {
            var regions = _context.MasterRegions.Where(a => a.Regionid == id || a.Parentregionid == id).Select(q => q).ToList();

            var response = regions.Where(a => a.Regionid == id).Select(q => new MasterDataGeolocation
            {
                Id = id,
                Name = q.Regionname,
                Level = q.Level,
                ChildRegions = regions.Where(w => w.Parentregionid == id).Select(s => new MasterDataGeolocation
                {
                    Id = s.Regionid,
                    Name = s.Regionname,
                    Level = s.Level,
                    ParentId = id
                }).ToList()
            }).FirstOrDefault();

            return response;
        }

        public async Task<MasterDataGeolocation> InsertMasterDataGeolocations(int? parentId, MasterDataGeolocation data)
        {
            MasterRegions region = new MasterRegions
            {
                Parentregionid = parentId == 0 ? null : parentId,
                Level = data.Level,
                Regionname = data.Name
            };

            _context.MasterRegions.Add(region);
            _context.SaveChanges();

            data.Id = region.Regionid;
            data.ParentId = parentId;

            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Add, HistoryOrigin.Marketplace, _userId, region, null, Guid.Empty, _changeHistoryHelper);
            return data;

        }

        public async Task<MasterDataGeolocation> UpdateMasterDataGeolocations(int id, MasterDataGeolocation data)
        {
            //Find region by ID
            var region = _context.MasterRegions.FirstOrDefault(a => a.Regionid == id);

            if (region == null)
                return null;

            var regionForHistory = region.Clone();

            //update values passed in request
            // region.Parentregionid = data.ParentId;
            // region.Level = data.Level;
            region.Regionname = data.Name;

            //save changes
            _context.MasterRegions.Update(region);
            _context.SaveChanges();

            data.Level = region.Level;
            data.Id = region.Regionid;
            data.ParentId = region.Parentregionid;

            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Change, HistoryOrigin.Marketplace, _userId, regionForHistory, data, Guid.Empty, _changeHistoryHelper);

            return data;
        }

        public async Task<bool> DeleteMasterDataGeolocations(int id)
        {
            var regionsToBeRemoved = new List<MasterRegions>();
            var region = _context.MasterRegions.FirstOrDefault(a => a.Regionid == id);

            if (region == null)
                return false;

            var lowerLevelRegions = _context.MasterRegions.Where(q => q.Level > region.Level).ToList();

            regionsToBeRemoved.Add(region);
            FetchChildRegions(regionsToBeRemoved, lowerLevelRegions, regionsToBeRemoved);
            
             var mappingDataLinks = _context.MappingData.Where(a => a.Datatypeid == 5 && regionsToBeRemoved.Select(q => q.Regionid).ToList().Contains(a.Sourceid)).ToList(); //check in MAPPING_DATA_LINK table
             mappingDataLinks.AddRange(_context.MappingData.Where(a => a.Datatypeid == 5 && regionsToBeRemoved.Select(q => q.Regionid).ToList().Contains(a.Targetid)).ToList());

            _context.MappingData.RemoveRange(mappingDataLinks);
            _context.MasterRegions.RemoveRange(regionsToBeRemoved);
            _context.SaveChanges();
            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Delete, HistoryOrigin.Marketplace, _userId, region, null, Guid.Empty, _changeHistoryHelper);
            return true;
        }

        private void FetchChildRegions(IEnumerable<MasterRegions> regions, IEnumerable<MasterRegions> lowerLevelRegions, List<MasterRegions> regionsToBeRemoved)
        {
            if (lowerLevelRegions.Any(a => regions.Select(q => q.Regionid).Contains((int)a.Parentregionid)))
            {
                var childRegions = lowerLevelRegions.Where(a => regions.Select(q => q.Regionid).Contains((int)a.Parentregionid)).ToList();
                regionsToBeRemoved.AddRange(childRegions);
                FetchChildRegions(childRegions, lowerLevelRegions, regionsToBeRemoved);
            }
        }

        public async Task<bool> CheckIfMappedToImportedProduct(int regionId)
        {
            var isMapped = false;
            isMapped = _context.MarketplaceProduct.Any(a => a.Regionid == regionId);
            return isMapped;
        }
    }
}
