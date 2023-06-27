using LoggingService.Client.Helpers;
using LoggingService.Client.LoggingService.Models;
using LoggingService.Client.References;
using Microsoft.Extensions.Logging;

namespace LoggingService.Client.LoggingService
{
    /// <summary>
    /// Client used to interact with the Remote Logging Service
    /// </summary>
    public class LoggingServiceClient : ILoggingServiceClient
    {
        private readonly ILoggingService _loggingService;
        private readonly ILogger<LoggingServiceClient> _logger;

        public LoggingServiceClient(ILoggingService logingService, ILogger<LoggingServiceClient> logger)
        {
            _loggingService = logingService;
            _logger = logger;
        }
        public async Task<bool> Send(GetLogRequest getLogRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                await _loggingService.LoggingAsync(new LogRequest
                {
                    Entries = getLogRequest.Entries.Select(x => new LogEntry
                    {
                        LogLevel = x.LogLevel,
                        ContextData = x.ContextData,
                        AppName = x.AppName,
                        EnvironmentName = x.EnvironmentName,
                        HostName = x.HostName,
                        InstanceId = x.InstanceId,
                        LogSource = x.LogSource,
                        Message = x.Message,
                        StackTrace = x.StackTrace,
                        UserId = x.UserId,
                        TimeStamp = DateTimeOffset.UtcNow
                    }).ToList()
                }, cancellationToken);
                return true;
            }
            catch (Exception e) 
            {
                _logger.LogError(e, $"Error calling External API - '{nameof(_loggingService)}'");
                await LoggingToLocal(getLogRequest);
                return false;
            }
        }
        public async Task LoggingToLocal(GetLogRequest clientLogRequest)
        {
            await Task.Run(() => clientLogRequest
                        .Entries?.ForEach(entry => _logger.Log(LogHelper.GetLogLevel(entry.LogLevel),
                        "{MessageID}{@DataObject}", Guid.NewGuid(), entry)));
        }
    }
}
