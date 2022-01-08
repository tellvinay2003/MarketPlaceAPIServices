using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using MarketPlaceService.API.Utilities;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    // [Authorize]
    // [Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/subscribers")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        readonly ILogger<SubscribersController> _logger;
        readonly ISubscriberService _subscriberService;

        readonly ITransactionLoggerService<SubscriberDataModel> _transactionLoggerService;
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
        private void ActivateTrace()
        {
            if (_traceId == Guid.Empty)
                _traceId = Request.Headers.ContainsKey("TraceId") ? Guid.Parse(Request.Headers["TraceId"]) : Guid.NewGuid();
            _subscriberService.TraceId = _traceId;
        }

        public SubscribersController(ISubscriberService subscriberService,ITransactionLoggerService<SubscriberDataModel> transactionLoggerService, ILogger<SubscribersController> logger, ICommonService commonService)
        {
            _logger = logger;
            _subscriberService = subscriberService;
            _transactionLoggerService=transactionLoggerService;
            _commonService = commonService;
        }


        /// <summary>
        /// Get list of all publishers
        /// </summary>
        /// <returns></returns>
        // [HttpGet("GetAll")]
        [HttpGet("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetRegisteredSubscribers()
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegisteredSubscribers", "SubscribersController", TraceId);
            Response<IEnumerable<SubscriberDataModel>> response = new Response<IEnumerable<SubscriberDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetSubscribersListAsync();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetRegisteredSubscribers", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SubscriberDataModel>>
                 {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId=TraceId
                 };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetRegisteredSubscribers", "SubscribersController", TraceId);
                // await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<IEnumerable<SubscriberDataModel>>
                // {
                //     TransactionData = result,
                //     TransactionStatus = "Success",
                //     TransactionType = "Information",
                //     InitiatedBy = HttpContext.User.ToString(),
                //     InitiatedOn = DateTime.Now
                // });

                if(result.IsNullOrEmpty())
                    return NoContent();
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetRegisteredSubscribers.", "SubscribersController", TraceId, ex);
                response = new Response<IEnumerable<SubscriberDataModel>>
                 {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId=TraceId
                 };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }



        /// <summary>
        /// Delete Subscriber by subscriberId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        
        [HttpDelete("{id}")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(SubscriberDataModel), Description = "Returns finded Publisher")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> DeleteSubscriberById(Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();

          ActivateTrace();         
          LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriberById", "SubscribersController", TraceId);
          Response<SubscriberDataModel> response = new Response<SubscriberDataModel>();
          try
          {
                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.DeleteSubscriber(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteSubscriberById", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDataModel> { ResponseCode = (int)Code.success, ResponseMessage = result,Status="Success",ExecutionTimeMS = watch.ElapsedMilliseconds,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriberById", "SubscribersController", TraceId);
                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<SubscriberDataModel>
                {
                    TransactionData = result,
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow
                }).ConfigureAwait(false);

                if(result == null)
                    return NotFound();

                return Ok(response);
          }
          catch(Exception ex)
          {
               LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteSubscriberById", "SubscribersController", TraceId, ex);
                response = new Response<SubscriberDataModel> 
                { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),Status="Failure",TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
          }
        }

        
        /// <summary>
        /// Edit Subscriber by SubscriberId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}", Name = "GetSubscriberById")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(SubscriberDataModel), Description = "Returns finded Publisher")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetSubscriberById(Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();

          ActivateTrace();         
          LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberById", "SubscribersController", TraceId);
          Response<SubscriberDataModel> response = new Response<SubscriberDataModel>();
          try
          {
                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.EditSubscriber(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "EditSubscriber", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDataModel> { ResponseCode = (int)Code.success, ResponseMessage = result,Status="Success",ExecutionTimeMS = watch.ElapsedMilliseconds,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberById", "SubscribersController", TraceId);
                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<SubscriberDataModel>
                {
                    TransactionData = result,
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow
                }).ConfigureAwait(false);

                if(result == null)
                    return NotFound();

                return Ok(response);
          }
          catch(Exception ex)
          {
               LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriberById", "SubscribersController", TraceId, ex);
                response = new Response<SubscriberDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),Status="Failure",TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
          }
        }


        /// <summary>
        /// Create new Subscriber
        /// </summary>
        /// <param name="SubscriberItem"></param>
        /// <returns></returns>
        [HttpPost("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
         public async Task<IActionResult> AddNewSubscriber([FromBody] SubscriberDataModel SubscriberItem, ApiVersion apiVersion)
        {
            if(SubscriberItem.ValidateObjectForNull() || SubscriberItem.SiteId.ValidateEmptyGuid()|| SubscriberItem.SubscriberName == string.Empty)
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "AddNewSubscriber", "SubscribersController", TraceId);

            Response<SubscriberDataModel> response = new Response<SubscriberDataModel>();
            try
            {
                var isOrganisationValid = await _commonService.CanOrganisationBeUsed((int)SubscriberItem.OrganizationId, EntityType.Subscriber, Guid.Empty, SubscriberItem.SiteId);
                if (!isOrganisationValid.Item1)
                {
                    return BadRequest(new Response<PublisherDataModel> { ResponseCode = (int)Code.BadRequest, Message = isOrganisationValid.Item2 });
                }

                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.AddNewSubscriber(SubscriberItem);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "AddNewSubscriber", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDataModel> { ResponseCode = (int)Code.Created, ResponseMessage = result,Status="Success",TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "AddNewSubscriber", "SubscribersController", TraceId);
                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<SubscriberDataModel>
                {
                    TransactionData = result,
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow
                }).ConfigureAwait(false);
                
                return CreatedAtRoute(nameof(GetSubscriberById), new {id = result.SubscriberId, version = apiVersion.ToString()}, response);
                //return Ok(response);
            }
            catch(Exception ex)
            {
               LoggingHelper.LogError(_logger, ExceptionType.System, "AddNewSubscriber", "SubscribersController", TraceId, ex);
                //response = new Response<SubscriberDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString() };

                if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<SubscriberDataModel> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding new subscriber failed. Another subscriber with the same Site and Organisation exists.")});

                if(ex is TimeoutException tex)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        
        /// <summary>
        /// update Subscriber
        /// </summary>
        /// <param name="SubscriberItem"></param>
        /// <returns></returns>
        [HttpPut("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> UpdateSubscriber([FromBody] SubscriberDataModel SubscriberItem)
        {
            //todo validation

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriber", "SubscribersController", TraceId);
            Response<SubscriberDataModel> response = new Response<SubscriberDataModel>();
            try
            {
                var isOrganisationValid = await _commonService.CanOrganisationBeUsed((int)SubscriberItem.OrganizationId, EntityType.Subscriber, SubscriberItem.SubscriberId, SubscriberItem.SiteId);
                if (!isOrganisationValid.Item1)
                {
                    return BadRequest(new Response<PublisherDataModel> { ResponseCode = (int)Code.BadRequest, Message = isOrganisationValid.Item2 });
                }
                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.UpdateSubscriber(SubscriberItem);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateSubscriber", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDataModel> { ResponseCode = (int)Code.success, ResponseMessage = result,Status="Success",TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriber", "SubscribersController", TraceId);
                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<SubscriberDataModel>
                {
                    TransactionData = result,
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow
                }).ConfigureAwait(false);
                return Ok(response);
            }
            catch(Exception ex)
            {
               LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateSubscriber", "SubscribersController", TraceId, ex);
                //response = new Response<SubscriberDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString() };

                if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<SubscriberDataModel> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Update failed. Another subscriber with the same Site and Organisation exists.")});


                if(ex is TimeoutException tex)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }
        
          
        /// <summary>
        /// Get list of enabled Subscribers
        /// </summary>
        /// <returns></returns>  
        [HttpGet("Subscribers")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetEnabledSubscribers()
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetEnabledSubscribers", "SubscribersController", TraceId);
            Response<IEnumerable<SubscriberDataModel>> response = new Response<IEnumerable<SubscriberDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetEnabledSubscribersListAsync();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetEnabledSubscribersListAsync", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SubscriberDataModel>> { ResponseCode = (int)Code.success, ResponseMessage = result,Status="Success",TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetEnabledSubscribers", "SubscribersController", TraceId);

                if(result.IsNullOrEmpty())
                    return NoContent();


                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetEnabledSubscribers", "SubscribersController", TraceId, ex);
                response = new Response<IEnumerable<SubscriberDataModel>> { ResponseCode = (int)Code.exceptionError,TraceId=TraceId, Message = ex.ToString() };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


       /// <summary>
       /// Functionality to fetch list of publisher id and supplierid stored against the subscriber
       /// </summary>
       /// <param name="SubscriberId"></param>
       /// <returns></returns>
        [HttpGet("suppliermap")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetSupplierMapSubscriberById(Guid SubscriberId)
        {
            if(SubscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierMapSubscriberById", "SubscriberController", TraceId);
            Response<IEnumerable<SubscriberSupplierDataModel>> response = new Response<IEnumerable<SubscriberSupplierDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetSupplierMapSubscriberByIdAsync(SubscriberId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSupplierMapSubscriberByIdAsync", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SubscriberSupplierDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierMapSubscriberByIdAsync", "SubscriberController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSupplierMapSubscriberByIdAsync", "SubscriberController", TraceId, ex);
                response = new Response<IEnumerable<SubscriberSupplierDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        [HttpGet("suppliermap/{id}", Name = "GetSupplierMapSubscriberByIdandPubId")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetSupplierMapSubscriberByIdandPubId(Guid SubscriberId,Guid PublisherId)
        {
            if(SubscriberId.ValidateEmptyGuid() || PublisherId.ValidateEmptyGuid())
                return BadRequest();
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierMapSubscriberByIdandPubId", "SubscriberController", TraceId);
            Response<SubscriberSupplierDataModel> response = new Response<SubscriberSupplierDataModel>();
            try
            {
                _logger.LogInformation("Subscriber service is called.");
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetSupplierMapSubscriberByIdandPubIdAsync(SubscriberId,PublisherId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSupplierMapSubscriberByIdandPubIdAsync", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberSupplierDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierMapSubscriberByIdandPubId", "SubscriberController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSupplierMapSubscriberByIdandPubId", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberSupplierDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        
        [HttpPost("suppliermap")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> AddNewSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId,int SupplierId, ApiVersion apiVersion)
        {
            if(SubscriberId.ValidateEmptyGuid() || PublisherId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierMapSubscriberById", "SubscriberController", TraceId);
            Response<SubscriberSupplierDataModel> response = new Response<SubscriberSupplierDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.AddNewSubscriberSupplierMap(SubscriberId,PublisherId,SupplierId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "AddNewSubscriberSupplierMap", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberSupplierDataModel>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierMapSubscriberById", "SubscriberController", TraceId);
                return CreatedAtRoute(nameof(GetSupplierMapSubscriberByIdandPubId), new {id=result.SubscriberSupplierId, version = apiVersion.ToString()}, response);
                //return Ok(response);
            }
            catch(Exception ex)
            {
               LoggingHelper.LogError(_logger, ExceptionType.System, "GetSupplierMapSubscriberById", "SubscriberController", TraceId, ex);
               response = new Response<SubscriberSupplierDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            } 
        }


        [HttpPut("suppliermap")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId,int SupplierId)
        {
            if(SubscriberId.ValidateEmptyGuid() || PublisherId.ValidateEmptyGuid() || SupplierId.ValidateInteger())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierMapSubscriberById", "SubscriberController", TraceId);
            Response<SubscriberSupplierDataModel> response = new Response<SubscriberSupplierDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.UpdateSubscriberSupplierMap(SubscriberId,PublisherId,SupplierId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateSubscriberSupplierMap", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberSupplierDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriberSupplierMap", "SubscriberController", TraceId);
                return Ok(response);
            }
            catch(Exception ex)
            {
               LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateSubscriberSupplierMap", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberSupplierDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            } 
        }

        [HttpDelete("suppliermap")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteSubscriberSupplierMap(Guid SubscriberId,Guid PublisherId)
        {
            if(SubscriberId.ValidateEmptyGuid() || PublisherId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriberSupplierMap", "SubscriberController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                    var watch = Stopwatch.StartNew();
                    var result = await _subscriberService.DeleteSubscriberSupplierMap(SubscriberId,PublisherId);
                    watch.Stop();
                    LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteSubscriberSupplierMap", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                    response = new Response<bool>
                   {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                    };
                    LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriberSupplierMap", "SubscriberController", TraceId);
                    return Ok(response);
            }
            catch(Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteSubscriberSupplierMap", "SubscriberController", TraceId, ex);
                 response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                 if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }



        [HttpGet("default/{subscriberId}", Name = "GetDefaultSubscriberById")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetDefaultSubscriberById(Guid subscriberId)
        {
            if(subscriberId.ValidateEmptyGuid())
                return BadRequest();
            
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetDefaultSubscriberById", "SubscriberController", TraceId);
            Response<SubscriberChargingPolicyDataModel> response = new Response<SubscriberChargingPolicyDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetDefaultSubscriberById(subscriberId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetDefaultSubscriberById", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberChargingPolicyDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetDefaultSubscriberById", "SubscriberController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetDefaultSubscriberById", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberChargingPolicyDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        [HttpPut("default")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateDefaultSubscriber(SubscriberChargingPolicyDataModel request)
        {
            //todo validations
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierMapSubscriberById", "SubscriberController", TraceId);
            Response<SubscriberChargingPolicyDataModel> response = new Response<SubscriberChargingPolicyDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.UpdateDefaultSubscriber(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateDefaultSubscriber", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberChargingPolicyDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateDefaultSubscriber", "SubscriberController", TraceId);
                return Ok(response);
            }
            catch(Exception ex)
            {
               LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateDefaultSubscriber", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberChargingPolicyDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            } 
        }


        [HttpGet("contractdefault")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetContractDefaultSubscriberById(Guid subscriberId)
        {
            if(subscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetContractDefaultSubscriberById", "SubscriberController", TraceId);
            Response<SubscriberDefaultDataModel> response = new Response<SubscriberDefaultDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetContractDefaultSubscriberById(subscriberId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetContractDefaultSubscriberById", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetContractDefaultSubscriberById", "SubscriberController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetContractDefaultSubscriberById", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberDefaultDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        [HttpPut("contractdefault")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateContractDefaultSubscriber(SubscriberDefaultDataModel request)
        {
            //todo validations

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateContractDefaultSubscriber", "SubscriberController", TraceId);
            Response<SubscriberDefaultDataModel> response = new Response<SubscriberDefaultDataModel>();
            try
            {
                _logger.LogInformation("Subscriber service is called.");
                var watch = Stopwatch.StartNew();
                var result = await _subscriberService.UpdateContractDefaultSubscriber(request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateContractDefaultSubscriber", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateContractDefaultSubscriber", "SubscriberController", TraceId);
                return Ok(response);
            }
            catch(Exception ex)
            {
               LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateContractDefaultSubscriber", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberDefaultDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            } 
        }

        #region Supplier Default Product Codes
        [HttpGet("defaultproductcode/{subscriberId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetSubscriberDefaultsProductCodes(Guid subscriberId)
        {
            if(subscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
            Response<IEnumerable<SubscriberDefaultsProductCode>> response = new Response<IEnumerable<SubscriberDefaultsProductCode>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetSubscriberDefaultsProductCodes(subscriberId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSubscriberDefaultsProductCodes", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SubscriberDefaultsProductCode>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultsProductCodes", "SubscriberController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriberDefaultsProductCodes", "SubscriberController", TraceId, ex);
                response = new Response<IEnumerable<SubscriberDefaultsProductCode>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpGet("defaultproductcode/{subscriberId}/{ruleId}", Name = "GetSubscriberDefaultsProductCodes")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId)
        {
            if(subscriberId.ValidateEmptyGuid() || ruleId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
            Response<SubscriberDefaultsProductCode> response = new Response<SubscriberDefaultsProductCode>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetSubscriberDefaultsProductCodes(subscriberId,ruleId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSubscriberDefaultsProductCodes", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultsProductCode>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultsProductCodes", "SubscriberController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriberDefaultsProductCodes", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberDefaultsProductCode>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpPost("defaultproductcode/{subscriberId}")]
        [EnableCors("odlPolicy")]
         public async Task<IActionResult> InsertSubscriberDefaultsProductCodes(Guid subscriberId, SubscriberDefaultsProductCode request, ApiVersion apiVersion)

        {
            if(subscriberId.ValidateEmptyGuid() || request.ValidateObjectForNull()) //todo
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
            Response<SubscriberDefaultsProductCode> response = new Response<SubscriberDefaultsProductCode>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.InsertSubscriberDefaultsProductCodes(subscriberId, request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertSubscriberDefaultsProductCodes", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultsProductCode>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
                
                return CreatedAtRoute(nameof(GetSubscriberDefaultsProductCodes), new{subscriberId = subscriberId, ruleId = result.RuleId, version = apiVersion.ToString()}, result);
                //return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertSubscriberDefaultsProductCodes", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberDefaultsProductCode>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
               if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }

        }

        [HttpPut("defaultproductcode/{subscriberId}/{ruleId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId, SubscriberDefaultsProductCode request)
        {
            if(subscriberId.ValidateEmptyGuid() || ruleId.ValidateEmptyGuid() || request.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
            Response<SubscriberDefaultsProductCode> response = new Response<SubscriberDefaultsProductCode>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.UpdateSubscriberDefaultsProductCodes(subscriberId,ruleId, request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateSubscriberDefaultsProductCodes", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultsProductCode>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateSubscriberDefaultsProductCodes", "SubscriberController", TraceId, ex);
                response = new Response<SubscriberDefaultsProductCode>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }

        [HttpDelete("defaultproductcode/{subscriberId}/{ruleId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteSubscriberDefaultsProductCodes(Guid subscriberId, Guid ruleId)
        {
            if(subscriberId.ValidateEmptyGuid() || ruleId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.DeleteSubscriberDefaultsProductCodes(subscriberId,ruleId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteSubscriberDefaultsProductCodes", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriberDefaultsProductCodes", "SubscriberController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteSubscriberDefaultsProductCodes", "SubscriberController", TraceId, ex);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }






        [HttpGet("defaultsellpricerules/{subscriberId}")]
        [EnableCors("odlPolicy")]
         public async Task<IActionResult> GetSubscriberDefaultSellPrices(Guid subscriberId)
        {
            if(subscriberId.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultSellPrices", "SubscribersController", TraceId);
            Response<IEnumerable<SubscriberDefaultSellingPrice>> response = new Response<IEnumerable<SubscriberDefaultSellingPrice>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetSubscriberDefaultSellPrices(subscriberId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSubscriberDefaultSellPrices", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SubscriberDefaultSellingPrice>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultSellPrices", "SubscribersController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriberDefaultSellPrices", "SubscribersController", TraceId, ex);
                response = new Response<IEnumerable<SubscriberDefaultSellingPrice>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }
        }

        [HttpGet("defaultsellpricerules/{subscriberId}/{ruleId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetSubscriberDefaultSellPriceById(Guid subscriberId, int ruleId)
        {
            if(subscriberId.ValidateEmptyGuid() || ruleId.ValidateInteger())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSubscriberDefaultSellPriceById", "SubscribersController", TraceId); 
            Response<SubscriberDefaultSellingPrice> response = new Response<SubscriberDefaultSellingPrice>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.GetSubscriberDefaultSellPriceById(subscriberId,ruleId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSubscriberDefaultSellPriceById", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultSellingPrice>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSubscriberDefaultSellPriceById", "SubscribersController", TraceId); 

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSubscriberDefaultSellPriceById", "SubscribersController", TraceId, ex);
                response = new Response<SubscriberDefaultSellingPrice>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }
        }
        

        [HttpPost("defaultsellpricepolicies/{subscriberId}")]
        [EnableCors("odlPolicy")]
         public async Task<IActionResult> InsertSubscriberDefaultsSellPrice(Guid subscriberId, SubscriberDefaultSellingPrice request,ApiVersion apiVersion)

        {
            if(subscriberId.ValidateEmptyGuid() || request.ValidateObjectForNull()) 
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "InsertSubscriberDefaultsSellPrice", "SubscribersController", TraceId);
            Response<SubscriberDefaultSellingPrice> response = new Response<SubscriberDefaultSellingPrice>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.InsertSubscriberDefaultsSellPrice(subscriberId, request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "InsertSubscriberDefaultsSellPrice", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultSellingPrice>
                {
                    ResponseCode = (int)Code.Created,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "InsertSubscriberDefaultsSellPrice", "SubscribersController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "InsertSubscriberDefaultsSellPrice", "SubscribersController", TraceId, ex);
                response = new Response<SubscriberDefaultSellingPrice>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
               if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }

        }


        [HttpPut("defaultsellpricepolicies/{subscriberId}/{ruleId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> UpdateSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId, SubscriberDefaultSellingPrice request)
        {
            if(subscriberId.ValidateEmptyGuid() || ruleId.ValidateInteger() || request.ValidateObjectForNull())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSubscriberDefaultsSellPrice", "SubscribersController", TraceId);
            Response<SubscriberDefaultSellingPrice> response = new Response<SubscriberDefaultSellingPrice>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.UpdateSubscriberDefaultsSellPrice(subscriberId,ruleId, request);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateSubscriberDefaultsSellPrice", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SubscriberDefaultSellingPrice>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSubscriberDefaultsSellPrice", "SubscribersController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateSubscriberDefaultsSellPrice", "SubscribersController", TraceId, ex);
                response = new Response<SubscriberDefaultSellingPrice>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }
        }


        [HttpDelete("defaultsellpricepolicies/{subscriberId}/{ruleId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> DeleteSubscriberDefaultsSellPrice(Guid subscriberId, int ruleId)
        {
            if(subscriberId.ValidateEmptyGuid() || ruleId.ValidateInteger())
                return BadRequest();

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSubscriberDefaultsSellPrice", "SubscribersController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _subscriberService.DeleteSubscriberDefaultsSellPrice(subscriberId,ruleId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteSubscriberDefaultsSellPrice", "SubscriberService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = result,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSubscriberDefaultsSellPrice", "SubscribersController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteSubscriberDefaultsSellPrice", "SubscribersController", TraceId, ex);
                response = new Response<bool>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500);
            }
        }

        #endregion

    }
}