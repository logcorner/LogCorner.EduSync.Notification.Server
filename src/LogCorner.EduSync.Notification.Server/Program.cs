using LogCorner.EduSync.Speech.Telemetry.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LogCorner.EduSync.Notification.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging((context, loggingBuilder) =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddSerilog(context.Configuration);
                        loggingBuilder.AddOpenTelemetry( context.Configuration);
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}