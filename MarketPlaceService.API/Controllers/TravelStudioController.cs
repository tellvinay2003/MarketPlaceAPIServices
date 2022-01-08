using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MarketPlaceService.API.CustomEntities;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.Entities;
using MarketPlaceService.Entities.Job;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using MarketPlaceService.API.Utilities;
using CommonUtilities;
using System.Linq;

namespace MarketPlaceService.API.Controllers
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TravelStudioController : ControllerBase
    {
        private ILogger _logger;
        private readonly ITravelStudioService _TravelStudioService;
        private readonly IAgentsService _agentsService;
        private readonly IServiceStatusesService _serviceStatusesService;
        private readonly ISupplierStatusesService _supplierStatusesService;
        private readonly ITransactionLoggerService<MarketplaceDataModel> _transactionLoggerService;

        private readonly IPackageStatuses _PackageStatuses;

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
            _TravelStudioService.TraceId = _traceId;
            _agentsService.TraceId =_traceId;
            _serviceStatusesService.TraceId =_traceId;
            _supplierStatusesService.TraceId =_traceId;
            _PackageStatuses.TraceId =_traceId;
        }
        public TravelStudioController(ILogger<TravelStudioController> logger, ITransactionLoggerService<MarketplaceDataModel> transactionLoggerService, ITravelStudioService TravelStudioService, IAgentsService agentsService, IServiceStatusesService serviceStatusesService, ISupplierStatusesService supplierStatusesService,IPackageStatuses PackageStatuses)
        {
            _logger = logger;
            _TravelStudioService = TravelStudioService;
            _agentsService = agentsService;
            _serviceStatusesService = serviceStatusesService;
            _supplierStatusesService = supplierStatusesService;
            _transactionLoggerService = transactionLoggerService;
            _PackageStatuses = PackageStatuses;
        }


        [HttpGet("bookingtypes/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<BookingTypeDataModel>>> GetAllBookingTypes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllBookingTypes", "TravelStudioController", TraceId);
            Response<IEnumerable<BookingTypeDataModel>> response = new Response<IEnumerable<BookingTypeDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetAllBookingTypesAsync(entityId, entityType);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<BookingTypeDataModel>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllBookingTypesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<BookingTypeDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllBookingTypes", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllBookingTypes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<BookingTypeDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };

                if (ex is TimeoutException)
                    return StatusCode(408);

                return StatusCode(500,response);
            }
        }


        [HttpGet("pricetypes/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<PriceTypeDataModel>>> GetAllPriceTypes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllPriceTypes", "TravelStudioController", TraceId);
            Response<IEnumerable<PriceTypeDataModel>> response = new Response<IEnumerable<PriceTypeDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetAllPriceTypesAsync(entityId, entityType);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<PriceTypeDataModel>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllPriceTypesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<PriceTypeDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllPriceTypes", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllPriceTypes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<PriceTypeDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }


        [HttpGet("communicationtypes/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<CommunicationTypeDataModel>>> GetAllCommunicatonTypes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllCommunicatonTypes", "TravelStudioController", TraceId);
            Response<IEnumerable<CommunicationTypeDataModel>> response = new Response<IEnumerable<CommunicationTypeDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetAllCommunicationTypesAsync(entityId, entityType);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<CommunicationTypeDataModel>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllCommunicationTypesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<CommunicationTypeDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllCommunicatonTypes", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllCommunicatonTypes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<CommunicationTypeDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }


        [HttpGet("seasontypes/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<SeasonTypesDataModel>>> GetAllSeasonTypes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllSeasonTypes", "TravelStudioController", TraceId);
            Response<IEnumerable<SeasonTypesDataModel>> response = new Response<IEnumerable<SeasonTypesDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetAllSeasonTypesAsync(entityId, entityType);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<SeasonTypesDataModel>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllSeasonTypesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SeasonTypesDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllSeasonTypes", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllSeasonTypes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<SeasonTypesDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }


        [HttpGet("chargingPolicies/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<ChargingPolicyDataModel>>> GetAllChargingPolicy(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllChargingPolicy", "TravelStudioController", TraceId);
            Response<IEnumerable<ChargingPolicyDataModel>> response = new Response<IEnumerable<ChargingPolicyDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetAllChargingPolicyAsync(entityId, entityType);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<ChargingPolicyDataModel>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllChargingPolicyAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<ChargingPolicyDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllChargingPolicy", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllChargingPolicy", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<ChargingPolicyDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }


        [HttpGet("suppliers/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<SupplierDataModel>>> GetSuppliers(Guid entityId, EntityType entityType, string supplierName)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSuppliers", "TravelStudioController", TraceId);
            Response<IEnumerable<SupplierDataModel>> response = new Response<IEnumerable<SupplierDataModel>>();
            try
            {
                _logger.LogInformation("SupplierName service is called.");
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetSuppliersAsync(entityId, entityType, supplierName);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<SupplierDataModel>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSuppliersAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<SupplierDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSuppliers", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSuppliers", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<SupplierDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }


        [HttpGet("suppliers/{entityId}/{entityType}/{supplierId}")]
        public async Task<ActionResult<SupplierDataModel>> GetSupplierName(Guid entityId, EntityType entityType, int supplierId)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierName", "TravelStudioController", TraceId);
            Response<SupplierDataModel> response = new Response<SupplierDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetSupplierNameAsync(entityId, entityType, supplierId);
                var data = JsonConvert.DeserializeObject<Response<SupplierDataModel>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSupplierNameAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<SupplierDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierName", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSupplierName", "TravelStudioController", TraceId, ex);
                response = new Response<SupplierDataModel>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    //Message = ex.ToString(),
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }

        [HttpGet("productcodes/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<ProductCodeData>>> GetProductCodes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetProductCodes", "TravelStudioController", TraceId);
            Response<IEnumerable<ProductCodeData>> response = new Response<IEnumerable<ProductCodeData>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetProductCodesAsync(entityId, entityType);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<ProductCodeData>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetProductCodesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<ProductCodeData>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetProductCodes", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetProductCodes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<ProductCodeData>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    //Message = ex.ToString(),
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }


        [HttpGet("regions/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetRegions(Guid entityId, EntityType entityType, string name)
        {
            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegions", "TravelStudioController", TraceId);
            Response<IEnumerable<RegionData>> response = new Response<IEnumerable<RegionData>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetRegions(entityId, entityType, name);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetRegions", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<RegionData>>>(result);
                response = new Response<IEnumerable<RegionData>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetRegions", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetRegions", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<RegionData>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // = ex.ToString(),
                    TraceId = TraceId,
                    ErrorMessage = ex.Message
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("agents/{entityId}/{entityType}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetAgents(Guid entityId, EntityType entityType, string agentName, int limit)
        {

            ActivateTrace();
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAgents", "TravelStudioController", TraceId);
            Response<IEnumerable<Agent>> response = new Response<IEnumerable<Agent>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _agentsService.GetAgents(entityId, entityType, agentName, limit);
                var finalResult = JsonConvert.DeserializeObject<Response<IEnumerable<Agent>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAgents", "AgentsService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<Agent>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalResult.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAgents", "TravelStudioController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAgents", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<Agent>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    //Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }
        }

        [HttpGet("agents/{entityId}/{entityType}/{agentId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetAgentById(Guid entityId, EntityType entityType, int agentId)
        {

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAgentById", "TravelStudioController", TraceId);
            Response<Agent> response = new Response<Agent>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _agentsService.GetAgentById(entityId, entityType, agentId);
                var finalResult = JsonConvert.DeserializeObject<Response<Agent>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAgentById", "AgentService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<Agent>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalResult.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAgentById", "TravelStudioController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAgentById", "TravelStudioController", TraceId, ex);
                response = new Response<Agent>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    //Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }
        }

        [HttpGet("servicestatuses/{entityId}/{entityType}")]
        public async Task<IActionResult> GetServiceStatuses(Guid entityId, EntityType entityType)
        {

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceStatuses", "TravelStudioController", TraceId);
            Response<IEnumerable<ServiceStatus>> response = new Response<IEnumerable<ServiceStatus>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _serviceStatusesService.GetServiceStatusAsync(entityId, entityType);
                var finalResult = JsonConvert.DeserializeObject<Response<IEnumerable<ServiceStatus>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServiceStatusAsync", "ServiceStatusesService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<ServiceStatus>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalResult.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceStatuses", "TravelStudioController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetServiceStatuses", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<ServiceStatus>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }
        }

        //packagestatuses

        [HttpGet("packagestatuses/{entityId}/{entityType}")]
        public async Task<IActionResult> GetPackageStatuses(Guid entityId, EntityType entityType)
        {

            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPackageStatuses", "TravelStudioController", TraceId);
            Response<IEnumerable<PackageStatus>> response = new Response<IEnumerable<PackageStatus>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Package call
                var result = await _PackageStatuses.GetPackageStatusAsync(entityId, entityType);
                var finalResult = JsonConvert.DeserializeObject<Response<IEnumerable<PackageStatus>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPackageStatusAsync", "StatusesService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<PackageStatus>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalResult.ResponseMessage.OrderBy(res => res.Name),
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetPackageStatuses", "TravelStudioController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPackageStatuses", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<PackageStatus>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get list of all supplier statuses
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [HttpGet("supplierstatuses/{entityId}/{entityType}")]
        public async Task<IActionResult> GetAllSupplierStatuses(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllSupplierStatuses", "TravelStudioController", TraceId);
            Response<IEnumerable<SupplierStatusDataModel>> response = new Response<IEnumerable<SupplierStatusDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _supplierStatusesService.GetAllSupplierStatusesAsync(entityId, entityType);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllSupplierStatusesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);

                var finalOutput = JsonConvert.DeserializeObject<Response<IEnumerable<SupplierStatusDataModel>>>(result);

                response = new Response<IEnumerable<SupplierStatusDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllSupplierStatuses", "TravelStudioController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllSupplierStatuses", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<SupplierStatusDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    //Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get supplier status name by supplier status id
        /// </summary>
        /// <param name="publisherId"></param>
        /// <param name="supplierStatusId"></param>
        /// <returns></returns>
        [HttpGet("supplierstatuses/{entityId}/{entityType}/{supplierStatusId}")]
        public async Task<IActionResult> GetSupplierStatusById(Guid entityId, EntityType entityType, int supplierStatusId)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetSupplierStatusById", "TravelStudioController", TraceId);
            Response<SupplierStatusDataModel> response = new Response<SupplierStatusDataModel>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _supplierStatusesService.GetSupplierStatusByIdAsync(entityId, entityType, supplierStatusId);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetSupplierStatusByIdAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);

                var finalOutput = JsonConvert.DeserializeObject<Response<SupplierStatusDataModel>>(result);

                response = new Response<SupplierStatusDataModel>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetSupplierStatusById", "TravelStudioController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetSupplierStatusById", "TravelStudioController", TraceId, ex);
                response = new Response<SupplierStatusDataModel>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    //Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get list of all service types
        /// </summary>
        /// <returns></returns>
        // [HttpGet("")]
        [HttpGet("servicetypes/{entityId}/{entityType}")]

        public async Task<IActionResult> GetServiceTypes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceTypes", "TravelStudioController", TraceId);
            Response<IEnumerable<ServiceTypeDataModel>> response = new Response<IEnumerable<ServiceTypeDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetServiceTypesAsync(entityId, entityType);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServiceTypesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);

                var finalOutput = JsonConvert.DeserializeObject<Response<IEnumerable<ServiceTypeDataModel>>>(result);
                
                response = new Response<IEnumerable<ServiceTypeDataModel>> { ResponseCode = (int)Code.success, ResponseMessage = finalOutput.ResponseMessage,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceTypes", "TravelStudioController", TraceId);


                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        ServiceTypeCollection = finalOutput.ResponseMessage
                    },

                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);


                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetServiceTypes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<ServiceTypeDataModel>> { ResponseCode = (int)Code.exceptionError,TraceId=TraceId };
                return BadRequest(response);
            }
        }

        [HttpGet("packagetypes/{entityId}/{entityType}")]
        public async Task<IActionResult> GetPackageTypes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetPackageTypes", "TravelStudioController", TraceId);
            Response<IEnumerable<PackageTypeDataModel>> response = new Response<IEnumerable<PackageTypeDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetPackageTypesAsync(entityId, entityType);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetPackageTypesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);


                var finalOutput = JsonConvert.DeserializeObject<Response<IEnumerable<PackageTypeDataModel>>>(result);
                
                response = new Response<IEnumerable<PackageTypeDataModel>> { ResponseCode = (int)Code.success, ResponseMessage = finalOutput.ResponseMessage,TraceId=TraceId };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetPackageTypes", "TravelStudioController", TraceId);


                await _transactionLoggerService.SaveTransactionLog(new TransactionLogDataModel<MarketplaceDataModel>
                {
                    TransactionData = new MarketplaceDataModel
                    {
                        PackageTypeCollection = finalOutput.ResponseMessage
                    },

                    TransactionStatus = "Success",
                    TransactionType = "Information",
                    InitiatedBy = HttpContext.User.ToString(),
                    InitiatedOn = DateTime.UtcNow,
                    TraceId = TraceId
                }).ConfigureAwait(false);


                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetPackageTypes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<PackageTypeDataModel>> { ResponseCode = (int)Code.exceptionError,TraceId=TraceId };
                return BadRequest(response);
            }
        }


        [HttpGet("regions/{entityId}/{entityType}/{id}")]
        public async Task<ActionResult<RegionData>> GetRegionById(Guid entityId, EntityType entityType, int id)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetRegionById", "TravelStudioController", TraceId);
            Response<RegionData> response = new Response<RegionData>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetRegionById(entityId, entityType, id);
                var data = JsonConvert.DeserializeObject<Response<RegionData>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetRegionById", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<RegionData>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetRegionById", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetRegionById", "TravelStudioController", TraceId, ex);
                response = new Response<RegionData>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    //Message = ex.ToString(),
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }

        /// <summary>
        /// Get specific publisher details by PublisherId
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [HttpGet("organisations/{entityId}/{entityType}")]
        // [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(OrganizationDataModel), Description = "Returns found Publisher")]
        // [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        // [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        // [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetOrganization(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetOrganization", "TravelStudioController", TraceId);
            Response<IEnumerable<OrganizationDataModel>> response = new Response<IEnumerable<OrganizationDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetOrganization(entityId, entityType);

                var finalOutput = result == null ? null : JsonConvert.DeserializeObject<IEnumerable<OrganizationDataModel>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetOrganization", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<OrganizationDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetOrganization", "TravelStudioController", TraceId);

                if (finalOutput.ValidateObjectForNull())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetOrganization", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<OrganizationDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }

        }

        /// <summary>
        /// Get service type types
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        [HttpGet("servicetypetypes/{entityId}/{entityType}")]
        // [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(OrganizationDataModel), Description = "Returns found Publisher")]
        // [SwaggerResponse((int)HttpStatusCode.OK, Description = "Returns 200")]
        // [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Missing or invalid Publisher id")]
        // [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        public async Task<IActionResult> GetServiceTypeTypes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetServiceTypeTypes", "TravelStudioController", TraceId);
            Response<IEnumerable<ServiceTypeType>> response = new Response<IEnumerable<ServiceTypeType>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetServiceTypeTypes(entityId, entityType);
                var finalOutput = JsonConvert.DeserializeObject<Response<IEnumerable<ServiceTypeType>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetServiceTypeTypes", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<ServiceTypeType>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetServiceTypeTypes", "TravelStudioController", TraceId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetServiceTypeTypes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<ServiceTypeType>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    Message = ex.ToString(),
                    TraceId = TraceId
                };
                return BadRequest(response);
            }

        }

        [HttpGet("mappingdatatypes/{entityId}/{entityType}/{dataTypeId}")]
        public async Task<ActionResult<IEnumerable<MappingDataTypesData>>> GetMappingTypesSiteData(Guid entityId, EntityType entityType, int dataTypeId, int? filterId)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetMappingTypesSiteData", "TravelStudioController", TraceId);
            Response<IEnumerable<MappingDataTypesData>> response = new Response<IEnumerable<MappingDataTypesData>>();
            try
            {
                _logger.LogInformation("CommunicationType service is called.");
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetMappingTypesSiteData(entityId, entityType, dataTypeId, filterId);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<MappingDataTypesData>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetMappingTypesSiteData", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<MappingDataTypesData>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetMappingTypesSiteData", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetMappingTypesSiteData", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<MappingDataTypesData>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }




        [HttpGet("Taxes/{entityId}/{entityType}")]
        public async Task<ActionResult<IEnumerable<Tax>>> GetAllTaxes(Guid entityId, EntityType entityType)
        {
            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetAllTaxes", "TravelStudioController", TraceId);
            Response<IEnumerable<Tax>> response = new Response<IEnumerable<Tax>>();
            try
            {
                var watch = Stopwatch.StartNew();
                var result = await _TravelStudioService.GetAllTaxesAsync(entityId, entityType);
                var data = JsonConvert.DeserializeObject<Response<IEnumerable<Tax>>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetAllTaxesAsync", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<Tax>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = data.ResponseMessage,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetAllTaxes", "TravelStudioController", TraceId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetAllTaxes", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<Tax>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    TraceId = TraceId
                };

                return BadRequest(response);
            }
        }


        [HttpGet("organisations/getprefix/{entityId}/{entityType}/{organisationId}")]
        [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetOrganizationsPrefix(Guid entityId, EntityType entityType, int organisationId)
        {
             if(entityId.ValidateEmptyGuid() || !Enum.IsDefined(typeof(EntityType), entityType) || organisationId.ValidateInteger())
                return BadRequest();


            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetOrganizationsPrefix", "TravelStudioController", TraceId);
            Response<IEnumerable<OrganizationPrefixDataModel>> response = new Response<IEnumerable<OrganizationPrefixDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetOrganizationsPrefix(entityId, entityType, organisationId);

                var finalOutput = result == null ? null : JsonConvert.DeserializeObject<IEnumerable<OrganizationPrefixDataModel>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetOrganizationsPrefix", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<OrganizationPrefixDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetOrganizationsPrefix", "TravelStudioController", TraceId);

                if (finalOutput.ValidateObjectForNull())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetOrganizationsPrefix", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<OrganizationPrefixDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };
                  if(ex is TimeoutException)
                    return StatusCode(408);
                return StatusCode(500,response);
            }

        }


         [HttpGet("users/getusers/{entityId}/{entityType}")]
         [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetUsers(Guid entityId, EntityType entityType,string name, int limit)
        {
             if(entityId.ValidateEmptyGuid() || !Enum.IsDefined(typeof(EntityType), entityType))
                return BadRequest();


            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetUsers", "TravelStudioController", TraceId);
            Response<IEnumerable<UserDataModel>> response = new Response<IEnumerable<UserDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetUsers(entityId, entityType,name,limit);

                var finalOutput = result == null ? null : JsonConvert.DeserializeObject<IEnumerable<UserDataModel>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetUsers", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<UserDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetUsers", "TravelStudioController", TraceId);

                if (finalOutput.ValidateObjectForNull())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetUsers", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<UserDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };

                   if(ex is TimeoutException)
                    return StatusCode(408);
                return StatusCode(500,response);
            }

        }


 [HttpGet("users/getuserbyid/{entityId}/{entityType}/{userId}")]
  public async Task<IActionResult> GetUserById(Guid entityId, EntityType entityType,int userId)
        {
             if(entityId.ValidateEmptyGuid() || !Enum.IsDefined(typeof(EntityType), entityType) || userId.ValidateInteger())
                return BadRequest();


            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetUserById", "TravelStudioController", TraceId);
            Response<IEnumerable<UserDataModel>> response = new Response<IEnumerable<UserDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetUserById(entityId, entityType,userId);

                var finalOutput = result == null ? null : JsonConvert.DeserializeObject<IEnumerable<UserDataModel>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetUserById", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<UserDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetUserById", "TravelStudioController", TraceId);

                if (finalOutput.ValidateObjectForNull())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetUserById", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<UserDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };

                   if(ex is TimeoutException)
                    return StatusCode(408);
                return StatusCode(500,response);
            }

        }





     [HttpGet("booking/getbookingstatus/{entityId}/{entityType}")]
         [EnableCors("odlPolicy")]
        public async Task<IActionResult> GetBookingStatus(Guid entityId, EntityType entityType)
        {
             if(entityId.ValidateEmptyGuid() || !Enum.IsDefined(typeof(EntityType), entityType))
                return BadRequest();


            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "GetBookingStatus", "TravelStudioController", TraceId);
            Response<IEnumerable<BookingStatusDataModel>> response = new Response<IEnumerable<BookingStatusDataModel>>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.GetBookingStatus(entityId, entityType);

                var finalOutput = result == null ? null : JsonConvert.DeserializeObject<IEnumerable<BookingStatusDataModel>>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "GetBookingStatus", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<IEnumerable<BookingStatusDataModel>>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "GetBookingStatus", "TravelStudioController", TraceId);

                if (finalOutput.ValidateObjectForNull())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingStatus", "TravelStudioController", TraceId, ex);
                response = new Response<IEnumerable<BookingStatusDataModel>>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };

                   if(ex is TimeoutException)
                    return StatusCode(408);
                return StatusCode(500,response);
            }

        }






        [HttpGet("job/jobsearch/{entityId}/{entityType}")]
         [EnableCors("odlPolicy")]
        public async Task<IActionResult> SearchJob(Guid entityId, EntityType entityType,BusinessProcess businessProcess,Enum queue,Enum jobType)
        {
             if(entityId.ValidateEmptyGuid() || !Enum.IsDefined(typeof(EntityType), entityType))
                return BadRequest();


            ActivateTrace();         
            LoggingHelper.LogInfo(_logger, LogType.Start, "SearchJob", "TravelStudioController", TraceId);
            Response<JobSearchResponse> response = new Response<JobSearchResponse>();
            try
            {
                var watch = Stopwatch.StartNew();
                // Service call
                var result = await _TravelStudioService.SearchJob(entityId, entityType,businessProcess,queue,jobType);

                var finalOutput = result == null ? null : JsonConvert.DeserializeObject<JobSearchResponse>(result);
                watch.Stop();
                LoggingHelper.LogPerformanceInfo(_logger, CallType.Service, "SearchJob", "TravelStudioService", TraceId, watch.ElapsedMilliseconds);
                response = new Response<JobSearchResponse>
                {
                    ResponseCode = (int)Code.success,
                    Status = "Success",
                    ResponseMessage = finalOutput,
                    ExecutionTimeMS = watch.ElapsedMilliseconds,
                    TraceId = TraceId
                };
                LoggingHelper.LogInfo(_logger, LogType.End, "SearchJob", "TravelStudioController", TraceId);

                if (finalOutput.ValidateObjectForNull())
                    return NotFound();

                return Ok(response);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(_logger, ExceptionType.System, "GetBookingStatus", "TravelStudioController", TraceId, ex);
                response = new Response<JobSearchResponse>
                {
                    ResponseCode = (int)Code.exceptionError,
                    Status = "Failure",
                    // Message = ex.ToString(),
                    TraceId = TraceId
                };

                   if(ex is TimeoutException)
                    return StatusCode(408);
                return StatusCode(500,response);
            }

        }






    }

}

