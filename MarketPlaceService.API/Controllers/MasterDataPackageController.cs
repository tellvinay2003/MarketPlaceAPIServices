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
    [Route("api/v{version:apiVersion}/package")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MasterDataPackageController : ControllerBase
    {
        private readonly ILogger<MasterDataPackageController> _logger;
        private readonly IMasterDataPackageService _masterDataPackage;
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
            _masterDataPackage.TraceId = _traceId;
            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _masterDataPackage.UserId = _userId;
        }

        public MasterDataPackageController(IMasterDataPackageService masterDataPackage, ICommonService commonService, ILogger<MasterDataPackageController> logger, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _masterDataPackage = masterDataPackage;
            _commonService = commonService;
            _transactionLoggerService = transactionLoggerService;
        }


         [HttpGet("{id}", Name = "GetMasterDataPackage")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult>  GetMasterDataPackage(int id)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataPackage", "GetMasterDataPackageController", TraceId);
            Response<IEnumerable<MasterDataPackage>> response = new Response<IEnumerable<MasterDataPackage>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataPackage.GetMasterDataPackage(id);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataPackage", "MasterDataPackageService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MasterDataPackage>>
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
                        MasterDataPackageCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataPackage", "MasterDataPackageController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataPackage", "MasterDataPackageController", TraceId, ex);
                response = new Response<IEnumerable<MasterDataPackage>>
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
        public async Task<IActionResult> InsertMasterDataPackage(int masterDataTypeId, MasterDataPackage data, ApiVersion apiVersion)
        {
            if(data.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataPackage", "MasterDataPackageController", TraceId);
            Response<MasterDataPackage> response = new Response<MasterDataPackage>();
            try
            {
               var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(masterDataTypeId, null, 0, data.Name, 0);
                if(duplicateNameExists)
                   return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("An item with name {0} already exists",data.Name)});               

                if(data.ServiceLink == null || (data.ServiceLink != null && data.ServiceLink.Id == null))
                    return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Package item not linked to Service item")});               

                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataPackage.InsertMasterDataPackage(masterDataTypeId,data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertMasterDataPackage", "MasterDataPackageService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataPackage>
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
                        MasterDataPackageCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataPackage", "MasterDataPackageController", TraceId);

                return CreatedAtRoute(nameof(GetMasterDataPackage), new{ id = result.Id, version = apiVersion.ToString()}, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertMasterDataPackage", "MasterDataPackageController", TraceId, ex);
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
                    return BadRequest(new Response<MasterDataPackage> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding {0} failed. Another item with the same Type and Name exists.", data.Name)});


                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


         [HttpPut("{masterDataTypeId}/{id}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateMasterDataPackage(int masterDataTypeId, int id, MasterDataPackage data)
        {
            if(id.ValidateInteger() || data.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataPackage", "MasterDataPackageController", TraceId);
            Response<MasterDataPackage> response = new Response<MasterDataPackage>();
            try
            {
               var duplicateNameExists = await _commonService.DuplicateMasterDataNameExists(masterDataTypeId, null, id, data.Name, 0);
                if(duplicateNameExists)
                   return BadRequest(new Response<MasterData> { ResponseCode = (int)Code.BadRequest, Message = string.Format("An item with name {0} already exists",data.Name)}); 
             
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataPackage.UpdateMasterDataPackage(masterDataTypeId,id,data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateMasterDataPackage", "MasterDataPackageService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataPackage>
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
                        MasterDataPackageCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataPackage", "MasterDataPackageController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateMasterDataPackage", "MasterDataPackageController", TraceId, ex);
                
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
                    return BadRequest(new Response<MasterDataPackage> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Update failed for {0}. Another item with the same Type and Name exists.", data.Name)});

               if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpDelete("{masterDataTypeId}/{id}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteMasterDataPackage(int masterDataTypeId,int id)
        {
            if(id.ValidateInteger())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataPackage", "MasterDataPackageController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataPackage.DeleteMasterDataPackage(masterDataTypeId,id);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteMasterDataPackage", "MasterDataPackageService", TraceId, watch.ElapsedMilliseconds);
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
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataPackage", "MasterDataPackageController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteMasterDataPackage", "MasterDataPackageController", TraceId, ex);
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
