using LogginServiceAPI.Helpers;
using LogginServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;

namespace LogginServiceAPI.Controllers
{
    /// <summary>
    /// The logging controller is responsible for handling the remote API logging requests
    /// </summary>
    public class LoggingController : Controller
    {
        private readonly ILogger<LoggingController> _logger;
        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] LogRequest request)
        {
            if (request == null || !request.Entries.Any())
            {
                return BadRequest("The request payload is empty or missing log entries.");
            }

            var logEvents = new List<LogEvent>();

            foreach (var entry in request.Entries)
            {
                //var logEvent = new LogEvent(
                //    entry.TimeStamp ?? DateTimeOffset.UtcNow,
                //    LogHelper.GetLogLevel(entry.LogLevel),
                //    null,
                //    new MessageTemplateParser().Parse(entry.Message),
                //    new[] { new LogEventProperty("LogSource", new ScalarValue(entry.TimeStamp)),
                //    new LogEventProperty("Message", new ScalarValue(entry.Message)),
                //    new LogEventProperty("RequestId", new ScalarValue(entry.RequestId)),
                //    new LogEventProperty("UserId", new ScalarValue(entry.UserId)),
                //    new LogEventProperty("ContextData", new ScalarValue(entry.ContextData)),
                //    new LogEventProperty("StackTrace", new ScalarValue(entry.StackTrace)),
                //    new LogEventProperty("HostName", new ScalarValue(entry.HostName)),
                //    new LogEventProperty("AppName", new ScalarValue(entry.AppName)),
                //    new LogEventProperty("LogFileName", new ScalarValue(entry.LogFileName)),
                //    new LogEventProperty("EnvironmentName", new ScalarValue(entry.EnvironmentName)),
                //    new LogEventProperty("InstanceId", new ScalarValue(entry.InstanceId))}
                //);

                var logEvent = new LogEvent(
                    entry.TimeStamp ?? DateTimeOffset.UtcNow,
                    LogHelper.GetLogLevel(entry.LogLevel),
                    null,
                    new MessageTemplateParser().Parse(entry.Message),
                    typeof(LogEntry).GetProperties()
                        .Select(x => new LogEventProperty(x.Name, new ScalarValue(x.GetValue(entry)))));

                //timeStamp": "",   "logLevel": "",   "logSource": "",   "message": "",   "requestId": "",   "userId": "",   "contextData": "",   "stackTrace": "",   "hostName": "",   "appName": "",   "logFileName": "",   "environmentName": "",   "instanceId
                logEvents.Add(logEvent);
            }

            await Task.Run(() => logEvents.ForEach(Log.Logger.Write));

            return Ok();
        }
    }
}
