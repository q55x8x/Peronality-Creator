namespace Personality_Creator.UI
{
    partial class VocabFileEditor
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
            this.lblVocabKexword = new System.Windows.Forms.Label();
            this.lstVocabItems = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtNewVocabItem = new System.Windows.Forms.TextBox();
            this.grpBoxInfo = new System.Windows.Forms.GroupBox();
            this.lblVocabItemCount = new System.Windows.Forms.Label();
            this.grpBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblVocabKexword
            // 
            this.lblVocabKexword.AutoSize = true;
            this.lblVocabKexword.Location = new System.Drawing.Point(6, 16);
            this.lblVocabKexword.Name = "lblVocabKexword";
            this.lblVocabKexword.Size = new System.Drawing.Size(99, 13);
            this.lblVocabKexword.TabIndex = 0;
            this.lblVocabKexword.Text = "VOCABKEYWORD";
            // 
            // lstVocabItems
            // 
            this.lstVocabItems.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstVocabItems.FormattingEnabled = true;
            this.lstVocabItems.Location = new System.Drawing.Point(0, 0);
            this.lstVocabItems.Name = "lstVocabItems";
            this.lstVocabItems.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstVocabItems.Size = new System.Drawing.Size(210, 545);
            this.lstVocabItems.TabIndex = 1;
            this.lstVocabItems.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstVocabItems_KeyDown);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(614, 519);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(614, 490);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtNewVocabItem
            // 
            this.txtNewVocabItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewVocabItem.Location = new System.Drawing.Point(216, 521);
            this.txtNewVocabItem.Name = "txtNewVocabItem";
            this.txtNewVocabItem.Size = new System.Drawing.Size(392, 20);
            this.txtNewVocabItem.TabIndex = 4;
            this.txtNewVocabItem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtNewVocabItem_KeyDown);
            // 
            // grpBoxInfo
            // 
            this.grpBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxInfo.Controls.Add(this.lblVocabItemCount);
            this.grpBoxInfo.Controls.Add(this.lblVocabKexword);
            this.grpBoxInfo.Location = new System.Drawing.Point(479, 12);
            this.grpBoxInfo.Name = "grpBoxInfo";
            this.grpBoxInfo.Size = new System.Drawing.Size(200, 100);
            this.grpBoxInfo.TabIndex = 5;
            this.grpBoxInfo.TabStop = false;
            this.grpBoxInfo.Text = "Info";
            // 
            // lblVocabItemCount
            // 
            this.lblVocabItemCount.AutoSize = true;
            this.lblVocabItemCount.Location = new System.Drawing.Point(6, 38);
            this.lblVocabItemCount.Name = "lblVocabItemCount";
            this.lblVocabItemCount.Size = new System.Drawing.Size(75, 13);
            this.lblVocabItemCount.TabIndex = 1;
            this.lblVocabItemCount.Text = "VocabItems: 0";
            // 
            // VocabFileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBoxInfo);
            this.Controls.Add(this.txtNewVocabItem);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lstVocabItems);
            this.Name = "VocabFileEditor";
            this.Size = new System.Drawing.Size(693, 545);
            this.grpBoxInfo.ResumeLayout(false);
            this.grpBoxInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVocabKexword;
        private System.Windows.Forms.ListBox lstVocabItems;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.TextBox txtNewVocabItem;
        private System.Windows.Forms.GroupBox grpBoxInfo;
        private System.Windows.Forms.Label lblVocabItemCount;
    }
}
