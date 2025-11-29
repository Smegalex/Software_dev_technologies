using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Services;

namespace FlexibleAutomationTool.UI.Services
{
    /// <summary>
    /// Handles AutomationEngine events and provides UI-friendly callbacks.
    /// Encapsulates translating engine events into UI notifications and selects relevant log entries.
    /// </summary>
    public class AutomationEventHandler : IDisposable
    {
        private readonly AutomationEngine _engine;
        private readonly Logger _logger;
        private readonly IMessageBoxService _messageBoxService;
        private readonly SynchronizationContext? _uiContext;

        // UI-friendly events invoked on the UI thread
        public event Action<string>? StatusUpdated;
        public event Action<LogEntry>? LogEntryAdded;

        // If true, a message box will be shown on rule failures. Default: true.
        public bool ShowMessageBoxOnFailure { get; set; } = true;

        public AutomationEventHandler(
            AutomationEngine engine,
            Logger logger,
            IMessageBoxService messageBoxService,
            SynchronizationContext? uiContext = null)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _uiContext = uiContext ?? SynchronizationContext.Current;

            // Subscribe to engine events
            _engine.RuleTriggered += OnRuleTriggered;
            _engine.RuleExecuted += OnRuleExecuted;
            _engine.RuleFailed += OnRuleFailed;
        }

        private void OnRuleTriggered(Rule rule)
        {
            if (rule == null) return;

            var message = $"Rule triggered: {rule.Name}";

            PostToUI(() =>
            {
                StatusUpdated?.Invoke(message);

                var latestLog = GetLatestLogForRule(rule);
                if (latestLog != null)
                    LogEntryAdded?.Invoke(latestLog);
            });
        }

        private void OnRuleExecuted(Rule rule)
        {
            if (rule == null) return;

            var message = $"Rule executed: {rule.Name}";

            PostToUI(() =>
            {
                StatusUpdated?.Invoke(message);

                var latestLog = GetLatestLogForRule(rule);
                if (latestLog != null)
                    LogEntryAdded?.Invoke(latestLog);
            });
        }

        private void OnRuleFailed(Rule rule, Exception ex)
        {
            if (rule == null) return;

            var message = $"Rule failed: {rule.Name}";

            PostToUI(() =>
            {
                StatusUpdated?.Invoke(message);

                if (ShowMessageBoxOnFailure)
                {
                    // Show a friendly message but include exception details if available
                    var title = "Rule Execution Error";
                    var body = ex == null ? $"Rule '{rule.Name}' failed." : $"Rule '{rule.Name}' failed:\n{ex.Message}";
                    _messageBoxService.Show(body, title);
                }

                var latestLog = GetLatestLogForRule(rule) ?? _logger.GetAll().LastOrDefault();
                if (latestLog != null)
                    LogEntryAdded?.Invoke(latestLog);
            });
        }

        /// <summary>
        /// Attempts to find the most relevant log entry for the provided rule.
        /// Falls back to the last global entry if none found for the rule.
        /// </summary>
        private LogEntry? GetLatestLogForRule(Rule rule)
        {
            try
            {
                // Prefer logs where LoggedBy matches the rule name
                var byName = _logger.GetAll()
                    .Where(e => string.Equals(e.LoggedBy, rule.Name, StringComparison.OrdinalIgnoreCase))
                    .LastOrDefault();

                if (byName != null) return byName;

                // If no direct match, try to find a log that mentions the rule name in the message
                var byMessage = _logger.GetAll()
                    .Where(e => e.Message != null && e.Message.Contains(rule.Name, StringComparison.OrdinalIgnoreCase))
                    .LastOrDefault();

                return byMessage;
            }
            catch
            {
                // If logger enumeration fails for any reason, return nothing
                return null;
            }
        }

        /// <summary>
        /// Posts an action to UI synchronization context when available.
        /// Falls back to using Application.OpenForms[0].BeginInvoke if no SynchronizationContext was captured.
        /// </summary>
        private void PostToUI(Action action)
        {
            if (action == null) return;

            if (_uiContext != null)
            {
                _uiContext.Post(_ => action(), null);
                return;
            }

            // Fallback: try to use the first open form to marshal to UI thread
            try
            {
                if (Application.OpenForms != null && Application.OpenForms.Count > 0)
                {
                    var form = Application.OpenForms[0];
                    if (form != null)
                    {
                        if (form.InvokeRequired)
                        {
                            form.BeginInvoke((Action)(() => action()));
                            return;
                        }
                        else
                        {
                            action();
                            return;
                        }
                    }
                }
            }
            catch
            {
                // ignore and fallback to direct call
            }

            // Last resort: execute inline (may still cause cross-thread issues if UI access occurs)
            action();
        }

        public void Dispose()
        {
            _engine.RuleTriggered -= OnRuleTriggered;
            _engine.RuleExecuted -= OnRuleExecuted;
            _engine.RuleFailed -= OnRuleFailed;
        }
    }
}