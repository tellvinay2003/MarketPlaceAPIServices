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
    public class MappingDataRatingRepository : BaseRepository, IMappingDataRatingRepository
    {
         private readonly IChangeHistoryHelper _changeHistoryHelper;
        private readonly IChangeHistoryRepository _changeHistoryRepository;
        private readonly ICommonRepository _commonRepository;
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

        public MappingDataRatingRepository(MarketplaceDbContext context, ICommonRepository commonRepository, IEnumerable<IChangeHistoryHelper> changeHistoryHelpers, IChangeHistoryRepository changeHistoryRepository) : base(context)
        {
            _commonRepository = commonRepository;
             _changeHistoryHelper = changeHistoryHelpers.FirstOrDefault(a=> a is MappingDataRatingsChangeHistoryHelper);
            _changeHistoryRepository = changeHistoryRepository;
        }

        public async Task<bool> DeleteMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            var dataTypeId = await _commonRepository.GetDataTypeIdFromName("rating"); //todo change to const            
            var mappedItem = _context.MappingData.FirstOrDefault(md => md.Datatypeid == dataTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && md.Sourceid == sourceId);

            if(mappedItem ==null)
                return false;
          

            //start of block to remove data from links in case item being removed is a child of some other data
            var linkToRemove = _context.MappingDataLink.FirstOrDefault(a=>a.Mappingdataid == mappedItem.Mappingdataid && ratingTypeId == a.Parentdataid);
            
            if(linkToRemove !=null)
            {
                 _context.MappingDataLink.Remove(linkToRemove);
            }
            //end of block to remove data from links in case item being removed is a child of some other data

            _context.MappingData.Remove(mappedItem);
            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(dataTypeId, HistoryAction.Delete, await _commonRepository.GetHistoryOriginFromMappingDirection(direction),_userId, mappedItem, null,siteId, _changeHistoryHelper);

            return true;
        }

        public async Task<IEnumerable<DataMapResponse>> GetMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            var dataTypeId = await _commonRepository.GetDataTypeIdFromName("rating"); //todo change to const

            var response = (from md in _context.MappingData 
            join mdl in _context.MappingDataLink on md.Mappingdataid equals mdl.Mappingdataid
            where md.Datatypeid == dataTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && mdl.Parentdataid == ratingTypeId
            select new DataMapResponse
            {
                SourceId = md.Sourceid,
                SourceName = md.Sourcename,
                TargetId = md.Targetid,
                TargetName = md.Targetname
            }).ToList();

            return response;
        }

        public async Task<DataMapResponse> GetMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            var dataTypeId = await _commonRepository.GetDataTypeIdFromName("rating"); //todo change to const

            var response = (from md in _context.MappingData 
            join mdl in _context.MappingDataLink on md.Mappingdataid equals mdl.Mappingdataid
            where md.Datatypeid == dataTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && mdl.Parentdataid == ratingTypeId && md.Sourceid == sourceId
            select new DataMapResponse
            {
                SourceId = md.Sourceid,
                SourceName = md.Sourcename,
                TargetId = md.Targetid,
                TargetName = md.Targetname
            }).FirstOrDefault();

            return response;
        }

        public async Task<DataMapResponse> InsertMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId, DataMap data)
        {
            var directionId =  await _commonRepository.GetMappingDirectionId(direction);
            var dataTypeId = await _commonRepository.GetDataTypeIdFromName("rating"); //todo change to const

            var mappingData = new Models.MappingData
            {
                Datatypeid = dataTypeId,
                Mappingdirectionid = directionId,
                Siteid = siteId,
                Sourceid = sourceId,
                Sourcename = data.SourceName,
                Targetid = data.TargetId,
                Targetname = data.TargetName
            };

            _context.MappingData.Add(mappingData);
             _context.SaveChanges();
            _context.MappingDataLink.Add(new MappingDataLink{ Parentdataid = ratingTypeId, Mappingdataid = mappingData.Mappingdataid});

            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(dataTypeId, HistoryAction.Add, await _commonRepository.GetHistoryOriginFromMappingDirection(direction),_userId, mappingData, null,siteId, _changeHistoryHelper);

            return new  DataMapResponse
            {
                SourceId = mappingData.Sourceid,
                SourceName = mappingData.Sourcename,
                TargetId = mappingData.Targetid,
                TargetName = mappingData.Targetname
            };
        }

        public async Task<DataMapResponse> UpdateMappedData(Entities.MappingDirection direction, Guid siteId, int ratingTypeId, int sourceId, DataMap data)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            var dataTypeId = await _commonRepository.GetDataTypeIdFromName("rating"); //todo change to const
           
            var mappedItem = _context.MappingData.FirstOrDefault(md => md.Datatypeid == dataTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && md.Sourceid == sourceId);
            
            if(mappedItem==null)
                return null;

            var mappedItemHistory = new Models.MappingData{
                Sourceid = mappedItem.Sourceid,
                Sourcename = mappedItem.Sourcename,
                Targetid = mappedItem.Targetid,
                Targetname = mappedItem.Targetname
            };

            mappedItem.Targetid = data.TargetId;
            mappedItem.Targetname = data.TargetName;

            _context.MappingData.Update(mappedItem);

            var response = new  DataMapResponse
            {
                SourceId = mappedItem.Sourceid,
                SourceName = mappedItem.Sourcename,
                TargetId = mappedItem.Targetid,
                TargetName = mappedItem.Targetname
            };

            await _changeHistoryRepository.SaveHistory(dataTypeId, HistoryAction.Change, await _commonRepository.GetHistoryOriginFromMappingDirection(direction),_userId, mappedItemHistory, response, siteId, _changeHistoryHelper);

            return response;
        }
    }
}
