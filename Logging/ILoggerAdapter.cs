using Microsoft.Extensions.Logging;

namespace Abstractions.Logging;

/// <summary>
/// Wraps the .net logger functionality to ensure there is no unnecessary memory alocation for arguments
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ILoggerAdapter<T>
{
    void Log(string message);

    void Log<T0>(string message, T0 arg0);

    void Log<T0, T1>(string message, T0 arg0, T1 arg1);

    void Log<T0, T1, T2>(string message, T0 arg0, T1 arg1, T2 arg2);
}