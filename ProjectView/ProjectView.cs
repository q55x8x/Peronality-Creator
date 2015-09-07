using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Personality_Creator.UI
{
    public partial class ProjectView : System.Windows.Forms.TreeView
    {
        ConcurrentBag<TreeNode> unfilteredTrees;

        public ProjectView()
        {
            InitializeComponent();
            this.txtSearch.Visible = false;
            this.PreviewKeyDown += this.previewKeyDown;
            this.txtSearch.TextChanged += this.txtSearch_TextChanged;
        }

        private void previewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Space && ModifierKeys == Keys.Control) //sadly the first key pressed gets suppressed and is not recognize by the textbox so the easy solution is to handle the popup search is by activating it through a simple key press
            {
                this.unfilteredTrees = new ConcurrentBag<TreeNode>();

                //Parallel.ForEach<TreeNode>(Nodes.Cast<TreeNode>(), element => this.unfilteredTrees.Add(element));

                foreach(TreeNode node in this.Nodes)
                {
                    unfilteredTrees.Add(node);
                }

                this.txtSearch.Visible = true;
                this.txtSearch.Focus();

            }
        }
        
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(this.txtSearch.Text == "")
            {
                this.txtSearch.Visible = false;

                this.Nodes.Clear();

                this.Nodes.AddRange(this.unfilteredTrees.ToArray());
            }
            else
            {
                this.SuspendLayout();
                this.Nodes.Clear();

                foreach (TreeNode node in this.unfilteredTrees)
                {
                    searchNode(node);
                }

                this.ResumeLayout();
            }
        }

        private void searchNode(TreeNode node)
        {
            if (node.Text.Contains(this.txtSearch.Text))
            {
                this.Nodes.Add(node);
            }
            else if (node.Nodes.Count > 0)
            {
                foreach(TreeNode childNode in node.Nodes)
                {
                    searchNode(childNode);
                }
            }
        }
    }
}
