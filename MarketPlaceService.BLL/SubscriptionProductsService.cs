using System;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using MarketPlaceService.DAL.Contract;
using System.Net.Http;
using Newtonsoft.Json;
using MarketPlaceService.BLL.UtilityService;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Linq;
using MarketPlaceService.Utilities;
using Microsoft.Extensions.Configuration;
using CommonUtilities;

namespace MarketPlaceService.BLL
{
    public class SubscriptionProductsService : ISubscriptionProductsService
    {
       private readonly ILogger<SiteService> _logger;
       private readonly IAPIManagerService _apiManagerService;

       private readonly IMarketPlaceProductRepository _marketPlaceProductRepository;

       private readonly ISubscriptionProductsRepository _subscriptionProductRespository;

       private readonly ISubscriberRepository _subscriberRepository;

       private readonly IMappingJsonUtilityService _mappingJsonUtilityService;
       private readonly ICommonRepository _commonRepository;
        private readonly IConfiguration _configuration;
        private readonly ICommonService _commonService;
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
                _subscriptionProductRespository.UserId = value;
            }
        }
       public SubscriptionProductsService(ISubscriptionProductsRepository subscriptionProductRespository,ILogger<SiteService> logger,IAPIManagerService apiManagerService,IMarketPlaceProductRepository marketPlaceProductRepository,ISubscriberRepository subscriberRepository,IMappingJsonUtilityService mappingJsonUtilityService,ICommonRepository commonRepository,IConfiguration configuration, ICommonService commonService)
        {
             _logger = logger;
             _subscriptionProductRespository=subscriptionProductRespository;
             _apiManagerService=apiManagerService;
             _marketPlaceProductRepository=marketPlaceProductRepository;
             _subscriberRepository=subscriberRepository;
             _mappingJsonUtilityService = mappingJsonUtilityService;
             _commonRepository = commonRepository;
            _configuration = configuration;
            _commonService = commonService;
        }
        public async Task<SubscriptionProduct> SubscriptionProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "SubscriptionProduct", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            subscriptionProductDataModel.TraceId=TraceId;
            var result = await _subscriptionProductRespository.SubscriptionProduct(subscriptionProductDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "SubscriptionProduct", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "SubscriptionProduct", "SubscriptionProductsService", TraceId);
            return result;
        }

        public async Task<SubscriptionProduct> UpdateSubscriptionProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriptionProduct", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.UpdateSubscriptionProduct(subscriptionProductDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateSubscriptionProduct", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriptionProduct", "SubscriptionProductsService", TraceId);
            return result;
        }

        public async Task<SubscriptionProduct> InsertSubscriberProductHistory(QueuedSubscription queuedSubscriberDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriberProductHistory", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            queuedSubscriberDataModel.TraceId=TraceId;
            var result = await _subscriptionProductRespository.InsertSubscriberProductHistory(queuedSubscriberDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertSubscriberProductHistory", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriberProductHistory", "SubscriptionProductsService", TraceId);
            return result;
        }

        public async Task<IEnumerable<SubscriptionStatus>> GetSubscriptionStatus()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriptionStatus", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.GetSubscriptionStatus();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetSubscriptionStatus", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriptionStatus", "SubscriptionProductsService", TraceId);
            return result; 
        }

        public async Task<IEnumerable<MarketPlaceProduct>> SearchSubscriptionProduct(MarketPlaceProductsSearch marketPlaceProductSearchDataModel)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "SearchSubscriptionProduct", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.SearchSubscriptionProduct(marketPlaceProductSearchDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "SearchSubscriptionProduct", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "SearchSubscriptionProduct", "SubscriptionProductsService", TraceId);
            return result; 
        }

        public async Task<IEnumerable<QueuedSubscription>> GetQueuedSubscriptions(int limit, string serverName)
        {
            //used Logtrace as this method gets called every second by adapter so by default this wont be logged
            LoggingHelper.LogTrace(_logger, LogType.Start, "GetQueuedSubscriptions", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.GetQueuedSubscriptions(limit,serverName);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetQueuedSubscriptions", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogTrace(_logger, LogType.End, "GetQueuedSubscriptions", "SubscriptionProductsService", TraceId);
            return result; 
        }

        public async Task<QueuedSubscription> GetQueuedSubscriptionsById(Guid subScriberProductQueuedId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetQueuedSubscriptionsById", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.GetQueuedSubscriptionsById(subScriberProductQueuedId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetQueuedSubscriptionsById", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetQueuedSubscriptionsById", "SubscriptionProductsService", TraceId);
            return result;
        }

        public async Task DeleteSubscriptionProductQueue(Guid subScriberProductQueuedId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriptionProductQueue", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            await _subscriptionProductRespository.DeleteSubscriptionProductQueue(subScriberProductQueuedId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteSubscriptionProductQueue", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriptionProductQueue", "SubscriptionProductsService", TraceId);
        }

        public async Task<QueuedSubscription> UpdateSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriptionQueue", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result= await _subscriptionProductRespository.UpdateSubscriptionQueue(queuedSubscriptionDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdateSubscriptionQueue", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriptionQueue", "SubscriptionProductsService", TraceId);
            return result;
        }

        public async Task<QueuedSubscription> InsertSubscriptionQueue(QueuedSubscription queuedSubscriptionDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriptionQueue", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.InsertSubscriptionQueue(queuedSubscriptionDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertSubscriptionQueue", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriptionQueue", "SubscriptionProductsService", TraceId);
            return result;
        }

        public async Task<SubscriberProductDataResponse> ProcessSubscriberProduct(SubscriberProductDataRequest request)
        {
             SubscriberProductDataResponse response = new SubscriberProductDataResponse();
             MarketPlaceTsProductQueue marketPlaceTsProductItem=new MarketPlaceTsProductQueue();
             LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessSubscriberProduct", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = string.Empty;
            try
            {
                var productStatusId =  request.JobTypeId==3?5:4; 
                // var mandatoryParams = new List<APIParam>{
                //     new APIParam{Value= request.MarketPlaceProductId.ToString()}
                // };                
                //result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Products,"",mandatoryParams, null, EntityType.Publisher,request.SubscriberId);
                SiteDataModel site = _subscriptionProductRespository.GetSiteDetailsFromMarketplaceProduct(request.MarketPlaceProductId);
                request.SiteId = site.SiteId;
                request.SiteName = site.SiteName;               

                result =await _marketPlaceProductRepository.GetMarketPlaceProduct(request);
                //todo call mapping function 

                var subscriber = await _subscriberRepository.GetSubscriberById(request.SubscriberId);
                var messageTypeId = await GetMessageTypeId(request.SubscriberProductId);
                
                response =  ConvertJsonUsingMappedData(result, subscriber.SiteId, messageTypeId, request.SubscriberId, request.MarketPlaceProductId);   
                if(!response.IsSuccess)
                    return response;    
                  //  var fieldValues = GetFieldPathsWithValues(response.Data);
                var siteJson = response.Data;

                //check if product was already subscribed by subscriber
                //fetch json from existing product
                //calculate diff on json
                //save json

                var productVersion = 1;
                var productDataDifference = string.Empty;
                var existingSubscriptionProduct = GetExistingSubscriptionProduct(request.MarketPlaceProductId, request.SubscriberId);

                if (existingSubscriptionProduct != null)
                {
                    productVersion = existingSubscriptionProduct.ProductVersion + 1;
                    productDataDifference = _commonService.GetJsonDifference(existingSubscriptionProduct.ProductData, response.Data, JsonType.Product);
                }

                var SubscriberProductStatus = await _subscriptionProductRespository.GetSubscriberProductStatus();
                SubscriptionProduct SubscriberProductData = new SubscriptionProduct
                {
                    MarketPlaceProductId = request.MarketPlaceProductId,
                    ProductData = siteJson,
                    ProductStatusId = (short)productStatusId,//(short)SubscriberProductStatus.FirstOrDefault(a=> a.Name.ToLower().Equals("subscribed-pending confirmation")).Id,
                    SubscribeProductId = request.SubscriberProductId,
                    TraceId=TraceId,
                    ProductVersion = productVersion,
                    ProductUpdateDifferenceData = productDataDifference
                };
                response = await UpdateSubscriptionProducts(SubscriberProductData);
                if(!response.IsSuccess)
                    return response;  

                response = await PostSubscriptionToSite(request, subscriber, siteJson, productDataDifference);
                if(!response.IsSuccess)
                    return response;    

            }
            catch(Exception e)
            {
                response.IsSuccess=false;
                response.Errors.Add(new Error
                {
                    Type = ErrorType.FetchingProductData,
                    ErrorMessage = e.Message
                });                
            }
            
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetMarketPlaceProduct", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSubscriberProduct", "SubscriptionProductsService", TraceId);
            
            return response; 
        }

        

        private SubscriptionProduct GetExistingSubscriptionProduct(Guid productId, Guid publisherId)
        {
            return _subscriptionProductRespository.GetExistingSubscriptionProduct(productId, publisherId);
        }

        private async Task<SubscriberProductDataResponse> PostSubscriptionToSite(SubscriberProductDataRequest productDataRequest, SubscriberDataModel subscriber, string productData, string productDataDifference)
        {
            SubscriberProductDataResponse response = new SubscriberProductDataResponse{
                IsSuccess=true,
                Errors = new List<Error>()
            };

            MarketPlaceTsProductQueue tsRequest = new MarketPlaceTsProductQueue            
            {
                SubscriberProductId = productDataRequest.SubscriberProductId,
                MarketplaceProductId = productDataRequest.MarketPlaceProductId,
                OrganisationId = subscriber.OrganizationId,
                SubscriberId = subscriber.SubscriberId,
                ProductData = productData,
                ProductDefaults = await GetSubscriberDefaultsJson(subscriber.SubscriberId),
                SiteId= productDataRequest.SiteId,
                SiteName = productDataRequest.SiteName,
                MarketplaceApiUrl = _configuration["AppSettings:MarketplaceApiUrl"],
                ProductUpdateDifferenceData = productDataDifference,
                JobTypeId = productDataRequest.JobTypeId,
                ProductTypeId = await _commonRepository.GetProductTypeId(productDataRequest.MarketPlaceProductId)
            };
            try
            {              
                var result = await _apiManagerService.PostResponseAsync(tsRequest,TravelStudioControllers.QueuedSubscribedProducts,"",null, null, EntityType.Subscriber,subscriber.SubscriberId);                
            }
            catch(Exception e)
            {
                response.IsSuccess=false;
                response.Errors.Add(new Error
                {
                    Type = ErrorType.SubscriptionPost,
                    ErrorMessage = e.Message
                });
            }
            return response;
        }

        private async Task<string> GetSubscriberDefaultsJson(Guid subscriberId)
        {
            SubscriberDefaultsModelForSubscription response = new SubscriberDefaultsModelForSubscription();
            response.Defaults = await _subscriberRepository.GetContractDefaultSubscriberById(subscriberId);
            response.ChargingPolicy = await _subscriberRepository.GetDefaultSubscriberById(subscriberId);
            response.ProductCodeRules = await _subscriberRepository.GetSubscriberDefaultsProductCodes(subscriberId);
            response.Suppliers = await _subscriberRepository.GetSupplierMapSubscriberByIdAsync(subscriberId);
            response.SellingPrices = await _subscriberRepository.GetSubscriberDefaultSellPrices(subscriberId);
            return JsonConvert.SerializeObject(response);
        }


        private async Task<SubscriberProductDataResponse> UpdateSubscriptionProducts(SubscriptionProduct request)
        {
            SubscriberProductDataResponse response = new SubscriberProductDataResponse{
                IsSuccess=true,
                Errors = new List<Error>()
            };
            try
            {              
                var subscriberProductUpdated = await _subscriptionProductRespository.UpdateSubscriptionProduct(request);                
            }
            catch(Exception e)
            {
                response.IsSuccess=false;
                response.Errors.Add(new Error
                {
                    Type = ErrorType.SaveData,
                    ErrorMessage = e.Message
                });
            }
            return response;
        }

        private SubscriberProductDataResponse ConvertJsonUsingMappedData(string sourceJson,Guid siteId,int messageTypeId, Guid subscriberId, Guid marketplaceProductId)
        {
            SubscriberProductDataResponse response = new SubscriberProductDataResponse{
                IsSuccess=true,
                Errors = new List<Error>()
            };
            var mappingDirectionId = _commonRepository.GetMappingDirectionId(MappingDirection.MpToSite).Result;
            var productTypeId = _commonRepository.GetProductTypeId(marketplaceProductId).Result;
            try
            {
                var subscriberDefaults = _subscriptionProductRespository.GetSubscriberDefaults(subscriberId, marketplaceProductId);
                var result = _apiManagerService.GetResponseAsync(TravelStudioControllers.Regions, "GetRegions", null, null, EntityType.Site, siteId);
                var allRegions = JsonConvert.DeserializeObject<Response<IEnumerable<RegionData>>>(result.Result);
                
                ProcessJSONRequest processJsonMappingRequest = new ProcessJSONRequest
                {
                    SiteId = siteId,
                    MessageTypeId = messageTypeId,
                    JsonString = sourceJson,
                    MappingDirection = Convert.ToInt32(mappingDirectionId),
                    subscriberDefaults = subscriberDefaults,
                    AllRegions = allRegions.ResponseMessage.ToList(),
                    ProductTypeId = productTypeId
                };

                var processJsonMappingResponse = _mappingJsonUtilityService.ProcessJsonMapping(processJsonMappingRequest);
                if(processJsonMappingResponse.IsSuccess)
                {
                    response.Data = processJsonMappingResponse.JsonString;                
                }
                else
                {
                    response.Data = string.Empty;
                    response.IsSuccess = false;
                    response.Errors.Add(new Error
                    {
                        Type = processJsonMappingResponse.MappingError == MappingErrorType.Exception ? ErrorType.Unknown : ErrorType.DataMappingIntegrity,
                        ErrorMessage = processJsonMappingResponse.Description.Aggregate((i, j) => i + "," + j).ToString()
                    });
                }
            }
            catch(Exception e)
            {
                response.IsSuccess=false;
                response.Errors.Add(new Error
                {
                    Type = ErrorType.Unknown,
                    ErrorMessage = e.Message
                });
            }
            return response;
        }

        public async Task<int> GetMessageTypeId(Guid subscriberProductId)
        {
            _logger.LogInformation("Repository call for GetMessageTypeId started");
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.GetMessageTypeIdAsync(subscriberProductId);
            watch.Stop();
            _logger.LogInformation("Execution Time of GetMessageTypeId repository call is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }

        public async Task<SubscriptionProductInfo> GetSubscriptionProductInfo(Guid marketplaceProductId, Guid subscriberId)
        {
            SubscriptionProductInfo result = new SubscriptionProductInfo();
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriptionProductInfo", "SubscriptionProductsService", TraceId);
            var watch = Stopwatch.StartNew();
            result.PublishedBy = await _subscriptionProductRespository.GetPublishedByName(marketplaceProductId);
            result.FirstSubscriptionDate = await _subscriptionProductRespository.GetFirstSubscriptionDate(marketplaceProductId, subscriberId);
            result.SubscriptionHistoryDetails = await _subscriptionProductRespository.GetSubscriptionHistoryDetails(marketplaceProductId, subscriberId);
            result.SubscriptionJsonString = await _subscriptionProductRespository.GetSubscriptionJsonString(marketplaceProductId, subscriberId);
            result.MarketplaceJsonString = await _subscriptionProductRespository.GetMarketplaceJsonString(marketplaceProductId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishedByName", "SubscriptionProductRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriptionProductInfo", "SubscriptionProductsService", TraceId);
            return result;
        }

        public async Task<SubscriptionProduct> UnsubscribeProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            _logger.LogInformation("Repository call for UnsubscribeProduct started");
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.UnsubscribeProduct(subscriptionProductDataModel.SubscribeProductId, subscriptionProductDataModel.MarketPlaceProductId);
            watch.Stop();
            _logger.LogInformation("Execution Time of UnsubscribeProduct repository call is: {duration}ms", watch.ElapsedMilliseconds);
        
            return subscriptionProductDataModel;
        }        

        public async Task<string> GetSubscriptionProductHistoryJson(Guid subscriberProductHistoryId)
        {
            _logger.LogInformation("Repository call for GetSubscriptionProductHistoryJson started");
            var watch = Stopwatch.StartNew();
            var result = await _subscriptionProductRespository.GetSubscriptionProductHistoryJson(subscriberProductHistoryId);
            watch.Stop();
            _logger.LogInformation("Execution Time of GetSubscriptionProductHistoryJson repository call is: {duration}ms", watch.ElapsedMilliseconds);
        
            return result;
        }   

    }
}
