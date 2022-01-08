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
using MarketPlaceService.Entities.Job;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MarketPlaceService.Utilities;
using MarketPlaceService.BLL.Contracts.Jobs;
using MarketPlaceService.BLL.Jobs;

namespace MarketPlaceService.BLL
{
    public class JobService:IJobService
    {

        
        private const string SERVICE_NAME = "JobService";
        private readonly IJobRepository _jobRepository;
        private ISiteRepository _siteRepository;
        private readonly IAPIManagerService _apiManagerService;
        private readonly ILogger<JobService> _logger;
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

        public JobService(IJobRepository jobRepository, ILogger<JobService> logger, IAPIManagerService apiManagerService, ISiteRepository siteRepository)
        {
            _logger = logger;
            _jobRepository = jobRepository;
            _apiManagerService = apiManagerService;
            _siteRepository = siteRepository;
             
           
        }

    public async Task<List<BusinessProcessDataModel>> GetBusinessProcess()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetBusinessProcess", SERVICE_NAME, TraceId);
            List<BusinessProcessDataModel> response =  new List<BusinessProcessDataModel>();
            foreach(BusinessProcess bp in Enum.GetValues(typeof(BusinessProcess)))
            {
                BusinessProcessDataModel bpModel = new BusinessProcessDataModel()
                {
                    Id = (int)bp,
                    Name = bp.GetDescription()
                    
                };
                response.Add(bpModel);
            }
            
            LoggingHelper.LogInfo(_logger, LogType.End, "GetBusinessProcess", SERVICE_NAME, TraceId);
            return response;
        }

        public async Task<List<QueueDataModel>> GetBusinessProcessQueue(BusinessProcess businessProcess)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetBusinessProcessQueue", SERVICE_NAME, TraceId);
            List<QueueDataModel> response =  new List<QueueDataModel>();
            Type type = null;
            
            switch (businessProcess)
            {
                case BusinessProcess.PublishingToMarketplace:
                type =typeof(BusinessProcessQueue.PublishingToMarketplace);
                break;
                case BusinessProcess.SubscribingInMarketplace:
                type =typeof(BusinessProcessQueue.SubscribingInMarketplace);
                break;
                case BusinessProcess.SubscribingAtTheSubscriber:
                type =typeof(BusinessProcessQueue.SubscribingAtTheSubscriber);
                break;
                case BusinessProcess.NewBookingInTheSubscriber:
                type =typeof(BusinessProcessQueue.NewBookingInTheSubscriber);
                break;
                  case BusinessProcess.UpdateBookingInTheSubscriber:
                type =typeof(BusinessProcessQueue.UpdateBookingInThePublisher);
                break;
                case BusinessProcess.UpdateBookingInThePublisher:
                type =typeof(BusinessProcessQueue.UpdateBookingInThePublisher);
                break;

            }
             
            foreach(var q in Enum.GetValues(type))
            {
                QueueDataModel queue = new QueueDataModel()
                {
                    Id = (int)q,
                    Name = ((Enum)q).GetDescription()
                    
                };
                response.Add(queue);
            }
            
            LoggingHelper.LogInfo(_logger, LogType.End, "GetBusinessProcessQueue", SERVICE_NAME, TraceId);
            return response;
 }

     public async Task<List<JobStatusDataModel>> GetJobStatus(bool forCurrentJobs )
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetJobProcess", SERVICE_NAME, TraceId);
            List<JobStatusDataModel> response =  new List<JobStatusDataModel>();

            if(forCurrentJobs)
            {
                foreach(JobStatus s in Enum.GetValues(typeof(JobStatus)))
                        {
                            JobStatusDataModel status = new JobStatusDataModel
                            {
                                Id = (int)s,
                                Name = s.ToString()
                                
                            };
                            response.Add(status);
                        }

            }
            else
            {
                foreach(JobHistoryStatus s in Enum.GetValues(typeof(JobHistoryStatus)))
                {
                    JobStatusDataModel status = new JobStatusDataModel
                    {
                        Id = (int)s,
                        Name = s.ToString()
                        
                    };
                    response.Add(status);
                }
            }  
                LoggingHelper.LogInfo(_logger, LogType.End, "GetJobProcess", SERVICE_NAME, TraceId);
                return response;
            }


