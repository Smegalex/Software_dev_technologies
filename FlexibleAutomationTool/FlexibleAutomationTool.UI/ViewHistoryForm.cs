using FlexibleAutomationTool.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FlexibleAutomationTool.UI
{
    public partial class ViewHistoryForm : Form
    {
        private readonly string? _filterLoggedBy;
        private readonly Services.AutomationEventHandler? _events;

        public ViewHistoryForm(IEnumerable<LogEntry> entries, Services.AutomationEventHandler? events = null, string? filterLoggedBy = null)
        {
            InitializeComponent();

            _filterLoggedBy = filterLoggedBy;
            _events = events;

            if (entries != null)
            {
                foreach (var e in entries.OrderBy(x => x.Timestamp))
                {
                    listBoxHistory.Items.Add($"[{e.Timestamp:yyyy-MM-dd HH:mm:ss}] {e.LoggedBy}: {e.Message}");
                }
            }

            if (_events != null)
            {
                _events.LogEntryAdded += OnLogEntryAdded;

                // Unsubscribe when form closes
                this.FormClosed += (s, e) => _events.LogEntryAdded -= OnLogEntryAdded;
            }
        }

        private void OnLogEntryAdded(LogEntry entry)
        {
            if (entry == null) return;

            // respect filter
            if (!string.IsNullOrWhiteSpace(_filterLoggedBy) && !string.Equals(entry.LoggedBy, _filterLoggedBy, StringComparison.OrdinalIgnoreCase))
                return;

            // Ensure we update control on UI thread
            if (listBoxHistory.InvokeRequired)
            {
                listBoxHistory.BeginInvoke((Action)(() => listBoxHistory.Items.Add($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] {entry.LoggedBy}: {entry.Message}")));
            }
            else
            {
                listBoxHistory.Items.Add($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss}] {entry.LoggedBy}: {entry.Message}");
            }
        }
    }
}
