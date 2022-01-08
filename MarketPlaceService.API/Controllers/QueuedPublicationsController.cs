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
    [Route("api/v{version:apiVersion}/queuedPublications")]
    [ApiVersion("1.0")]
    [ApiController]
    public class QueuedPublicationsController : ControllerBase
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
        private void ActivateTrace()
        {
            if (_traceId == Guid.Empty)
                _traceId = Request.Headers.ContainsKey("TraceId") ? Guid.Parse(Request.Headers["TraceId"]) : Guid.NewGuid();
            _profileService.TraceId = _traceId;
        }
        public QueuedPublicationsController(IProfileService profileService, ILogger<PublishersController> logger)
        {
            _logger = logger;
            _profileService = profileService;
        }
        

        #region Publication Queue
        [HttpGet("")]
        public async Task<IActionResult> GetQueuedPublications(int limit, string serverName)
        {
             ActivateTrace();
            //used Logtrace as this method gets called every second by adapter so by default this wont be logged
            LoggingHelper.LogTrace(_logger, LogType.Start, "GetQueuedPublications", "QueuedPublicationsController", TraceId);
            Response<IEnumerable<QueuedPublication>> response = new Response<IEnumerable<QueuedPublication>>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                var result = await _profileService.GetQueuedPublications(limit,serverName);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetQueuedPublications", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<IEnumerable<QueuedPublication>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogTrace(_logger, LogType.End, "GetQueuedPublications", "QueuedPublicationsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "GetQueuedPublications", "QueuedPublicationsController", TraceId, ex);
                  response = new Response<IEnumerable<QueuedPublication>>
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
        public async Task<IActionResult> UpdatePublicationQueue(Guid id, QueuedPublication queuedPublicationDataModel)
        {
            if(id.ValidateEmptyGuid() || queuedPublicationDataModel.ValidateObjectForNull())
                return BadRequest();

             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdatePublicationQueue", "QueuedPublicationsController", TraceId);
            Response<QueuedPublication> response = new Response<QueuedPublication>();
             
             try
             {
                var watch = Stopwatch.StartNew();
                queuedPublicationDataModel.JobId = id;
                var result = await _profileService.UpdatePublicationQueue(queuedPublicationDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdatePublicationQueue", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<QueuedPublication>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdatePublicationQueue", "QueuedPublicationsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "UpdatePublicationQueue", "QueuedPublicationsController", TraceId, ex);
                  response = new Response<QueuedPublication>
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
        

        [HttpPost("publicationHistory")]
        public async Task<IActionResult> InsertPublicationHistory(QueuedPublication queuedPublicationDataModel)
        {
            if(queuedPublicationDataModel.ValidateObjectForNull())
                return BadRequest();

             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertPublicationHistory", "QueuedPublicationsController", TraceId);
            Response<string> response = new Response<string>();
             
             try
             {
                var watch = Stopwatch.StartNew();                
                var result = await _profileService.InsertPublicationHistory(queuedPublicationDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertPublicationHistory", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<string>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = "Inserted Successfully",
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertPublicationHistory", "QueuedPublicationsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "InsertPublicationHistory", "QueuedPublicationsController", TraceId, ex);
                  response = new Response<string>
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

        [HttpPost("")]
        public async Task<IActionResult> InsertPublicationQueue(QueuedPublication queuedPublicationDataModel)
        {
            if (queuedPublicationDataModel.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertPublicationQueue", "QueuedPublicationsController", TraceId);
            Response<QueuedPublication> response = new Response<QueuedPublication>();

            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _profileService.InsertPublicationQueue(queuedPublicationDataModel);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertPublicationHistory", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<QueuedPublication>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertPublicationHistory", "QueuedPublicationsController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertPublicationHistory", "QueuedPublicationsController", TraceId, ex);
                response = new Response<QueuedPublication>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if (ex is TimeoutException)
                    return StatusCode(408);

                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublishedProductQueue(Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();

             ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeletePublishedProductQueue", "QueuedPublicationsController", TraceId);
            Response<string> response = new Response<string>();             
             try
             {
                var watch = Stopwatch.StartNew();
                await _profileService.DeletePublishedProductQueue(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeletePublishedProductQueue", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                 response = new Response<string>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = "",
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeletePublishedProductQueue", "QueuedPublicationsController", TraceId);
                return Ok(response);
             }
             catch(Exception ex)
             {
                  LoggingHelper.LogError(_logger, ExceptionType.System, "DeletePublishedProductQueue", "QueuedPublicationsController", TraceId, ex);
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

        // //TODO move this block 
        [HttpPost("InsertStaticDataUpdateQueue")]
        public async Task<IActionResult> InsertStaticDataUpdateQueue(StaticDataUpdateQueueRequest insertStaticDataUpdateQueueRequest)
        {
            if (insertStaticDataUpdateQueueRequest.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertStaticDataUpdateQueue", "QueuedPublicationsController", TraceId);
            Response<QueuedPublication> response = new Response<QueuedPublication>();

            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _profileService.InsertStaticDataUpdateQueue(insertStaticDataUpdateQueueRequest);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertStaticDataUpdateQueue", "ProfileService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<QueuedPublication>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertStaticDataUpdateQueue", "QueuedPublicationsController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertStaticDataUpdateQueue", "QueuedPublicationsController", TraceId, ex);
                response = new Response<QueuedPublication>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if (ex is TimeoutException)
                    return StatusCode(408);

                return StatusCode(500);
            }
        }
        #endregion
    }
}
