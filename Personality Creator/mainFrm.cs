using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using FarsiLibrary.Win;
using System.IO;
using System.IO.Compression;
using Personality_Creator.PersonaFiles;
using Personality_Creator.PersonaFiles.Scripts;
using static System.Text.RegularExpressions.Regex;

namespace Personality_Creator
{
    public partial class mainFrm : Form
    {
        private FastColoredTextBox currentEditor;
        private FATabStripItem currentTab;
        private PersonaFile currentFile;

        public FastColoredTextBox CurrentEditor
        {
            get
            {
                return currentEditor;
            }

            set
            {
                currentEditor = value;
            }
        }

        public FATabStripItem CurrentTab
        {
            get
            {
                return currentTab;
            }

            set
            {
                currentTab = value;
                if (value != null)
                {
                    this.CurrentEditor = (FastColoredTextBox)this.CurrentTab.Controls[0];
                    this.CurrentFile = (PersonaFile)this.CurrentTab.Tag;
                }
            }
        }

        public PersonaFile CurrentFile
        {
            get
            {
                return currentFile;
            }

            set
            {
                currentFile = value;
            }
        }

        public Dictionary<string, Personality> OpenedPersonas = new Dictionary<string, Personality>(); //dont know exactly if I want to put this into DataManager or not
        public Dictionary<string, PersonaFile> OpenedUnAssociatedFiles = new Dictionary<string, PersonaFile>();
        public mainFrm()
        {
            InitializeComponent();
            this.projectView.ImageList = DataManager.iconList;
            this.renameToolStripMenuItem.Enabled = false;
    }

        private void mainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.save();

            bool unsavedChanges = false;
            foreach (FATabStripItem tab in this.tbStrip.Items)
            {
                if (tab.Title.StartsWith("*"))
                {
                    unsavedChanges = true;
                    break;
                }
            }

