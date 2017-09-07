using System.IO;
using ForexSignals.AuthServer.BusinessLogic;
using ForexSignals.Data.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ForexSignals.AuthServer
{
    public class Startup
    {
         public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddSingleton<UserActions, UserActions>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();
            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
