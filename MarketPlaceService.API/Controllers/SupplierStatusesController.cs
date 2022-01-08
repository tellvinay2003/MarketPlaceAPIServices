//moved to travel studio controller

// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Linq;
// using System.Net;
// using System.Threading.Tasks;
// using AutoMapper;
// using MarketPlaceService.API.CustomEntities;
// using MarketPlaceService.API.Models;
// using MarketPlaceService.BLL.Contracts;
// using MarketPlaceService.Entities;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;
// using Swashbuckle.AspNetCore.Annotations;
// using Microsoft.AspNetCore.Cors;

// namespace MarketPlaceService.API.Controllers
// {
//     //[Authorize]
//     // [Route("api/[controller]")]
//     [Produces("application/json")]
//     // [Route("api/v{version:apiVersion}/[controller]/{action}")]
//     [Route("api/v{version:apiVersion}/supplierStatuses")]
//     [ApiVersion("1.0")]
//     [ApiController]
//     public class SupplierStatusesController : ControllerBase
//     {
//         readonly ILogger<SupplierStatusesController> _logger;
//         readonly ISupplierStatusesService _supplierStatusesService;
//         public SupplierStatusesController(ISupplierStatusesService supplierStatusesService, ILogger<SupplierStatusesController> logger)
//         {
//             _logger = logger;
//             _supplierStatusesService = supplierStatusesService;
//         }

//         /// <summary>
//         /// Get list of all supplier statuses
//         /// </summary>
//         /// <param name="entityId"></param>
//         /// <param name="entityType"></param>
//         /// <returns></returns>
//         [HttpGet("GetAllSupplierStatuses")]
//         public async Task<IActionResult> GetAllSupplierStatuses(Guid entityId,EntityTypeEnum entityType)
//         {
//             _logger.LogInformation("GetAllSupplierStatuses is called.");
//             Response<IEnumerable<SupplierStatusDataModel>> response = new Response<IEnumerable<SupplierStatusDataModel>>();
//             try
//             {
//                 _logger.LogInformation("Supplier Status service is called.");
//                 var watch = Stopwatch.StartNew();
//                 // Service call
//                 var result = await _supplierStatusesService.GetAllSupplierStatusesAsync(entityId,entityType);
//                 watch.Stop();
//                 _logger.LogInformation("Execution Time of Supplier Status service for GetAllSupplierStatusesAsync call is: {duration}ms", watch.ElapsedMilliseconds);

//                 var finalOutput = JsonConvert.DeserializeObject<Response<IEnumerable<SupplierStatusDataModel>>>(result);

//                 response = new Response<IEnumerable<SupplierStatusDataModel>>
//                 {
//                     ResponseCode = (int)Code.success,
//                     Status = "Success",
//                     ResponseMessage = finalOutput.ResponseMessage,
//                     ExecutionTimeMS = watch.ElapsedMilliseconds,
//                     TraceId = Guid.NewGuid()
//                 };
//                 _logger.LogInformation("GetServiceTypes executed successfully.");

//                 return Ok(response);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex.ToString());
//                 response = new Response<IEnumerable<SupplierStatusDataModel>>
//                 {
//                     ResponseCode = (int)Code.exceptionError,
//                     Status = "Failure",
//                     Message = ex.ToString(),
//                     TraceId = Guid.NewGuid()
//                 };
//                 return BadRequest(response);
//             }
//         }

//         /// <summary>
//         /// Get supplier status name by supplier status id
//         /// </summary>
//         /// <param name="publisherId"></param>
//         /// <param name="supplierStatusId"></param>
//         /// <returns></returns>
//         [HttpGet("GetSupplierStatusById")]
//         public async Task<IActionResult> GetSupplierStatusById(Guid entityId,EntityTypeEnum entityType, int supplierStatusId)
//         {
//             _logger.LogInformation("GetSupplierStatus is called.");
//             Response<SupplierStatusDataModel> response = new Response<SupplierStatusDataModel>();
//             try
//             {
//                 _logger.LogInformation("Supplier Status service is called.");
//                 var watch = Stopwatch.StartNew();
//                 // Service call
//                 var result = await _supplierStatusesService.GetSupplierStatusByIdAsync(entityId,entityType, supplierStatusId);
//                 watch.Stop();
//                 _logger.LogInformation("Execution Time of Supplier Status service for GetSupplierStatusAsync call is: {duration}ms", watch.ElapsedMilliseconds);

//                 var finalOutput = JsonConvert.DeserializeObject<Response<SupplierStatusDataModel>>(result);

//                 response = new Response<SupplierStatusDataModel>
//                 {
//                     ResponseCode = (int)Code.success,
//                     Status = "Success",
//                     ResponseMessage = finalOutput.ResponseMessage,
//                     ExecutionTimeMS = watch.ElapsedMilliseconds,
//                     TraceId = Guid.NewGuid()
//                 };
//                 _logger.LogInformation("GetServiceTypes executed successfully.");

//                 return Ok(response);
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex.ToString());
//                 response = new Response<SupplierStatusDataModel>
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