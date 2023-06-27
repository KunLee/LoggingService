using Microsoft.Extensions.Logging;

namespace LoggingService.Client.Helpers
{
    public static class LogHelper
    {
        public static LogLevel GetLogLevel(string level)
        {
            return (level?.ToLowerInvariant()) switch
            {
                "trace" => LogLevel.Trace,
                "debug" => LogLevel.Debug,
                "information" => LogLevel.Information,
                "warning" => LogLevel.Warning,
                "error" => LogLevel.Error,
                "fatal" => LogLevel.Critical,
                _ => LogLevel.None,
            };
        }
    }
}
