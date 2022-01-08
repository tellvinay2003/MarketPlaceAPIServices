using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MarketPlaceService.API.Middleware
{
    public class UseSwaggerUrlMiddleware
    {
        private readonly RequestDelegate _next;

        public UseSwaggerUrlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {   
            if(httpContext.Request.Path.Equals("/"))
                httpContext.Request.Path = "/swagger";

            await _next(httpContext);
        } 
    }

    public static class SwaggerUrlExtension
    {
        public static IApplicationBuilder UseSwaggerUrlExtension(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UseSwaggerUrlMiddleware>();
        }
    }
}
