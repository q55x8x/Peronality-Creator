using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator
{
    public class Personality
    {
        public string Name
        {
            get;
            private set;
        }

        public DirectoryInfo Path
        {
            get;
            set;
        }
        
        public Personality(DirectoryInfo rootPath)
        {
            this.Path = rootPath;
            this.Name = rootPath.Name;
        }
        public Personality(string rootPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(rootPath);
            this.Path = dirInfo;
            this.Name = dirInfo.Name;
        }
    }
}
