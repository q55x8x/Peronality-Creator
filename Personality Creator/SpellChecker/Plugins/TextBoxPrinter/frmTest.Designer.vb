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
        Me.tsbPrint = New System.Windows.Forms.ToolStripButton
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
        Me.PropertyGrid1.TabIndex = 3
        '
        'ToolStrip2
        '
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbPrint})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(624, 25)
        Me.ToolStrip2.TabIndex = 6
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'tsbPrint
        '
        Me.tsbPrint.AutoToolTip = False
        Me.tsbPrint.Image = Global.TextBoxPrinter.My.Resources.Resources.PrintHS
        Me.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPrint.Name = "tsbPrint"
        Me.tsbPrint.Size = New System.Drawing.Size(52, 22)
        Me.tsbPrint.Text = "Print"
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RichTextBox1.Location = New System.Drawing.Point(0, 25)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(366, 417)
        Me.RichTextBox1.TabIndex = 7
        Me.RichTextBox1.Text = ""
        '
        'frmTest
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 442)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.PropertyGrid1)
        Me.Controls.Add(Me.ToolStrip2)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "frmTest"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "i00 Text Box Printer"
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PropertyGrid1 As System.Windows.Forms.PropertyGrid
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbPrint As System.Windows.Forms.ToolStripButton
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox

End Class
