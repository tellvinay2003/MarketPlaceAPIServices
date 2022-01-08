using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using MarketPlaceService.API.Utilities;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/publishedproducts")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PublishedProductsController : ControllerBase
    {
        readonly ILogger<PublishersController> _logger;
        readonly IProfileService _profileService;

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

            }
        }
        private void ActivateTrace()
        {
            if (_traceId == Guid.Empty)
                _traceId = Request.Headers.ContainsKey("TraceId") ? Guid.Parse(Request.Headers["TraceId"]) : Guid.NewGuid();
            _profileService.TraceId = _traceId;

            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _profileService.UserId = _userId;
        }
        public PublishedProductsController(IProfileService profileService, ILogger<PublishersController> logger)
        {
            _logger = logger;
            _profileService = profileService;
        }


         /// <summary>
        /// Publish Product
        /// </summary>
        /// <returns></returns>
        // [HttpGet("")]
        [HttpPost("")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> PublishProduct(PublishedProductsDataModel publishedProductsDataModel)
        {
            if(publishedProductsDataModel.ValidateObjectForNull())
            return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "PublishProduct", "PublishedProductController", TraceId);
            Response<PublishedProductsDataModel> response = new Response<PublishedProductsDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.PublishProduct(publishedProductsDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "PublishProduct", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublishedProductsDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "PublishProduct", "PublishedProductController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "PublishProduct", "PublishedProductController", TraceId, ex);
                response = new Response<PublishedProductsDataModel>
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
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePublishedProduct(Guid id, PublishedProductsDataModel publishedProductsDataModel)
        {
            if(id.ValidateEmptyGuid() || publishedProductsDataModel.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublishedProduct", "PublishedProductController", TraceId);
            Response<PublishedProductsDataModel> response = new Response<PublishedProductsDataModel>();             
             try
             {
                var watch = Stopwatch.StartNew();
                publishedProductsDataModel.TraceId=TraceId; 
                var result = await _profileService.UpdatePublishedProduct(publishedProductsDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdatePublishedProduct", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<PublishedProductsDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublishedProduct", "PublishedProductController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "UpdatePublishedProduct", "PublishedProductController", TraceId, ex);
                  response = new Response<PublishedProductsDataModel>
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


        [HttpGet("allowedsubscribers")]
        public async Task<IActionResult> GetAllowedSubscribers([FromQuery]List<Guid> productIds)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllowedSubscribers", "PublishedProductController", TraceId);
            Response<IEnumerable<PublishedProductAllowedSubscriber>> response = new Response<IEnumerable<PublishedProductAllowedSubscriber>>();             
             try
             {
                var watch = Stopwatch.StartNew();
                var result = await _profileService.GetAllowedSubscribers(productIds);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllowedSubscribers", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<IEnumerable<PublishedProductAllowedSubscriber>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllowedSubscribers", "PublishedProductController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllowedSubscribers", "PublishedProductController", TraceId, ex);
                  response = new Response<IEnumerable<PublishedProductAllowedSubscriber>>
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


        [HttpPost("allowedsubscribers/{publishedproductId}/{subscriberId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> AllowedSubscriber(Guid publishedproductId,Guid subscriberId)
        {
             if(publishedproductId.ValidateEmptyGuid() || subscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "AllowedSubscriber", "PublishedProductController", TraceId);
            Response<PublishedProductAllowedSubscriber> response = new Response<PublishedProductAllowedSubscriber>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.AllowedSubscriber(publishedproductId,subscriberId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "AllowedSubscriber", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublishedProductAllowedSubscriber>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "AllowedSubscriber", "PublishedProductController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "AllowedSubscriber", "PublishedProductController", TraceId, ex);
                response = new Response<PublishedProductAllowedSubscriber>
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

        [HttpDelete("allowedsubscribers/{publishedProductId}/{subscriberId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteAllowedSubscriber(Guid publishedProductId,Guid subscriberId)
        {
             if(publishedProductId.ValidateEmptyGuid() || subscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteAllowedSubscriber", "PublishedProductController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.DeleteAllowedSubscriber(publishedProductId,subscriberId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteAllowedSubscriber", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteAllowedSubscriber", "PublishedProductController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteAllowedSubscriber", "PublishedProductController", TraceId, ex);
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

        [HttpPost("UnpublishProduct")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UnpublishProduct(PublishedProductsDataModel publishedProductsDataModel)
        {
            if(publishedProductsDataModel.ValidateObjectForNull())
            return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UnpublishProduct", "PublishedProductController", TraceId);
            Response<PublishedProductsDataModel> response = new Response<PublishedProductsDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _profileService.UnpublishProduct(publishedProductsDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UnpublishProduct", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<PublishedProductsDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UnpublishProduct", "PublishedProductController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UnpublishProduct", "PublishedProductController", TraceId, ex);
                response = new Response<PublishedProductsDataModel>
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

        [HttpGet("PublishedStatus")]
        public async Task<IActionResult> GetPublishedStatus()
        {
             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPublishedStatus", "PublisherController", TraceId);
            Response<IEnumerable<PublishedStatus>> response = new Response<IEnumerable<PublishedStatus>>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                var result = await _profileService.GetPublishedStatus();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPublishedStatus", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<IEnumerable<PublishedStatus>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetPublishedStatus", "PublishedController", TraceId);

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

    }
}
