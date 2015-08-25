using FarsiLibrary.Win;

namespace Personality_Creator
{
    class TabStripUtils
    {
        public static void flagTabAsModified(FATabStripItem tab)
        {
            if (!isTagFlaggedAsModified(tab))
            {
                tab.Title = tab.Title.Insert(0, "*");
            }
        }

        public static void unflagTabAsModified(FATabStripItem tab)
        {
            if (isTagFlaggedAsModified(tab))
            {
                tab.Title = tab.Title.Remove(0, 1);
            }
        }

        public static bool isTagFlaggedAsModified(FATabStripItem tab)
        {
            return tab.Title.StartsWith("*");
        }
    }
}
