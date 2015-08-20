using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Personality_Creator
{
    public class Folder
    {
        #region capsuled fields
        private Dictionary<string, Folder> folders = new Dictionary<string, Folder>();
        private Dictionary<string, PersonaFile> files = new Dictionary<string, PersonaFile>();
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
                    PersonaFile newFile = new PersonaFile(file, this, PersonaFileType.Fragment);
                    this.Files.Add(file.Name, newFile);
                    continue;
                }

                if(file.Extension == ".frag")
                {
                    PersonaFile newFIle = new PersonaFile(file, this, PersonaFileType.FragmentedScript);
                    this.Files.Add(file.Name, newFIle);
                    continue;
                }
                
                if(file.Extension == ".txt")
                {
                    Script newFile = new Script(file, this);
                    this.Files.Add(file.Name, newFile);
                    continue;
                }

                //default case
                PersonaFile defaultFile = new PersonaFile(file, this, PersonaFileType.Other);
                this.Files.Add(file.Name, defaultFile);
            }
        }

        public override string ToString()
        {
            return Directory.Name;
        }

        public static TreeNode getNode(Folder folder)
        {
            List<TreeNode> children = new List<TreeNode>();

            foreach(string subFolder in folder.Folders.Keys)
            {
                children.Add(getNode(folder.Folders[subFolder]));
            }
            foreach(string file in folder.Files.Keys)
            {
                TreeNode[] empty = new TreeNode[0];
                TreeNode child = new TreeNode(folder.Files[file].File.Name, 1, 1, empty);
                child.Tag = folder.Files[file];
                children.Add(child);
            }
            TreeNode node = new TreeNode(folder.Directory.Name, 0, 0, children.ToArray());
            node.Tag = folder;

            return node;
        }
    }
}
