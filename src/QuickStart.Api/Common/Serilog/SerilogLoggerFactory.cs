using Serilog;
using Serilog.Core;

namespace QuickStart.Api.Common.Serilog;

public static class SerilogLoggerFactory
{
    public static Logger CreateLogger()
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("serilog.settings.json", optional: false, reloadOnChange: true)
        .Build();

        return new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
    }
}
