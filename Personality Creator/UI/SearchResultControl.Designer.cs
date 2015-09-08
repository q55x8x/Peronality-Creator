namespace Personality_Creator.UI
{
    partial class SearchResultControl
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
            this.resultView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // resultView
            // 
            this.resultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultView.Location = new System.Drawing.Point(0, 0);
            this.resultView.Name = "resultView";
            this.resultView.Size = new System.Drawing.Size(1083, 570);
            this.resultView.TabIndex = 0;
            // 
            // SearchResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultView);
            this.Name = "SearchResultControl";
            this.Size = new System.Drawing.Size(1083, 570);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView resultView;
    }
}
