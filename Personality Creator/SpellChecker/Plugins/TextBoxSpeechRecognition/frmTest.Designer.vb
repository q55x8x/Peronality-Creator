<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTest
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
        Me.PropertyGrid1 = New System.Windows.Forms.PropertyGrid
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.tsbDictate = New System.Windows.Forms.ToolStripButton
        Me.tsbSpeak = New System.Windows.Forms.ToolStripButton
        Me.tsbStopSpeaking = New System.Windows.Forms.ToolStripButton
        Me.webKaraoke = New System.Windows.Forms.WebBrowser
        Me.pbSpeechProgress = New System.Windows.Forms.ProgressBar
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox
        Me.ToolStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PropertyGrid1
        '
        Me.PropertyGrid1.Dock = System.Windows.Forms.DockStyle.Right
        Me.PropertyGrid1.Location = New System.Drawing.Point(366, 25)
        Me.PropertyGrid1.Name = "PropertyGrid1"
        Me.PropertyGrid1.Size = New System.Drawing.Size(258, 417)
        Me.PropertyGrid1.TabIndex = 1
        '
        'ToolStrip2
        '
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbDictate, Me.tsbSpeak, Me.tsbStopSpeaking})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(624, 25)
        Me.ToolStrip2.TabIndex = 7
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'tsbDictate
        '
        Me.tsbDictate.AutoToolTip = False
        Me.tsbDictate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDictate.Name = "tsbDictate"
        Me.tsbDictate.Size = New System.Drawing.Size(48, 22)
        Me.tsbDictate.Text = "Dictate"
        '
        'tsbSpeak
        '
        Me.tsbSpeak.AutoToolTip = False
        Me.tsbSpeak.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSpeak.Name = "tsbSpeak"
        Me.tsbSpeak.Size = New System.Drawing.Size(42, 22)
        Me.tsbSpeak.Text = "Speak"
        '
        'tsbStopSpeaking
        '
        Me.tsbStopSpeaking.AutoToolTip = False
        Me.tsbStopSpeaking.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbStopSpeaking.Name = "tsbStopSpeaking"
        Me.tsbStopSpeaking.Size = New System.Drawing.Size(86, 22)
        Me.tsbStopSpeaking.Text = "Stop Speaking"
        Me.tsbStopSpeaking.Visible = False
        '
        'webKaraoke
        '
        Me.webKaraoke.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.webKaraoke.Location = New System.Drawing.Point(0, 378)
        Me.webKaraoke.MinimumSize = New System.Drawing.Size(20, 20)
        Me.webKaraoke.Name = "webKaraoke"
        Me.webKaraoke.ScrollBarsEnabled = False
        Me.webKaraoke.Size = New System.Drawing.Size(366, 64)
        Me.webKaraoke.TabIndex = 8
        Me.webKaraoke.Visible = False
        '
        'pbSpeechProgress
        '
        Me.pbSpeechProgress.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pbSpeechProgress.Location = New System.Drawing.Point(0, 370)
        Me.pbSpeechProgress.Name = "pbSpeechProgress"
        Me.pbSpeechProgress.Size = New System.Drawing.Size(366, 8)
        Me.pbSpeechProgress.TabIndex = 9
        Me.pbSpeechProgress.Visible = False
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox1.Location = New System.Drawing.Point(0, 25)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(366, 345)
        Me.RichTextBox1.TabIndex = 10
        Me.RichTextBox1.Text = ""
        '
        'frmTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 442)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.pbSpeechProgress)
        Me.Controls.Add(Me.webKaraoke)
        Me.Controls.Add(Me.PropertyGrid1)
        Me.Controls.Add(Me.ToolStrip2)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "frmTest"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "i00 TextBox Speech Recognition"
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PropertyGrid1 As System.Windows.Forms.PropertyGrid
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbDictate As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSpeak As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbStopSpeaking As System.Windows.Forms.ToolStripButton
    Friend WithEvents webKaraoke As System.Windows.Forms.WebBrowser
    Friend WithEvents pbSpeechProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox

End Class
