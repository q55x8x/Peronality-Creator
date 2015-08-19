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
        private DirectoryInfo path;
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

        public DirectoryInfo Path
        {
            get
            {
                return path;
            }

            internal set
            {
                path = value;
            }
        }
        #endregion

        public Personality(DirectoryInfo rootPath) : base (rootPath)
        {
            this.Path = rootPath;
            this.Name = rootPath.Name;
        }
        public Personality(string rootPath) : base (new DirectoryInfo(rootPath))
        {
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            this.Path = dirInfo;
            this.Name = dirInfo.Name;
        }
    }
}
