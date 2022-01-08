//moved to travel studio controller

// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Threading.Tasks;
// using MarketPlaceService.API.CustomEntities;
// using MarketPlaceService.BLL.Contracts;
// using MarketPlaceService.Entities;
// using System.Net;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Microsoft.AspNetCore.Cors;
// using Newtonsoft.Json;

// namespace MarketPlaceService.API.Controllers
// {
//     [Produces("application/json")]
//     [Route("api/v{version:apiVersion}/serviceStatuses")]
//     [ApiVersion("1.0")]
//     [ApiController]
//     public class ServiceStatusesController : ControllerBase
//     {
//         private ILogger<ServiceStatusesController> _logger;
//         private readonly IServiceStatusesService _serviceStatusesService;

//         public ServiceStatusesController(ILogger<ServiceStatusesController> logger, IServiceStatusesService serviceStatusesService)
//         {
//             _logger = logger;
//             _serviceStatusesService = serviceStatusesService;
// ;
//         }

//         [HttpGet("GetServiceStatuses")]
//         public async Task<IActionResult> GetServiceStatuses(Guid entityId,EntityTypeEnum entityType)
//         {
            
//             _logger.LogInformation("GetServiceStatuses is called.");
//             Response<IEnumerable<ServiceStatus>> response = new Response<IEnumerable<ServiceStatus>>();
//             try
//             {
//                 _logger.LogInformation("GetServiceStatuses service is called.");
//                 var watch = Stopwatch.StartNew();
//                 // Service call
//                var result = await _serviceStatusesService.GetServiceStatusAsync(entityId,entityType);
//                var finalResult = JsonConvert.DeserializeObject<Response<IEnumerable<ServiceStatus>>>(result);
//                 watch.Stop();
//                 _logger.LogInformation("Execution Time of GetServiceStatuses call is: {duration}ms", watch.ElapsedMilliseconds);
//                 response = new Response<IEnumerable<ServiceStatus>>
//                 {
//                     ResponseCode = (int)Code.success,
//                     Status = "Success",
//                     ResponseMessage = finalResult.ResponseMessage,
//                     ExecutionTimeMS = watch.ElapsedMilliseconds,
//                     TraceId = Guid.NewGuid()
//                 };
//                 _logger.LogInformation("GetServiceStatuses executed successfully.");

//                 return Ok(response);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex.ToString());
//                 response = new Response<IEnumerable<ServiceStatus>>
//                 {
//                     ResponseCode = (int)Code.exceptionError,
//                     Status = "Failure",
//                     Message = ex.ToString(),
//                     TraceId = Guid.NewGuid()
//                 };
//                 return BadRequest(response);
//             }
//         }
//     }
// }
