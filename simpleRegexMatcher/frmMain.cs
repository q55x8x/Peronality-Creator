using System;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static System.Text.RegularExpressions.Regex;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;

namespace simpleRegexMatcher
{
    public partial class frmMain : Form
    {
        string inputText = "";
        string outputText = "";

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnMatch_Click(object sender, EventArgs e)
        {
            bool first = true;

            this.inputText = File.ReadAllText(this.txtInputPath.Text);
            this.outputText = "";

            List<string> matches = new List<string>();

            foreach (Match match in Matches(this.inputText, this.txtPattern.Text))
            {
                if(!matches.Contains(match.Value))
                {
                    matches.Add(match.Value);
                    this.outputText += first ? match.Value : Environment.NewLine + match.Value;
                    first = false; //compiler will take care of optimizing this
                }
            }

            if(File.Exists(this.txtOutputPath.Text))
            {
                File.Delete(this.txtOutputPath.Text);
            }

            File.Create(this.txtOutputPath.Text).Close();

            File.WriteAllText(this.txtOutputPath.Text, this.outputText);

        }

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txtInputPath.Text = ofd.FileName;
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                this.txtOutputPath.Text = sfd.FileName;
            }
        }
    }
}
