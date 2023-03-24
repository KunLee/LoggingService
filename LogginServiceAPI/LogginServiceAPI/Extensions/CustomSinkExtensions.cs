using LogginServiceAPI.Sinks;
using Serilog.Configuration;
using Serilog;

namespace LogginServiceAPI.Extensions
{
    /// <summary>
    /// Provide an extension method for configuring a custom logging sink in the Serilog logging framework
    /// </summary>
    public static class CustomSinkExtensions
    {
        public static LoggerConfiguration CustomSink(
                  this LoggerSinkConfiguration loggerConfiguration)
        {
            return loggerConfiguration.Sink(new CustomSink());
        }
    }
}
