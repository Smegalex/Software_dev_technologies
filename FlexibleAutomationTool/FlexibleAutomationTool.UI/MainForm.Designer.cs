namespace FlexibleAutomationTool.UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            btnStop = new Button();
            btnAddRule = new Button();
            btnDeleteRule = new Button();
            btnViewHistory = new Button();
            btnEditRule = new Button();
            listBoxRules = new ListBox();
            propertyGridRule = new PropertyGrid();
            btnManualExecute = new Button();
            btnManualActivate = new Button();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(12, 12);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(93, 12);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(75, 23);
            btnStop.TabIndex = 2;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnAddRule
            // 
            btnAddRule.Location = new Point(12, 79);
            btnAddRule.Name = "btnAddRule";
            btnAddRule.Size = new Size(75, 23);
            btnAddRule.TabIndex = 3;
            btnAddRule.Text = "Add";
            btnAddRule.UseVisualStyleBackColor = true;
            btnAddRule.Click += btnAddRule_Click;
            // 
            // btnDeleteRule
            // 
            btnDeleteRule.Location = new Point(177, 79);
            btnDeleteRule.Name = "btnDeleteRule";
            btnDeleteRule.Size = new Size(75, 23);
            btnDeleteRule.TabIndex = 4;
            btnDeleteRule.Text = "Delete";
            btnDeleteRule.UseVisualStyleBackColor = true;
            btnDeleteRule.Click += btnDeleteRule_Click;
            // 
            // btnViewHistory
            // 
            btnViewHistory.Location = new Point(12, 45);
            btnViewHistory.Name = "btnViewHistory";
            btnViewHistory.Size = new Size(94, 23);
            btnViewHistory.TabIndex = 7;
            btnViewHistory.Text = "View History";
            btnViewHistory.UseVisualStyleBackColor = true;
            btnViewHistory.Click += btnViewHistory_Click;
            // 
            // btnEditRule
            // 
            btnEditRule.Location = new Point(93, 79);
            btnEditRule.Name = "btnEditRule";
            btnEditRule.Size = new Size(75, 23);
            btnEditRule.TabIndex = 8;
            btnEditRule.Text = "Edit";
            btnEditRule.UseVisualStyleBackColor = true;
            btnEditRule.Click += btnEditRule_Click;
            // 
            // listBoxRules
            // 
            listBoxRules.FormattingEnabled = true;
            listBoxRules.ItemHeight = 15;
            listBoxRules.Location = new Point(12, 137);
            listBoxRules.Name = "listBoxRules";
            listBoxRules.Size = new Size(240, 454);
            listBoxRules.TabIndex = 5;
            listBoxRules.SelectedIndexChanged += listBoxRules_SelectedIndexChanged;
            // 
            // propertyGridRule
            // 
            propertyGridRule.Location = new Point(270, 12);
            propertyGridRule.Name = "propertyGridRule";
            propertyGridRule.Size = new Size(632, 582);
            propertyGridRule.TabIndex = 9;
            // 
            // btnManualExecute
            // 
            btnManualExecute.Location = new Point(12, 108);
            btnManualExecute.Name = "btnManualExecute";
            btnManualExecute.Size = new Size(120, 23);
            btnManualExecute.TabIndex = 10;
            btnManualExecute.Text = "Manual Execute";
            btnManualExecute.UseVisualStyleBackColor = true;
            btnManualExecute.Visible = false;
            btnManualExecute.Click += btnManualExecute_Click;
            // 
            // btnManualActivate
            // 
            btnManualActivate.Location = new Point(140, 108);
            btnManualActivate.Name = "btnManualActivate";
            btnManualActivate.Size = new Size(110, 23);
            btnManualActivate.TabIndex = 11;
            btnManualActivate.Text = "Manual Activate";
            btnManualActivate.UseVisualStyleBackColor = true;
            btnManualActivate.Click += btnManualActivate_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 599);
            Controls.Add(propertyGridRule);
            Controls.Add(listBoxRules);
            Controls.Add(btnEditRule);
            Controls.Add(btnViewHistory);
            Controls.Add(btnDeleteRule);
            Controls.Add(btnAddRule);
            Controls.Add(btnStop);
            Controls.Add(btnStart);
            Controls.Add(btnManualActivate);
            Controls.Add(btnManualExecute);
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            ResumeLayout(false);
        }

        #endregion
        private Button btnStart;
        private Button btnStop;
        private Button btnAddRule;
        private Button btnDeleteRule;
        private ListBox listBoxRules;
        private Button btnViewHistory;
        private Button btnEditRule;
        private PropertyGrid propertyGridRule;
        private Button btnManualExecute;
        private Button btnManualActivate;
    }
}
