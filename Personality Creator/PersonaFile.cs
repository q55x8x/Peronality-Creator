using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator
{
    public class PersonaFile
    {
        #region capsuled fields
        private FileInfo file;
        private PersonaFileType fileType;
        private Folder parentFolder;
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

        public Folder ParentFolder
        {
            get
            {
                return parentFolder;
            }

            set
            {
                parentFolder = value;
            }
        }
        #endregion

        public PersonaFile(FileInfo file, Folder parentFolder, PersonaFileType type)
        {
            this.File = file;
            this.FileType = type;
            this.ParentFolder = parentFolder;
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
