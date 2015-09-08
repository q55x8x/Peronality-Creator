using FarsiLibrary.Win;
using System;
using System.IO;
using GlobalSearch;
using System.Collections.Generic;

namespace Personality_Creator
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public abstract class OpenableFile : ISearchable
    {
        private FileInfo file;
        private Personality persona;
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

        public Personality Persona
        {
            get
            {
                return persona;
            }

            set
            {
                persona = value;
            }
        }

        #region events

        public delegate void FileReferencClickedEventHandler(object sender, FileReferenceClickedEventArgs e);
        public abstract event FileReferencClickedEventHandler FileReferenceClicked;

        public class FileReferenceClickedEventArgs : EventArgs //Up to here this is only for Vocab files but we may have to change these event args in the future
        {
            public string VocabFileName = "";
        }

        public event ChangedEventHandler ContentChanged;

        #endregion


        public FATabStripItem tab;

        public abstract FATabStripItem CreateTab();
        public abstract void Save();
        public abstract void Redraw();

        protected virtual void OnContentChanged(EventArgs e)
        {
            ChangedEventHandler handler = ContentChanged;
            if (handler != null)
            {
                handler(this.tab, e);
            }
        }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            OpenableFile p = obj as OpenableFile;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Equals(p);
        }

        public bool Equals(OpenableFile file)
        {
            // If parameter is null return false:
            if ((object)file == null)
            {
                return false;
            }

            // Return true if the fields match:
            return File != null && file.File != null && File.FullName == file.File.FullName;
        }

        public override int GetHashCode()
        {
            return file.GetHashCode();
        }
        
        public abstract List<SearchResult> Search(string input, SearchCriteria criteria);

        public abstract string GetSearchContent();
    }
}
