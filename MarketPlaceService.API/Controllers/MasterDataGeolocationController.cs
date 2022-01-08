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
    [Route("api/v{version:apiVersion}/geolocation")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MasterDataGeolocationController : ControllerBase
    {
        private readonly ILogger<MasterDataGeolocationController> _logger;
        private readonly IMasterDataRegionService _masterDataRegionService;
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
            _masterDataRegionService.TraceId = _traceId;
            
            if (_userId == Guid.Empty)
                _userId = Request.Headers.ContainsKey("UserId") ? Guid.Parse(Request.Headers["UserId"]) : Guid.Empty;
            _masterDataRegionService.UserId = _userId;
        }

        public MasterDataGeolocationController(IMasterDataRegionService masterDataRegionService, ILogger<MasterDataGeolocationController> logger, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService)
        {
            _logger = logger;
            _masterDataRegionService = masterDataRegionService;
            _transactionLoggerService = transactionLoggerService;
        }
        /// <summary>
        /// Gets Geo locations from MarketPlace DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataGeolocations(string name)
        {
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);
            Response<IEnumerable<MasterDataGeolocation>> response = new Response<IEnumerable<MasterDataGeolocation>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRegionService.GetMasterDataGeolocations(name??string.Empty);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataGeolocations", "MasterDataRegionService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MasterDataGeolocation>>
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
                        MasterGeolocationCollection = result
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataGeolocations", "MasterDataGeoLocationController", TraceId, ex);
                response = new Response<IEnumerable<MasterDataGeolocation>>
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


        [HttpGet("{id}", Name = "GetMasterDataGeolocations")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetMasterDataGeolocations(int id)
        {
            if(id.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);
            Response<MasterDataGeolocation> response = new Response<MasterDataGeolocation>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRegionService.GetMasterDataGeolocations(id);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMasterDataGeolocations", "MasterDataRegionService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataGeolocation>
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
                        MasterGeolocationCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMasterDataGeolocations", "MasterDataGeoLocationController", TraceId, ex);
                response = new Response<MasterDataGeolocation>
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

        [HttpPost("{parentId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> InsertMasterDataGeolocations(int parentId, MasterDataGeolocation data, ApiVersion apiVersion)
        {
            if(parentId < 0 || data.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);
            Response<MasterDataGeolocation> response = new Response<MasterDataGeolocation>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRegionService.InsertMasterDataGeolocations(parentId, data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertMasterDataGeolocations", "MasterDataRegionService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataGeolocation>
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
                        MasterGeolocationCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);

               return CreatedAtRoute(nameof(GetMasterDataGeolocations), new{id = result.Id, version = apiVersion.ToString()}, response);
                //return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertMasterDataGeolocations", "MasterDataGeoLocationController", TraceId, ex);
                // response = new Response<MasterDataGeolocation>
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
                    return BadRequest(new Response<MasterDataGeolocation> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding {0} failed. Another region with the same name exists at this level.", data.Name)});


                if(ex is TimeoutException tex)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPut("{id}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateMasterDataGeolocations(int id, MasterDataGeolocation data)
        {
            if(id.ValidateInteger() || data.ValidateObjectForNull())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);
            Response<MasterDataGeolocation> response = new Response<MasterDataGeolocation>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
               var result = await _masterDataRegionService.UpdateMasterDataGeolocations(id, data);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateMasterDataGeolocations", "MasterDataRegionService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<MasterDataGeolocation>
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
                        MasterGeolocationCollection = Enumerable.Repeat(result,1)
                    },
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);

                if(result==null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateMasterDataGeolocations", "MasterDataGeoLocationController", TraceId, ex);
                // response = new Response<MasterDataGeolocation>
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
                    return BadRequest(new Response<MasterDataGeolocation> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Updating {0} failed. Another region with the same name exists at this level.", data.Name)});

               if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpDelete("{id}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteMasterDataGeolocations(int id)
        {
            if(id.ValidateInteger())
                return BadRequest();
             
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                _logger.LogInformation("MasterDataRegionService service is called.");
                var watch = Stopwatch.StartNew();
                // Service call

                var isMappedToProduct = await _masterDataRegionService.CheckIfMappedToImportedProduct(id);
                if(isMappedToProduct)
                    return BadRequest(new Response<bool> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Cannot be deleted as a published product uses this region.")});  

               var result = await _masterDataRegionService.DeleteMasterDataGeolocations(id);               
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteMasterDataGeolocations", "MasterDataRegionService", TraceId, watch.ElapsedMilliseconds);
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
               LoggingHelper.LogInfo(_logger, LogType.End, "DeleteMasterDataGeolocations", "MasterDataGeoLocationController", TraceId);

                if(!result)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteMasterDataGeolocations", "MasterDataGeoLocationController", TraceId, ex);
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
