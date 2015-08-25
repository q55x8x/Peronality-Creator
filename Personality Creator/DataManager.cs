using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Personality_Creator.Tools;

namespace Personality_Creator
{
    public static class DataManager
    {
        public static ImageList iconList = new ImageList();
        public static Settings settings = new Settings();
        public static string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string settingsPath = AppPath + @"\settings.bin";


        public static void initDataManager()
        {
            iconList.Images.Add(Properties.Resources.folder);
            iconList.Images.Add(Properties.Resources.file);
            Settings.load();
        }
    }
}
