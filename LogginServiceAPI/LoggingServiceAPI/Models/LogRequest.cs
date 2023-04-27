namespace LoggingServiceAPI.Models
{
    /// <summary>
    /// The request class with a batch of log entries
    /// </summary>
    public class LogRequest
    {
        public List<LogEntry> Entries { get; set; }
    }
}
