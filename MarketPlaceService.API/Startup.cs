using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using MarketPlaceService.API.Middleware;
using MarketPlaceService.API.Models;
using MarketPlaceService.API.Swagger;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using MarketPlaceService.BLL.Contracts;
using MarketPlaceService.BLL;
using MarketPlaceService.DAL;
using MarketPlaceService.DAL.Contract;
using Microsoft.EntityFrameworkCore;
using MarketPlaceService.DAL.Models;
using MarketPlaceService.BLL.Contracts.UtilityServiceContracts;
using MarketPlaceService.BLL.UtilityService;

namespace MarketPlaceService.API
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IHostingEnvironment _env;
        private readonly ObjectPoolProvider _objectPoolProvider;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment env, ObjectPoolProvider objectPoolProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _env = env;
            _objectPoolProvider = objectPoolProvider;
        }

        public IConfiguration _configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddControllers().AddNewtonsoftJson();           

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient();

            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                opt.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "odlPolicy",
                    builder =>
                    {
                         builder.WithOrigins("http://localhost:3000","https://localhost:5001","http://localhost:5000","http://localhost").AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            //services.AddSwaggerGen(opt =>
            //{
            //    opt.ExampleFilters();

            //    var xmlPath = GetXmlDataAnnotationFilePath();
            //    if (!string.IsNullOrEmpty(xmlPath))
            //    {
            //        opt.IncludeXmlComments(xmlPath);
            //    }

            //    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            //    {
            //        Description = "Authorization header. Example: \"bearer {token}\"",
            //        In = ParameterLocation.Header,
            //        Name = "authorization",
            //        Type = SecuritySchemeType.ApiKey
            //    });
            //    opt.OperationFilter<SecurityRequirementsOperationFilter>();
            //});



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://quizdeveloper.com/term-and-condition"),
                    Contact = new OpenApiContact
                    {
                        Name = "QuizDev",
                        Email = string.Empty,
                        Url = new Uri("https://quizdeveloper.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use license",
                        Url = new Uri("https://quizdeveloper.com"),
                    }
                });
            });


            services.AddSwaggerExamplesFromAssemblyOf<Program>();

            //var logger = new LoggerConfiguration()
            //   .Enrich.FromLogContext()
            //   .ReadFrom.Configuration(_configuration);

            //Serilog.Log.Logger = logger.CreateLogger();
            //Serilog.Log.Information("web api service is started.");
            

            services.AddDbContext<MarketplaceDbContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"), sqlServerOptions => sqlServerOptions.CommandTimeout(120)));

            services.AddServices();
            services.AddRepositories();
            services.AddUtilities();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();


                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(o => o.GroupName))
                    {
                        c.SwaggerEndpoint(
                            $"{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(options => options.AllowAnyOrigin()
            .AllowAnyHeader());  

            app.UseHttpContextMiddleware();

            app.UseSwaggerUrlExtension();

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //{
            //    foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(o => o.GroupName))
            //    {
            //        c.SwaggerEndpoint(
            //            $"{description.GroupName}/swagger.json",
            //            description.GroupName.ToUpperInvariant());
            //    }
            //});

            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned API v1.0");
            //    options.RoutePrefix = string.Empty;
            //    options.DocumentTitle = "Title Documentation";
            //    options.DocExpansion(DocExpansion.List);
            //});


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseWebServices();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //loggerFactory.AddFile(_configuration.GetSection("Logging"));

            _configuration = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }

        #region internal

        private string GetXmlDataAnnotationFilePath()
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (!File.Exists(xmlPath))
            {
                return null;
            }

            return xmlPath;
        }

        #endregion
    }
}
