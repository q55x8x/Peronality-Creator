using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Personality_Creator
{
    public class DataManager //dunno if the singleton pattern will come to use later or if I downgrade to static
    {
        public ImageList iconList = new ImageList();

        private DataManager instance;

        private DataManager()
        {
            this.iconList.Images.Add(Personality_Creator.Properties.Resources.folder);
            this.iconList.Images.Add(Personality_Creator.Properties.Resources.file);
        }

        public DataManager Instance
        {
            get
            {
                if(this.instance == null)
                {
                    this.instance = new DataManager();
                }
                return instance;
            }
        }
    }
}
