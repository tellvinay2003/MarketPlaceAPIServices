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
    public class MappingDataRepository : BaseRepository, IMappingDataRepository
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

        public MappingDataRepository(MarketplaceDbContext context, ICommonRepository commonRepository, IEnumerable<IChangeHistoryHelper> changeHistoryHelpers, IChangeHistoryRepository changeHistoryRepository) : base(context)
        {
            _commonRepository = commonRepository;
             _changeHistoryHelper = changeHistoryHelpers.FirstOrDefault(a=> a is MappingDataChangeHistoryHelper);
            _changeHistoryRepository = changeHistoryRepository;
        }
        public async Task<bool> DeleteMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            var mappedItem = _context.MappingData.FirstOrDefault(md => md.Datatypeid == dataMappingTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && md.Sourceid == sourceId);

            if(mappedItem ==null)
                return false;

            //Block to remove child items and links in case item being deleted is a parent
            var childItemLinks = (from md in _context.MappingData  
            join mdl in _context.MappingDataLink on md.Sourceid equals mdl.Parentdataid
            where md.Datatypeid == dataMappingTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && md.Sourceid == sourceId
            select mdl).ToList();

            if(childItemLinks !=null)
            {
                var childItems = _context.MappingData.Where(a=>childItemLinks.Select(q=> q.Mappingdataid).ToList().Contains(a.Mappingdataid)).Select(w=>w).ToList();
                _context.MappingDataLink.RemoveRange(childItemLinks);
                _context.MappingData.RemoveRange(childItems);
            }
            // End of Block to remove child items and links in case item being deleted is a parent

            //start of block to remove data from links in case item being removed is a child of some other data
            var linkToRemove = _context.MappingDataLink.FirstOrDefault(a=>a.Mappingdataid == mappedItem.Mappingdataid);

            if(linkToRemove !=null)
            {
                 _context.MappingDataLink.Remove(linkToRemove);
            }

            //end of block to remove data from links in case item being removed is a child of some other data

            _context.MappingData.Remove(mappedItem);
            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(dataMappingTypeId, HistoryAction.Delete, await _commonRepository.GetHistoryOriginFromMappingDirection(direction),_userId, mappedItem, null, siteId, _changeHistoryHelper);

            return true;
        }

        public async Task<IEnumerable<DataMapResponse>> GetMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            return _context.MappingData.Where(md => md.Datatypeid == dataMappingTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId).Select(a=> new DataMapResponse{
                SourceId = a.Sourceid,
                TargetId = a.Targetid,
                SourceName = a.Sourcename,
                TargetName = a.Targetname,
                MappingDataId=a.Mappingdataid  
            }).ToList();            
        }

        public async Task<DataMapResponse> GetMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            return _context.MappingData.Where(md => md.Datatypeid == dataMappingTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && md.Sourceid== sourceId).Select(a=> new DataMapResponse{
                SourceId = a.Sourceid,
                TargetId = a.Targetid,
                SourceName = a.Sourcename,
                TargetName = a.Targetname,
                MappingDataId=a.Mappingdataid
            }).FirstOrDefault(); 
        }

        public async Task<DataMapResponse> InsertMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId, DataMap request)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            MappingData data = new MappingData
            {
                Mappingdirectionid = directionId,
                Datatypeid = dataMappingTypeId,
                Siteid = siteId,
                Sourceid = sourceId,
                Sourcename = request.SourceName,
                Targetid = request.TargetId,
                Targetname = request.TargetName
            };

            _context.MappingData.Add(data);
            _context.SaveChanges();

            await _changeHistoryRepository.SaveHistory(dataMappingTypeId, HistoryAction.Add, await _commonRepository.GetHistoryOriginFromMappingDirection(direction),_userId, data, null,siteId, _changeHistoryHelper);

            return new DataMapResponse{                
                SourceId = sourceId,
                SourceName = request.SourceName,
                TargetId = request.TargetId,
                TargetName = request.TargetName,
                MappingDataId=data.Mappingdataid
            };
        }

        public async Task<DataMapResponse> UpdateMappedData(Entities.MappingDirection direction, int dataMappingTypeId, Guid siteId, int sourceId, DataMap request)
        {
            var directionId = await _commonRepository.GetMappingDirectionId(direction);
            MappingData mappedItem=null;
            ///MappingDataId passed only by Region Type Mapping,rest of the flows will use sourceId
            if(request.MappingDataId==Guid.Empty)
            {
               mappedItem = _context.MappingData.FirstOrDefault(md => md.Datatypeid == dataMappingTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && md.Sourceid== sourceId);
            }
            else
            {
               mappedItem = _context.MappingData.FirstOrDefault(md => md.Datatypeid == dataMappingTypeId && md.Mappingdirectionid == directionId && md.Siteid == siteId && md.Mappingdataid== request.MappingDataId);
            }
            if(mappedItem == null)
                return null;

            var mappedItemHistory = new Models.MappingData{
            Sourceid = mappedItem.Sourceid,
            Sourcename = mappedItem.Sourcename,
            Targetid = mappedItem.Targetid,
            Targetname = mappedItem.Targetname
            };

            mappedItem.Sourcename = request.SourceName;
            mappedItem.Targetid = request.TargetId;
            mappedItem.Targetname = request.TargetName;
            mappedItem.Sourceid= sourceId;

            _context.MappingData.Update(mappedItem);
            _context.SaveChanges();

            var response = new DataMapResponse{                
                SourceId = sourceId,
                SourceName = request.SourceName,
                TargetId = request.TargetId,
                TargetName = request.TargetName,
                MappingDataId=request.MappingDataId
            };

           await _changeHistoryRepository.SaveHistory(dataMappingTypeId, HistoryAction.Change, await _commonRepository.GetHistoryOriginFromMappingDirection(direction),_userId, mappedItemHistory, response, siteId, _changeHistoryHelper);

            return response; 

        }
    }
}
