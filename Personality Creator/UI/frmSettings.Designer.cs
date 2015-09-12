namespace Personality_Creator.UI
{
    partial class frmSettings
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
            this.grpBxVocabStyle = new System.Windows.Forms.GroupBox();
            this.propertyGridVocabStyle = new System.Windows.Forms.PropertyGrid();
            this.grpBxCommandStyle = new System.Windows.Forms.GroupBox();
            this.propertyGridCommandStyle = new System.Windows.Forms.PropertyGrid();
            this.grpBxResponseStyle = new System.Windows.Forms.GroupBox();
            this.propertyGridResponseStyle = new System.Windows.Forms.PropertyGrid();
            this.grpBxParanthesisStyle = new System.Windows.Forms.GroupBox();
            this.propertyGridParanthesisStyle = new System.Windows.Forms.PropertyGrid();
            this.grpBxVocabStyle.SuspendLayout();
            this.grpBxCommandStyle.SuspendLayout();
            this.grpBxResponseStyle.SuspendLayout();
            this.grpBxParanthesisStyle.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBxVocabStyle
            // 
            this.grpBxVocabStyle.Controls.Add(this.propertyGridVocabStyle);
            this.grpBxVocabStyle.Location = new System.Drawing.Point(12, 12);
            this.grpBxVocabStyle.Name = "grpBxVocabStyle";
            this.grpBxVocabStyle.Size = new System.Drawing.Size(279, 228);
            this.grpBxVocabStyle.TabIndex = 0;
            this.grpBxVocabStyle.TabStop = false;
            this.grpBxVocabStyle.Text = "#... Style";
            // 
            // propertyGridVocabStyle
            // 
            this.propertyGridVocabStyle.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGridVocabStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridVocabStyle.Location = new System.Drawing.Point(3, 16);
            this.propertyGridVocabStyle.Name = "propertyGridVocabStyle";
            this.propertyGridVocabStyle.Size = new System.Drawing.Size(273, 209);
            this.propertyGridVocabStyle.TabIndex = 0;
            this.propertyGridVocabStyle.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridVocabStyle_PropertyValueChanged);
            // 
            // grpBxCommandStyle
            // 
            this.grpBxCommandStyle.Controls.Add(this.propertyGridCommandStyle);
            this.grpBxCommandStyle.Location = new System.Drawing.Point(297, 12);
            this.grpBxCommandStyle.Name = "grpBxCommandStyle";
            this.grpBxCommandStyle.Size = new System.Drawing.Size(279, 228);
            this.grpBxCommandStyle.TabIndex = 1;
            this.grpBxCommandStyle.TabStop = false;
            this.grpBxCommandStyle.Text = "@... Style";
            // 
            // propertyGridCommandStyle
            // 
            this.propertyGridCommandStyle.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGridCommandStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridCommandStyle.Location = new System.Drawing.Point(3, 16);
            this.propertyGridCommandStyle.Name = "propertyGridCommandStyle";
            this.propertyGridCommandStyle.Size = new System.Drawing.Size(273, 209);
            this.propertyGridCommandStyle.TabIndex = 0;
            this.propertyGridCommandStyle.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridCommandStyle_PropertyValueChanged);
            // 
            // grpBxResponseStyle
            // 
            this.grpBxResponseStyle.Controls.Add(this.propertyGridResponseStyle);
            this.grpBxResponseStyle.Location = new System.Drawing.Point(15, 246);
            this.grpBxResponseStyle.Name = "grpBxResponseStyle";
            this.grpBxResponseStyle.Size = new System.Drawing.Size(279, 228);
            this.grpBxResponseStyle.TabIndex = 2;
            this.grpBxResponseStyle.TabStop = false;
            this.grpBxResponseStyle.Text = "[...] Style";
            // 
            // propertyGridResponseStyle
            // 
            this.propertyGridResponseStyle.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGridResponseStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridResponseStyle.Location = new System.Drawing.Point(3, 16);
            this.propertyGridResponseStyle.Name = "propertyGridResponseStyle";
            this.propertyGridResponseStyle.Size = new System.Drawing.Size(273, 209);
            this.propertyGridResponseStyle.TabIndex = 0;
            this.propertyGridResponseStyle.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridResponseStyle_PropertyValueChanged);
            // 
            // grpBxParanthesisStyle
            // 
            this.grpBxParanthesisStyle.Controls.Add(this.propertyGridParanthesisStyle);
            this.grpBxParanthesisStyle.Location = new System.Drawing.Point(300, 246);
            this.grpBxParanthesisStyle.Name = "grpBxParanthesisStyle";
            this.grpBxParanthesisStyle.Size = new System.Drawing.Size(279, 228);
            this.grpBxParanthesisStyle.TabIndex = 3;
            this.grpBxParanthesisStyle.TabStop = false;
            this.grpBxParanthesisStyle.Text = "(...) Style";
            // 
            // propertyGridParanthesisStyle
            // 
            this.propertyGridParanthesisStyle.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGridParanthesisStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridParanthesisStyle.Location = new System.Drawing.Point(3, 16);
            this.propertyGridParanthesisStyle.Name = "propertyGridParanthesisStyle";
            this.propertyGridParanthesisStyle.Size = new System.Drawing.Size(273, 209);
            this.propertyGridParanthesisStyle.TabIndex = 0;
            this.propertyGridParanthesisStyle.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridParanthesisStyle_PropertyValueChanged);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 482);
            this.Controls.Add(this.grpBxParanthesisStyle);
            this.Controls.Add(this.grpBxResponseStyle);
            this.Controls.Add(this.grpBxCommandStyle);
            this.Controls.Add(this.grpBxVocabStyle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.grpBxVocabStyle.ResumeLayout(false);
            this.grpBxCommandStyle.ResumeLayout(false);
            this.grpBxResponseStyle.ResumeLayout(false);
            this.grpBxParanthesisStyle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBxVocabStyle;
        private System.Windows.Forms.PropertyGrid propertyGridVocabStyle;
        private System.Windows.Forms.GroupBox grpBxCommandStyle;
        private System.Windows.Forms.PropertyGrid propertyGridCommandStyle;
        private System.Windows.Forms.GroupBox grpBxResponseStyle;
        private System.Windows.Forms.PropertyGrid propertyGridResponseStyle;
        private System.Windows.Forms.GroupBox grpBxParanthesisStyle;
        private System.Windows.Forms.PropertyGrid propertyGridParanthesisStyle;
    }
}