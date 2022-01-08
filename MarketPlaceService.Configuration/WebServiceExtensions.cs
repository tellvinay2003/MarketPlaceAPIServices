using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using MarketPlaceService.BLL;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL.Models;
using MarketPlaceService.DAL;
using MarketPlaceService.DAL.Contract;
using System;
using System.Linq;
using MarketPlaceService.DAL.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebServiceExtensions
    {
        public static IServiceCollection AddWebServices(
            this IServiceCollection services,
            IConfigurationSection BLLOptionsSection,
            IConfigurationSection DALOptionSection)
        {
            if (BLLOptionsSection == null)
            {
                throw new ArgumentNullException(nameof(BLLOptionsSection));
            }

            if (DALOptionSection == null)
            {
                throw new ArgumentNullException(nameof(DALOptionSection));
            }

            var bllSettings = BLLOptionsSection.Get<PublisherBLLOptions>();
            services.Configure<PublisherBLLOptions>(opt =>
            {
                opt.JwtSecretKey = BLLOptionsSection.GetValue<string>("JwtSecretKey");
                opt.WebApiUrl = BLLOptionsSection.GetValue<string>("WebApiUrl");
            });

            services.Configure<PublishersSqlRepositoryOption>(opt =>
            {
                opt.PublisherDbConnectionString = DALOptionSection.GetValue<string>("PublisherDbConnectionString");
            });

            services.TryAddSingleton<IPublisherRepository, PublisherRepository>();
            services.TryAddSingleton<ISiteRepository, SiteRepository>();
            services.TryAddScoped<IJwtTokenService, JwtTokenService>();

            services.AddHttpClient();
            services.AddHealthChecks()
                .AddCheck<PublisherRepository>("PublisherRepository");
            return services;
        }

        public static IApplicationBuilder UseWebServices(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/api/health", new HealthCheckOptions()
            {
                ResponseWriter = (httpContext, result) =>
                {
                    httpContext.Response.ContentType = "application/json";

                    var json = new JObject(
                        new JProperty("status", result.Status.ToString()),
                        new JProperty("results", new JObject(result.Entries.Select(pair =>
                            new JProperty(pair.Key, new JObject(
                                new JProperty("status", pair.Value.Status.ToString())))))));
                    return httpContext.Response.WriteAsync(
                        json.ToString(Formatting.Indented));
                }
            });

            return app;
        }
    }
}
