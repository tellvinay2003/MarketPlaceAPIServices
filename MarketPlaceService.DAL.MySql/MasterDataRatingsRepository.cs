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
    public class MasterDataRatingsRepository : BaseRepository, IMasterDataRatingsRepository
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

        public MasterDataRatingsRepository(MarketplaceDbContext context,IEnumerable<IChangeHistoryHelper> changeHistoryHelpers, IChangeHistoryRepository changeHistoryRepository) : base(context)
        {
            _changeHistoryHelper = changeHistoryHelpers.FirstOrDefault(a=> a is MasterDataRatingsChangeHistoryhelper);
            _changeHistoryRepository = changeHistoryRepository;
             _masterTypeId = context.MasterDataTypes.FirstOrDefault(a=>a.Datatypename.ToLower().Equals("rating")).Datatypeid;
        }

        public async Task<bool> DeleteMasterDataRatings(int ratingType, int ratingId)
        {
            var mappingDataRatings = _context.MappingData.Where(a => (a.Sourceid == ratingId || a.Targetid == ratingId) && a.Datatypeid == 7).ToList();
            var mappingDataLinkRatings = _context.MappingDataLink.Where(a => mappingDataRatings.Select(q => q.Mappingdataid).ToList().Contains(a.Mappingdataid)).ToList();

            var rating = _context.MasterData.FirstOrDefault(a=> a.Masterdataid==ratingId);
            var ratingLink = _context.MasterDataLink.FirstOrDefault(a=> a.Masterdataid==ratingId);

            _context.MappingDataLink.RemoveRange(mappingDataLinkRatings);
            _context.MappingData.RemoveRange(mappingDataRatings);
            _context.MasterDataLink.Remove(ratingLink);
            _context.MasterData.Remove(rating);
            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Delete,HistoryOrigin.Marketplace,_userId,rating, null,Guid.Empty, _changeHistoryHelper);
            return true;
        }

        public async Task<IEnumerable<MasterDataRating>> GetMasterDataRatings(int ratingType)
        {
            //fetch ratings by joining masterdata-> master_data_link - > master_data
            var ratings = (from masterData in _context.MasterData
            join masterDataLink in _context.MasterDataLink on masterData.Masterdataid equals masterDataLink.Parentmasterdataid
            join masterData2 in _context.MasterData on masterDataLink.Masterdataid equals masterData2.Masterdataid
            where masterData.Masterdataid==ratingType
            select new MasterDataRating
            {
                Id = masterData2.Masterdataid,
                Name = masterData2.Masterdataname
            }).ToList();

            return ratings;
        }

        public async Task<MasterDataRating> GetMasterDataRatings(int ratingType, int ratingId)
        {
            var rating = (from masterData in _context.MasterData
            join masterDataLink in _context.MasterDataLink on masterData.Masterdataid equals masterDataLink.Masterdataid
            join masterData2 in _context.MasterData on masterDataLink.Parentmasterdataid equals masterData2.Masterdataid
            where masterData.Datatypeid==ratingType
            select new MasterDataRating
            {
                Id = masterData.Masterdataid,
                Name = masterData.Masterdataname,
                Ratingtype = new MasterDataRatingType
                {
                    Id = masterData2.Masterdataid,
                    Name = masterData2.Masterdataname
                }
            }).FirstOrDefault();

            return rating;
        }

        public async Task<MasterDataRating> InsertMasterDataRatings(int ratingType, MasterDataRating data)
        {          

            Models.MasterData master = new Models.MasterData{
                Datatypeid = _masterTypeId,
                Masterdataname = data.Name
            };

            _context.MasterData.Add(master);
            _context.SaveChanges();
            //using new ID, insert data into master data link
            data.Id = master.Masterdataid;

            MasterDataLink link = new MasterDataLink{
                Masterdataid = master.Masterdataid,
                Parentmasterdataid = ratingType
            };

             _context.MasterDataLink.Add(link);
             _context.SaveChanges();

             await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Add, HistoryOrigin.Marketplace,_userId,master, null,Guid.Empty, _changeHistoryHelper);

             return data;

        }

        public async Task<MasterDataRating> UpdateMasterDataRatings(int ratingType, int ratingId, MasterDataRating data)
        {
            //fetch rating id
            var rating = _context.MasterData.FirstOrDefault(a=> a.Masterdataid==ratingId);

            if(rating == null)
                return null;

            //update and save
            var ratingHistory = new Models.MasterData{
                Masterdataname = rating.Masterdataname
            };

            data.Id = ratingId;

            rating.Masterdataname = data.Name;

            _context.MasterData.Update(rating);

            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(_masterTypeId, HistoryAction.Change,HistoryOrigin.Marketplace, _userId,ratingHistory, data,Guid.Empty, _changeHistoryHelper);

            return data;
        }

        
        public async Task<bool> CheckIfMappedToImportedProduct(int ratingId)
        {
            var isMapped = false;
            isMapped = _context.MarketplaceProductRating.Any(a => a.Ratingid == ratingId);
            return isMapped;
        }
    }
}
