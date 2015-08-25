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
    public abstract class PersonaFile : OpenableFile
    {
        #region capsuled fields
        
        #endregion

        #region properties
        
        #endregion

        internal PersonaFile(FileInfo file)
        {
            this.File = file;;
        }

        internal PersonaFile(string path) : this(new FileInfo(path))
        { }

        public static PersonaFile CreateInstance(FileInfo file)
        {
            if(file.Extension == ".txt")
            {
                return new Module(file);
            }

            return new Script(file);
        }

        public static PersonaFile CreateInstance(string path)
        {
            return PersonaFile.CreateInstance(new FileInfo(path));
        }
    }
}
