using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Triggers;
using FlexibleAutomationTool.Core.Actions;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.UI.Services;
using FlexibleAutomationTool.Core.Actions.InternalActions;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FlexibleAutomationTool.UI
{
    public partial class EditRuleForm : Form
    {
        private readonly IMessageBoxService _mb;

        public Rule EditedRule { get; private set; }

        public EditRuleForm(Rule rule, IMessageBoxService mb)
        {
            _mb = mb;
            EditedRule = rule;

            InitializeComponent();

            // populate designer fields from existing rule
            txtName.Text = rule.Name;
            txtDescription.Text = rule.Description;

            if (rule.Trigger is TimeTrigger tt)
            {
                cmbTriggerType.SelectedItem = "Time";
                nudHour.Value = tt.Hour;
                nudMinute.Value = tt.Minute;
            }
            else if (rule.Trigger is EventTrigger et)
            {
                cmbTriggerType.SelectedItem = "Event";
                txtEventSource.Text = et.EventSource?.ToString();
                // cannot restore condition expression reliably
            }

            // Populate action-specific fields
            if (rule.Action is MessageBoxAction ma)
            {
                cmbActionType.SelectedItem = "Message";
                txtMsgTitle.Text = ma.Title;
                txtMsgBody.Text = ma.Message;
            }
            else if (rule.Action is RunProgramAction ra)
            {
                cmbActionType.SelectedItem = "RunProgram";
                txtRunPath.Text = ra.Path;
                txtRunArgs.Text = ra.Arguments;
            }
            else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction ua)
            {
                cmbActionType.SelectedItem = "OpenUrl";
                txtUrl.Text = ua.Url;
            }
            else if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction fa)
            {
                cmbActionType.SelectedItem = "FileWrite";
                txtFilePath.Text = fa.FilePath;
                txtFileContent.Text = fa.Content;
            }
            else if (rule.Action is FlexibleAutomationTool.Core.Actions.MacroAction mac)
            {
                cmbActionType.SelectedItem = "Macro";
                // build macro text lines
                var lines = mac.Actions.Select(a =>
                {
                    switch (a)
                    {
                        case MessageBoxAction m: return $"Message|{m.Title}|{m.Message}";
                        case RunProgramAction r: return $"Run|{r.Path}|{r.Arguments}";
                        case FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction u: return $"OpenUrl|{u.Url}";
                        case FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction f: return $"FileWrite|{f.FilePath}|{f.Content}";
                        default: return null;
                    }
                }).Where(x => x != null);

                txtMacroDef.Text = string.Join(Environment.NewLine, lines);
            }

            // Ensure controls visibility reflects current action selection
            UpdateActionControls();

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            cmbTriggerType.SelectedIndexChanged += (s, e) => UpdateTriggerControls();
            cmbActionType.SelectedIndexChanged += (s, e) => UpdateActionControls();
        }

        private void UpdateTriggerControls()
        {
            var isTime = cmbTriggerType.SelectedItem?.ToString() == "Time";
            nudHour.Enabled = isTime;
            nudMinute.Enabled = isTime;
            txtEventSource.Enabled = !isTime;
            txtEventCondition.Enabled = !isTime;

            lblHour.Visible = isTime;
            nudHour.Visible = isTime;
            lblMinute.Visible = isTime;
            nudMinute.Visible = isTime;

            lblEventSource.Visible = !isTime;
            txtEventSource.Visible = !isTime;
            lblCondition.Visible = !isTime;
            txtEventCondition.Visible = !isTime;
        }

        private void UpdateActionControls()
        {
            var sel = cmbActionType.SelectedItem?.ToString();

            bool isMessage = sel == "Message";
            bool isRun = sel == "RunProgram";
            bool isUrl = sel == "OpenUrl";
            bool isFile = sel == "FileWrite";
            bool isMacro = sel == "Macro";

            if (isMacro)
            {
                // Only macro textbox visible
                lblMsgTitle.Visible = txtMsgTitle.Visible = false;
                lblMsgBody.Visible = txtMsgBody.Visible = false;

                lblProgramPath.Visible = txtRunPath.Visible = false;
                lblArguments.Visible = txtRunArgs.Visible = false;

                lblUrl.Visible = txtUrl.Visible = false;

                lblFilePath.Visible = txtFilePath.Visible = false;
                lblContent.Visible = txtFileContent.Visible = false;

                lblMacro.Visible = txtMacroDef.Visible = true;
                return;
            }

            // Message
            lblMsgTitle.Visible = txtMsgTitle.Visible = isMessage;
            lblMsgBody.Visible = txtMsgBody.Visible = isMessage;

            // Run
            lblProgramPath.Visible = txtRunPath.Visible = isRun;
            lblArguments.Visible = txtRunArgs.Visible = isRun;

            // Url
            lblUrl.Visible = txtUrl.Visible = isUrl;

            // File
            lblFilePath.Visible = txtFilePath.Visible = isFile;
            lblContent.Visible = txtFileContent.Visible = isFile;

            // Macro
            lblMacro.Visible = txtMacroDef.Visible = false;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                _mb.Show("Name is required.", "Validation");
                return;
            }

            EditedRule.Name = txtName.Text.Trim();
            EditedRule.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();

            // build trigger
            if (cmbTriggerType.SelectedItem?.ToString() == "Time")
            {
                EditedRule.Trigger = new TimeTrigger { Hour = (int)nudHour.Value, Minute = (int)nudMinute.Value };
            }
            else
            {
                Func<object?, bool>? cond = null;
                if (!string.IsNullOrWhiteSpace(txtEventCondition.Text))
                {
                    var condText = txtEventCondition.Text.Trim();
                    cond = (obj) => obj?.ToString()?.IndexOf(condText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                EditedRule.Trigger = new EventTrigger(string.IsNullOrWhiteSpace(txtEventSource.Text) ? null : txtEventSource.Text, cond);
            }

            // build action
            var act = cmbActionType.SelectedItem?.ToString();
            try
            {
                switch (act)
                {
                    case "Message":
                        EditedRule.Action = new MessageBoxAction(_mb) { Title = txtMsgTitle.Text ?? "", Message = txtMsgBody.Text ?? "" };
                        break;
                    case "RunProgram":
                        EditedRule.Action = new RunProgramAction { Path = txtRunPath.Text ?? "", Arguments = txtRunArgs.Text };
                        break;
                    case "OpenUrl":
                        EditedRule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction { Url = txtUrl.Text ?? "" };
                        break;
                    case "FileWrite":
                        EditedRule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction { FilePath = txtFilePath.Text ?? "", Content = txtFileContent.Text ?? "" };
                        break;
                    case "Macro":
                        var macro = new FlexibleAutomationTool.Core.Actions.MacroAction();
                        var lines = txtMacroDef.Text?.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                        foreach (var line in lines)
                        {
                            var parts = line.Split('|');
                            if (parts.Length == 0) continue;
                            var kind = parts[0].Trim();
                            if (string.Equals(kind, "Message", StringComparison.OrdinalIgnoreCase) && parts.Length >= 3)
                            {
                                var title = parts[1];
                                var body = string.Join('|', parts.Skip(2));
                                macro.Actions.Add(new MessageBoxAction(_mb) { Title = title, Message = body });
                            }
                            else if (string.Equals(kind, "Run", StringComparison.OrdinalIgnoreCase) && parts.Length >= 2)
                            {
                                var path = parts[1];
                                var args = parts.Length >= 3 ? string.Join('|', parts.Skip(2)) : null;
                                macro.Actions.Add(new RunProgramAction { Path = path, Arguments = args });
                            }
                            else if (string.Equals(kind, "OpenUrl", StringComparison.OrdinalIgnoreCase) && parts.Length >= 2)
                            {
                                macro.Actions.Add(new FlexibleAutomationTool.Core.Actions.InternalActions.OpenUrlAction { Url = parts[1] });
                            }
                            else if (string.Equals(kind, "FileWrite", StringComparison.OrdinalIgnoreCase) && parts.Length >= 3)
                            {
                                var path = parts[1];
                                var content = string.Join('|', parts.Skip(2));
                                macro.Actions.Add(new FlexibleAutomationTool.Core.Actions.InternalActions.FileWriteAction { FilePath = path, Content = content });
                            }
                        }
                        EditedRule.Action = macro;
                        break;
                    default:
                        EditedRule.Action = new MessageBoxAction(_mb) { Title = "Info", Message = "No action configured" };
                        break;
                }
            }
            catch (Exception ex)
            {
                _mb.Show("Failed to build action: " + ex.Message, "Error");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
