using MarketPlaceService.DAL.Contract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MarketPlaceService.Entities;
using System.Linq;
using MarketPlaceService.DAL.Models;

namespace MarketPlaceService.DAL
{
    public class SubscriptionProductRepository : BaseRepository, ISubscriptionProductsRepository
    {
        private readonly ICommonRepository _commonRepository;
        private readonly IPublisherRepository _publisherRepository;
        private string _user = string.Empty;
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
        public SubscriptionProductRepository(MarketplaceDbContext context,ICommonRepository commonRepository, IPublisherRepository publisherRepository) : base(context)
        {
            _commonRepository = commonRepository;
             _publisherRepository = publisherRepository;   
        }

        public async Task<SubscriptionProduct> SubscriptionProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            if(HasSubscriberSubscribedProduct(subscriptionProductDataModel))
                return subscriptionProductDataModel;

            var GetMessageTypeId= await _commonRepository.GetMessageTypeId(subscriptionProductDataModel.MarketPlaceProductId);

            //var processingStatues = await GetProcessingStatus();
            var subscribeStatus = await GetSubscribedStatus();
            //var queuedStatusId = processingStatues.Where(a=>a.StatusName.ToLower().Equals("new")).FirstOrDefault().StatusId;
            var pendingSubscriptionStatusId = subscribeStatus.FirstOrDefault(a=> a.Name.ToLower().Equals("pending")).Id;

            SubscriberProduct subscribeProductsItem = null;
            if (subscriptionProductDataModel.SubscribeProductId == Guid.Empty)
            {
                subscribeProductsItem = new SubscriberProduct
                {
                    Subscriberproductid = Guid.NewGuid(),
                    Marketplaceproductid = subscriptionProductDataModel.MarketPlaceProductId,
                    Subscriberid = subscriptionProductDataModel.SubscriberId,
                    Productversion = subscriptionProductDataModel.ProductVersion,
                    Productdata = string.Empty,
                    Productstatusid = 1,
                    Processedon = DateTime.UtcNow,
                    Subscribedon = DateTime.UtcNow,
                    Processedby = subscriptionProductDataModel.ProcessedBy != null ? subscriptionProductDataModel.ProcessedBy : string.Empty,
                    Subscribedby = _userId,
                    Productstatusnote = string.Empty,
                    Messagetypeid = GetMessageTypeId,
                   // Traceid = subscriptionProductDataModel.TraceId

                    // ProductSubStatusId =subscriptionProductDataModel.ProductSubStatusId

                };
                _context.SubscriberProduct.Add(subscribeProductsItem);
                subscriptionProductDataModel.SubscribeProductId = subscribeProductsItem.Subscriberproductid;
            }
            else
            {
                subscribeProductsItem = _context.SubscriberProduct.FirstOrDefault(a => a.Subscriberproductid == subscriptionProductDataModel.SubscribeProductId);

                subscribeProductsItem.Productstatusnote = string.Empty;
                subscribeProductsItem.Productstatusid = 1;
                subscribeProductsItem.Productsubstatusid = null;
                subscribeProductsItem.Subscribedon = DateTime.UtcNow;
                subscribeProductsItem.Processedon = DateTime.UtcNow;
                _context.SubscriberProduct.Update(subscribeProductsItem);
            }

            // Call Subscription  product queue
            SubscriberProductQueue SubscribedProductQueueItem = new SubscriberProductQueue
            {
                //Subscriberproductqueueid = Guid.NewGuid(),
                Marketplaceproductid = subscriptionProductDataModel.MarketPlaceProductId,
                Subscriberproductid = subscriptionProductDataModel.SubscribeProductId,
                Subscriberid = subscriptionProductDataModel.SubscriberId,
                Messagetypeid =GetMessageTypeId,
                Jobnote=string.Empty,
                Jobcreationdatetime=DateTime.UtcNow,
                Jobstatusid = 1,
                Jobtypeid = 1,
                Traceid = subscriptionProductDataModel.TraceId
            };
            _context.SubscriberProductQueue.Add(SubscribedProductQueueItem);
            await _context.SaveChangesAsync();
            return await Task.FromResult(subscriptionProductDataModel);
        }

        public bool HasSubscriberSubscribedProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            var allowedStatuses = new[] {1,2,4};
            var existingSubscribedProduct = GetExistingProduct(subscriptionProductDataModel.SubscriberId, subscriptionProductDataModel.MarketPlaceProductId);
            
            if((existingSubscribedProduct != null && existingSubscribedProduct.Subscriberproductid == Guid.Empty) //status was no

                || (existingSubscribedProduct != null && existingSubscribedProduct.Subscriberproductid != Guid.Empty 
                    && allowedStatuses.Contains((short)existingSubscribedProduct.Productstatusid )))
                return true;

