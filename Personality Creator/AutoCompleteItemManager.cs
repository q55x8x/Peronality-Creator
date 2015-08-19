using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FastColoredTextBoxNS;

namespace Personality_Creator
{
    static class AutoCompleteItemManager
    {
        public static List<string> Items = new List<string>();
        public static void load()
        {
            Items.AddRange(File.ReadAllLines(DataManager.AppPath + @"\data\commands.txt"));

            Items.AddRange(File.ReadAllLines(DataManager.AppPath + @"\data\keywords.txt"));

            Items.Add("testItem");
        }
    }
}
