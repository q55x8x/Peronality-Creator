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
        ImageList iconList = new ImageList();
        public Personality OpenPersona
        {
            get;
            set;
        }

        private string _SelectedItem;
        public string SelectedItem
        {
            get
            {
                return this._SelectedItem;
            }
            set
            {
                if (File.GetAttributes(value).HasFlag(FileAttributes.Directory))
                {
                    this.SelectedItemIsFolder = true;
                }
                else
                {
                    this.SelectedItemIsFolder = false;
                }
                this._SelectedItem = value;
            }
        }
        public bool SelectedItemIsFolder
        {
            get;
            set;
        }
        private void projectView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.SelectedItem = this.OpenPersona.Path.Parent.FullName + @"\" + this.projectView.SelectedNode.FullPath;
        }
        private void projectView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.projectView.SelectedNode = e.Node; //force selection change

            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStripProjectView.Show(this.projectView, e.Location.X, e.Location.Y);
                if (this.SelectedItemIsFolder)
                {
                    this.newScriptToolStripMenuItem.Enabled = true;
                    this.newFolderToolStripMenuItem.Enabled = true;
                }
                else
                {
                    this.newScriptToolStripMenuItem.Enabled = false;
                    this.newFolderToolStripMenuItem.Enabled = false;
                }
            }
        }

        private FATabStripItem _SelectedTab;
        public FATabStripItem CurrentTab
        {
            get
            {
                return this._SelectedTab;
            }
            set
            {
                this._SelectedTab = value;
                if (value != null)
                { 
                    this.CurrentEditor = ((FastColoredTextBox)this.CurrentTab.Controls[0]);
                }
            }
        }
        private void tbStrip_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
        {
            this.CurrentTab = this.tbStrip.SelectedItem;
        }

        public FastColoredTextBox CurrentEditor
        {
            get;
            set;
        }

        public mainFrm()
        {
            InitializeComponent();
            iconList.Images.Add(Personality_Creator.Properties.Resources.folder);
            iconList.Images.Add(Personality_Creator.Properties.Resources.file);

            this.projectView.ImageList = iconList;
        }

        #region style processing

        Style KeywordStyle = new TextStyle(Brushes.DarkBlue, Brushes.White, FontStyle.Regular);
        Style CommandStyle = new TextStyle(Brushes.DarkRed, Brushes.White, FontStyle.Regular);
        Style ResponseStyle = new TextStyle(Brushes.DarkMagenta, Brushes.White, FontStyle.Regular);
        Style InterruptStyle = new TextStyle(Brushes.DarkOrange, Brushes.White, FontStyle.Regular);
        Style GotoStyle = new TextStyle(Brushes.DarkRed, Brushes.White, FontStyle.Regular);
        Style FragmentStyle = new TextStyle(Brushes.DarkGreen, Brushes.White, FontStyle.Regular);

        private void Editor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            if (!this.CurrentTab.Title.StartsWith("*"))
            {
                this.CurrentTab.Title = this.CurrentTab.Title.Insert(0, "*");
            }

            e.ChangedRange.ClearStyle(KeywordStyle);
            e.ChangedRange.SetStyle(KeywordStyle, @"(?<![A-z_0-9öäüáéíóú+])\#[A-z_0-9öäüáéíóú+]+", RegexOptions.None);

            e.ChangedRange.ClearStyle(CommandStyle);
            e.ChangedRange.SetStyle(CommandStyle, @"(?<![A-z_0-9öäüáéíóú+])\@[A-z_0-9öäüáéíóú+]+", RegexOptions.None);

            e.ChangedRange.ClearStyle(ResponseStyle);
            e.ChangedRange.SetStyle(ResponseStyle, @"\[.+\]", RegexOptions.None);

            e.ChangedRange.ClearStyle(InterruptStyle);
            e.ChangedRange.SetStyle(InterruptStyle, @"(?i)(?<![A-z_0-9öäüáéíóú+\n])\@[A-z_0-9öäüáéíóú+]+\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);
            e.ChangedRange.SetStyle(InterruptStyle, @"(?i)(?<!\$\$frag)\(.+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(GotoStyle);
            e.ChangedRange.SetStyle(GotoStyle, @"(?i)(\@goto|then)\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(FragmentStyle);
            e.ChangedRange.SetStyle(FragmentStyle, @"(?i)\$\$frag\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.Multiline);
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
            if(e.KeyCode == Keys.S && ModifierKeys == Keys.Control)
            {
                saveCurrentFile();
            }
        }

        #endregion

        private  void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                openFile(ofd.FileName);
            }
        }
        private void projectView_DoubleClick(object sender, EventArgs e)
        {
            if (!SelectedItemIsFolder)
            {
                openFile(this.SelectedItem);
            }
        }

        private void openFile(string Path)
        {
            foreach(FATabStripItem aTab in this.tbStrip.Items)
            {
                if(aTab.Tag.ToString() == Path)
                {
                    tbStrip.SelectedItem = aTab;
                    return;
                }
            }

            FileInfo file = new FileInfo(Path);

            StreamReader sr = new StreamReader(Path);

            FastColoredTextBox editor = new FastColoredTextBox();
            editor.Dock = DockStyle.Fill;

            FATabStripItem tab = new FATabStripItem(file.Name, editor);
            tab.Tag = Path;

            tbStrip.AddTab(tab);
            tbStrip.SelectedItem = tab;
            editor.Focus();
            editor.TextChanged += Editor_TextChanged;
            editor.MouseMove += Editor_MouseMove;
            editor.MouseDown += Editor_MouseDown;
            editor.KeyDown += Editor_KeyDown;
            editor.KeyUp += Editor_KeyUp;

            editor.Text = sr.ReadToEnd();

            sr.Close();

            saveCurrentFile(); //TODO: just a workaround to remove asterisk after loading a file
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveCurrentFile();
        }

        private void saveCurrentFile()
        {
            File.WriteAllText(this.CurrentTab.Tag.ToString(), ((FastColoredTextBox)this.CurrentTab.Controls[0]).Text);
            if(this.CurrentTab.Title.StartsWith("*"))
            {
                this.CurrentTab.Title = this.CurrentTab.Title.Remove(0, 1);
            }
        }

        private void openPersonalityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.OpenPersona = new Personality(fbd.SelectedPath);
                this.projectView.Nodes.Add(addNode(new DirectoryInfo(fbd.SelectedPath)));
            }
        }

        private TreeNode addNode(DirectoryInfo dir)
        {
            TreeNode node = new TreeNode(dir.Name);
            node.ImageIndex = 0;

            foreach(DirectoryInfo subdir in dir.GetDirectories())
            {
                node.Nodes.Add(addNode(subdir));
            }

            foreach(FileInfo file in dir.GetFiles())
            {
                TreeNode fileNode = new TreeNode(file.Name);
                fileNode.ImageIndex = 1;
                fileNode.SelectedImageIndex = 1;

                node.Nodes.Add(fileNode);
            }

            return node;
        }

        private void newScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.InitialDirectory = this.SelectedItem;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(sfd.FileName);
                FileStream fs = new FileStream(file.FullName, FileMode.Create);
                fs.Close();

                TreeNode fileNode = new TreeNode(file.Name);
                fileNode.ImageIndex = 1;
                fileNode.SelectedImageIndex = 1;

                this.projectView.SelectedNode.Nodes.Add(fileNode);

                openFile(file.FullName);
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFolderDialog nfd = new NewFolderDialog();
            
            if(nfd.Show() == DialogResult.OK)
            {
                Directory.CreateDirectory(this.SelectedItem + @"\" + nfd.NewFolderName);
                this.projectView.SelectedNode.Nodes.Add(nfd.NewFolderName);
            }
        }

        private void tbStrip_TabStripItemClosing(TabStripItemClosingEventArgs e)
        {
            if(this.CurrentTab.Title.StartsWith("*"))
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

        private void mainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool unsavedChanges = false;
            foreach(FATabStripItem tab in this.tbStrip.Items)
            {
                if(tab.Title.StartsWith("*"))
                {
                    unsavedChanges = true;
                    break;
                }
            }

            if(unsavedChanges)
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

        private void hotkeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hotkeys hotkeys = new Hotkeys();
            hotkeys.Show();
        }

        #region assembling

        private void assembleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DirectoryInfo ReleaseDir = new DirectoryInfo(this.OpenPersona.Path + @"\Release");

            if (!Directory.Exists(ReleaseDir.FullName))
            {
                Directory.CreateDirectory(ReleaseDir.FullName);
            }
            if (!Directory.Exists(ReleaseDir + this.OpenPersona.Name))
            {
                Directory.CreateDirectory(ReleaseDir + this.OpenPersona.Name);
            }

            assembleDirectory(this.OpenPersona.Path);
            
            Directory.Delete(ReleaseDir + @"\" + this.OpenPersona.Name + @"\Fragments", true);

            string timestamp = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToShortTimeString();
            timestamp = timestamp.Replace(":", "_");
            timestamp = timestamp.Replace(".", "_");

            string zipName = this.OpenPersona.Name + "_" + "Release_" + timestamp + ".zip";

            string sourceFolder = ReleaseDir.FullName;

            string tempdest = this.OpenPersona.Path + @"\" + zipName; //workaround as zipping a directory into it self is not supported by ZipFile class
            string destFileName = ReleaseDir + @"\" + zipName;
            
            foreach(FileInfo file in ReleaseDir.GetFiles()) //workaround so zips do not accumulate in themselfes
            {
                if(file.Name.Contains(this.OpenPersona.Name + "_" + "Release_"))
                {
                    file.MoveTo(this.OpenPersona.Path + @"\" + file.Name);
                }
            }

            ZipFile.CreateFromDirectory(sourceFolder, tempdest, CompressionLevel.Optimal, false);

            foreach (FileInfo file in this.OpenPersona.Path.GetFiles())
            {
                if (file.Name.Contains(this.OpenPersona.Name + "_" + "Release_"))
                {
                    file.MoveTo(ReleaseDir + @"\" + file.Name);
                }
            }
        }

        private void assembleDirectory(DirectoryInfo dir)
        {
            DirectoryInfo releasePath = new DirectoryInfo(this.OpenPersona.Path + @"\Release\" + this.OpenPersona.Name);

            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if (subDir.Name == "Release")
                { continue; }

                string relPath = subDir.FullName.Replace(this.OpenPersona.Path.FullName, "");

                if (!Directory.Exists(releasePath + @"\" + relPath))
                {
                    Directory.CreateDirectory(releasePath + @"\" + relPath);
                }

                assembleDirectory(subDir);
            }

            foreach (FileInfo file in dir.GetFiles())
            {
                if (dir.Name == "Fragments") //needed fragments will recursive assemble themselves while a file is assembled that needs a fragment
                { break; }

                if (file.Extension == ".txt")
                {
                    assembleFile(file);
                }
                else
                {
                    string relPath = file.FullName.Replace(this.OpenPersona.Path.FullName, "");
                    File.Copy(file.FullName, this.OpenPersona.Path + @"\Release\" + this.OpenPersona.Name + relPath, true);
                }
            }
        }

        private void assembleFile(FileInfo file)
        {
            string relPath = file.FullName.Replace(this.OpenPersona.Path.FullName, "");
            string content = File.ReadAllText(file.FullName);
            string replaceFragment = "";

            foreach(Match regexMatch in Regex.Matches(content, @"(?i)\$\$frag\([A-z_0-9öäüáéíóú+\s]+\)"))
            {
                string fragmentName = Regex.Match(content.Substring(regexMatch.Index, regexMatch.Length), @"(?i)(?<=\$\$frag\()[A-z_0-9öäüáéíóú+\s]+(?=\))").Value;
                assembleFile(new FileInfo(this.OpenPersona.Path + @"\Fragments\" + fragmentName + @".txt")); //recursive fragment assembly
                replaceFragment = File.ReadAllText(this.OpenPersona.Path + @"\Release\" + this.OpenPersona.Name + @"\Fragments\" + fragmentName + @".txt"); //loaded already assembled fragments from the release
                content = content.Remove(regexMatch.Index, regexMatch.Length);
                content = content.Insert(regexMatch.Index, replaceFragment);
            }

            if (File.Exists(this.OpenPersona.Path + @"\Release\" + this.OpenPersona.Name + relPath))
            {
                File.Delete(this.OpenPersona.Path + @"\Release\" + this.OpenPersona.Name + relPath);
            }

            File.WriteAllText(this.OpenPersona.Path + @"\Release\" + this.OpenPersona.Name + relPath, content);
        }

        #endregion 
    }
}
