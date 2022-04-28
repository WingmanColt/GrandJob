
using HireMe.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HireMe
{
        public class Program
        {
            public static void Main(string[] args)
            {
                CreateHostBuilder(args).Build().Run();
            }

            public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        //  webBuilder.CaptureStartupErrors(true)
                        // .UseSetting("https_port", "5000")
                         webBuilder.UseUrls("http://grandjob.eu/", "https://localhost:44360/")
                        .UseStartup<Startup>();
                    }).ConfigureServices(services =>
                    {
                        services.AddHostedService<ConsumeScopedService>();
                        services.AddScoped<IDelayedTask, DelayedTask>();
                    });
    }
    
}