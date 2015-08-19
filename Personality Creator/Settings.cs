using System.Collections.Generic;

namespace Personality_Creator
{
    internal class Settings
    {
        public List<string> openedPersonas
        {
            get;
        }
        public string lastDir {
            get;
            set;
        }

        public Settings()
        {
            string openedPersonas = Properties.Settings.Default.openedPersonas;
            this.openedPersonas = new List<string>(openedPersonas.Split(','));

            this.lastDir = Properties.Settings.Default.lastDir;
        }

        public void save()
        {
            Properties.Settings.Default.openedPersonas = string.Join(",", this.openedPersonas.ToArray());
            Properties.Settings.Default.lastDir = this.lastDir;
            Properties.Settings.Default.Save();
        }
    }
}