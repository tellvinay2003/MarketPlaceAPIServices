using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using MarketPlaceService.API.Utilities;
using CommonUtilities;

namespace MarketPlaceService.API.Controllers
{
    //[Authorize]
    // [Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/sites")]
    [ApiVersion("1.0")]
    [ApiController]

    public class SitesController : ControllerBase
    {
        readonly ILogger<SitesController> _logger;
        readonly ITransactionLoggerService<IEnumerable<SiteDataModel>> _transactionLoggerService;
        readonly ISiteService _siteService;
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
            _siteService.TraceId = _traceId;
        }
        public SitesController(ISiteService siteService,ITransactionLoggerService<IEnumerable<SiteDataModel>> transactionLoggerService, ILogger<SitesController> logger)
        {
            _logger = logger;
            _siteService = siteService;
            _transactionLoggerService=transactionLoggerService;
        }

        /// <summary>
        /// Get list of all Sites
        /// </summary>
        /// <returns></returns>
        // [HttpGet("GetAll")]
        [HttpGet("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetRegisteredSites()
        {
            ActivateTrace();
            //_logger.LogInformation("[TraceId:{traceId}]  GetRegisteredSites is called.", TraceId);
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegisteredSites", "SitesController", TraceId);
            Response<IEnumerable<SiteDataModel>> response = new Response<IEnumerable<SiteDataModel>>();
            try
            {                
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _siteService.GetRegisteredSitesAsync();
                watch.Stop();
                //_logger.LogInformation("[TraceId:{traceId}]  Execution Time of GetRegisteredSites service call is: {duration}ms", TraceId, watch.ElapsedMilliseconds);
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetRegisteredSitesAsync", "SiteService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SiteDataModel>> { ResponseCode = (int)Code.success, ResponseMessage = result, TraceId =TraceId, ExecutionTimeMS = watch.ElapsedMilliseconds };
                //_logger.LogInformation("[TraceId:{traceId}]  GetRegisteredSites executed successfully.", TraceId);    
               await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<IEnumerable<SiteDataModel>>
                {
                    TransactionData = result,
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId=TraceId
                }).ConfigureAwait(false);

                LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegisteredSites", "SitesController", TraceId);
                if (result.IsNullOrEmpty())
                    return NoContent();
                return Ok(response);
            }
            catch (Exception ex)
            {
                //_logger.LogError("[TraceId:{" + TraceId.ToString() + "}  " + ex.ToString());
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetRegisteredSites", "SitesController", TraceId, ex);
                response = new Response<IEnumerable<SiteDataModel>> { ResponseCode = (int)Code.exceptionError, Message = ex.Message,TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }



               /// <summary>
        /// Get list of all Sites
        /// </summary>
        /// <returns></returns>
        // [HttpGet("GetAll")]
        [HttpGet("enabledsites")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetEnableSites()
        {
             ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetEnableSites", "SitesController", TraceId);
            Response<IEnumerable<SiteDataModel>> response = new Response<IEnumerable<SiteDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _siteService.GetEnableSites();
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetEnableSites", "SiteService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SiteDataModel>> { ResponseCode = (int)Code.success, ResponseMessage = result,TraceId=TraceId };
                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<IEnumerable<SiteDataModel>>
                {
                    TransactionData = result,
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId=TraceId
                }).ConfigureAwait(false);
                if(result.IsNullOrEmpty())
                    return NoContent();
                LoggingHelper.LogInfo(_logger, LogType.End, "GetEnableSites", "SitesController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetEnableSites", "SitesController", TraceId, ex);
                response = new Response<IEnumerable<SiteDataModel>> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        /// <summary>
        /// Get specific Site details by SiteId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetSiteById")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(SiteDataModel), Description = "Returns finded Publisher")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetSiteById([FromRoute] Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSiteById", "SitesController", TraceId);
            Response<SiteDataModel> response = new Response<SiteDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _siteService.GetSiteById(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSiteById", "SiteService", TraceId, watch.ElapsedMilliseconds);
                if(result==null)
                {
                  response = new Response<SiteDataModel> { ResponseCode = (int)Code.NotFound, ResponseMessage = result,TraceId=TraceId };
                }
                else{
                response = new Response<SiteDataModel> { ResponseCode = (int)Code.success, ResponseMessage = result,TraceId=TraceId };
                }
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSiteById", "SitesController", TraceId);	

                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<IEnumerable<SiteDataModel>>
                {
                    TransactionData = Enumerable.Repeat(result,1),
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId=TraceId
                }).ConfigureAwait(false);

                if(result == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSiteById", "SitesController", TraceId, ex);
                response = new Response<SiteDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }

        }


        /// <summary>
        /// Create new Site
        /// </summary>
        /// <returns></returns>
        // [HttpPost("CreatePublisher")]
        [HttpPost("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> AddNewSite([FromBody] SiteDataModel siteData, ApiVersion apiVersion)
        {
            if(siteData.ValidateObjectForNull()) //todo validations for proper site url
                return BadRequest();

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "AddNewSite", "SitesController", TraceId);
            Response<SiteDataModel> response = new Response<SiteDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                var validationresult= await _siteService.ValidateSite(siteData.Url);
                if(!validationresult)
                    return BadRequest(new Response<SiteDataModel> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Invalid Site URL",TraceId=TraceId)});               
                
                
                var result= await _siteService.AddNewSite(siteData);   
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "AddNewSite", "SiteService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SiteDataModel> { ResponseCode = (int)Code.Created, ResponseMessage = result,TraceId=TraceId };
                
                LoggingHelper.LogInfo(_logger, LogType.End, "AddNewSite", "SitesController", TraceId);	
                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<IEnumerable<SiteDataModel>>
                {
                    TransactionData = Enumerable.Repeat(result,1),
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId=TraceId
                }).ConfigureAwait(false); 
                
                return CreatedAtRoute(nameof(GetSiteById), new {id = result.SiteId, version = apiVersion.ToString(),TraceId=TraceId}, response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "AddNewSite", "SitesController", TraceId, ex);
                //response = new Response<SiteDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString() };
                
                 if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<SiteDataModel> { ResponseCode = (int)Code.BadRequest,TraceId=TraceId, Message = string.Format("Adding new site failed. Another site with the same Name or Url exists.")});

                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


        /// <summary>
        /// Delete Site
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(SiteDataModel), Description = "Returns finded Publisher")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> DeleteSiteById([FromRoute] Guid id)
        {
            if(id.ValidateEmptyGuid())
                return BadRequest();

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "DeleteSiteById", "SitesController", TraceId);
            Response<SiteDataModel> response = new Response<SiteDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _siteService.DeleteSiteById(id);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "DeleteSiteById", "SiteService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SiteDataModel> { ResponseCode = (int)Code.success, ResponseMessage = result,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "DeleteSiteById", "SitesController", TraceId);	

                await  _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<IEnumerable<SiteDataModel>>
                {
                    TransactionData = Enumerable.Repeat(result,1),
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId=TraceId
                }).ConfigureAwait(false);

                if(result == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "DeleteSiteById", "SitesController", TraceId, ex);
                response = new Response<SiteDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }

        }


