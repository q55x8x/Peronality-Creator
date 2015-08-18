using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Personality_Creator
{
    public class NewFolderDialog
    {
        public string Title
        { get; set; }

        public string Prompt
        { get; set; }

        public string NewFolderName
        {
            get;
            private set;
        }

        Form form = new Form();
        Label lblPrompt = new Label();
        TextBox txtInput = new TextBox();
        Button btnOK = new Button();
        Button btnCancel = new Button();

        public NewFolderDialog(string title, string prompt)
        {
            this.Title = title;
            this.Prompt = prompt;

            form.Text = Title;
            lblPrompt.Text = Prompt;

            txtInput.Text = "";
            txtInput.Width = 1000;
            txtInput.TextChanged += TxtInput_TextChanged;

            btnOK.Text = "OK";
            btnCancel.Text = "Cancel";
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            lblPrompt.SetBounds(9, 20, 372, 13);
            txtInput.SetBounds(12, 36, 372, 20);
            btnOK.SetBounds(228, 72, 75, 23);
            btnCancel.SetBounds(309, 72, 75, 23);

            lblPrompt.AutoSize = true;
            txtInput.Anchor = txtInput.Anchor | AnchorStyles.Right;
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { lblPrompt, txtInput, btnOK, btnCancel });
            form.ClientSize = new Size(Math.Max(300, lblPrompt.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = btnOK;
            form.CancelButton = btnCancel;
        }

        public NewFolderDialog()
        {
            this.Title = "New Folder";
            this.Prompt = "Input name for new folder";

            form.Text = Title;
            lblPrompt.Text = Prompt;

            txtInput.Text = "";
            txtInput.Width = 1000;
            txtInput.TextChanged += TxtInput_TextChanged;

            btnOK.Text = "OK";
            btnCancel.Text = "Cancel";
            btnOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            lblPrompt.SetBounds(9, 20, 372, 13);
            txtInput.SetBounds(12, 36, 372, 20);
            btnOK.SetBounds(228, 72, 75, 23);
            btnCancel.SetBounds(309, 72, 75, 23);

            lblPrompt.AutoSize = true;
            txtInput.Anchor = txtInput.Anchor | AnchorStyles.Right;
            btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { lblPrompt, txtInput, btnOK, btnCancel });
            form.ClientSize = new Size(Math.Max(300, lblPrompt.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = btnOK;
            form.CancelButton = btnCancel;
        }

        private void TxtInput_TextChanged(object sender, EventArgs e)
        {
            this.NewFolderName = this.txtInput.Text;
        }

        public DialogResult Show()
        {
            return form.ShowDialog();
        }
    }
}
