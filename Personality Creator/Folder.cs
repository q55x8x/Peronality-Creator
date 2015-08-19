using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator
{
    public class Folder : PersonaItem
    {
        #region capsuled fields
        private Dictionary<string, Folder> folders;
        private DirectoryInfo directory;
        #endregion

        #region properties
        public Dictionary<string, Folder> Folders
        {
            get
            {
                return folders;
            }

            set
            {
                folders = value;
            }
        }

        public DirectoryInfo Directory
        {
            get
            {
                return directory;
            }

            set
            {
                directory = value;
            }
        }
        #endregion

        public Folder(DirectoryInfo dir)
        {
            this.Directory = dir;
        }

        public override string ToString()
        {
            return Directory.Name;
        }
    }
}
