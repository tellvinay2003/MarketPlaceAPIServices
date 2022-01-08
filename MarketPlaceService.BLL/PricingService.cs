using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.UtilityService;
using MarketPlaceService.DAL.Contract;
using MarketPlaceService.Entities.TSv2ApiEntities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MarketPlaceService.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MarketPlaceService.BLL
{
    public class PricingService : IPricingService
    {
        private readonly IPricingRepository _pricingRepository;
        private readonly ILogger<PricingService> _logger;
        private readonly IAPIManagerService _apiManagerService;
        private readonly ITravelStudioService _travelStudioService;
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

        public PricingService(IPricingRepository pricingRepository, ILogger<PricingService> logger, IAPIManagerService apiManagerService, ITravelStudioService travelStudioService)
        {            
            _logger = logger;
            _pricingRepository = pricingRepository;
            _apiManagerService = apiManagerService;
            _travelStudioService = travelStudioService;
        }
        public async Task<GetServicePricesResponse> GetServicePricesFromTs(GetServicePricesRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServicePricesFromTs", "PricingService", TraceId);

            //TODO null check
            var subscriberId = await GetSubscriberId(request.SiteId, request.OrganisationId);
           
            var mpTsRequest = CheckIfHasAccess(request, subscriberId);
            
            List<SiteServiceData> siteServiceData = _pricingRepository.GetSiteServiceData(request.ArrayOfUIPricesAndAvailabilityRequest.Select(a => a.MarketplaceProductId).ToList(), subscriberId);
            mpTsRequest = UpdateGetServicePricesTsRequest(mpTsRequest, siteServiceData);

            var uniqueSiteIds =  mpTsRequest.ArrayOfUIPricesAndAvailabilityRequest.Select(e => e.SiteId).Distinct().ToList();
            List<Task<string>> tasks = new List<Task<string>>();
             List<Tuple<Guid, int, Task<string>>> taskList = new List<Tuple<Guid,int, Task<string>>>();

             var watch = Stopwatch.StartNew();
            foreach(var siteId in uniqueSiteIds)
            {
               var singleSiteRequest = GetSingleRequestToPublisher(mpTsRequest);
               var services = mpTsRequest.ArrayOfUIPricesAndAvailabilityRequest.Where(e => e.SiteId == siteId && e.HasAccess).Select(q=>q).ToArray();
               var agentIds = services.Select(a => a.ClientId).Distinct().ToList();
                foreach (var agent in agentIds)
                {
                    singleSiteRequest.ArrayOfUIPricesAndAvailabilityRequest = mpTsRequest.ArrayOfUIPricesAndAvailabilityRequest.Where(e => e.SiteId == siteId && e.HasAccess).Select(q=>q).ToArray();
                    singleSiteRequest.CLIENTID = siteServiceData.FirstOrDefault(x=>x.MarketplaceProductId == singleSiteRequest.ArrayOfUIPricesAndAvailabilityRequest.First().MarketplaceProductId).AgentId;     
                            
                    var task =  _apiManagerService.PostResponseAsync(singleSiteRequest, TravelStudioControllers.Pricing, "GetServicePricesFromTs", null, null, 0, siteId);
                    tasks.Add(task);
                    taskList.Add(new Tuple<Guid, int, Task<string>>(siteId, agent, task));
                }
                //TODO make call    organisation checking left as informed by Shani on 14 Oct
            }
             Task.WaitAll(tasks.ToArray());

            Response<GetServicePricesResponse> response = new Response<GetServicePricesResponse>
            {
                Status = "success",
                ResponseMessage = new GetServicePricesResponse
                {
                    Services = new List<PriceAndAvailabilityService>()
                },
                TraceId = TraceId
            };
          
            foreach(var taskItem in taskList)
            {
                var task = taskItem.Item3;
                try
                {
                    var result = JsonConvert.DeserializeObject<Response<GetServicePricesResponse>>(task.Result);

                    if (result == null || result.ResponseMessage == null || result.ResponseMessage.Services == null )
                        continue;

                    UpdateSubscriberDataForGetServicePrices(result.ResponseMessage, siteServiceData, taskItem.Item1);
                    response.ResponseMessage.Services.AddRange(result.ResponseMessage.Services);
                      
                }
                catch(Exception e)
                {
                    LoggingHelper.LogError(_logger, ExceptionType.System, "SplitAndPostRequestToPublishersForGetServicePricesFromTs", "PricingService", TraceId, new Exception($"Deserialisation of GetBookingPriceResponse From Publisher failed. Response = {task.Result}"));
                }                
            }

            //convert service ids
           // UpdateSubscriberDataForGetServicePrices(response.ResponseMessage,  siteServiceData);
            
            //var result = await _apiManagerService.PostResponseAsync(mpTsRequest, TravelStudioControllers.Pricing, "GetServicePricesFromTs", null, null, 0, mpTsRequest.ArrayOfUIPricesAndAvailabilityRequest.First().SiteId); 
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServicePricesFromTs", "APIManager", TraceId, watch.ElapsedMilliseconds);
            //var response = JsonConvert.DeserializeObject<Response<GetServicePricesResponse>>(result);
            LoggingHelper.LogInfo(_logger, LogType.End, "GetServicePricesFromTs", "PricingService", TraceId);
            
            return response.ResponseMessage;
           // return new GetServicePricesResponse();
        }

        private GetServicePricesRequest GetSingleRequestToPublisher(GetServicePricesRequest mpTsRequest)
        {
            var requestFormed = new GetServicePricesRequest();

            requestFormed.Authenticate = mpTsRequest.Authenticate;
            requestFormed.BestSeller = mpTsRequest.BestSeller;
            requestFormed.BestSellerSpecified = mpTsRequest.BestSellerSpecified;
            requestFormed.BOOKING_TYPE_ID = mpTsRequest.BOOKING_TYPE_ID;
            requestFormed.BookingRefNo = mpTsRequest.BookingRefNo;
            requestFormed.CalculateCommisson = mpTsRequest.CalculateCommisson;
            requestFormed.CalculateGSTTax = mpTsRequest.CalculateGSTTax;
            requestFormed.CalledFromFIT = mpTsRequest.CalledFromFIT;
            requestFormed.CLIENTID = mpTsRequest.CLIENTID;
            requestFormed.CountryCodeForPassengerTax = mpTsRequest.CountryCodeForPassengerTax;
            requestFormed.EstimatedServiceRatesInfo = mpTsRequest.EstimatedServiceRatesInfo;
            requestFormed.EstimatedServiceRatesInfo = mpTsRequest.EstimatedServiceRatesInfo;
            requestFormed.GEO_LOCATION_ID = mpTsRequest.GEO_LOCATION_ID;
            requestFormed.GEO_LOCATION_NAME = mpTsRequest.GEO_LOCATION_NAME;
            requestFormed.GEO_LOCATION_TREE_REQUIRED = mpTsRequest.GEO_LOCATION_TREE_REQUIRED;
            requestFormed.IMAGENOTREQUIRED = mpTsRequest.IMAGENOTREQUIRED;
            requestFormed.IsEstimatedService = mpTsRequest.IsEstimatedService;
            requestFormed.IsLinkedService = mpTsRequest.IsLinkedService;
            requestFormed.IsMeanPlanDetailsRequired = mpTsRequest.IsMeanPlanDetailsRequired;
            requestFormed.IsPackageService = mpTsRequest.IsPackageService;
            requestFormed.IsServiceOptionDescriptionRequired = mpTsRequest.IsServiceOptionDescriptionRequired;
            requestFormed.LeadPassengerCountryID = mpTsRequest.LeadPassengerCountryID;
            requestFormed.Modifiers = mpTsRequest.Modifiers;
            requestFormed.NationalityID = mpTsRequest.NationalityID;
            requestFormed.NotesRequired = mpTsRequest.NotesRequired;
            requestFormed.PRICE_TYPE_ID = mpTsRequest.PRICE_TYPE_ID;
            requestFormed.ProcessingMode = mpTsRequest.ProcessingMode;
            requestFormed.ProposalEnquiryId = mpTsRequest.ProposalEnquiryId;
            requestFormed.ReturnAppliedChargingPolicyDetails = mpTsRequest.ReturnAppliedChargingPolicyDetails;
            requestFormed.ReturnAttachedOptionExtra = mpTsRequest.ReturnAttachedOptionExtra;
            requestFormed.RETURNCHEAPESTPRICE = mpTsRequest.RETURNCHEAPESTPRICE;
            requestFormed.ReturnMandatoryExtraPrices = mpTsRequest.ReturnMandatoryExtraPrices;
            requestFormed.ReturnMatchCode = mpTsRequest.ReturnMatchCode;
            requestFormed.ReturnPrices = mpTsRequest.ReturnPrices;
            requestFormed.ServiceSearchType = mpTsRequest.ServiceSearchType;
            requestFormed.UseMultiplePricesByBookingAndPriceType = mpTsRequest.UseMultiplePricesByBookingAndPriceType;
            requestFormed.ValidateRulesModifier = mpTsRequest.ValidateRulesModifier;
            requestFormed.XMLRequest = mpTsRequest.XMLRequest;
            requestFormed.XMLResponse = mpTsRequest.XMLResponse;
            requestFormed.AdultCount = mpTsRequest.AdultCount;
            requestFormed.PaxOccupancies = mpTsRequest.PaxOccupancies;

            return requestFormed;
        }
       
        private GetServicePricesRequest UpdateGetServicePricesTsRequest(GetServicePricesRequest request, List<SiteServiceData> siteServiceData)
        {
            var bookingTypeId = 0;
            var priceTypeId = 0;
       
            foreach(var service in request.ArrayOfUIPricesAndAvailabilityRequest)
            {
                var siteService = siteServiceData.FirstOrDefault(a => a.MarketplaceProductId == service.MarketplaceProductId);
                siteService.TSServiceId = service.ServiceID;
                service.SiteId = siteService.SiteId;
                service.ServiceID = siteService.MPServiceId;
                service.ServiceCode = siteService.MPServiceId.ToString();
                service.BookingType = bookingTypeId;
                service.PriceType = priceTypeId;
                service.ClientId = siteService.AgentId; 

                var priceCode = service.RoomDetails.Where(a=>a.PriceCode != null).Select(q=>q.PriceCode).FirstOrDefault();
                if(!string.IsNullOrEmpty(priceCode))
                {
                    var fieldsAndValues = priceCode.Split(",").ToList();
                   
                    bookingTypeId = Convert.ToInt32(fieldsAndValues.FirstOrDefault(a => a.Split(":").First().ToLower().Equals("bt")).Split(":")[1]);
                    priceTypeId = Convert.ToInt32(fieldsAndValues.FirstOrDefault(a => a.Split(":").First().ToLower().Equals("pt")).Split(":")[1]);
                }

            }
            request.BOOKING_TYPE_ID = bookingTypeId;
            request.PRICE_TYPE_ID = priceTypeId;
            return request;
        }

        private GetServicePricesRequest CheckIfHasAccess(GetServicePricesRequest request, Guid subscriberId)
        {
           
            foreach(var item in request.ArrayOfUIPricesAndAvailabilityRequest)
            {  
                item.HasAccess =  _pricingRepository.CheckIfHasAccess(item.MarketplaceProductId, subscriberId);            
            }

            return request;
        }
        //TODO: null checks
        public async Task<Entities.TSv2ApiEntities.GetServiceExtraPricesResponse> GetServiceExtraPrices(Entities.TSv2ApiEntities.GetServiceExtraPricesRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceExtraPrices", "PricingService", TraceId);
            var response = new Entities.TSv2ApiEntities.GetServiceExtraPricesResponse();
            //get subscriber id based on organisation and siteid
            var subscriberId = await GetSubscriberId(request.SiteId, request.OrganisationId);
            //check if there is access for subscriber
            var subscriberHasAccess = _pricingRepository.CheckIfHasAccess(request.MarketplaceProductId, subscriberId);
            if (subscriberHasAccess)
            {
                //convert subscriber data to publisher data
                var serviceDetails = _pricingRepository.GetSiteServiceData(new List<Guid>() { request.MarketplaceProductId }, subscriberId);
                var subscriberServiceId = request.SERVICEID;
                request.SERVICEID = serviceDetails[0].MPServiceId;
                request.SiteId = serviceDetails[0].SiteId;
                request.ClientId = serviceDetails[0].AgentId;

            var watch = Stopwatch.StartNew();
                var result = await _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Pricing, "GetServiceExtraPricesFromTs", null, null, 0, request.SiteId);
                var jsonResponse = JsonConvert.DeserializeObject < Response < Entities.TSv2ApiEntities.GetServiceExtraPricesResponse>>(result);
                response = jsonResponse.ResponseMessage;
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.MPTSAPI, "PostResponseAsync", "APIManager", TraceId, watch.ElapsedMilliseconds);
                
                //convert response service id from publisher to subscriber
                response.ServiceId = subscriberServiceId;                
            }
            else
            {
                //return empty response with error message saying no access
                response.Errors = new RequestResponseError[1];
                response.Errors[0].ErrorMessage = "Current user does not have access to this service";
                string errorLogMessage = string.Format("Subscriber:{0} does not have access to marketplace product {1}", subscriberId, request.MarketplaceProductId);
                LoggingHelper.LogError(_logger, ExceptionType.User, "GetServiceExtraPrices", "PricingService", TraceId, new Exception());
            }

            LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceExtraPrices", "PricingService", TraceId);
            return response;
        }

        public async Task<Response<CalculateBookingPriceResponse>> GetBookingPrice(CalculateBookingPriceRequest request)
        {
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingPrice", "PricingService", TraceId);


            var subscriberId = await GetSubscriberId(request.CalculateBookingPriceRequestData.BasicInfo.SiteId, request.CalculateBookingPriceRequestData.BasicInfo.OrganisationId);

            List<SiteServiceData> siteServiceData = _pricingRepository.GetSiteServiceData(request.CalculateBookingPriceRequestData.ServiceInfo.Select(a => a.MarketplaceProductId).ToList(), subscriberId);

            //check if service has valid subscriber or not, if yes remove the service object from request. -- Deepraj
            UpdateServiceAccess(request.CalculateBookingPriceRequestData.ServiceInfo);

            UpdatePublisherData(request, siteServiceData); //replaces the serviceids with those of the publisher. also sets the site it for earch service.

            //for each site make a Calculate booking price call.

            var watch = Stopwatch.StartNew();  //TODO pass entity type and entity id 5,6th param, sepration for different publishers
            //var result = await _apiManagerService.PostResponseAsync(request, TravelStudioControllers.Pricing, "GetBookingPriceFromTs", null, null, 0, request.CalculateBookingPriceRequestData.ServiceInfo.FirstOrDefault().SiteId);
            //var response = JsonConvert.DeserializeObject<Response<CalculateBookingPriceResponse>>(result);
            var response = SplitAndPostRequestToPublishersForGetBookingPrice(request, siteServiceData);

            if (response==null)
            {
                return null;
            }

            //UpdateSubscriberData(response.ResponseMessage, siteServiceData);
            watch.Stop();
            LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetBookingPrice", "APIManager", TraceId, watch.ElapsedMilliseconds);
            
            LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingPrice", "PricingService", TraceId);

            return response;
        }

        public async Task<Guid> GetSubscriberId(Guid siteId, int organisationId)
        {
            var organisationIdsRootAndLevel1string = await _travelStudioService.GetOrganization(siteId, EntityType.Site);
            var organisationIdsRootAndLevel1 = JsonConvert.DeserializeObject<List<OrganizationDataModel>>(organisationIdsRootAndLevel1string);

            var organisationIdsToUse = GetRootAndValidLevel1Organisation(organisationIdsRootAndLevel1, organisationId);
            var subscribers = _pricingRepository.GetSubscribersBySite(siteId);

            return FetchSubscriberIdFromSubscribersAndOrganisations(subscribers, organisationIdsToUse);
        }

        private List<int> GetRootAndValidLevel1Organisation(List<OrganizationDataModel> organisationIdsRootAndLevel1, int passedOrganisationId)
        {
            List<int> organisationIds = new List<int>();
            var rootOrganisation = organisationIdsRootAndLevel1.FirstOrDefault(a => a.ParentOrganisationid == null);

            organisationIds.Add((int)rootOrganisation.Organisationid);

            if (rootOrganisation.Organisationid == passedOrganisationId)
                return organisationIds;

            organisationIds.Add(passedOrganisationId);
            
            return organisationIds;
        }

        private Guid FetchSubscriberIdFromSubscribersAndOrganisations(List<SubscriberDataModel> subscribers, List<int> organisationIdsTouse)
        {
            var subscriber = subscribers.FirstOrDefault(a => organisationIdsTouse.Contains((int)a.OrganizationId));
            if (subscriber == null)
                return Guid.Empty;

            return subscriber.SubscriberId;
        }
        

        private  Response<CalculateBookingPriceResponse> SplitAndPostRequestToPublishersForGetBookingPrice(CalculateBookingPriceRequest request, List<SiteServiceData> siteServiceData)
        {
            List<Response<CalculateBookingPriceResponse>> publisherResponses = new List<Response<CalculateBookingPriceResponse>>();
            Response<CalculateBookingPriceResponse> response = new Response<CalculateBookingPriceResponse>
            {
                Status = "success",
                ResponseMessage = new CalculateBookingPriceResponse
                {
                    CalculateBookingPriceResponseData = new CalculateBookingPriceResp
                    {
                        ServicePriceInfo = new List<CalcBookingPriceServicePrice>(),
                    }
                },
                TraceId = TraceId
            };
            var siteIds = request.CalculateBookingPriceRequestData.ServiceInfo.Select(a => a.SiteId).Distinct().ToList();
            List<Task<string>> tasks = new List<Task<string>>();
            List<Tuple<Guid, int, Task<string>>> taskList = new List<Tuple<Guid,int, Task<string>>>();
            foreach(var site in siteIds)
            {
                var services = request.CalculateBookingPriceRequestData.ServiceInfo.Where(a => a.SiteId == site && !a.isAccessNotAllowed).ToList();
                if (services != null && services.Count > 0)
                {
                    var agentIds = services.Select(a => a.AgentId).Distinct().ToList();
                    foreach (var agent in agentIds)
                    {
                        var req = GetBookingPriceRequestForSite(request, services.Where(a => a.AgentId == agent).ToList(), agent);
                        var task = _apiManagerService.PostResponseAsync(req, TravelStudioControllers.Pricing, "GetBookingPriceFromTs", null, null, 0, site);
                        tasks.Add(task);
                        taskList.Add(new Tuple<Guid, int, Task<string>>(site, agent, task));
                    }
                }
            }

            Task.WaitAll(tasks.ToArray());

            foreach(var taskItem in taskList)
            {
                var task = taskItem.Item3;
                try
                {                    
                    var result = JsonConvert.DeserializeObject<Response<CalculateBookingPriceResponse>>(task.Result);

                    if (result == null || result.ResponseMessage == null || result.ResponseMessage.CalculateBookingPriceResponseData == null || result.ResponseMessage.CalculateBookingPriceResponseData.ServicePriceInfo == null)
                        continue;

                    UpdateSubscriberData(result.ResponseMessage, siteServiceData, taskItem.Item1);

                    response.ResponseMessage.CalculateBookingPriceResponseData.ServicePriceInfo.AddRange(result.ResponseMessage.CalculateBookingPriceResponseData.ServicePriceInfo);
                }
                catch (Exception)
                {
                    LoggingHelper.LogError(_logger, ExceptionType.System, "SplitAndPostRequestToPublishersForGetBookingPrice", "PricingService", TraceId, new Exception($"Deserialisation of GetBookingPriceResponse From Publisher failed. Response = {task.Result}"));
                }                
            }

            var servicesWithNoAccess =  request.CalculateBookingPriceRequestData.ServiceInfo.Where(a => a.isAccessNotAllowed).ToList();

            foreach (var serviceWithNoAccess in servicesWithNoAccess)
            {
                var siteService = siteServiceData.FirstOrDefault(a => a.MPServiceId == serviceWithNoAccess.ServiceID);
                response.ResponseMessage.CalculateBookingPriceResponseData.ServicePriceInfo.Add(new CalcBookingPriceServicePrice
                {
                     ServiceID = siteService.TSServiceId,
                     isAccessNotAllowed = true,
                 });
            }

            return response;
        }

        private void UpdateServiceAccess(List<CalcBookingPriceService> services)
        {
            foreach (var service in services)
            {
                service.isAccessNotAllowed = !_pricingRepository.CheckIfHasAccess(service.MarketplaceProductId,service.SubscriberId);
            }
        }

        private CalculateBookingPriceRequest GetBookingPriceRequestForSite(CalculateBookingPriceRequest mainRequest, List<CalcBookingPriceService> services, int agentId)
        {            
            var finalRequest = new CalculateBookingPriceRequest
            {
                CalculateBookingPriceRequestData = new CalculateBookingPriceReq
                {
                    BasicInfo = new CalcBookingPriceBasicInfo
                    {
                        CurrencyISOCode = mainRequest.CalculateBookingPriceRequestData.BasicInfo.CurrencyISOCode,
                        AgentID = agentId
                    },
                    ServiceInfo = services,
                    Modifiers = mainRequest.CalculateBookingPriceRequestData.Modifiers,
                    PassengerInfo = mainRequest.CalculateBookingPriceRequestData.PassengerInfo
                }
            };

            return finalRequest;
        }

        private void UpdatePublisherData(CalculateBookingPriceRequest request, List<SiteServiceData> siteServiceData)
        {
            if(request==null || request.CalculateBookingPriceRequestData==null || request.CalculateBookingPriceRequestData.ServiceInfo==null)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingPrice", "PricingService", TraceId, new Exception("UpdatePublisherData - Null data in request"));
                return;
            }                    

            foreach(var service in request.CalculateBookingPriceRequestData.ServiceInfo)
            {
                var siteService = siteServiceData.FirstOrDefault(a => a.MarketplaceProductId == service.MarketplaceProductId);
                siteService.TSServiceId = service.ServiceID;
                service.SiteId = siteService.SiteId;
                service.ServiceID = siteService.MPServiceId;
                service.AgentId = siteService.AgentId;
            }

            //request.CalculateBookingPriceRequestData.BasicInfo.AgentID = request.CalculateBookingPriceRequestData.ServiceInfo.FirstOrDefault().AgentId;

        }

        private void UpdateSubscriberData(CalculateBookingPriceResponse response, List<SiteServiceData> siteServiceData, Guid siteId)
        {
            if (response == null || response.CalculateBookingPriceResponseData == null || response.CalculateBookingPriceResponseData.ServicePriceInfo == null)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingPrice", "PricingService", TraceId, new Exception("UpdateSubscriberData - Null data in request"));
                return;
            }

            foreach (var service in response.CalculateBookingPriceResponseData.ServicePriceInfo)
            {
                var siteService = siteServiceData.FirstOrDefault(a => a.MPServiceId == service.ServiceID && a.SiteId == siteId);

                service.ServiceID = siteService.TSServiceId;

                foreach (var option in service.OptionInfo.Options)
                {
                    option.IsPricesNotFound = option.ErrorMessage != null && option.ErrorMessage.Trim() == "Prices Not Found";
                }

                if(service.ExtraInfo != null && service.ExtraInfo.Extras !=null && service.ExtraInfo.Extras.Count() > 0)
                {
                    foreach (var extra in service.ExtraInfo.Extras)
                    {
                        extra.IsPricesNotFound = extra.ErrorMessage != null && extra.ErrorMessage.Trim() == "Prices Not Found";
                    }
                }

            }
        }

         private void UpdateSubscriberDataForGetServicePrices(GetServicePricesResponse response, List<SiteServiceData> siteServiceData, Guid siteId)
        {
            // if (response == null || response.CalculateBookingPriceResponseData == null || response.CalculateBookingPriceResponseData.ServicePriceInfo == null)
            // {
            //     LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingPrice", "PricingService", TraceId, new Exception("UpdateSubscriberData - Null data in request"));
            //     return;
            // }

            foreach (var service in response.Services)
            {
                var siteService = siteServiceData.FirstOrDefault(a => a.MPServiceId == int.Parse(service.ServiceID) && a.SiteId == siteId);

                if (siteService != null)
                {
                    service.ServiceID = siteService.TSServiceId.ToString();
                    service.ServiceCode = siteService.TSServiceId.ToString();
                }
            }
        }

    }
}