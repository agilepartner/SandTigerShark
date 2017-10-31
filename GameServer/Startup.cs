using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SandTigerShark.GameServer.Exceptions;
using SandTigerShark.GameServer.Services;
using SandTigerShark.GameServer.Services.Configurations;
using SandTigerShark.GameServer.Swagger;
using Swashbuckle.AspNetCore.Swagger;

namespace SandTigerShark.GameServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            LoggerFactory loggerFactory = ConfigureLogging();

            services.Configure<AzureConfig>(Configuration.GetSection("Azure"))
                    .AddSingleton(loggerFactory)
                    .AddMvc(c => c.Filters.Add(new ExceptionMapper(loggerFactory.CreateLogger<ExceptionMapper>())));
            Module.Bootstrap(services);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "SandTigerShark api", Version = "v1" });
                c.IncludeXmlComments($"{ApplicationEnvironment.ApplicationBasePath}\\SandTigerShark.GameServer.xml");
                c.DescribeAllEnumsAsStrings();
                c.OperationFilter<AddTokenHeaderOperationFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SandTigerShark api V1");
            });


            app.UseMvc();
        }

        private LoggerFactory ConfigureLogging()
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                         .AddDebug()
                         .AddNLog();

            return loggerFactory;
        }
    }
}