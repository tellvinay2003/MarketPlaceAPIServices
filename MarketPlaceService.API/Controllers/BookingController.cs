using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.API.Utilities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private Guid _traceId;
        private readonly ILogger<PricingController> _logger;
        private readonly IRequestResponseLoggingHelper _requestResponseLogger;
        private readonly IBookingService _bookingService;
        private const string CONTROLLER_NAME = "BookingController";
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

        public BookingController(IBookingService bookingService, ILogger<PricingController> logger, IRequestResponseLoggingHelper requestResponseLogger)
        {
            _requestResponseLogger = requestResponseLogger;
            _bookingService = bookingService;
            _logger = logger;
        }

        [HttpPost("QueueSubscriberBooking")]
        public async Task<ActionResult<QueueSubscriberBookingResponse>> QueueSubscriberBooking(QueueSubscriberBookingRequest request)
        {
            var response = new Response<QueueSubscriberBookingResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "QueueSubscriberBooking", CONTROLLER_NAME, TraceId);
                _requestResponseLogger.LogRequest<QueueSubscriberBookingRequest>(request, "QueueSubscriberBooking", CONTROLLER_NAME, HttpContext.Request.Path);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.QueueSubscriberBooking(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "QueueSubscriberBooking", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<QueueSubscriberBookingResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<QueueSubscriberBookingResponse>>(response, "QueueSubscriberBooking", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "QueueSubscriberBooking", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "QueueSubscriberBooking", CONTROLLER_NAME, TraceId, ex);
                response = new Response<QueueSubscriberBookingResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<QueueSubscriberBookingResponse>>(response, "QueueSubscriberBooking", CONTROLLER_NAME, HttpContext.Request.Path);
                return StatusCode(500, response);
            }

        }

        [HttpPost("ProcessSubscriberBooking")]
        public async Task<ActionResult<ProcessSubscriberBookingResponse>> ProcessSubscriberBooking(ProcessSubscriberBookingRequest request)
        {
            var response = new Response<ProcessSubscriberBookingResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessSubscriberBooking", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.ProcessSubscriberBooking(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "ProcessSubscriberBooking", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<ProcessSubscriberBookingResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = result.IsSuccess ? "Success" : "Failure",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId,
                    Message = result.IsSuccess ? string.Empty : result.ErrorText
                };


                LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSubscriberBooking", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessSubscriberBooking", CONTROLLER_NAME, TraceId, ex);
                response = new Response<ProcessSubscriberBookingResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }

        [HttpPost("ProcessSiteBooking")]
        public async Task<ActionResult<ProcessSiteBookingResponse>> ProcessSiteBooking(ProcessSiteBookingRequest request)
        {
            var response = new Response<ProcessSiteBookingResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessSiteBooking", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.ProcessSiteBooking(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "ProcessSiteBooking", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<ProcessSiteBookingResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSiteBooking", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessSiteBooking", CONTROLLER_NAME, TraceId, ex);
                response = new Response<ProcessSiteBookingResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }

        [HttpPost("ProcessSubscriberCallback")]
        public async Task<ActionResult<ProcessSubscriberCallbackResponse>> ProcessSubscriberCallback(ProcessSubscriberCallbackRequest request)
        {
            var response = new Response<ProcessSubscriberCallbackResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessSubscriberCallback", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.ProcessSubscriberCallback(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "ProcessSubscriberCallback", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<ProcessSubscriberCallbackResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "ProcessSubscriberCallback", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessSubscriberCallback", CONTROLLER_NAME, TraceId, ex);
                response = new Response<ProcessSubscriberCallbackResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }
        }

        [HttpPost("ProcessBookingUpdateFromPublisher")]
        public async Task<ActionResult<ProcessBookingUpdateFromPublisherResponse>> ProcessBookingUpdateFromPublisher(ProcessBookingUpdateFromPublisherRequest request)
        {
            var response = new Response<ProcessBookingUpdateFromPublisherResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "ProcessBookingUpdateFromPublisher", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.ProcessBookingUpdateFromPublisher(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "ProcessBookingUpdateFromPublisher", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<ProcessBookingUpdateFromPublisherResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "ProcessBookingUpdateFromPublisher", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ProcessBookingUpdateFromPublisher", CONTROLLER_NAME, TraceId, ex);
                response = new Response<ProcessBookingUpdateFromPublisherResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }

        /* [HttpGet("GetBookingReference")]
        public async Task<ActionResult<IEnumerable<string>>> GetBookingReference(string bookingReference,int limit)
        {
            var response = new Response<IEnumerable<string>>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingReference", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetBookingReference(bookingReference,limit);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetBookingReference", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<string>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingReference", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingReference", CONTROLLER_NAME, TraceId, ex);
                response = new Response<IEnumerable<string>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };
                return StatusCode(500, response);
            }

        }
 */
        [HttpPost("BookingSearch")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> BookingSearch(BookingSearchRequest request)
        {
            // if(request.)
            //   return BadRequest();
            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "BookingSearch", CONTROLLER_NAME, TraceId);
            _requestResponseLogger.LogRequest<BookingSearchRequest>(request, "BookingSearch", CONTROLLER_NAME, HttpContext.Request.Path);
            var response = new Response<BookingSearchResponse>();
            var objectResult = new List<PublishedProductsDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.BookingSearch(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "BookingSearch", CONTROLLER_NAME, TraceId, watch.ElapsedMilliseconds);

                response = new Response<BookingSearchResponse> { ResponseCode = (int)Code.success, ResponseMessage = result, TraceId = TraceId };
                _requestResponseLogger.LogResponse<Response<BookingSearchResponse>>(response, "BookingSearch", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "BookingSearch", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "BookingSearch", CONTROLLER_NAME, TraceId, ex);
                response = new Response<BookingSearchResponse> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(), TraceId = TraceId };
                _requestResponseLogger.LogResponse<Response<BookingSearchResponse>>(response, "BookingSearch", CONTROLLER_NAME, HttpContext.Request.Path);
                if (ex is TimeoutException)
                    return StatusCode(408);

                return StatusCode(500, response);
            }
        }

        [HttpGet("BookingStatuses")]
        [EnableCors("odlPolicy")]

        public async Task<ActionResult<GetBookingStatusResponse>> GetBookingStatus()
        {            
            var response = new Response<GetBookingStatusResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingStatus", CONTROLLER_NAME, TraceId);
                _requestResponseLogger.LogRequest<string>("", "GetBookingStatus", CONTROLLER_NAME, HttpContext.Request.Path);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetBookingStatus();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetBookingStatus", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetBookingStatusResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<GetBookingStatusResponse>>(response, "GetBookingStatus", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingStatus", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingStatus", CONTROLLER_NAME, TraceId, ex);
                response = new Response<GetBookingStatusResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<GetBookingStatusResponse>>(response, "GetBookingStatus", CONTROLLER_NAME, HttpContext.Request.Path);
                if (ex is TimeoutException)
                    return StatusCode(408, response);

                return StatusCode(500, response);
            }

        }

        [HttpPost("GetMpSubscriberBookingInfo")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<GetMpBookingInfoResponse>> GetMpSubscriberBookingInfo(GetMpBookingInfoRequest request)
        {
            if (request == null || request.BookingId.ValidateEmptyGuid())
                return BadRequest();
            var response = new Response<GetMpBookingInfoResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpSubscriberBookingInfo", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetMpSubscriberBookingInfo(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMpSubscriberBookingInfo", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetMpBookingInfoResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMpSubscriberBookingInfo", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMpSubscriberBookingInfo", CONTROLLER_NAME, TraceId, ex);
                response = new Response<GetMpBookingInfoResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };

                if (ex is TimeoutException)
                    return StatusCode(408, response);
                return StatusCode(500, response);
            }

        }


        /// <summary>
        /// Returns a list of publisher booking(sitebooking) records for a particular subscriber booking(marketeplacebooking)
        /// </summary>
        /// <param name="request">
        /// MarketPlace booking id needs to be passed as BookingId
        /// 
        /// </param>
        /// <returns></returns>


        [HttpPost("GetMpPublisherBookingInfo")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<GetMpBookingInfoResponse>> GetMpPublisherBookingInfo(GetMpBookingInfoRequest request)
        {
            if (request == null || request.BookingId.ValidateEmptyGuid())
                return BadRequest();
            var response = new Response<GetMpBookingInfoResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpPublisherBookingInfo", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetMpPublisherBookingInfo(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMpPublisherBookingInfo", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetMpBookingInfoResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMpPublisherBookingInfo", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMpPublisherBookingInfo", CONTROLLER_NAME, TraceId, ex);
                response = new Response<GetMpBookingInfoResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };

                if (ex is TimeoutException)
                    return StatusCode(408, response);
                return StatusCode(500, response);
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetMpSubscriberBookingHistoryInfo")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<GetMpBookingHistoryInfoResponse>> GetMpSubscriberBookingHistoryInfo(GetMpBookingHistoryInfoRequest request)
        {
            if (request == null || request.BookingId.ValidateEmptyGuid())
                return BadRequest();
            var response = new Response<GetMpBookingHistoryInfoResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpSubscriberBookingHistoryInfo", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetMpSubscriberBookingHistoryInfo(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMpSubscriberBookingHistoryInfo", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetMpBookingHistoryInfoResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMpSubscriberBookingHistoryInfo", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMpSubscriberBookingHistoryInfo", CONTROLLER_NAME, TraceId, ex);
                response = new Response<GetMpBookingHistoryInfoResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };

                if (ex is TimeoutException)
                    return StatusCode(408, response);
                return StatusCode(500, response);
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetMpPublisherBookingHistoryInfo")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<GetMpBookingHistoryInfoResponse>> GetMpPublisherBookingHistoryInfo(GetMpBookingHistoryInfoRequest request)
        {
            if (request == null || request.BookingId.ValidateEmptyGuid())
                return BadRequest();
            var response = new Response<GetMpBookingHistoryInfoResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpPublisherBookingHistoryInfo", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetMpPublisherBookingHistoryInfo(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMpPublisherBookingHistoryInfo", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetMpBookingHistoryInfoResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMpPublisherBookingHistoryInfo", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMpPublisherBookingHistoryInfo", CONTROLLER_NAME, TraceId, ex);
                response = new Response<GetMpBookingHistoryInfoResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };

                if (ex is TimeoutException)
                    return StatusCode(408, response);
                return StatusCode(500, response);
            }

        }

        [HttpPost("GetMpBookingJsonData")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<GetMpBookingJsonDataResponse>> GetMpBookingJsonData(GetMpBookingJsonDataRequest request)
        {
            if (request == null || ((request.RowId == null || (request.RowId ?? new Guid()).ValidateEmptyGuid()) && (request.BookingId == null || (request.BookingId ?? new Guid()).ValidateEmptyGuid())) || !Enum.IsDefined(typeof(EntityType), request.EntityType))
                return BadRequest();
            var response = new Response<GetMpBookingJsonDataResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetMpBookingJsonData", CONTROLLER_NAME, TraceId);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetMpBookingJsonData(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMpBookingJsonData", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetMpBookingJsonDataResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMpBookingJsonData", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMpBookingJsonData", CONTROLLER_NAME, TraceId, ex);
                response = new Response<GetMpBookingJsonDataResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };

                if (ex is TimeoutException)
                    return StatusCode(408, response);
                return StatusCode(500, response);
            }

        }

        [HttpPost("GetServiceApplicableRules")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<ApplicableRulesResponse>> GetServiceApplicableRules(ApplicableRulesRequest request)
        {
            if (request == null)
                return BadRequest();
            var response = new Response<ApplicableRulesResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceApplicableRules", CONTROLLER_NAME, TraceId);
                _requestResponseLogger.LogRequest<ApplicableRulesRequest>(request, "GetServiceApplicableRules", CONTROLLER_NAME, HttpContext.Request.Path);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetServiceApplicableRules(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServiceApplicableRules", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<ApplicableRulesResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<ApplicableRulesResponse>>(response, "GetServiceApplicableRules", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceApplicableRules", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetServiceApplicableRules", CONTROLLER_NAME, TraceId, ex);
                response = new Response<ApplicableRulesResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<ApplicableRulesResponse>>(response, "GetServiceApplicableRules", CONTROLLER_NAME, HttpContext.Request.Path);
                if (ex is TimeoutException)
                    return StatusCode(408, response);
                return StatusCode(500, response);
            }

        }

        [HttpPost("GetServiceRules")]
        [EnableCors("odlPolicy")]
        public async Task<ActionResult<ServiceRuleResponse>> GetServiceRules(ServiceRuleRequest request)
        {
            if (request == null)
                return BadRequest();
            var response = new Response<ServiceRuleResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceRules", CONTROLLER_NAME, TraceId);
                _requestResponseLogger.LogRequest<ServiceRuleRequest>(request, "GetServiceRules", CONTROLLER_NAME, HttpContext.Request.Path);
                var watch = Stopwatch.StartNew();
                var result = await _bookingService.GetServiceRules(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServiceRules", "BookingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<ServiceRuleResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<ServiceRuleResponse>>(response, "GetServiceRules", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceRules", CONTROLLER_NAME, TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetServiceRules", CONTROLLER_NAME, TraceId, ex);
                response = new Response<ServiceRuleResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.Message,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<ServiceRuleResponse>>(response, "GetServiceRules", CONTROLLER_NAME, HttpContext.Request.Path);
                if (ex is TimeoutException)
                    return StatusCode(408, response);
                return StatusCode(500, response);
            }

        }

    }
}