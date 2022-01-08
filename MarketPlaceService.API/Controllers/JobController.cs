using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.API.Utilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Job;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;


namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class JobController : ControllerBase
    {

        private Guid _traceId;
        private readonly ILogger<JobController> _logger;
        private readonly IJobService _jobService;
        private const string CONTROLLER_NAME = "JobController";

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
        }

        public JobController(IJobService jobService, ILogger<JobController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }



        [HttpGet("BusinessProcess")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<List<BusinessProcessDataModel>>> GetBusinessProcess()
        {
            const string METHOD_NAME = "GetBusinessProcess";
            var response = new Response<List<BusinessProcessDataModel>>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, METHOD_NAME, CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _jobService.GetBusinessProcess();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, METHOD_NAME, "JobService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<List<BusinessProcessDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, METHOD_NAME, CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, METHOD_NAME, CONTROLLER_NAME, TraceId, ex);
                response = new Response<List<BusinessProcessDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }

        [HttpGet("BusinessProcessQueue/{businessProcess}", Name = "BusinessProcessQueue")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<List<QueueDataModel>>> GetBusinessProcessQueue(BusinessProcess businessProcess)
        {
            const string METHOD_NAME = "GetBusinessProcessQueue";
            var response = new Response<List<QueueDataModel>>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, METHOD_NAME, CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _jobService.GetBusinessProcessQueue(businessProcess);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, METHOD_NAME, "JobService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<List<QueueDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, METHOD_NAME, CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, METHOD_NAME, CONTROLLER_NAME, TraceId, ex);
                response = new Response<List<QueueDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }




        [HttpGet("JobStatus/{forCurrentJobs}", Name = "JobStatus")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<List<JobStatusDataModel>>> GetJobStatus(bool forCurrentJobs)
        {
            const string METHOD_NAME = "GetJobStatus";
            var response = new Response<List<JobStatusDataModel>>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, METHOD_NAME, CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _jobService.GetJobStatus(forCurrentJobs);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, METHOD_NAME, "JobService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<List<JobStatusDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, METHOD_NAME, CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, METHOD_NAME, CONTROLLER_NAME, TraceId, ex);
                response = new Response<List<JobStatusDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }



        [HttpPost("JobSearch")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<List<JobSearchResponse>>> SearchJob(JobSearchRequest request)
        {
            const string METHOD_NAME = "SearchJob";
            var response = new Response<JobSearchResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, METHOD_NAME, CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _jobService.SearchJob(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, METHOD_NAME, "JobService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<JobSearchResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, METHOD_NAME, CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, METHOD_NAME, CONTROLLER_NAME, TraceId, ex);
                response = new Response<JobSearchResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }


        [HttpPost("JobInfo")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<JobInfoResponse>> JobInfo(JobInfoRequest request)
        {
            const string METHOD_NAME = "JobInfo";
            var response = new Response<JobInfoResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, METHOD_NAME, CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _jobService.JobInfo(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, METHOD_NAME, "JobService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<JobInfoResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, METHOD_NAME, CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, METHOD_NAME, CONTROLLER_NAME, TraceId, ex);
                response = new Response<JobInfoResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }


        [HttpPost("ResubmitJob")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<ResubmitJobResponse>> ResubmitJob(ResubmitJobRequest request)
        {
            const string METHOD_NAME = "ResubmitJob";
            var response = new Response<ResubmitJobResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, METHOD_NAME, CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _jobService.ResubmitJob(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, METHOD_NAME, "JobService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<ResubmitJobResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, METHOD_NAME, CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, METHOD_NAME, CONTROLLER_NAME, TraceId, ex);
                response = new Response<ResubmitJobResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }
    }
}
