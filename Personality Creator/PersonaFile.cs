using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator
{
    public class PersonaFile : PersonaItem
    {
        #region capsuled fields
        private FileInfo file;
        private PersonaFileType fileType;
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

        public PersonaFileType FileType
        {
            get
            {
                return fileType;
            }

            set
            {
                fileType = value;
            }
        }
        #endregion

        public PersonaFile(FileInfo file, PersonaFileType type)
        {
            this.File = file;
            this.FileType = type;
        }

        public override string ToString()
        {
            return File.Name;
        }
    }

    public enum PersonaFileType
    {
        Script,
        FragmentedScript,
        Fragment,
        Media,
        Flag,
        App,
        Plalist,
        Vocabulary,
        Other
    }
}
