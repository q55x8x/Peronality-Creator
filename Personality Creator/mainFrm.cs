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

        public Dictionary<string, Personality> OpenPersonas = new Dictionary<string, Personality>(); //dont know exactly if I want to put this into DataManager or not
        public mainFrm()
        {
            InitializeComponent();
            this.projectView.ImageList = DataManager.iconList;
        }

        private void OpenPersona(string path)
        {
            Personality persona = new Personality(path);
            this.OpenPersonas.Add(persona.Name, persona);
            this.projectView.Nodes.Add(persona.getRootNode());
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
            fbd.SelectedPath = DataManager.settings.lastOpenedPersonaPath;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                DataManager.settings.lastOpenedPersonaPath = fbd.SelectedPath;
                OpenPersona(fbd.SelectedPath);
            }
        }

        #endregion

        #region project view logic

        private void projectView_DoubleClick(object sender, EventArgs e)
        {
            if (projectView.SelectedNode.Tag.GetType() == typeof(Script))
            {
                openFile((PersonaFile)projectView.SelectedNode.Tag);
            }
            if (projectView.SelectedNode.Tag.GetType() == typeof(PersonaFile))
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

        private void openFile(PersonaFile file)
        {
            if(file.FileType == PersonaFileType.Script)
            {
                openScript((Script)file);
            }
        }

        private void openScript(Script script)
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

            this.tbStrip.AddTab(newTab);

            this.tbStrip.SelectedItem = newTab;

            this.ApplyStyle();
        }

        #endregion

        #region saving

        public void saveCurrentFile()
        {
            if(this.CurrentFile.FileType == PersonaFileType.Script)
            {
                ((Script)this.CurrentFile).Save(this.CurrentEditor.Text);

                if(this.CurrentTab.Title.StartsWith("*"))
                {
                    this.CurrentTab.Title = this.CurrentTab.Title.Remove(0, 1);
                }
            }
        }

        #endregion

        #region style processing

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
            var p = this.CurrentEditor.PointToPlace(e.Location);
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
            var p = this.CurrentEditor.PointToPlace(e.Location);
            if (CharIsGoto(p) && ModifierKeys == Keys.Control)
            {
                string gotoName = Regex.Match(this.CurrentEditor.GetLineText(p.iLine), @"(?i)(?<=\@goto|then)\([A-z_0-9öäüáéíóú+\s]+\)").Value.Trim("()".ToCharArray()); //sadly there is currently no better way of  
                int index = Regex.Match(this.CurrentEditor.Text, @"(?<=\n)\(" + gotoName + @"\)").Index; //jumping to a match :( then extract the index and
                Range range = this.CurrentEditor.GetRange(index, index + 1); //getting its range
                this.CurrentEditor.Navigate(range.ToLine); //to navigate to its line
            }
        }

        private bool CharIsGoto(Place place)
        {
            var mask = this.CurrentEditor.GetStyleIndexMask(new Style[] { GotoStyle });

            if (place.iChar < this.CurrentEditor.GetLineLength(place.iLine))
            {
                if ((this.CurrentEditor[place].style & mask) != 0)
                {
                    return true;
                }
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
