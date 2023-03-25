using Serilog.Events;

namespace LogginServiceAPI.Helpers
{
    public static class LogHelper
    {
        public static LogLevel GetLogLevel(string level)
        {
            switch (level?.ToLowerInvariant())
            {
                case "trace":
                    return LogLevel.Trace;
                case "debug":
                    return LogLevel.Debug;
                case "information":
                    return LogLevel.Information;
                case "warning":
                    return LogLevel.Warning;
                case "error":
                    return LogLevel.Error;
                case "fatal":
                    return LogLevel.Critical;
                default:
                    return LogLevel.None;
            }
        }
    }
}