private async Task<JobInfoResponse> SendJobInfoRequestToTsapi(JobInfoRequest request)
        {
            JobInfoResponse response = new JobInfoResponse();
                var searchResponse = await _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Job, "JobSearch", null, null, EntityType.Site, request.SiteId);
                var result =     JsonConvert.DeserializeObject<JobInfoResponse>(searchResponse);
                if( result!=null) response = result;
   
            return response;
}


private async Task<JobSearchResponse> SendSearchRequestToTsapi(JobSearchRequest request)
{
    List<Task<string>> tasks = new List<Task<string>>();
    List<Tuple<Guid,Task<string>>> taskList = new List<Tuple<Guid,Task<string>>>();
    var siteList = await _siteRepository.GetRegisteredSitesAsync();
    var allowedSites = request.AllowedSites;

    JobSearchResponse response = new JobSearchResponse(){JobRecords= new List<JobRecord>()};
    if(allowedSites!=null && allowedSites.Count> 1)
                {        

                 foreach (var siteId in allowedSites)
                {
                           
                    var task =  _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Job, "JobSearch", null, null, EntityType.Site, siteId);
                    tasks.Add(task);
                    taskList.Add(new Tuple<Guid, Task<string>>(siteId,task));
                   
                }
                Task.WaitAll(tasks.ToArray());

            foreach(var taskItem in taskList)
            {
               
               var resp = JsonConvert.DeserializeObject<Response<JobSearchResponse>>(taskItem.Item2.Result);
               if(resp == null ||  resp.ResponseMessage == null)continue;
                var result =  resp.ResponseMessage;

                    if (result == null || result.JobRecords == null  || result.JobRecords.Count<1)
                        continue;

                   foreach(var job in result.JobRecords)
                   {
                       job.SiteId = siteList.Where(s=>s.SiteId == taskItem.Item1).FirstOrDefault().SiteId ;
                       job.SiteName = siteList.Where(s=>s.SiteId == taskItem.Item1).FirstOrDefault().SiteName ;}

                response.JobRecords.AddRange(result.JobRecords);                     
              
                 
            }

                }

            else if(allowedSites != null && allowedSites.Count == 1)
            {
                  

                var searchResponse = await _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Job, "JobSearch", null,null, EntityType.Site, allowedSites[0]);
                var result =     JsonConvert.DeserializeObject<Response<JobSearchResponse>>(searchResponse);                
                if( result!=null &&result.ResponseMessage!=null&& result.ResponseMessage.JobRecords!=null && result.ResponseMessage.JobRecords.Count>0) 
                {
                       foreach(var job in result.ResponseMessage.JobRecords)
                    {
                        
                       job.SiteId = siteList.Where(s=>s.SiteId == allowedSites[0]).FirstOrDefault().SiteId ;
                       job.SiteName = siteList.Where(s=>s.SiteId == allowedSites[0]).FirstOrDefault().SiteName ;
                    }
                
                    response.JobRecords.AddRange(result.ResponseMessage.JobRecords);
                }
            }

            return response;
}
            public async Task<JobSearchResponse> SearchJob(JobSearchRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchJob", SERVICE_NAME, TraceId);
            var response =  (JobSearchResponse)null;
             List<JobSearchResponse> mpresponse =  new List<JobSearchResponse>();
            response = new  JobSearchResponse(){JobRecords = new List<JobRecord>()};
            var siteresponse = (JobSearchResponse)null;
            List<Tuple<Guid,Task<string>>> taskList = new List<Tuple<Guid,Task<string>>>();
            string searchResponse ="";
            List<Task<string>> tasks = new List<Task<string>>();
            List<Task<JobSearchResponse>> mptasks = new List<Task<JobSearchResponse>>();
            //List<Tuple<Guid,Task<JobSearchResponse>>> mptaskList = new List<Tuple<Guid,Task<JobSearchResponse>>>();
           
            switch (request.BusinessProcess)
            {
                case BusinessProcess.PublishingToMarketplace :
                if(request.CurrentJobsOnly)
                {
                  mpresponse.Add(await _jobRepository.SearchPublishedProductsQueue(request));            
                  
                }
                else
                {
                   mpresponse.Add(await  _jobRepository.SearchPublishedProductsQueueHistory(request));                 
                    
                }
                break;
                 case BusinessProcess.SubscribingInMarketplace :   
                 if(request.CurrentJobsOnly)
                {
                    
                   mpresponse.Add(await _jobRepository.SearchSubscriberProductsQueue(request));
                
                }
                else
                {
                    mpresponse.Add(await  _jobRepository.SearchSubscriberProductsQueueHistory(request));
                    
                }             
                break;
                 case BusinessProcess.SubscribingAtTheSubscriber : 

                  if(request.CurrentJobsOnly)
                {
                    
                   mpresponse.Add(await _jobRepository.SearchSubscriberProductTsUpdateQueue(request));
                   siteresponse = await SendSearchRequestToTsapi(request);
                   
                
                }
                else
                {
                    mpresponse.Add(await  _jobRepository.SearchSubscriberProductTsUpdateQueueHistory(request));
                    
                    siteresponse = await SendSearchRequestToTsapi(request);
                }     
                break;
                 case BusinessProcess.NewBookingInTheSubscriber :
                if(request.CurrentJobsOnly)
                {
                  mpresponse.Add(await _jobRepository.SearchMarketplaceBookingPushQueue(request));
                  mpresponse.Add(await _jobRepository.SearchSiteBookingPushQueue(request));
                  mpresponse.Add(await _jobRepository.SearchBookingUpdateFromPublisherQueue(request));
                  siteresponse = await SendSearchRequestToTsapi(request);
                }
                else
                {
                mpresponse.Add(await _jobRepository.SearchMarketplaceBookingPushQueueHistory(request));
                mpresponse.Add(await _jobRepository.SearchSiteBookingPushQueueHistory(request));
                mpresponse.Add(await _jobRepository.SearchBookingUpdateFromPublisherQueueHistory(request));
                siteresponse = await SendSearchRequestToTsapi(request);

                }

                 break;
                 case BusinessProcess.UpdateBookingInTheSubscriber :

  if(request.CurrentJobsOnly)
                {
                mpresponse.Add(await _jobRepository.SearchMarketplaceBookingPushQueue(request));
                mpresponse.Add(await _jobRepository.SearchSiteBookingPushQueue(request));
                 mpresponse.Add(await _jobRepository.SearchBookingUpdateFromPublisherQueue(request));
                  siteresponse = await SendSearchRequestToTsapi(request);
                    
                }
                else
                {

                mpresponse.Add(await _jobRepository.SearchMarketplaceBookingPushQueueHistory(request));
                mpresponse.Add(await _jobRepository.SearchSiteBookingPushQueueHistory(request));
                mpresponse.Add(await _jobRepository.SearchBookingUpdateFromPublisherQueueHistory(request));
                siteresponse = await SendSearchRequestToTsapi(request);

                }
                 break;   
                 case BusinessProcess.UpdateBookingInThePublisher :
                 if(request.CurrentJobsOnly)
                {

                  mpresponse.Add(await _jobRepository.SearchBookingUpdateFromPublisherQueue(request)); 
                  siteresponse = await SendSearchRequestToTsapi(request); 
                }
                else
                {
                    mpresponse.Add(await _jobRepository.SearchBookingUpdateFromPublisherQueueHistory(request));
                    siteresponse = await SendSearchRequestToTsapi(request); 
                }                
                
                 break;   
            }

            foreach (var r in mpresponse)
            {
                foreach(var job in r.JobRecords)
                job.SiteName="Marketplace";
                response.JobRecords.AddRange (r.JobRecords);
            }
            
            if(siteresponse!=null && siteresponse.JobRecords!=null && siteresponse.JobRecords.Count>0)
            response.JobRecords.AddRange(siteresponse.JobRecords);
            LoggingHelper.LogInfo(_logger, LogType.End, "SearchJob", SERVICE_NAME, TraceId);
               
            return response;

                //if(!string.IsNullOrEmpty(searchResponse))return JsonConvert.DeserializeObject<JobSearchResponse>(searchResponse);

               // return null;

        }


