using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using MarketPlaceService.API.Utilities;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/SubscriptionProducts")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SubscriptionProductsController : ControllerBase
    {

        readonly ILogger<PublishersController> _logger;
        readonly ISubscriptionProductsService _subscriptionProductsService;

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
            _subscriptionProductsService.TraceId = _traceId;
             if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _subscriptionProductsService.UserId = _userId;
        }
        public SubscriptionProductsController(ISubscriptionProductsService subscriptionProductsService,ILogger<PublishersController> logger)
        {
            _logger = logger;
            _subscriptionProductsService = subscriptionProductsService;
        }

        [HttpPost("")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> SubscriptionProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            if(subscriptionProductDataModel.ValidateObjectForNull())
            return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "SubscriptionProduct", "SubscriptionProductsController", TraceId);
            Response<SubscriptionProduct> response = new Response<SubscriptionProduct>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriptionProductsService.SubscriptionProduct(subscriptionProductDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "SubscriptionProduct", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriptionProduct>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "SubscriptionProduct", "SubscriptionProductsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "SubscriptionProduct", "SubscriptionProductsController", TraceId, ex);
                response = new Response<SubscriptionProduct>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }

        }


        [HttpGet("subscriptionStatus")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetSubscriptionStatus()
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriptionStatus", "SubscriptionProductsController", TraceId);
            Response<IEnumerable<SubscriptionStatus>> response = new Response<IEnumerable<SubscriptionStatus>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriptionProductsService.GetSubscriptionStatus();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSubscriptionStatus", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SubscriptionStatus>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriptionStatus", "SubscriptionProductsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriptionStatus", "SubscriptionProductsController", TraceId, ex);
                response = new Response<IEnumerable<SubscriptionStatus>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }

        }


        [HttpGet("subscriptionProductSearch")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> SearchSubscriptionProduct([FromQuery]MarketPlaceProductsSearch marketPlaceProductSearchDataModel)
        {
            if(marketPlaceProductSearchDataModel.PublisherId.ValidateEmptyGuid() || marketPlaceProductSearchDataModel.ProductTypeId.ValidateInteger() || marketPlaceProductSearchDataModel.SubscriberId.ValidateEmptyGuid())
                return BadRequest();
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchSubscriptionProduct", "SubscriptionProductsController", TraceId);
            Response<IEnumerable<MarketPlaceProduct>> response = new Response<IEnumerable<MarketPlaceProduct>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriptionProductsService.SearchSubscriptionProduct(marketPlaceProductSearchDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "SearchSubscriptionProduct", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MarketPlaceProduct>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "SearchSubscriptionProduct", "SubscriptionProductsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "SearchSubscriptionProduct", "SubscriptionProductsController", TraceId, ex);
                response = new Response<IEnumerable<MarketPlaceProduct>>
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


          [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscriptionProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriptionProduct", "SubscriptionProductsController", TraceId);
            if(subscriptionProductDataModel.ValidateObjectForNull())
                return BadRequest();
             
            Response<SubscriptionProduct> response = new Response<SubscriptionProduct>();             
             try
             {
                var watch = Stopwatch.StartNew();
                subscriptionProductDataModel.TraceId=TraceId;
                var result = await _subscriptionProductsService.UpdateSubscriptionProduct(subscriptionProductDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateSubscriptionProduct", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<SubscriptionProduct>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriptionProduct", "SubscriptionProductsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateSubscriptionProduct", "SubscriptionProductsController", TraceId, ex);
                  response = new Response<SubscriptionProduct>
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



         [HttpPost("Product")]
         public async Task<IActionResult> ProcessSubscriberProduct(SubscriberProductDataRequest request)
        {
            //  
             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessSubscriberProduct", "SubscriptionProductsController", TraceId);
            if(request.ValidateObjectForNull() || request.MarketPlaceProductId.ValidateEmptyGuid() || request.SubscriberId.ValidateEmptyGuid())
                return BadRequest();
           
            Response<SubscriberProductDataResponse> response = new Response<SubscriberProductDataResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriptionProductsService.ProcessSubscriberProduct(request);
                //var data = JsonConvert.DeserializeObject<Response<ProductData>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "ProcessSubscriberProduct", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberProductDataResponse>
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

                LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSubscriberProduct", "SubscriptionProductsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessSubscriberProduct", "SubscriptionProductsController", TraceId, ex);
                response = new Response<SubscriberProductDataResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }
        }

        [HttpGet("SubscribedProductHistory/{marketplaceProductId}/{subscriberId}")]
         public async Task<IActionResult> GetSubscriptionProductInfo(Guid marketplaceProductId, Guid subscriberId)
        {
            if(marketplaceProductId.ValidateEmptyGuid() || subscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriptionProductInfo", "SubscriptionProductsController", TraceId);
            Response<SubscriptionProductInfo> response = new Response<SubscriptionProductInfo>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriptionProductsService.GetSubscriptionProductInfo(marketplaceProductId, subscriberId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSubscriptionProductInfo", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriptionProductInfo>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriptionProductInfo", "SubscriptionProductsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriptionProductInfo", "SubscriptionProductsController", TraceId, ex);
                response = new Response<SubscriptionProductInfo>
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

        [HttpPost("unsubscribeproduct")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UnsubscribeProduct(SubscriptionProduct subscriptionProductDataModel)
        {
            if(subscriptionProductDataModel.ValidateObjectForNull())
            return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UnsubscribeProduct", "SubscriptionProductsController", TraceId);
            Response<SubscriptionProduct> response = new Response<SubscriptionProduct>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriptionProductsService.UnsubscribeProduct(subscriptionProductDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UnsubscribeProduct", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriptionProduct>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UnsubscribeProduct", "SubscriptionProductsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UnsubscribeProduct", "SubscriptionProductsController", TraceId, ex);
                response = new Response<SubscriptionProduct>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }

        }

        
        [HttpGet("SubscribedProductHistoryJson/{subscriberProductHistoryId}")]
         public async Task<IActionResult> GetSubscriptionProductHistoryJson(Guid subscriberProductHistoryId)
        {
            if(subscriberProductHistoryId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriptionProductHistoryJson", "SubscriptionProductsController", TraceId);
            Response<string> response = new Response<string>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriptionProductsService.GetSubscriptionProductHistoryJson(subscriberProductHistoryId);

                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSubscriptionProductHistoryJson", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<string>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriptionProductHistoryJson", "SubscriptionProductsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriptionProductHistoryJson", "SubscriptionProductsController", TraceId, ex);
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


    }
}
