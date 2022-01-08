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
using MarketPlaceService.API.Utilities;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/queuedSubscriptions")]
    [ApiVersion("1.0")]
    [ApiController]
    public class QueuedSubscriptionsController : ControllerBase
    {
        readonly ILogger<PublishersController> _logger;
        readonly ISubscriptionProductsService _subscriptionProductService;

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
            _subscriptionProductService.TraceId = _traceId;
        }
        public QueuedSubscriptionsController(ISubscriptionProductsService subscriptionProductService, ILogger<PublishersController> logger)
        {
            _logger = logger;
            _subscriptionProductService = subscriptionProductService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetQueuedSubscriptions(int limit, string serverName)
        {
            ActivateTrace();
            //used Logtrace as this method gets called every second by adapter so by default this wont be logged
            LoggingHelper.LogTrace(_logger, LogType.Start, "GetQueuedSubscriptions", "QueuedSubscriptionsController", TraceId);
            Response<IEnumerable<QueuedSubscription>> response = new Response<IEnumerable<QueuedSubscription>>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                var result = await _subscriptionProductService.GetQueuedSubscriptions(limit,serverName);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetQueuedSubscriptions", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<IEnumerable<QueuedSubscription>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogTrace(_logger, LogType.End, "GetQueuedSubscriptions", "QueuedSubscriptionsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "GetQueuedSubscriptions", "QueuedSubscriptionsController", TraceId, ex);
                  response = new Response<IEnumerable<QueuedSubscription>>
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

       [HttpGet("{id}")]
        public async Task<IActionResult> GetQueuedSubscriptionsById(Guid subscriberProductQueuedId)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetQueuedSubscriptionsById", "QueuedSubscriptionsController", TraceId);
            Response<QueuedSubscription> response = new Response<QueuedSubscription>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                var result = await _subscriptionProductService.GetQueuedSubscriptionsById(subscriberProductQueuedId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetQueuedSubscriptionsById", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<QueuedSubscription>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetQueuedSubscriptionsById", "QueuedSubscriptionsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "GetQueuedSubscriptionsById", "QueuedSubscriptionsController", TraceId, ex);
                  response = new Response<QueuedSubscription>
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


         [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionProductQueue(Guid id)
        {
             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriptionProductQueue", "QueuedSubscriptionsController", TraceId);
            if(id.ValidateEmptyGuid())
                return BadRequest();

            
            Response<string> response = new Response<string>();             
             try
             {
                var watch = Stopwatch.StartNew();
                await _subscriptionProductService.DeleteSubscriptionProductQueue(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteSubscriptionProductQueue", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<string>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = "",
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriptionProductQueue", "QueuedSubscriptionsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteSubscriptionProductQueue", "QueuedSubscriptionsController", TraceId, ex);
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


         [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscriptionQueue(Guid id, QueuedSubscription queuedSubscriptionDataModel)
        {
            if(id.ValidateEmptyGuid() || queuedSubscriptionDataModel.ValidateObjectForNull())
                return BadRequest();

             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriptionQueue", "QueuedSubscriptionsController", TraceId);
            Response<QueuedSubscription> response = new Response<QueuedSubscription>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                queuedSubscriptionDataModel.JobId = id;
                var result = await _subscriptionProductService.UpdateSubscriptionQueue(queuedSubscriptionDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateSubscriptionQueue", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<QueuedSubscription>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriptionQueue", "QueuedSubscriptionsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateSubscriptionQueue", "QueuedSubscriptionsController", TraceId, ex);
                  response = new Response<QueuedSubscription>
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

        [HttpPost("")]
        public async Task<IActionResult> InsertSubscriptionQueue(Guid id, QueuedSubscription queuedSubscriptionDataModel)
        {
            if (id.ValidateEmptyGuid() || queuedSubscriptionDataModel.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriptionQueue", "QueuedSubscriptionsController", TraceId);
            Response<QueuedSubscription> response = new Response<QueuedSubscription>();

            try
            {
                var watch = Stopwatch.StartNew();
                queuedSubscriptionDataModel.JobId = id;
                var result = await _subscriptionProductService.InsertSubscriptionQueue(queuedSubscriptionDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertSubscriptionQueue", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<QueuedSubscription>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriptionQueue", "QueuedSubscriptionsController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertSubscriptionQueue", "QueuedSubscriptionsController", TraceId, ex);
                response = new Response<QueuedSubscription>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if (ex is TimeoutException)
                    return StatusCode(408);

                return StatusCode(500, response);
            }
        }

        [HttpPost("subscriptionHistory")]
        public async Task<IActionResult> InsertSubscriberHistory(QueuedSubscription queuedSubscriberDataModel)
        {
             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriberHistory", "QueuedSubscriptionsController", TraceId);
            if(queuedSubscriberDataModel.ValidateObjectForNull())
                return BadRequest();
            
            Response<string> response = new Response<string>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                var result = await _subscriptionProductService.InsertSubscriberProductHistory(queuedSubscriberDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertSubscriberProductHistory", "SubscriptionProductsService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<string>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = "Inserted Successfully",
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriberHistory", "QueuedSubscriptionsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "InsertSubscriberHistory", "QueuedSubscriptionsController", TraceId, ex);
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
