using System.Collections.Generic;

namespace Personality_Creator
{
    internal class Settings
    {
        public List<string> openedPersonas
        {
            get;
        }
        public List<string> openedTabs
        {
            get;
        }
        public string lastDir {
            get;
            set;
        }

        public Settings()
        {
            this.openedPersonas = new List<string>(Properties.Settings.Default.openedPersonas.Split(','));
            this.openedTabs = new List<string>(Properties.Settings.Default.openedFiles.Split(','));

            this.lastDir = Properties.Settings.Default.lastDir;
        }

        public void save()
        {
            Properties.Settings.Default.openedPersonas = string.Join(",", this.openedPersonas.ToArray());
            Properties.Settings.Default.openedFiles = string.Join(",", this.openedTabs.ToArray());
            Properties.Settings.Default.lastDir = this.lastDir;
            Properties.Settings.Default.Save();
        }
    }
}