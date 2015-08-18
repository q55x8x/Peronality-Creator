using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace SyntaxTextBox
{
    public partial class SyntaxTextBox : RichTextBox
    {
        public Dictionary<char, Style> IndicatorStyles = new Dictionary<char, Style>();
        public Dictionary<string, Style> Keywords = new Dictionary<string, Style>();
        public Style TextStyle = new Style(); //usage must be thought through

        private char[] textChars;
        public SyntaxTextBox()
        {
            InitializeComponent();
            Style commandStyle = new Style();
            commandStyle.Foreground = Color.DarkRed;

            Style keywordStyle = new Style();
            keywordStyle.FontSize = 8;
            keywordStyle.Foreground = Color.DarkGreen;

            IndicatorStyles.Add('@', commandStyle);

            Keywords.Add("hello", keywordStyle);
            Keywords.Add("blubber", keywordStyle);

        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => ProcessHighlighting());
        }

        private void ProcessHighlighting()
        {
            this.textChars = this.Text.ToCharArray();

            for (int i = 0; i < textChars.Length; i++)
            {
                if(textChars[i-1] == ' ')
                {
                    bool isIndicator = false;

                    foreach (char indicator in IndicatorStyles.Keys)
                    {
                        if (textChars[i] == indicator)
                        {
                            isIndicator = true;
                            i += this.HighlightAt(i, IndicatorStyles[indicator]);
                        }
                    }

                    if(isIndicator)
                    {
                        continue;
                    }

                    bool isNoKeyword = false;

                    foreach (string keyword in Keywords.Keys)
                    {
                        for (int u = i; textChars[u + i] == ' '; u++)
                        {
                            if (textChars[u + i] != keyword[u])
                            {
                                isNoKeyword = true;
                                break;
                            }
                        }

                        if(!isNoKeyword) //keyword found
                        {
                            i += HighlightAt(i, Keywords[keyword]);
                            break;
                        }
                    }
                }
            }
        }

        //returns word-offset
        private int HighlightAt(int index, Style style)
        {
            int offset;

            for (offset = 0; textChars[index + (offset+1)] == ' '; offset++)
            {
                
            }

            this.Select(index, index + offset);
            this.SelectionBackColor = style.Background;
            this.SelectionColor = style.Foreground;
            this.SelectionFont = new Font(style.Fontfamily, style.FontSize, style.Fontstyle);


            return offset;
        }
    }

    [Serializable]
    public class Style
    {
        public Color Foreground = Color.Black;
        public Color Background = Color.White;
        public FontFamily Fontfamily = new FontFamily(System.Drawing.Text.GenericFontFamilies.SansSerif);
        public FontStyle Fontstyle = FontStyle.Regular;
        public int FontSize = 14;
        public Style(Color Foreground, Color Background, FontFamily Fontfamily, int Fontsize, FontStyle Fontstyle)
        {
            this.Foreground = Foreground;
            this.Background = Background;
            this.Fontfamily = Fontfamily;
            this.FontSize = Fontsize;
            this.Fontstyle = Fontstyle;
        }

        public Style()
        { }
    }
}
