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
        MarkerStyle GreenStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(180, Color.LightGreen)));
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
            if (CharIsGoto(p) && Control.ModifierKeys == Keys.Control
            || CharIsCheckFlag(p) && Control.ModifierKeys == Keys.Control)
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
                MatchCollection matches = Matches(this.editor.GetLineText(p.iLine), @"(?i)(?<=(\@goto\(|then\(|chance[0-9]{2}\())[A-z_0-9öäüáéíóú+\s]+(?=\))"); //extracting the goto specifier

                for (int i = matches.Count - 1; i >= 0; i--)
                {
                    if (matches[i].Groups[1].Index <= p.iChar)
                    {
                        string gotoName = matches[i].Groups[0].Value;
                        highlightTarget(gotoName);
                        break;
                    }
                }
            }
            else if (CharIsCheckFlag(p) && Control.ModifierKeys == Keys.Control)
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
                highlightTarget(foundTarget.Trim());
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

        private void Editor_MouseDown(Object sender, MouseEventArgs e)
        {
            foreach (Range range in highlightedText)
            {
                range.ClearStyle(GreenStyle);
            }
            highlightedText.Clear();
        }

        private void Editor_DblClick(object sender, EventArgs e)
        {
            string selectedText = this.editor.SelectedText;

            int lineNb = -1;
            foreach (string line in this.editor.Lines)
            {
                lineNb++;
                MatchCollection matches = Matches(line, $@"{selectedText}\b");
                foreach (Match match in matches)
                {
                    Range range = new Range(this.editor, match.Index, lineNb, (match.Index + selectedText.Length), lineNb);
                    range.SetStyle(GreenStyle);
                    highlightedText.Add(range);
                }
            }
        }

        private bool CharIsCheckFlag(Place place)
        {
            if (this.editor.GetStylesOfChar(place).Contains(CheckFlagStyle))
            {
                return true;
            }
            return false;
        }

        private void highlightTarget(string target)
        {
            int index = Match(this.editor.Text, $@"(?<=\n)\({target}\)").Index; //finding the goto destination
            Range range = this.editor.GetRange(index + target.Length + 2, index + target.Length + 2);
            this.editor.Selection = new Range(this.editor, range.Start.iLine);
            this.editor.DoCaretVisible();
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
