using LogginServiceAPI.Helpers;
using LogginServiceAPI.Models;
using LogginServiceAPI.Models.Utilities;

namespace LogginServiceAPI.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;
        private readonly IMessageUtilities<LogRequest> _logMessageUtilities;

        public LoggingService(ILogger<LoggingService> logger, IMessageUtilities<LogRequest> logMessageUtilities)
        {
            _logger= logger;
            _logMessageUtilities = logMessageUtilities;
        }
        public async Task<bool> Log(LogRequest message)
        {
            if (message == null || !message.Entries.Any())
            {
                _logger.LogError("The request payload is empty or missing log entries.");
                return false;
            }
            var encryptedMessage = _logMessageUtilities.Encrypt(message);

            await Task.Run(() => encryptedMessage.Entries
                      .ForEach(entry => _logger.Log(LogHelper.GetLogLevel(entry.LogLevel),
                        "{MessageID}{@DataObject}", Guid.NewGuid(), entry)));
            return true;
        }
    }
}
