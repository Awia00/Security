using System;

namespace Instaroot.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Log { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}