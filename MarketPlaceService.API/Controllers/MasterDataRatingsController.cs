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
    [Route("api/v{version:apiVersion}/ratings")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MasterDataRatingsController : ControllerBase
    {
        private readonly ILogger<MasterDataRatingsController> _logger;
        private readonly IMasterDataRatingsService _masterDataRatings;
        private readonly ICommonService _commonService;
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
            _masterDataRatings.TraceId = _traceId;
            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _masterDataRatings.UserId = _userId;
        }

        public MasterDataRatingsController(IMasterDataRatingsService masterDataRatings, ICommonService commonService, ILogger<MasterDataRatingsController> logger, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _masterDataRatings = masterDataRatings;
            _commonService = commonService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("{ratingType}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataRatings(int ratingType)
        {
            if(ratingType.ValidateInteger())
                return BadRequest();    
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataRatings", "MasterDataRatingsController", TraceId);
            Response<IEnumerable<MasterDataRating>> response = new Response<IEnumerable<MasterDataRating>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRatings.GetMasterDataRatings(ratingType);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataRatings", "MasterDataRatingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MasterDataRating>>
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
                        MasterDataRatingCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataRatings", "MasterDataRatingsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataRatings", "MasterDataRatingsController", TraceId, ex);
                response = new Response<IEnumerable<MasterDataRating>>
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

        [HttpGet("{ratingType}/{ratingId}", Name = "GetMasterDataRatings")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataRatings(int ratingType, int ratingId)
        {
            if(ratingType.ValidateInteger() || ratingId.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataRatings", "MasterDataRatingsController", TraceId);
            Response<MasterDataRating> response = new Response<MasterDataRating>();
            try
            {
                _logger.LogInformation("MasterDataRatingsService service is called.");
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRatings.GetMasterDataRatings(ratingType,ratingId);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataRatings", "MasterDataRatingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataRating>
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
                        MasterDataRatingCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataRatings", "MasterDataRatingsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataRatings", "MasterDataRatingsController", TraceId, ex);
                response = new Response<MasterDataRating>
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

        [HttpPost("{ratingType}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertMasterDataRatings(int ratingType, MasterDataRating data, ApiVersion apiVersion)
        {
            if(ratingType.ValidateInteger() || data.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataRatings", "MasterDataRatingsController", TraceId);
            Response<MasterDataRating> response = new Response<MasterDataRating>();
            try
            {
                var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(0, "rating", 0, data.Name, ratingType);
                if(duplicateNameExists)
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("An item with the name {0} already exists", data.Name)});               
                
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRatings.InsertMasterDataRatings(ratingType,data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertMasterDataRatings", "MasterDataRatingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataRating>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        MasterDataRatingCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataRatings", "MasterDataRatingsController", TraceId);

                return CreatedAtRoute(nameof(GetMasterDataRatings), new{ratingType = ratingType, ratingId = result.Id, version = apiVersion.ToString()}, response);
                //return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertMasterDataRatings", "MasterDataRatingsController", TraceId, ex);
                // response = new Response<MasterDataRating>
                // {
                //     ResponseCode = (int)Code.exceptionError,
                //     Status = "Failure",
                //     Message = ex.ToString(),
                //     TraceId = Guid.NewGuid()
                // };

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

                if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<MasterDataRating> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding {0} failed. Another item with the same Type and Name exists.", data.Name)});


                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPut("{ratingType}/{ratingId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateMasterDataRatings(int ratingType, int ratingId, MasterDataRating data)
        {
            if(ratingType.ValidateInteger() || ratingId.ValidateInteger() || data.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataRatings", "MasterDataRatingsController", TraceId);
            Response<MasterDataRating> response = new Response<MasterDataRating>();
            try
            {
                var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(0, "rating", ratingId, data.Name, ratingType);
                if(duplicateNameExists)
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("An item with the name {0} already exists", data.Name)});               
                
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRatings.UpdateMasterDataRatings(ratingType, ratingId, data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateMasterDataRatings", "MasterDataRatingService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataRating>
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
                        MasterDataRatingCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataRatings", "MasterDataRatingsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateMasterDataRatings", "MasterDataRatingsController", TraceId, ex);
                // response = new Response<MasterDataRating>
                // {
                //     ResponseCode = (int)Code.exceptionError,
                //     Status = "Failure",
                //     Message = ex.ToString(),
                //     TraceId = Guid.NewGuid()
                // };

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

                if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Update failed for {0}. Another item with the same Type and Name exists.", data.Name)});

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpDelete("{ratingType}/{ratingId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteMasterDataRatings(int ratingType, int ratingId)
        {
            if(ratingId.ValidateInteger() || ratingType.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataRatings", "MasterDataRatingsController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var isMappedToProduct = await _masterDataRatings.CheckIfMappedToImportedProduct(ratingId);
                if(isMappedToProduct)
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Cannot be deleted as a published product uses this rating")});               
                
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRatings.DeleteMasterDataRatings(ratingType, ratingId);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteMasterDataRatings", "MasterDataRatingService", TraceId, watch.ElapsedMilliseconds);
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
                        OperationResult = true
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                 LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataRatings", "MasterDataRatingsController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteMasterDataRatings", "MasterDataRatingsController", TraceId, ex);
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
