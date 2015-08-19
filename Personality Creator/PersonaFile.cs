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
        private FileInfo info;
        private PersonaFileType fileType;
        #endregion

        #region properties
        public FileInfo File
        {
            get
            {
                return info;
            }

            set
            {
                info = value;
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
        Media,
        Flag
    }
}
