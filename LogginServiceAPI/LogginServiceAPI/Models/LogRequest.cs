﻿namespace LogginServiceAPI.Models
{
    /// <summary>
    /// The request class with a batch of log entries
    /// </summary>
    public class LogRequest
    {
        public IList<LogEntry> Entries { get; set; }
    }
}
