using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MarketPlaceService.DAL.Models;

namespace MarketPlaceService.DAL
{
    public class QueuedUpdateRepository : BaseRepository, IQueuedUpdatesRepository
    {
        private readonly string _user = string.Empty;
        public QueuedUpdateRepository(MarketplaceDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Entities.SubscriberProductTsUpdateQueue>> GetQueuedUpdates(int limit, string serverName)
        {
            List<Entities.SubscriberProductTsUpdateQueue> queuedUpdateResponse = new List<Entities.SubscriberProductTsUpdateQueue>();
            queuedUpdateResponse = (from qu in _context.SubscriberProductTsUpdateQueue
                                    join sp in _context.SubscriberProduct on qu.Marketplaceproductid equals sp.Marketplaceproductid
                                    join js in _context.JobStatus on qu.Jobstatusid equals js.Jobstatusid
                                    where qu.Subscriberid == sp.Subscriberid && (js.Jobstatusname.ToLower().Equals("queued") || js.Jobstatusname.ToLower().Equals("retry"))
                                    select new Entities.SubscriberProductTsUpdateQueue
                                    {
                                        SubscriberProductTsUpdateQueueId = qu.Subscriberproducttsupdatequeueid,
                                        SubscriberId = qu.Subscriberid.Value,
                                        MessageTypeId = qu.Messagetypeid,
                                        TsId = qu.Tsid,
                                        SubscriberProductId = sp.Subscriberproductid,
                                        JobNote = qu.Jobnote
                                    }).Take(limit).ToList<Entities.SubscriberProductTsUpdateQueue>();
            if (queuedUpdateResponse != null && queuedUpdateResponse.Count() > 0)
            {
                var inProcessStatusId = (from status in _context.JobStatus
                                         where status.Jobstatusname.ToLower().Equals("processing")
                                         select status.Jobstatusid).FirstOrDefault();
                foreach (var row in queuedUpdateResponse)
                {
                    row.JobStatusId = inProcessStatusId;
                    row.JobStartDateTime = DateTime.UtcNow;
                    row.JobEndDateTime = new DateTime(1800, 01, 01);
                    await UpdateQueuedUpdate(row.SubscriberProductTsUpdateQueueId, row);
                }
            }
            return await Task.FromResult(queuedUpdateResponse);
        }

        public async Task<Entities.SubscriberProductTsUpdateQueue> GetQueuedUpdate(Guid id)
        {
            Entities.SubscriberProductTsUpdateQueue response = new Entities.SubscriberProductTsUpdateQueue();
            response = _context.SubscriberProductTsUpdateQueue.Where(x => x.Subscriberproducttsupdatequeueid == id).Select(a => new Entities.SubscriberProductTsUpdateQueue
            {
                SubscriberProductTsUpdateQueueId = a.Subscriberproducttsupdatequeueid,
                MarketPlaceProductId = a.Marketplaceproductid.Value,
                SubscriberId = a.Subscriberid.Value,
                JobStatusId = a.Jobstatusid,
                MessageTypeId = a.Messagetypeid,
                RetryCount = a.Retrycount,
            }).FirstOrDefault();
            return await Task.FromResult(response);
        }

        public async Task<Entities.SubscriberProductTsUpdateQueue> InsertSubscriberProductTsUpdateQueue(Entities.SubscriberProductTsUpdateQueue request)
        {
            Models.SubscriberProductTsUpdateQueue queuedUpdate = new Models.SubscriberProductTsUpdateQueue
            {
                Marketplaceproductid = request.MarketPlaceProductId == Guid.Empty ? (Guid?)null : request.MarketPlaceProductId,
                Subscriberid = request.SubscriberId == Guid.Empty ? (Guid?)null : request.SubscriberId,
                Jobstatusid = 1,
                Messagetypeid = request.MessageTypeId == 0 ? 1 : request.MessageTypeId,
                Retrycount = 0,
                Jobnote = request.JobNote?? string.Empty,
                Jobcreationdatetime = DateTime.UtcNow,
                Tsid = request.TsId
            };
            _context.SubscriberProductTsUpdateQueue.Add(queuedUpdate);
            _context.SaveChanges();
            return await Task.FromResult(request);
        }
        public async Task<Entities.SubscriberProductTsUpdateQueue> InsertBookingUpdateQueueFromSubscriber(Entities.SubscriberProductTsUpdateQueue request)
        {
            Models.BookingUpdateFromPublisherQueue queuedUpdate = new Models.BookingUpdateFromPublisherQueue
            {
                Sitebookingid = request.SiteBookingId,
                Jobstatusid =(short)MarketPlaceService.Entities.JobStatus.Queued,
                Retrycount = 0,
                Jobnote = request.JobNote?? string.Empty,
                Jobcreationdatetime = DateTime.UtcNow,
                Jobtypeid = (int)CallbackJobType.SubscriberCallBack,
                Traceid = request.TraceId
            };

            _context.BookingUpdateFromPublisherQueue.Add(queuedUpdate);
            _context.SaveChanges();
            
            return await Task.FromResult(request);
        }
        public async Task<Entities.SubscriberProductTsUpdateQueue> InsertBookingUpdateFromPublisherQueue(Entities.SubscriberProductTsUpdateQueue request)
        {
            var publisherBookingUpdateJobStatusId = (short)MarketPlaceService.Entities.JobStatus.Queued;

            var booking = ( from sb in _context.SiteBooking
                            join mb in _context.MarketplaceBooking on sb.Marketplacebookingid equals mb.Marketplacebookingid // join needs to be changed to sb.Marketplacebookingid
                            where sb.Sitebookingid == request.SiteBookingId
                            select new {
                                Sitebookingid = sb.Sitebookingid,
                                SiteBookingStatusid = sb.Statusid,
                                Marketplacebookingid = mb.Marketplacebookingid,
                                MarketplaceBookingStatusid = mb.Statusid,
                                Publishersiteid= sb.Publishersiteid
                            }).FirstOrDefault();
                            
             // booking : null the log error and return             
            // TSID : PublisherBookingId, null  and jobtype is bookingPush / create booking failed return

            if(booking == null)
            {
                throw new Exception("Site booking doesnot exist");
            }
            else
            {
                var publisherBookingQueueJobs = from bufpq in _context.BookingUpdateFromPublisherQueue
                                        where bufpq.Sitebookingid == booking.Sitebookingid
                                        select bufpq;
                
                var dormantQueueJob = publisherBookingQueueJobs != null && publisherBookingQueueJobs.Count() > 0 ? 
                publisherBookingQueueJobs.Where(pbqj => pbqj.Jobstatusid == (int)MarketPlaceService.Entities.JobStatus.Dormant).FirstOrDefault()
                : null;

                // exist dormant job then log info for sitebookingid ?? insert empty in queue history ?
                var dormantCriteria = new[] { 3,1 }; 
                
                if(dormantQueueJob == null)
                {
                    if(publisherBookingQueueJobs !=null && publisherBookingQueueJobs.Count() > 0)
                    {
                        publisherBookingUpdateJobStatusId = (short)MarketPlaceService.Entities.JobStatus.Dormant;
                    }
                    else if((!dormantCriteria.Contains((short)booking.SiteBookingStatusid) || !dormantCriteria.Contains((short)booking.MarketplaceBookingStatusid)) && request.CallBackJobTypeId == (int)MarketPlaceService.Entities.CallbackJobType.CDCBookingUpdate)
                        //(booking.SiteBookingStatusid != (int)BookingStatus.Synced && booking.SiteBookingStatusid != (int)BookingStatus.Error) || (booking.MarketplaceBookingStatusid != (int)BookingStatus.Synced && booking.MarketplaceBookingStatusid != (int)BookingStatus.Error)) && request.CallBackJobTypeId == (int)MarketPlaceService.Entities.CallbackJobType.CDCBookingUpdate)
                    {
                        publisherBookingUpdateJobStatusId = (short)MarketPlaceService.Entities.JobStatus.Dormant;
                    }
                
                    Models.BookingUpdateFromPublisherQueue queuedUpdate = new Models.BookingUpdateFromPublisherQueue
                    {
                        Publisherbookingid = request.TsId,
                        Sitebookingid = booking.Sitebookingid,
                        Publishersiteid = booking.Publishersiteid,
                        Jobstatusid = publisherBookingUpdateJobStatusId,
                        Retrycount = 0,
                        Jobnote = request.JobNote?? string.Empty,
                        Jobcreationdatetime = DateTime.UtcNow,
                        Jobtypeid = request.CallBackJobTypeId,
                        Traceid = request.TraceId
                    };

                    _context.BookingUpdateFromPublisherQueue.Add(queuedUpdate);
                    _context.SaveChanges();
                }
            
            }


            return await Task.FromResult(request);
        }

        public async Task<Entities.SubscriberProductTsUpdateQueue> UpdateQueuedUpdate(Guid id, Entities.SubscriberProductTsUpdateQueue request)
        {
            Models.SubscriberProductTsUpdateQueue queuedupdate = _context.SubscriberProductTsUpdateQueue.Where(a => a.Subscriberproducttsupdatequeueid == id).FirstOrDefault();
            if (queuedupdate != null)
            {
                DateTime defaultdt = new DateTime(1800, 01, 01);
                queuedupdate.Retrycount = request.RetryCount;
                if (request.JobStartDateTime > defaultdt)
                {
                    queuedupdate.Jobstartdatetime = request.JobStartDateTime;
                }

                if (request.JobEndDateTime > defaultdt)
                {
                    queuedupdate.Jobenddatetime = request.JobEndDateTime;
                }
                queuedupdate.Jobstatusid = request.JobStatusId;
                queuedupdate.Jobnote = string.IsNullOrEmpty(request.JobNote) ? queuedupdate.Jobnote : request.JobNote;

                _context.SubscriberProductTsUpdateQueue.Update(queuedupdate);
                await _context.SaveChangesAsync();
            }
            return await Task.FromResult(request);
        }

        public async Task<bool> DeleteQueuedUpdate(Guid id)
        {
            var queuedUpdate = _context.SubscriberProductTsUpdateQueue.FirstOrDefault(a => a.Subscriberproducttsupdatequeueid == id);
            if (queuedUpdate == null)
                return false;

            ActivateDormantSubscriberJob(queuedUpdate.Marketplaceproductid.Value, queuedUpdate.Subscriberid.Value);

            _context.SubscriberProductTsUpdateQueue.Remove(queuedUpdate);
            await _context.SaveChangesAsync();
            return true;
        }

        private void ActivateDormantSubscriberJob(Guid marketplaceProductId, Guid subscriberId)
        {
            //if the published product has another queue item in the dormant state then check if the published product status is a success and then reactivate the new queue status
            var subscriberProduct = _context.SubscriberProduct.FirstOrDefault(a => a.Marketplaceproductid == marketplaceProductId && a.Subscriberid == subscriberId);
            if (subscriberProduct.Productstatusid == 2)
            {
                var frozenSubscriberProductQueue = _context.SubscriberProductQueue.Where(a => a.Subscriberproductid == subscriberProduct.Subscriberproductid && a.Jobstatusid == 4 && a.Jobtypeid == 1).FirstOrDefault();
                if (frozenSubscriberProductQueue != null)
                {
                    frozenSubscriberProductQueue.Jobstatusid = 1;
                    _context.SubscriberProductQueue.Update(frozenSubscriberProductQueue);
                }
            }
        }

        public async Task<Entities.SubscriberProductTsUpdateQueue> InsertQueuedUpdateHistory(Entities.SubscriberProductTsUpdateQueue request)
        {
            var jobHistoryStatuses = _context.JobHistoryStatus.Select(a => a).ToList();
            short jobHistoryStatusId = jobHistoryStatuses.FirstOrDefault(a => a.Jobstatusname.ToLower().Equals("success")).Jobstatusid;
            string errorText = null;
            if (request.Errors != null && request.Errors.Count > 0)
            {
                //var error = GetErrorDetails(request.Errors.FirstOrDefault());
                //subStatusErrorId = error.ErrorId;
                errorText = request.Errors.Select(e => e.ErrorMessage).FirstOrDefault();
                jobHistoryStatusId = jobHistoryStatuses.FirstOrDefault(a => a.Jobstatusname.ToLower().Equals("error")).Jobstatusid;
            }

            Models.SubscriberProductTsUpdateQueueHistory queueHistory = new Models.SubscriberProductTsUpdateQueueHistory();

            queueHistory = _context.SubscriberProductTsUpdateQueue.Where(a=>a.Subscriberproducttsupdatequeueid == request.SubscriberProductTsUpdateQueueId).Select(q=> new Models.SubscriberProductTsUpdateQueueHistory
            {
                Marketplaceproductid = q.Marketplaceproductid.Value,
                Subscriberid = q.Subscriberid.Value,
                Jobhistorystatusid = jobHistoryStatusId,
                Messagetypeid = q.Messagetypeid,
                Retrycount = q.Retrycount,
                Jobnote = errorText ?? q.Jobnote,
                Jobcreationdatetime = q.Jobcreationdatetime,
                Jobstartdatetime = q.Jobstartdatetime,
                Jobenddatetime = DateTime.UtcNow,
                Tsid = q.Tsid,
                Traceid =request.TraceId
            }).FirstOrDefault();

            _context.SubscriberProductTsUpdateQueueHistory.Add(queueHistory);
            _context.SaveChanges();
            return await Task.FromResult(request);
        }
    }
}
