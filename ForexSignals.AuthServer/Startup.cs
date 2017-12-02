using System.IO;
using System.Text;
using ForexSignals.AuthServer.BusinessLogic;
using ForexSignals.Data.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ForexSignals.AuthServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc(opt =>
            {
                opt.Filters.Add(new RequireHttpsAttribute());
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddSingleton<UserActions, UserActions>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AppSettings> appSettings)
        {
            var config = appSettings.Value;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseCors(cfg =>
            {
                cfg.WithOrigins(appSettings.Value.UrlSettings.ClientUrl)
                    .WithMethods("OPTIONS", "GET", "POST");
            });

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config.UrlSettings.AuthUrl,
                    ValidAudience = config.UrlSettings.AuthUrl,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.AuthenticationSettings.SecretKey)),
                    ValidateLifetime = true
                }
            });

            app.UseMvc();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