/* 
        public async Task<JobInfoResponse> JobInfo(JobInfoRequest request)
        {
            
            LoggingHelper.LogInfo(_logger, LogType.Start, "JobInfo", SERVICE_NAME, TraceId);
            var response =  (JobInfoResponse)null;
            var siteresponse = (JobInfoResponse)null;
            
            List<JobInfoResponse> mpresponse =  new List<JobInfoResponse>();
            
            switch (request.BusinessProcess)

            {
                case BusinessProcess.PublishingToMarketplace :
                if(request.CurrentJobsOnly)
                {
                  mpresponse.Add(await _jobRepository.GetPublishedProductsQueueRecord(request));            
                  
                }
                else
                {
                   mpresponse.Add(await  _jobRepository.GetPublishedProductsQueueHistoryRecord(request));                 
                    
                }
                break;
                case BusinessProcess.SubscribingInMarketplace :
                  if(request.CurrentJobsOnly)
                {
                    
                   mpresponse.Add(await _jobRepository.GetSubscriberProductsQueueRecord(request));
                
                }
                else
                {
                    mpresponse.Add(await  _jobRepository.GetSubscriberProductsQueueHistoryRecord(request));
                    
                }             
               

                break;
                case BusinessProcess.SubscribingAtTheSubscriber :
                BusinessProcessQueue.SubscribingAtTheSubscriber queue = (BusinessProcessQueue.SubscribingAtTheSubscriber)request.ProcessQueueId;

                BusinessProcessJobType.SubscribingAtTheSubscriber jobtype = (BusinessProcessJobType.SubscribingAtTheSubscriber)request.ProcessQueueId;
                switch(queue)
                {
                    case BusinessProcessQueue.SubscribingAtTheSubscriber.ImportNewProduct:
                    case BusinessProcessQueue.SubscribingAtTheSubscriber.ImportProductUpdates:
                    case BusinessProcessQueue.SubscribingAtTheSubscriber.Unsubscribe:
                    siteresponse = await SendJobInfoRequestToTsapi(request);
                    break;
                    case BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductImport:
                    if(request.CurrentJobsOnly)
                    {
                    var result = await _jobRepository.GetSubscriberProductTsUpdateQueueRecord(request);
                    mpresponse.Add(result);
                    if(result==null)
                        siteresponse = await SendJobInfoRequestToTsapi(request);
                    }
                            else
                            {
                     var result = await _jobRepository.GetSubscriberProductTsUpdateQueueHistoryRecord(request);
                     mpresponse.Add(result);
                    if(result==null)
                        siteresponse = await SendJobInfoRequestToTsapi(request);
                        
                    }
                    break;
                    case BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductUpdatesImport:
                        siteresponse = await SendJobInfoRequestToTsapi(request);                        
                    
                    break;
                                      
                    
                }
                break;
                case BusinessProcess.NewBookingInTheSubscriber:
                BusinessProcessQueue.NewBookingInTheSubscriber queue2 = (BusinessProcessQueue.NewBookingInTheSubscriber)request.ProcessQueueId;

                BusinessProcessJobType.NewBookingInTheSubscriber jobtype2 = (BusinessProcessJobType.NewBookingInTheSubscriber)request.ProcessQueueId;
                switch(queue2)
                {
                    case BusinessProcessQueue.NewBookingInTheSubscriber.NewSubscriberBooking:
                    siteresponse = await SendJobInfoRequestToTsapi(request);
                    break;

                    case BusinessProcessQueue.NewBookingInTheSubscriber.ImportSubscriberBooking:
                    if(request.CurrentJobsOnly)
                    mpresponse.Add(await _jobRepository.GetMarketplaceBookingPushQueueRecord(request));
                   else
                    mpresponse.Add(await _jobRepository.GetMarketplaceBookingPushQueueHistoryRecord(request));
                    break;
                    case BusinessProcessQueue.NewBookingInTheSubscriber.PushPublisherBooking:
                     if(request.CurrentJobsOnly)
                    mpresponse.Add(await _jobRepository.GetSiteBookingPushQueueRecord(request));
                   else
                    mpresponse.Add(await _jobRepository.GetSiteBookingPushQueueHistoryRecord(request));
                    
                    break;
                    case BusinessProcessQueue.NewBookingInTheSubscriber.ImportPublisherBooking:

                    if(request.CurrentJobsOnly)
                    {
                    var result = await _jobRepository.GetBookingUpdateFromPublisherQueueRecord(request);
                    if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result = await _jobRepository.GetBookingUpdateFromPublisherQueueHistoryRecord(request);
                        if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);   

                    }
                    break;
                    case BusinessProcessQueue.NewBookingInTheSubscriber.ConfirmPublisherBookingImport:
                    if(request.CurrentJobsOnly)
                    {
                    var result = await _jobRepository.GetBookingUpdateFromPublisherQueueRecord(request);
                    if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result = await _jobRepository.GetBookingUpdateFromPublisherQueueHistoryRecord(request);
                        if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);       
                    }                                              
                    
                    break;
                            case BusinessProcessQueue.NewBookingInTheSubscriber.UpdateSubscriberBooking:
                    if(request.CurrentJobsOnly)
                    {
                    var result = await _jobRepository.GetBookingUpdateFromPublisherQueueRecord(request);
                    if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result = await _jobRepository.GetBookingUpdateFromPublisherQueueHistoryRecord(request);
                        if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);       
                    }                                              
                    
                    break;              
                    
                }



                break;
                case BusinessProcess.UpdateBookingInTheSubscriber :

                BusinessProcessQueue.UpdateBookingInTheSubscriber queue3 = (BusinessProcessQueue.UpdateBookingInTheSubscriber)request.ProcessQueueId;

                BusinessProcessJobType.UpdateBookingInTheSubscriber jobtype3 = (BusinessProcessJobType.UpdateBookingInTheSubscriber)request.ProcessQueueId;
                switch(queue3)
                {
                    case BusinessProcessQueue.UpdateBookingInTheSubscriber.SubscriberBookingUpdates:
                      if(request.CurrentJobsOnly)
                    {
                        var result = await _jobRepository.GetMarketplaceBookingPushQueueRecord(request);
                        mpresponse.Add(result);
                        var result2 =  await _jobRepository.GetSiteBookingPushQueueRecord(request);      
                        mpresponse.Add(result2);               
                        if(result==null && result2==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result = await _jobRepository.GetMarketplaceBookingPushQueueHistoryRecord(request);
                        mpresponse.Add(result);
                        var result2 =  await _jobRepository.GetSiteBookingPushQueueHistoryRecord(request);      
                        mpresponse.Add(result2);               
                        if(result==null && result2==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);         

                    }

                    break;

                    case BusinessProcessQueue.UpdateBookingInTheSubscriber.PushPublisherBookingUpdates:
                    if(request.CurrentJobsOnly)
                    mpresponse.Add(await _jobRepository.GetSiteBookingPushQueueRecord(request));
                    else
                    mpresponse.Add(await _jobRepository.GetSiteBookingPushQueueHistoryRecord(request));
                    break;


                    case BusinessProcessQueue.UpdateBookingInTheSubscriber.ImportPublisherBookingUpdates:
                    siteresponse = await SendJobInfoRequestToTsapi(request);      
                    
                    break;
                    case BusinessProcessQueue.UpdateBookingInTheSubscriber.ConfirmPublisherBookingUpdates:
                    if(request.CurrentJobsOnly)
                    {
                    var result =  await _jobRepository.GetSiteBookingPushQueueRecord(request);
                    if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result =  await _jobRepository.GetSiteBookingPushQueueHistoryRecord(request);
                        mpresponse.Add(result);
                            if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);   
                    }
                    break;
                    case BusinessProcessQueue.UpdateBookingInTheSubscriber.UpdateSubscriberBooking:
                    if(request.CurrentJobsOnly)
                    {
                    var result = await _jobRepository.GetBookingUpdateFromPublisherQueueRecord(request);
                    if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result = await _jobRepository.GetBookingUpdateFromPublisherQueueHistoryRecord(request);
                        if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);       
                    }                                              
                    
                    break;
                        
                    
                }

                break;
                case BusinessProcess.UpdateBookingInThePublisher :

                BusinessProcessQueue.UpdateBookingInThePublisher queue4 = (BusinessProcessQueue.UpdateBookingInThePublisher)request.ProcessQueueId;

                BusinessProcessJobType.UpdateBookingInThePublisher jobtype4 = (BusinessProcessJobType.UpdateBookingInThePublisher)request.ProcessQueueId;
                switch(queue4)
                {
                    case BusinessProcessQueue.UpdateBookingInThePublisher.PublisherBookingUpdates:
                      if(request.CurrentJobsOnly)
                    {
                        var result = await _jobRepository.GetMarketplaceBookingPushQueueRecord(request);
                        mpresponse.Add(result);
                        var result2 =  await _jobRepository.GetSiteBookingPushQueueRecord(request);      
                        mpresponse.Add(result2);               
                        if(result==null && result2==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result = await _jobRepository.GetMarketplaceBookingPushQueueHistoryRecord(request);
                        mpresponse.Add(result);
                        var result2 =  await _jobRepository.GetSiteBookingPushQueueHistoryRecord(request);      
                        mpresponse.Add(result2);               
                        if(result==null && result2==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);         

                    }

                    break;
                    case BusinessProcessQueue.UpdateBookingInThePublisher.ConfirmPublisherBookingUpdates:
                   if(request.CurrentJobsOnly)
                    {
                    var result = await _jobRepository.GetBookingUpdateFromPublisherQueueRecord(request);
                            mpresponse.Add(result);      
                    }
                    else
                    {
                        var result = await _jobRepository.GetBookingUpdateFromPublisherQueueHistoryRecord(request);
                            mpresponse.Add(result);
                    }   
                    break;

                    case BusinessProcessQueue.UpdateBookingInThePublisher.UpdateSubscriberBooking:
                  
                  if(request.CurrentJobsOnly)
                    {
                    var result = await _jobRepository.GetBookingUpdateFromPublisherQueueRecord(request);
                    mpresponse.Add(result);
                    if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);                  
                    }
                    else
                    {
                        var result = await _jobRepository.GetBookingUpdateFromPublisherQueueHistoryRecord(request);
                        mpresponse.Add(result);
                        if(result==null)                    
                        siteresponse = await SendJobInfoRequestToTsapi(request);       
                    }                                              
                    
                    break;
                        
                    
                }


                break;


            }
 
      if(mpresponse!=null && mpresponse.Count>0)
      {
          response = mpresponse.Where(r=>r.JobId!=null && r.JobId!=Guid.Empty).FirstOrDefault();
      }

        if(siteresponse!=null && siteresponse.JobId!=null)
      {
          response = siteresponse;
      }

      return response;
        } */





        public async Task<JobInfoResponse> JobInfo(JobInfoRequest request)
        {
            
            LoggingHelper.LogInfo(_logger, LogType.Start, "JobInfo", SERVICE_NAME, TraceId);
            var response =  (JobInfoResponse)null;
            var siteresponse = (JobInfoResponse)null;
            
            List<JobInfoResponse> mpresponse =  new List<JobInfoResponse>();
            if(request.SiteId==null ||request.SiteId == Guid.Empty)
            {
            switch (request.QueueTable)
            {
                 case DbJobQueue.BOOKINGUPDATEFROMPUBLISHERQUEUE:
                    response =  request.IsHistory? await _jobRepository.GetBookingUpdateFromPublisherQueueHistoryRecord(request):await _jobRepository.GetBookingUpdateFromPublisherQueueRecord(request);
                    break;
                case DbJobQueue.MARKETPLACEBOOKINGPUSHQUEUE:
                      response =  request.IsHistory? await _jobRepository.GetMarketplaceBookingPushQueueHistoryRecord(request):await _jobRepository.GetMarketplaceBookingPushQueueRecord(request);
                      break;
                case DbJobQueue.PUBLISHEDPRODUCTSQUEUE:
                    response =  request.IsHistory? await _jobRepository.GetPublishedProductsQueueHistoryRecord(request):await _jobRepository.GetPublishedProductsQueueRecord(request);
                      break;
                case DbJobQueue.SITEBOOKINGPUSHQUEUE:
                   response =  request.IsHistory? await _jobRepository.GetSiteBookingPushQueueHistoryRecord(request):await _jobRepository.GetSiteBookingPushQueueRecord(request);
                      break;        
                case DbJobQueue.SUBSCRIBERPRODUCTQUEUE:
                     response =  request.IsHistory? await _jobRepository.GetSubscriberProductsQueueHistoryRecord(request):await _jobRepository.GetSubscriberProductsQueueRecord(request);
                      break;
        
                case DbJobQueue.SUBSCRIBERPRODUCTTSUPDATEQUEUE:
                    response =  request.IsHistory? await _jobRepository.GetSubscriberProductTsUpdateQueueHistoryRecord(request):await _jobRepository.GetSubscriberProductTsUpdateQueueRecord(request);
                      break;


            }

                  return response;
            }
            else 
            {
                siteresponse = await SendJobInfoRequestToTsapi(request);  

                return siteresponse;

            }


        }








      
        public async Task<ResubmitJobResponse> ResubmitJob(ResubmitJobRequest request)
        {
            ResubmitJobResponse response = new ResubmitJobResponse
            {
                IsSuccess = true
            };

            try
            {

                if (request.SiteId != Guid.Empty)
                {
                    var result = await _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Job, "ResubmitJob", null, null, EntityType.Site, request.SiteId);
                    response = JsonConvert.DeserializeObject<Response<ResubmitJobResponse>>(result).ResponseMessage;
        }
                else
                {
                    IResubmitJob resubmitJob = GetDBJobQueueClass(request.QueueTable);

                    if (resubmitJob == null)
                    {
                        response.IsSuccess = false;
                        response.Error = new Error
                        {
                            ErrorMessage = "Invalid DB table enum passed."
                        };
                        return response;
                    }

                    var validateResubmitJob = resubmitJob.ValidateResubmitJob(request);

                    if (!validateResubmitJob.IsSuccess)
                    {
                        response.IsSuccess = false;
                        response.Error = new Error
                        {
                            ErrorMessage = validateResubmitJob.ErrorMessage
                        };
                        return response;
    }

                    response = resubmitJob.ResubmitJob(request);
                    
}
            }
            catch(Exception e)
            {
                response.IsSuccess = false;
                response.Error = new Error
                {
                    ErrorMessage = e.ToString()
                };
            }
            response.JobId = request.JobId;
            return response;
        }

        private IResubmitJob GetDBJobQueueClass(DbJobQueue queue)
        {
            switch (queue)
            {
                case DbJobQueue.BOOKINGUPDATEFROMPUBLISHERQUEUE:
                    return new BookingUpdateFromPublisherQueueResubmit(_jobRepository);
                case DbJobQueue.MARKETPLACEBOOKINGPUSHQUEUE:
                    return new MarketplaceBookingPushQueueResubmit(_jobRepository);
                case DbJobQueue.PUBLISHEDPRODUCTSQUEUE:
                    return new PublisherProductQueueResubmit(_jobRepository);
                case DbJobQueue.SITEBOOKINGPUSHQUEUE:
                    return new SiteBookingPushQueueResubmit(_jobRepository);
                case DbJobQueue.SUBSCRIBERPRODUCTQUEUE:
                    return new SubscriberProductQueueResubmit(_jobRepository);
                case DbJobQueue.SUBSCRIBERPRODUCTTSUPDATEQUEUE:
                    return new SubscriberProductTsUpdateQueueResubmit(_jobRepository);
            }
            return null;
        }
    }
}
