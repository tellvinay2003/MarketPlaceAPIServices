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
using MarketPlaceService.BLL;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/mappingdataratings")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MappingDataRatingsController : ControllerBase
    {
        private readonly ILogger<MappingDataRatingsController> _logger;
        private readonly IMappingDataRatingService _mappingDataRatingService;

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
            _mappingDataRatingService.TraceId = _traceId;
            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _mappingDataRatingService.UserId = _userId;
        }

        public MappingDataRatingsController(IMappingDataRatingService mappingDataRatingService, ILogger<MappingDataRatingsController> logger,  ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _mappingDataRatingService = mappingDataRatingService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("{direction}/{site}/{ratingType}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMappedData(Entities.MappingDirection direction, Guid site, int ratingType)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedData", "MappingDataRatingController", TraceId);
            Response<IEnumerable<DataMapResponse>> response = new Response<IEnumerable<DataMapResponse>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataRatingService.GetMappedData(direction, site, ratingType);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMappedData", "MappingDataRatingService", TraceId, watch.ElapsedMilliseconds);
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
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedData", "MappingDataRatingController", TraceId);

                if(result ==null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappedData", "MappingDataRatingController", TraceId, ex);
                response = new Response<IEnumerable<DataMapResponse>>
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

        [HttpGet("{direction}/{site}/{ratingtype}/{sourceid}", Name="GetMappedData")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMappedData(Entities.MappingDirection direction, Guid site, int ratingType, int sourceid)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappedData", "MappingDataRatingController", TraceId);
            Response<DataMapResponse> response = new Response<DataMapResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataRatingService.GetMappedData(direction, site,ratingType, sourceid);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMappedData", "MappingDataRatingService", TraceId, watch.ElapsedMilliseconds);
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
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMappedData", "MappingDataRatingController", TraceId);

                if(result ==null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappedData", "MappingDataRatingController", TraceId, ex);
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

        [HttpPost("{direction}/{site}/{ratingtype}/{sourceid}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertMappedData(Entities.MappingDirection direction, Guid site, int ratingtype, int sourceid, DataMap data, ApiVersion apiVersion)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMappedData", "MappingDataRatingController", TraceId);
            Response<DataMapResponse> response = new Response<DataMapResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataRatingService.InsertMappedData(direction, site,ratingtype, sourceid,data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertMappedData", "MappingDataRatingService", TraceId, watch.ElapsedMilliseconds);
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
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMappedData", "MappingDataRatingController", TraceId);

                return CreatedAtRoute(nameof(GetMappedData), new {direction = direction, site = site, ratingtype = ratingtype, sourceid = sourceid, version = apiVersion.ToString()}, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertMappedData", "MappingDataRatingController", TraceId, ex);
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

        [HttpPut("{direction}/{site}/{ratingtype}/{sourceid}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateMappedData(Entities.MappingDirection direction, Guid site, int ratingtype, int sourceid, DataMap data)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMappedData", "MappingDataRatingController", TraceId);
            Response<DataMapResponse> response = new Response<DataMapResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataRatingService.UpdateMappedData(direction, site,ratingtype, sourceid,data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateMappedData", "MappingDataRatingService", TraceId, watch.ElapsedMilliseconds);
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
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMappedData", "MappingDataRatingController", TraceId);

                if(result==null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateMappedData", "MappingDataRatingController", TraceId, ex);
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

        [HttpDelete("{direction}/{site}/{ratingtype}/{sourceid}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteMappedData(Entities.MappingDirection direction, Guid site, int ratingtype, int sourceid)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMappedData", "MappingDataRatingController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _mappingDataRatingService.DeleteMappedData(direction, site,ratingtype, sourceid);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteMappedData", "MappingDataRatingService", TraceId, watch.ElapsedMilliseconds);
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
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMappedData", "MappingDataRatingController", TraceId);

                if(!result)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappedData", "MappingDataRatingController", TraceId, ex);
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
