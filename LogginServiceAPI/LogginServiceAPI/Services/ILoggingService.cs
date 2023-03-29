using LogginServiceAPI.Models;

namespace LogginServiceAPI.Services
{
    /// <summary>
    /// Logging Service assists to log message via api calls
    /// </summary>
    public interface ILoggingService
    {
        Task<bool> Log(LogRequest message);
    }
}
