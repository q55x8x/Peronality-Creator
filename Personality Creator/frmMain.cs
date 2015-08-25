using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FarsiLibrary.Win;
using System.IO;
using Personality_Creator.PersonaFiles;
using Personality_Creator.PersonaFiles.Scripts;

namespace Personality_Creator
{
    public partial class frmMain : Form
    {
        private FATabStripItem CurrentTab;

        public Dictionary<string, PersonaFile> OpenedUnAssociatedFiles = new Dictionary<string, PersonaFile>();

        public frmMain()
        {
            InitializeComponent();
            this.projectView.ImageList = DataManager.iconList;
            this.renameToolStripMenuItem.Enabled = false;
    }

        private void mainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !tryCloseAllTabs();

            Settings.save();
        }

        private void mainFrm_Load(object sender, EventArgs e)
        {
            DataManager.initDataManager();
            AutoCompleteItemManager.load();

            if (DataManager.settings.openedPersonas.Count > 0)
            {
                foreach (string personaPath in DataManager.settings.openedPersonas)
                {
                    OpenPersonaIgnoreChecks(personaPath);
                }
            }
        }

        #region toolstripMenu logic

        private void hotkeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hotkeys hotkeys = new Hotkeys();
            hotkeys.Show();
        }

        private void openPersonalityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = DataManager.settings.lastOpenedPersonaDirectory;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                OpenPersona(fbd.SelectedPath);

                DataManager.settings.lastOpenedPersonaDirectory = fbd.SelectedPath;
            }
        }

        private void openScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = DataManager.settings.lastOpenedSingleFileDirectory;

            
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                PersonaFile newFile = PersonaFile.CreateInstance(ofd.FileName);
                this.addUnAssociatedFile(newFile);
                openFile(newFile);

                DataManager.settings.lastOpenedSingleFileDirectory = newFile.File.Directory.FullName;
            }
        }

        private void saveScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.saveCurrentFile();
        }

        #endregion

        #region project view logic

        private void addUnAssociatedFile(PersonaFile file)
        {
            TreeNode node = new TreeNode(file.File.Name, 1, 1) { Tag = file };
            this.projectView.Nodes.Add(node);
        }

        private void addAssociatedFile(PersonaFile file, TreeNode parentNode)
        {
            TreeNode node = new TreeNode(file.File.Name, 1, 1) { Tag = file };
            parentNode.Nodes.Add(node);
        }

        private void projectView_DoubleClick(object sender, EventArgs e)
        {
            if(this.projectView.SelectedNode.Tag is OpenableFile)
            { 
                openFile((OpenableFile)projectView.SelectedNode.Tag);
            }
        }

        private void projectView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.projectView.SelectedNode = e.Node;

            if (this.projectView.SelectedNode.Tag.GetType() == typeof(Folder) || this.projectView.SelectedNode.Tag.GetType() == typeof(Personality))
            {
                this.newScriptToolStripMenuItem.Enabled = true;
                this.newFolderToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.newScriptToolStripMenuItem.Enabled = false;
                this.newFolderToolStripMenuItem.Enabled = false;
            }

            if (this.projectView.SelectedNode.Tag.GetType() == typeof(Module))
            {
                this.cloneToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.cloneToolStripMenuItem.Enabled = false;
            }

            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStripProjectView.Show(Cursor.Position);
            }
        }
        private void projectView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                BeginEditNode();
            }
        }

        private void projectView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if(e.Label == null) //no rename occured
            {
                return;
            }

            if(e.Node.Tag.GetType() == typeof(Folder))
            {
                TreeNode parentNode = e.Node.Parent;
                Folder renamedFolder = (Folder)this.projectView.SelectedNode.Tag;

                string parentDir = ((Folder)e.Node.Parent.Tag).Directory.FullName;
                string newFolderPath = parentDir + @"\" + e.Label;

                renamedFolder.Directory.MoveTo(newFolderPath);
                renamedFolder = new Folder(newFolderPath); //refresh files inside dir, this is easier than doing all files manually

                TreeNode replacementNode = Folder.getNode(renamedFolder);

                this.projectView.Nodes.Remove(e.Node);

                parentNode.Nodes.Add(replacementNode);
            }
            else if (e.Node.Tag.GetType().BaseType.BaseType == typeof(PersonaFile))
            {
                PersonaFile renamendFile = (PersonaFile)e.Node.Tag;
                string newFullName = ((Folder)e.Node.Parent.Tag).Directory.FullName + @"\" + e.Label;
                File.Move(renamendFile.File.FullName, newFullName);
                renamendFile.File = new FileInfo(newFullName);
            }

            this.projectView.Invalidate();
        }

        #region context menu

        private void newScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = ((Folder)this.projectView.SelectedNode.Tag).Directory.FullName;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                PersonaFile file = PersonaFile.CreateInstance(sfd.FileName);

                File.Create(file.File.FullName).Close();

                this.addAssociatedFile(file, this.projectView.SelectedNode);

                openFile(file);
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFolderDialog nfd = new NewFolderDialog();

            if (nfd.Show() == DialogResult.OK)
            {
                string newFolderPath = ((Folder)this.projectView.SelectedNode.Tag).Directory.FullName + @"\" + nfd.NewFolderName;
                Directory.CreateDirectory(newFolderPath);

                Folder newFolder = new Folder(newFolderPath);
                
                this.projectView.SelectedNode.Nodes.Add(Folder.getNode(newFolder));
            }
        }

        //-------------------

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sry but this feature is currently bugged :(");
            throw new NotImplementedException(); //somehow context menu bugs out treeview Visuals - reproduce: rightclick -> rename a few times until editing does not trigger then try to get renaming triggered with F2 or tripple-click then rename -> treeview bugs out with Node-Texts but files are untouched from the error
            this.BeginEditNode();
        }

        //-------------------

        private void asEdgingDeclensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cloneSelectedModule(ScriptDeclensionType.Edging);
        }

        private void asChastityDeclensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cloneSelectedModule(ScriptDeclensionType.Chastity);
        }

        private void asBeggingDeclensionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cloneSelectedModule(ScriptDeclensionType.Begging);
        }

        private void cloneSelectedModule(ScriptDeclensionType declensionType)
        {
            TreeNode parentNode = this.projectView.SelectedNode.Parent;
            Module moduleToClone;
            Module moduleClone = null;

            if (this.projectView.SelectedNode.Tag.GetType() == typeof(Module))
            {
                moduleToClone = (Module)this.projectView.SelectedNode.Tag;
                moduleClone = moduleToClone.clone(declensionType);
            }

            if(moduleClone != null)
            {
                TreeNode newNode = new TreeNode(moduleClone.File.Name, 1, 1);
                newNode.Tag = moduleClone;
                int index = this.projectView.SelectedNode.Index + 1;
                parentNode.Nodes.Insert(index, newNode);
            }
        }

        #endregion

        private void BeginEditNode()
        {
            if (this.projectView.SelectedNode.Tag.GetType().BaseType.BaseType == typeof(PersonaFile))
            {
                this.projectView.SelectedNode.BeginEdit();
            }

            if (this.projectView.SelectedNode.Tag.GetType() == typeof(Folder))
            {
                Folder renamedFolder = (Folder)this.projectView.SelectedNode.Tag;
                List<FATabStripItem> tabsWithOpenFIles = new List<FATabStripItem>();

                foreach (PersonaFile file in renamedFolder.GetAllFilesAndFilesInSubDirs())
                {
                    foreach (FATabStripItem tab in this.tbStrip.Items)
                    {
                        if (tab.Tag == file)
                        {
                            tabsWithOpenFIles.Add(tab);
                        }
                    }
                }

                if(tabsWithOpenFIles.Count > 0)
                {
                    DialogResult result = MessageBox.Show("One or more files need to close before renaming can be done. Save all files before closing?", "Included files still open!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                    switch(result)
                    {
                        case DialogResult.Yes:
                            foreach (FATabStripItem tab in tabsWithOpenFIles)
                            {
                                closeTab(tab, true);
                            }
                            break;
                        case DialogResult.No:
                            foreach (FATabStripItem tab in tabsWithOpenFIles)
                            {
                                closeTab(tab, false);
                            }
                            break;
                        case DialogResult.Cancel:
                            return;
                    }

                    this.projectView.SelectedNode.BeginEdit();
                }
            }
        }

        #endregion

        #region tabStripLogic

        private void tbStrip_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
        {
            this.CurrentTab = tbStrip.SelectedItem;
        }

        private void tbStrip_TabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            e.Cancel = !tryCloseTab(e.Item); //cancel if tryClose did NOT succeed
        }

        private void tbStrip_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                this.contextMenuStripTabContainer.Show(Cursor.Position);
            }
        }

        #region contextMenu

        private void closeCurrentTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tryCloseTab(this.CurrentTab);
        }

        private void closeAllTabsExceptCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FATabStripItem> tabsToClose = new List<FATabStripItem>(); //need to make a copy because working with ref will point to the actual ItemCollection

            foreach(FATabStripItem tab in this.tbStrip.Items)
            {
                tabsToClose.Add(tab);
            }

            tabsToClose.Remove(this.CurrentTab);

            FATabStripItemCollection collection = new FATabStripItemCollection();
            collection.AddRange(tabsToClose.ToArray());

            tryCloseTabs(collection);
        }

        private void closeAllTabsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tryCloseAllTabs();
        }

        #endregion

        #endregion

        #region opening

        private void OpenPersona(string path)
        {
            if (!DataManager.settings.openedPersonas.Contains(path))
            {
                OpenPersonaIgnoreChecks(path);
                DataManager.settings.openedPersonas.Add(path);
            }
        }

        private void OpenPersonaIgnoreChecks(string path)
        {
            Personality persona = new Personality(path);
            this.projectView.Nodes.Add(persona.getRootNode());
        }

        private void openFile(OpenableFile file)
        {
            FATabStripItem newTab = file.CreateTab();

            file.ContentChanged += new ChangedEventHandler(processTabChanges);

            this.tbStrip.AddTab(newTab);

            this.tbStrip.SelectedItem = newTab;

            this.ApplyStyle();
        }

        private void processTabChanges(object sender, EventArgs e)
        {
            FATabStripItem tab = (FATabStripItem)sender;
            //set unsaved changes
            if (!tab.Title.StartsWith("*"))
            {
                tab.Title = tab.Title.Insert(0, "*");
            }
        }

        #endregion

        #region closing

        private bool tryCloseAllTabs()
        {
            return tryCloseTabs(this.tbStrip.Items);
        }

        private bool tryCloseTabs(FATabStripItemCollection tabs)
        {
            bool success = true;
            bool save = false;

            if (unsavedChanges(tabs))
            {
                DialogResult result = MessageBox.Show("Save all changed files?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (result)
                {
                    case DialogResult.Yes:
                        save = true;
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        success = false;
                        break;
                    default:
                        success = false;
                        break;
                }
            }

            if (success)
            {
                closeTabs(tabs, save);
            }

            return success;
        }

        private bool tryCloseTab(FATabStripItem tab)
        {
            bool success = true;
            bool save = false;

            if (tabHasUnsavedChanges(tab))
            {
                switch (MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        save = true;
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        success = false;
                        break;
                    default:
                        success = false;
                        break;
                }
            }

            if(success)
            {
                closeTab(tab, save);
            }

            return success;
        }

        public void closeTab(FATabStripItem tab, bool save)
        {
            this.saveTab(tab, save);
            this.tbStrip.Items.Remove(tab);
        }

        private void closeTabs(FATabStripItemCollection tabs, bool save) //again the collection points directly to the original collection so we have to make a copy to not mess up deletion
        {
            List<FATabStripItem> tabsToClose = new List<FATabStripItem>();

            foreach(FATabStripItem tab in tabs)
            {
                tabsToClose.Add(tab);
            }

            foreach (FATabStripItem tab in tabsToClose)
            {
                closeTab(tab, save);
            }

            tabsToClose = null;
        }

        public void closeAllTabs(bool save)
        {
            closeTabs(this.tbStrip.Items, save);
        }

        private bool unsavedChanges(FATabStripItemCollection items)
        {
            bool unsaved = false;
            foreach (FATabStripItem tab in items)
            {
                if (tabHasUnsavedChanges(tab))
                {
                    unsaved = true;
                    break;
                }
            }

            return unsaved;
        }

        public bool unsavedChanges() //overload currently unused but kept due to later features may be benefitting from it
        {
            return unsavedChanges(this.tbStrip.Items);
        }

        public bool tabHasUnsavedChanges(FATabStripItem tab)
        {
            return tab.Title.StartsWith("*");
        }

        #endregion

        #region saving

        public void saveCurrentFile()
        {
            saveTab(this.CurrentTab, true);
        }

        private void saveTab(FATabStripItem tab, bool save)
        {
            if (tabHasUnsavedChanges(tab) && save)
            {
                ((OpenableFile)tab.Tag).Save();
                tab.Title = tab.Title.Remove(0, 1);
            }
        }

        public void saveAllFiles()
        {
            foreach (FATabStripItem tab in this.tbStrip.Items)
            {
                saveTab(tab, true);
            }
        }

        #endregion

        #region editor logic

        private void ApplyStyle()
        {
            bool unsavedChangesBefore = false;
            if(tabHasUnsavedChanges(this.CurrentTab))
            {
                unsavedChangesBefore = true;
            }

            ((OpenableFile)this.CurrentTab.Tag).Redraw();

            if(!unsavedChangesBefore)
            {
                //if (this.CurrentTab.Title.StartsWith("*")) //this should be redundant
                //{
                this.CurrentTab.Title = this.CurrentTab.Title.Remove(0, 1);
                //}
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode selectedNode = this.projectView.SelectedNode;

            if (selectedNode.Parent == null)
            {
                if (MessageBox.Show(
                    "This will only remove the persona from here. It will NOT delete the files. Do you want to continue?",
                    "Delete Persona",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Folder personaToDelete = (Folder)selectedNode.Tag;
                    selectedNode.Remove();
                    DataManager.settings.openedPersonas.Remove(personaToDelete.Directory.FullName);
                }
            }
            else if (selectedNode.Tag.GetType() == typeof(Folder))
            {
                if (MessageBox.Show(
                    "This will delete this folder and ALL its content. Are you sure?",
                    "Delete Folder",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Folder folderToDelete = (Folder)selectedNode.Tag;
                    folderToDelete.Directory.Delete(true);
                    selectedNode.Remove();
                }
            }
            else
            {
                if (MessageBox.Show(
                    "This will delete this file. Are you sure?",
                    "Delete Script",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Script fileToDelete = (Script)selectedNode.Tag;
                    fileToDelete.File.Delete();
                    selectedNode.Remove();
                }
            }
        }
        #endregion


        //#region assembling

        //private void assembleToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    DirectoryInfo ReleaseDir = new DirectoryInfo(this.OpenPersona.Directory + @"\Release");

        //    if (!Directory.Exists(ReleaseDir.FullName))
        //    {
        //        Directory.CreateDirectory(ReleaseDir.FullName);
        //    }
        //    if (!Directory.Exists(ReleaseDir + this.OpenPersona.Name))
        //    {
        //        Directory.CreateDirectory(ReleaseDir + @"\" + this.OpenPersona.Name);
        //    }

        //    assembleDirectory(this.OpenPersona.Directory);

        //    Directory.Delete(ReleaseDir + @"\" + this.OpenPersona.Name + @"\Fragments", true);

        //    string timestamp = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
        //    timestamp = timestamp.Replace(":", "_");
        //    timestamp = timestamp.Replace(".", "_");

        //    string zipName = this.OpenPersona.Name + "_" + "Release_" + timestamp + ".zip";

        //    string sourceFolder = ReleaseDir.FullName;

        //    string tempdest = this.OpenPersona.Directory + @"\" + zipName; //workaround as zipping a directory into it self is not supported by ZipFile class
        //    string destFileName = ReleaseDir + @"\" + zipName;

        //    foreach(FileInfo file in ReleaseDir.GetFiles()) //workaround so zips do not accumulate in themselfes
        //    {
        //        if(file.Name.Contains(this.OpenPersona.Name + "_" + "Release_"))
        //        {
        //            file.MoveTo(this.OpenPersona.Directory + @"\" + file.Name);
        //        }
        //    }

        //    ZipFile.CreateFromDirectory(sourceFolder, tempdest, CompressionLevel.Optimal, false);

        //    foreach (FileInfo file in this.OpenPersona.Directory.GetFiles())
        //    {
        //        if (file.Name.Contains(this.OpenPersona.Name + "_" + "Release_"))
        //        {
        //            file.MoveTo(ReleaseDir + @"\" + file.Name);
        //        }
        //    }
        //}

        //private void assembleDirectory(DirectoryInfo dir)
        //{
        //    DirectoryInfo releasePath = new DirectoryInfo(this.OpenPersona.Directory + @"\Release\" + this.OpenPersona.Name);

        //    foreach (DirectoryInfo subDir in dir.GetDirectories())
        //    {
        //        if (subDir.Name == "Release")
        //        { continue; }

        //        string relPath = subDir.FullName.Replace(this.OpenPersona.Directory.FullName, "");

        //        if (!Directory.Exists(releasePath + @"\" + relPath))
        //        {
        //            Directory.CreateDirectory(releasePath + @"\" + relPath);
        //        }

        //        assembleDirectory(subDir);
        //    }

        //    foreach (FileInfo file in dir.GetFiles())
        //    {
        //        if (dir.Name == "Fragments") //needed fragments will recursive assemble themselves while a file is assembled that needs a fragment
        //        { break; }

        //        if (file.Extension == ".txt")
        //        {
        //            assembleFile(file);
        //        }
        //        else
        //        {
        //            string relPath = file.FullName.Replace(this.OpenPersona.Directory.FullName, "");
        //            File.Copy(file.FullName, this.OpenPersona.Directory + @"\Release\" + this.OpenPersona.Name + relPath, true);
        //        }
        //    }
        //}

        //private void assembleFile(FileInfo file)
        //{
        //    string relPath = file.FullName.Replace(this.OpenPersona.Directory.FullName, "");
        //    string content = File.ReadAllText(file.FullName);
        //    string replaceFragment = "";

        //    MatchCollection Matches = Regex.Matches(content, @"(?i)\$\$frag\([A-z_0-9öäüáéíóú+\s]+\)"); //Matches need to be refrshed after every replacment because the index would be off
        //    //including fragements
        //    while(Matches.Count >= 1) //foreach sadly not possible here as it doesn't allow to alter the collection from inside of it
        //    {
        //        string fragmentName = Regex.Match(content.Substring(Matches[0].Index, Matches[0].Length), @"(?i)(?<=\$\$frag\()[A-z_0-9öäüáéíóú+\s]+(?=\))").Value;
        //        assembleFile(new FileInfo(this.OpenPersona.Directory + @"\Fragments\" + fragmentName + @".txt")); //recursive fragment assembly
        //        replaceFragment = File.ReadAllText(this.OpenPersona.Directory + @"\Release\" + this.OpenPersona.Name + @"\Fragments\" + fragmentName + @".txt"); //loaded already assembled fragments from the release
        //        content = content.Remove(Matches[0].Index, Matches[0].Length);
        //        content = content.Insert(Matches[0].Index, replaceFragment);
        //        Matches = Regex.Matches(content, @"(?i)\$\$frag\([A-z_0-9öäüáéíóú+\s]+\)");
        //    }

        //    Matches = Regex.Matches(content, @"(?i)(?<=\n)\-.+\n");
        //    //removing comments and regions
        //    while (Matches.Count >= 1)
        //    {
        //        content = content.Remove(Matches[0].Index, Matches[0].Length);
        //        Matches = Regex.Matches(content, @"(?i)(?<=\n)\-.+\n");
        //    }

        //    if (File.Exists(this.OpenPersona.Directory + @"\Release\" + this.OpenPersona.Name + relPath))
        //    {
        //        File.Delete(this.OpenPersona.Directory + @"\Release\" + this.OpenPersona.Name + relPath);
        //    }

        //    File.WriteAllText(this.OpenPersona.Directory + @"\Release\" + this.OpenPersona.Name + relPath, content);
        //}

        //#endregion 
    }
}
