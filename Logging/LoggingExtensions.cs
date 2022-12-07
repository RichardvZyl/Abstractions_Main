
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Abstractions.Logging;

public static class LoggingExtensions
{
    public static IHostBuilder UseSerilog(IHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .AddEnvironmentVariables()
                .Build();

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

        _ = SerilogHostBuilderExtensions.UseSerilog(builder);

        return builder;
    }
}
