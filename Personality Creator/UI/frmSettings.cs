using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Personality_Creator;

namespace Personality_Creator.UI
{
    public partial class frmSettings : Form
    {

        public delegate void styleChangedEventHandler(object sender);
        public event styleChangedEventHandler styleChanged;

        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            this.propertyGridVocabStyle.SelectedObject = DataManager.settings.VocabStyle;
            this.propertyGridCommandStyle.SelectedObjects = new object[3] { DataManager.settings.CommandStyle, DataManager.settings.GotoStyle, DataManager.settings.CheckFlagStyle };
            this.propertyGridResponseStyle.SelectedObject = DataManager.settings.ResponseStyle;
            this.propertyGridParanthesisStyle.SelectedObject = DataManager.settings.ParanthesisStyle;
        }

        private void propertyGridVocabStyle_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            styleChanged(this.propertyGridVocabStyle);
        }

        private void propertyGridCommandStyle_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            styleChanged(this.propertyGridCommandStyle);
        }

        private void propertyGridResponseStyle_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            styleChanged(this.propertyGridResponseStyle);
        }

        private void propertyGridParanthesisStyle_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            styleChanged(this.propertyGridParanthesisStyle);
        }
    }
}
