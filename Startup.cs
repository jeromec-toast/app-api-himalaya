using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Tenant.API.Base.Startup;
using Tenant.API.Base.Util;
using Sa.Common.ADO.DataAccess;

namespace Tenant.Query
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XtraChef.Product.Startup"/> class.
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            //initialize the startup
            IConfigurationBuilder builder = TnBaseStartup.InitializeStartup(env);

            /*** Begin: API Specific Configuration ***/

            //set version number
            TnBaseStartup.Version = "1.0";

            //add user secrets
            builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
            // GJGwAWjB7C2tSSad65NO
            // I0FEO09BK0VEEN9DGJ9FGU8BMQ8DKTSP
            if (Configuration["ConnectionStrings:Default"] != null && Configuration["ConnectionStrings:Default"].Contains("MultipleActiveResultSets"))
            {
                Configuration["ConnectionStrings:Default"] = Configuration["ConnectionStrings:Default"].Replace("MultipleActiveResultSets=True;", "");
            }

            //Configuration["AWS.Logging:LogGroup"] = "xtraCHEF.Api.Product";

            /*** End: API Specific Configuration ***/
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //initialize base services
            TnBaseStartup.InitializeServices(Configuration, services);

            services.AddMvc().
                AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize);

            //menu contrext 
            services.AddDbContextPool<Context.UserContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("Default_SSL"));
                options.UseSqlServer(TnUtil.DecryptConnection.SetConnectionString(Configuration["ConnectionStrings:Default_SSL"]),
                sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                options.EnableSensitiveDataLogging(true);
                options.UseLoggerFactory(TnBaseStartup.LoggerFactory);
            });

            services.AddDbContextPool<Context.AppNotification.AppNotificationContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("Default_SSL"));
                options.UseSqlServer(TnUtil.DecryptConnection.SetConnectionString(Configuration["ConnectionStrings:Default_SSL"]),
                sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                options.EnableSensitiveDataLogging(true);
                options.UseLoggerFactory(TnBaseStartup.LoggerFactory);
            });

            //adding DbContext 'Invoice'
            services.AddDbContext<Context.Product.ProductContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("Default_SSL"));
                options.UseSqlServer(TnUtil.DecryptConnection.SetConnectionString(Configuration["ConnectionStrings:Default_SSL"]),
                sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                options.EnableSensitiveDataLogging(true);
                options.UseLoggerFactory(TnBaseStartup.LoggerFactory);
            });

            services.AddDbContext<Context.Authentication.AuthenticationContext>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("Default_SSL"));
                options.UseSqlServer(TnUtil.DecryptConnection.SetConnectionString(Configuration["ConnectionStrings:Default_SSL"]),
                sqlServerOptions => sqlServerOptions.CommandTimeout(60));
                options.EnableSensitiveDataLogging(true);
                options.UseLoggerFactory(TnBaseStartup.LoggerFactory);
            });

            //initialize scoped instances 
            services.AddScoped(typeof(Tenant.Query.Service.User.UserService), typeof(Tenant.Query.Service.User.UserService));
            services.AddScoped(typeof(Tenant.Query.Repository.User.UserRepository), typeof(Tenant.Query.Repository.User.UserRepository));

            services.AddScoped(typeof(Tenant.Query.Service.AppNotification.AppNotificationService), typeof(Tenant.Query.Service.AppNotification.AppNotificationService));
            services.AddScoped(typeof(Tenant.Query.Repository.AppNotification.AppNotificationRepository), typeof(Tenant.Query.Repository.AppNotification.AppNotificationRepository));

            //initialize scoped instance for product
            services.AddScoped(typeof(Service.Product.ProductService), typeof(Service.Product.ProductService));
            services.AddScoped(typeof(Repository.Product.ProductRepository), typeof(Repository.Product.ProductRepository));

            services.AddScoped(typeof(Service.Authentication.AuthenticationService), typeof(Service.Authentication.AuthenticationService));
            services.AddScoped(typeof(Repository.Authentication.AuthenticationRepository), typeof(Repository.Authentication.AuthenticationRepository));

            //Register the swagger generator, defining one or more swagger documnets
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(TnBaseStartup.Version, new OpenApiInfo { Title = "Tenant Query", Version = TnBaseStartup.Version });
                c.CustomSchemaIds(i => i.FullName);
            });

            services.AddSingleton<DataAccess>(_ => new DataAccess(TnUtil.DecryptConnection.SetConnectionString(Configuration["ConnectionStrings:Default"])));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //initialize application with base pipeline
            TnBaseStartup.InitializeApplication(Configuration, app, env, loggerFactory);

            /*** Begin: API Specific Configuration ***/

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{TnBaseStartup.Version}/swagger.json", $"Product API v{TnBaseStartup.Version}");
            });

            /*** End: API Specific Configuration ***/
        }
    }
}
