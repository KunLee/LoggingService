using System.ComponentModel.DataAnnotations;

namespace LogginServiceAPI.Models
{
    /// <summary>
    /// The request class with a batch of log entries
    /// </summary>
    public class LogRequest
    {
        [MaxLength(10)]
        public List<LogEntry> Entries { get; set; }
    }
}
