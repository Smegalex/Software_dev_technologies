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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(120, 10);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(400, 23);
            this.txtName.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(120, 40);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(400, 23);
            this.txtDescription.TabIndex = 1;
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
            // nudHour
            // 
            this.nudHour.Location = new System.Drawing.Point(160, 100);
            this.nudHour.Maximum = new decimal(new int[] {23,0,0,0});
            this.nudHour.Name = "nudHour";
            this.nudHour.Size = new System.Drawing.Size(60, 23);
            this.nudHour.TabIndex = 3;
            // 
            // nudMinute
            // 
            this.nudMinute.Location = new System.Drawing.Point(290, 100);
            this.nudMinute.Maximum = new decimal(new int[] {59,0,0,0});
            this.nudMinute.Name = "nudMinute";
            this.nudMinute.Size = new System.Drawing.Size(60, 23);
            this.nudMinute.TabIndex = 4;
            // 
            // txtEventSource
            // 
            this.txtEventSource.Location = new System.Drawing.Point(120, 130);
            this.txtEventSource.Name = "txtEventSource";
            this.txtEventSource.Size = new System.Drawing.Size(400, 23);
            this.txtEventSource.TabIndex = 5;
            // 
            // txtEventCondition
            // 
            this.txtEventCondition.Location = new System.Drawing.Point(120, 160);
            this.txtEventCondition.Name = "txtEventCondition";
            this.txtEventCondition.Size = new System.Drawing.Size(400, 23);
            this.txtEventCondition.TabIndex = 6;
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
            // txtMsgTitle
            // 
            this.txtMsgTitle.Location = new System.Drawing.Point(120, 220);
            this.txtMsgTitle.Name = "txtMsgTitle";
            this.txtMsgTitle.Size = new System.Drawing.Size(400, 23);
            this.txtMsgTitle.TabIndex = 8;
            // 
            // txtMsgBody
            // 
            this.txtMsgBody.Location = new System.Drawing.Point(120, 250);
            this.txtMsgBody.Multiline = true;
            this.txtMsgBody.Name = "txtMsgBody";
            this.txtMsgBody.Size = new System.Drawing.Size(400, 50);
            this.txtMsgBody.TabIndex = 9;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(340, 320);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 25);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(440, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // EditRuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 360);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtMsgBody);
            this.Controls.Add(this.txtMsgTitle);
            this.Controls.Add(this.cmbActionType);
            this.Controls.Add(this.txtEventCondition);
            this.Controls.Add(this.txtEventSource);
            this.Controls.Add(this.nudMinute);
            this.Controls.Add(this.nudHour);
            this.Controls.Add(this.cmbTriggerType);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtName);
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
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}
