using Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace Logging;

/// <inheritdoc />
public class LoggerAdapter<T> : ILoggerAdapter<T> where T : ILogger
{
    private readonly LogLevel _logLevel;
    private readonly ILogger<T> _logger;

    public LoggerAdapter(ILoggerProvider loggerProvider, LogLevel logLevel)
    {
        _logger = new Logger<T>(LoggerFactory.Create(builder =>
            _ = builder.AddProvider(loggerProvider).SetMinimumLevel(logLevel)));

        _logLevel = logLevel;
    }


    public void Log(string message)
    {
        if (_logger.IsEnabled(_logLevel))
            _logger.Log(_logLevel, message);
    }

    public void Log<T0>(string message, T0 arg0)
    {
        if (_logger.IsEnabled(_logLevel))
            _logger.Log(_logLevel, message, arg0);
    }

    public void Log<T0, T1>(string message, T0 arg0, T1 arg1)
    {
        if (_logger.IsEnabled(_logLevel))
            _logger.Log(_logLevel, message, arg0, arg1);
    }

    public void Log<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2)
    {
        if (_logger.IsEnabled(_logLevel))
            _logger.Log(_logLevel, message, arg0, arg1, arg2);
    }
}
