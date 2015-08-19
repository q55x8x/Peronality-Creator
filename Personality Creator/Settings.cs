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
        public string lastOpenedPersonaPath = @"C:\";

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
        }
    }
}
