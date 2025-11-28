using FlexibleAutomationTool.Core.Models;
using FlexibleAutomationTool.Core.Triggers;
using FlexibleAutomationTool.Core.Actions;
using FlexibleAutomationTool.Core.Interfaces;
using FlexibleAutomationTool.UI.Services;
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
            }

            if (rule.Action is FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction ma)
            {
                cmbActionType.SelectedItem = "Message";
                txtMsgTitle.Text = ma.Title;
                txtMsgBody.Text = ma.Message;
            }

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
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
            if (act == "Message")
            {
                EditedRule.Action = new FlexibleAutomationTool.Core.Actions.InternalActions.MessageBoxAction(_mb) { Title = txtMsgTitle.Text ?? "", Message = txtMsgBody.Text ?? "" };
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
