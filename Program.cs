using Microsoft.AspNetCore;
using Serilog;
using Serilog.Events;

namespace MovieRecommender;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .WriteTo.File("logs/movieApi_logs.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Debug("Starting web host");
            CreateWebHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}