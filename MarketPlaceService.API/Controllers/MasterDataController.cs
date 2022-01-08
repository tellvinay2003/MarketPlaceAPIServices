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
    [Route("api/v{version:apiVersion}/masterdata")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MasterDataController : ControllerBase
    {
        private readonly ILogger<MasterDataController> _logger;
        private readonly IMasterDataService _masterDataService;
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
            _masterDataService.TraceId = _traceId;

            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _masterDataService.UserId = _userId;
        }

        public MasterDataController(IMasterDataService masterDataService, ICommonService commonService, ILogger<MasterDataController> logger, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _masterDataService = masterDataService;
            _commonService = commonService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("{masterDataTypeId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataGenericAll(int masterDataTypeId)
        {
            if(masterDataTypeId.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();    
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGenericAll", "MasterDataController", TraceId);
            Response<IEnumerable<MasterData>> response = new Response<IEnumerable<MasterData>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataService.GetMasterDataGeneric(masterDataTypeId);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataGeneric", "MasterDataService", TraceId, watch.ElapsedMilliseconds);
                
                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        MasterDataCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = Guid.NewGuid()
                }).ConfigureAwait(false);               
                
                response = new Response<IEnumerable<MasterData>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGenericAll", "MasterDataController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataGenericAll", "MasterDataController", TraceId, ex);
                response = new Response<IEnumerable<MasterData>>
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

        [HttpGet("{masterdatatypeid}/{itemId}", Name = "GetMasterDataGeneric")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataGeneric(int masterDataTypeId, int itemId)
        {
            if(masterDataTypeId.ValidateInteger() || itemId.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();    
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGeneric", "MasterDataController", TraceId);
            Response<MasterData> response = new Response<MasterData>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataService.GetMasterDataGeneric(masterDataTypeId, itemId);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataGeneric", "MasterDataService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterData>
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
                        MasterDataCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGeneric", "MasterDataController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataGeneric", "MasterDataController", TraceId, ex);
                response = new Response<MasterData>
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

        [HttpPost("{masterDataTypeId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertMasterDataGeneric(int masterDataTypeId, MasterData item, ApiVersion apiVersion)
        {
            if(masterDataTypeId.ValidateInteger() || item.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();    
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataGeneric", "MasterDataController", TraceId);
            Response<MasterData> response = new Response<MasterData>();
            try
            {
                var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(masterDataTypeId, null, 0, item.Name, 0);
                if(duplicateNameExists)
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("An item with name {0} already exists.", item.Name)});               
                
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataService.InsertMasterDataGeneric(masterDataTypeId, item);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertMasterDataGeneric", "MasterDataService", TraceId, watch.ElapsedMilliseconds);
                
               await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        MasterDataCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                
                if(result == null)
                    return NotFound();

                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataGeneric", "MasterDataController", TraceId);
                
                response = new Response<MasterData>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };                

                return CreatedAtRoute(nameof(GetMasterDataGeneric), new {masterDataTypeId = masterDataTypeId, itemId = result.Id, version = apiVersion.ToString()}, response);
                //return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertMasterDataGeneric", "MasterDataController", TraceId, ex);
                // response = new Response<MasterData>
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
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding {0} failed. Another item with the same Type and Name exists.", item.Name)});

               if(ex is TimeoutException tex)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPut("{masterDataTypeId}/{itemId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateMasterDataGeneric(int masterDataTypeId, int itemId, MasterData item)
        {
            if(masterDataTypeId.ValidateInteger() || itemId.ValidateInteger() || item.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();    
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataGeneric", "MasterDataController", TraceId);
            Response<MasterData> response = new Response<MasterData>();
            try
            {
                var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(masterDataTypeId, null, itemId, item.Name, 0);
                if(duplicateNameExists)
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("An item with name {0} already exists.", item.Name)});               
                
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataService.UpdateMasterDataGeneric(masterDataTypeId,itemId, item);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateMasterDataGeneric", "MasterDataService", TraceId, watch.ElapsedMilliseconds);
               

               await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        MasterDataCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                
                if(result == null)
                    return NotFound();

                 response = new Response<MasterData>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataGeneric", "MasterDataController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateMasterDataGeneric", "MasterDataController", TraceId, ex);
                // response = new Response<MasterData>
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
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Update failed for {0}. Another item with the same Type and Name exists.", item.Name)});

                if(ex is TimeoutException tex)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpDelete("{masterDataTypeId}/{itemId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteMasterDataGeneric(int masterDataTypeId, int itemId)
        {
            if(masterDataTypeId.ValidateInteger() || itemId.ValidateInteger())
                return BadRequest();
            
            if(masterDataTypeId == 6)//Rating type 
            {
                var isMappedToProduct = await _masterDataService.CheckIfMappedToImportedProduct(itemId);
                    if(isMappedToProduct)
                        return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Cannot be deleted as a published product uses this rating type.")});               
            }  
             
            ActivateTrace();    
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataGeneric", "MasterDataController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                _logger.LogInformation("MasterDataService service is called.");
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataService.DeleteMasterDataGeneric(masterDataTypeId,itemId);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteMasterDataGeneric", "MasterDataService", TraceId, watch.ElapsedMilliseconds);
                

               await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        MasterDataCollection = null
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);

                if(!result)
                    return NotFound();

                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };

                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataGeneric", "MasterDataController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteMasterDataGeneric", "MasterDataController", TraceId, ex);
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
