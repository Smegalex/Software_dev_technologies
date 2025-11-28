using FlexibleAutomationTool.Core.Actions.InternalActions;
using FlexibleAutomationTool.Core.Actions;
using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Triggers;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.UI.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FlexibleAutomationTool.UI
{
    public partial class CreateRuleForm : Form
    {
        private readonly IMessageBoxService _messageBoxService;

        public Rule? CreatedRule { get; private set; }

        public CreateRuleForm(IMessageBoxService messageBoxService)
        {
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));

            InitializeComponent();

            // Ensure initial state
            UpdateTriggerControls();
            UpdateActionControls();
        }

        private void CmbTriggerType_SelectedIndexChanged(object? sender, EventArgs e) => UpdateTriggerControls();
        private void CmbActionType_SelectedIndexChanged(object? sender, EventArgs e) => UpdateActionControls();

        private void UpdateTriggerControls()
        {
            var isTime = cmbTriggerType.SelectedItem?.ToString() == "Time";
            nudHour.Enabled = isTime;
            nudMinute.Enabled = isTime;
            txtEventSource.Enabled = !isTime;
            txtEventCondition.Enabled = !isTime;

            // Also adjust visibility of labels to keep UI clear
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

            // Message
            lblMsgTitle.Visible = txtMsgTitle.Visible = isMessage || isMacro;
            lblMsgBody.Visible = txtMsgBody.Visible = isMessage || isMacro;

            // Run
            lblProgramPath.Visible = txtRunPath.Visible = isRun || isMacro;
            lblArguments.Visible = txtRunArgs.Visible = isRun || isMacro;

            // Url
            lblUrl.Visible = txtUrl.Visible = isUrl || isMacro;

            // File
            lblFilePath.Visible = txtFilePath.Visible = isFile || isMacro;
            lblContent.Visible = txtFileContent.Visible = isFile || isMacro;

            // Macro
            lblMacro.Visible = txtMacroDef.Visible = isMacro;
        }

        private void BtnOk_Click(object? sender, EventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                _messageBoxService.Show("Name is required.", "Validation");
                return;
            }

            // Build trigger
            Trigger trigger;
            if (cmbTriggerType.SelectedItem?.ToString() == "Time")
            {
                trigger = new TimeTrigger { Hour = (int)nudHour.Value, Minute = (int)nudMinute.Value };
            }
            else
            {
                var src = string.IsNullOrWhiteSpace(txtEventSource.Text) ? null : txtEventSource.Text;
                var condText = string.IsNullOrWhiteSpace(txtEventCondition.Text) ? null : txtEventCondition.Text;
                Func<object?, bool>? cond = null;
                if (!string.IsNullOrWhiteSpace(condText))
                {
                    cond = (obj) => obj?.ToString()?.IndexOf(condText, StringComparison.OrdinalIgnoreCase) >= 0;
                }
                trigger = new EventTrigger(src, cond);
            }

            // Build action
            ActionBase action;
            var actType = cmbActionType.SelectedItem?.ToString();
            try
            {
                switch (actType)
                {
                    case "Message":
                        action = new MessageBoxAction(_messageBoxService) { Title = txtMsgTitle.Text ?? "", Message = txtMsgBody.Text ?? "" };
                        break;
                    case "RunProgram":
                        action = new RunProgramAction { Path = txtRunPath.Text ?? "", Arguments = txtRunArgs.Text };
                        break;
                    case "OpenUrl":
                        action = new OpenUrlAction { Url = txtUrl.Text ?? "" };
                        break;
                    case "FileWrite":
                        action = new FileWriteAction { FilePath = txtFilePath.Text ?? "", Content = txtFileContent.Text ?? "" };
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
                                macro.Actions.Add(new MessageBoxAction(_messageBoxService) { Title = title, Message = body });
                            }
                            else if (string.Equals(kind, "Run", StringComparison.OrdinalIgnoreCase) && parts.Length >= 2)
                            {
                                var path = parts[1];
                                var args = parts.Length >= 3 ? string.Join('|', parts.Skip(2)) : null;
                                macro.Actions.Add(new RunProgramAction { Path = path, Arguments = args });
                            }
                            else if (string.Equals(kind, "OpenUrl", StringComparison.OrdinalIgnoreCase) && parts.Length >= 2)
                            {
                                macro.Actions.Add(new OpenUrlAction { Url = parts[1] });
                            }
                            else if (string.Equals(kind, "FileWrite", StringComparison.OrdinalIgnoreCase) && parts.Length >= 3)
                            {
                                var path = parts[1];
                                var content = string.Join('|', parts.Skip(2));
                                macro.Actions.Add(new FileWriteAction { FilePath = path, Content = content });
                            }
                        }
                        action = macro;
                        break;
                    default:
                        action = new MessageBoxAction(_messageBoxService) { Title = "Info", Message = "No action configured" };
                        break;
                }
            }
            catch (Exception ex)
            {
                _messageBoxService.Show("Failed to build action: " + ex.Message, "Error");
                return;
            }

            // Create rule
            var rule = new Rule
            {
                Name = txtName.Text.Trim(),
                Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim(),
                Trigger = trigger,
                Action = action,
                IsActive = true
            };

            CreatedRule = rule;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // Moved from designer: handle Cancel button click
        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
