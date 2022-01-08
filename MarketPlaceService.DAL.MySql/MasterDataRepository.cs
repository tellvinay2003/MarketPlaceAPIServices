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
    public class MasterDataRepository : BaseRepository, IMasterDataRepository
    {
        private readonly IChangeHistoryHelper _changeHistoryHelper;
        private readonly IChangeHistoryRepository _changeHistoryRepository;
       // private Guid _user = Guid.Empty;

        
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

        public MasterDataRepository(MarketplaceDbContext context,IEnumerable<IChangeHistoryHelper> changeHistoryHelpers, IChangeHistoryRepository changeHistoryRepository) : base(context)
        {
            _changeHistoryHelper = changeHistoryHelpers.FirstOrDefault(a=> a is MasterDataChangeHistoryHelper);
            _changeHistoryRepository = changeHistoryRepository;
        }
        public async Task<IEnumerable<Entities.MasterData>> GetMasterDataGeneric(int masterDataTypeId)
        {
            return _context.MasterData.Where(q=>q.Datatypeid == masterDataTypeId).Select(a=> new Entities.MasterData
            {
                Id = a.Masterdataid,
                Name = a.Masterdataname.Trim()
            }).OrderBy(x=>x.Name).ToList();
        }

        public async Task<Entities.MasterData> GetMasterDataGeneric(int masterDataTypeId, int itemId)
        {
            return _context.MasterData.Where(q=>q.Datatypeid == masterDataTypeId && q.Masterdataid==itemId).Select(a=> new Entities.MasterData
            {
                Id = a.Masterdataid,
                Name = a.Masterdataname.Trim()
            }).FirstOrDefault();
        }

        public async Task<Entities.MasterData> InsertMasterDataGeneric(int masterDataTypeId, Entities.MasterData item)
        {
            if(await GetMasterDataGeneric(masterDataTypeId) == null)
                return null;

            Models.MasterData data = new Models.MasterData
            {
                Datatypeid = masterDataTypeId,
                Masterdataname = item.Name
            };

            _context.MasterData.Add(data);
            _context.SaveChanges();
            item.Id = data.Masterdataid;

            await _changeHistoryRepository.SaveHistory(masterDataTypeId, HistoryAction.Add, HistoryOrigin.Marketplace, _userId,data, null,Guid.Empty, _changeHistoryHelper);
            
            return item;
        }

        public async Task<Entities.MasterData> UpdateMasterDataGeneric(int masterDataTypeId, int itemId, Entities.MasterData item)
        {
            if(await GetMasterDataGeneric(masterDataTypeId,itemId) == null)
                return null;
            
            var data = _context.MasterData.FirstOrDefault(md=> md.Masterdataid ==itemId && md.Datatypeid == masterDataTypeId);
            var dataForHistory = data.Clone();
            data.Masterdataname = item.Name;

            _context.MasterData.Update(data);
            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(masterDataTypeId, HistoryAction.Change, HistoryOrigin.Marketplace, _userId,dataForHistory, item, Guid.Empty, _changeHistoryHelper);

            return item;

        }

        public async Task<bool> DeleteMasterDataGeneric(int masterDataTypeId, int itemId)
        {
            if(await GetMasterDataGeneric(masterDataTypeId,itemId) == null)
                return false;
            
            List<MappingData> data = new List<MappingData>();
            Models.MasterData master = _context.MasterData.FirstOrDefault(a=> a.Datatypeid == masterDataTypeId && a.Masterdataid ==itemId);

            //remove the data from mapped table where MP is the target
            var mappedItem = _context.MappingData.Where(a=>a.Mappingdirection.Mappingdirectionname.ToLower().Equals("site-mp") && a.Targetid==itemId && a.Datatypeid == masterDataTypeId).ToList();
            if(mappedItem!=null)
                data.AddRange(mappedItem);

            //remove the data from mapped table where MP is the source
            mappedItem = _context.MappingData.Where(a=>a.Mappingdirection.Mappingdirectionname.ToLower().Equals("mp-site") && a.Sourceid==itemId && a.Datatypeid == masterDataTypeId).ToList();
            if(mappedItem!=null)
                data.AddRange(mappedItem);    

            var mappingDataLinks = _context.MappingDataLink.Where(a=> data.Select(q => q.Sourceid).ToList().Contains(a.Parentdataid)).ToList(); //check in MAPPING_DATA_LINK table
            mappingDataLinks.AddRange(_context.MappingDataLink.Where(a=> data.Select(q => q.Targetid).ToList().Contains(a.Parentdataid)).ToList());
            var mappingDataSub =  _context.MappingData.Where(a=> mappingDataLinks.Select(q => q.Mappingdataid).ToList().Contains(a.Mappingdataid) && a.Datatypeid == masterDataTypeId).ToList();

            //check if theres a link in master_data_link table
            var masterDataLinks = _context.MasterDataLink.Where(a=> a.Parentmasterdataid == itemId).ToList();
            var masterDataSubItems =  _context.MasterData.Where(a=> masterDataLinks.Select(q => q.Masterdataid).ToList().Contains(a.Masterdataid)).ToList();
              
            masterDataLinks.AddRange(_context.MasterDataLink.Where(a=> a.Masterdataid == itemId).ToList());
            
            _context.MappingDataLink.RemoveRange(mappingDataLinks);
            _context.MappingData.RemoveRange(mappingDataSub);
            _context.MappingData.RemoveRange(data);
            _context.MasterDataLink.RemoveRange(masterDataLinks);
            _context.MasterData.RemoveRange(masterDataSubItems);

            _context.MasterData.Remove(master);

            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(masterDataTypeId, HistoryAction.Delete , HistoryOrigin.Marketplace, _userId, master, null,Guid.Empty, _changeHistoryHelper);

            return true;

        }     
        public async Task<bool> CheckIfMappedToImportedProduct(int ratingTypeId)
        {
            var isMapped = false;
            var ratingIds = _context.MasterDataLink.Where(a=>a.Parentmasterdataid == ratingTypeId).Select(a=>a.Masterdataid).ToList();
            isMapped = _context.MarketplaceProductRating.Any(a => ratingIds.Select(q=>q).ToList().Contains(a.Ratingid));
            return isMapped;
        }  
    }
}