        /// <summary>
        /// Update Site
        /// </summary>
        /// <param name="siteData"></param>
        /// <returns></returns>
        [HttpPut("")]
        [EnableCors("odlPolicy")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> UpdateSite([FromBody] SiteDataModel siteData)
        {
            if(siteData.ValidateObjectForNull()) //todo validations
                return BadRequest();

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "UpdateSite", "SitesController", TraceId);
            Response<SiteDataModel> response = new Response<SiteDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                var validationresult= await _siteService.ValidateSite(siteData.Url);
                if(!validationresult)
                    return BadRequest(new Response<SiteDataModel> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Invalid Site URL"),TraceId=TraceId});               
                
                
                var result = await _siteService.UpdateSite(siteData);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "UpdateSite", "SiteService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SiteDataModel> { ResponseCode = (int)Code.success, ResponseMessage = result,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "UpdateSite", "SitesController", TraceId);	
                 await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<IEnumerable<SiteDataModel>>
                {
                    TransactionData = Enumerable.Repeat(result,1),
                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId=TraceId
                }).ConfigureAwait(false);
                
                if(result == null)
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "UpdateSite", "SitesController", TraceId, ex);
                //response = new Response<SiteDataModel> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString() };

                if(ex.IsUniqueConstraintViolated())
                    return BadRequest(new Response<SiteDataModel> { ResponseCode = (int)Code.BadRequest, Message = string.Format("Update failed. Another site with the same Name or Url exists."),TraceId=TraceId});

               if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


         /// <summary>
        /// Validate Site
        /// </summary>
        /// <returns></returns>
        // [HttpGet("GetAll")]
        [HttpGet("url")]
        public async Task<IActionResult> ValidateSite(string url)
        {
            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "ValidateSite", "SitesController", TraceId);
            Response<bool> response = new Response<bool>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _siteService.ValidateSite(url);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "ValidateSite", "SiteService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<bool> { ResponseCode = (int)Code.success, ResponseMessage = result,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "ValidateSite", "SitesController", TraceId);	

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "ValidateSite", "SitesController", TraceId, ex);
                response = new Response<bool> { ResponseCode = (int)Code.exceptionError, Message = ex.ToString(),TraceId=TraceId };
                if(ex is TimeoutException)
                    return StatusCode(408);
                
                return StatusCode(500,response);
            }
        }


    }
}