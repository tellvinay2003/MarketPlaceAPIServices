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
using Microsoft.AspNetCore.Http;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/mappingdataconfiguration")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MappingDataConfigurationController :ControllerBase
    {
        private readonly ILogger<MappingDataConfigurationController> _logger;
        private readonly IMappingDataConfigService _mappingDataConfigService;
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
            _mappingDataConfigService.TraceId = _traceId;
        }

        public MappingDataConfigurationController(ILogger<MappingDataConfigurationController> logger, IMappingDataConfigService mappingDataConfigService, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _mappingDataConfigService = mappingDataConfigService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("{direction}/{site}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMappingDataConfig(Entities.MappingDirection direction, Guid site)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappingDataConfig", "MappingDataConfigController", TraceId);
            Response<IEnumerable<MappingDataConfig>> response = new Response<IEnumerable<MappingDataConfig>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataConfigService.GetMappingDataConfig(direction, site);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMappingDataConfig", "MappingDataConfigService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MappingDataConfig>>
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
                        MappingDataConfigCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMappingDataConfig", "MappingDataConfigController", TraceId);

                if(result==null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappingDataConfig", "MappingDataConfigController", TraceId, ex);
                response = new Response<IEnumerable<MappingDataConfig>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
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

        [HttpGet("{direction}/{datamappingtype}/{site}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMappingDataConfig(Entities.MappingDirection direction, ushort datamappingtype, Guid site)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappingDataConfig", "MappingDataConfigController", TraceId);
            Response<MappingDataConfig> response = new Response<MappingDataConfig>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataConfigService.GetMappingDataConfig(direction,datamappingtype, site);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMappingDataConfig", "MappingDataConfigService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MappingDataConfig>
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
                        MappingDataConfigCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMappingDataConfig", "MappingDataConfigController", TraceId);

                if(result == null)
                    return NotFound(new Response<MappingDataConfig>{ResponseCode = (int)Code.NotFound});

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappingDataConfig", "MappingDataConfigController", TraceId, ex);
                response = new Response<MappingDataConfig>
                {
                    ResponseCode = (int)Code.ServerError,
                    Status = "Failure",
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
