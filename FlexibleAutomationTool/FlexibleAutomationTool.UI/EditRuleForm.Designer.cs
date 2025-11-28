namespace FlexibleAutomationTool.UI
{
    partial class EditRuleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cmbTriggerType = new System.Windows.Forms.ComboBox();
            this.nudHour = new System.Windows.Forms.NumericUpDown();
            this.nudMinute = new System.Windows.Forms.NumericUpDown();
            this.txtEventSource = new System.Windows.Forms.TextBox();
            this.txtEventCondition = new System.Windows.Forms.TextBox();
            this.cmbActionType = new System.Windows.Forms.ComboBox();
            this.txtMsgTitle = new System.Windows.Forms.TextBox();
            this.txtMsgBody = new System.Windows.Forms.TextBox();
            this.txtRunPath = new System.Windows.Forms.TextBox();
            this.txtRunArgs = new System.Windows.Forms.TextBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.txtFileContent = new System.Windows.Forms.TextBox();
            this.txtMacroDef = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblTriggerType = new System.Windows.Forms.Label();
            this.lblHour = new System.Windows.Forms.Label();
            this.lblMinute = new System.Windows.Forms.Label();
            this.lblEventSource = new System.Windows.Forms.Label();
            this.lblCondition = new System.Windows.Forms.Label();
            this.lblActionType = new System.Windows.Forms.Label();
            this.lblMsgTitle = new System.Windows.Forms.Label();
            this.lblMsgBody = new System.Windows.Forms.Label();
            this.lblProgramPath = new System.Windows.Forms.Label();
            this.lblArguments = new System.Windows.Forms.Label();
            this.lblUrl = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.lblMacro = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(10, 10);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(100, 23);
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(120, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(400, 23);
            this.txtName.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.Location = new System.Drawing.Point(10, 40);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(100, 23);
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(120, 40);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(400, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // lblTriggerType
            // 
            this.lblTriggerType.Location = new System.Drawing.Point(10, 70);
            this.lblTriggerType.Name = "lblTriggerType";
            this.lblTriggerType.Size = new System.Drawing.Size(100, 23);
            this.lblTriggerType.Text = "Trigger:";
            // 
            // cmbTriggerType
            // 
            this.cmbTriggerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTriggerType.FormattingEnabled = true;
            this.cmbTriggerType.Items.AddRange(new object[] {
            "Time",
            "Event"});
            this.cmbTriggerType.Location = new System.Drawing.Point(120, 70);
            this.cmbTriggerType.Name = "cmbTriggerType";
            this.cmbTriggerType.Size = new System.Drawing.Size(200, 23);
            this.cmbTriggerType.TabIndex = 2;
            // 
            // lblHour
            // 
            this.lblHour.Location = new System.Drawing.Point(120, 100);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new System.Drawing.Size(40, 23);
            this.lblHour.Text = "Hour:";
            // 
            // nudHour
            // 
            this.nudHour.Location = new System.Drawing.Point(160, 100);
            this.nudHour.Maximum = new decimal(new int[] {23,0,0,0});
            this.nudHour.Name = "nudHour";
            this.nudHour.Size = new System.Drawing.Size(60, 23);
            this.nudHour.TabIndex = 3;
            // 
            // lblMinute
            // 
            this.lblMinute.Location = new System.Drawing.Point(240, 100);
            this.lblMinute.Name = "lblMinute";
            this.lblMinute.Size = new System.Drawing.Size(50, 23);
            this.lblMinute.Text = "Minute:";
            // 
            // nudMinute
            // 
            this.nudMinute.Location = new System.Drawing.Point(290, 100);
            this.nudMinute.Maximum = new decimal(new int[] {59,0,0,0});
            this.nudMinute.Name = "nudMinute";
            this.nudMinute.Size = new System.Drawing.Size(60, 23);
            this.nudMinute.TabIndex = 4;
            // 
            // lblEventSource
            // 
            this.lblEventSource.Location = new System.Drawing.Point(10, 130);
            this.lblEventSource.Name = "lblEventSource";
            this.lblEventSource.Size = new System.Drawing.Size(100, 23);
            this.lblEventSource.Text = "Event Source:";
            // 
            // txtEventSource
            // 
            this.txtEventSource.Location = new System.Drawing.Point(120, 130);
            this.txtEventSource.Name = "txtEventSource";
            this.txtEventSource.Size = new System.Drawing.Size(400, 23);
            this.txtEventSource.TabIndex = 5;
            // 
            // lblCondition
            // 
            this.lblCondition.Location = new System.Drawing.Point(10, 160);
            this.lblCondition.Name = "lblCondition";
            this.lblCondition.Size = new System.Drawing.Size(100, 23);
            this.lblCondition.Text = "Condition:";
            // 
            // txtEventCondition
            // 
            this.txtEventCondition.Location = new System.Drawing.Point(120, 160);
            this.txtEventCondition.Name = "txtEventCondition";
            this.txtEventCondition.Size = new System.Drawing.Size(400, 23);
            this.txtEventCondition.TabIndex = 6;
            // 
            // lblActionType
            // 
            this.lblActionType.Location = new System.Drawing.Point(10, 190);
            this.lblActionType.Name = "lblActionType";
            this.lblActionType.Size = new System.Drawing.Size(100, 23);
            this.lblActionType.Text = "Action:";
            // 
            // cmbActionType
            // 
            this.cmbActionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbActionType.FormattingEnabled = true;
            this.cmbActionType.Items.AddRange(new object[] {
            "Message",
            "RunProgram",
            "OpenUrl",
            "FileWrite",
            "Macro"});
            this.cmbActionType.Location = new System.Drawing.Point(120, 190);
            this.cmbActionType.Name = "cmbActionType";
            this.cmbActionType.Size = new System.Drawing.Size(200, 23);
            this.cmbActionType.TabIndex = 7;
            // 
            // lblProgramPath
            // 
            this.lblProgramPath.Location = new System.Drawing.Point(10, 220);
            this.lblProgramPath.Name = "lblProgramPath";
            this.lblProgramPath.Size = new System.Drawing.Size(100, 23);
            this.lblProgramPath.Text = "Program Path:";
            // 
            // txtRunPath
            // 
            this.txtRunPath.Location = new System.Drawing.Point(120, 220);
            this.txtRunPath.Name = "txtRunPath";
            this.txtRunPath.Size = new System.Drawing.Size(400, 23);
            this.txtRunPath.TabIndex = 8;
            // 
            // lblArguments
            // 
            this.lblArguments.Location = new System.Drawing.Point(10, 250);
            this.lblArguments.Name = "lblArguments";
            this.lblArguments.Size = new System.Drawing.Size(100, 23);
            this.lblArguments.Text = "Arguments:";
            // 
            // txtRunArgs
            // 
            this.txtRunArgs.Location = new System.Drawing.Point(120, 250);
            this.txtRunArgs.Name = "txtRunArgs";
            this.txtRunArgs.Size = new System.Drawing.Size(400, 23);
            this.txtRunArgs.TabIndex = 9;
            // 
            // lblUrl
            // 
            this.lblUrl.Location = new System.Drawing.Point(10, 280);
            this.lblUrl.Name = "lblUrl";
            this.lblUrl.Size = new System.Drawing.Size(100, 23);
            this.lblUrl.Text = "URL:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(120, 280);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(400, 23);
            this.txtUrl.TabIndex = 10;
            // 
            // lblFilePath
            // 
            this.lblFilePath.Location = new System.Drawing.Point(10, 310);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(100, 23);
            this.lblFilePath.Text = "File Path:";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(120, 310);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(400, 23);
            this.txtFilePath.TabIndex = 11;
            // 
            // lblContent
            // 
            this.lblContent.Location = new System.Drawing.Point(10, 340);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(100, 23);
            this.lblContent.Text = "Content:";
            // 
            // txtFileContent
            // 
            this.txtFileContent.Location = new System.Drawing.Point(120, 340);
            this.txtFileContent.Multiline = true;
            this.txtFileContent.Name = "txtFileContent";
            this.txtFileContent.Size = new System.Drawing.Size(400, 60);
            this.txtFileContent.TabIndex = 12;
            // 
            // lblMacro
            // 
            this.lblMacro.Location = new System.Drawing.Point(10, 410);
            this.lblMacro.Name = "lblMacro";
            this.lblMacro.Size = new System.Drawing.Size(100, 23);
            this.lblMacro.Text = "Macro (one per line):";
            // 
            // txtMacroDef
            // 
            this.txtMacroDef.Location = new System.Drawing.Point(120, 410);
            this.txtMacroDef.Multiline = true;
            this.txtMacroDef.Name = "txtMacroDef";
            this.txtMacroDef.Size = new System.Drawing.Size(400, 80);
            this.txtMacroDef.TabIndex = 13;
            // 
            // lblMsgTitle
            // 
            this.lblMsgTitle.Location = new System.Drawing.Point(10, 220);
            this.lblMsgTitle.Name = "lblMsgTitle";
            this.lblMsgTitle.Size = new System.Drawing.Size(100, 23);
            this.lblMsgTitle.Text = "Msg Title:";
            // 
            // txtMsgTitle
            // 
            this.txtMsgTitle.Location = new System.Drawing.Point(120, 220);
            this.txtMsgTitle.Name = "txtMsgTitle";
            this.txtMsgTitle.Size = new System.Drawing.Size(400, 23);
            this.txtMsgTitle.TabIndex = 14;
            // 
            // lblMsgBody
            // 
            this.lblMsgBody.Location = new System.Drawing.Point(10, 250);
            this.lblMsgBody.Name = "lblMsgBody";
            this.lblMsgBody.Size = new System.Drawing.Size(100, 23);
            this.lblMsgBody.Text = "Msg Body:";
            // 
            // txtMsgBody
            // 
            this.txtMsgBody.Location = new System.Drawing.Point(120, 250);
            this.txtMsgBody.Multiline = true;
            this.txtMsgBody.Name = "txtMsgBody";
            this.txtMsgBody.Size = new System.Drawing.Size(400, 60);
            this.txtMsgBody.TabIndex = 15;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(340, 500);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 25);
            this.btnOk.TabIndex = 16;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(440, 500);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // EditRuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 540);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblTriggerType);
            this.Controls.Add(this.cmbTriggerType);
            this.Controls.Add(this.lblHour);
            this.Controls.Add(this.nudHour);
            this.Controls.Add(this.lblMinute);
            this.Controls.Add(this.nudMinute);
            this.Controls.Add(this.lblEventSource);
            this.Controls.Add(this.txtEventSource);
            this.Controls.Add(this.lblCondition);
            this.Controls.Add(this.txtEventCondition);
            this.Controls.Add(this.lblActionType);
            this.Controls.Add(this.cmbActionType);
            this.Controls.Add(this.lblProgramPath);
            this.Controls.Add(this.txtRunPath);
            this.Controls.Add(this.lblArguments);
            this.Controls.Add(this.txtRunArgs);
            this.Controls.Add(this.lblUrl);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.txtFileContent);
            this.Controls.Add(this.lblMacro);
            this.Controls.Add(this.txtMacroDef);
            this.Controls.Add(this.lblMsgTitle);
            this.Controls.Add(this.txtMsgTitle);
            this.Controls.Add(this.lblMsgBody);
            this.Controls.Add(this.txtMsgBody);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Name = "EditRuleForm";
            this.Text = "Edit Rule";
            ((System.ComponentModel.ISupportInitialize)(this.nudHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.ComboBox cmbTriggerType;
        private System.Windows.Forms.NumericUpDown nudHour;
        private System.Windows.Forms.NumericUpDown nudMinute;
        private System.Windows.Forms.TextBox txtEventSource;
        private System.Windows.Forms.TextBox txtEventCondition;
        private System.Windows.Forms.ComboBox cmbActionType;
        private System.Windows.Forms.TextBox txtMsgTitle;
        private System.Windows.Forms.TextBox txtMsgBody;
        private System.Windows.Forms.TextBox txtRunPath;
        private System.Windows.Forms.TextBox txtRunArgs;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.TextBox txtFileContent;
        private System.Windows.Forms.TextBox txtMacroDef;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblTriggerType;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.Label lblMinute;
        private System.Windows.Forms.Label lblEventSource;
        private System.Windows.Forms.Label lblCondition;
        private System.Windows.Forms.Label lblActionType;
        private System.Windows.Forms.Label lblMsgTitle;
        private System.Windows.Forms.Label lblMsgBody;
        private System.Windows.Forms.Label lblProgramPath;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.Label lblUrl;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Label lblMacro;
    }
}
