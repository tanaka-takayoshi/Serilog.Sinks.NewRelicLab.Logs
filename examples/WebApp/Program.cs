using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewRelic.LogEnrichers.Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.NewRelicLab.Logs;

namespace WebApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var file = File.CreateText(@"./selflog.txt");
            Serilog.Debugging.SelfLog.Enable(TextWriter.Synchronized(file));
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithNewRelicLogsInContext()
            .WriteTo.Console(new NewRelicFormatter())
            .WriteTo.NewRelicLogs()
            .WriteTo.File(new NewRelicFormatter(), @"D:\home\LogFiles\SeriLog\output.txt")
            .CreateLogger();

            try
            {
                Log.Information("Web Host‚ª‹N“®‚µ‚Ü‚µ‚½");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host‚ª—\Šú‚¹‚¸I—¹‚µ‚Ü‚µ‚½");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
