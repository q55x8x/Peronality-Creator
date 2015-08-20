using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Personality_Creator.PersonaFiles;
using Personality_Creator.PersonaFiles.Scripts;

namespace Personality_Creator
{
    public class PersonaFile
    {
        #region capsuled fields
        private FileInfo file;
        #endregion

        #region properties
        public FileInfo File
        {
            get
            {
                return file;
            }

            set
            {
                file = value;
            }
        }
        #endregion

        internal PersonaFile(FileInfo file)
        {
            this.File = file;;
        }

        internal PersonaFile(string path) : this(new FileInfo(path))
        { }

        public override string ToString()
        {
            return File.Name;
        }

        public static PersonaFile CreateInstance(FileInfo file)
        {
            if(file.Extension == ".txt")
            {
                return new Module(file);
            }

            return new PersonaFile(file);
        }

        public static PersonaFile CreateInstance(string path)
        {
            return PersonaFile.CreateInstance(new FileInfo(path));
        }
    }
}
