using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personality_Creator.UI
{
    public partial class Hotkeys : Form
    {
        public Hotkeys()
        {
            InitializeComponent();
        }

        private void Hotkeys_Load(object sender, EventArgs e)
        {
            this.lblText.Text = "Ctrl + S saves current file\r\n" +
                                "Left, Right, Up, Down, Home, End, PageUp, PageDown - moves caret\r\n" +
                                "Shift + (Left, Right, Up, Down, Home, End, PageUp, PageDown) -moves caret with selection\r\n" +
                                "Ctrl + F, Ctrl + H - shows Find and Replace dialogs\r\n" +
                                "F3 - find next\r\n" +
                                "Ctrl + G - shows GoTo dialog\r\n" +
                                "Ctrl + (C, V, X) -standard clipboard operations\r\n" +
                                "Ctrl + A - selects all text\r\n" +
                                "Ctrl + Z, Alt + Backspace, Ctrl + R - Undo / Redo opertions\r\n" +
                                "Ctrl + Click on @Goto, #Vocab, Then, @CheckFlag statement to jump to/open corresponding line/file\r\n" +
                                "Tab, Shift + Tab - increase / decrease left indent of selected range\r\n" +
                                "Ctrl + Home, Ctrl + End - go to first/ last char of the text\r\n" +
                                "Shift + Ctrl + Home, Shift + Ctrl + End - go to first/ last char of the text with selection\r\n" +
                                "Ctrl + Left, Ctrl + Right - go word left/ right\r\n" +
                                "Shift + Ctrl + Left, Shift + Ctrl + Right - go word left/ right with selection\r\n" +
                                "Ctrl + -, Shift + Ctrl + - -backward / forward navigation\r\n" +
                                "Ctrl + U, Shift + Ctrl + U - converts selected text to upper/ lower case\r\n" +
                                "Ins - switches between Insert Mode and Overwrite Mode\r\n" +
                                "Ctrl + Backspace, Ctrl + Del - remove word left/ right\r\n" +
                                "Alt + Mouse, Alt + Shift + (Up, Down, Right, Left) -enables column selection mode\r\n" +
                                "Alt + Up, Alt + Down - moves selected lines up / down\r\n" +
                                "Shift + Del - removes current line\r\n" +
                                "Ctrl + B, Ctrl + Shift - B, Ctrl + N, Ctrl + Shift + N - add, removes and navigates to bookmark\r\n" +
                                "Esc - closes all opened tooltips, menus and hints\r\n" +
                                "Ctrl + Wheel - zooming\r\n" +
                                "Ctrl + M, Ctrl + E - start / stop macro recording, executing of macro\r\n" +
                                "Alt + F[char] - finds nearest[char]\r\n" +
                                "Ctrl + (Up, Down) -scrolls Up / Down\r\n" +
                                "Ctrl + (NumpadPlus, NumpadMinus, 0) -zoom in, zoom out, no zoom\r\n" +
                                "Ctrl + I - forced AutoIndentChars of current line" + 
                                "Ctrl + Space - force open auto completion" +
                                "Ctrl + Space on project view - open project view search";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
