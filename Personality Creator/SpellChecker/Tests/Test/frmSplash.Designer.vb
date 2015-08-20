<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSplash
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSplash))
        Me.btnClose = New System.Windows.Forms.Button
        Me.bpBackGround = New Test.BufferedPanel
        Me.bpDocumentHolder = New Test.BufferedPanel
        Me.AutoGrowLabel4 = New Test.AutoGrowLabel
        Me.AutoGrowLabel3 = New Test.AutoGrowLabel
        Me.ProductLink = New Test.AutoGrowLabel
        Me.AutoGrowLabel1 = New Test.AutoGrowLabel
        Me.bpCommandBar = New Test.BufferedPanel
        Me.chkAccept = New System.Windows.Forms.CheckBox
        Me.cmsLinks = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.VBForumsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CodeProjectToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.bpBackGround.SuspendLayout()
        Me.bpDocumentHolder.SuspendLayout()
        Me.bpCommandBar.SuspendLayout()
        Me.cmsLinks.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(306, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Exit"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'bpBackGround
        '
        Me.bpBackGround.Controls.Add(Me.bpDocumentHolder)
        Me.bpBackGround.Controls.Add(Me.bpCommandBar)
        Me.bpBackGround.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpBackGround.Location = New System.Drawing.Point(0, 0)
        Me.bpBackGround.Name = "bpBackGround"
        Me.bpBackGround.Size = New System.Drawing.Size(384, 262)
        Me.bpBackGround.TabIndex = 5
        '
        'bpDocumentHolder
        '
        Me.bpDocumentHolder.AutoScroll = True
        Me.bpDocumentHolder.Controls.Add(Me.AutoGrowLabel4)
        Me.bpDocumentHolder.Controls.Add(Me.AutoGrowLabel3)
        Me.bpDocumentHolder.Controls.Add(Me.ProductLink)
        Me.bpDocumentHolder.Controls.Add(Me.AutoGrowLabel1)
        Me.bpDocumentHolder.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpDocumentHolder.Location = New System.Drawing.Point(0, 0)
        Me.bpDocumentHolder.Name = "bpDocumentHolder"
        Me.bpDocumentHolder.Size = New System.Drawing.Size(384, 233)
        Me.bpDocumentHolder.TabIndex = 5
        '
        'AutoGrowLabel4
        '
        Me.AutoGrowLabel4.BackColor = System.Drawing.Color.Transparent
        Me.AutoGrowLabel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.AutoGrowLabel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.AutoGrowLabel4.ForeColor = System.Drawing.Color.Black
        Me.AutoGrowLabel4.Location = New System.Drawing.Point(0, 280)
        Me.AutoGrowLabel4.Name = "AutoGrowLabel4"
        Me.AutoGrowLabel4.Size = New System.Drawing.Size(367, 200)
        Me.AutoGrowLabel4.TabIndex = 7
        Me.AutoGrowLabel4.Text = resources.GetString("AutoGrowLabel4.Text")
        '
        'AutoGrowLabel3
        '
        Me.AutoGrowLabel3.BackColor = System.Drawing.Color.Transparent
        Me.AutoGrowLabel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.AutoGrowLabel3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.AutoGrowLabel3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.AutoGrowLabel3.Location = New System.Drawing.Point(0, 120)
        Me.AutoGrowLabel3.Name = "AutoGrowLabel3"
        Me.AutoGrowLabel3.Size = New System.Drawing.Size(367, 160)
        Me.AutoGrowLabel3.TabIndex = 6
        Me.AutoGrowLabel3.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "A simple ""I am using i00 Spell check in my project"" will surffice." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "All conte" & _
            "nt in the i00SpellCheck project is, and remains, the property of i00 Productions" & _
            ", regardless of its usage." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'ProductLink
        '
        Me.ProductLink.BackColor = System.Drawing.Color.Transparent
        Me.ProductLink.Cursor = System.Windows.Forms.Cursors.Hand
        Me.ProductLink.Dock = System.Windows.Forms.DockStyle.Top
        Me.ProductLink.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ProductLink.ForeColor = System.Drawing.Color.Blue
        Me.ProductLink.Location = New System.Drawing.Point(0, 80)
        Me.ProductLink.Name = "ProductLink"
        Me.ProductLink.Size = New System.Drawing.Size(367, 40)
        Me.ProductLink.TabIndex = 5
        Me.ProductLink.Text = "i00 .NET spell checker! - No 3rd party components required!"
        '
        'AutoGrowLabel1
        '
        Me.AutoGrowLabel1.BackColor = System.Drawing.Color.Transparent
        Me.AutoGrowLabel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.AutoGrowLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.AutoGrowLabel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.AutoGrowLabel1.Location = New System.Drawing.Point(0, 0)
        Me.AutoGrowLabel1.Name = "AutoGrowLabel1"
        Me.AutoGrowLabel1.Size = New System.Drawing.Size(367, 80)
        Me.AutoGrowLabel1.TabIndex = 4
        Me.AutoGrowLabel1.Text = "If you wish to use ANY part of this spell checker in your projects you must first" & _
            " state that you are going to use it in a post on VB Forums or Code Project under" & _
            " the thread:"
        '
        'bpCommandBar
        '
        Me.bpCommandBar.BackColor = System.Drawing.Color.Transparent
        Me.bpCommandBar.Controls.Add(Me.chkAccept)
        Me.bpCommandBar.Controls.Add(Me.btnClose)
        Me.bpCommandBar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bpCommandBar.Location = New System.Drawing.Point(0, 233)
        Me.bpCommandBar.Name = "bpCommandBar"
        Me.bpCommandBar.Size = New System.Drawing.Size(384, 29)
        Me.bpCommandBar.TabIndex = 4
        '
        'chkAccept
        '
        Me.chkAccept.AutoSize = True
        Me.chkAccept.Location = New System.Drawing.Point(3, 6)
        Me.chkAccept.Name = "chkAccept"
        Me.chkAccept.Size = New System.Drawing.Size(114, 17)
        Me.chkAccept.TabIndex = 2
        Me.chkAccept.TabStop = False
        Me.chkAccept.Text = "Accept Agreement"
        Me.chkAccept.UseVisualStyleBackColor = True
        '
        'cmsLinks
        '
        Me.cmsLinks.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.VBForumsToolStripMenuItem, Me.CodeProjectToolStripMenuItem})
        Me.cmsLinks.Name = "cmsLinks"
        Me.cmsLinks.Size = New System.Drawing.Size(153, 70)
        '
        'VBForumsToolStripMenuItem
        '
        Me.VBForumsToolStripMenuItem.Name = "VBForumsToolStripMenuItem"
        Me.VBForumsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.VBForumsToolStripMenuItem.Text = "VB Forums"
        '
        'CodeProjectToolStripMenuItem
        '
        Me.CodeProjectToolStripMenuItem.Name = "CodeProjectToolStripMenuItem"
        Me.CodeProjectToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.CodeProjectToolStripMenuItem.Text = "Code Project"
        '
        'frmSplash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(384, 262)
        Me.Controls.Add(Me.bpBackGround)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "frmSplash"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Agreement"
        Me.bpBackGround.ResumeLayout(False)
        Me.bpDocumentHolder.ResumeLayout(False)
        Me.bpCommandBar.ResumeLayout(False)
        Me.bpCommandBar.PerformLayout()
        Me.cmsLinks.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bpCommandBar As Test.BufferedPanel
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents bpBackGround As Test.BufferedPanel
    Friend WithEvents bpDocumentHolder As Test.BufferedPanel
    Friend WithEvents AutoGrowLabel1 As Test.AutoGrowLabel
    Friend WithEvents ProductLink As Test.AutoGrowLabel
    Friend WithEvents AutoGrowLabel3 As Test.AutoGrowLabel
    Friend WithEvents AutoGrowLabel4 As Test.AutoGrowLabel
    Friend WithEvents chkAccept As System.Windows.Forms.CheckBox
    Friend WithEvents cmsLinks As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents VBForumsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CodeProjectToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
