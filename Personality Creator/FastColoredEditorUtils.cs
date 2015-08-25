using FastColoredTextBoxNS;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.Regex;


namespace Personality_Creator
{
    class FastColoredEditorUtils
    {
        public static MarkerStyle GreenStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(180, Color.LightGreen)));

        public static bool cursorIsOnTextOfStyle(Place place, Style style, FastColoredTextBox editor)
        {
            return editor.GetStylesOfChar(place).Contains(style);
        }

        public static void SelectText(string target, FastColoredTextBox editor)
        {
            int index = Match(editor.Text, $@"(?<=\n)\({target}\)").Index; //finding the goto destination
            Range range = editor.GetRange(index + target.Length + 2, index + target.Length + 2);
            editor.Selection = new Range(editor, range.Start.iLine);
            editor.DoCaretVisible();
        }

        public static List<Range> highlightText(string text, FastColoredTextBox editor)
        {
            List<Range> highlightedText = new List<Range>();
            int lineNb = -1;
            foreach (string line in editor.Lines)
            {
                lineNb++;
                MatchCollection matches = Matches(line, $@"{text}\b");
                foreach (Match match in matches)
                {
                    Range range = new Range(editor, match.Index, lineNb, (match.Index + text.Length), lineNb);
                    range.SetStyle(GreenStyle);
                    highlightedText.Add(range);
                }
            }
            return highlightedText;
        }

        public static void clearHighlightedText(List<Range> highlightedText)
        {
            foreach (Range range in highlightedText)
            {
                range.ClearStyle(GreenStyle);
            }
            highlightedText.Clear();
        }

    }
}
