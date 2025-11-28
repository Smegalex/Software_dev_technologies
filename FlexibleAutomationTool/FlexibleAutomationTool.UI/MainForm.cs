using FlexibleAutomationTool.Core.Actions.InternalActions;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using FlexibleAutomationTool.Core.Services;
using FlexibleAutomationTool.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FlexibleAutomationTool.UI
{
    public partial class MainForm : Form
    {
        private readonly IRuleRepository _repo;
        private readonly Logger _logger;
        private readonly ServiceManager _svcManager;
        private readonly Scheduler _scheduler;
        private readonly AutomationEngine _engine;
        private readonly IMessageBoxService _messageBoxService;
        private readonly MessageBoxAction _messageBoxAction;
        private readonly AutomationEventHandler _eventHandler;
        private readonly IServiceProvider _serviceProvider;

        // Constructor updated to receive dependencies from DI
        public MainForm(
            IRuleRepository repo,
            Logger logger,
            ServiceManager svcManager,
            Scheduler scheduler,
            AutomationEngine engine,
            IMessageBoxService messageBoxService,
            MessageBoxAction messageBoxAction,
            AutomationEventHandler eventHandler,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _svcManager = svcManager ?? throw new ArgumentNullException(nameof(svcManager));
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _messageBoxAction = messageBoxAction ?? throw new ArgumentNullException(nameof(messageBoxAction));
            _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // Підписка на UI івенти
            _eventHandler.StatusUpdated += OnStatusUpdated;
            _eventHandler.LogEntryAdded += OnLogEntryAdded;

            RefreshRulesList();

            _engine.Start();
        }

        // Прості UI callback методи
        private void OnStatusUpdated(string message)
        {
            this.Text = $"Automation Tool - {message}";
            // statusLabel.Text = message;
        }

        private void OnLogEntryAdded(LogEntry entry)
        {
            // Використовуємо LogEntry з Timestamp, LoggedBy, Message
            var logMessage = $"[{entry.Timestamp:HH:mm:ss}] {entry.LoggedBy}: {entry.Message}";

            // Додаємо у logListBox замість Console
            logListBox.Items.Add(logMessage);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _messageBoxAction.Title = "Greeting";
            _messageBoxAction.Message = "Hello from DI!";
            _messageBoxAction.Execute();
        }

        // Ensure engine and scheduler are stopped and subscriptions removed when form closes
        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Dispose event handler first to unsubscribe from engine events
            _eventHandler?.Dispose();
            // Stop engine and scheduler to ensure background work ends
            _engine?.Stop();

            _scheduler?.Stop();
        }

        // Start / Stop
        private void btnStart_Click(object sender, EventArgs e) => _engine.Start();
        private void btnStop_Click(object sender, EventArgs e) => _engine.Stop();

        // Add / Delete
        private void btnAddRule_Click(object sender, EventArgs e)
        {
            // Resolve CreateRuleForm from DI so it receives IMessageBoxService automatically
            using var dlg = _serviceProvider.GetRequiredService<CreateRuleForm>();
            var res = dlg.ShowDialog(this);
            if (res == DialogResult.OK && dlg.CreatedRule != null)
            {
                _repo.Add(dlg.CreatedRule);
                _engine.CreateRule(dlg.CreatedRule);
                RefreshRulesList();
            }
        }

        private void btnDeleteRule_Click(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItem is Rule selected)
            {
                _repo.Delete(selected.Id);
                RefreshRulesList();
            }
        }

        private Rule BuildRuleFromForm()
        {
            // TODO: replace with real CreateRuleForm dialog. For now build a simple rule using a MessageBoxAction
            var r = new Rule
            {
                Name = "HelloRule",
                Description = "Sample rule created from UI",
                Trigger = new FlexibleAutomationTool.Core.Triggers.EventTrigger(null),
                Action = new Core.Actions.InternalActions.MessageBoxAction(_messageBoxService) { Title = "Rule", Message = "Rule executed" },
                IsActive = true
            };

            return r;
        }

        private void RefreshRulesList()
        {
            listBoxRules.Items.Clear();
            foreach (var r in _repo.GetAll())
            {
                listBoxRules.Items.Add(r);
            }
        }

        private void listBoxRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItem is Rule selected)
            {
                // propertyGrid is added to designer; show selected rule's properties
                try
                {
                    propertyGridRule.SelectedObject = selected;
                }
                catch
                {
                    // ignore if property grid not present
                }
            }
            else
            {
                try { propertyGridRule.SelectedObject = null; } catch { }
            }
        }

        // New: View History for selected rule or global
        private void btnViewHistory_Click(object? sender, EventArgs e)
        {
            IEnumerable<FlexibleAutomationTool.Core.Models.LogEntry> entries;
            string? filterBy = null;
            if (listBoxRules.SelectedItem is Rule selected)
            {
                // Show history filtered by rule name
                filterBy = selected.Name;
                entries = _engine.GetHistory().Where(le => string.Equals(le.LoggedBy, selected.Name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                entries = _engine.GetHistory();
            }

            var dlg = new ViewHistoryForm(entries, _eventHandler, filterBy, _logger);
            // show modeless so user can continue interacting with main form
            dlg.Show(this);
        }

        // New: Edit rule using CreateRuleForm by repopulating fields - CreateRuleForm does not support editing, so use EditRuleForm
        private void btnEditRule_Click(object? sender, EventArgs e)
        {
            if (listBoxRules.SelectedItem is Rule selected)
            {
                using var dlg = new EditRuleForm(selected, _messageBoxService);
                var res = dlg.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    _repo.Update(dlg.EditedRule);
                    _logger.Log(dlg.EditedRule.Name, "Edited");
                    RefreshRulesList();
                }
            }
        }
    }
}