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
    public class MasterDataServiceTypeRepository :BaseRepository, IMasterDataServiceTypeRepository
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
        public MasterDataServiceTypeRepository(MarketplaceDbContext context,IEnumerable<IChangeHistoryHelper> changeHistoryHelpers,IChangeHistoryRepository changeHistoryRepository) : base(context)
        {
             _masterTypeId = context.MasterDataTypes.FirstOrDefault(a=>a.Datatypename.ToLower().Equals("service type")).Datatypeid;
            _changeHistoryHelper = changeHistoryHelpers.FirstOrDefault(a=> a is MasterDataServiceTypeChangeHistoryHelper);
            _changeHistoryRepository = changeHistoryRepository;
        }        

        public async Task<MasterDataServiceType> GetMasterDataServiceType(int id)
        {
            return _context.MasterData.Where(a=>a.Datatype.Datatypename.ToLower().Equals("service type") && a.Masterdataid==id).Select(q=> new MasterDataServiceType
            {
                Id = q.Masterdataid,
                Name = q.Masterdataname,
                UsePublishedAddress = q.Servicetypepublishermainaddress.HasValue ? q.Servicetypepublishermainaddress.Value : false,
                Ratings = _context.MasterDataLink.Where(a=>a.Parentmasterdataid == id).Select(w=> new Entities.MasterData
                {
                    Id = w.Masterdata.Masterdataid,
                    Name = w.Masterdata.Masterdataname
                }).ToList()
            }).FirstOrDefault();            
        }

        public async Task<IEnumerable<MasterDataServiceType>> GetMasterDataServiceTypes()
        {
            return _context.MasterData.Where(a=>a.Datatype.Datatypename.ToLower().Equals("service type")).Select(q=> new MasterDataServiceType
            {
                Id = q.Masterdataid,
                Name = q.Masterdataname,
                UsePublishedAddress = q.Servicetypepublishermainaddress.HasValue ? q.Servicetypepublishermainaddress.Value : false,
                Ratings = _context.MasterDataLink.Where(a=>a.Parentmasterdataid == q.Masterdataid).Select(w=> new Entities.MasterData
                {
                    Id = w.Masterdata.Masterdataid,
                    Name = w.Masterdata.Masterdataname,


                }).OrderBy(w=>w.Name).ToList()
            }).OrderBy(q=>q.Name).ToList();
        }

        public async Task<MasterDataServiceType> InsertMasterDataServiceType(MasterDataServiceType data)
        {            
            var serviceType = new Models.MasterData
            {
                Datatypeid = _masterTypeId,
                Masterdataname = data.Name,
                Servicetypepublishermainaddress = data.UsePublishedAddress
            };

            _context.MasterData.Add(serviceType);
            _context.SaveChanges();

            if(data.Ratings!=null && data.Ratings.Count() > 0 )
            {
                var ratingTypes = data.Ratings.Select(a=> new MasterDataLink
                {
                    Parentmasterdataid = serviceType.Masterdataid,
                    Masterdataid = (int)a.Id
                }).ToList();

                 _context.MasterDataLink.AddRange(ratingTypes);
                _context.SaveChanges();
            }

            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Add, HistoryOrigin.Marketplace, _userId,serviceType, null, Guid.Empty, _changeHistoryHelper);

            return await GetMasterDataServiceType(serviceType.Masterdataid);
            
        }

        public async Task<MasterDataServiceType> UpdateMasterDataServiceType(int id, MasterDataServiceType data)
        {
            var serviceType = _context.MasterData.FirstOrDefault(a=> a.Masterdataid==id);

            if(serviceType == null)
                return null;

            var serviceTypeHistory = new Models.MasterData
            {
                Masterdataname = serviceType.Masterdataname,
                Servicetypepublishermainaddress = serviceType.Servicetypepublishermainaddress
            };
            
            serviceType.Masterdataname = data.Name;
            serviceType.Servicetypepublishermainaddress = data.UsePublishedAddress;

            var ratingtypes = _context.MasterDataLink.Where(a=> a.Parentmasterdataid == id).ToList();
            _context.MasterDataLink.RemoveRange(ratingtypes);

            if(data.Ratings!=null && data.Ratings.Count() > 0 )
            {
                var ratingTypes = data.Ratings.Select(a=> new MasterDataLink
                {
                    Parentmasterdataid = id,
                    Masterdataid = (int)a.Id
                }).ToList();

                 _context.MasterDataLink.AddRange(ratingTypes);                
            }
            
            _context.SaveChanges();
              if(serviceTypeHistory.Masterdataname!=data.Name)
             await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Change, HistoryOrigin.Marketplace, _userId,serviceTypeHistory, data, Guid.Empty, _changeHistoryHelper);
            return await GetMasterDataServiceType(id);

        }

        public async Task<bool> DeleteMasterDataServiceType(int id)
        {
            if(await GetMasterDataServiceType(id) == null)
                return false;
            var mappingData = _context.MappingData.Where(a => (a.Sourceid == id || a.Targetid == id) && a.Datatypeid == 1).ToList();
           
            var serviceType = _context.MasterData.FirstOrDefault(a=> a.Masterdataid==id);

            var ratingtypes = _context.MasterDataLink.Where(a=> a.Parentmasterdataid == id).ToList();

            _context.MappingData.RemoveRange(mappingData);
            _context.MasterDataLink.RemoveRange(ratingtypes);
            _context.MasterData.Remove(serviceType);
            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Delete, HistoryOrigin.Marketplace, _userId,serviceType, null, Guid.Empty, _changeHistoryHelper);
            return true;
        }
        public async Task<bool> CheckIfMappedToImportedProduct(int id)
        {
            var isMapped = false;
            isMapped = _context.MarketplaceProduct.Any(a => a.Servicetypeid == id);
            return isMapped;
        }
    }
}
