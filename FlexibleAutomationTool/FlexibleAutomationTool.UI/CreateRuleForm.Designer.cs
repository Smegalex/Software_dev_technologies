using System;
using System.Windows.Forms;
using System.Drawing;

namespace FlexibleAutomationTool.UI
{
    partial class CreateRuleForm
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlMain = new Panel();
            lblName = new Label();
            txtName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblTriggerType = new Label();
            cmbTriggerType = new ComboBox();
            lblHour = new Label();
            nudHour = new NumericUpDown();
            lblMinute = new Label();
            nudMinute = new NumericUpDown();
            lblEventSource = new Label();
            txtEventSource = new TextBox();
            lblCondition = new Label();
            txtEventCondition = new TextBox();
            sep = new Label();
            lblActionType = new Label();
            cmbActionType = new ComboBox();
            lblMsgTitle = new Label();
            txtMsgTitle = new TextBox();
            lblMsgBody = new Label();
            txtMsgBody = new TextBox();
            lblProgramPath = new Label();
            txtRunPath = new TextBox();
            lblArguments = new Label();
            txtRunArgs = new TextBox();
            lblUrl = new Label();
            txtUrl = new TextBox();
            lblFilePath = new Label();
            txtFilePath = new TextBox();
            lblContent = new Label();
            txtFileContent = new TextBox();
            lblMacro = new Label();
            txtMacroDef = new TextBox();
            btnOk = new Button();
            btnCancel = new Button();
            pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudHour).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudMinute).BeginInit();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.AutoScroll = true;
            pnlMain.Controls.Add(lblName);
            pnlMain.Controls.Add(txtName);
            pnlMain.Controls.Add(lblDescription);
            pnlMain.Controls.Add(txtDescription);
            pnlMain.Controls.Add(lblTriggerType);
            pnlMain.Controls.Add(cmbTriggerType);
            pnlMain.Controls.Add(lblHour);
            pnlMain.Controls.Add(nudHour);
            pnlMain.Controls.Add(lblMinute);
            pnlMain.Controls.Add(nudMinute);
            pnlMain.Controls.Add(lblEventSource);
            pnlMain.Controls.Add(txtEventSource);
            pnlMain.Controls.Add(lblCondition);
            pnlMain.Controls.Add(txtEventCondition);
            pnlMain.Controls.Add(sep);
            pnlMain.Controls.Add(lblActionType);
            pnlMain.Controls.Add(cmbActionType);
            pnlMain.Controls.Add(lblMsgTitle);
            pnlMain.Controls.Add(txtMsgTitle);
            pnlMain.Controls.Add(lblMsgBody);
            pnlMain.Controls.Add(txtMsgBody);
            pnlMain.Controls.Add(lblProgramPath);
            pnlMain.Controls.Add(txtRunPath);
            pnlMain.Controls.Add(lblArguments);
            pnlMain.Controls.Add(txtRunArgs);
            pnlMain.Controls.Add(lblUrl);
            pnlMain.Controls.Add(txtUrl);
            pnlMain.Controls.Add(lblFilePath);
            pnlMain.Controls.Add(txtFilePath);
            pnlMain.Controls.Add(lblContent);
            pnlMain.Controls.Add(txtFileContent);
            pnlMain.Controls.Add(lblMacro);
            pnlMain.Controls.Add(txtMacroDef);
            pnlMain.Controls.Add(btnOk);
            pnlMain.Controls.Add(btnCancel);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(0, 0);
            pnlMain.Name = "pnlMain";
            pnlMain.Size = new Size(649, 713);
            pnlMain.TabIndex = 0;
            // 
            // lblName
            // 
            lblName.Location = new Point(10, 13);
            lblName.Name = "lblName";
            lblName.Size = new Size(120, 20);
            lblName.TabIndex = 0;
            lblName.Text = "Name:";
            // 
            // txtName
            // 
            txtName.Location = new Point(140, 10);
            txtName.Name = "txtName";
            txtName.Size = new Size(480, 23);
            txtName.TabIndex = 1;
            // 
            // lblDescription
            // 
            lblDescription.Location = new Point(10, 43);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(120, 20);
            lblDescription.TabIndex = 2;
            lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(140, 40);
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(480, 23);
            txtDescription.TabIndex = 3;
            // 
            // lblTriggerType
            // 
            lblTriggerType.Location = new Point(10, 73);
            lblTriggerType.Name = "lblTriggerType";
            lblTriggerType.Size = new Size(120, 20);
            lblTriggerType.TabIndex = 4;
            lblTriggerType.Text = "Trigger Type:";
            // 
            // cmbTriggerType
            // 
            cmbTriggerType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTriggerType.Items.AddRange(new object[] { "Time", "Event" });
            cmbTriggerType.Location = new Point(140, 70);
            cmbTriggerType.Name = "cmbTriggerType";
            cmbTriggerType.Size = new Size(220, 23);
            cmbTriggerType.TabIndex = 5;
            cmbTriggerType.SelectedIndexChanged += CmbTriggerType_SelectedIndexChanged;
            // 
            // lblHour
            // 
            lblHour.Location = new Point(10, 103);
            lblHour.Name = "lblHour";
            lblHour.Size = new Size(120, 20);
            lblHour.TabIndex = 6;
            lblHour.Text = "Hour:";
            // 
            // nudHour
            // 
            nudHour.Location = new Point(140, 100);
            nudHour.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
            nudHour.Name = "nudHour";
            nudHour.Size = new Size(60, 23);
            nudHour.TabIndex = 7;
            // 
            // lblMinute
            // 
            lblMinute.Location = new Point(220, 103);
            lblMinute.Name = "lblMinute";
            lblMinute.Size = new Size(60, 20);
            lblMinute.TabIndex = 8;
            lblMinute.Text = "Minute:";
            // 
            // nudMinute
            // 
            nudMinute.Location = new Point(281, 100);
            nudMinute.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            nudMinute.Name = "nudMinute";
            nudMinute.Size = new Size(60, 23);
            nudMinute.TabIndex = 9;
            // 
            // lblEventSource
            // 
            lblEventSource.Location = new Point(10, 133);
            lblEventSource.Name = "lblEventSource";
            lblEventSource.Size = new Size(120, 20);
            lblEventSource.TabIndex = 10;
            lblEventSource.Text = "Event Source:";
            // 
            // txtEventSource
            // 
            txtEventSource.Location = new Point(140, 130);
            txtEventSource.Name = "txtEventSource";
            txtEventSource.Size = new Size(480, 23);
            txtEventSource.TabIndex = 11;
            // 
            // lblCondition
            // 
            lblCondition.Location = new Point(10, 163);
            lblCondition.Name = "lblCondition";
            lblCondition.Size = new Size(120, 20);
            lblCondition.TabIndex = 12;
            lblCondition.Text = "Condition:";
            // 
            // txtEventCondition
            // 
            txtEventCondition.Location = new Point(140, 160);
            txtEventCondition.Name = "txtEventCondition";
            txtEventCondition.Size = new Size(480, 23);
            txtEventCondition.TabIndex = 13;
            // 
            // sep
            // 
            sep.BorderStyle = BorderStyle.Fixed3D;
            sep.Location = new Point(10, 190);
            sep.Name = "sep";
            sep.Size = new Size(600, 2);
            sep.TabIndex = 14;
            // 
            // lblActionType
            // 
            lblActionType.Location = new Point(10, 205);
            lblActionType.Name = "lblActionType";
            lblActionType.Size = new Size(120, 20);
            lblActionType.TabIndex = 15;
            lblActionType.Text = "Action Type:";
            // 
            // cmbActionType
            // 
            cmbActionType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbActionType.Items.AddRange(new object[] { "Message", "RunProgram", "OpenUrl", "FileWrite", "Macro" });
            cmbActionType.Location = new Point(140, 202);
            cmbActionType.Name = "cmbActionType";
            cmbActionType.Size = new Size(220, 23);
            cmbActionType.TabIndex = 16;
            cmbActionType.SelectedIndexChanged += CmbActionType_SelectedIndexChanged;
            // 
            // lblMsgTitle
            // 
            lblMsgTitle.Location = new Point(10, 235);
            lblMsgTitle.Name = "lblMsgTitle";
            lblMsgTitle.Size = new Size(120, 20);
            lblMsgTitle.TabIndex = 17;
            lblMsgTitle.Text = "Title:";
            // 
            // txtMsgTitle
            // 
            txtMsgTitle.Location = new Point(140, 232);
            txtMsgTitle.Name = "txtMsgTitle";
            txtMsgTitle.Size = new Size(480, 23);
            txtMsgTitle.TabIndex = 18;
            // 
            // lblMsgBody
            // 
            lblMsgBody.Location = new Point(10, 265);
            lblMsgBody.Name = "lblMsgBody";
            lblMsgBody.Size = new Size(120, 20);
            lblMsgBody.TabIndex = 19;
            lblMsgBody.Text = "Message:";
            // 
            // txtMsgBody
            // 
            txtMsgBody.Location = new Point(140, 262);
            txtMsgBody.Multiline = true;
            txtMsgBody.Name = "txtMsgBody";
            txtMsgBody.ScrollBars = ScrollBars.Vertical;
            txtMsgBody.Size = new Size(480, 80);
            txtMsgBody.TabIndex = 20;
            // 
            // lblProgramPath
            // 
            lblProgramPath.Location = new Point(10, 345);
            lblProgramPath.Name = "lblProgramPath";
            lblProgramPath.Size = new Size(120, 20);
            lblProgramPath.TabIndex = 21;
            lblProgramPath.Text = "Program Path:";
            // 
            // txtRunPath
            // 
            txtRunPath.Location = new Point(140, 342);
            txtRunPath.Name = "txtRunPath";
            txtRunPath.Size = new Size(480, 23);
            txtRunPath.TabIndex = 22;
            // 
            // lblArguments
            // 
            lblArguments.Location = new Point(10, 375);
            lblArguments.Name = "lblArguments";
            lblArguments.Size = new Size(120, 20);
            lblArguments.TabIndex = 23;
            lblArguments.Text = "Arguments:";
            // 
            // txtRunArgs
            // 
            txtRunArgs.Location = new Point(140, 372);
            txtRunArgs.Name = "txtRunArgs";
            txtRunArgs.Size = new Size(480, 23);
            txtRunArgs.TabIndex = 24;
            // 
            // lblUrl
            // 
            lblUrl.Location = new Point(10, 405);
            lblUrl.Name = "lblUrl";
            lblUrl.Size = new Size(120, 20);
            lblUrl.TabIndex = 25;
            lblUrl.Text = "URL:";
            // 
            // txtUrl
            // 
            txtUrl.Location = new Point(140, 402);
            txtUrl.Name = "txtUrl";
            txtUrl.Size = new Size(480, 23);
            txtUrl.TabIndex = 26;
            // 
            // lblFilePath
            // 
            lblFilePath.Location = new Point(10, 435);
            lblFilePath.Name = "lblFilePath";
            lblFilePath.Size = new Size(120, 20);
            lblFilePath.TabIndex = 27;
            lblFilePath.Text = "File Path:";
            // 
            // txtFilePath
            // 
            txtFilePath.Location = new Point(140, 432);
            txtFilePath.Name = "txtFilePath";
            txtFilePath.Size = new Size(480, 23);
            txtFilePath.TabIndex = 28;
            // 
            // lblContent
            // 
            lblContent.Location = new Point(10, 465);
            lblContent.Name = "lblContent";
            lblContent.Size = new Size(120, 20);
            lblContent.TabIndex = 29;
            lblContent.Text = "Content:";
            // 
            // txtFileContent
            // 
            txtFileContent.Location = new Point(140, 462);
            txtFileContent.Multiline = true;
            txtFileContent.Name = "txtFileContent";
            txtFileContent.ScrollBars = ScrollBars.Vertical;
            txtFileContent.Size = new Size(480, 80);
            txtFileContent.TabIndex = 30;
            // 
            // lblMacro
            // 
            lblMacro.Location = new Point(10, 545);
            lblMacro.Name = "lblMacro";
            lblMacro.Size = new Size(120, 20);
            lblMacro.TabIndex = 31;
            lblMacro.Text = "Macro (one per line):";
            // 
            // txtMacroDef
            // 
            txtMacroDef.Location = new Point(140, 542);
            txtMacroDef.Multiline = true;
            txtMacroDef.Name = "txtMacroDef";
            txtMacroDef.ScrollBars = ScrollBars.Vertical;
            txtMacroDef.Size = new Size(480, 120);
            txtMacroDef.TabIndex = 32;
            // 
            // btnOk
            // 
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(406, 672);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 25);
            btnOk.TabIndex = 33;
            btnOk.Text = "OK";
            btnOk.Click += BtnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(520, 672);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 25);
            btnCancel.TabIndex = 34;
            btnCancel.Text = "Cancel";
            btnCancel.Click += BtnCancel_Click;
            // 
            // CreateRuleForm
            // 
            ClientSize = new Size(649, 713);
            Controls.Add(pnlMain);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CreateRuleForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Create Rule";
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudHour).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudMinute).EndInit();
            ResumeLayout(false);

            // Note: do not call user code here in designer file
        }

        // Designer fields
        private System.Windows.Forms.Panel pnlMain;

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;

        private System.Windows.Forms.Label lblTriggerType;
        private System.Windows.Forms.ComboBox cmbTriggerType;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.NumericUpDown nudHour;
        private System.Windows.Forms.Label lblMinute;
        private System.Windows.Forms.NumericUpDown nudMinute;
        private System.Windows.Forms.Label lblEventSource;
        private System.Windows.Forms.TextBox txtEventSource;
        private System.Windows.Forms.Label lblCondition;
        private System.Windows.Forms.TextBox txtEventCondition;

        private System.Windows.Forms.Label lblActionType;
        private System.Windows.Forms.ComboBox cmbActionType;

        private System.Windows.Forms.Label lblMsgTitle;
        private System.Windows.Forms.TextBox txtMsgTitle;
        private System.Windows.Forms.Label lblMsgBody;
        private System.Windows.Forms.TextBox txtMsgBody;

        private System.Windows.Forms.Label lblProgramPath;
        private System.Windows.Forms.TextBox txtRunPath;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.TextBox txtRunArgs;

        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.TextBox txtUrl;

        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.TextBox txtFileContent;

        private System.Windows.Forms.Label lblMacro;
        private System.Windows.Forms.TextBox txtMacroDef;

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Label sep;
    }
}
