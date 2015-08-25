using System.IO;
using FarsiLibrary.Win;
using i00SpellCheck;
using FastColoredTextBoxPlugin;
using FastColoredTextBoxNS;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using static System.Text.RegularExpressions.Regex;
using System;

namespace Personality_Creator.PersonaFiles
{
    public class Script : PersonaFile
    {
        #region capsuled fields

        private FastColoredTextBox editor;

        Style KeywordStyle = new TextStyle(Brushes.DarkBlue, Brushes.White, FontStyle.Regular);
        Style CommandStyle = new TextStyle(Brushes.DarkRed, Brushes.White, FontStyle.Regular);
        Style ResponseStyle = new TextStyle(Brushes.DarkMagenta, Brushes.White, FontStyle.Regular);
        Style ParanthesisStyle = new TextStyle(Brushes.DarkOrange, Brushes.White, FontStyle.Regular);
        Style GotoStyle = new TextStyle(Brushes.DarkRed, Brushes.White, FontStyle.Regular);
        Style FragmentStyle = new TextStyle(Brushes.DarkBlue, Brushes.White, FontStyle.Regular);
        //Style CommentStyle = new TextStyle(Brushes.DarkGreen, Brushes.White, FontStyle.Regular);

        #endregion

        #region properties

        #endregion

        public Script(FileInfo file) : base (file)
        { }

        public Script(string file) : base(file)
        { }

        public override void Save()
        {
            System.IO.File.WriteAllText(this.File.FullName, this.editor.Text);
        }

        public string Read()
        {
            return System.IO.File.ReadAllText(this.File.FullName);
        }

        public override void Redraw()
        {
            this.editor.OnTextChanged(); //redraws editor styles
        }

        public override FATabStripItem CreateTab()
        {
            this.editor = new FastColoredTextBox();

            this.tab = new FATabStripItem();
            this.tab.Title = this.File.Name;
            this.tab.Tag = this;

            //load the spellchecker extension for FastColoredTextBox
            SpellCheckFastColoredTextBox spellCheckerTextBox = new SpellCheckFastColoredTextBox();
            ControlExtensions.LoadSingleControlExtension(editor, spellCheckerTextBox);
            //spellCheckerTextBox.SpellCheckMatch = @"(?<!<[^>]*)[^<^>]*"; // ignore HTML tags
            spellCheckerTextBox.SpellCheckMatch = @"^([\w']+)| ([\w']+)|>([\w']+)"; // Only process words starting a line, following a space or a tag

            this.editor.Parent = this.tab;
            this.editor.Dock = DockStyle.Fill;
            this.editor.Text = this.Read();
            this.editor.Focus();

            this.editor.TextChanged += this.Editor_TextChanged;
            this.editor.KeyDown += this.Editor_KeyDown;
            this.editor.MouseMove += this.Editor_MouseMove;
            this.editor.MouseUp += Editor_MouseUp;

            AutocompleteMenu autoMenu = new AutocompleteMenu(this.editor);
            autoMenu.Items.SetAutocompleteItems(AutoCompleteItemManager.Items);
            autoMenu.MinFragmentLength = 1;
            autoMenu.TopLevel = true;
            autoMenu.Items.MaximumSize = new System.Drawing.Size(200, 300);
            autoMenu.Items.Width = 200;

            return this.tab;
        }

        private void Editor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            this.OnContentChanged(EventArgs.Empty);

            //colorization
            e.ChangedRange.ClearStyle(KeywordStyle);
            e.ChangedRange.SetStyle(KeywordStyle, @"(?<![A-z_0-9öäüáéíóú+])\#[A-z_0-9öäüáéíóú+]+", RegexOptions.None);

            e.ChangedRange.ClearStyle(CommandStyle);
            e.ChangedRange.SetStyle(CommandStyle, @"(?<![A-z_0-9öäüáéíóú+])\@[A-z_0-9öäüáéíóú+]+", RegexOptions.None);

            e.ChangedRange.ClearStyle(ResponseStyle);
            e.ChangedRange.SetStyle(ResponseStyle, @"\[.+\]", RegexOptions.None);

            e.ChangedRange.ClearStyle(ParanthesisStyle);
            e.ChangedRange.SetStyle(ParanthesisStyle, @"(?i)(?<=[A-z_0-9öäüáéíóú+\n])\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(GotoStyle);
            e.ChangedRange.SetStyle(GotoStyle, @"(?i)(\@goto|then)\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(FragmentStyle);
            e.ChangedRange.SetStyle(FragmentStyle, @"(?i)\$\$frag\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);

            //e.ChangedRange.ClearStyle(CommentStyle);
            //e.ChangedRange.SetStyle(CommentStyle, @"(?i)(?<!.)-.*", RegexOptions.None);

            ////code folding
            //e.ChangedRange.SetFoldingMarkers(@"-region", @"-endregion");
        }

        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            Cursor.Position = Cursor.Position; //force cursor redraw
            if (e.KeyCode == Keys.S && Control.ModifierKeys == Keys.Control)
            {
                this.Save();
            }
        }

        private void Editor_MouseMove(object sender, MouseEventArgs e)
        {
            Place p = this.editor.PointToPlace(e.Location);
            if (CharIsGoto(p) && Control.ModifierKeys == Keys.Control)
            {
                this.editor.Cursor = Cursors.Hand;
            }
            else
            {
                this.editor.Cursor = Cursors.IBeam;
            }
        }

        private void Editor_MouseUp(object sender, MouseEventArgs e)
        {
            Place p = this.editor.PointToPlace(e.Location);

            if (CharIsGoto(p) && Control.ModifierKeys == Keys.Control)
            {
                string gotoName = Match(this.editor.GetLineText(p.iLine), @"(?i)(?<=\@goto\(|then\()[A-z_0-9öäüáéíóú+\s]+(?=\))").Value; //extracting the goto specifier
                int index = Match(this.editor.Text, $@"(?<=\n)\({gotoName}\)").Index; //finding the goto destination
                Range range = this.editor.GetRange(index + gotoName.Length + 2, index + gotoName.Length + 2);
                this.editor.Selection = new Range(this.editor, range.Start.iLine);
                this.editor.DoCaretVisible();
            }
        }

        private bool CharIsGoto(Place place)
        {
            if (this.editor.GetStylesOfChar(place).Contains(GotoStyle))
            {
                return true;
            }
            return false;
        }

        private void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            Cursor.Position = Cursor.Position; //force cursor redraw
        }
    }

    public enum ScriptDeclensionType
    {
        Default,
        Chastity,
        Edging,
        Begging
    }
}
