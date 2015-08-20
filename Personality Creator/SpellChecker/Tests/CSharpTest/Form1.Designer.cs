namespace CSharpTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.ToolStrip3 = new System.Windows.Forms.ToolStrip();
            this.tsbSpellCheck = new System.Windows.Forms.ToolStripButton();
            this.tsbProperties = new System.Windows.Forms.ToolStripButton();
            this.tsiEnabled = new System.Windows.Forms.ToolStripButton();
            this.PropertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.ToolStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBox1
            // 
            this.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.TextBox1.Location = new System.Drawing.Point(0, 25);
            this.TextBox1.Multiline = true;
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox1.Size = new System.Drawing.Size(374, 417);
            this.TextBox1.TabIndex = 0;
            this.TextBox1.Text = "This test demonstrates the use of i00 Spell Check in C#\r\n\r\nThe quic brown fox jun" +
                "ped ovr the lazy dog!";
            // 
            // ToolStrip3
            // 
            this.ToolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSpellCheck,
            this.tsbProperties,
            this.tsiEnabled});
            this.ToolStrip3.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip3.Name = "ToolStrip3";
            this.ToolStrip3.Size = new System.Drawing.Size(624, 25);
            this.ToolStrip3.TabIndex = 8;
            this.ToolStrip3.Text = "ToolStrip3";
            // 
            // tsbSpellCheck
            // 
            this.tsbSpellCheck.AutoToolTip = false;
            this.tsbSpellCheck.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSpellCheck.Name = "tsbSpellCheck";
            this.tsbSpellCheck.Size = new System.Drawing.Size(72, 22);
            this.tsbSpellCheck.Text = "Spell Check";
            this.tsbSpellCheck.Click += new System.EventHandler(this.tsbSpellCheck_Click);
            // 
            // tsbProperties
            // 
            this.tsbProperties.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbProperties.AutoToolTip = false;
            this.tsbProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbProperties.Name = "tsbProperties";
            this.tsbProperties.Size = new System.Drawing.Size(64, 22);
            this.tsbProperties.Text = "Properties";
            this.tsbProperties.Click += new System.EventHandler(this.tsbProperties_Click);
            // 
            // tsiEnabled
            // 
            this.tsiEnabled.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsiEnabled.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiEnabled.Name = "tsiEnabled";
            this.tsiEnabled.Size = new System.Drawing.Size(53, 22);
            this.tsiEnabled.Text = "Enabled";
            this.tsiEnabled.Click += new System.EventHandler(this.tsiEnabled_Click);
            // 
            // PropertyGrid1
            // 
            this.PropertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.PropertyGrid1.Location = new System.Drawing.Point(374, 25);
            this.PropertyGrid1.Name = "PropertyGrid1";
            this.PropertyGrid1.Size = new System.Drawing.Size(250, 417);
            this.PropertyGrid1.TabIndex = 9;
            this.PropertyGrid1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 442);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.PropertyGrid1);
            this.Controls.Add(this.ToolStrip3);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "i00 .Net Spell Check C# Test";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ToolStrip3.ResumeLayout(false);
            this.ToolStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBox1;
        internal System.Windows.Forms.ToolStrip ToolStrip3;
        internal System.Windows.Forms.ToolStripButton tsbSpellCheck;
        internal System.Windows.Forms.ToolStripButton tsbProperties;
        internal System.Windows.Forms.ToolStripButton tsiEnabled;
        private System.Windows.Forms.PropertyGrid PropertyGrid1;
    }
}

