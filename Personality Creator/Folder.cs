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
        private Dictionary<string, PersonaFile> files;
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

        public Dictionary<string, PersonaFile> Files
        {
            get
            {
                return files;
            }

            set
            {
                files = value;
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

            foreach(DirectoryInfo subDir in this.Directory.GetDirectories())
            {
                this.Folders.Add(subDir.Name, new Folder(subDir));
            }

            foreach(FileInfo file in this.Directory.GetFiles())
            {
                if (this.Directory.Name == "Fragments")
                {
                    PersonaFile newFile = new PersonaFile(file, PersonaFileType.Fragment);
                    this.Files.Add(file.Name, newFile);
                    continue;
                }

                if(file.Extension == ".frag")
                {
                    PersonaFile newFIle = new PersonaFile(file, PersonaFileType.FragmentedScript);
                    this.Files.Add(file.Name, newFIle);
                    continue;
                }
                
                if(file.Extension == ".txt")
                {
                    PersonaFile newFile = new PersonaFile(file, PersonaFileType.Script);
                    this.Files.Add(file.Name, newFile);
                    continue;
                }

                //default case
                PersonaFile defaultFile = new PersonaFile(file, PersonaFileType.Other);
                this.Files.Add(file.Name, defaultFile);
            }
        }

        public override string ToString()
        {
            return Directory.Name;
        }
    }
}
