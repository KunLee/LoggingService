namespace LoggingService.Client.LoggingService.Models
{
    public class GetLogEntry
    {
        public string LogLevel { get; set; }
        public string LogSource { get; set; }
        public string Message { get; set; }
        public string? UserId { get; set; }
        public string? ContextData { get; set; }
        public string? StackTrace { get; set; }
        public string? HostName { get; set; }
        public string? AppName { get; set; }
        public string? EnvironmentName { get; set; }
        public string? InstanceId { get; set; }
    }
}
