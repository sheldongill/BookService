using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace BookService
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var appName = System.Reflection.Assembly.GetEntryAssembly().GetName();

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                        optional: true)
                    .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();
            try
            {
                Log.Information("Initialising {appName}", appName);
                var host = WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .UseSerilog()
                        .Build();
                Log.Information("Starting {appName}", appName);
                host.Run();
                Log.Information("Stopping {appName}", appName);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected termination.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }

            return 0;
        }

        public static IWebHost BuildWebHost(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                        .UseSetting("EFDesignTime","yes")
                        .UseStartup<Startup>()
                        .Build();
    }
}
