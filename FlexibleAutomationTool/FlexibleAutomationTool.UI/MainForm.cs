using FlexibleAutomationTool.Core.Actions.InternalActions;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Repositories;
using FlexibleAutomationTool.Core.Services;
using FlexibleAutomationTool.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using FlexibleAutomationTool.Core.Facades;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using FlexibleAutomationTool.Core.Actions;

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

        // Track currently attached MacroAction and associated rule name for logging
        private FlexibleAutomationTool.Core.Actions.MacroAction? _attachedMacro;
        private string? _attachedMacroRuleName;

        // Timer to debounce ListChanged events from BindingList when editing via PropertyGrid collection editor
        private System.Windows.Forms.Timer? _macroEditTimer;
        private string? _pendingMacroLogName;
        private bool _macroTemporarilyDetached = false;

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
                // Bring window to front when shown so it appears in Alt+Tab order as expected
                this.Shown += (s, e) =>
                {
                    try
                    {
                        // Temporary TopMost toggle is a reliable way to ensure the window gets foreground focus
                        this.TopMost = true;
                        this.Activate();
                        this.BringToFront();
                        this.TopMost = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to bring MainForm to front: " + ex);
                    }

                    Debug.WriteLine("MainForm Shown event fired");
                };
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

            // Wire property grid events for logging (only on edits)
            try
            {
                propertyGridRule.PropertyValueChanged += PropertyGridRule_PropertyValueChanged;
                // allow deselecting a rule by clicking empty area in the list box
                listBoxRules.MouseDown += ListBoxRules_MouseDown;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to attach handlers: " + ex);
            }

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

            // detach any macro subscriptions
            DetachMacroSubscription();

            // stop and dispose timer
            if (_macroEditTimer != null)
            {
                _macroEditTimer.Stop();
                _macroEditTimer.Tick -= MacroEditTimer_Tick;
                _macroEditTimer.Dispose();
                _macroEditTimer = null;
            }
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

        private void RefreshRulesList()
        {
            listBoxRules.Items.Clear();
            // Only display active user rules; hide internal '__System__' placeholder
            foreach (var r in _repo.GetAll().Where(x => x.IsActive && !string.Equals(x.Name, "__System__", StringComparison.OrdinalIgnoreCase)))
            {
                listBoxRules.Items.Add(r);
            }

            // update manual execute button visibility
            btnManualExecute.Visible = listBoxRules.SelectedItem is Rule;
        }

        private void listBoxRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            // detach any previous macro subscription
            DetachMacroSubscription();

            if (listBoxRules.SelectedItem is Rule selected)
            {
                try
                {
                    propertyGridRule.SelectedObject = selected;
                }
                catch
                {
                }

                // if selected rule contains a MacroAction, attach to its Actions.ListChanged
                if (selected.Action is FlexibleAutomationTool.Core.Actions.MacroAction mac)
                {
                    AttachMacroSubscription(mac, selected.Name);
                }
            }
            else
            {
                try { propertyGridRule.SelectedObject = null; } catch { }
            }

            // update manual execute button visibility
            btnManualExecute.Visible = listBoxRules.SelectedItem is Rule;
        }

        // Clear selection when clicking empty area in the rules list so user can view global history
        private void ListBoxRules_MouseDown(object? sender, MouseEventArgs e)
        {
            try
            {
                if (sender is ListBox lb)
                {
                    var idx = lb.IndexFromPoint(e.Location);
                    if (idx == ListBox.NoMatches)
                    {
                        // clear selection and property grid
                        lb.ClearSelected();
                        try { propertyGridRule.SelectedObject = null; } catch { }
                        // no logging for UI deselect as requested

                        // detach macro subscription when deselecting
                        DetachMacroSubscription();
                    }
                }
            }
            catch { }

            // update manual execute button visibility
            btnManualExecute.Visible = listBoxRules.SelectedItem is Rule;
        }

        // Attach/detach helpers for MacroAction collection edits
        private void AttachMacroSubscription(FlexibleAutomationTool.Core.Actions.MacroAction mac, string ruleName)
        {
            try
            {
                DetachMacroSubscription();
                _attachedMacro = mac;
                _attachedMacroRuleName = ruleName;
                _macroTemporarilyDetached = false;
                // BindingList provides ListChanged event when collection editor changes collection
                _attachedMacro.Actions.ListChanged += MacroActions_ListChanged;
            }
            catch { }
        }

        private void DetachMacroSubscription()
        {
            try
            {
                if (_attachedMacro != null)
                {
                    // If temporarily detached we may already have removed handler; use try/catch
                    try { _attachedMacro.Actions.ListChanged -= MacroActions_ListChanged; } catch { }
                    _attachedMacro = null;
                    _attachedMacroRuleName = null;
                    _macroTemporarilyDetached = false;
                }
            }
            catch { }
        }

        private void MacroActions_ListChanged(object? sender, ListChangedEventArgs e)
        {
            try
            {
                if (_attachedMacro == null) return;

                // Temporarily detach to avoid receiving multiple events for the same edit operation
                if (!_macroTemporarilyDetached)
                {
                    try { _attachedMacro.Actions.ListChanged -= MacroActions_ListChanged; } catch { }
                    _macroTemporarilyDetached = true;
                }

                // Debounce multiple ListChanged events fired by the PropertyGrid collection editor
                _pendingMacroLogName = _attachedMacroRuleName ?? "PropertyGrid";

                if (_macroEditTimer == null)
                {
                    _macroEditTimer = new System.Windows.Forms.Timer();
                    _macroEditTimer.Interval = 250; // ms
                    _macroEditTimer.Tick += MacroEditTimer_Tick;
                }

                // restart timer
                _macroEditTimer.Stop();
                _macroEditTimer.Start();
            }
            catch { }
        }

        private void MacroEditTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (_macroEditTimer != null)
                {
                    _macroEditTimer.Stop();
                }

                var loggedBy = _pendingMacroLogName ?? "PropertyGrid";
                _logger.Log(loggedBy, "Edited: Actions");

                _pendingMacroLogName = null;

                // Reattach handler after edit completed so future edits will be observed
                if (_attachedMacro != null && _macroTemporarilyDetached)
                {
                    try { _attachedMacro.Actions.ListChanged += MacroActions_ListChanged; } catch { }
                    _macroTemporarilyDetached = false;
                }
            }
            catch { }
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

        // PropertyGrid logging for edits only
        private void PropertyGridRule_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                var obj = propertyGridRule.SelectedObject;
                var loggedBy = (obj is Rule r) ? r.Name : obj?.ToString() ?? "PropertyGrid";

                // Use PropertyDescriptor name if available to get the single final property name
                var propName = e.ChangedItem?.PropertyDescriptor?.Name;
                if (string.IsNullOrEmpty(propName))
                    propName = e.ChangedItem?.Label ?? string.Empty;

                if (!string.IsNullOrEmpty(propName))
                {
                    _logger.Log(loggedBy, $"Edited: {propName}");

                    // If rule name changed, refresh list display so new name appears
                    if (string.Equals(propName, "Name", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            // replace the selected item with itself to force the ListBox to redraw using updated ToString()
                            var selIdx = listBoxRules.SelectedIndex;
                            if (selIdx >= 0 && selIdx < listBoxRules.Items.Count)
                            {
                                var objItem = listBoxRules.Items[selIdx];
                                listBoxRules.BeginUpdate();
                                listBoxRules.Items[selIdx] = objItem;
                                listBoxRules.EndUpdate();
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    _logger.Log(loggedBy, "Edited");
                }
            }
            catch { }
        }

        // Manual Execute: run selected rule ignoring IsActive or trigger
        private void BtnManualExecute_Click(object? sender, EventArgs e)
        {
            try
            {
                if (listBoxRules.SelectedItem is Rule selected)
                {
                    // Inject platform services into action before executing
                    InjectPlatformServicesIntoAction(selected.Action);

                    // Execute regardless of IsActive or trigger
                    try
                    {
                        selected.Execute();
                        _logger.Log(selected.Name, "Manual Execute");
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(selected.Name, "Manual Execute Failed: " + ex.Message);
                        _messageBoxService.Show("Execution failed: " + ex.Message, "Error");
                    }
                }
            }
            catch { }
        }

        private void InjectPlatformServicesIntoAction(ActionBase action)
        {
            if (action is MessageBoxAction mba)
            {
                var svc = _svcManager.Get<IMessageBoxService>(nameof(IMessageBoxService));
                if (svc != null) mba.MessageBoxService = svc;
            }
            else if (action is MacroAction mac)
            {
                foreach (var a in mac.Actions)
                {
                    InjectPlatformServicesIntoAction(a);
                }
            }
        }

        // Manual Activate: raise manual activation on ManualTrigger instances
        private void BtnManualActivate_Click(object? sender, EventArgs e)
        {
            try
            {
                var rules = _repo.GetAll();
                foreach (var r in rules)
                {
                    if (r.Trigger is FlexibleAutomationTool.Core.Triggers.ManualTrigger mt)
                    {
                        // Signal the manual trigger
                        mt.RaiseEvent();

                        // If the trigger is now ready, execute immediately instead of waiting for the scheduler
                        try
                        {
                            if (r.CheckTrigger())
                            {
                                // Inject any required platform services into actions (e.g., IMessageBoxService)
                                InjectPlatformServicesIntoAction(r.Action);

                                // Use the same dispatcher the engine uses so execution and logging are consistent
                                var dispatcher = _serviceProvider.GetRequiredService<FlexibleAutomationTool.Core.Services.ICommandDispatcher>();
                                dispatcher.Dispatch(r);

                                _logger.Log(r.Name, "Manual Activation: executed");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Log(r.Name, "Manual Activation failed: " + ex.Message);
                        }
                    }
                }

                _logger.Log("Manual", "Manual Activation triggered");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Manual activate failed: " + ex);
            }
        }

        // Designer-wired handlers (matching case used in Designer)
        private void btnManualExecute_Click(object sender, EventArgs e) => BtnManualExecute_Click(sender, e);
        private void btnManualActivate_Click(object sender, EventArgs e) => BtnManualActivate_Click(sender, e);
    }
}