using GameServer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

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

            services.AddSingleton(loggerFactory)
                    .AddScoped<IGameRepository, GameRepository>()
                    .AddMvc(c => c.Filters.Add(new ExceptionMapper(loggerFactory.CreateLogger<ExceptionMapper>())));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
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