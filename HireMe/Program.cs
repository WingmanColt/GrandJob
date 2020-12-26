using HireMe.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;

namespace HireMe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
        .ConfigureLogging(config =>
         {

             config.ClearProviders();

          //   config.AddConfiguration(Configuration.GetSection("Logging"));
             config.AddDebug();
             config.AddEventSourceLogger();

             if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Microsoft.AspNetCore.Hosting.EnvironmentName.Development)
             {
                 config.AddConsole();
             }
         })


            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
              .ConfigureServices(services =>
               {
                   services.AddHostedService<ConsumeScopedService>();
                   services.AddScoped<IDelayedTask, DelayedTask>();
               });

        }
    }
}
