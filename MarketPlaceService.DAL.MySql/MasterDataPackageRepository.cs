using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.Entities;
using System.Linq;
using MarketPlaceService.DAL.Utilities;

namespace MarketPlaceService.DAL
{
    public class MasterDataPackageRepository : BaseRepository, IMasterDataPackageRepository
    {
        private readonly int _masterTypeId;
        private readonly IChangeHistoryHelper _changeHistoryHelper;
        private readonly IChangeHistoryRepository _changeHistoryRepository;
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
        public MasterDataPackageRepository(MarketplaceDbContext context,IEnumerable<IChangeHistoryHelper> changeHistoryHelpers,IChangeHistoryRepository changeHistoryRepository) : base(context)
        {
            // _masterTypeId = context.MasterDataTypes.FirstOrDefault(a=>a.Datatypename.ToLower().Equals("service type")).Datatypeid;
            _changeHistoryHelper = changeHistoryHelpers.FirstOrDefault(a=> a is MasterDataServiceTypeChangeHistoryHelper);
            _changeHistoryRepository = changeHistoryRepository;
        }

        public async Task<IEnumerable<MasterDataPackage>> GetMasterDataPackage(int id)
        {
            return _context.MasterData.Where(a=>a.Datatypeid == id).Select(q=> new MasterDataPackage
            {
                Id = q.Masterdataid,
                Name = q.Masterdataname,
                ServiceLink = _context.MasterDataLink.Where(a=>a.Parentmasterdataid == q.Masterdataid).Select(w=> new Entities.MasterData
                {
                    Id = w.Masterdata.Masterdataid,
                    Name = w.Masterdata.Masterdataname,
                }).FirstOrDefault()
            }).OrderBy(q=>q.Name).ToList();
        }
     
        public async Task<MasterDataPackage> InsertMasterDataPackage(int masterDataTypeId,MasterDataPackage data)
        {
            var PackageLink = new Models.MasterData
            {
                Datatypeid = _context.MasterDataTypes.FirstOrDefault(a=>a.Datatypeid==masterDataTypeId).Datatypeid,
                Masterdataname = data.Name,
            };

            _context.MasterData.Add(PackageLink);
            _context.SaveChanges();

            data.Id = PackageLink.Masterdataid;

             MasterDataLink link = new MasterDataLink{
                Masterdataid = (int)data.ServiceLink.Id,
                Parentmasterdataid = PackageLink.Masterdataid
            };

             _context.MasterDataLink.Add(link);
             _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(masterDataTypeId, HistoryAction.Add, HistoryOrigin.Marketplace, _userId,PackageLink, null, Guid.Empty, _changeHistoryHelper);

            return data;
        }

        public async Task<MasterDataPackage> UpdateMasterDataPackage(int masterDataTypeId,int id, MasterDataPackage data)
        {
             var Packagelink = _context.MasterData.FirstOrDefault(a=> a.Masterdataid==id);

            if(Packagelink == null)
                return null;

            //update and save
            var PackagelinkHistory = new Models.MasterData{
                Masterdataname = Packagelink.Masterdataname
            };

            data.Id = id;

            Packagelink.Masterdataname = data.Name;

            var PackageTypes = _context.MasterDataLink.Where(a=> a.Parentmasterdataid == id).ToList();
            _context.MasterDataLink.RemoveRange(PackageTypes);

             MasterDataLink link = new MasterDataLink{
                Masterdataid = (int)data.ServiceLink.Id,
                Parentmasterdataid = Packagelink.Masterdataid
            };

            _context.MasterDataLink.Add(link);
            _context.MasterData.Update(Packagelink);

            _context.SaveChanges();

            var PackageNewdata=new MasterDataServiceType{
                Id=id,
                Name=data.Name,
            };

            await _changeHistoryRepository.SaveHistory(masterDataTypeId, HistoryAction.Change,HistoryOrigin.Marketplace, _userId,PackagelinkHistory, PackageNewdata,Guid.Empty, _changeHistoryHelper);

            return data;
        }

        public async Task<bool> DeleteMasterDataPackage(int masterDataTypeId,int id)
        {
            if(await GetMasterDataPackage(id) == null)
                return false;
            var mappingData = _context.MappingData.Where(a => (a.Sourceid == id || a.Targetid == id) && a.Datatypeid == masterDataTypeId).ToList();
           
            var packageLink = _context.MasterData.FirstOrDefault(a=> a.Masterdataid==id);
        
            var serviceLink = _context.MasterDataLink.Where(a=> a.Parentmasterdataid == id).ToList();

            _context.MappingData.RemoveRange(mappingData);
            _context.MasterDataLink.RemoveRange(serviceLink);
            _context.MasterData.Remove(packageLink);
            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Delete, HistoryOrigin.Marketplace, _userId,packageLink, null, Guid.Empty, _changeHistoryHelper);
            return true;
        }

    }
}
