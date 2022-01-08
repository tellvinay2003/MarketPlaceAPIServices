using MarketPlaceService.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarketPlaceService.Entities;
using System.Linq;
using MarketPlaceService.DAL.Models;
using Newtonsoft.Json;
using MarketPlaceService.Entities.Booking;
using MarketPlaceService.DAL.Utilities;
namespace MarketPlaceService.DAL
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        private readonly IPricingRepository _pricingRepository;

        public BookingRepository(MarketplaceDbContext context, IPricingRepository pricingRepository) : base(context)
        {
            _pricingRepository = pricingRepository;
        }

        public async Task<List<SiteServiceData>> GetSiteServiceData(List<Guid> marketplaceProductIds, Guid subscriberId)
        {
            return _pricingRepository.GetSiteServiceData(marketplaceProductIds, subscriberId);
        }

        public async Task<bool> QueueSubscriberBooking(Guid subscriberSiteId, int bookingId, Guid traceId, bool insertmarketplaceBookingRecord)
        {
            
            if (insertmarketplaceBookingRecord)
            {
                var insertMpBookingJob = _context.MarketplaceBooking.Add(new MarketplaceBooking()
                {
                    Createdon = DateTime.UtcNow,
                    Bookingversion = 0,
                    Statusid = (int)BookingStatus.Syncing,
                    Subscribersiteid = subscriberSiteId,
                    SubscriberBookingId = bookingId
                });
            }

            var insertMpBookingPushQueueJob = _context.MarketplaceBookingPushQueue.Add(new MarketplaceBookingPushQueue()
            {
                Subscribersiteid = subscriberSiteId,
                Bookingid = bookingId,
                Retrycount = 0,
                Jobcreateddatetime = DateTime.UtcNow,
                Traceid = traceId,
                Jobstatusid = 1,
                Jobtypeid = insertmarketplaceBookingRecord? (short)1 : (short)2
            }); // Jobtypeid : BookingInsert(1) , BookingUpdate(2)
            var response = _context.SaveChanges();
            if (response > 0)
                return true;
            else
                return false;
        }
        public async void QueueSiteBookingRecord(Guid publisherSiteId, Guid subscriberSiteId, BookingInfoResponse bookinginfo, Guid traceId, Guid marketplaceBookingPushQueueId, Guid siteBookingId)
        {
            var bookingData = JsonConvert.SerializeObject(bookinginfo);           
             

            var siteBookingPushQueue = new SiteBookingPushQueue
            {
                Bookingdata = bookingData,
                Publisherbookingid = bookinginfo.BookingId,
                Jobcreateddatetime = DateTime.UtcNow,
                Jobstatusid = 1,
                Processingnote = string.Empty,
                Retrycount = 1,
                Publishersiteid = publisherSiteId,
                Subscribersiteid = subscriberSiteId,
                Traceid = traceId,
                Marketplacebookingpushqueueid = marketplaceBookingPushQueueId, //todo not required . incorrect.
                Jobtypeid = 1,
                Sitebookingid = siteBookingId,
                
            };
           
            _context.SiteBookingPushQueue.Add(siteBookingPushQueue);
            _context.SaveChanges();
        }

        public async Task<Guid> InsertIntoSiteBooking(string bookingData, Guid publisherSiteId, Guid marketplaceBookingId)
        {            
            var siteBooking = new SiteBooking()
            {
                Bookingdata = bookingData,
                Publishersiteid = publisherSiteId,
                Statusid =(int)BookingStatus.Syncing,
                Createdon = DateTime.UtcNow,
                Marketplacebookingid = marketplaceBookingId, 
                Processingnote = string.Empty
                
            };

            var siteBookingHistory = new SiteBookingHistory()
            {
                Bookingdata = bookingData,
                Publishersiteid = publisherSiteId,
                Statusid = (int)BookingStatus.Syncing,
                Createdon = DateTime.UtcNow,
                Marketplacebookingid = marketplaceBookingId, //todo check this
                Processingnote = string.Empty,
                Bookingversion = 1,
                Sitebooking = siteBooking
               


            };

            _context.SiteBooking.Add(siteBooking);
            _context.SiteBookingHistory.Add(siteBookingHistory);
            _context.SaveChanges();

            return siteBooking.Sitebookingid;
        }

        public async Task<MarketplaceBookingPushQueueData> GetMarketplaceBookingPushQueueRecord(int bookingId, Guid subscriberSiteId)
        {
            return _context.MarketplaceBookingPushQueue.Where(a => a.Bookingid == bookingId && a.Subscribersiteid == subscriberSiteId).Select(q => new MarketplaceBookingPushQueueData
            {
                BookingId = bookingId,
                SubscriberSiteId = subscriberSiteId,
                MarketplaceBookingPushQueueId = q.Marketplacebookingpushqueueid
            }).FirstOrDefault();
        }

        public async Task<bool> UpdateSiteBookingStatus(ProcessSiteBookingRequest request, int bookingStatusId)
        {
            Guid siteBookingId = request.BookingDetails != null ? request.BookingDetails.SiteBookingId : new Guid();
            
            var siteBooking = ( from sb in _context.SiteBooking 
                                where sb.Sitebookingid == siteBookingId
                                select sb).FirstOrDefault();

            if(siteBooking == null){
                throw new Exception("Site booking does not exist");    
            }

            siteBooking.Statusid = bookingStatusId;

            _context.SiteBooking.Update(siteBooking);
            _context.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateBookingStatusOnSubscriberCallback(ProcessSubscriberCallbackRequest request)
        {
            request.IsProcessedSuccessfully = string.IsNullOrEmpty(request.JobNote);
            /*
                1. Update site booking status to synced
                    a. If there exist no dormant job in BookingUpdateFromPublisherQueue
                2. Update marketplace booking to synced
                    a. If all site bookings are synced
                    b. If there exist no dormant job in MarketplaceBookingPushQueue
                3. Update BookingUpdateFromPublisherQueue job status to queued if its a dormant job.
                4. Update MarketplaceBookingPushQueue job status to queued if its a dormant job.
            */

            var siteBooking = (from sb in _context.SiteBooking
                               where sb.Sitebookingid == request.SiteBookingId
                               select sb).FirstOrDefault();

            if (siteBooking == null)
            {
                throw new Exception("Site booking doesnot exist");
            }

            if (request.IsProcessedSuccessfully)
            {
                // Check if there exist a dormant job for passed siteBookingID
                var dormantBookingUpdateFromPublisherQueue = (from bufpq in _context.BookingUpdateFromPublisherQueue
                                                              where bufpq.Sitebookingid == request.SiteBookingId
                                                              && bufpq.Jobstatusid == (int)MarketPlaceService.Entities.JobStatus.Dormant
                                                              select bufpq).FirstOrDefault();

                if (dormantBookingUpdateFromPublisherQueue != null)
                {
                    //If there exist dormant job, update the satus to queqed 
                    dormantBookingUpdateFromPublisherQueue.Jobstatusid = (int)MarketPlaceService.Entities.JobStatus.Queued;
                    _context.BookingUpdateFromPublisherQueue.Update(dormantBookingUpdateFromPublisherQueue);
                }
                else
                {
                    // If there is no dormant job then update siteBooking status to synced
                    siteBooking.Statusid = (int)BookingStatus.Synced;
                    _context.SiteBooking.Update(siteBooking);

                    

                    // Check if any of the siteBooking is unsynced
                    var marketplaceBookingId = siteBooking.Marketplacebookingid;
                    var unSyncedSiteBooking = _context.SiteBooking.Where(a => a.Marketplacebookingid == marketplaceBookingId && a.Sitebookingid != siteBooking.Sitebookingid).Any(q => q.Statusid != (int)BookingStatus.Synced);
                    var erroredSiteBooking = _context.SiteBooking.Where(a => a.Marketplacebookingid == marketplaceBookingId ).Any(q => q.Statusid == (int)BookingStatus.Error);
                   

                    // check if there exist any dormant job for marketplace booking
                    var dormantMarketplaceBookingPushQueue = (from mbpq in _context.MarketplaceBookingPushQueue
                                                              join mb in _context.MarketplaceBooking on mbpq.Bookingid equals mb.SubscriberBookingId
                                                              join sb in _context.SiteBooking on mb.Marketplacebookingid equals sb.Marketplacebookingid
                                                              where mb.Marketplacebookingid == siteBooking.Marketplacebookingid
                                                              && mbpq.Jobstatusid == (short)MarketPlaceService.Entities.JobStatus.Dormant
                                                              select mbpq).FirstOrDefault();

                    //TODO: if there exist unSynced site booking then enable the first one

                    // if there exist dormnat job then enable the first one
                    if (dormantMarketplaceBookingPushQueue != null)
                    {
                        dormantMarketplaceBookingPushQueue.Jobstatusid = (short)MarketPlaceService.Entities.JobStatus.Queued;
                        _context.MarketplaceBookingPushQueue.Update(dormantMarketplaceBookingPushQueue);
                    }

                    if (!unSyncedSiteBooking && dormantMarketplaceBookingPushQueue == null)
                    {
                        // if no marketplace booking dormnat job and all site bookings are synced then update marketplace booking status to synced
                        var marketplaceBooking = (from mb in _context.MarketplaceBooking
                                                  where mb.Marketplacebookingid == siteBooking.Marketplacebookingid
                                                  select mb).FirstOrDefault();

                        marketplaceBooking.Statusid = erroredSiteBooking?(int)BookingStatus.Error:(int)BookingStatus.Synced;
                        _context.MarketplaceBooking.Update(marketplaceBooking);

                        var marketplaceBookingHistory  =  new MarketplaceBookingHistory()
                                    {

                                        Marketplacebookingid = marketplaceBooking.Marketplacebookingid,
                                        Createdon = DateTime.UtcNow,
                                        Bookingname = marketplaceBooking.Bookingname,
                                        Bookingdata = marketplaceBooking.Bookingdata,
                                        Bookingversion = marketplaceBooking.Bookingversion,
                                        Statusid = marketplaceBooking.Statusid,
                                        SubscriberBookingId = marketplaceBooking.SubscriberBookingId,
                                        SubscriberBookingRef = marketplaceBooking.SubscriberBookingRef,
                                        Subscriberid = marketplaceBooking.Subscriberid,
                                        Subscribersiteid = marketplaceBooking.Subscribersiteid
                    
                                    };

                            _context.MarketplaceBookingHistory.Add(marketplaceBookingHistory);        

                    }

                    
                }
            }
            else
            {
                //TODO: Need to handle mareketplace booking status in case of error.
                siteBooking.Statusid = (int)BookingStatus.Error;
                siteBooking.Processingnote = request.JobNote;
                _context.SiteBooking.Update(siteBooking);
               
                var marketplaceBooking =  _context.MarketplaceBooking.Where(mb => mb.Marketplacebookingid == siteBooking.Marketplacebookingid).FirstOrDefault();
                marketplaceBooking.Statusid = (int)BookingStatus.Error;
                _context.MarketplaceBooking.Update(marketplaceBooking);

                 var dormantBookingUpdateFromPublisherQueue = (from bufpq in _context.BookingUpdateFromPublisherQueue
                                                              where bufpq.Sitebookingid == request.SiteBookingId
                                                              && bufpq.Jobstatusid == (int)MarketPlaceService.Entities.JobStatus.Dormant
                                                              select bufpq).FirstOrDefault();

                if (dormantBookingUpdateFromPublisherQueue != null)
                {
                    //If there exist dormant job, update the satus to queqed 
                    dormantBookingUpdateFromPublisherQueue.Jobstatusid = (int)MarketPlaceService.Entities.JobStatus.Queued;
                    _context.BookingUpdateFromPublisherQueue.Update(dormantBookingUpdateFromPublisherQueue);
                }

            }
            var siteBookingHistory = new SiteBookingHistory()
            {
                Bookingdata = siteBooking.Bookingdata,
                Publishersiteid = siteBooking.Publishersiteid,
                Statusid = siteBooking.Statusid,
                Createdon = DateTime.UtcNow,
                Marketplacebookingid = siteBooking.Marketplacebookingid.Value, //todo check this
                Processingnote = string.Empty,
                Bookingversion = siteBooking.Bookingversion,
                Publisherbookinginfo = siteBooking.Publisherbookinginfo,
                Publisherbookinginfodiff = siteBooking.Publisherbookinginfodiff,
                Subscriberbookinginfodiff = siteBooking.Subscriberbookinginfodiff,
                Sitebookingid = siteBooking.Sitebookingid
            };

            _context.SiteBookingHistory.Add(siteBookingHistory);

            _context.SaveChanges();

            return await Task.FromResult(true);
        }

        public async Task<Guid> GetMarketplaceBookingId(int bookingId, Guid subscriberSiteId)
        {
            var marketplaceBooking = _context.MarketplaceBooking.FirstOrDefault(a => a.SubscriberBookingId == bookingId && a.Subscribersiteid == subscriberSiteId);
            if (marketplaceBooking == null)
                return Guid.Empty;
            return marketplaceBooking.Marketplacebookingid;           
        }

        public async Task<Guid?> GetSiteBookingId(Guid marketplaceBookingId, Guid publisherSiteId)
        {
            var siteBooking = _context.SiteBooking.FirstOrDefault(a => a.Marketplacebookingid == marketplaceBookingId && a.Publishersiteid == publisherSiteId);
            if (siteBooking == null)
                return null;

            return siteBooking.Sitebookingid;
        }



        public async Task UpdateMarketPlaceBooking(Guid marketplaceBookingId, BookingInfoResponse bookingInfo, Guid subscriberId)
        {
            var marketplaceBookingRecord = _context.MarketplaceBooking.FirstOrDefault(a => a.Marketplacebookingid == marketplaceBookingId);
            marketplaceBookingRecord.Bookingname = bookingInfo.BookingName;
            marketplaceBookingRecord.Bookingdata = JsonConvert.SerializeObject(bookingInfo);
            marketplaceBookingRecord.SubscriberBookingRef = bookingInfo.BookingReferenceNumber;
            marketplaceBookingRecord.Subscriberid = subscriberId;
            marketplaceBookingRecord.Processedon = DateTime.UtcNow;
            marketplaceBookingRecord.Bookingversion += 1;


            //insert into history
            MarketplaceBookingHistory marketplaceBookingHistory = new MarketplaceBookingHistory
            {
                Marketplacebookingid = marketplaceBookingId,
                Createdon = DateTime.UtcNow,
                Bookingname = marketplaceBookingRecord.Bookingname,
                Bookingdata = marketplaceBookingRecord.Bookingdata,
                Bookingversion = marketplaceBookingRecord.Bookingversion,
                Statusid = marketplaceBookingRecord.Statusid,
                SubscriberBookingId = marketplaceBookingRecord.SubscriberBookingId,
                SubscriberBookingRef = marketplaceBookingRecord.SubscriberBookingRef,
                Subscriberid = marketplaceBookingRecord.Subscriberid,
                Subscribersiteid = marketplaceBookingRecord.Subscribersiteid
            };

            _context.MarketplaceBookingHistory.Add(marketplaceBookingHistory);
            _context.MarketplaceBooking.Update(marketplaceBookingRecord);
            _context.SaveChanges();
        }

 
        public async Task<string> GetSavedPublisherBookingInfo(ProcessBookingUpdateFromPublisherRequest request)
        {
           return _context.SiteBooking.FirstOrDefault(a => a.Sitebookingid == request.SiteBookingId).Publisherbookinginfo;
        }

        public async Task UpdatePublisherInfoAndDiff(string publisherBookingInfoStr, string bookingInfoDiff, Guid siteBookingId,string bookingName, string bookingReference, string jobNote)
        {
            var siteBookingRecord = _context.SiteBooking.FirstOrDefault(a => a.Sitebookingid == siteBookingId);
            siteBookingRecord.Publisherbookinginfodiff = bookingInfoDiff;
            siteBookingRecord.Publisherbookinginfo = publisherBookingInfoStr;
            siteBookingRecord.Bookingname = bookingName;
            siteBookingRecord.PublisherBookingRef = bookingReference;
            siteBookingRecord.Processingnote = jobNote;
            
            if(!String.IsNullOrEmpty(jobNote))
            {siteBookingRecord.Statusid = (int)BookingStatus.Error;}
            else
            {siteBookingRecord.Statusid = (int)BookingStatus.WaitingForCallbackFromSubscriber;}

            _context.SiteBooking.Update(siteBookingRecord);
            _context.SaveChanges();

            var siteBookingHistory = new SiteBookingHistory()
            {
                Bookingdata = siteBookingRecord.Bookingdata,
                Publishersiteid = siteBookingRecord.Publishersiteid,
                Statusid = siteBookingRecord.Statusid,
                Createdon = DateTime.UtcNow,
                Marketplacebookingid = siteBookingRecord.Marketplacebookingid.Value, //todo check this
                Processingnote = jobNote,
                Bookingversion = siteBookingRecord.Bookingversion,
                Publisherbookinginfo = siteBookingRecord.Publisherbookinginfo,
                Publisherbookinginfodiff = siteBookingRecord.Publisherbookinginfodiff,
                Subscriberbookinginfodiff = siteBookingRecord.Subscriberbookinginfodiff,
                Sitebookingid = siteBookingRecord.Sitebookingid,
                
            };

            _context.SiteBookingHistory.Add(siteBookingHistory);
            _context.SaveChanges();
        }

        public async Task<bool> UpdateSubscriberBookingInfoAndDiff(string bookingInfo, string bookingInfoDiff, Guid siteBookingId)
        {
            var siteBookingRecord = _context.SiteBooking.FirstOrDefault(a => a.Sitebookingid == siteBookingId);
            siteBookingRecord.Subscriberbookinginfodiff = bookingInfoDiff;
            siteBookingRecord.Bookingdata = bookingInfo;

            var siteBookingHistory = new SiteBookingHistory()
            {
                Bookingdata = siteBookingRecord.Bookingdata,
                Publishersiteid = siteBookingRecord.Publishersiteid,
                Statusid = siteBookingRecord.Statusid,
                Createdon = DateTime.UtcNow,
                Marketplacebookingid = siteBookingRecord.Marketplacebookingid.Value, //todo check this
                Processingnote = string.Empty,
                Bookingversion = siteBookingRecord.Bookingversion,
                Publisherbookinginfo = siteBookingRecord.Publisherbookinginfo,
                Publisherbookinginfodiff = siteBookingRecord.Publisherbookinginfodiff,
                Subscriberbookinginfodiff = siteBookingRecord.Subscriberbookinginfodiff,
                Sitebookingid = siteBookingRecord.Sitebookingid
            };          


            _context.SiteBooking.Update(siteBookingRecord);
            _context.SiteBookingHistory.Add(siteBookingHistory);
            _context.SaveChanges();


            return true;
        }
        public async Task UpdateSiteBookingStatusId(Guid siteBookingId, int bookingStatusId)
        {
            var siteBookingData =  _context.SiteBooking.FirstOrDefault(a=>a.Sitebookingid == siteBookingId);
            siteBookingData.Statusid = bookingStatusId;
            _context.SiteBooking.Update(siteBookingData);
            _context.SaveChanges();

            var siteBookingHistory = new SiteBookingHistory()
            {
                Bookingdata = siteBookingData.Bookingdata,
                Publishersiteid = siteBookingData.Publishersiteid,
                Statusid = siteBookingData.Statusid,
                Createdon = DateTime.UtcNow,
                Marketplacebookingid = siteBookingData.Marketplacebookingid.Value, //todo check this
                Processingnote = string.Empty,
                Bookingversion = siteBookingData.Bookingversion,
                Publisherbookinginfo = siteBookingData.Publisherbookinginfo,
                Publisherbookinginfodiff = siteBookingData.Publisherbookinginfodiff,
                Subscriberbookinginfodiff = siteBookingData.Subscriberbookinginfodiff,
                Sitebookingid = siteBookingData.Sitebookingid
            };

            _context.SiteBookingHistory.Add(siteBookingHistory);
            _context.SaveChanges();
        }

        public async Task<List<MappingData>> GetBookedOptionStatusMapping(Guid siteid, int direction)
        {
            return _context.MappingData.Where(a=>a.Datatypeid == 10 && a.Siteid == siteid && a.Mappingdirectionid == direction).ToList();
        }

        public async Task<List<MappingData>> GetSubscriberBookedOptionStatusMapping(Guid siteBookingId, int direction)
        {
            var subscriberSiteId = (from sb in _context.SiteBooking
                                    join mb in _context.MarketplaceBooking on sb.Marketplacebookingid equals mb.Marketplacebookingid
                                    where sb.Sitebookingid == siteBookingId
                                    select mb.Subscribersiteid).FirstOrDefault();
            return _context.MappingData.Where(a=>a.Datatypeid == 10 && a.Siteid == subscriberSiteId && a.Mappingdirectionid == direction).ToList();
        }

        public async Task<SiteBooking> GetSiteBookingData(Guid siteBookingId)
        {
            var siteBookingData =  _context.SiteBooking.FirstOrDefault(a=>a.Sitebookingid == siteBookingId);
            return siteBookingData;
        }

        public async Task<MarketplaceBooking> GetMarketplaceBookingData(Guid marketplacebookingid)
         {
             var marketplaceBookingData = _context.MarketplaceBooking.FirstOrDefault(a=>a.Marketplacebookingid == marketplacebookingid);
             return marketplaceBookingData;
         }


         public async Task<IEnumerable<string>> GetBookingReference(string bookingReference, int limit)
        {
            var subscriberBookingRef =   _context.MarketplaceBooking.Where(a => a.SubscriberBookingRef.Contains(bookingReference))
            .Select(q => q.SubscriberBookingRef);

             var publisherBookingRef =   _context.SiteBooking.Where(a => a.PublisherBookingRef.Contains(bookingReference))
            .Select(q => q.PublisherBookingRef);

            IEnumerable<string> result =  new List<string>();
            result.Concat(subscriberBookingRef);
            result.Concat(publisherBookingRef);

            result = limit>0?result.Take(limit):result;
            return result;
        }



        public async Task<BookingSearchResponse> BookingSearch(BookingSearchRequest request)
         {//join bs in _context.BookingUpdateStatus on bs.Bookingupdatestatusid equals sb.Statusid
           BookingSearchResponse response = new BookingSearchResponse();  
             if(request.EntityType == EntityType.Subscriber)
             {
              // var searchResult =  _context.MarketplaceBooking.Where(a=> a.Subscriberid == request.EntityId);
             var searchResult = (from mb in _context.MarketplaceBooking
                  join bs in _context.BookingUpdateStatus on  mb.Statusid equals bs.Bookingupdatestatusid where mb.Subscriberid == request.EntityId
                  orderby mb.Createdon descending
                  select new BookingModel

                   {
                    BookingReference  = mb.SubscriberBookingRef,
                    BookingStatusName = ((BookingStatus)bs.Bookingupdatestatusid).GetDescription(),
                    BookingStatusId = bs.Bookingupdatestatusid,
                    BookingDate = mb.Createdon,
                    BookingName = mb.Bookingname!=null?mb.Bookingname.Trim():"",
                    BookingId = mb.Marketplacebookingid
                   });

                searchResult = !String.IsNullOrEmpty(request.BookingReference)? searchResult.Where(s=>s.BookingReference == request.BookingReference):searchResult;
                searchResult = request.StatusId !=null && request.StatusId>=0? searchResult.Where(s=> request.StatusId== s.BookingStatusId):searchResult;
                searchResult = request.FromDate!=null? searchResult.Where(s=>s.BookingDate >= request.FromDate  ):searchResult;
                searchResult = request.ToDate!=null? searchResult.Where(s=>s.BookingDate <= request.ToDate  ):searchResult;
               
                response.Bookings = searchResult;
           // return marketplaceBookingData;
             }

             else if(request.EntityType == EntityType.Publisher)
             {

                var searchResult = (from sb in _context.SiteBooking
                                    join sbp in _context.SiteBookingPublisher on sb.Sitebookingid equals sbp.Sitebookingid
                  join bs in _context.BookingUpdateStatus on  sb.Statusid equals bs.Bookingupdatestatusid 
                                    where sbp.Publisherid == request.EntityId && sbp.Isprimarypublisher.Value
                   orderby sb.Createdon descending                 
                   select new BookingModel

                   {
                    BookingReference  = sb.PublisherBookingRef,
                    BookingStatusName = ((BookingStatus)bs.Bookingupdatestatusid).GetDescription(),
                    BookingStatusId = bs.Bookingupdatestatusid,
                    BookingDate = sb.Createdon,
                    BookingName = !string.IsNullOrEmpty(sb.Bookingname)?sb.Bookingname.Trim():_context.MarketplaceBooking.Where(m=>m.Marketplacebookingid == sb.Marketplacebookingid).FirstOrDefault().Bookingname.Trim(),
                    BookingId = sb.Sitebookingid
                   });

                searchResult = !String.IsNullOrEmpty(request.BookingReference)? searchResult.Where(s=>s.BookingReference == request.BookingReference):searchResult;
                searchResult = request.StatusId !=null && request.StatusId>=0? searchResult.Where(s=> request.StatusId== s.BookingStatusId):searchResult;
                searchResult = request.FromDate!=null? searchResult.Where(s=>s.BookingDate >= request.FromDate  ):searchResult;
                searchResult = request.ToDate!=null? searchResult.Where(s=>s.BookingDate <= request.ToDate  ):searchResult;
               
                response.Bookings = searchResult;

             }
        
            
           /*  List<BookingModel> bookingsList =  new List<BookingModel>();
            bookingsList.Add(new BookingModel{ BookingName = "Test Booking 1" ,BookingReference="abc123", BookingStatusName="Syncing"}); 
            bookingsList.Add(new BookingModel{ BookingName = "Test Booking 2" ,BookingReference="abc124", BookingStatusName="Synced"}); 
            bookingsList.Add(new BookingModel{ BookingName = "Test Booking 3" ,BookingReference="abc125", BookingStatusName="Error"}); 
            */
          
           return response;
         }


        public async Task<GetBookingStatusResponse> GetBookingStatus()
         {
        
            var bookingStatusList = (from bs in _context.BookingUpdateStatus 
            select new BookingStatusDataModel {
                            StatusId = bs.Bookingupdatestatusid,
                            StatusName = ((BookingStatus)bs.Bookingupdatestatusid).GetDescription()
                    }).ToList();
           
            /* List<BookingStatusDataModel> bookingStatusList =  new List<BookingStatusDataModel>();
            bookingStatusList.Add(new BookingStatusDataModel{ StatusId = 1 ,StatusName="Error"}); 
            bookingStatusList.Add(new BookingStatusDataModel{ StatusId = 2 ,StatusName="Syncing"}); 
            bookingStatusList.Add(new BookingStatusDataModel{ StatusId = 3 ,StatusName="Synced"}); 
            */
           GetBookingStatusResponse response = new GetBookingStatusResponse{BookingStatusList = bookingStatusList};
           return response;
         }


public async Task<GetMpBookingInfoResponse> GetMpSubscriberBookingInfo(GetMpBookingInfoRequest request)
         {

    GetMpBookingInfoResponse response = new GetMpBookingInfoResponse();

            
              var searchResult = (from mb in _context.MarketplaceBooking
                  join bs in _context.BookingUpdateStatus on  mb.Statusid equals bs.Bookingupdatestatusid where mb.Marketplacebookingid == request.BookingId
                  orderby mb.Createdon descending
                  select new BookingRecordModel{

                    Id = mb.Marketplacebookingid,
                    Date = mb.Createdon,
                    Status = ((BookingStatus)bs.Bookingupdatestatusid).GetDescription(),
                    Details = mb.Processingnote,
                    SubscriberData = mb.Bookingdata

                   })                    
                .ToList();
                        
            
             response.BookingRecords = searchResult;
            return response;
          

         }                   



public async Task<GetMpBookingInfoResponse> GetMpPublisherBookingInfo(GetMpBookingInfoRequest request)
         {
             GetMpBookingInfoResponse response = new GetMpBookingInfoResponse();

            var searchResult = (from sb in _context.SiteBooking
                                join bs in _context.BookingUpdateStatus on sb.Statusid equals bs.Bookingupdatestatusid
                                where sb.Marketplacebookingid == request.BookingId
                                join sp in _context.SiteBookingPublisher on sb.Sitebookingid equals sp.Sitebookingid
                                join p in _context.Publisher on sp.Publisherid equals p.PublisherId
                                join sbh in _context.SiteBookingHistory on sb.Sitebookingid equals sbh.Sitebookingid
                                
                                where sp.Isprimarypublisher.Value &&
                                sbh.Createdon == (from sbh2 in _context.SiteBookingHistory
                                                  where sbh2.Sitebookingid == sbh.Sitebookingid
                                                  select sbh2.Createdon).Max()
                                orderby sb.Createdon descending

                                select new BookingRecordModel
                                {

                                    Id = sbh.Sitebookinghistoryid,
                                    Date = sb.Createdon,
                                    Status = ((BookingStatus)bs.Bookingupdatestatusid).GetDescription(),
                                    Details = sb.Processingnote,
                                    EntityName = p.PublisherName,
                                    BookingReference = sb.PublisherBookingRef

                                    // SubscriberData = sb.Bookingdata,
                                    // PublisherData = sb.Publisherbookinginfo

                                })                    
                .ToList();
            
                              
             response.BookingRecords = searchResult;
            
             
         return response;


           
         }


         public async Task<GetMpBookingHistoryInfoResponse> GetMpSubscriberBookingHistoryInfo(GetMpBookingHistoryInfoRequest request)
         {
            GetMpBookingHistoryInfoResponse response = new GetMpBookingHistoryInfoResponse();           
                  var searchResult = (from mbh in _context.MarketplaceBookingHistory
                  join bs in _context.BookingUpdateStatus on  mbh.Statusid equals bs.Bookingupdatestatusid where mbh.Marketplacebookingid == request.BookingId
                  orderby mbh.Createdon descending 
                  select new BookingRecordModel{

                    Id = mbh.Marketplacebookinghistoryid,
                    Date = mbh.Createdon,
                    Status = ((BookingStatus)bs.Bookingupdatestatusid).GetDescription(),
                    StatusId = bs.Bookingupdatestatusid,
                    Details = mbh.Processingnote,
                   // SubscriberData = mbh.Bookingdata

                   })                    
                .ToList();

               foreach (var record in searchResult)
            {
                record.Event = record.StatusId!=(int)BookingStatus.Error? BookingEvent.BookingUpdate.GetDescription():BookingEvent.BookingUpdateError.GetDescription();
            }
            var firstHistoryRecord = searchResult.Last();

            if (firstHistoryRecord.StatusId == (int)BookingStatus.Syncing)
            {
                firstHistoryRecord.Event = BookingEvent.BookingNew.GetDescription();
            }
            response.BookingRecords = searchResult;
            
        
         return response;
         
           
         }




public async Task<GetMpBookingHistoryInfoResponse> GetMpPublisherBookingHistoryInfo(GetMpBookingHistoryInfoRequest request)
         {
            GetMpBookingHistoryInfoResponse response = new GetMpBookingHistoryInfoResponse();           
          
              var searchResult = (from sbh in _context.SiteBookingHistory
                  join bs in _context.BookingUpdateStatus on  sbh.Statusid equals bs.Bookingupdatestatusid where sbh.Sitebookingid == request.BookingId
                  orderby sbh.Createdon descending
                  select new BookingRecordModel{
                    Id = sbh.Sitebookinghistoryid,
                    Date = sbh.Createdon,
                    Status = ((BookingStatus)bs.Bookingupdatestatusid).GetDescription(),
                    StatusId = bs.Bookingupdatestatusid,                                
                    Details = sbh.Processingnote                
                   // PublisherData = sbh.Publisherbookinginfo

                   })                    
                .ToList();


            foreach (var record in searchResult)
            {
                record.Event = record.StatusId!=(int)BookingStatus.Error? BookingEvent.BookingUpdate.GetDescription():BookingEvent.BookingUpdateError.GetDescription();
            }
            var firstHistoryRecord = searchResult.Last();

            if (firstHistoryRecord.StatusId == (int)BookingStatus.Syncing)
            {
                firstHistoryRecord.Event = BookingEvent.BookingNew.GetDescription();
            }


            response.BookingRecords = searchResult;            
            
            
             
         return response;
         
           
         }

public async Task<GetMpBookingJsonDataResponse> GetMpPublisherBookingJsonData(GetMpBookingJsonDataRequest request)
         {
                GetMpBookingJsonDataResponse response = (from sbh in _context.SiteBookingHistory
                   where sbh.Sitebookinghistoryid == request.RowId
                   select new GetMpBookingJsonDataResponse{
                    
                   JsonData = request.JsonDataType == EntityType.Publisher? sbh.Publisherbookinginfo : sbh.Bookingdata

                   }).FirstOrDefault(); 

                   return response;

         }


public async Task<GetMpBookingJsonDataResponse> GetLatestMpPublisherBookingJsonData(GetMpBookingJsonDataRequest request)
         {

 var latestRecord = (from sbh in _context.SiteBookingHistory
                               where sbh.Sitebookingid == request.BookingId && sbh.Createdon ==
                               (from sbh2 in _context.SiteBookingHistory
                                where sbh2.Sitebookingid == sbh.Sitebookingid
                                && sbh2.Sitebookingid == request.BookingId
                                    select sbh2.Createdon).Max()
                                select sbh).FirstOrDefault();
    var jsonData =   (request.JsonDataType == EntityType.Publisher)? latestRecord.Publisherbookinginfo:latestRecord.Bookingdata;
    GetMpBookingJsonDataResponse response =new GetMpBookingJsonDataResponse();
    response.JsonData =jsonData;
    return response;
     }



  public async Task<GetMpBookingJsonDataResponse> GetLatestMpSubscriberBookingJsonData(GetMpBookingJsonDataRequest request)
         {
               
                var latestRecord = (from mbh in _context.MarketplaceBookingHistory
                               where 
                               mbh.Marketplacebookingid==request.BookingId &&
                               mbh.Createdon ==
                               (from mbh2 in _context.MarketplaceBookingHistory
                                where mbh2.Marketplacebookingid == mbh.Marketplacebookingid
                                    select mbh2.Createdon).Max()
                                select mbh).FirstOrDefault();
                var jsonData =   latestRecord.Bookingdata;
                GetMpBookingJsonDataResponse response =new GetMpBookingJsonDataResponse();
                response.JsonData =jsonData;
                return response;
         }    

     public async Task<GetMpBookingJsonDataResponse> GetMpSubscriberBookingJsonData(GetMpBookingJsonDataRequest request)
         {
             GetMpBookingJsonDataResponse response = new GetMpBookingJsonDataResponse();
             response = (from mbh in _context.MarketplaceBookingHistory
                   where mbh.Marketplacebookinghistoryid == request.RowId
                   select new GetMpBookingJsonDataResponse{
                    
                   JsonData = mbh.Bookingdata

                   }).FirstOrDefault();   
                   return response;

         }    






         public async Task<GetMpBookingJsonDataResponse> GetMpBookingJsonData(GetMpBookingJsonDataRequest request)
         {
        

            GetMpBookingJsonDataResponse response = new GetMpBookingJsonDataResponse();           
          if(request.EntityType == EntityType.Publisher)
             {  
                 if(request.RowId != null)
                 {
                 response = (from sbh in _context.SiteBookingHistory
                   where sbh.Sitebookinghistoryid == request.RowId
                   select new GetMpBookingJsonDataResponse{
                    
                   JsonData = request.JsonDataType == EntityType.Publisher? sbh.Publisherbookinginfo : sbh.Bookingdata

                   }).FirstOrDefault(); 
                 }
                   else if(request.BookingId != null)
                   {
                       response = (from sbh in _context.SiteBookingHistory
                       where sbh.Sitebookingid == request.BookingId
                       group sbh by sbh.Createdon into g
                       select new GetMpBookingJsonDataResponse{
                        JsonData =request.JsonDataType == EntityType.Publisher? g.OrderByDescending(t=>t.Createdon).FirstOrDefault().Publisherbookinginfo : g.FirstOrDefault().Bookingdata
                
                   }).FirstOrDefault(); 
                       
                    
                   }
             }

            else if(request.EntityType == EntityType.Subscriber)
             {  
                  if(request.RowId != null)
                 {
                 response = (from mbh in _context.MarketplaceBookingHistory
                   where mbh.Marketplacebookinghistoryid == request.RowId
                   select new GetMpBookingJsonDataResponse{
                    
                   JsonData = mbh.Bookingdata

                   }).FirstOrDefault();   

                 }
                    else if(request.BookingId != null)
                   {
                       response = (from sbh in _context.MarketplaceBookingHistory
                       where sbh.Marketplacebookingid == request.BookingId
                       group sbh by sbh.Createdon into g
                       select new GetMpBookingJsonDataResponse{
                        JsonData = g.OrderByDescending(t=>t.Createdon).FirstOrDefault().Bookingdata
                 //  JsonData = sbh.Publisherbookinginfo

                   }).FirstOrDefault();               
                          
             }
              
           
         }
         return response;    

         }


        public bool InsertSiteBookingPublisherData(Guid siteBookingId, List<SiteServiceData> siteService)
        {
            var siteBookingPublishers = _context.SiteBookingPublisher.Where(a => a.Sitebookingid == siteBookingId).Select(q => q).ToList();

            var primaryPublisher = siteService.First();
            List<SiteBookingPublisher> siteBookingPublishersData;

            if (siteBookingPublishers==null || !siteBookingPublishers.Any())
            {
                siteBookingPublishersData = GetSiteBookingPublisherToBeAdded(siteBookingId, siteService.Select(a => a.PublisherId).Distinct().ToList(), primaryPublisher.PublisherId);
            }
            else
            {
                var publisherIds = siteService.Where(a => !siteBookingPublishers.Select(q => q.Publisherid).ToList().Contains(a.PublisherId)).Select(w=>w.PublisherId).Distinct().ToList();
                siteBookingPublishersData = GetSiteBookingPublisherToBeAdded(siteBookingId, publisherIds, Guid.Empty);
            }

            if (siteBookingPublishersData == null)
                return false;

            _context.SiteBookingPublisher.AddRange(siteBookingPublishersData);
            _context.SaveChanges();

            return true;

        }

        
        private List<SiteBookingPublisher> GetSiteBookingPublisherToBeAdded(Guid siteBookingId, List<Guid> publishers, Guid primaryPublisher)
        {
            if (publishers == null || !publishers.Any())
                return null;

            var response =  publishers.Select(ss => new SiteBookingPublisher
            {
                Publisherid = ss,
                Sitebookingid = siteBookingId
            }).Distinct().ToList();

            if (primaryPublisher == Guid.Empty)
                return response;

            response.FirstOrDefault(a => a.Publisherid == primaryPublisher).Isprimarypublisher = true;

            return response;
        }
    }
}
