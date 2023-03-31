using System.ComponentModel.DataAnnotations;

namespace LogginServiceAPI.Models
{
    public class LogEntry
    {
        [MaxLength(11)]
        public string LogLevel { get; set; }
        [MaxLength(50)]
        public string LogSource { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        [MaxLength(50)]
        public string? UserId { get; set; }
        [MaxLength(5000)]
        public string? ContextData { get; set; }
        [MaxLength(5000)]
        public string? StackTrace { get; set; }
        [MaxLength(150)]
        public string? HostName { get; set; }
        [MaxLength(150)]
        public string? AppName { get; set; }
        [MaxLength(150)]
        public string? EnvironmentName { get; set; }
        [MaxLength(150)]
        public string? InstanceId { get; set; }
        public DateTimeOffset? TimeStamp { get; set; }
    }
}