            if (unsavedChanges)
            {
                switch (MessageBox.Show("Save all changed files?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        foreach (FATabStripItem tab in this.tbStrip.Items)
                        {
                            this.CurrentTab = tab;
                            saveCurrentFile();
                        }
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void mainFrm_Load(object sender, EventArgs e)
        {
            DataManager.initDataManager();
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
                DataManager.settings.lastOpenedPersonaDirectory = fbd.SelectedPath;
                OpenPersona(fbd.SelectedPath);
            }
        }

        private void openScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = DataManager.settings.lastOpenedSingleFileDirectory;

            
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                PersonaFile newFile = new PersonaFile(ofd.FileName);
                this.addUnAssociatedFile(newFile);
                openFile(newFile);
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
            if(this.projectView.SelectedNode.Tag.GetType().BaseType.BaseType == typeof(PersonaFile))
            { 
                openFile((PersonaFile)projectView.SelectedNode.Tag);
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

                FileStream fs = new FileStream(file.File.FullName, FileMode.Create);
                fs.Close();

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

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sry but this feature is currently bugged :(");
            throw new NotImplementedException(); //somehow context menu bugs out treeview Visuals - reproduce: rightclick -> rename a few times until editing does not trigger then try to get renaming triggered with F2 or tripple-click then rename -> treeview bugs out with Node-Texts but files are untouched from the error
            this.BeginEditNode();
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
            if (this.CurrentTab.Title.StartsWith("*"))
            {
                switch (MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        saveCurrentFile();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        #endregion

        #region opening
        
        private void OpenPersona(string path)
        {
            Personality persona = new Personality(path);
            this.OpenedPersonas.Add(persona.Name, persona);
            this.projectView.Nodes.Add(persona.getRootNode());
        }

        private void openFile(PersonaFile file)
        {
            if(file.GetType().BaseType == typeof(Script))
            {
                openScript((Script)file);
            }
        }

        private void openScript(Script script)
        {
            if(script.GetType() == typeof(Module))
            {
                openModule((Module)script);
            }
            else if(script.GetType() == typeof(Fragment))
            {
                openFragment((Fragment)script);
            }
            else if(script.GetType() == typeof(FragmentedScript))
            {
                openFragmentedScript((FragmentedScript)script);
            }
        }

        private void openModule(Module script)
        {
            FastColoredTextBox newEditor = new FastColoredTextBox();

            FATabStripItem newTab = new FATabStripItem();
            newTab.Title = script.File.Name;
            newTab.Tag = script;

            newEditor.Parent = newTab;
            newEditor.Dock = DockStyle.Fill;
            newEditor.Text = script.Read();
            newEditor.Focus();

            newEditor.TextChanged += this.Editor_TextChanged;
            newEditor.KeyDown += Editor_KeyDown;
            newEditor.MouseMove += Editor_MouseMove;
            newEditor.MouseDown += Editor_MouseDown;

            this.tbStrip.AddTab(newTab);

            this.tbStrip.SelectedItem = newTab;

            this.ApplyStyle();
        }

        private void openFragment(Fragment script)
        {
            throw new NotImplementedException();
        }

        private void openFragmentedScript(FragmentedScript script)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region saving

        public void saveCurrentFile()
        {
            if(this.CurrentFile.GetType().BaseType == typeof(Script))
            {
                ((Script)this.CurrentFile).Save(this.CurrentEditor.Text);

                if(this.CurrentTab.Title.StartsWith("*"))
                {
                    this.CurrentTab.Title = this.CurrentTab.Title.Remove(0, 1);
                }
            }
        }

        public void closeTab(FATabStripItem tab, bool save)
        {
            if (tab.Title.StartsWith("*") && save)
            {
                ((Script)tab.Tag).Save(((FastColoredTextBox)tab.Controls[0]).Text);
                tab.Title = tab.Title.Remove(0, 1);
            }
            this.tbStrip.Items.Remove(tab);
        }

        #endregion

        #region editor logic

        Style KeywordStyle = new TextStyle(Brushes.DarkBlue, Brushes.White, FontStyle.Regular);
        Style CommandStyle = new TextStyle(Brushes.DarkRed, Brushes.White, FontStyle.Regular);
        Style ResponseStyle = new TextStyle(Brushes.DarkMagenta, Brushes.White, FontStyle.Regular);
        Style InterruptStyle = new TextStyle(Brushes.DarkOrange, Brushes.White, FontStyle.Regular);
        Style GotoStyle = new TextStyle(Brushes.DarkRed, Brushes.White, FontStyle.Regular);
        Style FragmentStyle = new TextStyle(Brushes.DarkBlue, Brushes.White, FontStyle.Regular);
        Style CommentStyle = new TextStyle(Brushes.DarkGreen, Brushes.White, FontStyle.Regular);


        private void Editor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            //set unsaved changes
            if (!this.CurrentTab.Title.StartsWith("*"))
            {
                this.CurrentTab.Title = this.CurrentTab.Title.Insert(0, "*");
            }

            //colorization
            e.ChangedRange.ClearStyle(KeywordStyle);
            e.ChangedRange.SetStyle(KeywordStyle, @"(?<![A-z_0-9öäüáéíóú+])\#[A-z_0-9öäüáéíóú+]+", RegexOptions.None);

            e.ChangedRange.ClearStyle(CommandStyle);
            e.ChangedRange.SetStyle(CommandStyle, @"(?<![A-z_0-9öäüáéíóú+])\@[A-z_0-9öäüáéíóú+]+", RegexOptions.None);

            e.ChangedRange.ClearStyle(ResponseStyle);
            e.ChangedRange.SetStyle(ResponseStyle, @"\[.+\]", RegexOptions.None);

            e.ChangedRange.ClearStyle(InterruptStyle);
            e.ChangedRange.SetStyle(InterruptStyle, @"(?i)(?<![A-z_0-9öäüáéíóú+\n])\@[A-z_0-9öäüáéíóú+]+\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);
            e.ChangedRange.SetStyle(InterruptStyle, @"(?i)(?<!.)\([A-z_0-9öäüáéíóú+]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(GotoStyle);
            e.ChangedRange.SetStyle(GotoStyle, @"(?i)(\@goto|then)\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(FragmentStyle);
            e.ChangedRange.SetStyle(FragmentStyle, @"(?i)\$\$frag\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(CommentStyle);
            e.ChangedRange.SetStyle(CommentStyle, @"(?i)(?<!.)-.*", RegexOptions.None);

            //code folding
            e.ChangedRange.SetFoldingMarkers(@"-region", @"-endregion");
        }

        private void ApplyStyle()
        {
            bool unsavedChangesBefore = false;
            if(this.CurrentTab.Title.StartsWith("*"))
            {
                unsavedChangesBefore = true;
            }

            this.CurrentEditor.OnTextChanged(); //redraws editor styles

            if(!unsavedChangesBefore)
            {
                //if (this.CurrentTab.Title.StartsWith("*")) //this should be redundant
                //{
                this.CurrentTab.Title = this.CurrentTab.Title.Remove(0, 1);
                //}
            }
        }

        private void Editor_MouseMove(object sender, MouseEventArgs e)
        {
            Place p = this.CurrentEditor.PointToPlace(e.Location);
            if (CharIsGoto(p) && ModifierKeys == Keys.Control)
            {
                this.CurrentEditor.Cursor = Cursors.Hand;
            }
            else
            {
                this.CurrentEditor.Cursor = Cursors.IBeam;
            }
        }

        private void Editor_MouseDown(object sender, MouseEventArgs e)
        {
            Place p = this.CurrentEditor.PointToPlace(e.Location);

            if (CharIsGoto(p) && ModifierKeys == Keys.Control)
            {
                string gotoName = Match(this.CurrentEditor.GetLineText(p.iLine), @"(?i)(?<=\@goto\(|then\()[A-z_0-9öäüáéíóú+\s]+(?=\))").Value; //sadly there is currently no better way of  
                int index = Match(this.CurrentEditor.Text, @"(?<=\n)\(" + gotoName + @"\)").Index; //jumping to a match :( then extract the index and
                Range range = this.CurrentEditor.GetRange(index, index + 1); //getting its range
                //this.CurrentEditor.Navigate(range.ToLine); //to navigate to its line
                Place line = new Place(gotoName.Length + 2, range.ToLine);
                this.currentEditor.Selection.Start = line;
                this.currentEditor.DoCaretVisible();
            }
        }

        private bool CharIsGoto(Place place)
        {
            if(this.currentEditor.GetStylesOfChar(place).Contains(GotoStyle))
            {
                return true;
            }
            return false;
        }

        private void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            Cursor.Position = Cursor.Position; //force cursor redraw
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Position = Cursor.Position; //force cursor redraw
            if (e.KeyCode == Keys.S && ModifierKeys == Keys.Control)
            {
                saveCurrentFile();
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
