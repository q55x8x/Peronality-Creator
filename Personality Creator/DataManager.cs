using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Personality_Creator.Tools;
using System.Drawing;
using System.ComponentModel;

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

    [Serializable]
    public class StyleData
    {
        private Color foreColor;
        private Color backColor;

        private FontStyle fontStyle;

        [Browsable(false)]
        public SolidBrush ForeBrush
        {
            get
            {
                return new SolidBrush(ForeColor);
            }

            set
            {
                ForeColor = value.Color;
            }
        }

        [Browsable(false)]
        public SolidBrush BackgroundBrush
        {
            get
            {
                return new SolidBrush(BackColor);
            }

            set
            {
                BackColor = value.Color;
            }
        }

        [Category("Text")]
        [Description("Set the font style")]
        [TypeConverter(typeof(FontStyle))]
        public FontStyle FontStyle
        {
            get
            {
                return fontStyle;
            }

            set
            {
                fontStyle = value;
            }
        }

        [Category("Color")]
        [Description("Set the foreground color")]
        [DisplayName("Foreground color")]
        public Color ForeColor
        {
            get
            {
                return foreColor;
            }

            set
            {
                foreColor = value;
            }
        }

        [Category("Color")]
        [Description("Set the background color")]
        [DisplayName("Background color")]
        public Color BackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                backColor = value;
            }
        }

        public StyleData(Color foreColor, Color backColor, FontStyle fontStyle)
        {
            this.ForeColor = foreColor;
            this.BackColor = backColor;
            this.fontStyle = fontStyle;
        }
    }
}
