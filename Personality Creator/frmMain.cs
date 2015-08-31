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
            //this.renameToolStripMenuItem.Enabled = false;

            this.recentlyOpenedScriptsToolStripMenuItem.EntryClicked += (object sender, EventArgs e) => {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                OpenableFile file = PersonaFile.CreateInstance(menuItem.Text);
                openFile(file);
            };

            this.recentlyOpenedPersonalitiesToolStripMenuItem.EntryClicked += (object sender, EventArgs e) => {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                OpenPersona(menuItem.Text);
            };
        }

        private void mainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !trySaveTabs(copyTabCollectionAsList(this.tbStrip.Items));

            DataManager.settings.last10OpenedScripts = this.recentlyOpenedScriptsToolStripMenuItem.Entries;
            DataManager.settings.last10OpenedPersonas = this.recentlyOpenedPersonalitiesToolStripMenuItem.Entries;

            Settings.save();
        }

        private OpenableFile findFileInTree(TreeNodeCollection nodes, string filePath)
        {
            OpenableFile foundFile = null;
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is OpenableFile && ((OpenableFile)node.Tag).File.FullName == filePath)
                {
                    foundFile =  (OpenableFile)node.Tag;
                }
                else
                {
                    foundFile = findFileInTree(node.Nodes, filePath);
                }
                if (foundFile != null)
                {
                    return foundFile;
                }
            }
            return foundFile;
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

            foreach (string filePath in DataManager.settings.openedTabs)
            {

                OpenableFile file = findFileInTree(this.projectView.Nodes, filePath);
                if (file == null)
                {
                    file = PersonaFile.CreateInstance(filePath);
                }
                openFileIgnoreChecks(file);
            }

            this.recentlyOpenedScriptsToolStripMenuItem.Entries = DataManager.settings.last10OpenedScripts;
            this.recentlyOpenedPersonalitiesToolStripMenuItem.Entries = DataManager.settings.last10OpenedPersonas;
        }

  

        #region project view logic

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
                this.projectView.SelectedNode.BeginEdit();
            }
        }
        private void projectView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null) //no rename occured
            {
                return;
            }

            if (e.Node.Tag is Folder)
            {
                TreeNode parentNode = e.Node.Parent;
                Folder renamedFolder = (Folder)this.projectView.SelectedNode.Tag;

                string parentDir = ((Folder)e.Node.Parent.Tag).Directory.FullName;
                string newFolderPath = parentDir + @"\" + e.Label;
                DirectoryInfo dirInfo = new DirectoryInfo(newFolderPath);

                if (dirInfo.Exists)
                {
                    e.CancelEdit = true;
                    e.Node.EndEdit(true);
                    return;
                }

                e.Node.EndEdit(false);

                renamedFolder.Directory.MoveTo(newFolderPath);

                updateFolderChildrenPath(renamedFolder);
            }
            else if (e.Node.Tag is PersonaFile)
            {
                PersonaFile renamedFile = (PersonaFile)e.Node.Tag;
                string newFullName = ((Folder)e.Node.Parent.Tag).Directory.FullName + @"\" + e.Label;
                FileInfo fileInfo = new FileInfo(newFullName);

                if (fileInfo.Exists)
                {
                    e.CancelEdit = true;
                    e.Node.EndEdit(true);
                    return;
                }

                e.Node.EndEdit(false);
                File.Move(renamedFile.File.FullName, newFullName);
                renamedFile.File = fileInfo;

                if (renamedFile.tab != null)
                {
                    if (TabStripUtils.isTagFlaggedAsModified(renamedFile.tab))
                    {
                        renamedFile.tab.Title = e.Label;
                        TabStripUtils.flagTabAsModified(renamedFile.tab);
                    }
                    else
                    {
                        renamedFile.tab.Title = e.Label;
                    }

                    int index = DataManager.settings.openedTabs.IndexOf(renamedFile.File.FullName);
                    if (index >= 0)
                    {
                        DataManager.settings.openedTabs[index] = newFullName;
                    }
                }
            }

            this.projectView.Invalidate();
        }

        private void updateFolderChildrenPath(Folder folder)
        {
            foreach (PersonaFile file in folder.Files.Values)
            {
                string newFullName = folder.Directory.FullName + file.File.Name;

                int index = DataManager.settings.openedTabs.IndexOf(file.File.FullName);
                if (index >= 0)
                {
                    DataManager.settings.openedTabs[index] = newFullName;
                }

                file.File = new FileInfo(newFullName);
            }
            foreach (Folder childFolder in folder.Folders.Values)
            {
                string newFullName = folder.Directory.FullName + childFolder.Directory.Name;
                childFolder.Directory = new DirectoryInfo(newFullName);
                updateFolderChildrenPath(childFolder);
            }
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
            //MessageBox.Show("Sry but this feature is currently bugged :(");
            //throw new NotImplementedException(); //somehow context menu bugs out treeview Visuals - reproduce: rightclick -> rename a few times until editing does not trigger then try to get renaming triggered with F2 or tripple-click then rename -> treeview bugs out with Node-Texts but files are untouched from the error
            this.projectView.SelectedNode.BeginEdit();
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

        #endregion

        #region tabStripLogic

        private List<FATabStripItem> copyTabCollectionAsList(FATabStripItemCollection tabs)
        {
            List<FATabStripItem> list = new List<FATabStripItem>();
            foreach(FATabStripItem tab in tabs)
            {
                list.Add(tab);
            }

            return list;
        }

        private void tbStrip_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
        {
            this.CurrentTab = tbStrip.SelectedItem;
        }

        private void tbStrip_TabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            e.Cancel = !tryCloseTab(e.Item);
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
            FATabStripItem tab = this.CurrentTab;
            int index = this.tbStrip.Items.IndexOf(tab);
            if (tryCloseTab(tab))
            {
                if (this.tbStrip.Items.Count > 1)
                {
                    if (index >= this.tbStrip.Items.Count)
                    {
                        index = this.tbStrip.Items.Count - 1;
                    }
                    this.tbStrip.SelectedItem = this.tbStrip.Items[index];
                }
            }
        }

        private void closeAllTabsExceptCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<FATabStripItem> tabsToClose = new List<FATabStripItem>(); //need to make a copy because working with ref will point to the actual ItemCollection

            foreach(FATabStripItem tab in this.tbStrip.Items)
            {
                tabsToClose.Add(tab);
            }

            tabsToClose.Remove(this.CurrentTab);

            tryCloseTabs(tabsToClose);
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
            this.recentlyOpenedPersonalitiesToolStripMenuItem.addEntry(path);
        }

        private void OpenPersonaIgnoreChecks(string path)
        {
            Personality persona = new Personality(path);
            TreeNode personaRoot = persona.getRootNode();
            this.projectView.Nodes.Add(personaRoot);
            personaRoot.Expand();
        }

        private void openFile(OpenableFile file)
        {
            if (!DataManager.settings.openedTabs.Contains(file.File.FullName))
            {
                openFileIgnoreChecks(file);
                DataManager.settings.openedTabs.Add(file.File.FullName);
            } else
            {
                foreach(FATabStripItem tab in this.tbStrip.Items)
                {
                    if(tab.Tag.Equals(file))
                    {
                        this.tbStrip.SelectedItem = tab;
                        tab.Focus();
                    }
                }
            }
            this.recentlyOpenedScriptsToolStripMenuItem.addEntry(file.File.FullName);
        }

        private void openFileIgnoreChecks(OpenableFile file)
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
            TabStripUtils.flagTabAsModified(tab);
        }

        #endregion

        #region closing

        private bool tryCloseAllTabs()
        {
            return tryCloseTabs(copyTabCollectionAsList(this.tbStrip.Items));
        }

        private bool tryCloseTabs(IList<FATabStripItem> tabs)
        {
            if (trySaveTabs(tabs))
            {
                closeTabs(tabs);
                return true;
            }
            return false;
        }

        private bool tryCloseTab(FATabStripItem tab)
        {
            if (TabStripUtils.isTagFlaggedAsModified(tab))
            {
                DialogResult result = MessageBox.Show("Save all changes to this file?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (result)
                {
                    case DialogResult.Yes:
                        saveTab(tab);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
            closeTab(tab);
            return true;
        }

        private bool trySaveTabs(IList<FATabStripItem> tabs)
        {
            if (unsavedChanges(tabs))
            {
                DialogResult result = MessageBox.Show("Save all changed files?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (result)
                {
                    case DialogResult.Yes:
                        saveTabs(tabs);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                    default:
                        return false;
                }
            }
            return true;
        }

        public void closeTab(FATabStripItem tab)
        {
            this.tbStrip.Items.Remove(tab);
            DataManager.settings.openedTabs.Remove(((OpenableFile)tab.Tag).File.FullName);
        }

        private void closeTabs(IList<FATabStripItem> tabs)
        {
            foreach(FATabStripItem tab in tabs)
            {
                closeTab(tab);
            }
        }

        private bool unsavedChanges(IList<FATabStripItem> items)
        {
            bool unsaved = false;
            foreach (FATabStripItem tab in items)
            {
                if (TabStripUtils.isTagFlaggedAsModified(tab))
                {
                    unsaved = true;
                    break;
                }
            }

            return unsaved;
        }

        public bool unsavedChanges() //overload currently unused but kept due to later features may be benefitting from it
        {
            return unsavedChanges(copyTabCollectionAsList(tbStrip.Items));
        }

        #endregion

        #region saving

        public void saveCurrentFile()
        {
            saveTab(this.CurrentTab);
        }

        private void saveTab(FATabStripItem tab)
        {
            if (TabStripUtils.isTagFlaggedAsModified(tab))
            {
                ((OpenableFile)tab.Tag).Save();
            }
        }

        public void saveAllFiles()
        {
            saveTabs((IList<FATabStripItem>)this.tbStrip.Items);
        }

        public void saveTabs(IList<FATabStripItem> tabs)
        {
            foreach (FATabStripItem tab in tabs)
            {
                saveTab(tab);
            }
        }

        #endregion

        #region editor logic

        private void ApplyStyle()
        {
            bool unsavedChangesBefore = false;
            if(TabStripUtils.isTagFlaggedAsModified(this.CurrentTab))
            {
                unsavedChangesBefore = true;
            }

            ((OpenableFile)this.CurrentTab.Tag).Redraw();

            if(!unsavedChangesBefore)
            {
                TabStripUtils.unflagTabAsModified(this.CurrentTab);
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

        #region menu

        #region file

        private void openScript_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = DataManager.settings.lastOpenedSingleFileDirectory;


            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PersonaFile newFile = PersonaFile.CreateInstance(ofd.FileName);
                openFile(newFile);

                DataManager.settings.lastOpenedSingleFileDirectory = newFile.File.Directory.FullName;
            }
        }

        private void saveScript_Click(object sender, EventArgs e)
        {
            this.saveCurrentFile();
        }

        private void saveAll_Click(object sender, EventArgs e)
        {
            this.saveAllFiles();
        }

        private void openPersonality_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = DataManager.settings.lastOpenedPersonaDirectory;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                OpenPersona(fbd.SelectedPath);

                DataManager.settings.lastOpenedPersonaDirectory = fbd.SelectedPath;
            }
        }

        #endregion

        #region edit

        private void copy_Click(object sender, EventArgs e)
        {
            ((FastColoredTextBoxNS.FastColoredTextBox)((OpenableFile)this.CurrentTab.Tag).tab.Controls?[0]).Copy(); //still looks very messy we may have to e.g. wrap these functions in OpenableFile so every fileType can react differently to the calls
        }

        private void cut_Click(object sender, EventArgs e)
        {
            ((FastColoredTextBoxNS.FastColoredTextBox)((OpenableFile)this.CurrentTab.Tag).tab.Controls?[0]).Cut();
        }

        private void paste_Click(object sender, EventArgs e)
        {
            ((FastColoredTextBoxNS.FastColoredTextBox)((OpenableFile)this.CurrentTab.Tag).tab.Controls?[0]).Paste();
        }

        private void undo_Click(object sender, EventArgs e)
        {
            ((FastColoredTextBoxNS.FastColoredTextBox)((OpenableFile)this.CurrentTab.Tag).tab.Controls?[0]).Undo();
        }

        private void redo_Click(object sender, EventArgs e)
        {
            ((FastColoredTextBoxNS.FastColoredTextBox)((OpenableFile)this.CurrentTab.Tag).tab.Controls?[0]).Redo();
        }

        private void find_Click(object sender, EventArgs e)
        {
            ((FastColoredTextBoxNS.FastColoredTextBox)((OpenableFile)this.CurrentTab.Tag).tab.Controls?[0]).ShowFindDialog();
        }

        #endregion

        #region makro


        private void record_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void execute_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region toolstripMenu

        private void hotkeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hotkeys hotkeys = new Hotkeys();
            hotkeys.Show();
        }

        #endregion

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
