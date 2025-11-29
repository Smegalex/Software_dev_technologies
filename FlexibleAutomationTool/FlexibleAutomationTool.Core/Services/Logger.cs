using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Data;
using FlexibleAutomationTool.Core.Repositories;

namespace FlexibleAutomationTool.Core.Services
{
    public class Logger
    {
        private readonly List<LogEntry> _entries = new();
        private readonly object _lock = new();
        public event Action<LogEntry>? LogAdded;

        private readonly IExecutionHistoryRepository? _historyRepo;

        public Logger(IExecutionHistoryRepository? historyRepo = null)
        {
            _historyRepo = historyRepo;
        }

        public void Log(string loggedBy, string message)
        {
            var entry = new LogEntry { LoggedBy = loggedBy, Message = message };
            lock (_lock)
            {
                _entries.Add(entry);
            }

            try
            {
                LogAdded?.Invoke(entry);
            }
            catch { }

            _historyRepo?.Add(0, DateTime.UtcNow, loggedBy, message);
        }

        public IEnumerable<LogEntry> GetAll()
        {
            lock (_lock)
            {
                return _entries.ToArray();
            }
        }
    }
}
