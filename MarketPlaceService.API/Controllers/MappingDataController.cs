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
    [Route("api/v{version:apiVersion}/datamappings")]
    [ApiVersion("1.0")]
    [ApiController]
    public class DataMappingsController : ControllerBase
    {
        private readonly ILogger<DataMappingsController> _logger;
        private readonly IMappingDataService _mappingDataService;

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
            _mappingDataService.TraceId = _traceId;
            
            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _mappingDataService.UserId = _userId;
        }


        public DataMappingsController(IMappingDataService mappingDataService, ILogger<DataMappingsController> logger,  ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _mappingDataService = mappingDataService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("{direction}/{datamappingtype}/{site}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMappedData(Entities.MappingDirection direction, int datamappingtype, Guid site)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedData", "MappingDataController", TraceId);
            Response<IEnumerable<DataMapResponse>> response = new Response<IEnumerable<DataMapResponse>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataService.GetMappedData(direction, datamappingtype, site);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMappedData", "MappingDataService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<DataMapResponse>>
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
                       MappingDataCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedData", "MappingDataController", TraceId);

                if(result == null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappedData", "MappingDataController", TraceId, ex);
                response = new Response<IEnumerable<DataMapResponse>>
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

        [HttpGet("{direction}/{datamappingtype}/{site}/{sourceid}", Name = "GetMappedDataId")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMappedDataId(Entities.MappingDirection direction, int datamappingtype, Guid site, int sourceid)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedDataId", "MappingDataController", TraceId);
            Response<DataMapResponse> response = new Response<DataMapResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataService.GetMappedData(direction, datamappingtype, site,sourceid);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMappedData", "MappingDataService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<DataMapResponse>
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
                        MappingDataCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedDataId", "MappingDataController", TraceId);

                if(result==null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappedDataId", "MappingDataController", TraceId, ex);
                response = new Response<DataMapResponse>
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

        [HttpPost("{direction}/{datamappingtype}/{site}/{sourceid}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertMappedData(Entities.MappingDirection direction, int datamappingtype, Guid site, int sourceid, DataMap request, ApiVersion apiVersion)
        {
             
            _logger.LogInformation("InsertMappedData is called.");
            Response<DataMapResponse> response = new Response<DataMapResponse>();
            try
            {
                ActivateTrace();         
                LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMappedData", "MappingDataController", TraceId);
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataService.InsertMappedData(direction, datamappingtype, site,sourceid,request);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertMappedData", "MappingDataService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<DataMapResponse>
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
                        MappingDataCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMappedData", "MappingDataController", TraceId);

                return CreatedAtRoute(nameof(GetMappedDataId), new {direction= direction, datamappingtype = datamappingtype, site = site, sourceid = sourceid, version = apiVersion.ToString() }, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertMappedData", "MappingDataController", TraceId, ex);
                response = new Response<DataMapResponse>
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

        [HttpPut("{direction}/{datamappingtype}/{site}/{sourceid}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateMappedData(Entities.MappingDirection direction, int datamappingtype, Guid site, int sourceid, DataMap request)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMappedData", "MappingDataController", TraceId);
            Response<DataMapResponse> response = new Response<DataMapResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataService.UpdateMappedData(direction, datamappingtype, site,sourceid,  request);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateMappedData", "MappingDataService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<DataMapResponse>
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
                        MappingDataCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMappedData", "MappingDataController", TraceId);

                if(result == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateMappedData", "MappingDataController", TraceId, ex);
                response = new Response<DataMapResponse>
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

        [HttpDelete("{direction}/{datamappingtype}/{site}/{sourceid}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteMappedData(Entities.MappingDirection direction, int datamappingtype, Guid site, int sourceid)
        {
             
            Response<bool> response = new Response<bool>();
            try
            {
                ActivateTrace();         
                LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMappedData", "MappingDataController", TraceId);
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataService.DeleteMappedData(direction, datamappingtype, site,sourceid);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteMappedData", "MappingDataService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<bool>
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
                        OperationResult = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMappedData", "MappingDataController", TraceId);

                if(!result)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteMappedData", "MappingDataController", TraceId, ex);
                response = new Response<bool>
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
