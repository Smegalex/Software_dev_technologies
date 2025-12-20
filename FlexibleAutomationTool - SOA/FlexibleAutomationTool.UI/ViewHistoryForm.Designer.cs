namespace FlexibleAutomationTool.UI
{
    partial class ViewHistoryForm
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
            this.listBoxHistory = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBoxHistory
            // 
            this.listBoxHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxHistory.FormattingEnabled = true;
            this.listBoxHistory.ItemHeight = 15;
            this.listBoxHistory.Location = new System.Drawing.Point(0, 0);
            this.listBoxHistory.Name = "listBoxHistory";
            this.listBoxHistory.Size = new System.Drawing.Size(800, 450);
            this.listBoxHistory.TabIndex = 0;
            // 
            // ViewHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBoxHistory);
            this.Name = "ViewHistoryForm";
            this.Text = "History";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxHistory;
    }
}
