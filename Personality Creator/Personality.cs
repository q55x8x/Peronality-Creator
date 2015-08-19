using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
                return name;
            }

            internal set
            {
                name = value;
            }
        }
        #endregion

        public Personality(DirectoryInfo rootPath) : base (rootPath)
        {
            this.Directory = rootPath;
            this.Name = rootPath.Name;
        }
        public Personality(string rootPath) : base (new DirectoryInfo(rootPath))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            this.Directory = dirInfo;
            this.Name = dirInfo.Name;
        }

        public void getRootNode()
        {

        }
    }
}
