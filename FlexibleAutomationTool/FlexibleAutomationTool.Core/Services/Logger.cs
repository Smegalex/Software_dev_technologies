using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexibleAutomationTool.Core.Models;

namespace FlexibleAutomationTool.Core.Services
{
    public class Logger
    {
        private readonly List<LogEntry> _entries = new();
        public event Action<LogEntry>? LogAdded;

        public void Log(string loggedBy, string message)
        {
            var entry = new LogEntry { LoggedBy = loggedBy, Message = message };
            _entries.Add(entry);
            try { LogAdded?.Invoke(entry); } catch { }
        }

        public IEnumerable<LogEntry> GetAll() => _entries;
    }
}
