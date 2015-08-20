using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Personality_Creator.PersonaFiles;
using Personality_Creator.PersonaFiles.Scripts;

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
                PersonaFile newFile = PersonaFile.CreateInstance(file);

                this.Files.Add(file.Name, newFile);
            }
        }

        public Folder(string path) : this(new DirectoryInfo(path))
        { }

        public List<PersonaFile> GetAllFilesAndFilesInSubDirs()
        {
            List<PersonaFile> files = new List<PersonaFile>();
            files.AddRange(this.Files.Values);
            foreach(Folder subDir in this.Folders.Values)
            {
                files.AddRange(subDir.GetAllFilesAndFilesInSubDirs());
            }
            return files;
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
