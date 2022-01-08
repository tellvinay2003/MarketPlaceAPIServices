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

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/masterdataconfig")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MasterDataConfigController : ControllerBase
    {
        private readonly ILogger<MasterDataConfigController> _logger;
        private readonly IMasterDataConfigService _masterDataConfigService;

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
            _masterDataConfigService.TraceId = _traceId;
        }

        public MasterDataConfigController(IMasterDataConfigService masterDataConfigService, ILogger<MasterDataConfigController> logger,  ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _masterDataConfigService = masterDataConfigService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataConfig()
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataConfig", "MasterDataConfigController", TraceId);
            Response<IEnumerable<MasterDataConfig>> response = new Response<IEnumerable<MasterDataConfig>>();
            try
            {
                _logger.LogInformation("MasterDataConfigService service is called.");
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataConfigService.GetMasterDataConfig();               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataConfig", "MasterDataConfigService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MasterDataConfig>>
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
                        MasterDataConfigCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataConfig", "MasterDataConfigController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataConfig", "MasterDataConfigController", TraceId, ex);
                response = new Response<IEnumerable<MasterDataConfig>>
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


        [HttpGet("{id}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataConfig(int id)
        {
            if(id.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataConfig", "MasterDataConfigControler", TraceId);
            Response<MasterDataConfig> response = new Response<MasterDataConfig>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataConfigService.GetMasterDataConfig(id);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataConfig", "MasterDataConfigService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataConfig>
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
                        MasterDataConfigCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataConfig", "MasterDataConfigControler", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataConfig", "MasterDataConfigControler", TraceId, ex);
                response = new Response<MasterDataConfig>
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
        
    }
}
