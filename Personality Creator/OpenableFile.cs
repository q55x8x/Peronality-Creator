using FarsiLibrary.Win;
using System;
using System.IO;

namespace Personality_Creator
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public abstract class OpenableFile
    {
        private FileInfo file;
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

        public event ChangedEventHandler ContentChanged;
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
        
    }
}
