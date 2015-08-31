using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Personality_Creator
{   
    [Serializable]
    public class Settings
    {
        public string lastOpenedPersonaDirectory = @"C:\";
        public string lastOpenedSingleFileDirectory = @"C:\";
        public List<string> openedPersonas = new List<string>();
        public List<string> openedTabs = new List<string>();
        public List<string> last10OpenedScripts = new List<string>();
        public List<string> last10OpenedPersonas = new List<string>();

        public static void save()
        {
            Tools.BinarySerializer.Serialize<Settings>(DataManager.settings, DataManager.settingsPath);
        }

        public static void load()
        {
            if (!File.Exists(DataManager.settingsPath))
            {
                Tools.BinarySerializer.Serialize<Settings>(new Settings(), DataManager.settingsPath);
            }
            DataManager.settings = Tools.BinarySerializer.Deserialize<Settings>(DataManager.settingsPath);

            DataManager.settings.openedTabs = initListIfNull(DataManager.settings.openedTabs);
            DataManager.settings.openedPersonas = initListIfNull(DataManager.settings.openedPersonas);
            DataManager.settings.last10OpenedScripts = initListIfNull(DataManager.settings.last10OpenedScripts);
            DataManager.settings.last10OpenedPersonas = initListIfNull(DataManager.settings.last10OpenedPersonas);
        }

        private static List<string> initListIfNull(List<string> list)
        {
            if(list == null)
            {
                return new List<string>();
            }
            return list;
        }
    }
}
