using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MarketPlaceService.BLL.UtilityService;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;
using MarketPlaceService.Utilities;
using CommonUtilities;
using Newtonsoft.Json.Serialization;
using System.Web;

namespace MarketPlaceService.BLL
{
    public class ProfileService : IProfileService
    {
        private readonly IPublisherRepository _publisherRepository;
        private readonly ICommonRepository _commonRepository;

        private readonly ILogger<ProfileService> _logger;
        private ISiteRepository _siteRepository;
        private readonly IAPIManagerService _apiManagerService;
        private readonly ICommonService _commonService;
        private readonly ISubscriptionProductsRepository _subscriptionProductRepository;
        readonly IMappingJsonUtilityService _mappingJsonUtilityService;

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
                _publisherRepository.UserId = value;
            }
        }
        public ProfileService(IPublisherRepository publisherRepository, ICommonRepository commonRepository, ISiteRepository siteRepository, IMappingJsonUtilityService mappingJsonUtilityService, ILogger<ProfileService> logger, IAPIManagerService apiManagerService, ICommonService commonService, ISubscriptionProductsRepository subscriptionProductRepository)
        {
            _publisherRepository = publisherRepository;   
            _commonRepository = commonRepository;         
            _logger = logger;
            _siteRepository = siteRepository;
            _mappingJsonUtilityService = mappingJsonUtilityService;
            _apiManagerService=apiManagerService;
            _commonService = commonService;
            _subscriptionProductRepository = subscriptionProductRepository;
        }
        public async Task<PublisherDataModel> AddNewPublisher(PublisherDataModel publisherItem)
        {
            try
            {
                LoggingHelper.LogInfo(_logger, LogType.Start, "AddNewPublisher", "ProfileService", TraceId);
                var watch = Stopwatch.StartNew();                
                var result = await _publisherRepository.AddNewPublisher(publisherItem);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "AddNewPublisher", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "AddNewPublisher", "ProfileService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeletePublisher(Guid id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeletePublisher", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.DeletePublisher(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeletePublisher", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeletePublisher", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublisherDataModel> GetPublisherById(Guid id)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherById", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublisherByIdAsync(id);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublisherById", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherById", "ProfileService", TraceId);
            return result;
        }

        public Task<IEnumerable<PublisherDataModel>> GetPublishersListAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PublisherDataModel>> GetPublishersListAsync()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishersListAsync", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublishersListAsync();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishersListAsync", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishersListAsync", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublisherDataModel> UpdatePublisher(PublisherDataModel publisherItem)
        {
            try
            {                
                LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublisher", "ProfileService", TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _publisherRepository.UpdatePublisher(publisherItem);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdatePublisher", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublisher", "ProfileService", TraceId);
                return result;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        //  public async Task<GeoTreeDataModel> GetGeoTree()
        // {
        //     _logger.LogInformation("Repository call for GetGeoTree started");
        //     var watch = Stopwatch.StartNew();
        //     var result = await _publisherRepository.GetGeoTree();
        //     watch.Stop();
        //     _logger.LogInformation("Execution Time of GetGeoTree repository call is: {duration}ms", watch.ElapsedMilliseconds);
        //     return result;
        // }

        // public async Task<IEnumerable<ServiceTypeDataModel>> GetServiceTypesAsync()
        // {
        //     _logger.LogInformation("Repository call for GetServiceTypes started");
        //     var watch = Stopwatch.StartNew();
        //     var result = await _publisherRepository.GetServiceTypesAsync();
        //     watch.Stop();
        //     _logger.LogInformation("Execution Time of Publisher repository call is: {duration}ms", watch.ElapsedMilliseconds);
        //     return result;
        // }

        // public async Task<IEnumerable<RegionDataModel>> GetLocationsAsync()
        // {
        //     _logger.LogInformation("Repository call for GetLocations started");
        //     var watch = Stopwatch.StartNew();
        //     var result = await _publisherRepository.GetLocationsAsync();
        //     watch.Stop();
        //     _logger.LogInformation("Execution Time of Publisher repository call is: {duration}ms", watch.ElapsedMilliseconds);
        //     return result;
        // }


        // Vinay
        public async Task<string> SearchProductAsync(int serviceTypeId, int RegionId, string productName, Guid publisherId)
        {
            string serviceStatusIds = "", supplierStatusIds = "";
            DateTime? contractStartDate=null; 
            //_logger.LogInformation("Repository call for GetLocations started");
            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchProductAsync", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var publisherDefaultTask =  GetPublisherDefaults(publisherId);
            var publisher = await GetPublisherById(publisherId);
            var url = await _commonRepository.GetSiteUrlBySiteId(publisher.SiteId);
            var publisherDefaults = await publisherDefaultTask;
            if(publisherDefaults!=null)
            {
                serviceStatusIds = string.Join(",",publisherDefaults.ServiceStatuses.ToArray());
                supplierStatusIds = string.Join(",",publisherDefaults.SupplierStatuses.ToArray());
                contractStartDate = publisherDefaults.ContractDate;
            }

            var optionalParams = new List<APIParam>
            {
                new APIParam { Name = "serviceTypeId", Value = serviceTypeId.ToString()},
                new APIParam {Name = "regionId", Value = RegionId.ToString()},                
                new APIParam{Name="organisationId", Value = publisher.OrganizationId.ToString()}
            };

            if(!string.IsNullOrEmpty(productName))
                optionalParams.Add(new APIParam{Name = "productName", Value = HttpUtility.UrlEncode(productName)});

            //var query = string.Format("{0}api/v1/controller/{1}", url, "SearchProduct" + "?serviceTypeId=" + serviceTypeId + "&regionId=" + RegionId + "&productName=" + productName + "&organisationId=" + publisher.OrganizationId);
            
            if(!string.IsNullOrEmpty(serviceStatusIds))
            {
                //query = query + "&serviceStatusId=" + serviceStatusIds;
                optionalParams.Add(new APIParam{ Name = "serviceStatusId", Value = serviceStatusIds});
            }
                

            if(!string.IsNullOrEmpty(supplierStatusIds))
            {
                //query = query + "&supplierStatusId=" + supplierStatusIds;
                optionalParams.Add(new APIParam{ Name = "supplierStatusId", Value = supplierStatusIds});
            }

            if(contractStartDate != null)
            {
                //query = query + "&contractStartDate=" + ((DateTime)contractStartDate).ToString("yyyy-MM-dd");
                optionalParams.Add(new APIParam{ Name = "contractStartDate", Value =  ((DateTime)contractStartDate).ToString("yyyy-MM-dd")});
            }

            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Products,"SearchProduct", null, optionalParams, EntityType.Publisher, publisherId);

            watch.Stop();
            //_logger.LogInformation("Execution Time of Publisher repository call is: {duration}ms", watch.ElapsedMilliseconds);
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "ApIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "SearchProductAsync", "ProfileService", TraceId);
            return result;
        }


        // public async Task<IEnumerable<PublishedProductsDataModel>> SearchUnpublishedProductAsync()
        // {
        //     _logger.LogInformation("Repository call for GetLocations started");
        //     var watch = Stopwatch.StartNew();
        //     var result = await _publisherRepository.SearchUnpublishedProductAsync();
        //     watch.Stop();
        //     _logger.LogInformation("Execution Time of Publisher repository call is: {duration}ms", watch.ElapsedMilliseconds);
        //     return result;
        // }

        public async Task<PublishedProductsDataModel> PublishProduct(PublishedProductsDataModel publishedProductsDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "PublishProduct", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.PublishProduct(publishedProductsDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "PublishProduct", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "PublishProduct", "ProfileService", TraceId);
            return result;
        }

        public async Task<IEnumerable<QueuedPublication>> GetQueuedPublications(int limit, string serverName)
        {
            //used Logtrace as this method gets called every second by adapter so by default this wont be logged
            LoggingHelper.LogTrace(_logger, LogType.Start, "GetQueuedPublications", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetQueuedPublications(limit,serverName);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetQueuedPublications", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogTrace(_logger, LogType.End, "GetQueuedPublications", "ProfileService", TraceId);
            return result;
        }

        public async Task<QueuedPublication> UpdatePublicationQueue(QueuedPublication queuedPublicationDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublicationQueue", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
           var result = await _publisherRepository.UpdatePublicationQueue(queuedPublicationDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdatePublicationQueue", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublicationQueue", "ProfileService", TraceId);
            return result;
        }

        public async Task<IEnumerable<PublishedStatus>> GetPublishStatus()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishStatus", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
           var result = await _publisherRepository.GetPublishStatus();
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishStatus", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishStatus", "ProfileService", TraceId);
            return result;
        }

        //public async Task<IEnumerable<ProcessingStatus>> GetProcessingStatus()
        //{
        //     LoggingHelper.LogInfo(_logger, LogType.Start, "GetProcessingStatus", "ProfileService", TraceId);
        //    var watch = Stopwatch.StartNew();
        //   var result = await _publisherRepository.GetProcessingStatus();
        //    watch.Stop();
        //    LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetProcessingStatus", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
        //    LoggingHelper.LogInfo(_logger, LogType.End, "GetProcessingStatus", "ProfileService", TraceId);
        //    return result;
        //}

         public async Task<PublishedProductsDataModel> UpdatePublishedProduct(PublishedProductsDataModel publishedProductsDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublishedProduct", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.UpdatePublishedProduct(publishedProductsDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdatePublishedProduct", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublishedProduct", "ProfileService", TraceId);
            return result;
        }

        public async Task<QueuedPublication> InsertPublicationHistory(QueuedPublication queuedPublicationDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertPublicationHistory", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            queuedPublicationDataModel.TraceId=TraceId;
            var result = await _publisherRepository.InsertPublicationHistory(queuedPublicationDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertPublicationHistory", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertPublicationHistory", "ProfileService", TraceId);
            return result;
        }

        public async Task<QueuedPublication> InsertPublicationQueue(QueuedPublication queuedPublicationDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertPublicationQueue", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            queuedPublicationDataModel.TraceId = TraceId;
            var result = await _publisherRepository.InsertPublicationQueue(queuedPublicationDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertPublicationQueue", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertPublicationQueue", "ProfileService", TraceId);
            return result;
        }

        public async Task DeletePublishedProductQueue(Guid publishedProductQueueId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeletePublishedProductQueue", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            await _publisherRepository.DeletePublishedProductQueue(publishedProductQueueId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeletePublishedProductQueue", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeletePublishedProductQueue", "ProfileService", TraceId);
        }        

        public async Task<string> GetPublishedProductDataString(Guid publishedProductId)
        {
             _logger.LogInformation("Repository call for GetPublishedProductDataString started");
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublishedProductDataString(publishedProductId);
            watch.Stop();
            _logger.LogInformation("Execution Time of Publisher repository call for GetPublishedProductDataString is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }

       
        public async Task<PublishedProductInfo> GetPublishedProductInfo(Guid publishedProductId)
        {
            PublishedProductInfo result = new PublishedProductInfo();
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishedProductInfo", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            result.FirstPublicationDate = await _publisherRepository.GetFirstPublicationDate(publishedProductId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetFirstPublicationDate", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            watch.Restart();
            result.PublicationHistoryDetails = await _publisherRepository.GetPublicationHistoryDetails(publishedProductId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublicationHistoryDetails", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            watch.Restart();
            result.CurrentSubscribers = await _publisherRepository.GetCurrentSubscribers(publishedProductId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetCurrentSubscribers", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            watch.Restart();
            result.SiteJsonString = await _publisherRepository.GetPublishedProductDataString(publishedProductId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishedProductDataString", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            watch.Restart();
            result.MarketplaceJsonString = await _publisherRepository.GetPublishedProductDataMarketplaceString(publishedProductId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishedProductDataMarketplaceString", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishedProductInfo", "ProfileService", TraceId);
            return result;
        }
        

        public async Task<GetPublishProductResponse> GetPublishProduct(int productId, Guid publisherId)
        {
            _logger.LogInformation("Repository call for GetLocations started");
            var watch = Stopwatch.StartNew();
            var publisher = GetPublisherById(publisherId).Result;
            var result = await _publisherRepository.GetPublishProduct(productId, publisherId);
            watch.Stop();
            _logger.LogInformation("Execution Time of Publisher repository call is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }

        public async Task<IEnumerable<PublisherAgentMap>> GetPublisherAgentMaps(Guid publisherId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherAgentMaps", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublisherAgentMaps(publisherId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublisherAgentMaps", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherAgentMaps", "ProfileService", TraceId);
            return result;
        }       

        public async Task<PublisherAgentMap> GetPublisherAgentMaps(Guid publisherId, Guid subscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherAgentMaps", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublisherAgentMaps(publisherId,subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublisherAgentMaps", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherAgentMaps", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublisherAgentMap> InsertPublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertPublisherAgentMaps", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.InsertPublisherAgentMaps(request, publisherId,subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertPublisherAgentMaps", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertPublisherAgentMaps", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublisherAgentMap> UpdatePublisherAgentMaps(PublisherAgentMap request, Guid publisherId, Guid subscriberId)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublisherAgentMaps", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.UpdatePublisherAgentMaps(request, publisherId,subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UpdatePublisherAgentMaps", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublisherAgentMaps", "ProfileService", TraceId);
            return result;
        }

        public async Task<bool> DeletePublisherAgentMaps(Guid publisherId, Guid subscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeletePublisherAgentMaps", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.DeletePublisherAgentMaps(publisherId,subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeletePublisherAgentMaps", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeletePublisherAgentMaps", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublisherDefaultDataModel> GetPublisherDefaults(Guid publisherId)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherDefaults", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublisherDefaults(publisherId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublisherDefaults", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherDefaults", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublisherDefaultDataModel> InsertUpdatePublisherDefaults(Guid publisherId, PublisherDefaultDataModel request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertUpdatePublisherDefaults", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.InsertUpdatePublisherDefaults(publisherId, request);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertUpdatePublisherDefaults", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertUpdatePublisherDefaults", "ProfileService", TraceId);
            return result;
        }

        public async Task<IEnumerable<PublishedProductsDataModel>> GetPublishedUnpublishedProducts(Guid publisherId, int productType, IEnumerable<ServiceDataModel> products, int? publishedStatus, IEnumerable<PackageDataModel> packages, ProductType productTypeId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishedUnpublishedProducts", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublishedUnpublishedProducts(publisherId, productType, products, publishedStatus, packages, productTypeId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishedUnpublishedProducts", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishedUnpublishedProducts", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublishedProductDataResponse> ProcessPublishedProduct(ProductDataRequest request)
        {
            PublishedProductDataResponse response = new PublishedProductDataResponse();
            LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessPublishedProduct", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = string.Empty;
            var searchMethod = string.Empty;
            try
            {
                var mandatoryParams = new List<APIParam>{
                    new APIParam{Value= request.ProductId.ToString()}
                };    
                
                if(request.ProductType == ProductType.Package)
                {
                    searchMethod = "PackageInfo";
                }

                result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Products, searchMethod, mandatoryParams, null, EntityType.Publisher,request.PublisherId);

                var publisher = await GetPublisherById(request.PublisherId);
                var messageTypeId = await GetMessageTypeId(request.PublishedProductId);

                response =  ConvertJsonUsingMappedData(result, publisher.SiteId, messageTypeId);                              
                if(!response.IsSuccess)
                    return response;

                if (request.ProductType == ProductType.Package)
                {
                    response.Data = GetProductDataFromPackageInfo(JsonConvert.DeserializeObject<PackageData>(response.Data), publisher.SiteId);
                }

                var convertedJson = response.Data;    
                var fieldValues = GetFieldPathsWithValues(convertedJson);

                var productVersion = 1;
                var productDataDifference = string.Empty;
                var existingPublishedProduct = GetExistingPublishedProduct(request.ProductId, request.PublisherId, request.ProductType);
                
                if(existingPublishedProduct !=null)
                {
                    productVersion = existingPublishedProduct.ProductVersion + 1;
                    productDataDifference = _commonService.GetJsonDifference(existingPublishedProduct.ProductData, result, JsonType.Product);
                }
                
                var publisherProductStatus = await _publisherRepository.GetPublisherProductStatus(); //Modify to pass statusname and get statusid
                PublishedProductsDataModel publishedProductData = new PublishedProductsDataModel
                {
                    PublishedProductId = request.PublishedProductId,
                    ProductData = result,
                    PublisherProductStatusId = (short)publisherProductStatus.FirstOrDefault(a=> a.Name.ToLower().Equals("published")).Id,
                    TraceId=TraceId,
                    ProductVersion = productVersion,
                    ProductUpdateDifferenceData = productDataDifference
                };
                response = await UpdatePublishedProductData(publishedProductData);
                if(!response.IsSuccess)
                    return response;
                var camelCaseFormatter = new JsonSerializerSettings();
                camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ProductData p = JsonConvert.DeserializeObject<ProductData>(convertedJson); 
                
                if (request.ProductType != ProductType.Package)
                {
                    p.ImportSupplierAddress  = await _publisherRepository.IsImportSupplierAddress(p.ServiceTypeId);
                }
               
                convertedJson = JsonConvert.SerializeObject(p,camelCaseFormatter) ;

                var messageTypes = await _publisherRepository.GetMessageTypes();
                var serviceMessageTypeId = messageTypes.FirstOrDefault(q => q.Name.ToLower().Equals("serviceproductjson")).Id;

                response = await InsertUpdateMarketPlaceProductData(fieldValues, request.PublishedProductId, convertedJson, serviceMessageTypeId); //passing MessageType as service since regardless of what the product is, on the marketplace we are storing it as a service.
                if(!response.IsSuccess)
                    return response;
                var marketplaceProductId = response.MarketplaceProductId;
                
                if(existingPublishedProduct!=null) //if product existed, then insert a subscription queue record for each subscriber who subscribed to the product
                {
                    var subscriberIds = GetSubscriberIdsForSubscribedProduct(marketplaceProductId);

                    if(subscriberIds!=null && subscriberIds.Any())
                    {
                        InsertSubscriptionQueueRecordForSubscribers(marketplaceProductId, subscriberIds, (short)request.JobTypeId);
                    }                    
                }                              
            }
            catch(Exception e)
            {
                response.IsSuccess=false;
                response.Errors.Add(new Error
                {
                    Type = ErrorType.FetchingProductData,
                    ErrorMessage = e.ToString()
                });                
            }
            
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "ProcessPublishedProduct", "ProfileService", TraceId);
            
            return response; 
        }

        private string GetProductDataFromPackageInfo(PackageData package, Guid siteId)
        {

            foreach (var item in package.PackageItinerary)
           { 
               if(item.PackageDayItinerary.Count()>1)
               {
                   foreach (var itemPackageItem in item.PackageDayItinerary)
                   {
                        ItineraryText Itineraryitem=new ItineraryText();
                        Itineraryitem.Id = itemPackageItem.Id;
                        Itineraryitem.ItineraryTypeId = item.ItineraryTypeId;
                        Itineraryitem.ItineraryTypeName = item.ItineraryTypeName;                  
                        Itineraryitem.LanguageId = item.LanguageId;
                        Itineraryitem.LanguageName = item.LanguageName;
                        Itineraryitem.Name =itemPackageItem.SequentialId.ToString() ?? string.Empty;
                        Itineraryitem.MultiDay = true;
                        Itineraryitem.Dates = item.Dates;
                        Itineraryitem.Text = itemPackageItem.text ?? string.Empty;
                        Itineraryitem.AppliedType = item.AppliedType;
                        if(package.ItineraryText==null)
                        package.ItineraryText = new List<ItineraryText>(); 
                        package.ItineraryText.Add(Itineraryitem);      
                   }                 
               }
               else
               {
                  ItineraryText Itineraryitem=new ItineraryText();
                  Itineraryitem.Id = item.PackageDayItinerary.FirstOrDefault().Id;
                  Itineraryitem.ItineraryTypeId = item.ItineraryTypeId;
                  Itineraryitem.ItineraryTypeName = item.ItineraryTypeName;                  
                  Itineraryitem.LanguageId = item.LanguageId;
                  Itineraryitem.LanguageName = item.LanguageName;
                  Itineraryitem.Name = item.PackageDayItinerary.FirstOrDefault().Description ?? string.Empty;
                  Itineraryitem.MultiDay = false;
                  Itineraryitem.Dates = item.Dates;
                  Itineraryitem.Text = item.PackageDayItinerary.FirstOrDefault().text ?? string.Empty;
                  Itineraryitem.AppliedType = item.AppliedType;
                  if(package.ItineraryText==null)
                  package.ItineraryText = new List<ItineraryText>(); 
                  package.ItineraryText.Add(Itineraryitem);
               }               
           }

            ProductData productData = new ProductData
            {
               
                ServiceId = package.PackageId,
                Description = package.Description,
                CurrencyISO = package.CurrencyIsoCode,
                LongName = package.LongName,
                DisplayName = package.DisplayName,
                ShortName = package.ShortName,
                SearchPriority = package.SearchPriority,
                TermsExclusion = package.TermsExclusion,
                TermsInclusion = package.TermsInclusion,
                TimeOfService = package.TimeOfService,
                ServiceDuration = package.ServiceDuration,
                RegionId = package.RegionId,
                RegionName = package.RegionName!=null? package.RegionName.Trim():"",
                //PickupOptionId = package.PickupOptionId,
                //DropoffOptionId = package.DropoffOptionId,
                Facilities = package.Facilities,
                TermList = package.TermList,
                Notes = package.Notes,
                AdditionalRegions = package.AdditionalRegions,

                ApplicableRatings  = package.ApplicableRatings,
                Supplier = new Supplier{
                    Id = 0,
                    Name = ""
                },
                ImportSupplierAddress = true,
                Options = package.Elements.Select(e=> new OptionData
                {
                    Id = e.Id,
                    Name = e.Name,
                    Active = e.Active,
                    Type = e.Type,
                    //Status = e.Status
                }).ToList(),
                Extras = package.Optionals.Select(o=> new ExtraData
                {
                    Id = o.Id,
                    Name = string.Format("{0}-{1}", o.Name.Trim(), o.ServiceName.Trim()).Truncate(50),
                    IsOption = o.IsOptionalOption,
                    Type = o.Type,
                    Mandatory = o.Mandatory
                    //Status =  o.Status
                }).ToList(),        
                ItineraryText = package.ItineraryText       
            };

            foreach(ExtraData e in productData.Extras)
            {
              e.Type.Name = e.Name;
            }
            productData.Status = _publisherRepository.GetPackageToServiceMapForType(siteId, 16, package.PackageStatusId);
            productData.ServiceTypeId = _publisherRepository.GetPackageToServiceMapForType(siteId, 15, package.PackageTypeId);
            productData.ServiceTypeName = _publisherRepository.GetMasterDataName(productData.ServiceTypeId);
            productData.StatusName  =  _publisherRepository.GetMasterDataName(productData.Status);
                       
            return JsonConvert.SerializeObject(productData, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private List<Guid> GetSubscriberIdsForSubscribedProduct(Guid marketplaceProductId)
        {
            return _subscriptionProductRepository.GetSubscriberIdsForSubscribedProduct(marketplaceProductId);
        }

        private void InsertSubscriptionQueueRecordForSubscribers(Guid marketplaceProductId, List<Guid> subscriberIds, short jobTypeId)
        {
            foreach(var subscriberId in subscriberIds)
            {
                QueuedSubscription queuedSubscription = new QueuedSubscription
                {
                    MarketplaceProductId = marketplaceProductId,
                    SubscriberId = subscriberId,
                    JobTypeId = jobTypeId
                };
                _subscriptionProductRepository.InsertSubscriptionQueue(queuedSubscription);
            }
        }


        private PublishedProductsDataModel GetExistingPublishedProduct(int productId, Guid publisherId, Entities.ProductType productType)
        {
            return _publisherRepository.GetExistingPublishedProduct(productId, publisherId, productType);
        }

        private async Task<PublishedProductDataResponse> UpdatePublishedProductData(PublishedProductsDataModel request)
        {
            PublishedProductDataResponse response = new PublishedProductDataResponse{
                IsSuccess = true,
                Errors = new List<Error>()
            };
            try
            {              
                var publishProductUpdated = await _publisherRepository.UpdatePublishedProduct(request);                
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

        private PublishedProductDataResponse ConvertJsonUsingMappedData(string sourceJson, Guid siteId, int messageTypeId)
        {
            PublishedProductDataResponse response = new PublishedProductDataResponse{
                IsSuccess=true,
                Errors = new List<Error>()
            };           

            try
            {
                var mappingDirectionId = _commonRepository.GetMappingDirectionId(MappingDirection.SiteToMp).Result;

                ProcessJSONRequest processJsonMappingRequest = new ProcessJSONRequest();
                processJsonMappingRequest.SiteId = siteId;
                processJsonMappingRequest.MessageTypeId = messageTypeId;
                processJsonMappingRequest.JsonString = sourceJson;
                processJsonMappingRequest.MappingDirection = Convert.ToInt32(mappingDirectionId);

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

        private async Task<PublishedProductDataResponse> InsertUpdateMarketPlaceProductData(IEnumerable<RetriveFieldPathResponse> fields, Guid publishedProductId, string json, int messageTypeId)
        {
            PublishedProductDataResponse response = new PublishedProductDataResponse{
                IsSuccess=true,
                Errors = new List<Error>()
            };
            try
            {
                _logger.LogInformation("Repository call for InsertMarketPlaceProductData started");
                var watch = Stopwatch.StartNew();
                var result = await _publisherRepository.InsertUpdateMarketPlaceProductData(fields, publishedProductId, json, messageTypeId);
                response.MarketplaceProductId = result.MarketplaceProductId;
                watch.Stop();
                _logger.LogInformation("Execution Time of InsertMarketPlaceProductData repository call is: {duration}ms", watch.ElapsedMilliseconds);
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

        public async Task<int> GetMessageTypeId(Guid publishedProductId)
        {
            _logger.LogInformation("Repository call for GetMessageTypeId started");
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetMessageTypeIdAsync(publishedProductId);
            watch.Stop();
            _logger.LogInformation("Execution Time of GetMessageTypeId repository call is: {duration}ms", watch.ElapsedMilliseconds);
            return result;
        }

        private  IEnumerable<RetriveFieldPathResponse> GetFieldPathsWithValues(string jsonString)
        {
            List<string> fieldPathList = new List<string>();
            fieldPathList = GetFieldPaths();
            
            var fieldValues =  _mappingJsonUtilityService.RetrieveValue(fieldPathList, jsonString);
            return fieldValues;
        }

        private List<string> GetFieldPaths()
        {
            List<string> fieldPathList = new List<string>();
            fieldPathList.Add(DataMappingConstants.serviceTypeId);
            fieldPathList.Add(DataMappingConstants.serviceLongName);
            fieldPathList.Add(DataMappingConstants.serviceShortName);
            fieldPathList.Add(DataMappingConstants.regionId);
            fieldPathList.Add(DataMappingConstants.ratings);
            
            return fieldPathList;
        }

        public async Task<IEnumerable<PublishedProductAllowedSubscriber>> GetAllowedSubscribers(List<Guid> productIds)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllowedSubscribers", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetAllowedSubscribers(productIds);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetAllowedSubscribers", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetAllowedSubscribers", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublishedProductAllowedSubscriber> AllowedSubscriber(Guid publishedproductId,Guid subscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "AllowedSubscriber", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.AllowedSubscriber(publishedproductId,subscriberId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "AllowedSubscriber", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "AllowedSubscriber", "ProfileService", TraceId);
            return result;
        }

        public async Task<bool> DeleteAllowedSubscriber(Guid publishedProductId,Guid subscriberId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteAllowedSubscriber", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.DeleteAllowedSubscriber(publishedProductId,subscriberId, 1); // 1-Access removed
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "DeleteAllowedSubscriber", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "DeleteAllowedSubscriber", "ProfileService", TraceId);
            return result;
        }

        public async Task<IEnumerable<ManageSubscribersResponse>> SearchManageSubscribers(ManageSubscribersSearch manageSubscribersDataModel)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchManageSubscribers", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.SearchManageSubscribers(manageSubscribersDataModel);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "SearchManageSubscribers", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "SearchManageSubscribers", "ProfileService", TraceId);
            return result;
        }
        public async Task<QueuedPublication> InsertStaticDataUpdateQueue(StaticDataUpdateQueueRequest insertStaticDataUpdateQueueRequest)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertStaticDataUpdateQueue", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.InsertStaticDataUpdateQueue(insertStaticDataUpdateQueueRequest);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "InsertStaticDataUpdateQueue", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "InsertStaticDataUpdateQueue", "ProfileService", TraceId);
            return new QueuedPublication();
        }

        public async Task<PublishedProductsDataModel> UnpublishProduct(PublishedProductsDataModel publishedProductsDataModel)
        {
            /*
                1. update the status in published_products to unpublished
                2. find all the subcribers
                3. loop through the subscribers and call deleteAllowedSubscriber
                4. also insert into subscriber_product_history 
                5. insert into published_product_history
            */
            LoggingHelper.LogInfo(_logger, LogType.Start, "UnpublishProduct", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.UpdatePublishedProductStatus(publishedProductsDataModel.PublishedProductId, "unpublished");
            if(result)
            {
                var subscriberList = await _publisherRepository.GetPublishedProductSubscriberList(publishedProductsDataModel.PublishedProductId);
                foreach(var subscriber in subscriberList)
                {
                    await _publisherRepository.DeleteAllowedSubscriber(subscriber.PublisherProductId,subscriber.SubscriberId, 2); // 2-Unpublish
                }
                await _publisherRepository.InsertPublicationHistoryForUnpublishProduct(publishedProductsDataModel.PublishedProductId, null, 1); // 1- Unpublish

                
                publishedProductsDataModel.PublisherProductStatusId = 4;
                publishedProductsDataModel.ProcessingStatusName = "Unpublished";
            }
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "UnpublishProduct", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "UnpublishProduct", "ProfileService", TraceId);

            return publishedProductsDataModel;
        }

        public async Task<List<PublishedStatus>> GetPublishedStatus()
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishedStatus", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublishedStatus();
            if(result != null)
            {
                result.Add(new Entities.PublishedStatus{
                    StatusId = 5,
                    StatusName = "No"
                });
            }
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishedStatus", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishedStatus", "ProfileService", TraceId);
            return result;
        }

        public async Task<string> GetPublishedProductHistoryJsonData(Guid publishedProductHistoryId)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishedProductHistoryJsonData", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublishedProductHistoryJsonData(publishedProductHistoryId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublishedProductHistoryJsonData", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishedProductHistoryJsonData", "ProfileService", TraceId);
            return result;
        }

        public async Task<string> SearchPackageProductAsync(int packageTypeId, int regionId, string productName, Guid publisherId)
        {
            string packageStatusIds = "";
            DateTime? packageContractStartDate=null; 
            
            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchPackageProductAsync", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var publisherDefaultTask =  GetPublisherDefaultsForPackage(publisherId);
            var publisher = await GetPublisherById(publisherId);
            var url = await _commonRepository.GetSiteUrlBySiteId(publisher.SiteId);
            var publisherDefaults = await publisherDefaultTask;
            if(publisherDefaults!=null)
            {
                packageStatusIds = string.Join(",",publisherDefaults.PackageStatuses.ToArray());
                packageContractStartDate = publisherDefaults.PackagePriceStartDate;
            }

            var optionalParams = new List<APIParam>
            {
                new APIParam {Name = "packageTypeId", Value = packageTypeId.ToString()},
                new APIParam {Name = "regionId", Value = regionId.ToString()},                
                new APIParam {Name = "organisationId", Value = publisher.OrganizationId.ToString()}
            };

            if(!string.IsNullOrEmpty(productName))
                optionalParams.Add(new APIParam{Name = "productName", Value = productName});
            
            if(!string.IsNullOrEmpty(packageStatusIds))
            {
                optionalParams.Add(new APIParam{ Name = "packageStatusId", Value = packageStatusIds});
            }

            if(packageContractStartDate != null)
            {
                optionalParams.Add(new APIParam{ Name = "packageContractStartDate", Value =  ((DateTime)packageContractStartDate).ToString("yyyy-MM-dd")});
            }

            var result = await _apiManagerService.GetResponseAsync(TravelStudioControllers.Products,"SearchPackage", null, optionalParams, EntityType.Publisher, publisherId);

            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetResponseAsync", "ApIManagerService", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "SearchPackageProductAsync", "ProfileService", TraceId);
            return result;
        }

        public async Task<PublisherDefaultDataModel> GetPublisherDefaultsForPackage(Guid publisherId)
        {
             LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherDefaultsForPackage", "ProfileService", TraceId);
            var watch = Stopwatch.StartNew();
            var result = await _publisherRepository.GetPublisherDefaultsForPackage(publisherId);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Repo, "GetPublisherDefaultsForPackage", "PublisherRepository", TraceId, watch.ElapsedMilliseconds);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherDefaultsForPackage", "ProfileService", TraceId);
            return result;
        }

    }
}
