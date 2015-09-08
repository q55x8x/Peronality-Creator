using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Personality_Creator
{
    public class Personality : Folder
    {
        #region capsuled fields
        private string name;
        #endregion

        #region properties
        public string Name
        {
            get
            {
                return this.Directory.Name;
            }

            internal set
            {
                name = value;
            }
        }
        #endregion

        public Personality(DirectoryInfo rootPath) : base(rootPath)
        {
            this.Directory = rootPath;
            this.Persona = this;
        }

        public Personality(string rootPath) : this(new DirectoryInfo(rootPath))
        { }

        public TreeNode getRootNode()
        {
            TreeNode node = getNode(this);
            node.Tag = this;
            node.Name = this.Name;

            return node;
        }
    }
}
