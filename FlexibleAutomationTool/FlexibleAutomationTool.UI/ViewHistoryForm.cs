using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FlexibleAutomationTool.UI
{
    public partial class ViewHistoryForm : Form
    {
        private readonly string? _filterLoggedBy;
        private readonly Logger? _logger;

        public ViewHistoryForm(IEnumerable<LogEntry> entries, Services.AutomationEventHandler? events = null, string? filterLoggedBy = null, Logger? logger = null)
        {
            InitializeComponent();

            _filterLoggedBy = filterLoggedBy;
            _logger = logger;

            if (entries != null)
            {
                foreach (var e in entries.OrderBy(x => x.Timestamp))
                {
                    listBoxHistory.Items.Add($"[{e.Timestamp:yyyy-MM-dd HH:mm:ss}] {e.LoggedBy}: {e.Message}");
                }
            }

            if (_logger != null)
            {
                _logger.LogAdded += OnLogAdded;
                this.FormClosed += (s, e) => _logger.LogAdded -= OnLogAdded;
            }
            else if (events != null)
            {
                events.LogEntryAdded += OnLogEntryAdded;
                this.FormClosed += (s, e) => events.LogEntryAdded -= OnLogEntryAdded;
            }
        }

        private void OnLogEntryAdded(LogEntry entry)
        {
            if (entry == null) return;

            if (!string.IsNullOrWhiteSpace(_filterLoggedBy) && !string.Equals(entry.LoggedBy, _filterLoggedBy, StringComparison.OrdinalIgnoreCase))
                return;

            if (listBoxHistory.InvokeRequired)
            {
                listBoxHistory.BeginInvoke((Action)(() => listBoxHistory.Items.Add($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] {entry.LoggedBy}: {entry.Message}")));
            }
            else
            {
                listBoxHistory.Items.Add($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] {entry.LoggedBy}: {entry.Message}");
            }
        }

        private void OnLogAdded(LogEntry entry) => OnLogEntryAdded(entry);
    }
}
