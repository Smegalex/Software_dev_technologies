using FlexibleAutomationTool.Core.Actions.InternalActions;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using FlexibleAutomationTool.Core.Services;
using FlexibleAutomationTool.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using FlexibleAutomationTool.Core.Facades;
using System.Diagnostics;

namespace FlexibleAutomationTool.UI
{
    public partial class MainForm : Form
    {
        private readonly IRuleRepository _repo;
        private readonly Logger _logger;
        private readonly ServiceManager _svcManager;
        private readonly IAutomationFacade _facade;
        private readonly IMessageBoxService _messageBoxService;
        private readonly MessageBoxAction _messageBoxAction;
        private readonly AutomationEventHandler _eventHandler;
        private readonly IServiceProvider _serviceProvider;

        // Constructor updated to receive dependencies from DI
        public MainForm(
            IRuleRepository repo,
            Logger logger,
            ServiceManager svcManager,
            IAutomationFacade facade,
            IMessageBoxService messageBoxService,
            MessageBoxAction messageBoxAction,
            AutomationEventHandler eventHandler,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // Ensure form is visible and centered (diagnostic for missing window)
            try
            {
                this.StartPosition = FormStartPosition.CenterScreen;
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.Opacity = 1.0;
                Debug.WriteLine("MainForm ctor: initialized and set visibility properties");
                this.Shown += (s, e) => Debug.WriteLine("MainForm Shown event fired");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainForm visibility setup failed: " + ex);
            }

            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _svcManager = svcManager ?? throw new ArgumentNullException(nameof(svcManager));
            _facade = facade ?? throw new ArgumentNullException(nameof(facade));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _messageBoxAction = messageBoxAction ?? throw new ArgumentNullException(nameof(messageBoxAction));
            _eventHandler = eventHandler ?? throw new ArgumentNullException(nameof(eventHandler));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // Initialization that may throw should not prevent form from showing
            try
            {
                // Підписка на UI івенти (status only)
                _eventHandler.StatusUpdated += OnStatusUpdated;

                RefreshRulesList();

                _facade.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("MainForm initialization error: " + ex);
                try { MessageBox.Show($"Initialization error:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { }
            }
        }

        // Прості UI callback методи
        private void OnStatusUpdated(string message)
        {
            this.Text = $"Automation Tool - {message}";
            // statusLabel.Text = message;
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
            _facade?.Stop();
        }

        // Start / Stop
        private void btnStart_Click(object sender, EventArgs e) => _facade.Start();
        private void btnStop_Click(object sender, EventArgs e) => _facade.Stop();

        // Add / Delete
        private void btnAddRule_Click(object sender, EventArgs e)
        {
            // Resolve CreateRuleForm from DI so it receives IMessageBoxService automatically
            using var dlg = _serviceProvider.GetRequiredService<CreateRuleForm>();
            var res = dlg.ShowDialog(this);
            if (res == DialogResult.OK && dlg.CreatedRule != null)
            {
                _facade.CreateRule(dlg.CreatedRule);
                RefreshRulesList();
            }
        }

        private void btnDeleteRule_Click(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItem is Rule selected)
            {
                _repo.Delete(selected.Id);
                _logger.Log(selected.Name, "Deleted");
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
            // Only display active user rules; hide internal '__System__' placeholder
            foreach (var r in _repo.GetAll().Where(x => x.IsActive && !string.Equals(x.Name, "__System__", StringComparison.OrdinalIgnoreCase)))
            {
                listBoxRules.Items.Add(r);
            }
        }

        private void listBoxRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxRules.SelectedItem is Rule selected)
            {
                try
                {
                    propertyGridRule.SelectedObject = selected;
                }
                catch
                {
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
                entries = _facade.GetHistory().Where(le => string.Equals(le.LoggedBy, selected.Name, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                entries = _facade.GetHistory();
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