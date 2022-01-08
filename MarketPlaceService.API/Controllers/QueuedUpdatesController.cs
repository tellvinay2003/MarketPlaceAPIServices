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
    [Route("api/v{version:apiVersion}/queuedUpdates")]
    [ApiVersion("1.0")]
    [ApiController]
    public class QueuedUpdatesController : ControllerBase
    {
         readonly ILogger<PublishersController> _logger;

         private readonly IQueuedUpdateService _iQueuedUpdateService;

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
            _iQueuedUpdateService.TraceId = _traceId;
        }

         public QueuedUpdatesController(IQueuedUpdateService iQueuedUpdateService,ILogger<PublishersController> logger)
         {
            _logger = logger;
            _iQueuedUpdateService=iQueuedUpdateService;
         }


        [HttpGet("")]
        [EnableCors("odlPolicy")]
         public async Task<IActionResult> GetQueuedUpdates(int limit,string serverName)
        {
            Response<IEnumerable<SubscriberProductTsUpdateQueue>> response = new Response<IEnumerable<SubscriberProductTsUpdateQueue>>();
            try
            {
                ActivateTrace();
                //used Logtrace as this method gets called every second by adapter so by default this wont be logged
                LoggingHelper.LogTrace(_logger, LogType.Start, "GetQueuedUpdates", "QueuedUpdatesController", TraceId);
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _iQueuedUpdateService.GetQueuedUpdates(limit,serverName);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetQueuedUpdates", "QueuedUpdateService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SubscriberProductTsUpdateQueue>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogTrace(_logger, LogType.End, "GetQueuedUpdates", "QueuedUpdatesController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetQueuedUpdates", "QueuedUpdatesController", TraceId, ex);
                response = new Response<IEnumerable<SubscriberProductTsUpdateQueue>>
                {
                    ResponseCode = (int)Code.success,
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
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetQueuedUpdate(Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetQueuedUpdate", "QueuedUpdatesController", TraceId);
            Response<SubscriberProductTsUpdateQueue> response = new Response<SubscriberProductTsUpdateQueue>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _iQueuedUpdateService.GetQueuedUpdate(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetQueuedUpdate", "QueuedUpdateService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetQueuedUpdate", "QueuedUpdatesController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetQueuedUpdate", "QueuedUpdatesController", TraceId, ex);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.success,
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
        [EnableCors("odlPolicy")]
         public async Task<IActionResult> InsertQueuedUpdate(SubscriberProductTsUpdateQueue request)

        {
            if(request.ValidateObjectForNull()) 
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertQueuedUpdate", "QueuedUpdatesController", TraceId);
            Response<SubscriberProductTsUpdateQueue> response = new Response<SubscriberProductTsUpdateQueue>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _iQueuedUpdateService.InsertQueuedUpdate(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertQueuedUpdate", "QueuedUpdateService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertQueuedUpdate", "QueuedUpdatesController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertQueuedUpdate", "QueuedUpdatesController", TraceId, ex);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.success,
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
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateQueuedUpdate(Guid id,SubscriberProductTsUpdateQueue request)
        {
            if(id.ValidateEmptyGuid() || request.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateQueuedUpdate", "QueuedUpdatesController", TraceId);
            Response<SubscriberProductTsUpdateQueue> response = new Response<SubscriberProductTsUpdateQueue>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _iQueuedUpdateService.UpdateQueuedUpdate(id, request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateQueuedUpdate", "QueuedUpdateService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateQueuedUpdate", "QueuedUpdatesController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateQueuedUpdate", "QueuedUpdatesController", TraceId, ex);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.success,
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
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteQueuedUpdate(Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteQueuedUpdate", "QueuedUpdatesController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _iQueuedUpdateService.DeleteQueuedUpdate(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteQueuedUpdate", "QueuedUpdateService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteQueuedUpdate", "QueuedUpdatesController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteQueuedUpdate", "QueuedUpdatesController", TraceId, ex);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPost("queuedUpdateHistory")]
        [EnableCors("odlPolicy")]
         public async Task<IActionResult> InsertQueuedUpdateHistory(SubscriberProductTsUpdateQueue request)

        {
            if(request.ValidateObjectForNull()) 
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertQueuedUpdateHistory", "QueuedUpdatesController", TraceId);
            Response<SubscriberProductTsUpdateQueue> response = new Response<SubscriberProductTsUpdateQueue>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                request.TraceId=TraceId;
                var result = await _iQueuedUpdateService.InsertQueuedUpdateHistory(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertQueuedUpdateHistory", "QueuedUpdateService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertQueuedUpdateHistory", "QueuedUpdatesController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertQueuedUpdateHistory", "QueuedUpdatesController", TraceId, ex);
                response = new Response<SubscriberProductTsUpdateQueue>
                {
                    ResponseCode = (int)Code.success,
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
