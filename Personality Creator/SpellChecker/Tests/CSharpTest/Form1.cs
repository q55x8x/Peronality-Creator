using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using i00SpellCheck;

namespace CSharpTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Enable control extensions
            //this will enable control extensions on ALL POSSIBLE CONTROLS ON THIS form AND ALL POSSIBLE CONTROLS ON ALL OWNED FORMS AS THEY OPEN automatically :)
            this.EnableControlExtensions();

            ////To load a single control extension on a control call: 
            //ControlExtensions.LoadSingleControlExtension(TextBox1, New TextBoxPrinter.TextBoxPrinter());

            ////To enable spell check on single line textboxes you will need to call:
            //TextBox1. 

            ////If you wanted to pass in options you can do so by handling the ControlExtensionAdding event PRIOR to calling EnableControlExtensions:
            //ControlExtensions.ControlExtensionAdding += ControlExtensionAdding;
            ////Also refer to the commented ControlExtensionAdding Sub in this form for more info
            
            ////You can also enable spell checking on an individual Control (if supported):
            //TextBox1.EnableSpellCheck(null);

            ////To disable the spell check on a Control:
            //TextBox1.DisableSpellCheck();

            ////To see if the spell check is enabled on a Control:
            //bool SpellCheckEnabled = TextBox1.IsSpellCheckEnabled();
            ////To see if another control extension is loaded (in this case call see if the TextBoxPrinter Extension is loaded on TextBox1):
            //var PrinterExtLoaded = TextBox1.ExtensionCast<TextBoxPrinter.TextBoxPrinter>() != null;

            ////To change options on an individual Control:
            //TextBox1.SpellCheck(true, null).Settings.AllowAdditions = true;
            //TextBox1.SpellCheck(true, null).Settings.AllowIgnore = true;
            //TextBox1.SpellCheck(true, null).Settings.AllowRemovals = true;
            //TextBox1.SpellCheck(true, null).Settings.ShowMistakes = true;
            ////etc

            ////To set control extension options / call methods from control extensions (in this case call Print() from TextBox1):
            //object PrinterExt = TextBox1.ExtensionCast<TextBoxPrinter.TextBoxPrinter>();
            //PrinterExt.Print();
            
            ////To show a spellcheck dialog for an individual Control:
            //var iSpellCheckDialog = TextBox1.SpellCheck(true,null) as i00SpellCheck.SpellCheckControlBase.iSpellCheckDialog;
            //if (iSpellCheckDialog != null) {
            //    iSpellCheckDialog.ShowDialog();
            //}

            ////To load a custom dictionary from a saved file:
            //i00SpellCheck.FlatFileDictionary Dictionary = new i00SpellCheck.FlatFileDictionary("c:\\Custom.dic", false);

            ////To create a new blank dictionary and save it as a file
            //i00SpellCheck.FlatFileDictionary Dictionary = new i00SpellCheck.FlatFileDictionary("c:\\Custom.dic", true);
            //Dictionary.Add("CustomWord1");
            //Dictionary.Add("CustomWord2");
            //Dictionary.Add("CustomWord3");
            //Dictionary.Save(Dictionary.Filename, true);

            ////To Load a custom dictionary for an individual Control:
            //TextBox1.SpellCheck(true, null).CurrentDictionary = Dictionary;

            ////To Open the dictionary editor for a dictionary associated with a Control:
            ////NOTE: this should only be done after the dictionary has loaded (Control.SpellCheck.CurrentDictionary.Loading = False)
            //TextBox1.SpellCheck(true, null).CurrentDictionary.ShowUIEditor();

            ////Repaint all of the controls that use the same dictionary...
            //TextBox1.SpellCheck(true, null).InvalidateAllControlsWithSameDict(true);


            //set the object for the property grid
            PropertyGrid1.SelectedObject = TextBox1.SpellCheck(true, null);

            //everything below here is for cosmetics...

            UpdateEnabledCheck();
 
            var ToolBoxIcon = new ToolboxBitmapAttribute(typeof(PropertyGrid));
            tsbProperties.Image = ToolBoxIcon.GetImage(typeof(PropertyGrid), false);

            TextBox1.SelectionStart = 0;
            TextBox1.SelectionLength = 0;

            var ico = Icon.ExtractAssociatedIcon("i00SpellCheck.exe");
            using (ico) {
                var b=new Bitmap(16,16);
                var g = Graphics.FromImage(b);
                using (g)
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    g.DrawIcon(ico,new Rectangle(0, 0, b.Width, b.Height));
                }
                tsbSpellCheck.Image = b;
            }

            this.Icon = Icon.ExtractAssociatedIcon("i00SpellCheck.exe");
        }



        ////This is used to setup spell check settings when the spell check extension is loaded:
        //static i00SpellCheck.SpellCheckSettings SpellCheckSettings = null;//Static for settings to be shared amongst all controls, use "i00SpellCheck.SpellCheckSettings SpellCheckSettings = null;" in the method below for control specific settings...
        //private void ControlExtensionAdding(object sender, i00SpellCheck.MiscControlExtension.ControlExtensionAddingEventArgs e)
        //{
        //    var SpellCheckControlBase = e.Extension as SpellCheckControlBase;
        //    if (SpellCheckControlBase != null)
        //    {
        //        //i00SpellCheck.SpellCheckSettings SpellCheckSettings = null;
        //        if (SpellCheckSettings == null)
        //        {
        //            SpellCheckSettings = new i00SpellCheck.SpellCheckSettings();
        //            SpellCheckSettings.AllowAdditions = true; //Specifies if you want to allow the user to add words to the dictionary
        //            SpellCheckSettings.AllowIgnore = true; //Specifies if you want to allow the user ignore words
        //            SpellCheckSettings.AllowRemovals = true; //Specifies if you want to allow users to delete words from the dictionary
        //            SpellCheckSettings.AllowInMenuDefs = true; //Specifies if the in menu definitions should be shown for correctly spelled words
        //            SpellCheckSettings.AllowChangeTo = true; //Specifies if "Change to..." (to change to a synonym) should be shown in the menu for correctly spelled words
        //        }
        //        SpellCheckControlBase.Settings = SpellCheckSettings;
        //    }
        //}


        //show and hide the property grid
        private void tsbProperties_Click(object sender, EventArgs e)
        {
            tsbProperties.Checked = ! tsbProperties.Checked;
            PropertyGrid1.Visible = tsbProperties.Checked;
        }

        #region "Enable / Disable Spell Check"

        private void UpdateEnabledCheck()
        {
	        var ts = (ToolStrip)tsiEnabled.Owner;
            System.Windows.Forms.VisualStyles.CheckBoxState state = TextBox1.IsSpellCheckEnabled() ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal : System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal;
	        var Size = System.Windows.Forms.CheckBoxRenderer.GetGlyphSize(this.CreateGraphics(), state);

	        int bWidth = 0;
	        int bHeight = 0;

	        bWidth = ts.ImageScalingSize.Width;
	        bHeight = ts.ImageScalingSize.Height;

	        Point Offset = new Point(0, 0);

	        if (Size.Width < ts.ImageScalingSize.Width) {
		        Offset.X = Convert.ToInt32(((ts.ImageScalingSize.Width - Size.Width) / 2));
	        } else {
		        bWidth = Size.Width;
	        }
	        if (Size.Height < ts.ImageScalingSize.Height) {
		        Offset.Y = Convert.ToInt32(((ts.ImageScalingSize.Height - Size.Height) / 2));
	        } else {
		        bHeight = Size.Height;
	        }


	        Bitmap b = new Bitmap(bWidth, bHeight);
            Graphics g = Graphics.FromImage(b); 
            using (g) {
		        g.TranslateTransform(Offset.X, Offset.Y);
		        System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(g, new Point(0, 0), state);
	        }
	        tsiEnabled.Image = b;
	        tsiEnabled.Visible = true;
        }

        private void tsiEnabled_Click(object sender, EventArgs e)
        {
            if (TextBox1.IsSpellCheckEnabled())
            {
                TextBox1.DisableSpellCheck();
            }
            else
            {
                TextBox1.EnableSpellCheck();
            }
            UpdateEnabledCheck();
        }

        #endregion

        //show the spellcheck dialog
        private void tsbSpellCheck_Click(object sender, EventArgs e)
        {
            var iSpellCheckDialog = TextBox1.SpellCheck(false,null) as i00SpellCheck.SpellCheckControlBase.iSpellCheckDialog;
            if (iSpellCheckDialog != null)
            {
                iSpellCheckDialog.ShowDialog();
            }
        }

    }
}
