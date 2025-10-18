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
        public void Log(string ruleName, string message) => _entries.Add(new LogEntry { RuleName = ruleName, Message = message });
        public IEnumerable<LogEntry> GetAll() => _entries;
    }
}
