using Serilog.Events;

namespace LogginServiceAPI.Helpers
{
    public static class LogHelper
    {
        public static LogEventLevel GetLogLevel(string level)
        {
            switch (level?.ToLowerInvariant())
            {
                case "debug":
                    return LogEventLevel.Debug;
                case "information":
                    return LogEventLevel.Information;
                case "warning":
                    return LogEventLevel.Warning;
                case "error":
                    return LogEventLevel.Error;
                case "fatal":
                    return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Verbose;
            }
        }
    }
}
