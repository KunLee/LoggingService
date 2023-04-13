using System.ComponentModel.DataAnnotations;

namespace LoggingService.Client.LoggingService.Models
{
    public class GetLogRequest
    {
        [MaxLength(10)]
        public List<GetLogEntry> Entries { get; set; }
    }
}