            return false;
        }

        private SubscriberProduct GetExistingProduct(Guid subscriberId, Guid marketplaceProductId)
        {
           return  (from sp in _context.SubscriberProduct 
               
                where sp.Marketplaceproductid == marketplaceProductId && sp.Subscriberid == subscriberId
                select sp).FirstOrDefault();
        }

        //private async Task<IEnumerable<Entities.ProcessingStatus>> GetProcessingStatus()
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

        private async Task<IEnumerable<Entities.Status>> GetSubscribedStatus()
        {
             IList<Entities.Status> statuses = new List<Entities.Status>();
             statuses = (from s in _context.SubscribeProductStatus
             select new Entities.Status
             {
                 Id = s.Productstatusid,
                 Name = s.Productstatusname
             }).ToList();

             return await Task.FromResult(statuses);
        }

        public async Task<IEnumerable<SubscriptionStatus>> GetSubscriptionStatus()
        {
            List<SubscriptionStatus> GetSubscriptionStatusList=new List<SubscriptionStatus>();
            GetSubscriptionStatusList = (from sps in _context.SubscribeProductStatus
            select new Entities.SubscriptionStatus{
                SubscriptionStatusId = sps.Productstatusid,
                SubscriptionStatusName = sps.Productstatusname
            }).ToList();     
            GetSubscriptionStatusList.Add(new SubscriptionStatus {
                SubscriptionStatusId=6,
                SubscriptionStatusName="No"
            });
            return await Task.FromResult(GetSubscriptionStatusList);
        }

        public async Task<IEnumerable<MarketPlaceProduct>> SearchSubscriptionProduct(MarketPlaceProductsSearch marketPlaceProductSearchDataModel)
        {
            List<MarketPlaceProduct> GetMarketPlaceProducts=new List<MarketPlaceProduct>();
            List<int> allRegionIDs=GetRegionAndChildRegions(marketPlaceProductSearchDataModel.RegionId);
            var subscriberProductStatuses = _context.SubscribeProductStatus.Select(q=>q).ToList();
            var subscriberProductSubStatuses = _context.SubscribeProductSubStatus.Select(q=>q).ToList();
            


            GetMarketPlaceProducts = (from s in _context.MarketplaceProduct
                                      join p in _context.PublishedProducts on s.Publishedproductid equals p.PublishedProductId
                                      join r in _context.MasterRegions on s.Regionid equals r.Regionid
                                      join sub in _context.PublishedProductAllowedSubscriber on p.PublishedProductId equals sub.Publishedproductid
                                      where p.PublisherId == marketPlaceProductSearchDataModel.PublisherId
                                      && sub.Subscriberid==marketPlaceProductSearchDataModel.SubscriberId
                                      && s.Servicetypeid == marketPlaceProductSearchDataModel.ProductTypeId
                                      && allRegionIDs.Contains(r.Regionid)
                                      select new MarketPlaceProduct
                                      {
                                          MarketPlaceProductId = s.Marketplaceproductid,
                                          PublishedProductId = s.Publishedproductid,
                                          ProcessedOn = s.Processedon,
                                          ProcessedBy = s.Processedby,
                                          ServiceTypeId = s.Servicetypeid,
                                          RegionId = s.Regionid,
                                          ProductLongName = p.ProductTypeId == 2 ? s.Productlongname + " (Pkg)" : s.Productlongname,
                                          ProductShortName = s.Productshortname,
                                          RegionName = r.Regionname,                                          
                                          MessageTypeId = (int)p.Messagetypeid,
                                          IsSubscribed = false,
                                          IsSubscribable = true                                          
                                      }).ToList();

            GetMarketPlaceProducts = UpdateSubscribedData(GetMarketPlaceProducts, marketPlaceProductSearchDataModel);

            if(marketPlaceProductSearchDataModel.SubscriptionStatusId == 6) //this is for subscription status as NO
            {
                GetMarketPlaceProducts = GetMarketPlaceProducts.Where(a => a.ProcessingStatusId == 0).ToList();
            }
            else if(marketPlaceProductSearchDataModel.SubscriptionStatusId > 0)
            {
                GetMarketPlaceProducts = GetMarketPlaceProducts.Where(a => a.ProcessingStatusId == marketPlaceProductSearchDataModel.SubscriptionStatusId).ToList();
            }

            if(marketPlaceProductSearchDataModel.Ratings !=null && marketPlaceProductSearchDataModel.Ratings.Any())
            {        
                var marketplaceproductIds = (from pp in _context.PublishedProducts 
                join mp in _context.MarketplaceProduct on pp.PublishedProductId equals mp.Publishedproductid
                join mpr in _context.MarketplaceProductRating on mp.Marketplaceproductid equals mpr.Marketplaceproductid
                where marketPlaceProductSearchDataModel.Ratings.Contains(mpr.Ratingid)
                select mp.Marketplaceproductid).ToList();
               
                GetMarketPlaceProducts = GetMarketPlaceProducts.Where(a=> marketplaceproductIds.Contains(a.MarketPlaceProductId)).ToList();
            }

            if(!string.IsNullOrEmpty(marketPlaceProductSearchDataModel.ProductLongName))
            {
                 GetMarketPlaceProducts = GetMarketPlaceProducts.Where(a=> a.ProductLongName.ToLower().Contains(marketPlaceProductSearchDataModel.ProductLongName.ToLower())).ToList();
            }

            if(!string.IsNullOrEmpty(marketPlaceProductSearchDataModel.ProductShortName))
            {
                GetMarketPlaceProducts = GetMarketPlaceProducts.Where(a=> a.ProductShortName.ToLower().Contains(marketPlaceProductSearchDataModel.ProductShortName.ToLower())).ToList();
            }

            GetMarketPlaceProducts.Where(q=>q.IsSubscribed).ToList().ForEach(a=> a.ProcessingStatusName = subscriberProductStatuses.First(s=> s.Productstatusid == a.ProcessingStatusId).Productstatusname);

            GetMarketPlaceProducts.Where(q=>q.IsSubscribed && q.ProductSubStatusId != null).ToList().ForEach(a=> a.ProductSubStatusName = subscriberProductSubStatuses.First(s=> s.Productsubstatusid == a.ProductSubStatusId).Productsubstatusname);         
                     
           return await Task.FromResult(GetMarketPlaceProducts);
        }

        private List<MarketPlaceProduct> UpdateSubscribedData(List<MarketPlaceProduct> data, MarketPlaceProductsSearch marketPlaceProductSearchDataModel)
        {
            if (data == null)
                return null;

            var resubscribableProductStatuses = new[] { 3, 5 }; //if a product is in the following status then it can be subscribed to

            var subscriberSiteId = _context.Subscriber.FirstOrDefault(a => a.SubscriberId == marketPlaceProductSearchDataModel.SubscriberId).SiteId;

            var subscribedProductsBySubscriberForPublisher = (from sp in _context.SubscriberProduct
                                                              join s in _context.Subscriber on sp.Subscriberid equals s.SubscriberId
                                                              join mp in _context.MarketplaceProduct on sp.Marketplaceproductid equals mp.Marketplaceproductid
                                                              join p in _context.PublishedProducts on mp.Publishedproductid equals p.PublishedProductId
                                                              where s.SiteId == subscriberSiteId && p.PublisherId == marketPlaceProductSearchDataModel.PublisherId
                                                              select new
                                                              {
                                                                  MarketPlaceProductId = mp.Marketplaceproductid,
                                                                  SubscriberProductId = sp.Subscriberproductid,
                                                                  ProductStatusId = sp.Productstatusid,
                                                                  s.SubscriberId,
                                                                  s.SubscriberName,
                                                                  ErrorMessage = sp.Productstatusnote,
                                                                  ProductSubStatusId = sp.Productsubstatusid,
                                                                  TraceId = sp.Traceid
                                                              }).ToList();

            foreach(var product in data)
            {   
                var alreadySubscribedProduct = subscribedProductsBySubscriberForPublisher.FirstOrDefault(a => a.MarketPlaceProductId == product.MarketPlaceProductId);

                if (alreadySubscribedProduct == null)
                    continue;

                product.ProcessingStatusId = alreadySubscribedProduct.ProductStatusId;
                product.IsSubscribed = true;
                product.ProductDisabled = alreadySubscribedProduct.SubscriberId != marketPlaceProductSearchDataModel.SubscriberId;
                product.IsSubscribable = !product.ProductDisabled && resubscribableProductStatuses.Contains(alreadySubscribedProduct.ProductStatusId);
                product.UnSubscribableReason = product.ProductDisabled ? $"This product cannot be subscribed to as it has already been subscribed to by {alreadySubscribedProduct.SubscriberName}." : string.Empty;
                product.SubscriberProductId = alreadySubscribedProduct.SubscriberProductId;
                product.ProductSubStatusId = alreadySubscribedProduct.ProductSubStatusId;
                product.TraceId = alreadySubscribedProduct.TraceId;
                if(product.IsSubscribable)
                {
                    product.ErrorMessage = alreadySubscribedProduct.ErrorMessage;
                    
                }
            }


            return data;
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


        public async Task<SubscriptionProduct> UpdateSubscriptionProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            short? subStatusErrorId = null;
            string errorText=null;

            if(subscriptionProductDataModel.Errors != null && subscriptionProductDataModel.Errors.Count > 0)
            {
                var error = _commonRepository.GetErrorDetails(subscriptionProductDataModel.Errors.FirstOrDefault());
                subStatusErrorId = error.ErrorId;
                errorText = error.ErrorMessage;
            }

            SubscriberProduct subscribeProduct=new SubscriberProduct();
            subscribeProduct = (from sp in _context.SubscriberProduct
            where sp.Subscriberproductid==subscriptionProductDataModel.SubscribeProductId
            select sp).FirstOrDefault();

            subscribeProduct.Productdata = subscriptionProductDataModel.ProductData??subscribeProduct.Productdata;
            subscribeProduct.Productstatusid = subscriptionProductDataModel.ProductStatusId==null ? subscribeProduct.Productstatusid : subscriptionProductDataModel.ProductStatusId;
            subscribeProduct.Productsubstatusid = subStatusErrorId ?? subscribeProduct.Productsubstatusid;
            subscribeProduct.Productstatusnote = errorText ??subscribeProduct.Productstatusnote;
            subscribeProduct.Processedon =  DateTime.UtcNow;
            subscribeProduct.Tsid = subscriptionProductDataModel.TsId ?? subscribeProduct.Tsid;
            subscribeProduct.Traceid=subscriptionProductDataModel.TraceId;
            subscribeProduct.Productversion = subscriptionProductDataModel.ProductVersion == 0 ? subscribeProduct.Productversion : subscriptionProductDataModel.ProductVersion;
            subscribeProduct.Productdatadiff = subscriptionProductDataModel.ProductUpdateDifferenceData?? subscribeProduct.Productdatadiff;

            if (subscriptionProductDataModel.UpdateProductHistory)
            {
                var subscriberProductHistory = _context.SubscriberProductHistory.FirstOrDefault(a => a.Subscriberproductid == subscriptionProductDataModel.SubscribeProductId && a.Productstatusid == 4);
                if(subscriberProductHistory!=null)
                {
                    subscriberProductHistory.Productstatusid = subscriptionProductDataModel.ProductStatusId;
                    subscriberProductHistory.Productsubstatusid = subStatusErrorId ?? subscriberProductHistory.Productsubstatusid;
                    subscriberProductHistory.Productstatusnote = errorText ?? subscriberProductHistory.Productstatusnote;
                    subscriberProductHistory.Processedon = DateTime.UtcNow;
                }
            }


            _context.SubscriberProduct.Update(subscribeProduct);
            await _context.SaveChangesAsync();
            return await Task.FromResult(subscriptionProductDataModel);

        }

        public async Task<SubscriptionProduct> InsertSubscriberProductHistory(QueuedSubscription queuedSubscriberDataModel)
        {
            SubscriptionProduct response=new SubscriptionProduct();
            var subscriberProductQueueRecord = _context.SubscriberProductQueue.Where(a=>a.Subscriberproductqueueid==queuedSubscriberDataModel.JobId).FirstOrDefault();
            short? subStatusErrorId = null;
            var  subscriberProductStatuses = _context.SubscribeProductStatus.Select(a => a).ToList();
            var jobHistoryStatuses = _context.JobHistoryStatus.Select(a => a).ToList();
            short? subscribeProductStatusId = 4; // subscriberProductStatuses.FirstOrDefault(a => a.Productstatusname.ToLower().Equals("subscribed-pending confirmation")).Productstatusid;
            short? jobHistoryStatusId = jobHistoryStatuses.FirstOrDefault(a => a.Jobstatusname.ToLower().Equals("success")).Jobstatusid;
            string errorText=null;


            if(queuedSubscriberDataModel.Errors!= null && queuedSubscriberDataModel.Errors.Count > 0)
            {
                var error = _commonRepository.GetErrorDetails(queuedSubscriberDataModel.Errors.FirstOrDefault());
                subStatusErrorId = error.ErrorId;
                errorText = error.ErrorMessage;
                subscribeProductStatusId = subscriberProductStatuses.FirstOrDefault(a => a.Productstatusname.ToLower().Equals("error")).Productstatusid;
                jobHistoryStatusId = jobHistoryStatuses.FirstOrDefault(a => a.Jobstatusname.ToLower().Equals("error")).Jobstatusid;
            }


            if(queuedSubscriberDataModel.InsertSubscriptionHistory && subscriberProductQueueRecord.Jobtypeid != 3)
            {
                var Subscriberproducthistory = _context.SubscriberProduct.Where(a=>a.Subscriberproductid==subscriberProductQueueRecord.Subscriberproductid).Select(q=> new SubscriberProductHistory
                {
                    Subscriberproductid  = q.Subscriberproductid,
                    Marketplaceproductid = q.Marketplaceproductid,
                    Subscriberid = q.Subscriberid,
                    Productstatusid = (short)subscribeProductStatusId,
                    Productdata = q.Productdata,
                    Processedon = q.Processedon,
                    Processedby = q.Processedby,
                    Productversion = q.Productversion,
                    Messagetypeid =  q.Messagetypeid,
                    Productsubstatusid = subStatusErrorId ?? q.Productsubstatusid,
                    Productstatusnote = errorText ?? q.Productstatusnote,
                    Subscribedon = q.Subscribedon,
                    CreatedBy= q.Subscribedby,
                    Subscriberproductqueueid = queuedSubscriberDataModel.JobId,
                    Traceid=queuedSubscriberDataModel.TraceId,
                    Productdatadiff = q.Productdatadiff
                }).FirstOrDefault();

                   
                 _context.SubscriberProductHistory.Add(Subscriberproducthistory);
            }            

            var SubscriberProductQueueHistory = _context.SubscriberProductQueue.Where(a=>a.Subscriberproductqueueid==queuedSubscriberDataModel.JobId).Select(q=> new SubscriberProductQueueHistory
            {
                Marketplaceproductid = q.Marketplaceproductid,
                Subscriberid = q.Subscriberid,
                Messagetypeid = q.Messagetypeid,
                Jobnote = errorText ?? q.Jobnote,
                Retrycount = q.Retrycount,
                Jobcreationdatetime=q.Jobcreationdatetime,
                Jobstartdatetime = (DateTime)q.Jobstartdatetime,
                Jobenddatetime = (DateTime)q.Jobenddatetime,
                Productsubstatusid= subStatusErrorId,
                Subscriberproductid = q.Subscriberproductid,
                Jobhistorystatusid= (short)jobHistoryStatusId,
                Subscriberproductqueueid = q.Subscriberproductqueueid,
                Traceid=queuedSubscriberDataModel.TraceId,
                Jobtypeid = q.Jobtypeid
            }).FirstOrDefault();
           
            _context.SubscriberProductQueueHistory.Add(SubscriberProductQueueHistory);
            await _context.SaveChangesAsync();
            return response;
        }

      public async Task<IEnumerable<QueuedSubscription>> GetQueuedSubscriptions(int limit, string serverName)
      {
          IList<QueuedSubscription> Subscription=new List<QueuedSubscription>();
           Subscription = (from sub in _context.SubscriberProductQueue 
           join status in _context.JobStatus on sub.Jobstatusid equals status.Jobstatusid
          where status.Jobstatusname.ToLower().Equals("queued") || status.Jobstatusname.ToLower().Equals("retry")
            select new QueuedSubscription
            {
                JobId = sub.Subscriberproductqueueid,
                MarketplaceProductId = sub.Marketplaceproductid,
                SubscriberId = sub.Subscriberid,
                MessageTypeId = sub.Messagetypeid,
                RetryCount = sub.Retrycount, 
                SubscriberProductId = sub.Subscriberproductid ,
                JobTypeId = sub.Jobtypeid,
                TraceId=sub.Traceid??Guid.NewGuid()
            }).Take(limit).ToList<QueuedSubscription>();

            if(Subscription!=null && Subscription.Count()>0)
            {
                var inProcessStatusId = (from status in _context.JobStatus where status.Jobstatusname.ToLower().Equals("processing")
                select status.Jobstatusid).FirstOrDefault();
                foreach(var row in Subscription)
                {
                    row.JobStatusId = inProcessStatusId;
                    row.JobStartDateTime = DateTime.UtcNow;
                    row.JobEndDateTime = new DateTime(1800,01,01);
                    row.ProcessedBy = serverName;
                    await UpdateSubscriptionQueue(row);
                }             
            }
          return await Task.FromResult(Subscription);
      }

      public async Task<QueuedSubscription> GetQueuedSubscriptionsById(Guid subScriberProductQueuedId)
      {
          QueuedSubscription Subscription=new QueuedSubscription();
          var inProcessStatusId = (from status in _context.JobStatus where status.Jobstatusname.ToLower().Equals("processing")
          select status.Jobstatusid).FirstOrDefault();
           Subscription = (from sub in _context.SubscriberProductQueue 
            select new QueuedSubscription
            {
                JobId = sub.Subscriberproductqueueid,
                MarketplaceProductId = sub.Marketplaceproductid,
                SubscriberId = sub.Subscriberid,
                MessageTypeId = sub.Messagetypeid,
                RetryCount = sub.Retrycount,
                JobNote=sub.Jobnote,
                JobStatusId=sub.Jobstatusid,
                JobCreationDateTime=sub.Jobcreationdatetime,
                JobStartDateTime=sub.Jobstartdatetime,
                JobEndDateTime=sub.Jobenddatetime,    
                TraceId=sub.Traceid??Guid.NewGuid()        
            }).FirstOrDefault();
          return await Task.FromResult(Subscription);
      }

      public async Task DeleteSubscriptionProductQueue(Guid subScriberProductQueuedId)
      {
          SubscriberProductQueue SubscriptionProductQeueue=new SubscriberProductQueue();
           
            SubscriptionProductQeueue = _context.SubscriberProductQueue.FirstOrDefault(a=>a.Subscriberproductqueueid==subScriberProductQueuedId);

            _context.SubscriberProductQueue.Remove(SubscriptionProductQeueue);
            await _context.SaveChangesAsync();
      }

      public async Task<QueuedSubscription> UpdateSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel)
      {
            SubscriberProductQueue queuedSubscription = _context.SubscriberProductQueue.Where(a=>a.Subscriberproductqueueid==queuedSubscriptionDataModel.JobId).FirstOrDefault();
            if(queuedSubscription!=null)
            {
                DateTime defaultdt = new DateTime(1800,01,01);
                queuedSubscription.Retrycount = queuedSubscriptionDataModel.RetryCount;
                if(queuedSubscriptionDataModel.JobStartDateTime > defaultdt)
                {
                    queuedSubscription.Jobstartdatetime = queuedSubscriptionDataModel.JobStartDateTime;
                }

                if(queuedSubscriptionDataModel.JobEndDateTime>defaultdt)
                {
                    queuedSubscription.Jobenddatetime = queuedSubscriptionDataModel.JobEndDateTime;
                }
                queuedSubscription.Jobstatusid = queuedSubscriptionDataModel.JobStatusId;
                queuedSubscription.Jobnote = string.IsNullOrEmpty(queuedSubscriptionDataModel.JobNote) ? queuedSubscription.Jobnote: queuedSubscriptionDataModel.JobNote;

                _context.SubscriberProductQueue.Update(queuedSubscription);
                await _context.SaveChangesAsync();
            }
            
            return await Task.FromResult(queuedSubscriptionDataModel);
      }

        public async Task<IEnumerable<Entities.Status>> GetSubscriberProductStatus()
        {
             IList<Entities.Status> status = new List<Entities.Status>();
             status = (from m in _context.SubscribeProductStatus 
             select new Entities.Status
             {
                 Id = m.Productstatusid,
                 Name = m.Productstatusname
             }).ToList();

             return status;
        }

        public async Task<int> GetMessageTypeIdAsync(Guid subscriberProductId)
        {
            int messageTypeId = 0;          
            var subscriberProduct = (from subscriberProducts in _context.SubscriberProduct where subscriberProducts.Subscriberproductid == subscriberProductId  select subscriberProducts).FirstOrDefault();
            if(subscriberProduct != null)
                messageTypeId = (int)subscriberProduct.Messagetypeid;

            return messageTypeId;
        }

        public async Task<int> GetMappingDirection()
        {
            int mappingDirectionId=0;
            string mappingDirectionName="MP-Site";
            var mappingDirection=(from mp in _context.MappingDirection where mp.Mappingdirectionname ==mappingDirectionName select mp).FirstOrDefault();
            if(mappingDirection!=null)
            {
               mappingDirectionId=mappingDirection.Mappingdirectionid;
            }
            return mappingDirectionId;
        }

        public async Task<string> GetMarketplaceJsonString(Guid marketplaceProductId)
        {
            var response = string.Empty;
            var result = (from mp in _context.MarketplaceProduct
                            where mp.Marketplaceproductid == marketplaceProductId
                            select mp).FirstOrDefault();
            if(result != null) 
                response = result.Productdata;
            return response;
        }

        public async Task<string> GetSubscriptionJsonString(Guid marketplaceProductId, Guid subscriberId)
        {
            var response = string.Empty;
            var result = _context.SubscriberProduct.Where(a => a.Marketplaceproductid == marketplaceProductId && a.Subscriberid == subscriberId).FirstOrDefault();
            if(result != null) 
                response = !string.IsNullOrEmpty(result.Productdatadiff) ? result.Productdatadiff : result.Productdata;
            return response;
        }
        public async Task<IEnumerable<HistoryDetail>> GetSubscriptionHistoryDetails(Guid marketplaceProductId, Guid subscriberId)
        {
            var response = new List<HistoryDetail>();
            response = (from sph in _context.SubscriberProductHistory
                        join sps in _context.SubscribeProductStatus
                        on sph.Productstatusid equals sps.Productstatusid
                        where sph.Marketplaceproductid == marketplaceProductId 
                        && sph.Subscriberid == subscriberId
                        select new HistoryDetail{
                            Date = sph.Processedon,
                            Event = (sph.Productversion>1 && sph.Productstatusid == 2)? "Updated" :sps.Productstatusname,
                            Message = sph.Productstatusnote,
                            UserName = _context.UserAccount.Where(z => z.Id == sph.CreatedBy).Select(y => y.FirstName + ' ' + y.LastName).FirstOrDefault(),
                            SubStatusId = sph.Productsubstatusid,
                            SubStatusName = sph.Productsubstatusid != null ? _context.SubscribeProductSubStatus.FirstOrDefault(a=>a.Productsubstatusid == sph.Productsubstatusid).Productsubstatusname : null,
                            TraceId = sph.Traceid,
                            HistoryId = sph.Subscriberproducthistoryid,
                            JsonExists = !string.IsNullOrEmpty(sph.Productdata) ? true : false
                        }).ToList();

            return response;
        }
        public async Task<string> GetPublishedByName(Guid marketplaceProductId)
        {
            var response = string.Empty;
            var result = (from mp in _context.MarketplaceProduct 
                            join pp in _context.PublishedProducts on mp.Publishedproductid equals pp.PublishedProductId
                            join p in _context.Publisher on pp.PublisherId equals p.PublisherId
                            where mp.Marketplaceproductid == marketplaceProductId
                            select p).FirstOrDefault();
            if(result != null) 
                response = result.PublisherName;
            return response;
        }

        public async Task<DateTime> GetFirstSubscriptionDate(Guid marketplaceProductId, Guid subscriberId)
        {
            var response = new DateTime();
            var result = (from sp in _context.SubscriberProduct 
                            where sp.Marketplaceproductid == marketplaceProductId &&
                                sp.Subscriberid == subscriberId
                            select sp).FirstOrDefault();
            if(result != null) 
                response = result.Subscribedon;
            return response;
        }

        public SubscriberDefaultsModelForSubscription GetSubscriberDefaults(Guid subscriberId, Guid marketplaceProductId)
        {
            SubscriberDefaultsModelForSubscription response = new SubscriberDefaultsModelForSubscription();
            var serviceTypeId = _context.MarketplaceProduct.Where(a=>a.Marketplaceproductid == marketplaceProductId).Select(x=>x.Servicetypeid).FirstOrDefault();
            var regionId = _context.MarketplaceProduct.Where(a=>a.Marketplaceproductid == marketplaceProductId).Select(x=>x.Regionid).FirstOrDefault();

            var chargingPolicies = (from scp in _context.SubscriberChargingPolicy
                                    where scp.SubscriberId == subscriberId 
                                    select new ServiceTypeTypeChargingPolicy{
                                       ServiceTypeTypeId = scp.ServiceTypeTypeId,
                                       OptionChargingPolicyId = scp.OptionChargingPolicyId,
                                       ExtraChargingPolicyId = scp.ExtraChargingPolicyId
                                    }).ToList();

            response.ChargingPolicy = new SubscriberChargingPolicyDataModel();
            response.ChargingPolicy.DefaultChargingPolicy = new List<ServiceTypeTypeChargingPolicy>();
            response.ChargingPolicy.DefaultChargingPolicy = chargingPolicies;

            var defaults = (from sd in _context.SubscriberDefault
                            where sd.SubscriberId == subscriberId
                            select new SubscriberDefaultDataModel{
                                SeasonTypeID = sd.SeasonTypeId,
                                BuyPriceTypeID = sd.BuyPriceTypeId,
                                BuyBookingTypeID = sd.BuyBookingTypeId
                            }).FirstOrDefault();

          
            if(defaults != null)
            {
                response.Defaults = new SubscriberDefaultDataModel{
                    SeasonTypeID = defaults.SeasonTypeID,
                    BuyPriceTypeID = defaults.BuyPriceTypeID,
                    BuyBookingTypeID = defaults.BuyBookingTypeID,
                };
               
            }

            response.ChargingPolicy.DefaultCommunicationType = new CommunicationType();
            response.ChargingPolicy.DefaultCommunicationType.Id = _context.SubscriberDefault.FirstOrDefault(a=>a.SubscriberId == subscriberId)?.CommunicationTypeId;

            response.Suppliers = (from mp in _context.MarketplaceProduct
                                    join pp in _context.PublishedProducts on mp.Publishedproductid equals pp.PublishedProductId
                                    join ss in _context.SubscriberSupplier on pp.PublisherId equals ss.PublisherId
                                    where mp.Marketplaceproductid == marketplaceProductId && 
                                            ss.SubscriberId == subscriberId 
                                            select new SubscriberSupplierDataModel{
                                                SupplierId = ss.SupplierId,
                                                SubscriberId = ss.SubscriberId
                                            }).ToList();

            response.ProductCodeRules = (from spc in _context.SubscriberProductCode 
                                            join spcst in _context.SubscriberProductCodeServiceType on spc.SubscriberProductCodeId equals spcst.SubscriberProductCodeId
                                            where (spc.RegionId == null || spc.RegionId == regionId) && spcst.ServiceTypeId == serviceTypeId && spc.SubscriberId == subscriberId 
                                            
                                            select new SubscriberDefaultsProductCode{
                                                ProductCodeId=spc.ProductCodeId,
                                                ApplyToExtras = (bool)spc.ApplytoExtras,
                                                ApplyToOptions = (bool)spc.ApplytoOptions,
                                                AllServiceTypesSelected = spc.Allservicetypes
                                            }).ToList();

             List<SubscriberDefaultSellingPrice> sellingPriceRes = _context.SubscriberDefaultSellingPrices.Where(a=>a.Subscriberid==subscriberId).Select(q=> new SubscriberDefaultSellingPrice
            {
                RuleId=q.Subscriberdefaultsellingpriceid,
                SubscriberId = subscriberId,
                RegionId = q.Regionid,
                Sequence=q.Sequence,
                AllServiceTypesSelected=q.Allservicetypes,
                ServiceTypes=_context.SubscriberDefaultSellingPriceServiceType.Where(a=>a.Subscriberdefaultsellingpriceid==q.Subscriberdefaultsellingpriceid).Select(a=>a.Servicetypeid).ToList(),
                SubscriberDefaultSellingPricePolicy=_context.SubscriberDefaultSellingPricePolicies.Where(a=>a.Subscriberdefaultsellingpriceid==q.Subscriberdefaultsellingpriceid).Select(x=>new SubscriberDefaultSellingPricePolicy{
                   SubscriberDefaultSellingPricePolicyId=x.Subscriberdefaultsellingpricepolicyid,
                   BookingTypeId=x.Bookingtypeid,
                   PriceTypeId=x.Pricetypeid,
                   TaxId=x.Taxid,
                }).ToList(),
            }).ToList();

        
            response.SellingPrices = sellingPriceRes;
           
            return response;
        }

        public SiteDataModel GetSiteDetailsFromMarketplaceProduct(Guid marketplaceProductId)
        {
            var site = (from mp in _context.MarketplaceProduct
                       join pp in _context.PublishedProducts on mp.Publishedproductid equals pp.PublishedProductId
                       join p in _context.Publisher on pp.PublisherId equals p.PublisherId
                       join s in _context.Site on p.SiteId equals s.SiteId
                       where mp.Marketplaceproductid == marketplaceProductId
                       select new SiteDataModel
                       {
                           SiteName = s.SiteName,
                           SiteId = s.SiteId
                       }).FirstOrDefault();

            return site;
        }

        public async Task<QueuedSubscription> InsertSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel)
        {
            var subscriberProduct = _context.SubscriberProduct.FirstOrDefault(a => a.Marketplaceproductid == queuedSubscriptionDataModel.MarketplaceProductId && a.Subscriberid == queuedSubscriptionDataModel.SubscriberId);

            if (subscriberProduct == null)
                return null;

            if (subscriberProduct.Productstatusid == 5)
                return null;

            var existingingJobs = _context.SubscriberProductQueue.Where(a => a.Subscriberproductid == subscriberProduct.Subscriberproductid && a.Jobtypeid == queuedSubscriptionDataModel.JobTypeId).ToList();

            if (existingingJobs != null && existingingJobs.Any())
            {
                if (existingingJobs.Any(a => a.Jobstatusid == 4))
                    return null;

                var subscribeJobStatusesForDormantJob = new short[]{ 3, 4 };

                if (subscribeJobStatusesForDormantJob.Contains(subscriberProduct.Productstatusid) || existingingJobs.Any())
                {
                    AddSubscriberProductQueue(subscriberProduct, _user, 4, queuedSubscriptionDataModel.JobTypeId);
                    _context.SaveChanges();
                    return queuedSubscriptionDataModel;
                }
            }

            AddSubscriberProductQueue(subscriberProduct, _user, 1, queuedSubscriptionDataModel.JobTypeId);

            _context.SaveChanges();
            return queuedSubscriptionDataModel;
        }

        private void AddSubscriberProductQueue(SubscriberProduct subscriptionProductDataModel, string user, short jobStatusId, short? jobTypeId)
        {
            SubscriberProductQueue SubscribedProductQueueItem = new SubscriberProductQueue
            {
                //Subscriberproductqueueid = Guid.NewGuid(),
                Marketplaceproductid = subscriptionProductDataModel.Marketplaceproductid,
                Subscriberproductid = subscriptionProductDataModel.Subscriberproductid,
                Subscriberid = subscriptionProductDataModel.Subscriberid,
                Messagetypeid = subscriptionProductDataModel.Messagetypeid,
                Jobnote = string.Empty,
                Jobcreationdatetime = DateTime.UtcNow,
                Jobstatusid = jobStatusId ,
                Jobtypeid = jobTypeId
            };
            _context.SubscriberProductQueue.Add(SubscribedProductQueueItem);
        }

        public SubscriptionProduct GetExistingSubscriptionProduct(Guid marketplaceProductId, Guid subscriberId)
        {
            return _context.SubscriberProduct.Where(a => a.Marketplaceproductid == marketplaceProductId && a.Subscriberid == subscriberId && a.Tsid > 0).Select(q => new SubscriptionProduct
            {
                ProductData = q.Productdata,
                ProductVersion = q.Productversion
            }).FirstOrDefault();
        }

        public List<Guid> GetSubscriberIdsForSubscribedProduct(Guid marketplaceProductId)
        {
            return _context.SubscriberProduct.Where(a => a.Marketplaceproductid == marketplaceProductId).Select(q => q.Subscriberid).ToList();
        }

        public async Task<bool> UnsubscribeProduct(Guid subscriberProductId, Guid marketplaceProductId)
        {
            var getSubscribedProduct=_context.SubscriberProduct.Where(a=>a.Subscriberproductid == subscriberProductId).Select(x=>x).FirstOrDefault();
            if(getSubscribedProduct!=null)
            {
                getSubscribedProduct.Productstatusid=_context.SubscribeProductStatus.FirstOrDefault(a => a.Productstatusname.ToLower().Equals("unsubscribed")).Productstatusid;
                _context.SubscriberProduct.Update(getSubscribedProduct);
                _context.SaveChanges();

               _publisherRepository.InsertSubscribeProductQueue(getSubscribedProduct.Marketplaceproductid,getSubscribedProduct.Subscriberproductid,getSubscribedProduct.Subscriberid, 3);
               await  _publisherRepository.InsertSubscriptionHistoryForUnsubscribeProduct(subscriberProductId, 3, _userId);
               return true;
            }                          
            return false;
        }

        public async Task<string> GetSubscriptionProductHistoryJson(Guid subscriberProductHistoryId)
        {
           return _context.SubscriberProductHistory.FirstOrDefault(a => a.Subscriberproducthistoryid == subscriberProductHistoryId).Productdata;
        }

    }
}
