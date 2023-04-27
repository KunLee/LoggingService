using System.ComponentModel.DataAnnotations;

namespace LoggingService.Client.LoggingService.Models
{
    public class GetLogRequest
    {
        public List<GetLogEntry> Entries { get; set; }
    }
}
