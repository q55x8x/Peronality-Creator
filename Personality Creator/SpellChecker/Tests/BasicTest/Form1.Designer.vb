<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsiAbout = New System.Windows.Forms.ToolStripButton
        Me.tsiNewForm = New System.Windows.Forms.ToolStripButton
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.TextBox1.Location = New System.Drawing.Point(0, 25)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBox1.Size = New System.Drawing.Size(384, 239)
        Me.TextBox1.TabIndex = 0
        Me.TextBox1.Text = "This is an extremmly basic test of i00 .Net Spell Check in aaction!"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsiAbout, Me.tsiNewForm})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(384, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsiAbout
        '
        Me.tsiAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsiAbout.AutoToolTip = False
        Me.tsiAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsiAbout.Image = CType(resources.GetObject("tsiAbout.Image"), System.Drawing.Image)
        Me.tsiAbout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsiAbout.Name = "tsiAbout"
        Me.tsiAbout.Size = New System.Drawing.Size(44, 22)
        Me.tsiAbout.Text = "About"
        '
        'tsiNewForm
        '
        Me.tsiNewForm.AutoToolTip = False
        Me.tsiNewForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsiNewForm.Image = CType(resources.GetObject("tsiNewForm.Image"), System.Drawing.Image)
        Me.tsiNewForm.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsiNewForm.Name = "tsiNewForm"
        Me.tsiNewForm.Size = New System.Drawing.Size(66, 22)
        Me.tsiNewForm.Text = "New Form"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 264)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "i00 .Net Spell Check Basic Test"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsiAbout As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsiNewForm As System.Windows.Forms.ToolStripButton

End Class
