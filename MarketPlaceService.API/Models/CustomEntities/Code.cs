using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.API.CustomEntities
{
    public enum Code
    {
        success = 200,
        validationError = 2,
        dataNotFound = 3,
        exceptionError = 4,
        Timeout = 408,
        ServerError = 500,
        Created =  201,           
        NoContent = 204, 
        NotModified = 304, 
        BadRequest = 400,        
        Unauthorized = 401,
        ForBidden = 403,        
        NotFound = 404,
    }
}
