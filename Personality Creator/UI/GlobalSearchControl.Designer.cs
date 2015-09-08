namespace Personality_Creator.UI
{
    partial class GlobalSearchControl
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lblSearchFor = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.txtSearchInput = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.ResultsView = new System.Windows.Forms.TreeView();
            this.chkBxCaseSensitive = new System.Windows.Forms.CheckBox();
            this.chkBxRegex = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ResultsView);
            this.splitContainer1.Size = new System.Drawing.Size(1224, 555);
            this.splitContainer1.SplitterDistance = 349;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lblSearchFor);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1224, 349);
            this.splitContainer2.SplitterDistance = 75;
            this.splitContainer2.TabIndex = 0;
            // 
            // lblSearchFor
            // 
            this.lblSearchFor.AutoSize = true;
            this.lblSearchFor.Location = new System.Drawing.Point(4, 4);
            this.lblSearchFor.Name = "lblSearchFor";
            this.lblSearchFor.Size = new System.Drawing.Size(59, 13);
            this.lblSearchFor.TabIndex = 0;
            this.lblSearchFor.Text = "Search For";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.chkBxRegex);
            this.splitContainer3.Panel1.Controls.Add(this.chkBxCaseSensitive);
            this.splitContainer3.Panel1.Controls.Add(this.txtSearchInput);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.btnSearch);
            this.splitContainer3.Size = new System.Drawing.Size(1145, 349);
            this.splitContainer3.SplitterDistance = 317;
            this.splitContainer3.TabIndex = 0;
            // 
            // txtSearchInput
            // 
            this.txtSearchInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchInput.Location = new System.Drawing.Point(0, 0);
            this.txtSearchInput.Name = "txtSearchInput";
            this.txtSearchInput.Size = new System.Drawing.Size(1145, 20);
            this.txtSearchInput.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(1067, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // ResultsView
            // 
            this.ResultsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultsView.Location = new System.Drawing.Point(0, 0);
            this.ResultsView.Name = "ResultsView";
            this.ResultsView.Size = new System.Drawing.Size(1224, 202);
            this.ResultsView.TabIndex = 0;
            // 
            // chkBxCaseSensitive
            // 
            this.chkBxCaseSensitive.AutoSize = true;
            this.chkBxCaseSensitive.Checked = true;
            this.chkBxCaseSensitive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBxCaseSensitive.Location = new System.Drawing.Point(3, 26);
            this.chkBxCaseSensitive.Name = "chkBxCaseSensitive";
            this.chkBxCaseSensitive.Size = new System.Drawing.Size(94, 17);
            this.chkBxCaseSensitive.TabIndex = 1;
            this.chkBxCaseSensitive.Text = "Case sensitive";
            this.chkBxCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // chkBxRegex
            // 
            this.chkBxRegex.AutoSize = true;
            this.chkBxRegex.Location = new System.Drawing.Point(105, 26);
            this.chkBxRegex.Name = "chkBxRegex";
            this.chkBxRegex.Size = new System.Drawing.Size(116, 17);
            this.chkBxRegex.TabIndex = 2;
            this.chkBxRegex.Text = "Regular expression";
            this.chkBxRegex.UseVisualStyleBackColor = true;
            // 
            // GlobalSearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GlobalSearchControl";
            this.Size = new System.Drawing.Size(1224, 555);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblSearchFor;
        private System.Windows.Forms.SplitContainer splitContainer3;
        public System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.TreeView ResultsView;
        public System.Windows.Forms.CheckBox chkBxCaseSensitive;
        public System.Windows.Forms.TextBox txtSearchInput;
        public System.Windows.Forms.CheckBox chkBxRegex;
    }
}
