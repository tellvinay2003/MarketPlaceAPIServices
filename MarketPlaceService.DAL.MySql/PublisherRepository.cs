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
    public class PublisherRepository : BaseRepository, IPublisherRepository, IHealthCheck
    {
        private readonly string _user = string.Empty;
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
        public PublisherRepository(MarketplaceDbContext context,ICommonRepository commonRepository) : base(context)
        {
             _commonRepository = commonRepository;
        }
        public async Task<PublisherDataModel> AddNewPublisher(PublisherDataModel publisher)
        {
            
           var publisherDataModel = new Publisher
            {
               // PublisherId = Guid.NewGuid(),
                PublisherName = publisher.PublisherName,
                SiteId = publisher.SiteId,
                OrganizationId = publisher.OrganizationId,
                Enabled = publisher.Enabled               
            };

            _context.Publisher.Add(publisherDataModel);
            await _context.SaveChangesAsync();
            publisher.PublisherId=publisherDataModel.PublisherId;

            return await Task.FromResult(publisher);
        }

        public async Task<bool> DeletePublisher(Guid id)
        {
            var publisher = _context.Publisher.Where(p=>p.PublisherId == id).FirstOrDefault();
            if(publisher != null)
            {
                publisher.Enabled = !publisher.Enabled;
                
                await _context.SaveChangesAsync();
            }
            return await Task.FromResult(publisher.Enabled);
        }
        public async Task<PublisherDataModel> UpdatePublisher(PublisherDataModel publisher)
        {
            var updatePublisherDetails=_context.Publisher.Where(p=>p.PublisherId==publisher.PublisherId).Select(x=>x).FirstOrDefault();
            if(updatePublisherDetails==null)
            {
              return null;
            }
                updatePublisherDetails.PublisherName = publisher.PublisherName;
                updatePublisherDetails.SiteId = publisher.SiteId;
                updatePublisherDetails.OrganizationId = publisher.OrganizationId;
                updatePublisherDetails.Enabled = publisher.Enabled;                               
                _context.Publisher.Update(updatePublisherDetails);
                await _context.SaveChangesAsync();
            return await Task.FromResult(publisher);
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        public async Task<PublisherDataModel> GetPublisherByIdAsync(Guid id)
        {
            PublisherDataModel publisherDataModel = new PublisherDataModel();
            publisherDataModel = (from publisher in _context.Publisher
                                  join site in _context.Site on publisher.SiteId equals site.SiteId
                where publisher.PublisherId == id
                select new PublisherDataModel { 
                                SiteId = publisher.SiteId, 
                                OrganizationId = publisher.OrganizationId,
                                PublisherName = publisher.PublisherName,
                                Enabled = publisher.Enabled,
                                SiteName = site.SiteName,
                                PublisherId = id
                             }).FirstOrDefault();


            return await Task.FromResult(publisherDataModel);
        }



        public async Task<IEnumerable<PublisherDataModel>> GetPublishersListAsync()
        {
            List<PublisherDataModel> publisherDataModel = new List<PublisherDataModel>();
            publisherDataModel = (from publisher in _context.Publisher
                                  join site in _context.Site 
                                  on publisher.SiteId equals site.SiteId
                                  orderby publisher.PublisherName
                where site.Enabled
                select new PublisherDataModel { 
                                PublisherId = publisher.PublisherId,
                                SiteId = publisher.SiteId, 
                                OrganizationId = publisher.OrganizationId,
                                PublisherName = publisher.PublisherName,
                                Enabled = publisher.Enabled,
                                SiteName = site.SiteName
                             }).ToList();


            return await Task.FromResult(publisherDataModel);
        }


        public async Task<PublishedProductsDataModel> PublishProduct(PublishedProductsDataModel publishedProductsDataModel)
        {
            //check if this product is already published or in process of being published            
           if(HasPublisherPublishedProduct(publishedProductsDataModel))
                return publishedProductsDataModel;

            var publishedStatuses = await GetPublishStatus();
            //var processingStatues = await GetProcessingStatus();
            //var queuedStatusId = processingStatues.Where(a=>a.StatusName.ToLower().Equals("new")).FirstOrDefault().StatusId;
           
            var newStatusId = publishedStatuses.Where(a=>a.StatusName.ToLower().Equals("new")).FirstOrDefault().StatusId;

            var messageTypes = await GetMessageTypes();
            PublishedProducts publishedProductsItem = null;

            var messageTypeId = 0;

            
            //var publisherProductStatus = await GetPublisherProductStatus();
            if (publishedProductsDataModel.PublishedProductId == Guid.Empty)
            {
                publishedProductsItem = new PublishedProducts
                {
                    PublishedProductId = Guid.NewGuid(),
                    PublisherId = publishedProductsDataModel.PublisherId,
                    ProductTypeId = (short)publishedProductsDataModel.ProductTypeId,
                    ProductId = publishedProductsDataModel.ProductId,
                    ProductVersion = publishedProductsDataModel.ProductVersion,
                    ProductData = publishedProductsDataModel.ProductData != null ? publishedProductsDataModel.ProductData : string.Empty,
                    PublishedStatusId = newStatusId,
                    Publisherproductstatusid = 1,
                    ProcessedOn = DateTime.UtcNow,
                    //ProcessedBy = publishedProductsDataModel.ProcessedBy,
                    PublishedOn = DateTime.UtcNow,
                    ProcessedBy = publishedProductsDataModel.ProcessedBy != null ? publishedProductsDataModel.ProcessedBy : string.Empty,
                    PublishedBy = _userId,
                    ProcessingNote = publishedProductsDataModel.ProcessingNote != null ? publishedProductsDataModel.ProcessingNote : string.Empty,
                    Messagetypeid = GetMessageTypeId(messageTypes, publishedProductsDataModel.ProductTypeId)
                };

                _context.PublishedProducts.Add(publishedProductsItem); 

                publishedProductsDataModel.PublishedProductId = publishedProductsItem.PublishedProductId;
            }
            else
            {
                publishedProductsItem = _context.PublishedProducts.FirstOrDefault(a => a.PublishedProductId == publishedProductsDataModel.PublishedProductId);
                publishedProductsItem.ProcessingNote = string.Empty;
                publishedProductsItem.Publisherproductstatusid = 1;
                publishedProductsItem.Productsubstatusid = null;
                publishedProductsItem.PublishedOn = DateTime.UtcNow;
                _context.PublishedProducts.Update(publishedProductsItem);
            }

            // Call published product queue
            PublishedProductsQueue publishedProductsQueueItem = new PublishedProductsQueue
            {
                PublishedProductQueueId = Guid.NewGuid(),
                PublishedProductTypeId = (short)publishedProductsDataModel.ProductTypeId,
                PublishedProductId = publishedProductsItem.PublishedProductId,
                PublisherId = publishedProductsItem.PublisherId,
                Jobstatusid = 1,
                ProcessingNote = publishedProductsDataModel.ProcessingNote != null ? publishedProductsDataModel.ProcessingNote : string.Empty,
                CreationDateTime = DateTime.UtcNow,
                ProductId = publishedProductsDataModel.ProductId,
                ProcessedBy = publishedProductsDataModel.ProcessedBy != null ? publishedProductsDataModel.ProcessedBy : string.Empty,
                Jobtypeid = 1
            };
            _context.PublishedProductsQueue.Add(publishedProductsQueueItem);
            await _context.SaveChangesAsync();

            return await Task.FromResult(publishedProductsDataModel);
        }

        private int GetMessageTypeId(IEnumerable<MessageType> messageTypes, Entities.ProductType productType)
        {
            switch(productType)
            {
                case Entities.ProductType.Package:
                    return messageTypes.FirstOrDefault(q => q.Name.ToLower().Equals("packageproductjson")).Id;
                case Entities.ProductType.Service:
                    return messageTypes.FirstOrDefault(q => q.Name.ToLower().Equals("serviceproductjson")).Id;
            }
            return 1;
        }

        public bool HasPublisherPublishedProduct(PublishedProductsDataModel publishedProductsDataModel)
        {
            var allowedStatuses = new[] {1,2};
            var existingPublishedProduct = GetExistingProduct(publishedProductsDataModel.PublisherId, publishedProductsDataModel.ProductId);
            
            if((existingPublishedProduct != null && publishedProductsDataModel.PublishedProductId == Guid.Empty) //status was no

                || (existingPublishedProduct != null && publishedProductsDataModel.PublishedProductId != Guid.Empty 
                    && allowedStatuses.Contains((short)existingPublishedProduct.Publisherproductstatusid )))
                return true;

            return false;
        }

        private PublishedProducts GetExistingProduct(Guid publisherId, int productId)
        {
           return  (from pp in _context.PublishedProducts 
               
                where pp.ProductId ==  productId && pp.PublisherId == publisherId
                select pp).FirstOrDefault();

        }
        
        public async Task<IEnumerable<Entities.MessageType>> GetMessageTypes()
        {
             IList<Entities.MessageType> messageTypes = new List<Entities.MessageType>();
             messageTypes = (from m in _context.MessageTypes 
             select new Entities.MessageType
             {
                 Id = m.Messagetypeid,
                 Name = m.Messagetypename
             }).ToList();

             return messageTypes;
        }

        public async Task<IEnumerable<Entities.Status>> GetPublisherProductStatus()
        {
             IList<Entities.Status> status = new List<Entities.Status>();
             status = (from m in _context.PublisherProductStatus 
             select new Entities.Status
             {
                 Id = m.Productstatusid,
                 Name = m.Productstatusname
             }).ToList();

             return status;
        }

        public async Task<IEnumerable<QueuedPublication>> GetQueuedPublications(int limit, string serverName)
        {
            IList<QueuedPublication> publications = new List<QueuedPublication>();
            publications = (from pubs in _context.PublishedProductsQueue 
            join pubstatus in _context.JobStatus on pubs.Jobstatusid equals pubstatus.Jobstatusid
            where pubstatus.Jobstatusname.ToLower().Equals("queued") || pubstatus.Jobstatusname.ToLower().Equals("retry")
            select new QueuedPublication
            {
                JobId = pubs.PublishedProductQueueId,
                PublishedProductId = pubs.PublishedProductId,
                ProductId = pubs.ProductId,
                PublisherId = pubs.PublisherId,
                RetryCount = pubs.RetryCount,
                JobTypeId = pubs.Jobtypeid,
                ProductType = Enum.Parse<Entities.ProductType>(pubs.PublishedProductTypeId.ToString())
            }).Take(limit).ToList<QueuedPublication>();

            if(publications!=null && publications.Count()>0)
            {
                var inProcessStatusId = (from status in _context.JobStatus where status.Jobstatusname.ToLower().Equals("processing")
                select status.Jobstatusid).FirstOrDefault();
                foreach(var row in publications)
                {
                    row.JobStatusId = inProcessStatusId;
                    row.JobStartTime = new DateTime(1800,01,01);
                    row.JobEndTime = new DateTime(1800,01,01);
                    row.ProcessedBy = serverName;
                    await UpdatePublicationQueue(row);
                    //UpdatePublishedProductStatusToProcessing(row.PublishedProductId);
                }             
            }
            return await Task.FromResult(publications);
        }

        //private bool UpdatePublishedProductStatusToProcessing(Guid publishedProductId)
        //{
        //    var processingStatusId = _context.PublisherProductStatus.FirstOrDefault(q=> q.Productstatusname.ToLower().Equals("pending")).Productstatusid;
        //    var publishedProduct = _context.PublishedProducts.FirstOrDefault(q=> q.PublishedProductId==publishedProductId);

        //    if(publishedProduct==null)
        //        return false;
            
        //    publishedProduct. = processingStatusId;
        //    _context.PublishedProducts.Update(publishedProduct);

        //    _context.SaveChanges();
        //    return true;

        //}

        public async Task<IEnumerable<Entities.PublishedStatus>> GetPublishStatus()
        {
             IList<Entities.PublishedStatus> statuses = new List<Entities.PublishedStatus>();
             statuses = (from s in _context.PublishedStatus 
             select new Entities.PublishedStatus
             {
                 StatusId = s.PublishedStatusId,
                 StatusName = s.PublishedStatusName
             }).ToList();

             return await Task.FromResult(statuses);
        }

        //public async Task<IEnumerable<Entities.ProcessingStatus>> GetProcessingStatus()
        //{
        //     IList<Entities.ProcessingStatus> statuses = new List<Entities.ProcessingStatus>();
        //     statuses = (from s in _context.ProcessingStatus 
        //     select new Entities.ProcessingStatus
        //     {
        //         StatusId = s.ProcessingStatusId,
        //         StatusName = s.ProcessingStatusName
        //     }).ToList();

        //     return await Task.FromResult(statuses);
        //}

        public async Task<QueuedPublication> UpdatePublicationQueue(QueuedPublication queuedPublicationDataModel)
        {
            PublishedProductsQueue queuedPublications = _context.PublishedProductsQueue.Where(a=>a.PublishedProductQueueId==queuedPublicationDataModel.JobId).FirstOrDefault();
            if(queuedPublications!=null)
            {
                DateTime defaultdt = new DateTime(1800,01,01);
                queuedPublications.RetryCount = queuedPublicationDataModel.RetryCount;
                if(queuedPublicationDataModel.JobStartTime > defaultdt)
                {
                    queuedPublications.JobStartDateTime = queuedPublicationDataModel.JobStartTime;
                    }

                if(queuedPublicationDataModel.JobEndTime>defaultdt)
                {
                    queuedPublications.JobEndDateTime = queuedPublicationDataModel.JobEndTime;
                }
                queuedPublications.Jobstatusid = queuedPublicationDataModel.JobStatusId;
                queuedPublications.ProcessedBy = queuedPublicationDataModel.ProcessedBy==null ? queuedPublications.ProcessedBy : queuedPublicationDataModel.ProcessedBy;
                queuedPublications.ProcessingNote = string.IsNullOrEmpty(queuedPublicationDataModel.ProcessingNote) ? queuedPublications.ProcessingNote: queuedPublicationDataModel.ProcessingNote;

                _context.PublishedProductsQueue.Update(queuedPublications);
                await _context.SaveChangesAsync();
            }
            
            return await Task.FromResult(queuedPublicationDataModel);
        }

        public async Task<PublishedProductsDataModel> UpdatePublishedProduct(PublishedProductsDataModel publishedProductsDataModel)
        {

            short? subStatusErrorId = null;
            string errorText=null;
            if(publishedProductsDataModel.Errors!= null && publishedProductsDataModel.Errors.Count > 0)
            {
                var error = _commonRepository.GetErrorDetails(publishedProductsDataModel.Errors.FirstOrDefault());
                subStatusErrorId = error.ErrorId;
                errorText = error.ErrorMessage;
            }


            PublishedProducts publishedProduct = new PublishedProducts();
            publishedProduct = (from pp in _context.PublishedProducts
            where pp.PublishedProductId==publishedProductsDataModel.PublishedProductId
            select pp).FirstOrDefault();

            if(publishedProductsDataModel.ProductData!=null)
            {
                publishedProduct.ProductData = publishedProductsDataModel.ProductData;
            }
            publishedProduct.Publisherproductstatusid = publishedProductsDataModel.PublisherProductStatusId==null ? publishedProduct.Publisherproductstatusid : publishedProductsDataModel.PublisherProductStatusId;
            publishedProduct.PublishedStatusId = publishedProductsDataModel.PublishedStatusId==null ? publishedProduct.PublishedStatusId : publishedProductsDataModel.PublishedStatusId;
            publishedProduct.ProcessingNote = errorText ?? publishedProductsDataModel.ProcessingNote;
            publishedProduct.ProcessedOn =  DateTime.UtcNow;
            publishedProduct.Productsubstatusid = subStatusErrorId ?? publishedProduct.Productsubstatusid;
            publishedProduct.Traceid=publishedProductsDataModel.TraceId;
            publishedProduct.ProductVersion = publishedProductsDataModel.ProductVersion;
            publishedProduct.Productdatadiff = publishedProductsDataModel.ProductUpdateDifferenceData; 



            _context.PublishedProducts.Update(publishedProduct);

            await _context.SaveChangesAsync();

            return await Task.FromResult(publishedProductsDataModel);
        }


        public async Task<QueuedPublication> InsertPublicationHistory(QueuedPublication queuedPublicationDataModel)
        {
            QueuedPublication response = new QueuedPublication();
            var publishedProductId = _context.PublishedProductsQueue.Where(a=>a.PublishedProductQueueId==queuedPublicationDataModel.JobId).FirstOrDefault().PublishedProductId;
            var publisherProductStatuses = _context.PublisherProductStatus.Select(a => a).ToList();
            var jobHistoryStatuses = _context.JobHistoryStatus.Select(a => a).ToList();
            short? subStatusErrorId = null;
            short? publisherProductStatusId= publisherProductStatuses.FirstOrDefault(a=>a.Productstatusname.ToLower().Equals("published")).Productstatusid;
            short? jobHistoryStatusId = jobHistoryStatuses.FirstOrDefault(a => a.Jobstatusname.ToLower().Equals("success")).Jobstatusid;
            string errorText=null;

            if(queuedPublicationDataModel.Errors!= null && queuedPublicationDataModel.Errors.Count > 0)
            {
                var error = _commonRepository.GetErrorDetails(queuedPublicationDataModel.Errors.FirstOrDefault());
                subStatusErrorId = error.ErrorId;
                errorText = error.ErrorMessage;
                jobHistoryStatusId = jobHistoryStatuses.FirstOrDefault(a => a.Jobstatusname.ToLower().Equals("error")).Jobstatusid;
                publisherProductStatusId = publisherProductStatuses.FirstOrDefault(a => a.Productstatusname.ToLower().Equals("error")).Productstatusid; 
            }

            if(queuedPublicationDataModel.InsertPublishHistory)
            {
                var publishedproducthistory = _context.PublishedProducts.Where(a=>a.PublishedProductId==publishedProductId).Select(q=> new PublishedProductsHistory
                {
                    PublishedProductHistoryId = Guid.NewGuid(),
                    PublishedProductId = q.PublishedProductId,
                    ProductId = q.ProductId,
                    PublisherId = q.PublisherId,
                    ProductTypeId = q.ProductTypeId,
                    ProductData = q.ProductData,
                    ProcessedOn = q.ProcessedOn,
                    ProcessedBy = q.ProcessedBy,
                    ProductVersion = q.ProductVersion,
                    Publisherproductstatusid = publisherProductStatusId,
                    PublishedStatusId = q.PublishedStatusId,
                    ProcessingNote =  errorText ?? q.ProcessingNote,
                    PublishedOn = q.PublishedOn,
                    CreatedBy = q.PublishedBy,
                    Messagetypeid = q.Messagetypeid,
                    Productsubstatusid = subStatusErrorId ?? q.Productsubstatusid,
                    Publishedproductqueueid = queuedPublicationDataModel.JobId,
                    Traceid=queuedPublicationDataModel.TraceId,
                    Productdatadiff = q.Productdatadiff
                }).FirstOrDefault();
                 _context.PublishedProductsHistory.Add(publishedproducthistory);
            }        

            var publishedProductQueueHistory = _context.PublishedProductsQueue.Where(a=>a.PublishedProductQueueId==queuedPublicationDataModel.JobId).Select(q=> new PublishedProductsQueueHistory
            {
                PublishedProductsQueueHistoryId = Guid.NewGuid(),
                PublishedProductTypeId = q.PublishedProductTypeId,
                PublishedProductId = q.PublishedProductId,
                PublisherId = q.PublisherId,
                Jobhistorystatusid = jobHistoryStatusId,
                ProcessingNote = errorText ?? q.ProcessingNote,
                RetryCount = q.RetryCount,
                CreationDateTime = q.CreationDateTime,
                JobStartDateTime = (DateTime)q.JobStartDateTime,
                JobEndDateTime = (DateTime)q.JobEndDateTime,
                ProductId = q.ProductId,
                ProcessedBy = q.ProcessedBy,
                Productsubstatusid = subStatusErrorId,
                Publishedproductqueueid = q.PublishedProductQueueId,
                Traceid=queuedPublicationDataModel.TraceId ,
                Jobtypeid = q.Jobtypeid
            }).FirstOrDefault();

           
            _context.PublishedProductsQueueHistory.Add(publishedProductQueueHistory);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<QueuedPublication> InsertPublicationQueue(QueuedPublication queuedPublicationDataModel)
        {
            if (queuedPublicationDataModel.JobTypeId == 1 || queuedPublicationDataModel.JobTypeId == 9)
            {
                var publisherId = (from p in _context.Publisher
                                   join pp in _context.PublishedProducts on p.PublisherId equals pp.PublisherId
                                   where p.SiteId == queuedPublicationDataModel.SiteId
                                   && pp.ProductId == queuedPublicationDataModel.ProductId
                                   && pp.ProductTypeId == Convert.ToInt16(queuedPublicationDataModel.ProductType)
                                   select p.PublisherId).FirstOrDefault();

                var publishedProduct = _context.PublishedProducts.FirstOrDefault(a => a.ProductId == queuedPublicationDataModel.ProductId && a.PublisherId == publisherId && a.ProductTypeId == Convert.ToInt16(queuedPublicationDataModel.ProductType));

                if (publishedProduct == null || publishedProduct.Publisherproductstatusid == 4 || publishedProduct.Publisherproductstatusid == 3 )
                    return null;

                var existingingJobs = _context.PublishedProductsQueue.Where(a => a.PublishedProductId == publishedProduct.PublishedProductId).ToList();

                if (existingingJobs != null)
                {
                    if (existingingJobs.Any(a => a.Jobstatusid == 4)) //if there's a frozen/ waiting job
                        return null;

                    if (publishedProduct.Publisherproductstatusid == 3 || existingingJobs.Any()) //If published product is in error state or existing job
                    {
                        AddPublishedProductQueue(publishedProduct, _user, 4);
                        _context.SaveChanges();
                        return queuedPublicationDataModel;
                    }
                }

                AddPublishedProductQueue(publishedProduct, _user, 1);
            }
            else
            {
                //do something else here
            }

            _context.SaveChanges();
            return queuedPublicationDataModel;
        }

        private void AddPublishedProductQueue(PublishedProducts publishedProduct, string user, short jobStatusId)
        {
            PublishedProductsQueue publishedProductsQueueItem = new PublishedProductsQueue
            {
                PublishedProductQueueId = Guid.NewGuid(),
                PublishedProductTypeId = publishedProduct.ProductTypeId,
                PublishedProductId = publishedProduct.PublishedProductId,
                PublisherId = publishedProduct.PublisherId,
                Jobstatusid = jobStatusId,
                ProcessingNote = string.Empty,
                CreationDateTime = DateTime.UtcNow,
                ProductId = publishedProduct.ProductId,
                ProcessedBy = string.Empty,
                Jobtypeid = 1,
                
            };
            _context.PublishedProductsQueue.Add(publishedProductsQueueItem);
        }

        private Error GetErrorDetails(Error error)
        {
            //hardcoded ids now. please get from the db
            switch(error.Type)
            {
                case ErrorType.DataMappingIntegrity:
                    error.ErrorId = 1;
                    break;
                case ErrorType.ReadingMapping:
                    error.ErrorId = 2;
                    break;
                case ErrorType.FetchingProductData:
                case ErrorType.SaveData:
                case ErrorType.Unknown:
                    error.ErrorId=3;
                    break;
            }

            return error;
        }

        public async Task DeletePublishedProductQueue(Guid publishedProductQueueId)
        {
            PublishedProductsQueue publishedProductsQueue = new PublishedProductsQueue();
           
            publishedProductsQueue = _context.PublishedProductsQueue.Where(a=>a.PublishedProductQueueId==publishedProductQueueId).FirstOrDefault();

            //if the published product has another queue item in the dormant state then check if the published product status is a success and then reactivate the new queue status
            ActivateDormantPublisherJob(publishedProductsQueue.PublishedProductId);

            _context.PublishedProductsQueue.Remove(publishedProductsQueue);
            await _context.SaveChangesAsync();
        }

        private void ActivateDormantPublisherJob(Guid publishedProductId)
        {
            var publishedProduct = _context.PublishedProducts.FirstOrDefault(a => a.PublishedProductId == publishedProductId);
            if (publishedProduct.Publisherproductstatusid == 2)
            {
                var frozenPublishedProductQueue = _context.PublishedProductsQueue.Where(a => a.PublishedProductId == publishedProduct.PublishedProductId && a.Jobstatusid == 4 && a.Jobtypeid == 1).FirstOrDefault();
                if (frozenPublishedProductQueue != null)
                {
                    frozenPublishedProductQueue.Jobstatusid = 1;
                    _context.PublishedProductsQueue.Update(frozenPublishedProductQueue);
                }
            }
        }

        public async Task<string> GetPublishedProductDataString(Guid publishedProductId)
        {
            var response = _context.PublishedProducts.Where(a=>a.PublishedProductId==publishedProductId).FirstOrDefault();
            if (response != null)
                return !string.IsNullOrEmpty(response.Productdatadiff) ? response.Productdatadiff : response.ProductData;
            return string.Empty;
        }
        public async Task<IEnumerable<CurrentSubscriber>> GetCurrentSubscribers(Guid publishedProductId)
        {
            List<CurrentSubscriber> response = new List<CurrentSubscriber>();
            
            response = (from marketplaceProduct in _context.MarketplaceProduct 
                        join subscriberProduct in _context.SubscriberProduct on marketplaceProduct.Marketplaceproductid equals subscriberProduct.Marketplaceproductid
                        join subscriber in _context.Subscriber on subscriberProduct.Subscriberid equals subscriber.SubscriberId
                        where marketplaceProduct.Publishedproductid == publishedProductId
                        select new CurrentSubscriber {  
                             SubscriberId = subscriber.SubscriberId,
                            SubscriberName = subscriber.SubscriberName,
                            SubscriptionDate = subscriberProduct.Subscribedon
                        }).ToList();   
            return response;
        }
         public async Task<string> GetPublishedProductDataMarketplaceString(Guid publishedProductId)
        {
            var response = string.Empty;
            var result = _context.MarketplaceProduct.Where(a=>a.Publishedproductid==publishedProductId).FirstOrDefault();
            if(result != null) 
                response = result.Productdata;
            return response;
        }


        public async Task<string> GetSiteUrlBasedOnSiteId(Guid id)
        {
            var siteUrl = (from site in _context.Site
                            where site.SiteId == id
                            select site.Url).FirstOrDefault();

           return await Task.FromResult(siteUrl);
        }

        public async Task<GetPublishProductResponse> GetPublishProduct(int productId, Guid publisherId)
        {
            GetPublishProductResponse statuses = new GetPublishProductResponse();
            statuses = (from s in _context.PublishedProducts
                        where s.PublisherId == publisherId && s.ProductId == productId
                        select new GetPublishProductResponse
                        {
                            PublishProductId = s.PublishedProductId
                        }).FirstOrDefault();

            return await Task.FromResult(statuses);
        }

        public async Task<IEnumerable<PublisherAgentMap>> GetPublisherAgentMaps(Guid publisherId)
        {
            List<PublisherAgentMap> response = new List<PublisherAgentMap>();
            response = _context.PublisherAgent.Where(a=> a.PublisherId== publisherId).Select(pa=>
            new PublisherAgentMap
            {
                Id = pa.PublisherAgentId,
                PublisherId = publisherId,
                SubscriberId = pa.SubscriberD,
                AgentId = pa.AgentId,
                OrganisationId = pa.OrganisationId,
                UserId = pa.UserId,
                BookingPrefixId = pa.BookingPrefixId
            }).ToList();

            return response;
        }

        public async Task<PublisherAgentMap> GetPublisherAgentMaps(Guid publisherId, Guid subscriberId)
        {
            
            PublisherAgentMap response = new PublisherAgentMap();
            response = _context.PublisherAgent.Where(a=> a.PublisherId== publisherId && a.SubscriberD== subscriberId).Select(pa=>
            new PublisherAgentMap
            {
                Id = pa.PublisherAgentId,
                PublisherId = publisherId,
                SubscriberId = pa.SubscriberD,
                AgentId = pa.AgentId,
                OrganisationId = pa.OrganisationId,
                UserId = pa.UserId,
                BookingPrefixId = pa.BookingPrefixId
            
            }).FirstOrDefault();
            return response;
        }

        public async Task<PublisherAgentMap> InsertPublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId)
        {      
            request.PublisherId = publisherId;
            request.SubscriberId = subscriberId;
            PublisherAgent publisherAgent = new PublisherAgent
            {
                AgentId = request.AgentId,
                PublisherId = publisherId,
                SubscriberD = subscriberId,
                OrganisationId = request.OrganisationId,
                UserId = request.UserId,
                BookingPrefixId = request.BookingPrefixId
            };

            _context.PublisherAgent.Add(publisherAgent);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<PublisherAgentMap> UpdatePublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId)
        {   
            request.PublisherId = publisherId;
            request.SubscriberId=subscriberId;
            
            var publisherAgent = _context.PublisherAgent.FirstOrDefault(a=> a.SubscriberD==subscriberId && a.PublisherId==publisherId);
            if(publisherAgent == null)
                return null;
            publisherAgent.AgentId = request.AgentId;      

            publisherAgent.OrganisationId = request.OrganisationId;
            publisherAgent.UserId = request.UserId;
            publisherAgent.BookingPrefixId = request.BookingPrefixId;
            _context.PublisherAgent.Update(publisherAgent);
            await _context.SaveChangesAsync();          
            
            return request;
        }

        public async Task<bool> DeletePublisherAgentMaps(Guid publisherId, Guid subscriberId)
        {
            var publisherAgent = _context.PublisherAgent.FirstOrDefault(a=> a.SubscriberD==subscriberId && a.PublisherId==publisherId);
            if(publisherAgent == null)
                return false;

            _context.PublisherAgent.Remove(publisherAgent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PublisherDefaultDataModel> GetPublisherDefaults(Guid publisherId)
        {
            PublisherDefaultDataModel response = new PublisherDefaultDataModel();
            
            response.ContractDate = _context.PublisherDefault.FirstOrDefault(a=>a.PublisherId == publisherId)?.ContractDate;
            response.ServiceStatuses = _context.PublisherServiceStatus.Where(a=> a.PublisherId== publisherId && a.ServiceStatusId>0).Select(q=>q.ServiceStatusId ).ToList();
            response.SupplierStatuses = _context.PublisherSupplierStatus.Where(a=> a.PublisherId== publisherId).Select(q=>q.SupplierStatusId).ToList();
            response.PackagePriceStartDate = _context.PublisherDefault.FirstOrDefault(a=>a.PublisherId == publisherId)?.PackagePriceStartDate;
            response.PackageStatuses = _context.PublisherServiceStatus.Where(a=> a.PublisherId== publisherId && a.PackageStatusId!=null && a.PackageStatusId>0).Select(q=>(int)q.PackageStatusId).ToList();
           
            return response;

        }

        public async Task<PublisherDefaultDataModel> InsertUpdatePublisherDefaults(Guid publisherId, PublisherDefaultDataModel request)
        {
            List<PublisherServiceStatus> publisherServiceStatuses = _context.PublisherServiceStatus.Where(a=> a.PublisherId == publisherId && a.ServiceStatusId>0 ).Select(q=>q).ToList();
            List<PublisherServiceStatus> publisherPackageStatuses = _context.PublisherServiceStatus.Where(a=> a.PublisherId == publisherId && a.PackageStatusId>0 ).Select(q=>q).ToList();
            List<PublisherSupplierStatus> publisherSupplierStatuses = _context.PublisherSupplierStatus.Where(a=> a.PublisherId == publisherId).Select(q=>q).ToList();
            PublisherDefault publisherDefault = _context.PublisherDefault.FirstOrDefault(a=>a.PublisherId == publisherId);
            List<PublisherServiceStatus> publisherServiceStatusesAdded = new List<PublisherServiceStatus>();
            List<PublisherSupplierStatus> publisherSupplierStatusesAdded = new List<PublisherSupplierStatus>();
            List<PublisherServiceStatus> publisherPackageStatusesAdded = new List<PublisherServiceStatus>();

            if(publisherServiceStatuses!=null && publisherServiceStatuses.Count>0)
            {
                 _context.PublisherServiceStatus.RemoveRange(publisherServiceStatuses);   
            }

             if(publisherPackageStatuses!=null && publisherPackageStatuses.Count>0)
            {
                 _context.PublisherServiceStatus.RemoveRange(publisherPackageStatuses);   
            }

            if(publisherSupplierStatuses!=null && publisherSupplierStatuses.Count >0)
            {
                 _context.PublisherSupplierStatus.RemoveRange(publisherSupplierStatuses);   
            }

            if(publisherDefault!=null)
            {
                publisherDefault.ContractDate = request.ContractDate;
                publisherDefault.PackagePriceStartDate = request.PackagePriceStartDate;
                _context.PublisherDefault.Update(publisherDefault);
            }
            else if (request.ContractDate != null || request.PackagePriceStartDate !=null )
                {                  
                    PublisherDefault PublisherDefaultAdd  = new PublisherDefault
                    {
                        PublisherId = publisherId,
                        ContractDate = request.ContractDate,
                        PackagePriceStartDate = request.PackagePriceStartDate
                    };                     
                    _context.PublisherDefault.Add(PublisherDefaultAdd);
                }




            if(request.ServiceStatuses !=null && request.ServiceStatuses.Count>0)
            {
                publisherServiceStatusesAdded = request.ServiceStatuses.Select(a=> new PublisherServiceStatus { ServiceStatusId = a, PublisherId = publisherId}).ToList();
                _context.PublisherServiceStatus.AddRange(publisherServiceStatusesAdded);
            }

            if(request.PackageStatuses !=null && request.PackageStatuses.Count>0)
            {
                publisherPackageStatusesAdded = request.PackageStatuses.Select(a=> new PublisherServiceStatus { PackageStatusId = a, PublisherId = publisherId , ServiceStatusId=0}).ToList();
                _context.PublisherServiceStatus.AddRange(publisherPackageStatusesAdded);
            }

             if(request.SupplierStatuses !=null && request.SupplierStatuses.Count>0)
            {
                publisherSupplierStatusesAdded = request.SupplierStatuses.Select(a=> new PublisherSupplierStatus { SupplierStatusId = a, PublisherId = publisherId}).ToList();
                _context.PublisherSupplierStatus.AddRange(publisherSupplierStatusesAdded);
            }

            _context.SaveChanges();

            return request;
        }

        public async Task<IEnumerable<PublishedProductsDataModel>> GetPublishedUnpublishedProducts(Guid publisherId, int productType, IEnumerable<ServiceDataModel> products, int? publishedStatus, IEnumerable<PackageDataModel> packages, Entities.ProductType productTypeId)
        {
            if((productTypeId == Entities.ProductType.Service && products == null) || (productTypeId == Entities.ProductType.Package && packages == null))
                return null;

            var finalOutput = new List<PublishedProductsDataModel>();
            var rePublishableProductStatuses = new[] { 3,4 }; //if a product is in the following status then it can be subscribed to

            var publishedProducts = _context.PublishedProducts.Where(pp=> pp.PublisherId== publisherId && pp.ProductTypeId == (short)productTypeId)
            .Select(a=>a).ToList();

            var publishedProductStatuses = _context.PublisherProductStatus.Select(q=>q).ToList();

            var publishedProductSubStatuses = _context.PublisherProductSubStatus.Select(p=>p).ToList();
            
            if(productTypeId == Entities.ProductType.Service)
                                {
                finalOutput = GetPublishedUnpublishedServices(products, publishedProducts, rePublishableProductStatuses, publishedProductSubStatuses);
            }
            else
            {
                var publisherName = _context.Publisher.FirstOrDefault(a=>a.PublisherId == publisherId).PublisherName;
            
                finalOutput = GetPublishedUnpublishedPackages(packages, publishedProducts, rePublishableProductStatuses, publishedProductSubStatuses, publisherName);
            }
            
            finalOutput.Where(q=>q.IsPublished).ToList().ForEach(a=> a.ProcessingStatusName = publishedProductStatuses.FirstOrDefault(s=> s.Productstatusid == a.PublisherProductStatusId).Productstatusname);
            
            if(publishedStatus == 5) //this is for published status as NO
            {
                finalOutput = finalOutput.Where(a => a.PublisherProductStatusId == null).ToList();
            }
            else if(publishedStatus > 0)
            {
                finalOutput = finalOutput.Where(a => a.PublisherProductStatusId == publishedStatus).ToList();
            }
            
            return finalOutput;

        }

        private List<PublishedProductsDataModel> GetPublishedUnpublishedServices(IEnumerable<ServiceDataModel> services, List<PublishedProducts> publishedProducts, int[] rePublishableProductStatuses, List<PublisherProductSubStatus> publishedProductSubStatuses)
        {
            var finalOutput = (from service in services 
                                join pp in publishedProducts on service.Serviceid equals pp.ProductId into allProducts
                                from a in allProducts.DefaultIfEmpty()                                
                                select new PublishedProductsDataModel
                                {
                                    ProductName = service.Servicelongname,
                                    SupplierName = service.SupplierName,
                                    ServiceType = service.ServiceTypeName,
                                    LocationName = service.RegionName, 
                                    Status = service.ServiceStatusName,
                                    ProductId = service.Serviceid,
                                    PublishedProductId = a == null ? Guid.Empty : a.PublishedProductId,
                                    IsPublished = a==null ? false : true,
                                    PublisherProductStatusId =  a == null ? null : a.Publisherproductstatusid,
                                    ProductSubStatusId = a == null ? null : a.Productsubstatusid,
                                    ProductSubStatusName = (a != null && a.Productsubstatusid != null) ? (publishedProductSubStatuses.FirstOrDefault(pps=>pps.Productsubstatusid == a.Productsubstatusid).Productsubstatusname) : null,
                                    ErrorMessage = a == null ? null : a.ProcessingNote,
                                    IsPublishable = a == null || rePublishableProductStatuses.Contains((short)a.Publisherproductstatusid),
                                    ProcessedOn = a == null ? (DateTime?)null : a.ProcessedOn,
                                    ProductTypeId = Entities.ProductType.Service
                                }).ToList();

            return finalOutput;
        }

        private List<PublishedProductsDataModel> GetPublishedUnpublishedPackages(IEnumerable<PackageDataModel> packages, List<PublishedProducts> publishedProducts, int[] rePublishableProductStatuses, List<PublisherProductSubStatus> publishedProductSubStatuses, string publisherName)
        {
            var finalOutput = (from package in packages 
                                join pp in publishedProducts on package.PackageId equals pp.ProductId into allProducts
                                from a in allProducts.DefaultIfEmpty()                                
                                select new PublishedProductsDataModel
                                {
                                    ProductName = package.PackageLongname,
                                    SupplierName = publisherName,
                                    ServiceType = package.PackageTypeName,
                                    LocationName = package.RegionName, 
                                    Status = package.PackageStatusName,
                                    ProductId = package.PackageId,
                                    PublishedProductId = a == null ? Guid.Empty : a.PublishedProductId,
                                    IsPublished = a==null ? false : true,
                                    PublisherProductStatusId =  a == null ? null : a.Publisherproductstatusid,
                                    ProductSubStatusId = a == null ? null : a.Productsubstatusid,
                                    ProductSubStatusName = (a != null && a.Productsubstatusid != null) ? (publishedProductSubStatuses.FirstOrDefault(pps=>pps.Productsubstatusid == a.Productsubstatusid).Productsubstatusname) : null,
                                    ErrorMessage = a == null ? null : a.ProcessingNote,
                                    IsPublishable = a == null || rePublishableProductStatuses.Contains((short)a.Publisherproductstatusid),
                                    ProcessedOn = a == null ? (DateTime?)null : a.ProcessedOn,
                                    ProductTypeId = Entities.ProductType.Package
                                }).ToList();
            return finalOutput;
        }

         public async Task<int> GetMessageTypeIdAsync(Guid publishedProductId)
        {
            int messageTypeId = 0;
          
            var publishedProduct = (from publishedProducts in _context.PublishedProducts where publishedProducts.PublishedProductId == publishedProductId  select publishedProducts).FirstOrDefault();
            if(publishedProduct != null)
                messageTypeId = (int)publishedProduct.Messagetypeid;

            return messageTypeId;
        }

     
        public async Task<PublishedProductDataResponse> InsertUpdateMarketPlaceProductData(IEnumerable<RetriveFieldPathResponse> fieldList, Guid publishedProductId, string json, int messageTypeId)
        {
            PublishedProductDataResponse response = new PublishedProductDataResponse();
            var regionId = fieldList.FirstOrDefault(a=>a.FieldPath == DataMappingConstants.regionId).Value.FirstOrDefault();
            var serviceLongName = fieldList.FirstOrDefault(a=>a.FieldPath == DataMappingConstants.serviceLongName).Value.FirstOrDefault();
            var serviceTypeId = fieldList.FirstOrDefault(a=>a.FieldPath == DataMappingConstants.serviceTypeId).Value.FirstOrDefault();
            var serviceShortName = fieldList.FirstOrDefault(a=>a.FieldPath == DataMappingConstants.serviceShortName).Value.FirstOrDefault();

            var marketplaceRequest = _context.MarketplaceProduct.FirstOrDefault(a => a.Publishedproductid == publishedProductId);

            if (marketplaceRequest == null)
            {
                marketplaceRequest = new MarketplaceProduct
                {
                    Marketplaceproductid = Guid.NewGuid(),
                    Publishedproductid = publishedProductId,
                    Processedon = DateTime.UtcNow,
                    Regionid = regionId != null ? Convert.ToInt32(regionId) : 0,
                    Productlongname = serviceLongName,
                    Productshortname = serviceShortName,
                    Servicetypeid = serviceTypeId != null ? Convert.ToInt32(serviceTypeId) : 0,
                    Productdata = json,
                    Messagetypeid = messageTypeId,
                    Processedby = _user,                    
                };

                _context.MarketplaceProduct.Add(marketplaceRequest);
            }
            else
            {
                marketplaceRequest.Productlongname = serviceLongName;
                marketplaceRequest.Productshortname = serviceShortName;
                marketplaceRequest.Regionid = regionId != null ? Convert.ToInt32(regionId) : 0;
                marketplaceRequest.Productdata = json;
                _context.MarketplaceProduct.Update(marketplaceRequest);
            }

            var marketplaceProductId = marketplaceRequest.Marketplaceproductid;
            response.MarketplaceProductId = marketplaceRequest.Marketplaceproductid;

            List<string> ratingList = new List<string>();
            ratingList = fieldList.FirstOrDefault(a => a.FieldPath == DataMappingConstants.ratings).Value.ToList();

            var existingRatings = _context.MarketplaceProductRating.Where(a => a.Marketplaceproductid == marketplaceProductId).ToList();

            if (existingRatings != null && existingRatings.Count > 0)
            {
                //remove ratings that are no more present
                var ratingsToDelete = existingRatings.Where(a => !ratingList.Contains(a.Ratingid.ToString())).ToList();
                if (ratingsToDelete != null)
                    _context.MarketplaceProductRating.RemoveRange(ratingsToDelete);

                //ratings to add
                ratingList = ratingList.Where(a => !existingRatings.Select(q => q.Ratingid).ToList().Contains(Convert.ToInt32(a))).ToList();
            }
            
            var marketplaceRatings = ratingList.Where(x =>  Convert.ToInt32(x) > 0).Select(a => new MarketplaceProductRating
            {
                Marketplaceproductid = marketplaceProductId,
                Ratingid = Convert.ToInt32(a)
            });
            
            _context.MarketplaceProductRating.AddRange(marketplaceRatings);
            _context.SaveChanges();
                        
           return response;
        } 

        public async Task<IEnumerable<HistoryDetail>> GetPublicationHistoryDetails(Guid publishedProductId)
        {
            var response = new List<HistoryDetail>();
            response = (from pph in _context.PublishedProductsHistory
                        join pps in _context.PublisherProductStatus
                        on pph.Publisherproductstatusid equals pps.Productstatusid
                        where pph.PublishedProductId == publishedProductId 
                        select new HistoryDetail{
                            Date = pph.ProcessedOn,
                           // Event = pps.Productstatusname,
                            Event = (pph.ProductVersion>1 && pph.Publisherproductstatusid == 2)? "Updated" :pps.Productstatusname,
                            Message = pph.ProcessingNote,
                            UserName = _context.UserAccount.Where(z => z.Id == pph.CreatedBy).Select(y => y.FirstName + ' ' + y.LastName).FirstOrDefault(),
                            SubStatusId = pph.Productsubstatusid,
                            SubStatusName = pph.Productsubstatusid != null ? _context.PublisherProductSubStatus.FirstOrDefault(a=>a.Productsubstatusid == pph.Productsubstatusid).Productsubstatusname : null,
                            HistoryId = pph.PublishedProductHistoryId,
                            JsonExists = !string.IsNullOrEmpty(pph.ProductData) ? true : false
                        }).ToList();

            return response;
        }

        public async Task<DateTime> GetFirstPublicationDate(Guid publishedProductId)
        {
            var response = new DateTime();
            var result = (from pp in _context.PublishedProducts 
                            where pp.PublishedProductId == publishedProductId 
                            select pp).FirstOrDefault();
            if(result != null) 
                response = result.PublishedOn;
            return response;
        }

        public PublishedProductsDataModel GetExistingPublishedProduct(int productId, Guid publisherId, Entities.ProductType productType)
        {
            return _context.PublishedProducts.Where(pp => pp.ProductId == productId && pp.PublisherId == publisherId && pp.Publisherproductstatusid == 2 && pp.ProductTypeId == Convert.ToInt16(productType)).Select(a=> new PublishedProductsDataModel
            {
                ProductData = a.ProductData,
                ProductVersion = a.ProductVersion,
            }).FirstOrDefault();
        }

        public async Task<IEnumerable<Entities.PublishedProductAllowedSubscriber>> GetAllowedSubscribers(List<Guid> productIds)
        {
            List<Entities.PublishedProductAllowedSubscriber> response=new List<Entities.PublishedProductAllowedSubscriber>();
            response=(from s in _context.PublishedProductAllowedSubscriber 
                      where productIds.Contains(s.Publishedproductid) select new Entities.PublishedProductAllowedSubscriber{
                       PublishedProductAllowedSubscriberId=s.Publishedproductallowedsubscriberid,
                       PublisherProductId=s.Publishedproductid,
                       SubscriberId=s.Subscriberid
                      }).ToList(); 
            return await Task.FromResult(response);
        
        }

        public async Task<Entities.PublishedProductAllowedSubscriber> AllowedSubscriber(Guid publishedproductId,Guid subscriberId)
        {
           Entities.PublishedProductAllowedSubscriber response=new Entities.PublishedProductAllowedSubscriber();
           var checkExisitsSubscriber=_context.PublishedProductAllowedSubscriber.Where(a=>a.Publishedproductid==publishedproductId&&a.Subscriberid==subscriberId).Select(x=>x).FirstOrDefault();
           if(checkExisitsSubscriber==null)
           {
           var allowedSubscriber=new Models.PublishedProductAllowedSubscriber
           {
              Publishedproductid=publishedproductId,
              Subscriberid=subscriberId
           };
          _context.PublishedProductAllowedSubscriber.Add(allowedSubscriber);              
          _context.SaveChanges();
           response.PublishedProductAllowedSubscriberId=allowedSubscriber.Publishedproductallowedsubscriberid;

           await InsertPublicationHistoryForUnpublishProduct(publishedproductId, subscriberId, 2);
           var marketplaceProductId = _context.MarketplaceProduct.Where(x => x.Publishedproductid == publishedproductId).Select(x => x.Marketplaceproductid).FirstOrDefault();
            var subscribedProduct = _context.SubscriberProduct.Where(a => a.Marketplaceproductid == marketplaceProductId && a.Subscriberid == subscriberId).Select(x=>x).FirstOrDefault();
            if(subscribedProduct != null)
            {
                await InsertSubscriptionHistoryForUnsubscribeProduct(subscribedProduct.Subscriberproductid, 4, null);
            }
           }           
           response.PublisherProductId=publishedproductId;
           response.SubscriberId=subscriberId;
           return await Task.FromResult(response);
        }

        public async Task<bool> DeleteAllowedSubscriber(Guid publishedProductId,Guid subscriberId, int calledFrom)
        {
            var getMarketPlaceId=_context.MarketplaceProduct.Where(x=>x.Publishedproductid==publishedProductId).Select(x=>x.Marketplaceproductid).FirstOrDefault();
            var allowedSubscriber = _context.PublishedProductAllowedSubscriber.FirstOrDefault(a=> a.Publishedproductid==publishedProductId && a.Subscriberid==subscriberId);
            if(allowedSubscriber == null)
                return false;

            _context.PublishedProductAllowedSubscriber.Remove(allowedSubscriber);
            await _context.SaveChangesAsync();
            await InsertPublicationHistoryForUnpublishProduct(publishedProductId, subscriberId, 3);
 
            var getSubscribedProduct=_context.SubscriberProduct.Where(a=>a.Marketplaceproductid==getMarketPlaceId && a.Subscriberid==subscriberId).Select(x=>x).FirstOrDefault();
            if(getSubscribedProduct!=null)
            {
                getSubscribedProduct.Productstatusid=_context.SubscribeProductStatus.FirstOrDefault(a => a.Productstatusname.ToLower().Equals("unsubscribed")).Productstatusid;
                await InsertSubscriptionHistoryForUnsubscribeProduct(getSubscribedProduct.Subscriberproductid, calledFrom, null);
                _context.SubscriberProduct.Update(getSubscribedProduct);
                _context.SaveChanges();

              InsertSubscribeProductQueue(getSubscribedProduct.Marketplaceproductid,getSubscribedProduct.Subscriberproductid,subscriberId, 3);
            }                          
            return true;
        }

        public async void  InsertSubscribeProductQueue(Guid marketplaceProductId,Guid subscriberProductId,Guid subscriberId, short jobTypeId)
        {
              var checkSubscriberProductQueue=_context.SubscriberProductQueue.Where(a=>a.Marketplaceproductid==marketplaceProductId && a.Subscriberproductid==subscriberProductId).Select(x=>x).FirstOrDefault();
              if(checkSubscriberProductQueue==null)
              {
                var GetMessageTypeId= await _commonRepository.GetMessageTypeId(marketplaceProductId);
                SubscriberProductQueue SubscribedProductQueueItem = new SubscriberProductQueue
                {
                    Marketplaceproductid = marketplaceProductId,
                    Subscriberproductid = subscriberProductId,
                    Subscriberid = subscriberId,
                    Messagetypeid = GetMessageTypeId,
                    Jobnote = string.Empty,
                    Jobcreationdatetime = DateTime.UtcNow,
                    Jobstatusid = 1,
                    Jobtypeid = jobTypeId
                };
                _context.SubscriberProductQueue.Add(SubscribedProductQueueItem);
                _context.SaveChanges();
              }
        }

        public async Task<IEnumerable<ManageSubscribersResponse>> SearchManageSubscribers(ManageSubscribersSearch manageSubscribersDataModel)
        {
             List<ManageSubscribersResponse> GetMarketPlaceProducts=new List<ManageSubscribersResponse>();
            List<int> allRegionIDs=GetRegionAndChildRegions(manageSubscribersDataModel.RegionId);
            GetMarketPlaceProducts = (from s in _context.MarketplaceProduct
                                      join p in _context.PublishedProducts on s.Publishedproductid equals p.PublishedProductId
                                      join r in _context.MasterRegions on s.Regionid equals r.Regionid
                                      where manageSubscribersDataModel.ProductTypeId.Contains(s.Servicetypeid)
                                      && allRegionIDs.Contains(r.Regionid)
                                      && p.PublisherId==manageSubscribersDataModel.PublisherId
                                      && p.Publisherproductstatusid != 4 //unpublished
                                      select new ManageSubscribersResponse
                                      {
                                          MarketPlaceProductId = s.Marketplaceproductid,
                                          PublishedProductId = s.Publishedproductid,
                                          ServiceTypeId = s.Servicetypeid,
                                          RegionId = s.Regionid,
                                          ProductLongName = s.Productlongname,
                                          ProductShortName=s.Productshortname,
                                          RegionName = r.Regionname,                                          
                                          MessageTypeId = (int)p.Messagetypeid,
                                          ProductTypeId = p.ProductTypeId
                                      }).ToList();
            if(!string.IsNullOrEmpty(manageSubscribersDataModel.ProductName))
            {
                 GetMarketPlaceProducts = GetMarketPlaceProducts.Where(a=> a.ProductLongName.ToLower().Contains(manageSubscribersDataModel.ProductName.ToLower())).ToList();
            }
            
            var getAllowedSubscribers=(from mp in _context.MarketplaceProduct
                                       join asub in _context.PublishedProductAllowedSubscriber on mp.Publishedproductid equals asub.Publishedproductid
                                       join sub in _context.Subscriber on asub.Subscriberid equals sub.SubscriberId
                                       join subpro in _context.SubscriberProduct on new {mp.Marketplaceproductid,asub.Subscriberid} equals new{subpro.Marketplaceproductid,subpro.Subscriberid} into gj
                                       from x in gj.DefaultIfEmpty()
                                        select new ManageSubscriberDetails{
                                          SubscriberId=asub.Subscriberid,
                                          SubscriberName=sub.SubscriberName,
                                          PublishedProductId=mp.Publishedproductid,
                                          SubscriberProductId=x.Subscriberproductid
                                       }).ToList();

             if(getAllowedSubscribers.Count>=1)
             {
               GetMarketPlaceProducts.ToList().ForEach(a=> a.Subscribers = getAllowedSubscribers.Where(x=>x.PublishedProductId==a.PublishedProductId).Select(w=>new ManageSubscriberDetails{SubscriberId=w.SubscriberId,SubscriberName=w.SubscriberName,PublishedProductId=w.PublishedProductId,SubscriberProductId=w.SubscriberProductId}).ToList());
               if(manageSubscribersDataModel.InCludeSubscriber==2)
               {
                  if(manageSubscribersDataModel.SubscriberId == Guid.Empty)
                  {
                     GetMarketPlaceProducts=GetMarketPlaceProducts.Where(x=>x.Subscribers.Count==0).ToList();
                  }
                  else
                  { 
                     GetMarketPlaceProducts=GetMarketPlaceProducts.Where(x=>x.Subscribers.Any(p=>p.SubscriberId==manageSubscribersDataModel.SubscriberId)).ToList();
                  }
               }
              else if(manageSubscribersDataModel.InCludeSubscriber==3)
               {
                   if(manageSubscribersDataModel.SubscriberId == Guid.Empty)
                   {
                     GetMarketPlaceProducts=GetMarketPlaceProducts.Where(x=>x.Subscribers.Count!=0).ToList();
                   }
                  else 
                  {
                     GetMarketPlaceProducts=GetMarketPlaceProducts.Where(x=>!x.Subscribers.Any(p=>p.SubscriberId==manageSubscribersDataModel.SubscriberId)).ToList();
                  }
               }
             }      

            return await Task.FromResult(GetMarketPlaceProducts);
        }

         private List<int> GetRegionAndChildRegions(int regionId)
        {
            
            var allRegions = _context.MasterRegions.Select(a=>a).ToList();

            List<int> allRegionIDs = new List<int>();
            AddChildRegionsToList(allRegionIDs, allRegions, (regionId > 0 ? Enumerable.Repeat(regionId,1) : allRegions.Where(a=> a.Parentregionid == null).Select(q=>q.Regionid)));

            return allRegionIDs;
        }

        private void AddChildRegionsToList(List<int> finalRegionIDs, List<DAL.Models.MasterRegions> allRegions,  IEnumerable<int> parentRegionIds)
        {            
            if(parentRegionIds!= null && parentRegionIds.Count() > 0)
            {      
                finalRegionIDs.AddRange(parentRegionIds);
                List<int> childRegions =  GetInnerRegionIDs(allRegions, parentRegionIds);        
               AddChildRegionsToList(finalRegionIDs,allRegions, childRegions);
            }
        }

        private List<int> GetInnerRegionIDs(List<DAL.Models.MasterRegions> allRegions, IEnumerable<int> parentRegionIds)
        {
            return allRegions.Where(r=> r.Parentregionid!=null && parentRegionIds.Contains((int)r.Parentregionid)).Select(a=> a.Regionid).ToList();
        }
        public async Task<bool> InsertStaticDataUpdateQueue(StaticDataUpdateQueueRequest insertStaticDataUpdateQueueRequest)
        {
           
            Models.StaticDataUpdateQueue staticDataUpdateQueueItem = new Models.StaticDataUpdateQueue
            {
                Staticdataid = insertStaticDataUpdateQueueRequest.ProductId,
                Staticdatatypeid = insertStaticDataUpdateQueueRequest.JobTypeId,
                Serviceid = insertStaticDataUpdateQueueRequest.ServiceId,
                Siteid = insertStaticDataUpdateQueueRequest.SiteId,
                Jobcreationdatetime = DateTime.UtcNow,
                Jobstatusid = 1,
                Retrycount = 0,
                Processingnote = string.Empty,
                Packageid = insertStaticDataUpdateQueueRequest.PackageId
            };
            _context.StaticDataUpdateQueue.Add(staticDataUpdateQueueItem);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> UpdatePublishedProductStatus(Guid publishedProductId, string status)
        {
            var publishedProduct = _context.PublishedProducts.FirstOrDefault(a=>a.PublishedProductId == publishedProductId);
            if(publishedProduct != null)
            {
                var statusId = _context.PublisherProductStatus.FirstOrDefault(a=>a.Productstatusname.ToLower() == status).Productstatusid;
                publishedProduct.Publisherproductstatusid = statusId;
                _context.Update(publishedProduct);
                var response = _context.SaveChanges();
                if (response > 0)
                    return true;
                else
                    return false;
                }
            return false;
        }

        public async Task<List<Entities.PublishedProductAllowedSubscriber>>  GetPublishedProductSubscriberList(Guid publishedProductId)
        {
            var publishedProductSubscriberList = (from pp in _context.PublishedProducts 
            join ppas in _context.PublishedProductAllowedSubscriber on pp.PublishedProductId equals ppas.Publishedproductid 
            where pp.PublishedProductId == publishedProductId
            select new Entities.PublishedProductAllowedSubscriber
            {
                PublisherProductId = pp.PublishedProductId,
                SubscriberId = ppas.Subscriberid
            }).ToList();
            return publishedProductSubscriberList;
        }

        public async Task<List<Entities.PublishedStatus>> GetPublishedStatus()
        {
            var result = (from pps in _context.PublisherProductStatus
            select new Entities.PublishedStatus{
                StatusId = pps.Productstatusid,
                StatusName = pps.Productstatusname
            }).ToList();            
            return result;
        }

        public async Task InsertPublicationHistoryForUnpublishProduct(Guid publishedProductId, Guid? subscriberId, int calledFrom)
        {
            var messageText = calledFrom == 1 ? "Product unpublished by publisher" : "";
            if(calledFrom == 2 && subscriberId != null)//include subscriber
            {   
                var subscriberName =_context.Subscriber.FirstOrDefault(a => a.SubscriberId == subscriberId).SubscriberName;
                messageText = "Access granted to Subscriber " + subscriberName;
            }
            else if(calledFrom == 3 && subscriberId != null)//exclude subscriber
            {   
                var subscriberName =_context.Subscriber.FirstOrDefault(a => a.SubscriberId == subscriberId).SubscriberName;
                messageText = "Access revoked from Subscriber " + subscriberName;
            }
            var publishedproducthistory = _context.PublishedProducts.Where(a=>a.PublishedProductId==publishedProductId).Select(q=> new PublishedProductsHistory
                {
                    PublishedProductHistoryId = Guid.NewGuid(),
                    PublishedProductId = q.PublishedProductId,
                    ProductId = q.ProductId,
                    PublisherId = q.PublisherId,
                    ProductTypeId = q.ProductTypeId,
                    ProductData = q.ProductData,
                    ProcessedOn = DateTime.UtcNow,
                    ProcessedBy = q.ProcessedBy,
                    ProductVersion = q.ProductVersion,
                    Publisherproductstatusid =  calledFrom == 1 ? 4 : q.Publisherproductstatusid, // unpublished
                    PublishedStatusId = q.PublishedStatusId,
                    ProcessingNote =  messageText,
                    PublishedOn = q.PublishedOn,
                    CreatedBy = _userId,
                    Messagetypeid = q.Messagetypeid,
                    Productsubstatusid = null,
                    Publishedproductqueueid = null,
                    Traceid=null,
                    Productdatadiff = q.Productdatadiff
                }).FirstOrDefault();
                 _context.PublishedProductsHistory.Add(publishedproducthistory);
                 await _context.SaveChangesAsync();
        }
        
        public async Task InsertSubscriptionHistoryForUnsubscribeProduct(Guid? subscriberProductId, int calledFrom, Guid? userId)
        {
            var messageText = string.Empty;
            messageText = calledFrom == 1 ? "Access revoked by publisher" : (calledFrom == 2 ? "Product unpublished by publisher" :  (calledFrom == 4 ? "Access granted by publisher" :"Product unsubscribed by subscriber"));
            var Subscriberproducthistory = _context.SubscriberProduct.Where(a=>a.Subscriberproductid==subscriberProductId).Select(q=> new SubscriberProductHistory
                {
                    Subscriberproductid  = q.Subscriberproductid,
                    Marketplaceproductid = q.Marketplaceproductid,
                    Subscriberid = q.Subscriberid,
                    Productstatusid = 5, //unsubscribed
                    Productdata = q.Productdata,
                    Processedon = DateTime.UtcNow,
                    Processedby = q.Processedby,
                    Productversion = q.Productversion,
                    Messagetypeid =  q.Messagetypeid,
                    Productsubstatusid = null,
                    Productstatusnote = messageText,
                    Subscribedon = q.Subscribedon,
                    CreatedBy = userId != null ? userId : _userId,
                    Subscriberproductqueueid = null,
                    Traceid= null,
                    Productdatadiff = q.Productdatadiff
                }).FirstOrDefault();

                 _context.SubscriberProductHistory.Add(Subscriberproducthistory);
                  _context.SaveChanges();
        }

        public async Task<string> GetPublishedProductHistoryJsonData(Guid publishedProductHistoryId)
        {
            var publishedProductsHistory = _context.PublishedProductsHistory.FirstOrDefault(a => a.PublishedProductHistoryId == publishedProductHistoryId);
            return publishedProductsHistory != null ? string.IsNullOrEmpty(publishedProductsHistory.Productdatadiff) ? publishedProductsHistory.ProductData : publishedProductsHistory.Productdatadiff : ""; 
		}

          public async Task<bool> IsImportSupplierAddress(int serviceTypeId)
        {
            var result = (from m in _context.MasterData 
            where m.Masterdataid == serviceTypeId
            select m.Servicetypepublishermainaddress).FirstOrDefault();
            return  result.HasValue ? !result.Value : true;
        }

        public async Task<PublisherDefaultDataModel> GetPublisherDefaultsForPackage(Guid publisherId)
        {
            PublisherDefaultDataModel response = new PublisherDefaultDataModel();
            
            response.PackagePriceStartDate = _context.PublisherDefault.FirstOrDefault(a=>a.PublisherId == publisherId)?.PackagePriceStartDate;
            response.PackageStatuses = _context.PublisherServiceStatus.Where(a=> a.PublisherId== publisherId && a.PackageStatusId != null).Select(q=>q.PackageStatusId.Value).ToList();

            return response;
        }

        public int GetPackageToServiceMapForType(Guid siteId, int dataTypeId, int sourceId)
        {
            var masterDataLink = _context.MasterDataLink.FirstOrDefault(a => a.Parentmasterdataid == sourceId);

            if (masterDataLink == null)
                return 0;

            return masterDataLink.Masterdataid;
        }

        
        public string GetMasterDataName(int masterDataId)
        {
            var masterData = _context.MasterData.FirstOrDefault(a => a.Masterdataid == masterDataId);

            if (masterData == null)
                return "";

            return masterData.Masterdataname.Trim();
            
        }

    }
}


//  dataModel.PublisherName = publisher.Result.PublisherName;
//                     dataModel.ProductName = item.Servicelongname;
//                     dataModel.SupplierName = item.SupplierName;
//                     dataModel.ServiceType = item.ServiceTypeName;
//                     dataModel.LocationName = item.RegionName;
//                     dataModel.Status = item.ServiceStatusName;
//                     dataModel.ProductId = item.Serviceid;
//                     if (responseFromDb != null)
//                     {
//                         dataModel.PublishedProductId = responseFromDb.PublishProductId;
//                         dataModel.IsPublished = true;
//                     }