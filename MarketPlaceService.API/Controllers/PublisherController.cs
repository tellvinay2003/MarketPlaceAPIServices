using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.API.Models;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Cors;
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using MarketPlaceService.API.Utilities;
using Microsoft.EntityFrameworkCore;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    //[Authorize]
    // [Route("api/[controller]")]
    [Produces("application/json")]
    // [Route("api/v{version:apiVersion}/[controller]/{action}")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        readonly ILogger<PublishersController> _logger;
        readonly IProfileService _profileService;
        readonly ITransactionLoggerService<MarketplaceDataModel> _transactionLoggerService;
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
        private void ActivateTrace()
        {
            if (_traceId == Guid.Empty)
                _traceId = Request.Headers.ContainsKey("TraceId") ? Guid.Parse(Request.Headers["TraceId"]) : Guid.NewGuid();
            _profileService.TraceId = _traceId;
        }

        public PublishersController(IProfileService profileService, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService, ILogger<PublishersController> logger, ICommonService commonService)
        {
            _logger = logger;
            _profileService = profileService;
            _transactionLoggerService=transactionLoggerService;
            _commonService = commonService;
        }

        #region Publishers

        /// <summary>
        /// Get list of all publishers
        /// </summary>
        /// <returns></returns>
        // [HttpGet("GetAll")]
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetRegisteredPublishers()
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegisteredPublishers", "PublisherController", TraceId);
            Response<IEnumerable<PublisherDataModel>> response = new Response<IEnumerable<PublisherDataModel>>();
            try
            {
                _logger.LogInformation("Publisher service is called.");
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.GetPublishersListAsync();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublishersListAsync", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                // response = new Response<IEnumerable<PublisherDataModel>> { ResponseCode = (int)Code.success, ResponseMessage = result };

               

                response = new Response<IEnumerable<PublisherDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                 await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        PublisherCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

                LoggingHelper.LogInfo(_logger, LogType.End, "GetRegisteredPublishers", "PublishedController", TraceId);
                 
                if(result.IsNullOrEmpty())
                    return NoContent();

                return Ok(response);
            }            
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetRegisteredPublishers", "PublishedController", TraceId, ex);
                // response = new Response<IEnumerable<PublisherDataModel>> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString() };
                response = new Response<IEnumerable<PublisherDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }       


        /// <summary>
        /// Get specific publisher details by PublisherId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPublisherById")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PublisherDataModel), Description = "Returns finded Publisher")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetPublisherById([FromRoute] Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();
            
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherById", "PublisherController", TraceId);
            Response<PublisherDataModel> response = new Response<PublisherDataModel>();
            try
            {
                _logger.LogInformation("Publisher service is called.");
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.GetPublisherById(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublisherById", "ProfileService", TraceId, watch.ElapsedMilliseconds);

                
                response = new Response<PublisherDataModel> 
                { 
                    ResponseCode = (int)Code.success,
                    ResponseMessage = result,
                    TraceId=TraceId
                };

                // response.ResponseMessage.ToList().Add(result);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherById", "PublishedController", TraceId);

                 await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {

                    TransactionData = new MarketplaceDataModel
                    {
                        PublisherCollection = Enumerable.Repeat(result, 1)
                    },

                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

                if(result==null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPublisherById", "PublishedController", TraceId, ex);
                response = new Response<PublisherDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }

        }


        /// <summary>
        /// Create new publisher
        /// </summary>
        /// <returns></returns>
        // [HttpPost("CreatePublisher")]
        [HttpPost("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> AddNewPublisher([FromBody] PublisherDataModel publisherItem, ApiVersion apiVersion)
        {
            if(publisherItem.ValidateObjectForNull() ||publisherItem.SiteId.ValidateEmptyGuid() || publisherItem.PublisherName== string.Empty)
                return BadRequest();
            
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "AddNewPublisher", "PublisherController", TraceId);
            Response<PublisherDataModel> response = new Response<PublisherDataModel>();
            try
            {
                var isOrganisationValid = await _commonService.CanOrganisationBeUsed((int)publisherItem.OrganizationId, EntityType.Publisher, Guid.Empty, publisherItem.SiteId);
                if (!isOrganisationValid.Item1)
                {
                    return BadRequest(new Response<PublisherDataModel> { ResponseCode = (int)Code.BadRequest, Message = isOrganisationValid.Item2 });
                }

                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.AddNewPublisher(publisherItem);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "AddNewPublisher", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublisherDataModel> { ResponseCode = (int)Code.Created, ResponseMessage = result,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "AddNewPublisher", "PublishedController", TraceId);

                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                     TransactionData = new MarketplaceDataModel
                     {
                         PublisherCollection = Enumerable.Repeat(result, 1)
                     },

                     TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = Guid.NewGuid()
                }).ConfigureAwait(false);

                return CreatedAtRoute(nameof(GetPublisherById), new { id = result.PublisherId, version = apiVersion.ToString()}, response);
                //return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "AddNewPublisher", "PublishedController", TraceId, ex);
                response = new Response<PublisherDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };

                if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<PublisherDataModel> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding new publisher failed. Another publisher with the same Site and Organisation exists.")});
                
                if(ex is TimeoutException tex)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        /// <summary>
        /// Update existing publisher details by PublisherId
        /// </summary>
        /// <returns></returns>
        // [HttpPut("UpdatePublisher")]
        [HttpPut("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> UpdatePublisher([FromBody] PublisherDataModel publisherItem)
        {
            //todo validations
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublisher", "PublisherController", TraceId);
            Response<PublisherDataModel> response = new Response<PublisherDataModel>();
            try
            {
                var isOrganisationValid = await _commonService.CanOrganisationBeUsed((int)publisherItem.OrganizationId, EntityType.Publisher, publisherItem.PublisherId, publisherItem.SiteId);
                if (!isOrganisationValid.Item1)
                {
                    return BadRequest(new Response<PublisherDataModel> { ResponseCode = (int)Code.BadRequest, Message = isOrganisationValid.Item2 });
                }
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.UpdatePublisher(publisherItem);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdatePublisher", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                if(result==null)
                {
                    response = new Response<PublisherDataModel> { ResponseCode = (int)Code.dataNotFound, ResponseMessage = result,TraceId=TraceId };
                }
                else{
                response = new Response<PublisherDataModel> { ResponseCode = (int)Code.success, ResponseMessage = result,TraceId=TraceId };
                }
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublisher", "PublishedController", TraceId);
                await  _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                     TransactionData = new MarketplaceDataModel
                     {
                         PublisherCollection = Enumerable.Repeat(result, 1)
                     },
                     TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

                 if(result == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdatePublisher", "PublishedController", TraceId, ex);
                response = new Response<PublisherDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };

                if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<PublisherDataModel> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Update failed. Another publisher with the same Site and Organisation exists.")});

                if(ex is TimeoutException tex)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        /// <summary>
        /// Delete existing publisher by PublisherId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>       
        [HttpDelete("{id}")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> DeletePublisher(Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeletePublisher", "PublisherController", TraceId);
         //   Response<PublisherDataModel> response = new Response<PublisherDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.DeletePublisher(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeletePublisher", "ProfileService", TraceId, watch.ElapsedMilliseconds);
             var   response = new Response<bool> { ResponseCode = (int)Code.success, ResponseMessage = result,TraceId=TraceId };
             //  var response = result;
                LoggingHelper.LogInfo(_logger, LogType.End, "DeletePublisher", "PublishedController", TraceId);

                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        OperationResult = result
                    },

                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

               

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeletePublisher", "PublishedController", TraceId, ex);
               var response = new Response<bool> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        #endregion

        
        #region Misc

        

        [HttpGet("PublishStatus")]
        public async Task<IActionResult> GetPublishStatus()
        {
             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishStatus", "PublisherController", TraceId);
            Response<IEnumerable<PublishedStatus>> response = new Response<IEnumerable<PublishedStatus>>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                var result = await _profileService.GetPublishStatus();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublishStatus", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<IEnumerable<PublishedStatus>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishStatus", "PublishedController", TraceId);

                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        PublishedStatusCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "GetPublishStatus", "PublishedController", TraceId, ex);
                  response = new Response<IEnumerable<PublishedStatus>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
             }
        }

        //[HttpGet("ProcessingStatus")]
        //public async Task<IActionResult> GetProcessingStatus()
        //{
        //     ActivateTrace();         
        //    LoggingHelper.LogInfo(_logger, LogType.Start, "GetProcessingStatus", "PublisherController", TraceId);
        //    Response<IEnumerable<ProcessingStatus>> response = new Response<IEnumerable<ProcessingStatus>>();
             
        //     try
        //     {
        //        var watch = Stopwatch.StartNew();
        //        var result = await _profileService.GetProcessingStatus();
        //        watch.Stop();
        //        LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetProcessingStatus", "ProfileService", TraceId, watch.ElapsedMilliseconds);
        //         response = new Response<IEnumerable<ProcessingStatus>>
        //        {
        //            ResponseCode = (int)Code.success,
        //            Status = "Success",
        //            ResponseMessage = result,
        //            ExecutionTimeMS = watch.ElapsedMilliseconds,
        //            TraceId = TraceId
        //        };
        //        LoggingHelper.LogInfo(_logger, LogType.End, "GetProcessingStatus", "PublishedController", TraceId);
        //        return Ok(response);
        //     }
        //     catch(Exception ex)
        //     {
        //          LoggingHelper.LogError(_logger, ExceptionType.System, "GetProcessingStatus", "PublishedController", TraceId, ex);
        //          response = new Response<IEnumerable<ProcessingStatus>>
        //        {
        //            ResponseCode = (int)Code.exceptionError,
        //            Status = "Failure",
        //            Message = ex.ToString(),
        //            TraceId = TraceId
        //        };
                
        //        if(ex is TimeoutException)
        //            return StatusCode(408);
                
        //        return StatusCode(500,response);
        //     }
        //}        

        #endregion


        #region Products
        /// <summary>
        /// Get list of all unpublished products
        /// </summary>
        /// <returns></returns>
        // [HttpGet("")]
        [HttpGet("ProductSearch")]
        public async Task<IActionResult> SearchProduct(int serviceTypeId, int RegionId, string productName, Guid publisherId, int? publishedStatus, ProductType productTypeId) //productTypeId service/package
        {
            if(serviceTypeId.ValidateInteger() || RegionId.ValidateInteger() || publisherId.ValidateEmptyGuid())
                return BadRequest();
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchProduct", "PublishersController", TraceId);
            Response<IEnumerable<PublishedProductsDataModel>> response = new Response<IEnumerable<PublishedProductsDataModel>>();
            IList<PublishedProductsDataModel> objectResult = new List<PublishedProductsDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();

                // Service call to get Publisher item
                var publisher = _profileService.GetPublisherById(publisherId);

                // Service call for Search product
               // _logger.LogInformation("Publisher service is called for SearchProductAsync .");
               var serviceOutput = new Response<IEnumerable<ServiceDataModel>>();
               var packageOutput = new Response<IEnumerable<PackageDataModel>>();

               productTypeId = productTypeId == 0 ? ProductType.Service : productTypeId;

               if(productTypeId == ProductType.Service) //service type
               {
                    var result = await _profileService.SearchProductAsync(serviceTypeId, RegionId, productName, publisherId);
                    watch.Stop();
                    LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "SearchProductAsync", "ProfileService", TraceId, watch.ElapsedMilliseconds);

                     serviceOutput = JsonConvert.DeserializeObject<Response<IEnumerable<ServiceDataModel>>>(result);

                    if(serviceOutput.ResponseMessage.IsNullOrEmpty())
                        return NoContent();
               }
               else //package type
               {
                    var result = await _profileService.SearchPackageProductAsync(serviceTypeId, RegionId, productName, publisherId);
                    watch.Stop();
                    LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "SearchPackageProductAsync", "ProfileService", TraceId, watch.ElapsedMilliseconds);

                     packageOutput = JsonConvert.DeserializeObject<Response<IEnumerable<PackageDataModel>>>(result);

                    if(packageOutput.ResponseMessage.IsNullOrEmpty())
                        return NoContent();
               }
                var publishedUnpublishedProducts = await _profileService.GetPublishedUnpublishedProducts(publisherId, serviceTypeId, serviceOutput.ResponseMessage, publishedStatus, packageOutput.ResponseMessage, productTypeId);

                response = new Response<IEnumerable<PublishedProductsDataModel>> { ResponseCode = (int)Code.success, ResponseMessage = publishedUnpublishedProducts,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "SearchProduct", "PublishersController", TraceId);
                              
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "SearchProduct", "PublishersController", TraceId, ex);
                response = new Response<IEnumerable<PublishedProductsDataModel>> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };
                
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

       
        

        [HttpPost("Product")]
         public async Task<IActionResult> ProcessPublishedProduct(ProductDataRequest request)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessPublishedProduct", "PublisherController", TraceId);
            if(request.ValidateObjectForNull() || request.ProductId.ValidateInteger() || request.PublisherId.ValidateEmptyGuid())
                return BadRequest();
            
            Response<PublishedProductDataResponse> response = new Response<PublishedProductDataResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.ProcessPublishedProduct(request);
                //var data = JsonConvert.DeserializeObject<Response<ProductData>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "ProcessPublishedProduct", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublishedProductDataResponse>
                {                    
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                if(result.IsSuccess)
                {
                    response.ResponseCode = (int)Code.success;
                    response.Status="success";
                }
                else
                {
                    response.ResponseCode = (int)Code.exceptionError;
                    response.Status="failure";
                }

                LoggingHelper.LogInfo(_logger, LogType.End, "ProcessPublishedProduct", "PublishedController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessPublishedProduct", "PublishedController", TraceId, ex);
                response = new Response<PublishedProductDataResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        // [HttpGet("Product")]
        //  public async Task<IActionResult> GetPublishedProductDataString(Guid publishedProductId)
        // {
        //     if(publishedProductId.ValidateEmptyGuid())
        //         return BadRequest();

        //     _logger.LogInformation("SearchProGetPublishedProductDataStringduct is called.");
        //     Response<string> response = new Response<string>();
        //     try
        //     {
        //         _logger.LogInformation("GetPublishedProductDataString service is called.");
        //         var watch = Stopwatch.StartNew();
        //         // Service call
                 //var result = await _profileService.GetPublishedProductDataString(publishedProductId);
        //        // var data = JsonConvert.DeserializeObject<Response<ProductData>>(result);

        //         watch.Stop();
        //         _logger.LogInformation("Execution Time of service for GetPublishedProductDataString call is: {duration}ms", watch.ElapsedMilliseconds);
        //         response = new Response<string>
        //         {
        //             ResponseCode = (int)Code.success,
        //             Status = "Success",
        //             ResponseMessage = result,
        //             ExecutionTimeMS = watch.ElapsedMilliseconds,
        //             TraceId = Guid.NewGuid()
        //         };

        //         _logger.LogInformation("GetPublishedProductDataString executed successfully.");

        //         return Ok(response);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex.ToString());
        //         response = new Response<string>
        //         {
        //             ResponseCode = (int)Code.exceptionError,
        //             Status = "Failure",
        //             Message = ex.ToString(),
        //             TraceId = Guid.NewGuid()
        //         };

        //         if(ex is TimeoutException)
        //             return StatusCode(408);
                
        //         return StatusCode(500);
        //     }
        // }

        [HttpGet("Product")]
         public async Task<IActionResult> GetPublishedProductInfo(Guid publishedProductId)
        {
            if(publishedProductId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishedProductInfo", "PublisherController", TraceId);
            Response<PublishedProductInfo> response = new Response<PublishedProductInfo>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.GetPublishedProductInfo(publishedProductId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublishedProductInfo", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublishedProductInfo>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishedProductInfo", "PublishedController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPublishedProductInfo", "PublishedController", TraceId, ex);
                response = new Response<PublishedProductInfo>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        #endregion
   
      

        #region Publisher Agent maps

        [HttpGet("mappedsubscribers/{publisherId}")]
         public async Task<IActionResult> GetPublisherAgentMap(Guid publisherId)
        {
            if(publisherId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherAgentMap", "PublisherController", TraceId);
            Response<IEnumerable<PublisherAgentMap>> response = new Response<IEnumerable<PublisherAgentMap>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.GetPublisherAgentMaps(publisherId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublisherAgentMaps", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<PublisherAgentMap>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherAgentMap", "PublishedController", TraceId);

                if(result.IsNullOrEmpty())
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPublisherAgentMap", "PublishedController", TraceId, ex);
                response = new Response<IEnumerable<PublisherAgentMap>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpGet("mappedsubscribers/{publisherId}/{subscriberId},", Name = "GetPublisherAgentMaps")]
         public async Task<IActionResult> GetPublisherAgentMaps(Guid publisherId, Guid subscriberId)
        {
            if(publisherId.ValidateEmptyGuid() || subscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherAgentMaps", "PublisherController", TraceId);
            Response<PublisherAgentMap> response = new Response<PublisherAgentMap>();
            try
            {
                 if(publisherId == Guid.Empty || subscriberId ==Guid.Empty)
                {
                    return BadRequest();
                }
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.GetPublisherAgentMaps(publisherId, subscriberId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublisherAgentMaps", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublisherAgentMap>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherAgentMaps", "PublishedController", TraceId);

                if(result == null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPublisherAgentMaps", "PublishedController", TraceId, ex);
                response = new Response<PublisherAgentMap>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPost("mappedsubscribers/{publisherId}/{subscriberId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertPublisherAgentMaps(Guid publisherId, Guid subscriberId, PublisherAgentMap request, ApiVersion apiVersion)
        {
            if(publisherId.ValidateEmptyGuid() || subscriberId.ValidateEmptyGuid() || request.ValidateObjectForNull() )
                return BadRequest();
            if(request.AgentId.ValidateInteger()|| request.OrganisationId.ValidateObjectForNull()|| request.UserId.ValidateObjectForNull()||request.BookingPrefixId.ValidateObjectForNull())
                return StatusCode(500,"Please ensure all 3 mandatory fields (Agent, Organisation Prefix and Booking Owner) have been selected.");
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertPublisherAgentMaps", "PublisherController", TraceId);
            Response<PublisherAgentMap> response = new Response<PublisherAgentMap>();
            try
            {
                 if(publisherId == Guid.Empty || subscriberId ==Guid.Empty)
                {
                    return BadRequest();
                }
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.InsertPublisherAgentMaps(request, publisherId,subscriberId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertPublisherAgentMaps", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublisherAgentMap>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "InsertPublisherAgentMaps", "PublishedController", TraceId);

                if(result == null)
                    return NotFound();

               return CreatedAtRoute(nameof(GetPublisherAgentMaps), new {publisherId = publisherId, subscriberId =subscriberId, version = apiVersion.ToString() }, response);
               //return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertPublisherAgentMaps", "PublishedController", TraceId, ex);
                response = new Response<PublisherAgentMap>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

               if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPut("mappedsubscribers/{publisherId}/{subscriberId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdatePublisherAgentMaps(Guid publisherId, Guid subscriberId, PublisherAgentMap request)
        {
            if(publisherId.ValidateEmptyGuid() || subscriberId.ValidateEmptyGuid() || request.ValidateObjectForNull())
                return BadRequest();
            if(request.AgentId.ValidateInteger()|| request.OrganisationId.ValidateObjectForNull()|| request.UserId.ValidateObjectForNull()||request.BookingPrefixId.ValidateObjectForNull())
                return StatusCode(400,"Please ensure all 3 mandatory fields (Agent, Organisation Prefix and Booking Owner) have been selected.");


            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublisherAgentMaps", "PublisherController", TraceId);
            Response<PublisherAgentMap> response = new Response<PublisherAgentMap>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.UpdatePublisherAgentMaps(request, publisherId,subscriberId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdatePublisherAgentMaps", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublisherAgentMap>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublisherAgentMaps", "PublishedController", TraceId);

                if(result == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdatePublisherAgentMaps", "PublishedController", TraceId, ex);
                response = new Response<PublisherAgentMap>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        [HttpDelete("mappedsubscribers/{publisherId}/{subscriberId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeletePublisherAgentMaps(Guid publisherId, Guid subscriberId)
        {
            
            if(publisherId.ValidateEmptyGuid() || subscriberId.ValidateEmptyGuid())
                return BadRequest();
            

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeletePublisherAgentMaps", "PublisherController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.DeletePublisherAgentMaps(publisherId,subscriberId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeletePublisherAgentMaps", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "DeletePublisherAgentMaps", "PublishedController", TraceId);

                if(!result)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeletePublisherAgentMaps", "PublishedController", TraceId, ex);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        #endregion
        
        #region PublisherDefaults

         [HttpGet("defaults/{publisherId}")]
         public async Task<IActionResult> GetPublisherDefaults(Guid publisherId)
        {
            if(publisherId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublisherDefaults", "PublisherController", TraceId);
            Response<PublisherDefaultDataModel> response = new Response<PublisherDefaultDataModel>();
            try
            {
                 if(publisherId == Guid.Empty)
                {
                    return BadRequest();
                }
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.GetPublisherDefaults(publisherId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublisherDefaults", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublisherDefaultDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublisherDefaults", "PublishedController", TraceId);

                if(result == null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPublisherDefaults", "PublishedController", TraceId, ex);
                response = new Response<PublisherDefaultDataModel>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPost("defaults")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertUpdatePublisherDefaults(Guid publisherId, PublisherDefaultDataModel request)
        {
            if(publisherId.ValidateEmptyGuid() || request.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertUpdatePublisherDefaults", "PublisherController", TraceId);
            Response<PublisherDefaultDataModel> response = new Response<PublisherDefaultDataModel>();
            try
            {
                 if(publisherId == Guid.Empty)
                {
                    return BadRequest();
                }
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.InsertUpdatePublisherDefaults(publisherId, request);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertUpdatePublisherDefaults", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublisherDefaultDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "InsertUpdatePublisherDefaults", "PublishedController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertUpdatePublisherDefaults", "PublishedController", TraceId, ex);
                response = new Response<PublisherDefaultDataModel>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpGet("manageSubscribersSearch")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> SearchManageSubscribers([FromQuery]ManageSubscribersSearch manageSubscribersDataModel)
        {
            // if(manageSubscribersDataModel.PublisherId.ValidateEmptyGuid() || manageSubscribersDataModel.SubscriberId.ValidateEmptyGuid())
            //     return BadRequest();
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "manageSubscribersDataModel", "PublishedController", TraceId);
            Response<IEnumerable<ManageSubscribersResponse>> response = new Response<IEnumerable<ManageSubscribersResponse>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.SearchManageSubscribers(manageSubscribersDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "manageSubscribersDataModel", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<ManageSubscribersResponse>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "manageSubscribersDataModel", "PublishedController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "manageSubscribersDataModel", "PublishedController", TraceId, ex);
                response = new Response<IEnumerable<ManageSubscribersResponse>>
                {
                    ResponseCode = (int)Code.ServerError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpGet("ProductHistoryJsonData")]
         public async Task<IActionResult> GetPublishedProductHistoryJsonData(Guid publishedProductHistoryId)
        {
            if(publishedProductHistoryId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishedProductHistoryJsonData", "PublisherController", TraceId);
            Response<string> response = new Response<string>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.GetPublishedProductHistoryJsonData(publishedProductHistoryId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublishedProductHistoryJsonData", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<string>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishedProductHistoryJsonData", "PublishedController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPublishedProductHistoryJsonData", "PublishedController", TraceId, ex);
                response = new Response<string>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        #endregion
    }
}