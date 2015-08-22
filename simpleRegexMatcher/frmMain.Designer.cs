namespace simpleRegexMatcher
{
    partial class frmMain
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPattern = new System.Windows.Forms.TextBox();
            this.btnMatch = new System.Windows.Forms.Button();
            this.lblPattern = new System.Windows.Forms.Label();
            this.lblInput = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnBrowseInput = new System.Windows.Forms.Button();
            this.txtInputPath = new System.Windows.Forms.TextBox();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtPattern
            // 
            this.txtPattern.Location = new System.Drawing.Point(59, 36);
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(695, 20);
            this.txtPattern.TabIndex = 1;
            // 
            // btnMatch
            // 
            this.btnMatch.Location = new System.Drawing.Point(760, 34);
            this.btnMatch.Name = "btnMatch";
            this.btnMatch.Size = new System.Drawing.Size(75, 23);
            this.btnMatch.TabIndex = 2;
            this.btnMatch.Text = "Match";
            this.btnMatch.UseVisualStyleBackColor = true;
            this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click);
            // 
            // lblPattern
            // 
            this.lblPattern.AutoSize = true;
            this.lblPattern.Location = new System.Drawing.Point(12, 39);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(41, 13);
            this.lblPattern.TabIndex = 3;
            this.lblPattern.Text = "Pattern";
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Location = new System.Drawing.Point(12, 9);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(31, 13);
            this.lblInput.TabIndex = 4;
            this.lblInput.Text = "Input";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(12, 70);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(39, 13);
            this.lblOutput.TabIndex = 5;
            this.lblOutput.Text = "Output";
            // 
            // btnBrowseInput
            // 
            this.btnBrowseInput.Location = new System.Drawing.Point(760, 4);
            this.btnBrowseInput.Name = "btnBrowseInput";
            this.btnBrowseInput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseInput.TabIndex = 7;
            this.btnBrowseInput.Text = "Browse...";
            this.btnBrowseInput.UseVisualStyleBackColor = true;
            this.btnBrowseInput.Click += new System.EventHandler(this.btnBrowseInput_Click);
            // 
            // txtInputPath
            // 
            this.txtInputPath.Location = new System.Drawing.Point(59, 6);
            this.txtInputPath.Name = "txtInputPath";
            this.txtInputPath.Size = new System.Drawing.Size(695, 20);
            this.txtInputPath.TabIndex = 6;
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Location = new System.Drawing.Point(760, 65);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutput.TabIndex = 9;
            this.btnBrowseOutput.Text = "Browse...";
            this.btnBrowseOutput.UseVisualStyleBackColor = true;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(59, 67);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(695, 20);
            this.txtOutputPath.TabIndex = 8;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 99);
            this.Controls.Add(this.btnBrowseOutput);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.btnBrowseInput);
            this.Controls.Add(this.txtInputPath);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.lblInput);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.btnMatch);
            this.Controls.Add(this.txtPattern);
            this.Name = "frmMain";
            this.Text = "Quick Regex Matcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtPattern;
        private System.Windows.Forms.Button btnMatch;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.Button btnBrowseInput;
        private System.Windows.Forms.TextBox txtInputPath;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
    }
}

