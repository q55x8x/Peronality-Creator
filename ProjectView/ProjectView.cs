﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personality_Creator.UI
{
    public partial class ProjectView : System.Windows.Forms.TreeView
    {
        TreeNode[] unfilteredTrees;

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
                this.unfilteredTrees = new TreeNode[this.Nodes.Count];

                this.Nodes.CopyTo(this.unfilteredTrees, 0);

                this.txtSearch.Visible = true;
                this.txtSearch.Focus();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if(this.txtSearch.Text == "")
            {
                this.txtSearch.Visible = false;
            }
            else
            {

            }
        }

        private void filterTreeView()
        {
        }
    }
}