using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Job;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.DAL.Utilities;

namespace MarketPlaceService.DAL
{
    public class JobRepository :BaseRepository, IJobRepository
    {
         private const int MAX_RECORDS = 1000;
          public JobRepository(MarketplaceDbContext context) : base(context)
        {

        }

      public async Task<JobSearchResponse> SearchPublishedProductsQueueHistory(JobSearchRequest request)
      {
           List<Enum> jobTypes  = new List<Enum>();
           List<short> dbJobTypes  = null;
           List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
           Object queueName; 
           //Enum.TryParse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString(),out QueueName);
           queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;
           
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
           foreach(var jobtype in jobTypes)
           map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,(short)jobtype.GetAttribute<DbJobType>().JobType));               
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,(short)jobtype.GetAttribute<DbJobType>().JobType));  
                }
           }

           var records = (from ppqh in _context.PublishedProductsQueueHistory
            join pub in _context.Publisher on ppqh.PublisherId equals pub.PublisherId
            //join site in _context.Site on pub.SiteId equals site.SiteId
            join status in _context.JobHistoryStatus on  ppqh.Jobhistorystatusid equals status.Jobstatusid
            orderby ppqh.CreationDateTime descending
            select new  JobRecord{
                            JobId = ppqh.PublishedProductsQueueHistoryId,
                         //   SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                            Created = ppqh.CreationDateTime,
                            Started = ppqh.JobStartDateTime,
                            DbJobTypeId = (short)ppqh.Jobtypeid,
                            IsHistory = true,
                            QueueTable = DbJobQueue.PUBLISHEDPRODUCTSQUEUE,

                         
                            
                          //JobTypeName =map.FirstOrDefault(m=>(short?)m.Item3 == ppqh.Jobtypeid).Item2.ToString(),
                          // JobTypeName = (from m in map where m.Item3 == (short)ppqh.Jobtypeid select m.Item2.ToString() ).FirstOrDefault(),
                          // ProcessQueueName = (from m in map where m.Item3 == (short)ppqh.Jobtypeid select m.Item1.ToString() ).FirstOrDefault(),
                            Duration = ppqh.JobEndDateTime!=null?((TimeSpan)(ppqh.JobEndDateTime - ppqh.JobStartDateTime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
           
                  
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }
                       
                        foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);  
                       }
              //  records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId):records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;


      }

    public async Task<JobSearchResponse> SearchPublishedProductsQueue(JobSearchRequest request)
      {
            List<Enum> jobTypes  = new List<Enum>();
            List<short> dbJobTypes  = null;
           List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
            Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
           if(request.ProcessQueueId>0 && queueName is Enum)           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
           foreach(var jobtype in jobTypes)
           map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,(short)jobtype.GetAttribute<DbJobType>().JobType));               
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,(short)jobtype.GetAttribute<DbJobType>().JobType));  
                }
           }

          

            var records = (from ppq in _context.PublishedProductsQueue
            join pub in _context.Publisher on ppq.PublisherId equals pub.PublisherId
           // join site in _context.Site on pub.SiteId equals site.SiteId
            join status in _context.JobStatus on  ppq.Jobstatusid equals status.Jobstatusid
            orderby ppq.CreationDateTime descending
            select new  JobRecord{
                            JobId = ppq.PublishedProductQueueId,
                         //   SiteId =site.SiteId,                           
                         //   SiteName =  site.SiteName,
                            StatusId = status.Jobstatusid,
                            StatusName = status.Jobstatusname,
                            Created = ppq.CreationDateTime,
                            Started = ppq.JobStartDateTime,
                            DbJobTypeId = (short)ppq.Jobtypeid,                            
                            IsHistory = false,
                            QueueTable = DbJobQueue.PUBLISHEDPRODUCTSQUEUE,
                          //  JobTypeName = (from m in map where m.Item3 == (short)ppq.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                         //   ProcessQueueName = (from m in map where m.Item3 == (short)ppq.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Duration = ppq.JobEndDateTime!=null?((TimeSpan)(ppq.JobEndDateTime - ppq.JobStartDateTime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();

                
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }
                      foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }

                       
                records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;

           JobSearchResponse response = new JobSearchResponse(){JobRecords = new List<JobRecord>()};
           if(records!=null )
           response.JobRecords = records.ToList();
           return response;
      }


         public async Task<JobSearchResponse> SearchSubscriberProductsQueue(JobSearchRequest request)
      {

           List<Enum> jobTypes  = new List<Enum>();
           List<short> dbJobTypes  = null;
           List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
          Object queueName; 
          queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;
          var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
          short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }


          var records = (from spq in _context.SubscriberProductQueue
            join sub in _context.Subscriber on spq.Subscriberid equals sub.SubscriberId
          //  join site in _context.Site on sub.SiteId equals site.SiteId
            join status in _context.JobStatus on  spq.Jobstatusid equals status.Jobstatusid
            orderby spq.Jobcreationdatetime descending
            select new  JobRecord{
                            JobId = spq.Subscriberproductqueueid,
                           // SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                            Created = spq.Jobcreationdatetime,
                            Started = spq.Jobstartdatetime,
                            DbJobTypeId = (short)spq.Jobtypeid,                            
                            IsHistory = false,
                            QueueTable = DbJobQueue.SUBSCRIBERPRODUCTQUEUE,
                         //     JobTypeName = (from m in map where m.Item3 == (short)spq.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                         //   ProcessQueueName = (from m in map where m.Item3 == (short)spq.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Duration = spq.Jobenddatetime!=null?((TimeSpan)(spq.Jobenddatetime - spq.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
           
               
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }

                     foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }
                       
                records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;

          
      }


      public async Task<JobSearchResponse> SearchSubscriberProductsQueueHistory(JobSearchRequest request)
      {
          
          List<Enum> jobTypes  = new List<Enum>();
          List<short> dbJobTypes  = null;
          List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
            Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;     
           
          var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
            short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }

                    
            var records = (from spqh in _context.SubscriberProductQueueHistory
            join sub in _context.Subscriber on spqh.Subscriberid equals sub.SubscriberId
           // join site in _context.Site on sub.SiteId equals site.SiteId
            join status in _context.JobHistoryStatus on  spqh.Jobhistorystatusid equals status.Jobstatusid
            orderby spqh.Jobcreationdatetime descending
            select new  JobRecord{
                            JobId = spqh.Subscriberproductqueuehistoryid,
                           // SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                            Created = spqh.Jobcreationdatetime,
                            Started = spqh.Jobstartdatetime,
                            DbJobTypeId = (short)spqh.Jobtypeid,                            
                            IsHistory = true,
                            QueueTable = DbJobQueue.SUBSCRIBERPRODUCTQUEUE,
                            // JobTypeName = (from m in map where m.Item3 == (short)spqh.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                            //ProcessQueueName = (from m in map where m.Item3 == (short)spqh.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Duration = spqh.Jobenddatetime!=null?((TimeSpan)(spqh.Jobenddatetime - spqh.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
         
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }

                     foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }
                       
                records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
          
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }


      public async Task<JobSearchResponse> SearchSubscriberProductTsUpdateQueue(JobSearchRequest request)
      {
            List<Enum> jobTypes  = new List<Enum>();
            List<short> dbJobTypes  = null;
          List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
           Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;       
            
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
             short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }


                    
            var records = (from sptsuq in _context.SubscriberProductTsUpdateQueue
            join sub in _context.Subscriber on sptsuq.Subscriberid equals sub.SubscriberId
           // join site in _context.Site on sub.SiteId equals site.SiteId
            join status in _context.JobStatus on  sptsuq.Jobstatusid equals status.Jobstatusid
            orderby sptsuq.Jobcreationdatetime descending
            select new  JobRecord{
                            JobId = sptsuq.Subscriberproducttsupdatequeueid,
                           // SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                           // DbJobTypeId = (short)sptsuq.jobtyp,
                            Created = sptsuq.Jobcreationdatetime,
                            Started = sptsuq.Jobstartdatetime,
                            
                            IsHistory = false,
                            QueueTable = DbJobQueue.SUBSCRIBERPRODUCTTSUPDATEQUEUE,
                         //JobTypeName = (from m in map where m.Item3 == (short)sptsuq. select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                         // ProcessQueueName = (from m in map where m.Item3 == (short)sptsuq.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Duration = sptsuq.Jobenddatetime!=null?((TimeSpan)(sptsuq.Jobenddatetime - sptsuq.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
         
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }

                     foreach (JobRecord r in records )
                       { 
                           // if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  

                            r.JobTypeId =  Convert.ToInt16(BusinessProcessJobType.SubscribingAtTheSubscriber.ConfirmProductImport);
                            r.JobTypeName =  BusinessProcessJobType.SubscribingAtTheSubscriber.ConfirmProductImport.GetDescription();                                 
                            r.ProcessQueueId = Convert.ToInt16(BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductImport);                           
                            r.ProcessQueueName = BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductImport.GetDescription();
                           // r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                          //  r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                          //  r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                          //  r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }
                       
                //records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
              //  records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
          
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }
      

        public async Task<JobSearchResponse> SearchSubscriberProductTsUpdateQueueHistory(JobSearchRequest request)
      {
            List<Enum> jobTypes  = new List<Enum>();
            List<short> dbJobTypes  = null;
            List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
            Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;       
           
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
          short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
                            
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }

          
            var records = (from sptsuqh in _context.SubscriberProductTsUpdateQueueHistory
            join sub in _context.Subscriber on sptsuqh.Subscriberid equals sub.SubscriberId
          //  join site in _context.Site on sub.SiteId equals site.SiteId
            join status in _context.JobHistoryStatus on  sptsuqh.Jobhistorystatusid equals status.Jobstatusid
            orderby sptsuqh.Jobcreationdatetime descending
            select new  JobRecord{
                            JobId = sptsuqh.Subscriberproducttsupdatequeuehistoryid,
                          //  SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                            Created = sptsuqh.Jobcreationdatetime,
                            Started = sptsuqh.Jobstartdatetime,
                            IsHistory = true,
                            QueueTable = DbJobQueue.SUBSCRIBERPRODUCTTSUPDATEQUEUE,
                         
                     //       JobTypeName = (from m in map where m.Item3 == (short)sptsuqh.jo select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                      //    ProcessQueueName = (from m in map where m.Item3 == (short)sptsuq.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Duration = sptsuqh.Jobenddatetime!=null?((TimeSpan)(sptsuqh.Jobenddatetime - sptsuqh.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
            
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }


                     foreach (JobRecord r in records )
                       { 
                           
                                                  
                            r.JobTypeId =  Convert.ToInt16(BusinessProcessJobType.SubscribingAtTheSubscriber.ConfirmProductImport);
                            r.JobTypeName =  BusinessProcessJobType.SubscribingAtTheSubscriber.ConfirmProductImport.GetDescription();                                 
                            r.ProcessQueueId = Convert.ToInt16(BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductImport);                           
                            r.ProcessQueueName = BusinessProcessQueue.SubscribingAtTheSubscriber.ConfirmProductImport.GetDescription();
                       
                            /* 
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                    */   
                     }
                       
              //  records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
        //        records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
          
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }



        public async Task<JobSearchResponse> SearchMarketplaceBookingPushQueue(JobSearchRequest request)
        {
            List<Enum> jobTypes = new List<Enum>();
            List<short> dbJobTypes = null;
            List<Tuple<Enum, Enum, short>> map = new List<Tuple<Enum, Enum, short>>();
            Object queueName;
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess], request.ProcessQueueId.ToString()) : null;

            var queues = Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
            short jobTypeId;
            if (request.ProcessQueueId > 0 && queueName is Enum)
            {
                jobTypes = BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
                foreach (var jobtype in jobTypes)
                {
                    jobTypeId = 0;
                    if (jobtype.GetAttribute<DbJobType>() != null) { jobTypeId = (short)jobtype.GetAttribute<DbJobType>().JobType; }
                    else if (jobtype.GetAttribute<DbCallBackJobType>() != null) { jobTypeId = (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType; }
                    else if (jobtype.GetAttribute<DbJobTypeId>() != null) { jobTypeId = (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId; }


                    map.Add(new Tuple<Enum, Enum, short>((Enum)queueName, jobtype, jobTypeId));
                }
            }
            else
            {
                foreach (var q in queues)
                {
                    jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                    foreach (var jobtype in jobTypes)
                    {
                        jobTypeId = 0;
                        if (jobtype.GetAttribute<DbJobType>() != null) { jobTypeId = (short)jobtype.GetAttribute<DbJobType>().JobType; }
                        else if (jobtype.GetAttribute<DbCallBackJobType>() != null) { jobTypeId = (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType; }
                        else if (jobtype.GetAttribute<DbJobTypeId>() != null) { jobTypeId = (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId; }

                        map.Add(new Tuple<Enum, Enum, short>((Enum)q, jobtype, jobTypeId));
                    }

                }
            }


            var records = (from mpbpq in _context.MarketplaceBookingPushQueue
                               //  join site in _context.Site on mpbpq.Subscribersiteid equals site.SiteId
                           join status in _context.JobStatus on mpbpq.Jobstatusid equals status.Jobstatusid
                           orderby mpbpq.Jobcreateddatetime descending
                           select new JobRecord
                           {
                               JobId = mpbpq.Marketplacebookingpushqueueid,
                               // SiteName =  site.SiteName,
                               StatusName = status.Jobstatusname,
                               Created = (DateTime)mpbpq.Jobcreateddatetime,
                               Started = mpbpq.Jobstartdatetime,
                               DbJobTypeId = (short)mpbpq.Jobtypeid,

                               IsHistory = false,
                               QueueTable = DbJobQueue.MARKETPLACEBOOKINGPUSHQUEUE,
                               //     JobTypeName = (from m in map where m.Item3 == (short)mpbpq.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                               //     ProcessQueueName = (from m in map where m.Item3 == (short)mpbpq.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                               Duration = mpbpq.Jobenddatetime != null ? ((TimeSpan)(mpbpq.Jobenddatetime - mpbpq.Jobstartdatetime)).TotalMilliseconds : 0
                           }).Take(MAX_RECORDS).ToList();


            dbJobTypes = new List<short>();
            foreach (Enum j in jobTypes)
            {
                DbJobType jobType = j.GetAttribute<DbJobType>();
                if (jobType != null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                      DbCallBackJobType DbCallBackJobTypeId = j.GetAttribute<DbCallBackJobType>();
                   if(DbCallBackJobTypeId!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbCallBackJobType>().callBackJobType);

                     DbJobTypeId DbJobTypeId = j.GetAttribute<DbJobTypeId>();
                   if(DbJobTypeId!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobTypeId>().dbJobTypeId);
            }


            foreach (JobRecord r in records)
            {
                if (map.FirstOrDefault(m => (short?)m.Item3 == r.DbJobTypeId) == null) continue;

                r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m => (short?)m.Item3 == r.DbJobTypeId).Item2);
                r.JobTypeName = map.FirstOrDefault(m => (short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m => (short?)m.Item3 == r.DbJobTypeId).Item1);
                r.ProcessQueueName = map.FirstOrDefault(m => (short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();
            }

            //  records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
            records = request.JobStatusId != null ? records.Where(r => r.StatusId == request.JobStatusId).ToList() : records;
            records = request.FromDate != null ? records.Where(r => r.Created >= request.FromDate).ToList() : records;
            records = request.ToDate != null ? records.Where(s => s.Created <= request.ToDate).ToList() : records;
            records = dbJobTypes.Count > 0 ? records.Where(s => dbJobTypes.Contains(s.DbJobTypeId)).ToList() : records;

            JobSearchResponse response = new JobSearchResponse { JobRecords = records.ToList() };
            return response;
        }

      public async Task<JobSearchResponse> SearchMarketplaceBookingPushQueueHistory(JobSearchRequest request)
      {
         List<Enum> jobTypes  = new List<Enum>();
         List<short> dbJobTypes  = null;
         List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
         Object queueName; 
         queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;  
            
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
  short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }

           
            var records = (from mpbpqh in _context.MarketplaceBookingPushQueueHistory          
           // join site in _context.Site on mpbpqh.Subscribersiteid equals site.SiteId
            join status in _context.JobHistoryStatus on  mpbpqh.Jobstatusid equals status.Jobstatusid
            orderby mpbpqh.Jobcreateddatetime descending
            select new  JobRecord{
                            JobId = mpbpqh.Marketplacebookingpushqueuehistoryid,
                           // SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                            DbJobTypeId = (short) mpbpqh.Jobtypeid,
                            
                            IsHistory = false,
                            QueueTable = DbJobQueue.MARKETPLACEBOOKINGPUSHQUEUE,
                           // JobTypeName = (from m in map where m.Item3 == (short)mpbpqh.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                           // ProcessQueueName = (from m in map where m.Item3 == (short)mpbpqh.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Created = (DateTime)mpbpqh.Jobcreateddatetime,
                            Started = mpbpqh.Jobstartdatetime,
                            Duration = mpbpqh.Jobenddatetime!=null?((TimeSpan)(mpbpqh.Jobenddatetime - mpbpqh.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
           
           
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                      DbCallBackJobType DbCallBackJobTypeId = j.GetAttribute<DbCallBackJobType>();
                   if(DbCallBackJobTypeId!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbCallBackJobType>().callBackJobType);

                     DbJobTypeId DbJobTypeId = j.GetAttribute<DbJobTypeId>();
                   if(DbJobTypeId!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobTypeId>().dbJobTypeId);
                 }


                     foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }
                       


             //   records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }


 public async Task<JobSearchResponse> SearchBookingUpdateFromPublisherQueue(JobSearchRequest request)
      {
            List<Enum> jobTypes  = new List<Enum>();
            List<short> dbJobTypes  = null;
            List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
            Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;       
           
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
  short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }

          
            var records = (from bufpq in _context.BookingUpdateFromPublisherQueue           
            //join site in _context.Site on bufpq.Publishersiteid equals site.SiteId
            join status in _context.JobStatus on  bufpq.Jobstatusid equals status.Jobstatusid
            orderby bufpq.Jobcreationdatetime descending
            select new  JobRecord{
                            JobId = bufpq.Bookingupdatefrompublisherqueueid,
                          //  SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                            DbJobTypeId =(short)bufpq.Jobtypeid,
                            
                            IsHistory = false,
                            QueueTable = DbJobQueue.BOOKINGUPDATEFROMPUBLISHERQUEUE,
                           // JobTypeName = (from m in map where m.Item3 == (short)bufpq.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                          //  ProcessQueueName = (from m in map where m.Item3 == (short)bufpq.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Created = bufpq.Jobcreationdatetime,
                            Started = bufpq.Jobstartdatetime,
                            Duration = bufpq.Jobenddatetime!=null?((TimeSpan)(bufpq.Jobenddatetime - bufpq.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
           
                  
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);

                       DbCallBackJobType DbCallBackJobTypeId = j.GetAttribute<DbCallBackJobType>();
                   if(DbCallBackJobTypeId!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbCallBackJobType>().callBackJobType);

                     DbJobTypeId DbJobTypeId = j.GetAttribute<DbJobTypeId>();
                   if(DbJobTypeId!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobTypeId>().dbJobTypeId);
                 }

                     foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }
                       
              //  records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }




      public async Task<JobSearchResponse> SearchBookingUpdateFromPublisherQueueHistory(JobSearchRequest request)
      {
           List<Enum> jobTypes  = new List<Enum>();
           List<short> dbJobTypes  = null;
           List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
            Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;     
           
           
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
  short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }
          
          
            var records = (from bufpqh in _context.BookingUpdateFromPublisherQueueHistory          
            //join site in _context.Site on bufpqh.Publishersiteid equals site.SiteId
            join status in _context.JobHistoryStatus on  bufpqh.Jobstatusid equals status.Jobstatusid
            orderby bufpqh.Jobcreationdatetime descending
            select new  JobRecord{
                            JobId = bufpqh.Bookingupdatefrompublisherqueuehistoryid,
                            DbJobTypeId = (short) bufpqh.Jobtypeid,
                            
                            IsHistory = true,
                            QueueTable = DbJobQueue.BOOKINGUPDATEFROMPUBLISHERQUEUE,
                           // SiteName =  site.SiteName,
                           // JobTypeName = (from m in map where m.Item3 == (short)bufpqh.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                           // ProcessQueueName = (from m in map where m.Item3 == (short)bufpqh.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            StatusName = status.Jobstatusname,
                            Created = bufpqh.Jobcreationdatetime,
                            Started = bufpqh.Jobstartdatetime,
                            Duration = bufpqh.Jobenddatetime!=null?((TimeSpan)(bufpqh.Jobenddatetime - bufpqh.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
           
           
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                    
                     DbCallBackJobType DbCallBackJobTypeId = j.GetAttribute<DbCallBackJobType>();
                   if(DbCallBackJobTypeId!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbCallBackJobType>().callBackJobType);

                   
                 }
                       
                          foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }

            //    records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }


      public async Task<JobSearchResponse> SearchSiteBookingPushQueue(JobSearchRequest request)
      {
          
            List<Enum> jobTypes  = new List<Enum>();
            List<short> dbJobTypes  = null;
           List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
           Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;
                      
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
      short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }

            var records = (from spbq in _context.SiteBookingPushQueue         
           // join site in _context.Site on spbq.Publishersiteid equals site.SiteId
            join status in _context.JobStatus on  spbq.Jobstatusid equals status.Jobstatusid
            orderby spbq.Jobcreateddatetime descending
            select new  JobRecord{
                            JobId = spbq.Sitebookingpushqueueid,
                        //    SiteName =  site.SiteName,
                            StatusName = status.Jobstatusname,
                            DbJobTypeId  = (short) spbq.Jobtypeid,
                            
                            IsHistory = false,
                            QueueTable = DbJobQueue.SITEBOOKINGPUSHQUEUE,
                          //  JobTypeName = (from m in map where m.Item3 == (short)spbq.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                           // ProcessQueueName = (from m in map where m.Item3 == (short)spbq.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            Created = (DateTime)spbq.Jobcreateddatetime,
                            Started = spbq.Jobstartdatetime,
                            Duration = spbq.Jobenddatetime!=null?((TimeSpan)(spbq.Jobenddatetime - spbq.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();
           
           
                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }
                       
    foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }

              //  records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;

           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }


            public async Task<JobSearchResponse> SearchSiteBookingPushQueueHistory(JobSearchRequest request)
      {
            List<Enum> jobTypes  = new List<Enum>();
            List<short> dbJobTypes  = null;
            List<Tuple<Enum,Enum,short>> map =  new List<Tuple<Enum, Enum, short>>();
             Object queueName; 
            queueName = request.ProcessQueueId != null ? Enum.Parse(BusinessProcessQueue.QueueMapping[request.BusinessProcess],request.ProcessQueueId.ToString()) : null;   
           
           var queues =  Enum.GetValues(BusinessProcessQueue.QueueMapping[request.BusinessProcess]);
  short jobTypeId ;
           if(request.ProcessQueueId>0 && queueName is Enum)
           {
           jobTypes =  BusinessProcessJobType.QueueJobTypeMapping[(Enum)queueName];
            foreach(var jobtype in jobTypes)
                {
                     jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                
                     map.Add(new Tuple<Enum, Enum, short>((Enum)queueName,jobtype,jobTypeId));    
                }
           }
           else
           {
                foreach (var q in queues ){jobTypes.AddRange(BusinessProcessJobType.QueueJobTypeMapping[(Enum)q]);
                foreach(var jobtype in jobTypes)
                {
                    jobTypeId=0;
                    if(jobtype.GetAttribute<DbJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobType>().JobType ;}
                    else if(jobtype.GetAttribute<DbCallBackJobType>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbCallBackJobType>().callBackJobType ;}
                    else if(jobtype.GetAttribute<DbJobTypeId>()!=null) {jobTypeId= (short)jobtype.GetAttribute<DbJobTypeId>().dbJobTypeId ;}
            
                map.Add(new Tuple<Enum, Enum, short>((Enum)q,jobtype,jobTypeId)); 
                }

                }
           }

          
          
            var records = (from spbqh in _context.SiteBookingPushQueueHistory         
           // join site in _context.Site on spbqh.Publishersiteid equals site.SiteId
            join status in _context.JobStatus on  spbqh.Jobstatusid equals status.Jobstatusid
            orderby spbqh.Jobcreateddatetime descending
            select new  JobRecord{
                            JobId = spbqh.Sitebookingpushqueuehistoryid,
                            //SiteName =  site.SiteName,
                            DbJobTypeId = (short) spbqh.Jobtypeid,                            
                            IsHistory = true,
                            QueueTable = DbJobQueue.SITEBOOKINGPUSHQUEUE,
                          //  JobTypeName = (from m in map where m.Item3 == (short)spbqh.Jobtypeid select m.Item2.GetDescription().ToString() ).FirstOrDefault(),
                           // ProcessQueueName = (from m in map where m.Item3 == (short)spbqh.Jobtypeid select m.Item1.GetDescription().ToString() ).FirstOrDefault(),
                            StatusName = status.Jobstatusname,
                            Created = (DateTime)spbqh.Jobcreateddatetime,
                            Started = spbqh.Jobstartdatetime,
                            Duration = spbqh.Jobenddatetime!=null?((TimeSpan)(spbqh.Jobenddatetime - spbqh.Jobstartdatetime)).TotalMilliseconds:0
                            }).Take(MAX_RECORDS).ToList();


                 dbJobTypes  =  new List<short>();
                 foreach(Enum j  in jobTypes)
                 {
                   DbJobType jobType = j.GetAttribute<DbJobType>();
                   if(jobType!=null)
                    dbJobTypes.Add((short)j.GetAttribute<DbJobType>().JobType);
                 }

                     foreach (JobRecord r in records )
                       { 
                            if(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId)==null) continue;
                                                  
                            r.JobTypeId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2);                         
                            r.JobTypeName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item2.GetDescription().ToString();
                            r.ProcessQueueId = Convert.ToInt16(map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1);
                            r.ProcessQueueName = map.FirstOrDefault(m=>(short?)m.Item3 == r.DbJobTypeId).Item1.GetDescription().ToString();                           
                       }
                       
             //   records = request.SiteId!=null? records.Where(r=>r.SiteId == request.SiteId).ToList():records;
                records = request.JobStatusId !=null? records.Where(r=> r.StatusId== request.JobStatusId).ToList():records;
                records = request.FromDate!=null? records.Where(r=>r.Created >= request.FromDate  ).ToList():records;
                records = request.ToDate!=null? records.Where(s=>s.Created <= request.ToDate  ).ToList():records;               
                records =  dbJobTypes.Count>0?records.Where(s=>dbJobTypes.Contains(s.DbJobTypeId)).ToList():records;           
          
           JobSearchResponse response = new JobSearchResponse{JobRecords = records.ToList()};
           return response;
      }




       public async Task<JobInfoResponse> GetPublishedProductsQueueRecord(JobInfoRequest request)
       {
             var record = (from q in _context.PublishedProductsQueue
             join status in _context.JobStatus on  q.Jobstatusid equals status.Jobstatusid
            where q.PublishedProductQueueId == request.JobId
            select new  JobInfoResponse{
                            JobId = q.PublishedProductQueueId,
                            JobNote = q.ProcessingNote,
                            JobCreatedDate = q.CreationDateTime,
                            JobStartDate = q.JobStartDateTime,
                            JobEndDate = q.JobEndDateTime,
                            ProductId =q.ProductId,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.RetryCount,
                            ProcessedBy =  q.ProcessedBy
                        
          }).FirstOrDefault();
          return record;
          
        }       
      public async  Task<JobInfoResponse> GetPublishedProductsQueueHistoryRecord(JobInfoRequest request)
      {
             var record = (from q in _context.PublishedProductsQueueHistory
             join status in _context.JobHistoryStatus on  q.Jobhistorystatusid equals status.Jobstatusid
            where q.PublishedProductsQueueHistoryId == request.JobId
            select new  JobInfoResponse{
                            JobId = q.PublishedProductsQueueHistoryId,
                            JobNote = q.ProcessingNote,
                            JobCreatedDate = q.CreationDateTime,
                            JobStartDate = q.JobStartDateTime,
                            JobEndDate = q.JobEndDateTime,
                            ProductId =q.ProductId,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.RetryCount,
                            ProcessedBy =  q.ProcessedBy
                        
          }).FirstOrDefault();
          return record;
      }
     
        public async Task<JobInfoResponse> GetSubscriberProductsQueueRecord(JobInfoRequest request)
        {
             var record = (from q in _context.SubscriberProductQueue
             join status in _context.JobStatus on  q.Jobstatusid equals status.Jobstatusid
             where q.Subscriberproductqueueid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Subscriberproductqueueid,
                            JobNote = q.Jobnote,
                            JobCreatedDate = q.Jobcreationdatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                           // ProductId =q.ProductId,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                            ProcessedBy =  q.Processedby
                        
          }).FirstOrDefault();
          return record;
        }

       public async Task<JobInfoResponse> GetSubscriberProductsQueueHistoryRecord(JobInfoRequest request)
       {
              var record = (from q in _context.SubscriberProductQueueHistory
             join status in _context.JobHistoryStatus on  q.Jobhistorystatusid equals status.Jobstatusid
             where q.Subscriberproductqueueid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Subscriberproductqueuehistoryid,
                            JobNote = q.Jobnote,
                            JobCreatedDate = q.Jobcreationdatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                           // ProductId =q.ProductId,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                            ProcessedBy =  q.Processedby
                        
          }).FirstOrDefault();
          return record;
       }

       public async Task<JobInfoResponse> GetSubscriberProductTsUpdateQueueRecord(JobInfoRequest request)
       {
             var record = (from q in _context.SubscriberProductTsUpdateQueue
             join status in _context.JobStatus on  q.Jobstatusid equals status.Jobstatusid
             where q.Subscriberproducttsupdatequeueid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Subscriberproducttsupdatequeueid,
                            JobNote = q.Jobnote,
                            JobCreatedDate = q.Jobcreationdatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                           // ProductId =q.ProductId,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                           // ProcessedBy =  q.
                        
          }).FirstOrDefault();
          return record;
       }

     public async   Task<JobInfoResponse> GetSubscriberProductTsUpdateQueueHistoryRecord(JobInfoRequest request)
     {
          
          
             var record = (from q in _context.SubscriberProductTsUpdateQueueHistory
             join status in _context.JobHistoryStatus on  q.Jobhistorystatusid equals status.Jobstatusid
             where q.Subscriberproducttsupdatequeuehistoryid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Subscriberproducttsupdatequeuehistoryid,
                            JobNote = q.Jobnote,
                            JobCreatedDate = q.Jobcreationdatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                           // ProductId =q.ProductId,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                           // ProcessedBy =  q.
                        
          }).FirstOrDefault();
          return record;
     }

     public async  Task<JobInfoResponse> GetMarketplaceBookingPushQueueRecord(JobInfoRequest request)
     {
          var record = (from q in _context.MarketplaceBookingPushQueue
             join status in _context.JobStatus on  q.Jobstatusid equals status.Jobstatusid
             where q.Marketplacebookingpushqueueid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Marketplacebookingpushqueueid,
                            JobNote = q.Processingnote,
                            JobCreatedDate = q.Jobcreateddatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                            BookingId =q.Bookingid,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                           // ProcessedBy =  q.
                        
          }).FirstOrDefault();
          return record;

     }

      public async  Task<JobInfoResponse> GetMarketplaceBookingPushQueueHistoryRecord(JobInfoRequest request)
      {
           var record = (from q in _context.MarketplaceBookingPushQueueHistory
             join status in _context.JobHistoryStatus on  q.Jobstatusid equals status.Jobstatusid
             where q.Marketplacebookingpushqueuehistoryid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Marketplacebookingpushqueuehistoryid,
                            JobNote = q.Processingnote,
                            JobCreatedDate = q.Jobcreateddatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                            BookingId =q.Bookingid,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                           // ProcessedBy =  q.
                        
          }).FirstOrDefault();
          return record;
      }


       public async Task<JobInfoResponse> GetBookingUpdateFromPublisherQueueRecord(JobInfoRequest request)
       {
            var record = (from q in _context.BookingUpdateFromPublisherQueue
             join status in _context.JobStatus on  q.Jobstatusid equals status.Jobstatusid
             where q.Bookingupdatefrompublisherqueueid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Bookingupdatefrompublisherqueueid,
                            JobNote = q.Jobnote,
                            JobCreatedDate = q.Jobcreationdatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                           // BookingId =q.,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                           // ProcessedBy =  q.
                        
          }).FirstOrDefault();
          return record;
       }

       public async  Task<JobInfoResponse> GetBookingUpdateFromPublisherQueueHistoryRecord(JobInfoRequest request)
       {

             var record = (from q in _context.BookingUpdateFromPublisherQueueHistory
             join status in _context.JobHistoryStatus on  q.Jobstatusid equals status.Jobstatusid
             where q.Bookingupdatefrompublisherqueuehistoryid == request.JobId
             select new  JobInfoResponse{
                            JobId = q.Bookingupdatefrompublisherqueuehistoryid,
                            JobNote = q.Jobnote,
                            JobCreatedDate = q.Jobcreationdatetime,
                            JobStartDate = q.Jobstartdatetime,
                            JobEndDate = q.Jobenddatetime,
                           // BookingId =q.,
                            JobStatus = status.Jobstatusname,
                            RetryCount = q.Retrycount,
                           // ProcessedBy =  q.
                        
          }).FirstOrDefault();
          return record;

       }

      public async  Task<JobInfoResponse> GetSiteBookingPushQueueRecord(JobInfoRequest request)
      {

               var record = (from q in _context.SiteBookingPushQueue
                         join status in _context.JobStatus on  q.Jobstatusid equals status.Jobstatusid
                         where q.Sitebookingpushqueueid == request.JobId
                         select new  JobInfoResponse{
                                        JobId = q.Sitebookingpushqueueid,
                                        JobNote = q.Processingnote,
                                        JobCreatedDate = q.Jobcreateddatetime,
                                        JobStartDate = q.Jobstartdatetime,
                                        JobEndDate = q.Jobenddatetime,
                                       //  BookingId =q,
                                        JobStatus = status.Jobstatusname,
                                        RetryCount = q.Retrycount,
                                    //     ProcessedBy =  q.p
                                   
                         }).FirstOrDefault();
                         return record;
      }

     public async Task<JobInfoResponse> GetSiteBookingPushQueueHistoryRecord(JobInfoRequest request)
     {

          var record = (from q in _context.SiteBookingPushQueueHistory
                    join status in _context.JobHistoryStatus on  q.Jobstatusid equals status.Jobstatusid
                    where q.Sitebookingpushqueuehistoryid == request.JobId
                    select new  JobInfoResponse{
                                   JobId = q.Sitebookingpushqueuehistoryid,
                                   JobNote = q.Processingnote,
                                   JobCreatedDate = q.Jobcreateddatetime,
                                   JobStartDate = q.Jobstartdatetime,
                                   JobEndDate = q.Jobenddatetime,
                                   // BookingId =q.,
                                   JobStatus = status.Jobstatusname,
                                   RetryCount = q.Retrycount,
                                   // ProcessedBy =  q.
                              
                    }).FirstOrDefault();
                    return record;

     }


        public bool SaveChanges()
        {
            _context.SaveChanges();
            return true;
        }

        public BookingUpdateFromPublisherQueue UpdateBookingUpdateFromPublisherQueueData(Guid queueId)
        {
            var queue = _context.BookingUpdateFromPublisherQueue.FirstOrDefault(a => a.Bookingupdatefrompublisherqueueid == queueId);
            queue.Jobstatusid = 1;
            queue.Retrycount = 1;

            _context.BookingUpdateFromPublisherQueue.Update(queue);
            return queue;
        }

        public MarketplaceBookingPushQueue UpdateMarketplaceBookingPushQueueData(Guid queueId)
        {
            var queue = _context.MarketplaceBookingPushQueue.FirstOrDefault(a => a.Marketplacebookingpushqueueid == queueId);
            queue.Jobstatusid = 1;
            queue.Retrycount = 1;

            _context.MarketplaceBookingPushQueue.Update(queue);
            return queue;
        }

        public PublishedProductsQueue UpdatePublishedProductsQueueData(Guid queueId)
        {
            var queue = _context.PublishedProductsQueue.FirstOrDefault(a => a.PublishedProductQueueId == queueId);
            queue.Jobstatusid = 1;
            queue.RetryCount = 1;

            _context.PublishedProductsQueue.Update(queue);
            return queue;
        }

        public SiteBookingPushQueue UpdateSiteBookingPushQueueData(Guid queueId)
        {
            var queue = _context.SiteBookingPushQueue.FirstOrDefault(a => a.Sitebookingpushqueueid == queueId);
            queue.Jobstatusid = 1;
            queue.Retrycount = 1;

            _context.SiteBookingPushQueue.Update(queue);
            return queue;
        }

        public SubscriberProductQueue UpdateSubscriberProductQueueData(Guid queueId)
        {
            var queue = _context.SubscriberProductQueue.FirstOrDefault(a => a.Subscriberproductqueueid == queueId);
            queue.Jobstatusid = 1;
            queue.Retrycount = 1;

            _context.SubscriberProductQueue.Update(queue);
            return queue;
        }

        public Models.SubscriberProductTsUpdateQueue UpdateSubscriberProductTsUpdateQueueData(Guid queueId)
        {
            var queue = _context.SubscriberProductTsUpdateQueue.FirstOrDefault(a => a.Subscriberproducttsupdatequeueid == queueId);
            queue.Jobstatusid = 1;
            queue.Retrycount = 1;

            _context.SubscriberProductTsUpdateQueue.Update(queue);
            return queue;
        }

        //test

        public BookingUpdateFromPublisherQueue InsertBookingUpdateFromPublisherQueueData(Guid queueHistoryId, Guid traceId)
        {
            var historyRecord = _context.BookingUpdateFromPublisherQueueHistory.FirstOrDefault(a => a.Bookingupdatefrompublisherqueuehistoryid == queueHistoryId);

            BookingUpdateFromPublisherQueue queueRecord = new BookingUpdateFromPublisherQueue
            {
                Publisherbookingid = historyRecord.Publisherbookingid,
                Publishersiteid = historyRecord.Publishersiteid,
                Retrycount = 1,
                Jobstatusid = 1,
                Jobtypeid = historyRecord.Jobtypeid,
                Jobnote = string.Empty,
                Jobcreationdatetime = DateTime.UtcNow,
                Traceid = traceId,
                Sitebookingid = historyRecord.Sitebookingid
            };

            _context.BookingUpdateFromPublisherQueue.Add(queueRecord);
            return queueRecord;
        }

        public MarketplaceBookingPushQueue InsertMarketplaceBookingPushQueueData(Guid queueHistoryId, Guid traceId)
        {
            var historyRecord = _context.MarketplaceBookingPushQueueHistory.FirstOrDefault(a => a.Marketplacebookingpushqueuehistoryid == queueHistoryId);

            MarketplaceBookingPushQueue queueRecord = new MarketplaceBookingPushQueue
            {
                Bookingid = historyRecord.Bookingid,
                Subscribersiteid = historyRecord.Subscribersiteid,
                Jobcreateddatetime = DateTime.UtcNow,
                Processingnote = string.Empty,
                Jobstatusid = 1,
                Traceid = traceId,
                Jobtypeid = historyRecord.Jobtypeid,
                Retrycount = 1
            };

            _context.MarketplaceBookingPushQueue.Add(queueRecord);

            return queueRecord;
        }

        public PublishedProductsQueue InsertPublishedProductsQueueData(Guid queueHistoryId, Guid traceId)
        {
            var historyRecord = _context.PublishedProductsQueueHistory.FirstOrDefault(a => a.PublishedProductsQueueHistoryId == queueHistoryId);

            PublishedProductsQueue queueRecord = new PublishedProductsQueue
            {
                PublishedProductTypeId = historyRecord.PublishedProductTypeId,
                PublishedProductId = historyRecord.PublishedProductId,
                PublisherId = historyRecord.PublisherId,
                ProcessingNote = string.Empty,
                ProductId = historyRecord.ProductId.Value,
                RetryCount = 1,
                CreationDateTime = DateTime.UtcNow,
                ProcessedBy = historyRecord.ProcessedBy,
                Jobstatusid = 1,
                Jobtypeid = historyRecord.Jobtypeid,
                Traceid = traceId
            };

            _context.PublishedProductsQueue.Add(queueRecord);

            return queueRecord;
        }

        public SiteBookingPushQueue InsertSiteBookingPushQueueData(Guid queueHistoryId, Guid traceId)
        {
            var historyRecord = _context.SiteBookingPushQueueHistory.FirstOrDefault(a => a.Sitebookingpushqueuehistoryid == queueHistoryId);

            SiteBookingPushQueue queueRecord = new SiteBookingPushQueue
            {
                Publishersite = historyRecord.Publishersite,
                Subscribersiteid = historyRecord.Subscribersiteid,
                Publisherbookingid = historyRecord.Bookingid,
                Bookingdata = historyRecord.Bookingdata,
                Jobcreateddatetime = DateTime.UtcNow,
                Processingnote = string.Empty,
                Jobstatusid = 1,
                Traceid = traceId,
                Jobtypeid = historyRecord.Jobtypeid,
                Retrycount = 1,
                Sitebookingid = historyRecord.Sitebookingid
            };

            _context.SiteBookingPushQueue.Add(queueRecord);

            return queueRecord;
        }

        public SubscriberProductQueue InsertSubscriberProductQueueData(Guid queueHistoryId, Guid traceId)
        {
            var historyRecord = _context.SubscriberProductQueueHistory.FirstOrDefault(a => a.Subscriberproductqueuehistoryid == queueHistoryId);

            SubscriberProductQueue queueRecord = new SubscriberProductQueue
            {
                Subscriberproductid = historyRecord.Subscriberproductid,
                Marketplaceproductid = historyRecord.Marketplaceproductid,
                Subscriberid = historyRecord.Subscriberid,
                Jobstatusid = 1,
                Messagetypeid = historyRecord.Messagetypeid,
                Retrycount = 1,
                Jobnote = string.Empty,
                Jobcreationdatetime = DateTime.UtcNow,
                Traceid = traceId,
                Jobtypeid = historyRecord.Jobtypeid
            };

            _context.SubscriberProductQueue.Add(queueRecord);

            return queueRecord;
        }

        public Models.SubscriberProductTsUpdateQueue InsertSubscriberProductTsUpdateQueueData(Guid queueHistoryId, Guid traceId)
        {
            var historyRecord = _context.SubscriberProductTsUpdateQueueHistory.FirstOrDefault(a => a.Subscriberproducttsupdatequeuehistoryid == queueHistoryId);

            Models.SubscriberProductTsUpdateQueue queueRecord = new Models.SubscriberProductTsUpdateQueue
            {
                Marketplaceproductid = historyRecord.Marketplaceproductid,
                Subscriberid = historyRecord.Subscriberid,
                Jobstatusid = 1,
                Messagetypeid = historyRecord.Messagetypeid,
                Retrycount = 1,
                Jobcreationdatetime = DateTime.UtcNow,
                Tsid = historyRecord.Tsid,
                Traceid = traceId
            };

            _context.SubscriberProductTsUpdateQueue.Add(queueRecord);

            return queueRecord;
        }

        //NEW

        public PublishedProductsQueueHistory GetPublishedProductQueueHistoryItem(Guid historyId)
        {
            return _context.PublishedProductsQueueHistory.FirstOrDefault(a => a.PublishedProductsQueueHistoryId == historyId);
        }

        public List<PublishedProductsQueue> GetPublishedProductQueueItems()
        {
            return _context.PublishedProductsQueue.Select(a => a).ToList();
        }

        public SubscriberProductQueueHistory getSubscriberProductQueueHistoryItem(Guid historyId)
        {
            return _context.SubscriberProductQueueHistory.FirstOrDefault(a => a.Subscriberproductqueuehistoryid == historyId);
        }

        public List<SubscriberProductQueue> GetSubscriberProductQueueItems()
        {
            return _context.SubscriberProductQueue.Select(a => a).ToList();
        }

    }
}
