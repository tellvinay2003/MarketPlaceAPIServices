using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrmController : ControllerBase
    {
        [HttpGet("Status")]
        public ActionResult GetStatus()
        {
            
                return Ok();

        }

        [HttpGet("Type")]
        public ActionResult GetType()
        {

            return Ok();

        }
    }
}
