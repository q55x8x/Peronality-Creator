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
using System.Collections.Generic;

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
        Style CheckFlagStyle = new TextStyle(Brushes.DarkRed, Brushes.White, FontStyle.Regular);
        Style FragmentStyle = new TextStyle(Brushes.DarkBlue, Brushes.White, FontStyle.Regular);
        //Style CommentStyle = new TextStyle(Brushes.DarkGreen, Brushes.White, FontStyle.Regular);

        private List<Range> highlightedText = new List<Range>();


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
            spellCheckerTextBox.SpellCheckMatch = @"^([\w']+)| ([\w']+)|>([\w']+)"; // Only process words starting a line, following a space or a tag

            this.editor.Parent = this.tab;
            this.editor.Dock = DockStyle.Fill;
            this.editor.Text = this.Read();
            this.editor.Focus();

            this.editor.TextChanged += this.Editor_TextChanged;
            this.editor.KeyDown += this.Editor_KeyDown;
            this.editor.MouseMove += this.Editor_MouseMove;
            this.editor.MouseUp += Editor_MouseUp;
            this.editor.MouseDown += Editor_MouseDown;
            this.editor.DoubleClick += Editor_DblClick;

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
            e.ChangedRange.SetStyle(ParanthesisStyle, @"(?i)(?<=[A-z_0-9öäüáéíóú+\n])\([A-z_0-9öäüáéíóú,+\s]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(GotoStyle);
            e.ChangedRange.SetStyle(GotoStyle, @"(?i)(\@goto|then|chance[0-9]{2})\([A-z_0-9öäüáéíóú+\s]+\)", RegexOptions.None);

            e.ChangedRange.ClearStyle(CheckFlagStyle);
            e.ChangedRange.SetStyle(CheckFlagStyle, @"(?i)(\@checkflag)\([A-z_0-9öäüáéíóú,+\s]+\)", RegexOptions.None);

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
            if (FastColoredEditorUtils.cursorIsOnTextOfStyle(p, GotoStyle, this.editor) && Control.ModifierKeys == Keys.Control
            || FastColoredEditorUtils.cursorIsOnTextOfStyle(p, CheckFlagStyle, this.editor) && Control.ModifierKeys == Keys.Control)
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

            if (FastColoredEditorUtils.cursorIsOnTextOfStyle(p, GotoStyle, this.editor) && Control.ModifierKeys == Keys.Control)
            {
                string gotoName = findGotoClickTarget(p);
                if (gotoName != null)
                {
                    FastColoredEditorUtils.SelectText(gotoName, this.editor);
                }
            }
            else if (FastColoredEditorUtils.cursorIsOnTextOfStyle(p, GotoStyle, this.editor) && Control.ModifierKeys == Keys.Control)
            {
                string foundTarget = findCheckFlagClickedTarget(p);
                FastColoredEditorUtils.SelectText(foundTarget.Trim(), this.editor);
            }
        }

        private string findGotoClickTarget(Place p)
        {
            MatchCollection matches = Matches(this.editor.GetLineText(p.iLine), @"(?i)(?<=(\@goto\(|then\(|chance[0-9]{2}\())[A-z_0-9öäüáéíóú+\s]+(?=\))"); //extracting the goto specifier

            string gotoName = null;
            for (int i = matches.Count - 1; i >= 0 && gotoName == null; i--)
            {
                if (matches[i].Groups[1].Index <= p.iChar)
                {
                    gotoName = matches[i].Groups[0].Value;
                }
            }

            return gotoName;
        }

        private string findCheckFlagClickedTarget(Place p)
        {
            Match checkflagTargetsMatch = Match(this.editor.GetLineText(p.iLine), @"(?i)(?<=\@checkflag\()([A-z_0-9öäüáéíóú+\s]+)(,([A-z_0-9öäüáéíóú+\s]+))*(?=\))"); //extracting the checkflag specifier

            string foundTarget = null;
            for (int i = checkflagTargetsMatch.Groups[3].Captures.Count - 1; i >= 0 && foundTarget == null; i--)
            {
                if (checkflagTargetsMatch.Groups[3].Captures[i].Index <= p.iChar)
                {
                    foundTarget = checkflagTargetsMatch.Groups[3].Captures[i].Value;
                }
            }

            if (foundTarget == null)
            {
                foundTarget = checkflagTargetsMatch.Groups[1].Value;
            }

            return foundTarget;
        }

        private void Editor_KeyUp(object sender, KeyEventArgs e)
        {
            Cursor.Position = Cursor.Position; //force cursor redraw
        }

        private void Editor_MouseDown(Object sender, MouseEventArgs e)
        {
            FastColoredEditorUtils.clearHighlightedText(highlightedText);
        }

        private void Editor_DblClick(object sender, EventArgs e)
        {
            this.highlightedText = FastColoredEditorUtils.highlightText(this.editor.SelectedText, this.editor);
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
