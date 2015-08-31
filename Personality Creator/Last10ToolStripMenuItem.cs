using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personality_Creator
{
    public delegate void EntryClickedEventHandler(object sender, EventArgs e);

    class Last10ToolStripMenuItem : ToolStripMenuItem
    {
        public event EntryClickedEventHandler EntryClicked;

        public override bool Enabled
        {
            get
            {
                return DropDownItems.Count > 0;
            }
        }

        public List<string> Entries
        {
            get
            {
                List<string> entries = new List<string>();
                foreach(ToolStripMenuItem item in DropDownItems)
                {
                    entries.Add(item.Text);
                }
                return entries;
            }

            set
            {
                foreach(string entry in value)
                {
                    addEntry(entry);
                }
            }
        }

        public void addEntry(string entry)
        {
            if (alreadyHasEntry(entry))
            {
                moveEntryForward(entry);
            } else
            {

                if (DropDownItems.Count == 10)
                {
                    DropDownItems.RemoveAt(0);
                }

                ToolStripMenuItem menuEntry = new ToolStripMenuItem(entry);
                menuEntry.Click += (object sender, EventArgs e) =>
                {
                    OnEntryClicked((ToolStripMenuItem)sender, EventArgs.Empty);
                };

                DropDownItems.Add(menuEntry);
            }
        }

        protected void OnEntryClicked(ToolStripMenuItem entry , EventArgs e)
        {
            EntryClickedEventHandler handler = EntryClicked;
            if (handler != null)
            {
                handler(entry, e);
            }
        }

        protected bool alreadyHasEntry(string entry)
        {
            foreach(ToolStripMenuItem item in DropDownItems) {
                if(item.Text == entry)
                {
                    return true;
                }
            }
            return false;
        }

        protected void moveEntryForward(string entry)
        {
            ToolStripMenuItem entryItem = null;
            for (int i = 0; i < DropDownItems.Count && entryItem == null; i++)
            {
                if (DropDownItems[i].Text == entry)
                {
                    entryItem = (ToolStripMenuItem)DropDownItems[i];
                }
            }
            if(entryItem != null)
            {
                DropDownItems.Remove(entryItem);
                DropDownItems.Add(entryItem);
            }
        }
    }
}
