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
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using System.Linq;
using MarketPlaceService.API.Utilities;
using CommonUtilities;
using Newtonsoft.Json.Linq;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/changehistory")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ChangeHistoryController : ControllerBase
    {
        private readonly ILogger<ChangeHistoryController> _logger;
        private readonly IChangeHistoryService _changeHistoryService;
        private readonly ITransactionLoggerService<MarketplaceDataModel> _transactionLoggerService;

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
            _changeHistoryService.TraceId = _traceId;
        }

        public ChangeHistoryController(IChangeHistoryService changeHistoryService, ILogger<ChangeHistoryController> logger, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _changeHistoryService = changeHistoryService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("{dataType}/{origin}/{site}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site)
        {
             if(dataType.ValidateInteger())
                return BadRequest();
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetChangeHistory", "ChangeHistoryController", TraceId);
            Response<IEnumerable<ChangeHistory>> response = new Response<IEnumerable<ChangeHistory>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _changeHistoryService.GetChangeHistory(dataType, origin, site);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetChangeHistory", "ChangeHistoryService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<ChangeHistory>>
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
                        ChangeHistoryCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetChangeHistory", "ChangeHistoryController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetChangeHistory", "ChangeHistoryController", TraceId, ex);
                response = new Response<IEnumerable<ChangeHistory>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

             await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        Error = ex
                    },
                    TransactionStatus = "Error",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }
        
        [HttpGet("{dataType}/{origin}/{site}/{pagenmber}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetChangeHistory(int dataType, HistoryOrigin origin, Guid site, int pagenumber)
        {
            if(dataType.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetChangeHistory", "ChangeHistoryController", TraceId);
            Response<IEnumerable<ChangeHistory>> response = new Response<IEnumerable<ChangeHistory>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _changeHistoryService.GetChangeHistory(dataType, origin, site, pagenumber);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetChangeHistory", "ChangeHistoryService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<ChangeHistory>>
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
                        ChangeHistoryCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetChangeHistory", "ChangeHistoryController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetChangeHistory", "ChangeHistoryController", TraceId, ex);
                response = new Response<IEnumerable<ChangeHistory>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        Error = ex
                    },
                    TransactionStatus = "Error",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }



        //[HttpGet("GetComparisonResult/")]
        //public Task<JObject> GetComparisonResult([FromQuery] string updatedData, [FromQuery] string previousData)
        //{
        //    // Delete this
        //    string inputJsonPrev = System.IO.File.ReadAllText(@"C:\Users\vinay\Downloads\Samples\MarketplaceAppSampleJson_New.Json");
        //    string inputJsonNew = System.IO.File.ReadAllText(@"C:\Users\vinay\Downloads\Samples\MarketplaceAppSampleJson_Old.Json");

        //    JObject odlJson = JObject.Parse(inputJsonPrev);
        //    JObject newJson = JObject.Parse(inputJsonNew);

        //    var result = JsonUtility.GetDiffJson(odlJson, newJson);
        //    return Task.FromResult(result);
        //}


    }
}
