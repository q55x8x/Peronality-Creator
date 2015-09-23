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

            Items.AddRange(File.ReadAllLines(DataManager.AppPath + @"\data\commandfilters.txt"));

            Items.AddRange(File.ReadAllLines(DataManager.AppPath + @"\data\keywords.txt"));

            Items.Add("testItem");
        }

        public static List<string> getItemsWithVocabFiles(Personality persona)
        {
            List<string> items = new List<string>(); 

            string[] itemsCopy = new string[Items.Count]; //Have to copy all items to not change "Items"
            Items.CopyTo(itemsCopy);

            items.AddRange(itemsCopy);

            try
            {
                foreach (FileInfo file in new DirectoryInfo(persona.Directory.FullName + @"\Vocabulary\").GetFiles())
                {
                    items.Add(file.Name.Replace("#", "").Replace(".txt", ""));
                }
            }
            catch (DirectoryNotFoundException dirNotFoundEx)
            { }

            return items;
        }
    }
}
