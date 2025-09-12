using System.Diagnostics;
using Serilog;
using Serilog.Debugging;

namespace QuickStart.Api.Extensions;
public static class SerilogExtensions
{
  public static void SetupSerilog(
    this WebApplicationBuilder builder, string settingsFile = "serilog.json")
  {
    SelfLog.Enable(message => Debug.WriteLine(message));
    try
    {
      var environment = builder.Environment.EnvironmentName;
      var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile(settingsFile, optional: false, reloadOnChange: true)
        .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true)
        .Build();

        Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

      builder.Host.UseSerilog((context, loggerConfig) =>
        loggerConfig.ReadFrom.Configuration(configuration));
     
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Failed to configure Serilog. Message: {ex.Message}");
      Console.WriteLine($"Failed to configure Serilog. Stack Trace: {ex.StackTrace}");
      
    }
    
  }
}