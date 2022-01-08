using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.UtilityService;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Booking;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketPlaceService.BLL
{
    public class BookingService : IBookingService
    {
        private const string SERVICE_NAME = "BookingService";
        private readonly IBookingRepository _bookingRepository;
        private readonly IAPIManagerService _apiManagerService;
        private readonly IPricingService _pricingService;
        private readonly IChangesDetectedService _changesDetectedService;
        private readonly ILogger<BookingService> _logger;
        private readonly ICommonService _commonService;
        private readonly ICommonRepository _commonRepository;
        private readonly IPricingRepository _pricingRepository;
        private readonly IMappingDataService _mappingService;
        private Guid _traceId;
        public Guid TraceId
        {
            get
            {
                if (_traceId == Guid.Empty)
                    _traceId = Guid.NewGuid();
                return _traceId;
            }
            set
            {
                _traceId = value;
            }
        }

        public BookingService(IBookingRepository bookingRepository, ILogger<BookingService> logger, IAPIManagerService apiManagerService, IPricingService pricingService, IChangesDetectedService changesDetectedService, ICommonService commonService, ICommonRepository commonRepository, IPricingRepository pricingRepository, IMappingDataService mappingService)
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _apiManagerService = apiManagerService;
            _pricingService = pricingService;
            _changesDetectedService = changesDetectedService;
            _commonService = commonService;
            _commonRepository = commonRepository;
            _pricingRepository = pricingRepository;
            _mappingService = mappingService;
        }

        //This method will be called by the ProcessSubscriberBooking method in the BookingService in the TSAPI. This will only queue the request with the booking Info JSON.
        public async Task<QueueSubscriberBookingResponse> QueueSubscriberBooking(QueueSubscriberBookingRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "QueueSubscriberBooking", SERVICE_NAME, TraceId);
            var response = new QueueSubscriberBookingResponse();
            var watch = Stopwatch.StartNew();
            bool insertmarketplaceBookingRecord = false;
            var marketplaceBookingId =await  _bookingRepository.GetMarketplaceBookingId(request.BookingId, request.SubscriberSiteId);
            if (marketplaceBookingId == Guid.Empty)
                insertmarketplaceBookingRecord = true;
            var result = await _bookingRepository.QueueSubscriberBooking(request.SubscriberSiteId, request.BookingId, TraceId, insertmarketplaceBookingRecord);
            if (result)
                response.IsSuccess = true;
            else
                response.IsSuccess = false;         
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "QueueSubscriberBooking", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "QueueSubscriberBooking", SERVICE_NAME, TraceId);
            return response;
        }

        public async Task<ProcessSubscriberBookingResponse> ProcessSubscriberBooking(ProcessSubscriberBookingRequest request)
        {
            var response = new ProcessSubscriberBookingResponse
            {
                IsSuccess = true
            };
            LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessSubscriberBooking", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var marketplaceBookingPushQueueData = await _bookingRepository.GetMarketplaceBookingPushQueueRecord(request.BookingId, request.SubscriberSiteId); //todo not needed

            var bookingInfo = await GetBookingInfo(request);

            var subscriberId = await _pricingService.GetSubscriberId(request.SubscriberSiteId, bookingInfo.OrganisationId);

            if(subscriberId == Guid.Empty)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessSubscriberBooking", SERVICE_NAME, TraceId, new Exception("SubscriberId not found for organisation."));
                response.IsSuccess = false;
                response.ErrorText = "SubscriberId not found for organisation.";
                return response;
            }

            var siteServiceData = await _bookingRepository.GetSiteServiceData(bookingInfo.BookedServices.Select(a => a.MarketplaceProductId).ToList(), subscriberId);
            UpdateSubscriberDataToPublisherData(bookingInfo, siteServiceData);

            //todo check for access
            var marketplaceBookingId = await _bookingRepository.GetMarketplaceBookingId(request.BookingId, request.SubscriberSiteId);
            await _bookingRepository.UpdateMarketPlaceBooking(marketplaceBookingId,bookingInfo, subscriberId);

            bookingInfo.Notes = await FilterAndMapMarketplaceNotes(bookingInfo.Notes, request.SubscriberSiteId);

            //split booking into publisher booking based on siteid pick up publisher bsed on first service (if multiple publisher per site )
            var splitBookingInfo = GetSplitBookingDataByPublisherSite(bookingInfo, siteServiceData, request.SubscriberSiteId);
            if(!splitBookingInfo.Any())
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessSubscriberBooking", SERVICE_NAME, TraceId, new Exception("SplitBooking List is empty"));
                return null;
            }

            foreach(var booking in splitBookingInfo)
            {
                try
                {
                    //filter out notes.

                    var siteBookingInfo = JsonConvert.SerializeObject(booking.Item2);
                    var siteBookingid = await _bookingRepository.GetSiteBookingId(marketplaceBookingId, booking.Item1);
                    var bookingInfoToBeQueued = booking.Item2;
                    if(siteBookingid!=null)
                    {
                        var sitebooking = await _bookingRepository.GetSiteBookingData((Guid)siteBookingid);
                        if (sitebooking.Bookingdata == siteBookingInfo)
                            continue;

                        var bookinginfoDiff = _commonService.GetJsonDifference(sitebooking.Bookingdata, siteBookingInfo, JsonType.SubscriberBooking);
                        bookingInfoToBeQueued = JsonConvert.DeserializeObject<BookingInfoResponse>(bookinginfoDiff);                        
                        await _bookingRepository.UpdateSubscriberBookingInfoAndDiff(siteBookingInfo, bookinginfoDiff, siteBookingid.Value);
                        _bookingRepository.QueueSiteBookingRecord(booking.Item1, request.SubscriberSiteId, bookingInfoToBeQueued, TraceId, marketplaceBookingPushQueueData.MarketplaceBookingPushQueueId, siteBookingid.Value);
                    }
                    else
                    {                        
                       siteBookingid = await _bookingRepository.InsertIntoSiteBooking(siteBookingInfo, booking.Item1, marketplaceBookingId);
                        _bookingRepository.QueueSiteBookingRecord(booking.Item1, request.SubscriberSiteId, bookingInfoToBeQueued, TraceId, marketplaceBookingPushQueueData.MarketplaceBookingPushQueueId, siteBookingid.Value);
                    }

                    _bookingRepository.InsertSiteBookingPublisherData(siteBookingid.Value, siteServiceData.Where(a=>a.SiteId == booking.Item1).ToList());

                    await _bookingRepository.UpdateSiteBookingStatusId(siteBookingid.Value, (int)BookingStatus.WaitingForCallbackFromPublisher);
                    
                }
                catch(Exception e)
                {
                    LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessSubscriberBooking", SERVICE_NAME, TraceId, new Exception($"Error when queueing split booking. : {e.ToString()}"));
                }
            }

            //todo create publsiherbookingjobs for each site.

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "ProcessSubscriberBooking", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSubscriberBooking", SERVICE_NAME, TraceId);

            return response;
        }      
        
        private async  Task<List<BookingNote>> FilterAndMapMarketplaceNotes(List<BookingNote> notes, Guid subscriberSiteId)
        {
            if (notes == null || !notes.Any())
                return notes;

            var noteTypeMappingSiteToMp = await _mappingService.GetMappedData(MappingDirection.MpToSite, 13, subscriberSiteId);
            var noteStatusMappingSiteToMp = await _mappingService.GetMappedData(MappingDirection.MpToSite, 11, subscriberSiteId);

            notes = notes.Where(a => noteTypeMappingSiteToMp.Select(q => q.TargetId).ToList().Contains(a.NoteTypeId)).Where(b => noteStatusMappingSiteToMp.Select(r => r.TargetId).ToList().Contains(b.NoteStatusId)).ToList();

            foreach(var note in notes)
            {
                note.NoteTypeId = noteTypeMappingSiteToMp.FirstOrDefault(a => a.TargetId == note.NoteTypeId).SourceId;
                note.NoteStatusId = noteStatusMappingSiteToMp.FirstOrDefault(a => a.TargetId == note.NoteStatusId).SourceId;
            }

            return notes;
        }

        private async Task<List<BookingNote>> FilterAndMapPublisherNotes(List<BookingNote> notes, Guid publisherSiteId)
        {
            
            var noteTypeMappingMpToSite = await _mappingService.GetMappedData(MappingDirection.SiteToMp, 13, publisherSiteId);
            
            var noteStatusMappingMpToSite = await _mappingService.GetMappedData(MappingDirection.SiteToMp, 11, publisherSiteId);

             var notesToUse = notes.Where(a => noteTypeMappingMpToSite.Select(q => q.TargetId).ToList().Contains(a.NoteTypeId)).Where(b => noteStatusMappingMpToSite.Select(r => r.TargetId).ToList().Contains(b.NoteStatusId)).ToList();

            if (notesToUse == null || !notesToUse.Any())
                return null;

            return notesToUse.Select(a=> new BookingNote
            {
                Attachment = a.Attachment,
                AttachmentName = a.AttachmentName,
                NoteEndDate = a.NoteEndDate,
                NoteEventDate = a.NoteEventDate,
                NoteId = a.NoteId,
                Subject = a.Subject,
                Text = a.Text,
                NoteStatusId = noteStatusMappingMpToSite.FirstOrDefault(q => q.TargetId == a.NoteStatusId).SourceId,
                NoteTypeId = noteTypeMappingMpToSite.FirstOrDefault(q => q.TargetId == a.NoteTypeId).SourceId
            }).ToList();
        }

        public async Task<ProcessSiteBookingResponse> ProcessSiteBooking(ProcessSiteBookingRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessQueuedSiteBooking", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();
            //apply mapping MP-Publisher mapping
            //pick up client data based on map
            // pick up defaults and mapping
            //pick up priceid from btpt in pricecode
            //create final req and store it 
            //call site quepublisher booking
            request.BookingDetails.SiteBookingId = request.SiteBookingId;
            var bookingInfoResponseString = await _apiManagerService.PostResponseAsync(request.BookingDetails, TravelStudioControllers.Booking, "QueuePublisherBooking", null, null, EntityType.Site, request.PublisherSiteId);
            //After queuing on the publisher end i.e. After making api call update the site booking status to WaitingForCallbackFromPublisher
            await _bookingRepository.UpdateSiteBookingStatusId(request.SiteBookingId,(int)BookingStatus.WaitingForCallbackFromPublisher);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "ProcessSiteBooking", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSiteBooking", SERVICE_NAME, TraceId);

            return new ProcessSiteBookingResponse();
        }

        private async Task<BookingInfoResponse> GetBookingInfo(ProcessSubscriberBookingRequest request)
        {
            BookingInfoRequest bookingInfoRequest = new BookingInfoRequest
            {
                BookingId = request.BookingId
            };

            var bookingInfoResponseString = await _apiManagerService.PostResponseAsync(bookingInfoRequest, TravelStudioControllers.Booking, "GetSubscriberBookingInfo", null, null, EntityType.Site, request.SubscriberSiteId);

            if (string.IsNullOrEmpty(bookingInfoResponseString))
                return null;

            return JsonConvert.DeserializeObject<Response<BookingInfoResponse>>(bookingInfoResponseString).ResponseMessage;
        }

        private async void UpdateSubscriberDataToPublisherData(BookingInfoResponse bookingInfo, List<SiteServiceData> siteServiceData)
        {
            if (bookingInfo == null || bookingInfo.BookedServices == null || !bookingInfo.BookedServices.Any())
                return;

            foreach (var service in bookingInfo.BookedServices)
            {
                var siteService = siteServiceData.FirstOrDefault(a => a.MarketplaceProductId == service.MarketplaceProductId);
                service.ServiceId = siteService.MPServiceId;
                service.PublisherSiteId = siteService.SiteId;
                service.ProductTypeId = _commonRepository.GetProductTypeId(service.MarketplaceProductId).Result;
            }
        }

        private List<Tuple<Guid, BookingInfoResponse>> GetSplitBookingDataByPublisherSite(BookingInfoResponse bookinginfo, List<SiteServiceData> siteServiceData, Guid subscriberSiteId)
        {
            List<Tuple<Guid, BookingInfoResponse>> response = new List<Tuple<Guid, BookingInfoResponse>>();
            var distinctPublisherSites = siteServiceData.Select(a => a.SiteId).Distinct().ToList();

            foreach (var site in distinctPublisherSites)
            {
                var bookingInfoForSite = GetBookingInfoForSite(bookinginfo, siteServiceData, site, subscriberSiteId);
                response.Add(new Tuple<Guid, BookingInfoResponse>(site, bookingInfoForSite));
            }

            return response;
        }

        private BookingInfoResponse GetBookingInfoForSite(BookingInfoResponse bookinginfo, List<SiteServiceData> siteServiceData, Guid siteId, Guid subscriberSiteId)
        {
            var bookedServices = bookinginfo.BookedServices.Where(a => a.PublisherSiteId == siteId).ToList();

            var siteToMpDirectionId = _commonRepository.GetMappingDirectionId(MappingDirection.SiteToMp).Result;
            var mpToSiteDirectionId = _commonRepository.GetMappingDirectionId(MappingDirection.MpToSite).Result;

            var siteToMpMapping = _bookingRepository.GetBookedOptionStatusMapping(subscriberSiteId,  siteToMpDirectionId).Result;
            var mpToSiteMapping = _bookingRepository.GetBookedOptionStatusMapping(siteId, mpToSiteDirectionId).Result;

            foreach(var service in bookedServices)
            {
                service.ProductTypeId =  _commonRepository.GetProductTypeId(service.MarketplaceProductId).Result;
                foreach(var option in service.BookedOptions)
                {
                    var mpId = siteToMpMapping.FirstOrDefault(a=>a.Sourceid == option.Status).Targetid;
                    option.Status = mpToSiteMapping.FirstOrDefault(a=>a.Sourceid == mpId).Targetid;
                }
            }

            var assignedPassengers = bookedServices.SelectMany(a => a.BookedOptions).SelectMany(q => q.AssignedPassengers).Distinct().ToList();

            var passengers = bookinginfo.BookingPassengers.Where(a => assignedPassengers.Contains(a.PassengerId)).ToList();

            var notes = FilterAndMapPublisherNotes(bookinginfo.Notes, siteId).Result;

            BookingInfoResponse response = new BookingInfoResponse
            {
                BookedServices = bookedServices,
                BookingPassengers = passengers,
                BookingId = bookinginfo.BookingId,
                BookingReferenceNumber = bookinginfo.BookingReferenceNumber,
                CurrencyISOCode = bookinginfo.CurrencyISOCode,
                ClientId = siteServiceData.FirstOrDefault(a => a.SiteId == siteId).AgentId,
                BookingPrefixId = siteServiceData.FirstOrDefault(a => a.SiteId == siteId).BookingPrefixId.HasValue? siteServiceData.FirstOrDefault(a => a.SiteId == siteId).BookingPrefixId.Value : 0,
                BookingOwnerId = siteServiceData.FirstOrDefault(a => a.SiteId == siteId).BookingOwnerId.HasValue ? siteServiceData.FirstOrDefault(a => a.SiteId == siteId).BookingOwnerId.Value : 0,
                BookingName = bookinginfo.BookingName,
                Notes = notes
            };

            return response;
        }

        

        public async Task<ProcessSubscriberCallbackResponse> ProcessSubscriberCallback(ProcessSubscriberCallbackRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessSubscriberCallback", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var siteServiceData = await _bookingRepository.UpdateBookingStatusOnSubscriberCallback(request);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "ProcessSubscriberCallback", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSubscriberCallback", SERVICE_NAME, TraceId);

            return await Task.FromResult(new ProcessSubscriberCallbackResponse());
        }


        public async Task<ProcessBookingUpdateFromPublisherResponse> ProcessBookingUpdateFromPublisher(ProcessBookingUpdateFromPublisherRequest request)
        {   //if bookingid is null no need to make the call. update sitebooking with job note.
            string publisherBookingInfoStr = null;
            PublisherBookingInfo publisherBookingInfo;
            if(request.PublisherBookingId != 0)
            {           
                publisherBookingInfoStr = await GetPublisherBookingInfo(request);
            }

            var bookingName = "";
            var publisherBookingReference="";
            var bookingInfoDiff = string.Empty;
            publisherBookingInfo = publisherBookingInfoStr!=null? JsonConvert.DeserializeObject<Response<PublisherBookingInfo>>(publisherBookingInfoStr).ResponseMessage:null;
                               
            if(!string.IsNullOrEmpty(publisherBookingInfoStr) && publisherBookingInfo!=null)
            {
                publisherBookingInfo = MapBookedOptionStatusId(publisherBookingInfo, request.PublisherSiteId, request.SiteBookingId);
                publisherBookingInfoStr = JsonConvert.SerializeObject(publisherBookingInfo);
                var savedPublisherBookingInfoStr = await _bookingRepository.GetSavedPublisherBookingInfo(request);
                bookingName = publisherBookingInfo.BookingName!=null?publisherBookingInfo.BookingName.Trim():"";
                publisherBookingReference = publisherBookingInfo.BookingReferenceNumber;
                if (!string.IsNullOrEmpty(savedPublisherBookingInfoStr))
                {
                    bookingInfoDiff = _commonService.GetJsonDifference(savedPublisherBookingInfoStr, publisherBookingInfoStr, JsonType.PublisherBooking);
                }           
            }   
            await _bookingRepository.UpdatePublisherInfoAndDiff(publisherBookingInfoStr, bookingInfoDiff, request.SiteBookingId,bookingName,publisherBookingReference,request.JobNote); //site bookign history insert
            
           
            //await _bookingRepository.UpdateSiteBookingStatusId(request.SiteBookingId,(int)BookingStatus.WaitingForCallbackFromSubscriber);
            
            var siteBookingData = _bookingRepository.GetSiteBookingData(request.SiteBookingId).Result;

            var marketplaceBookingData = _bookingRepository.GetMarketplaceBookingData((Guid)siteBookingData.Marketplacebookingid).Result;

            var queueBookingUpdateFromPublisher =  new QueueBookingUpdateFromPublisherRequest{
                BookingData = string.IsNullOrEmpty(bookingInfoDiff) ? publisherBookingInfoStr : bookingInfoDiff,
                SiteBookingId = siteBookingData.Sitebookingid,
                BookingVersion = siteBookingData.Bookingversion.HasValue ? siteBookingData.Bookingversion.Value : 1,
                SubscriberbookingId = (int)marketplaceBookingData.SubscriberBookingId,
                SubscriberBookingRef = marketplaceBookingData.SubscriberBookingRef,
                JobNote = request.JobNote
            };

            await _apiManagerService.PostResponseAsync(queueBookingUpdateFromPublisher, TravelStudioControllers.Booking, "QueueBookingUpdateFromPublisher", null, null, EntityType.Site, (Guid)marketplaceBookingData.Subscribersiteid);
            
            return new ProcessBookingUpdateFromPublisherResponse();
        }

        private PublisherBookingInfo MapBookedOptionStatusId(PublisherBookingInfo publisherBookingInfo, Guid publisherSiteId, Guid siteBookingId)
        {
            var response = new PublisherBookingInfo();
            response = publisherBookingInfo;
            var siteToMpDirectionId = _commonRepository.GetMappingDirectionId(MappingDirection.SiteToMp).Result;
            var mpToSiteDirectionId = _commonRepository.GetMappingDirectionId(MappingDirection.MpToSite).Result;

            var siteToMpMapping = _bookingRepository.GetBookedOptionStatusMapping(publisherSiteId, siteToMpDirectionId).Result;
            var mpToSiteMapping = _bookingRepository.GetSubscriberBookedOptionStatusMapping(siteBookingId, mpToSiteDirectionId).Result;

            foreach(var service in response.BookedServices)
            {
                foreach(var option in service.BookedOptions)
                {
                    var mpId = siteToMpMapping.FirstOrDefault(a=>a.Sourceid == option.BookedOptionStatusId).Targetid;
                    option.BookedOptionStatusId = mpToSiteMapping.FirstOrDefault(a=>a.Sourceid == mpId).Targetid;
                }
            }

           return response;
        }

        private async Task<string> GetPublisherBookingInfo(ProcessBookingUpdateFromPublisherRequest request)
        {
            BookingInfoRequest publisherBookingInfoRequest = new BookingInfoRequest
            {
                BookingId = request.PublisherBookingId
            };

            var publisherBookingInfoResponseString = await _apiManagerService.PostResponseAsync(publisherBookingInfoRequest, TravelStudioControllers.Booking, "GetPublisherBookingInfo", null, null, EntityType.Site, request.PublisherSiteId);

            if (string.IsNullOrEmpty(publisherBookingInfoResponseString))
                return null;

            return publisherBookingInfoResponseString;
        } 





        public async Task<IEnumerable<string>> GetBookingReference(string bookingReference, int limit)
        {   LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingReference", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var result = await _bookingRepository.GetBookingReference(bookingReference,limit);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "GetBookingReference", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingReference", SERVICE_NAME, TraceId);

            return result;
        }


            public async Task<BookingSearchResponse> BookingSearch(BookingSearchRequest request)
            {
                  LoggingHelper.LogInfo(_logger, LogType.Start, "BookingSearch", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var result = await _bookingRepository.BookingSearch(request);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "BookingSearch", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "BookingSearch", SERVICE_NAME, TraceId);

            return result;
            }    




        public async Task<GetBookingStatusResponse> GetBookingStatus()
        {   LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingStatus", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var result = await _bookingRepository.GetBookingStatus();

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "GetBookingStatus", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingStatus", SERVICE_NAME, TraceId);

            return result;
        }



        public async Task<GetMpBookingInfoResponse> GetMpSubscriberBookingInfo(GetMpBookingInfoRequest request)
        {   LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpSubscriberBookingInfo", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var result = await _bookingRepository.GetMpSubscriberBookingInfo(request);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "GetMpSubscriberBookingInfo", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMpSubscriberBookingInfo", SERVICE_NAME, TraceId);

            return result;
        }

        public async Task<GetMpBookingInfoResponse> GetMpPublisherBookingInfo(GetMpBookingInfoRequest request)
        {   LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpPublisherBookingInfo", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var result = await _bookingRepository.GetMpPublisherBookingInfo(request);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "GetMpPublisherBookingInfo", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMpPublisherBookingInfo", SERVICE_NAME, TraceId);

            return result;
        }

       public async Task<GetMpBookingHistoryInfoResponse> GetMpPublisherBookingHistoryInfo(GetMpBookingHistoryInfoRequest request)
        {   LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpPublisherBookingHistoryInfo", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var result = await _bookingRepository.GetMpPublisherBookingHistoryInfo(request);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "GetMpPublisherBookingHistoryInfo", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMpPublisherBookingHistoryInfo", SERVICE_NAME, TraceId);

            return result;
        }

       public async Task<GetMpBookingHistoryInfoResponse> GetMpSubscriberBookingHistoryInfo(GetMpBookingHistoryInfoRequest request)
        {   LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpSubscriberBookingHistoryInfo", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();

            var result = await _bookingRepository.GetMpSubscriberBookingHistoryInfo(request);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "GetMpSubscriberBookingHistoryInfo", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMpSubscriberBookingHistoryInfo", SERVICE_NAME, TraceId);

            return result;
        }

          public async Task<GetMpBookingJsonDataResponse> GetMpBookingJsonData(GetMpBookingJsonDataRequest request)
        {   LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpBookingJsonData", SERVICE_NAME, TraceId);
            var watch = Stopwatch.StartNew();
            var result = (GetMpBookingJsonDataResponse) null;

             if(request.EntityType == EntityType.Publisher)
             {  
                 if(request.RowId != null)
                 {
            result = await _bookingRepository.GetMpPublisherBookingJsonData(request);
                 }
                 else if(request.BookingId != null)
                   {
                   result = await _bookingRepository.GetLatestMpPublisherBookingJsonData(request);
                   }
             }
             else if (request.EntityType == EntityType.Subscriber)

             {
                  if(request.RowId != null)
                 {
            result = await _bookingRepository.GetMpSubscriberBookingJsonData(request);
                 }
                 else if(request.BookingId != null)
                   {
                   result = await _bookingRepository.GetLatestMpSubscriberBookingJsonData(request);
                   }
             }

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPAPI, "GetMpBookingJsonData", SERVICE_NAME, TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetMpBookingJsonData", SERVICE_NAME, TraceId);

            return result;
        }


        public async Task<ApplicableRulesResponse> GetServiceApplicableRules(ApplicableRulesRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceApplicableRules", "BookingService", TraceId);

            //get organisationId from bookingId
            var organisationId = request.GetApplicableRulesRequestData.OrganisationId;

            var subscriberId = await _pricingService.GetSubscriberId(request.GetApplicableRulesRequestData.SiteId, organisationId);

            List<SiteServiceData> siteServiceData = _pricingRepository.GetSiteServiceData(request.GetApplicableRulesRequestData.ServiceInfo.Select(a => a.MarketplaceProductId).ToList(), subscriberId);

            //check if service has valid subscriber or not, if yes remove the service object from request. -- Deepraj
            //UpdateServiceAccess(request.ServiceInfo);

            UpdatePublisherDataForRules(request.GetApplicableRulesRequestData, siteServiceData); //replaces the serviceids with those of the publisher. also sets the site it for earch service.

            //for each site make a Calculate booking price call.

            var watch = Stopwatch.StartNew();  //TODO pass entity type and entity id 5,6th param, sepration for different publishers
            //var result = await _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Pricing, "GetBookingPriceFromTs", null, null, 0, request.CalculateBookingPriceRequestData.ServiceInfo.FirstOrDefault().SiteId);
            //var response = JsonConvert.DeserializeObject<Response<CalculateBookingPriceResponse>>(result);
            var response = SplitAndPostRequestToPublishersForApplicableRules(request, siteServiceData);

            if (response == null)
            {
                return null;
            }

            //UpdateSubscriberData(response.ResponseMessage, siteServiceData);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetBookingPrice", "APIManager", TraceId, watch.ElapsedMilliseconds);

            LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingPrice", "PricingService", TraceId);

            return response;
        }


        private ApplicableRulesResponse SplitAndPostRequestToPublishersForApplicableRules(ApplicableRulesRequest request, List<SiteServiceData> siteServiceData)
        {
            List<Response<ApplicableRulesResponse>> publisherResponses = new List<Response<ApplicableRulesResponse>>();
            ApplicableRulesResponse response = new ApplicableRulesResponse
            {
                Rules = new List<ApplicableServiceRuleResponse>()
            };
            var siteIds = request.GetApplicableRulesRequestData.ServiceInfo.Select(a => a.SiteId).Distinct().ToList();
            List<Task<string>> tasks = new List<Task<string>>();
            List<Tuple<Guid, Task<string>>> taskList = new List<Tuple<Guid, Task<string>>>();
            foreach (var site in siteIds)
            {
                var services = request.GetApplicableRulesRequestData.ServiceInfo.Where(a => a.SiteId == site).ToList();
                if (services != null && services.Count > 0)
                {
                    var req = GetApplicableRulesRequestForSite(request, services);
                    var task = _apiManagerService.PostResponseAsync(req, TravelStudioControllers.Booking, "GetApplicableRulesFromTs", null, null, 0, site);
                    tasks.Add(task);
                    taskList.Add(new Tuple<Guid, Task<string>>(site, task));
                }
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var taskItem in taskList)
            {
                var task = taskItem.Item2;
                try
                {
                    var result = JsonConvert.DeserializeObject<Response<ApplicableRulesResponse>>(task.Result);

                    if (result == null || result.ResponseMessage == null || result.ResponseMessage.Rules == null || !result.ResponseMessage.Rules.Any())
                        continue;

                    UpdateSubscriberDataForRules(result.ResponseMessage, siteServiceData, taskItem.Item1);

                    response.Rules.AddRange(result.ResponseMessage.Rules);
                }
                catch (Exception)
                {
                    LoggingHelper.LogError(_logger, ExceptionType.System, "SplitAndPostRequestToPublishersForGetBookingPrice", "PricingService", TraceId, new Exception($"Deserialisation of GetBookingPriceResponse From Publisher failed. Response = {task.Result}"));
                }
            }

            //var servicesWithNoAccess = request.CalculateBookingPriceRequestData.ServiceInfo.Where(a => a.isAccessNotAllowed).ToList();

            //foreach (var serviceWithNoAccess in servicesWithNoAccess)
            //{
            //    var siteService = siteServiceData.FirstOrDefault(a => a.MPServiceId == serviceWithNoAccess.ServiceID);
            //    response.ResponseMessage.CalculateBookingPriceResponseData.ServicePriceInfo.Add(new CalcBookingPriceServicePrice
            //    {
            //        ServiceID = siteService.TSServiceId,
            //        isAccessNotAllowed = true,
            //    });
            //}

            return response;
        }


        private ServiceRuleResponse SplitAndPostRequestToPublishersForServiceRules(ServiceRuleRequest request, List<SiteServiceData> siteServiceData)
        {
            List<Response<ServiceRuleResponse>> publisherResponses = new List<Response<ServiceRuleResponse>>();
            ServiceRuleResponse response = new ServiceRuleResponse
            {
                ApplicableRulesResponse = new GetApplicableRulesResponseData()
            };
            var siteIds = request.GetBookingApplicableRules.ServiceInfo.Select(a => a.SiteId).Distinct().ToList();
            List<Task<string>> tasks = new List<Task<string>>();
            List<Tuple<Guid, Task<string>>> taskList = new List<Tuple<Guid, Task<string>>>();
            foreach (var site in siteIds)
            {
                var services = request.GetBookingApplicableRules.ServiceInfo.Where(a => a.SiteId == site).ToList();
                if (services != null && services.Count > 0)
                {
                    var req = GetServiceRulesRequestForSite(request, services);
                    var task = _apiManagerService.PostResponseAsync(req, TravelStudioControllers.Booking, "GetServiceRulesFromTs", null, null, 0, site);
                    tasks.Add(task);
                    taskList.Add(new Tuple<Guid, Task<string>>(site, task));
                }
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var taskItem in taskList)
            {
                var task = taskItem.Item2;
                try
                {
                    var result = JsonConvert.DeserializeObject<Response<ServiceRuleResponse>>(task.Result);

                    //if (result == null || result.ResponseMessage == null || result.ResponseMessage.Rules == null || !result.ResponseMessage.Rules.Any())
                    //    continue;

                    //UpdateSubscriberDataForRules(result.ResponseMessage, siteServiceData, taskItem.Item1);

                    response = result.ResponseMessage;
                }
                catch (Exception)
                {
                    LoggingHelper.LogError(_logger, ExceptionType.System, "SplitAndPostRequestToPublishersForGetBookingPrice", "PricingService", TraceId, new Exception($"Deserialisation of GetBookingPriceResponse From Publisher failed. Response = {task.Result}"));
                }
            }           

            return response;
        }


        private void UpdateSubscriberDataForRules(ApplicableRulesResponse response, List<SiteServiceData> siteServiceData, Guid siteId)
        {
            if (response == null || response.Rules == null || !response.Rules.Any())
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetApplicableRules", "BookingService", TraceId, new Exception("UpdateSubscriberDataForRules - Null data in request"));
                return;
            }

            foreach (var rule in response.Rules)
            {
               var siteService = siteServiceData.FirstOrDefault(a => a.MPServiceId == rule.ServiceId && a.SiteId == siteId);

               rule.ServiceId = siteService != null ?  siteService.TSServiceId : 0;                
            }
        }

        private ApplicableRulesRequest GetApplicableRulesRequestForSite(ApplicableRulesRequest request, List<ApplicableRuleService> services)
        {
            var finalRequest = new ApplicableRulesRequest
            {
                GetApplicableRulesRequestData = new ApplicableRulesRequestData
                {
                    ServiceInfo = services,
                    BookingTypeID = request.GetApplicableRulesRequestData.BookingTypeID,
                    PriceTypeID = request.GetApplicableRulesRequestData.PriceTypeID,
                    ClientID = request.GetApplicableRulesRequestData.ClientID > 0 ? request.GetApplicableRulesRequestData.ClientID : services.FirstOrDefault().AgentId,
                    ApplicableRuleType = request.GetApplicableRulesRequestData.ApplicableRuleType,
                    ReturnAllApplicableRules = request.GetApplicableRulesRequestData.ReturnAllApplicableRules,
                    RuleFromDate= request.GetApplicableRulesRequestData.RuleFromDate,
                    RuleToDate= request.GetApplicableRulesRequestData.RuleToDate
                }
            };

            return finalRequest;
        }

        private ServiceRuleRequest GetServiceRulesRequestForSite(ServiceRuleRequest request, List<ApplicableRuleService> services)
        {
            var finalRequest = new ServiceRuleRequest
            {
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                ServiceRuleModifier = new ServiceRuleModifier
                {
                    ReturnAllApplicableRules = request.ServiceRuleModifier.ReturnAllApplicableRules,
                    ReturnAllServiceRules = request.ServiceRuleModifier.ReturnAllServiceRules,
                    ReturnApplicableRules = true
                },
                GetBookingApplicableRules = new ApplicableRulesRequestData
                {
                    ServiceInfo = services,
                    BookingTypeID = request.GetBookingApplicableRules.BookingTypeID,
                    PriceTypeID = request.GetBookingApplicableRules.PriceTypeID,
                    ClientID = services.First().AgentId,
                    ApplicableRuleType = request.GetBookingApplicableRules.ApplicableRuleType,                    
                }                
            };

            return finalRequest;
        }

        private bool UpdatePublisherDataForRules(ApplicableRulesRequestData request, List<SiteServiceData> siteServiceData)
        {
            if (request == null || request.ServiceInfo == null)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdatePublisherDataForRules", "BookingService", TraceId, new Exception("UpdatePublisherDataForRules - Null data in request"));
                return false;
            }

            foreach (var service in request.ServiceInfo)
            {
                var siteService = siteServiceData.FirstOrDefault(a => a.MarketplaceProductId == service.MarketplaceProductId);
                siteService.TSServiceId = service.ServiceID;
                service.SiteId = siteService.SiteId;
                service.ServiceID = siteService.MPServiceId;
                service.AgentId = siteService.AgentId;
            }

            return true;
        }


        public async Task<ServiceRuleResponse> GetServiceRules(ServiceRuleRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceRules", "BookingService", TraceId);

            //get organisationId from bookingId
            var organisationId = request.OrganisationId;

            var subscriberId = await _pricingService.GetSubscriberId(request.SiteId, organisationId);

            List<SiteServiceData> siteServiceData = _pricingRepository.GetSiteServiceData(request.GetBookingApplicableRules.ServiceInfo.Select(a => a.MarketplaceProductId).ToList(), subscriberId);

            //check if service has valid subscriber or not, if yes remove the service object from request. -- Deepraj
            //UpdateServiceAccess(request.ServiceInfo);

            UpdatePublisherDataForRules(request.GetBookingApplicableRules, siteServiceData); //replaces the serviceids with those of the publisher. also sets the site it for earch service.

            //for each site make a Calculate booking price call.

            var watch = Stopwatch.StartNew();  //TODO pass entity type and entity id 5,6th param, sepration for different publishers
            //var result = await _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Pricing, "GetBookingPriceFromTs", null, null, 0, request.CalculateBookingPriceRequestData.ServiceInfo.FirstOrDefault().SiteId);
            //var response = JsonConvert.DeserializeObject<Response<CalculateBookingPriceResponse>>(result);
            var response = SplitAndPostRequestToPublishersForServiceRules(request, siteServiceData);

            if (response == null)
            {
                return null;
            }

            //UpdateSubscriberData(response.ResponseMessage, siteServiceData);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServiceRules", "APIManager", TraceId, watch.ElapsedMilliseconds);

            LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceRules", "PricingService", TraceId);

            return response;
        }
    }
}
