using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexibleAutomationTool.Core.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string LoggedBy { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Error { get; set; }
    }
}
