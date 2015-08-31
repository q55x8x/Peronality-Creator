namespace Personality_Creator
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mnuStrip = new System.Windows.Forms.MenuStrip();
            this.tollStripMenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.openScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentlyOpenedScriptsToolStripMenuItem = new Personality_Creator.Last10ToolStripMenuItem();
            this.saveScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.openPersonalityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentlyOpenedPersonalitiesToolStripMenuItem = new Personality_Creator.Last10ToolStripMenuItem();
            this.hotkeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbStrip = new FarsiLibrary.Win.FATabStrip();
            this.projectView = new System.Windows.Forms.TreeView();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.contextMenuStripProjectView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.cloneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asEdgingDeclensionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asChastityDeclensionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asBeggingDeclensionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.contextMenuStripTabContainer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeCurrentTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllTabsExceptCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllTabsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStrip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.contextMenuStripProjectView.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.contextMenuStripTabContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuStrip
            // 
            this.mnuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mnuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tollStripMenuFile,
            this.editToolStripMenuItem,
            this.hotkeysToolStripMenuItem});
            this.mnuStrip.Location = new System.Drawing.Point(0, 0);
            this.mnuStrip.Name = "mnuStrip";
            this.mnuStrip.Size = new System.Drawing.Size(1128, 24);
            this.mnuStrip.TabIndex = 1;
            this.mnuStrip.Text = "menuStrip1";
            // 
            // tollStripMenuFile
            // 
            this.tollStripMenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openScriptToolStripMenuItem,
            this.recentlyOpenedScriptsToolStripMenuItem,
            this.saveScriptToolStripMenuItem,
            this.toolStripMenuItem2,
            this.openPersonalityToolStripMenuItem,
            this.recentlyOpenedPersonalitiesToolStripMenuItem});
            this.tollStripMenuFile.Name = "tollStripMenuFile";
            this.tollStripMenuFile.Size = new System.Drawing.Size(37, 20);
            this.tollStripMenuFile.Text = "File";
            // 
            // openScriptToolStripMenuItem
            // 
            this.openScriptToolStripMenuItem.Name = "openScriptToolStripMenuItem";
            this.openScriptToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.openScriptToolStripMenuItem.Text = "Open Script";
            this.openScriptToolStripMenuItem.Click += new System.EventHandler(this.openScriptToolStripMenuItem_Click);
            // 
            // recentlyOpenedScriptsToolStripMenuItem
            // 
            this.recentlyOpenedScriptsToolStripMenuItem.Enabled = false;
            this.recentlyOpenedScriptsToolStripMenuItem.Entries = ((System.Collections.Generic.List<string>)(resources.GetObject("recentlyOpenedScriptsToolStripMenuItem.Entries")));
            this.recentlyOpenedScriptsToolStripMenuItem.Name = "recentlyOpenedScriptsToolStripMenuItem";
            this.recentlyOpenedScriptsToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.recentlyOpenedScriptsToolStripMenuItem.Text = "Recently opened scripts";
            // 
            // saveScriptToolStripMenuItem
            // 
            this.saveScriptToolStripMenuItem.Name = "saveScriptToolStripMenuItem";
            this.saveScriptToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.saveScriptToolStripMenuItem.Text = "Save Script";
            this.saveScriptToolStripMenuItem.Click += new System.EventHandler(this.saveScriptToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(228, 6);
            // 
            // openPersonalityToolStripMenuItem
            // 
            this.openPersonalityToolStripMenuItem.Name = "openPersonalityToolStripMenuItem";
            this.openPersonalityToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.openPersonalityToolStripMenuItem.Text = "Open Personality";
            this.openPersonalityToolStripMenuItem.Click += new System.EventHandler(this.openPersonalityToolStripMenuItem_Click);
            // 
            // recentlyOpenedPersonalitiesToolStripMenuItem
            // 
            this.recentlyOpenedPersonalitiesToolStripMenuItem.Enabled = false;
            this.recentlyOpenedPersonalitiesToolStripMenuItem.Entries = ((System.Collections.Generic.List<string>)(resources.GetObject("recentlyOpenedPersonalitiesToolStripMenuItem.Entries")));
            this.recentlyOpenedPersonalitiesToolStripMenuItem.Name = "recentlyOpenedPersonalitiesToolStripMenuItem";
            this.recentlyOpenedPersonalitiesToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.recentlyOpenedPersonalitiesToolStripMenuItem.Text = "Recently opened personalities";
            // 
            // hotkeysToolStripMenuItem
            // 
            this.hotkeysToolStripMenuItem.Name = "hotkeysToolStripMenuItem";
            this.hotkeysToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.hotkeysToolStripMenuItem.Text = "Hotkeys";
            this.hotkeysToolStripMenuItem.Click += new System.EventHandler(this.hotkeysToolStripMenuItem_Click);
            // 
            // tbStrip
            // 
            this.tbStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbStrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.tbStrip.Location = new System.Drawing.Point(0, 0);
            this.tbStrip.Name = "tbStrip";
            this.tbStrip.Size = new System.Drawing.Size(890, 448);
            this.tbStrip.TabIndex = 2;
            this.tbStrip.Text = "faTabStrip1";
            this.tbStrip.TabStripItemClosing += new FarsiLibrary.Win.TabStripItemClosingHandler(this.tbStrip_TabStripItemClosing);
            this.tbStrip.TabStripItemSelectionChanged += new FarsiLibrary.Win.TabStripItemChangedHandler(this.tbStrip_TabStripItemSelectionChanged);
            this.tbStrip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tbStrip_MouseUp);
            // 
            // projectView
            // 
            this.projectView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectView.LabelEdit = true;
            this.projectView.Location = new System.Drawing.Point(0, 0);
            this.projectView.Name = "projectView";
            this.projectView.Size = new System.Drawing.Size(234, 448);
            this.projectView.TabIndex = 3;
            this.projectView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.projectView_AfterLabelEdit);
            this.projectView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.projectView_NodeMouseClick);
            this.projectView.DoubleClick += new System.EventHandler(this.projectView_DoubleClick);
            this.projectView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.projectView_KeyDown);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tbStrip);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.projectView);
            this.splitContainer.Size = new System.Drawing.Size(1128, 448);
            this.splitContainer.SplitterDistance = 890;
            this.splitContainer.TabIndex = 4;
            // 
            // contextMenuStripProjectView
            // 
            this.contextMenuStripProjectView.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripProjectView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newScriptToolStripMenuItem,
            this.newFolderToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.renameToolStripMenuItem,
            this.toolStripMenuItem3,
            this.cloneToolStripMenuItem});
            this.contextMenuStripProjectView.Name = "contextMenuStripProjectView";
            this.contextMenuStripProjectView.Size = new System.Drawing.Size(135, 126);
            // 
            // newScriptToolStripMenuItem
            // 
            this.newScriptToolStripMenuItem.Name = "newScriptToolStripMenuItem";
            this.newScriptToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.newScriptToolStripMenuItem.Text = "New Script";
            this.newScriptToolStripMenuItem.Click += new System.EventHandler(this.newScriptToolStripMenuItem_Click);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.newFolderToolStripMenuItem.Text = "New Folder";
            this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.newFolderToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(131, 6);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(131, 6);
            // 
            // cloneToolStripMenuItem
            // 
            this.cloneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asEdgingDeclensionToolStripMenuItem,
            this.asChastityDeclensionToolStripMenuItem,
            this.asBeggingDeclensionToolStripMenuItem});
            this.cloneToolStripMenuItem.Name = "cloneToolStripMenuItem";
            this.cloneToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.cloneToolStripMenuItem.Text = "Clone";
            // 
            // asEdgingDeclensionToolStripMenuItem
            // 
            this.asEdgingDeclensionToolStripMenuItem.Name = "asEdgingDeclensionToolStripMenuItem";
            this.asEdgingDeclensionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.asEdgingDeclensionToolStripMenuItem.Text = "as edging declension";
            this.asEdgingDeclensionToolStripMenuItem.Click += new System.EventHandler(this.asEdgingDeclensionToolStripMenuItem_Click);
            // 
            // asChastityDeclensionToolStripMenuItem
            // 
            this.asChastityDeclensionToolStripMenuItem.Name = "asChastityDeclensionToolStripMenuItem";
            this.asChastityDeclensionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.asChastityDeclensionToolStripMenuItem.Text = "as chastity declension";
            this.asChastityDeclensionToolStripMenuItem.Click += new System.EventHandler(this.asChastityDeclensionToolStripMenuItem_Click);
            // 
            // asBeggingDeclensionToolStripMenuItem
            // 
            this.asBeggingDeclensionToolStripMenuItem.Name = "asBeggingDeclensionToolStripMenuItem";
            this.asBeggingDeclensionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.asBeggingDeclensionToolStripMenuItem.Text = "as begging declension";
            this.asBeggingDeclensionToolStripMenuItem.Click += new System.EventHandler(this.asBeggingDeclensionToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1128, 22);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.splitContainer);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1128, 448);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 24);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(1128, 486);
            this.toolStripContainer.TabIndex = 7;
            this.toolStripContainer.Text = "toolStripContainer";
            // 
            // contextMenuStripTabContainer
            // 
            this.contextMenuStripTabContainer.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripTabContainer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeCurrentTabToolStripMenuItem,
            this.closeAllTabsExceptCurrentToolStripMenuItem,
            this.closeAllTabsToolStripMenuItem});
            this.contextMenuStripTabContainer.Name = "contextMenuStripTabContainer";
            this.contextMenuStripTabContainer.Size = new System.Drawing.Size(222, 70);
            // 
            // closeCurrentTabToolStripMenuItem
            // 
            this.closeCurrentTabToolStripMenuItem.Name = "closeCurrentTabToolStripMenuItem";
            this.closeCurrentTabToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.closeCurrentTabToolStripMenuItem.Text = "Close current tab";
            this.closeCurrentTabToolStripMenuItem.Click += new System.EventHandler(this.closeCurrentTabToolStripMenuItem_Click);
            // 
            // closeAllTabsExceptCurrentToolStripMenuItem
            // 
            this.closeAllTabsExceptCurrentToolStripMenuItem.Name = "closeAllTabsExceptCurrentToolStripMenuItem";
            this.closeAllTabsExceptCurrentToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.closeAllTabsExceptCurrentToolStripMenuItem.Text = "Close all tabs except current";
            this.closeAllTabsExceptCurrentToolStripMenuItem.Click += new System.EventHandler(this.closeAllTabsExceptCurrentToolStripMenuItem_Click);
            // 
            // closeAllTabsToolStripMenuItem
            // 
            this.closeAllTabsToolStripMenuItem.Name = "closeAllTabsToolStripMenuItem";
            this.closeAllTabsToolStripMenuItem.Size = new System.Drawing.Size(221, 22);
            this.closeAllTabsToolStripMenuItem.Text = "Close all tabs";
            this.closeAllTabsToolStripMenuItem.Click += new System.EventHandler(this.closeAllTabsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripMenuItem4,
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem5,
            this.findToolStripMenuItem,
            this.gotoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(149, 6);
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.findToolStripMenuItem.Text = "Find";
            // 
            // gotoToolStripMenuItem
            // 
            this.gotoToolStripMenuItem.Name = "gotoToolStripMenuItem";
            this.gotoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gotoToolStripMenuItem.Text = "Goto";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(149, 6);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 510);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.mnuStrip);
            this.MainMenuStrip = this.mnuStrip;
            this.Name = "frmMain";
            this.Text = "Personality Creator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainFrm_FormClosing);
            this.Load += new System.EventHandler(this.mainFrm_Load);
            this.mnuStrip.ResumeLayout(false);
            this.mnuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbStrip)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.contextMenuStripProjectView.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.contextMenuStripTabContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mnuStrip;
        private System.Windows.Forms.ToolStripMenuItem tollStripMenuFile;
        private FarsiLibrary.Win.FATabStrip tbStrip;
        private System.Windows.Forms.ToolStripMenuItem openScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPersonalityToolStripMenuItem;
        private System.Windows.Forms.TreeView projectView;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripProjectView;
        private System.Windows.Forms.ToolStripMenuItem newScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hotkeysToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTabContainer;
        private System.Windows.Forms.ToolStripMenuItem closeCurrentTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllTabsExceptCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllTabsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem cloneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asEdgingDeclensionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asChastityDeclensionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asBeggingDeclensionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private Personality_Creator.Last10ToolStripMenuItem recentlyOpenedScriptsToolStripMenuItem;
        private Personality_Creator.Last10ToolStripMenuItem recentlyOpenedPersonalitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem gotoToolStripMenuItem;
    }
}

