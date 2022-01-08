using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonUtilities;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.TSv2ApiEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class PricingController : ControllerBase
    {
        private Guid _traceId;
        private readonly ILogger<PricingController> _logger;
        private readonly IRequestResponseLoggingHelper _requestResponseLogger;
        private readonly IPricingService _pricingService;
        private const string CONTROLLER_NAME = "PricingController";
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

        public PricingController(IPricingService pricingService, ILogger<PricingController> logger, IRequestResponseLoggingHelper requestResponseLogger)
        {
            _requestResponseLogger = requestResponseLogger;
            _pricingService = pricingService;
            _logger = logger;
        }
        [HttpPost("GetServicePricesFromTs")]
        public async Task<ActionResult<GetServicePricesResponse>> GetServicePricesFromTs(GetServicePricesRequest request)
        {
            var response = new Response<GetServicePricesResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetServicePricesFromTs", "PricingController", TraceId);
                _requestResponseLogger.LogRequest<GetServicePricesRequest>(request, "GetServicePricesFromTs", CONTROLLER_NAME, HttpContext.Request.Path);
                var watch = Stopwatch.StartNew();               
                var result = await _pricingService.GetServicePricesFromTs(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServicePricesFromTs", "PricingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetServicePricesResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<GetServicePricesResponse>>(response, "GetServicePricesFromTs", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetServicePricesFromTs", "PricingController", TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetServicePricesFromTs", "PricingController", TraceId, ex);
                response = new Response<GetServicePricesResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<GetServicePricesResponse>>(response, "GetServicePricesFromTs", CONTROLLER_NAME, HttpContext.Request.Path);
                return StatusCode(500, response);
            }
        }

        [HttpPost("GetServiceExtraPrices")]
        public async Task<ActionResult<GetServiceExtraPricesResponse>> GetServiceExtraPrices(GetServiceExtraPricesRequest request)
        {
            var response = new Response<GetServiceExtraPricesResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceExtraPrices", "PricingController", TraceId);
                _requestResponseLogger.LogRequest<GetServiceExtraPricesRequest>(request, "GetServiceExtraPrices", CONTROLLER_NAME, HttpContext.Request.Path);
                var watch = Stopwatch.StartNew();
                var result = await _pricingService.GetServiceExtraPrices(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServiceExtraPrices", "PricingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<GetServiceExtraPricesResponse>
                {
                    ResponseCode =(int) Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<GetServiceExtraPricesResponse>>(response, "GetServiceExtraPrices", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceExtraPrices", "PricingController", TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetServiceExtraPrices", "PricingController", TraceId, ex);
                response = new Response<GetServiceExtraPricesResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<GetServiceExtraPricesResponse>>(response, "GetServiceExtraPrices", CONTROLLER_NAME, HttpContext.Request.Path);
                return StatusCode(500, response);
            }
        }

        [HttpPost("GetBookingPrices")]
        public async Task<ActionResult<CalculateBookingPriceResponse>> GetBookingPrices(CalculateBookingPriceRequest request)
        {
            var response = new Response<CalculateBookingPriceResponse>();
            try
            {
                ActivateTrace();
                LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingPrices", "PricingController", TraceId);
                _requestResponseLogger.LogRequest<CalculateBookingPriceRequest>(request, "GetBookingPrices", CONTROLLER_NAME, HttpContext.Request.Path);
                var watch = Stopwatch.StartNew();
                var result = await _pricingService.GetBookingPrice(request);
                //var priceResult = JsonConvert.DeserializeObject<Response<CalculateBookingPriceResponse>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetBookingPrices", "PricingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<CalculateBookingPriceResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<CalculateBookingPriceResponse>>(response, "GetBookingPrices", CONTROLLER_NAME, HttpContext.Request.Path);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingPrices", "PricingController", TraceId);
                return StatusCode(200, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingPrices", CONTROLLER_NAME, TraceId, ex);
                response = new Response<CalculateBookingPriceResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                _requestResponseLogger.LogResponse<Response<CalculateBookingPriceResponse>>(response, "GetBookingPrices", CONTROLLER_NAME, HttpContext.Request.Path);
                return StatusCode(500, response);
            }
        }
    }
}
