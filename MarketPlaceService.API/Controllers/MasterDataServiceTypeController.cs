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
    [Route("api/v{version:apiVersion}/servicetype")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MasterDataServiceTypeController : ControllerBase
    {
        private readonly ILogger<MasterDataServiceTypeController> _logger;
        private readonly IMasterDataServiceTypeService _masterDataServiceType;
        private readonly ICommonService _commonService;

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
            _masterDataServiceType.TraceId = _traceId;
            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _masterDataServiceType.UserId = _userId;
        }

        private readonly ITransactionLoggerService<MarketplaceDataModel> _transactionLoggerService;

        public MasterDataServiceTypeController(IMasterDataServiceTypeService masterDataServiceType, ICommonService commonService, ILogger<MasterDataServiceTypeController> logger, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _masterDataServiceType = masterDataServiceType;
            _commonService = commonService;
            _transactionLoggerService = transactionLoggerService;
        }

        [HttpGet("")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult>  GetMasterDataServiceTypes()
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataServiceTypes", "MasterDataServiceTypeController", TraceId);
            Response<IEnumerable<MasterDataServiceType>> response = new Response<IEnumerable<MasterDataServiceType>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataServiceType.GetMasterDataServiceTypes();               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataServiceTypes", "MasterDataServiceTypeService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MasterDataServiceType>>
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
                        MasterDataServiceTypeCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataServiceTypes", "MasterDataServiceTypeController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataServiceTypes", "MasterDataServiceTypeController", TraceId, ex);
                response = new Response<IEnumerable<MasterDataServiceType>>
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

        [HttpGet("{id}", Name = "GetMasterDataServiceType")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult>  GetMasterDataServiceType(int id)
        {
            if(id.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataServiceType", "MasterDataServiceTypeController", TraceId);
            Response<MasterDataServiceType> response = new Response<MasterDataServiceType>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataServiceType.GetMasterDataServiceType(id);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataServiceType", "MasterDataServiceTypeService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataServiceType>
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
                        MasterDataServiceTypeCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataServiceType", "MasterDataServiceTypeController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataServiceType", "MasterDataServiceTypeController", TraceId, ex);
                response = new Response<MasterDataServiceType>
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

        [HttpPost("")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertMasterDataServiceType(MasterDataServiceType data, ApiVersion apiVersion)
        {
            if(data.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataServiceType", "MasterDataServiceTypeController", TraceId);
            Response<MasterDataServiceType> response = new Response<MasterDataServiceType>();
            try
            {
                var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(0, "service type", 0, data.Name, 0);
                if(duplicateNameExists)
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Service type {0} already exists",data.Name)});               
          
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataServiceType.InsertMasterDataServiceType(data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertMasterDataServiceType", "MasterDataServiceTypeService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataServiceType>
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
                        MasterDataServiceTypeCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataServiceType", "MasterDataServiceTypeController", TraceId);

                return CreatedAtRoute(nameof(GetMasterDataServiceType), new{ id = result.Id, version = apiVersion.ToString()}, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertMasterDataServiceType", "MasterDataServiceTypeController", TraceId, ex);
                // response = new Response<MasterDataServiceType>
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
                    return BadRequest(new Response<MasterDataServiceType> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding {0} failed. Another item with the same Type and Name exists.", data.Name)});


                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPut("{id}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateMasterDataServiceType(int id, MasterDataServiceType data)
        {
            if(id.ValidateInteger() || data.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataServiceType", "MasterDataServiceTypeController", TraceId);
            Response<MasterDataServiceType> response = new Response<MasterDataServiceType>();
            try
            {
                var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(0, "service type", id, data.Name, 0);
                if(duplicateNameExists)
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Service type {0} already exists",data.Name)}); 
             
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataServiceType.UpdateMasterDataServiceType(id,data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateMasterDataServiceType", "MasterDataServiceTypeService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataServiceType>
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
                        MasterDataServiceTypeCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataServiceType", "MasterDataServiceTypeController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateMasterDataServiceType", "MasterDataServiceTypeController", TraceId, ex);
                // response = new Response<MasterDataServiceType>
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
                    return BadRequest(new Response<MasterDataServiceType> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Update failed for {0}. Another item with the same Type and Name exists.", data.Name)});

               if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpDelete("{id}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteMasterDataServiceType(int id)
        {
            if(id.ValidateInteger())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataServiceType", "MasterDataServiceTypeController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var isMappedToProduct = await _masterDataServiceType.CheckIfMappedToImportedProduct(id);
                if(isMappedToProduct)
                   return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Cannot be deleted as a published product uses this service type")});               
                
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataServiceType.DeleteMasterDataServiceType(id);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteMasterDataServiceType", "MasterDataServiceTypeService", TraceId, watch.ElapsedMilliseconds);
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
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataServiceType", "MasterDataServiceTypeController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteMasterDataServiceType", "MasterDataServiceTypeController", TraceId, ex);
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
