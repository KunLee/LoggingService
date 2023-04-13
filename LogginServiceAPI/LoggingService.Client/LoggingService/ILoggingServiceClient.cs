using LoggingService.Client.LoggingService.Models;

namespace LoggingService.Client.LoggingService
{
    public interface ILoggingServiceClient
    {
        /// <summary>
        /// Send a log request
        /// </summary>
        /// <param name="logRequest">Send the Log Request entity with a list of entries to logging service</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The http status returned from the Logging Service</returns>
        Task<bool> Send(GetLogRequest logRequest, CancellationToken cancellationToken = default);
    }
}
