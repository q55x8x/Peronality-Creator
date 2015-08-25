<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpellCheckDialog
    Inherits WindowAnimation 'System.Windows.Forms.Form

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
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtChangeTo = New System.Windows.Forms.TextBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnChangeAll = New System.Windows.Forms.Button
        Me.btnIgnore = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.HtmlSpellCheck1 = New i00SpellCheck.HTMLSpellCheck
        Me.cmsHTMLSpellCheck = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mtsChangeTo = New i00SpellCheck.MenuTextSeperator
        Me.tstbChangeTo = New System.Windows.Forms.ToolStripTextBox
        Me.tsiRevertTo = New System.Windows.Forms.ToolStripMenuItem
        Me.pbChangeAll = New System.Windows.Forms.ProgressBar
        Me.btnRevertAll = New System.Windows.Forms.Button
        Me.pnlSuggestions = New System.Windows.Forms.Panel
        Me.lvSuggestions = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.pnlPleaseWait = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.bpIcon = New i00SpellCheck.BufferedPanel
        Me.Label2 = New System.Windows.Forms.Label
        Me.pnlBottomContent = New System.Windows.Forms.Panel
        Me.btnSkip = New System.Windows.Forms.Button
        Me.btnRevert = New System.Windows.Forms.Button
        Me.pnlContent = New System.Windows.Forms.Panel
        Me.btnAdd = New i00SpellCheck.SplitButton
        Me.cmsAdd = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsiAddCaseSensitive = New System.Windows.Forms.ToolStripMenuItem
        Me.tsiAddCaseInsensitive = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1.SuspendLayout()
        Me.cmsHTMLSpellCheck.SuspendLayout()
        Me.pnlSuggestions.SuspendLayout()
        Me.pnlPleaseWait.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.pnlBottomContent.SuspendLayout()
        Me.pnlContent.SuspendLayout()
        Me.cmsAdd.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(-3, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Change to:"
        '
        'txtChangeTo
        '
        Me.txtChangeTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtChangeTo.Location = New System.Drawing.Point(68, 0)
        Me.txtChangeTo.Name = "txtChangeTo"
        Me.txtChangeTo.Size = New System.Drawing.Size(132, 20)
        Me.txtChangeTo.TabIndex = 4
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(297, 273)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnChange
        '
        Me.btnChange.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChange.Location = New System.Drawing.Point(297, 157)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(75, 23)
        Me.btnChange.TabIndex = 1
        Me.btnChange.Tag = ""
        Me.btnChange.Text = "Change"
        Me.btnChange.UseVisualStyleBackColor = True
        '
        'btnChangeAll
        '
        Me.btnChangeAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnChangeAll.Location = New System.Drawing.Point(297, 186)
        Me.btnChangeAll.Name = "btnChangeAll"
        Me.btnChangeAll.Size = New System.Drawing.Size(75, 23)
        Me.btnChangeAll.TabIndex = 1
        Me.btnChangeAll.Text = "Change All"
        Me.btnChangeAll.UseVisualStyleBackColor = True
        '
        'btnIgnore
        '
        Me.btnIgnore.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnIgnore.Location = New System.Drawing.Point(297, 41)
        Me.btnIgnore.Name = "btnIgnore"
        Me.btnIgnore.Size = New System.Drawing.Size(75, 23)
        Me.btnIgnore.TabIndex = 6
        Me.btnIgnore.Text = "Ignore"
        Me.btnIgnore.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.HtmlSpellCheck1)
        Me.Panel1.Controls.Add(Me.pbChangeAll)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(279, 139)
        Me.Panel1.TabIndex = 7
        '
        'HtmlSpellCheck1
        '
        Me.HtmlSpellCheck1.AllowWebBrowserDrop = False
        Me.HtmlSpellCheck1.BackColor = System.Drawing.SystemColors.Window
        Me.HtmlSpellCheck1.CaseMistakeColor = System.Drawing.Color.Green
        Me.HtmlSpellCheck1.ContextMenuStrip = Me.cmsHTMLSpellCheck
        Me.HtmlSpellCheck1.Dictionary = Nothing
        Me.HtmlSpellCheck1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.HtmlSpellCheck1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HtmlSpellCheck1.IsWebBrowserContextMenuEnabled = False
        Me.HtmlSpellCheck1.Location = New System.Drawing.Point(0, 0)
        Me.HtmlSpellCheck1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.HtmlSpellCheck1.MistakeColor = System.Drawing.Color.Red
        Me.HtmlSpellCheck1.Name = "HtmlSpellCheck1"
        Me.HtmlSpellCheck1.Size = New System.Drawing.Size(277, 127)
        Me.HtmlSpellCheck1.TabIndex = 0
        '
        'cmsHTMLSpellCheck
        '
        Me.cmsHTMLSpellCheck.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mtsChangeTo, Me.tstbChangeTo, Me.tsiRevertTo})
        Me.cmsHTMLSpellCheck.Name = "ContextMenuStrip1"
        Me.cmsHTMLSpellCheck.Size = New System.Drawing.Size(161, 69)
        '
        'mtsChangeTo
        '
        Me.mtsChangeTo.AutoSize = False
        Me.mtsChangeTo.BackColor = System.Drawing.SystemColors.Control
        Me.mtsChangeTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.mtsChangeTo.Name = "mtsChangeTo"
        Me.mtsChangeTo.Size = New System.Drawing.Size(1920, 18)
        Me.mtsChangeTo.Text = "Change to:"
        '
        'tstbChangeTo
        '
        Me.tstbChangeTo.Name = "tstbChangeTo"
        Me.tstbChangeTo.Size = New System.Drawing.Size(100, 23)
        '
        'tsiRevertTo
        '
        Me.tsiRevertTo.Image = Global.i00SpellCheck.My.Resources.Resources.Undo
        Me.tsiRevertTo.Name = "tsiRevertTo"
        Me.tsiRevertTo.Size = New System.Drawing.Size(160, 22)
        Me.tsiRevertTo.Text = "Revert to..."
        '
        'pbChangeAll
        '
        Me.pbChangeAll.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pbChangeAll.Location = New System.Drawing.Point(0, 127)
        Me.pbChangeAll.Name = "pbChangeAll"
        Me.pbChangeAll.Size = New System.Drawing.Size(277, 10)
        Me.pbChangeAll.TabIndex = 13
        '
        'btnRevertAll
        '
        Me.btnRevertAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRevertAll.Location = New System.Drawing.Point(297, 215)
        Me.btnRevertAll.Name = "btnRevertAll"
        Me.btnRevertAll.Size = New System.Drawing.Size(75, 23)
        Me.btnRevertAll.TabIndex = 8
        Me.btnRevertAll.Text = "Revert All"
        Me.btnRevertAll.UseVisualStyleBackColor = True
        '
        'pnlSuggestions
        '
        Me.pnlSuggestions.Controls.Add(Me.lvSuggestions)
        Me.pnlSuggestions.Controls.Add(Me.Label1)
        Me.pnlSuggestions.Controls.Add(Me.txtChangeTo)
        Me.pnlSuggestions.Location = New System.Drawing.Point(3, 53)
        Me.pnlSuggestions.Name = "pnlSuggestions"
        Me.pnlSuggestions.Size = New System.Drawing.Size(200, 65)
        Me.pnlSuggestions.TabIndex = 9
        '
        'lvSuggestions
        '
        Me.lvSuggestions.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.lvSuggestions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvSuggestions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lvSuggestions.FullRowSelect = True
        Me.lvSuggestions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvSuggestions.HideSelection = False
        Me.lvSuggestions.Location = New System.Drawing.Point(0, 26)
        Me.lvSuggestions.MultiSelect = False
        Me.lvSuggestions.Name = "lvSuggestions"
        Me.lvSuggestions.OwnerDraw = True
        Me.lvSuggestions.Size = New System.Drawing.Size(200, 39)
        Me.lvSuggestions.TabIndex = 5
        Me.lvSuggestions.UseCompatibleStateImageBehavior = False
        Me.lvSuggestions.View = System.Windows.Forms.View.Details
        '
        'pnlPleaseWait
        '
        Me.pnlPleaseWait.Controls.Add(Me.Panel4)
        Me.pnlPleaseWait.Location = New System.Drawing.Point(3, 6)
        Me.pnlPleaseWait.Name = "pnlPleaseWait"
        Me.pnlPleaseWait.Size = New System.Drawing.Size(155, 44)
        Me.pnlPleaseWait.TabIndex = 10
        '
        'Panel4
        '
        Me.Panel4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Panel4.Controls.Add(Me.bpIcon)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Location = New System.Drawing.Point(3, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(149, 38)
        Me.Panel4.TabIndex = 0
        '
        'bpIcon
        '
        Me.bpIcon.Location = New System.Drawing.Point(3, 3)
        Me.bpIcon.Name = "bpIcon"
        Me.bpIcon.Size = New System.Drawing.Size(32, 32)
        Me.bpIcon.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(41, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 26)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Checking spelling ..." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please wait ..."
        '
        'pnlBottomContent
        '
        Me.pnlBottomContent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlBottomContent.Controls.Add(Me.pnlSuggestions)
        Me.pnlBottomContent.Controls.Add(Me.pnlPleaseWait)
        Me.pnlBottomContent.Location = New System.Drawing.Point(12, 157)
        Me.pnlBottomContent.Name = "pnlBottomContent"
        Me.pnlBottomContent.Size = New System.Drawing.Size(279, 139)
        Me.pnlBottomContent.TabIndex = 11
        '
        'btnSkip
        '
        Me.btnSkip.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSkip.Location = New System.Drawing.Point(297, 70)
        Me.btnSkip.Name = "btnSkip"
        Me.btnSkip.Size = New System.Drawing.Size(75, 23)
        Me.btnSkip.TabIndex = 12
        Me.btnSkip.Tag = ""
        Me.btnSkip.Text = "Next"
        Me.btnSkip.UseVisualStyleBackColor = True
        '
        'btnRevert
        '
        Me.btnRevert.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRevert.Location = New System.Drawing.Point(297, 99)
        Me.btnRevert.Name = "btnRevert"
        Me.btnRevert.Size = New System.Drawing.Size(75, 23)
        Me.btnRevert.TabIndex = 12
        Me.btnRevert.Text = "Revert"
        Me.btnRevert.UseVisualStyleBackColor = True
        '
        'pnlContent
        '
        Me.pnlContent.Controls.Add(Me.btnAdd)
        Me.pnlContent.Controls.Add(Me.btnChange)
        Me.pnlContent.Controls.Add(Me.pnlBottomContent)
        Me.pnlContent.Controls.Add(Me.btnChangeAll)
        Me.pnlContent.Controls.Add(Me.btnRevertAll)
        Me.pnlContent.Controls.Add(Me.btnClose)
        Me.pnlContent.Controls.Add(Me.btnIgnore)
        Me.pnlContent.Controls.Add(Me.Panel1)
        Me.pnlContent.Controls.Add(Me.btnRevert)
        Me.pnlContent.Controls.Add(Me.btnSkip)
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(0, 0)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(384, 308)
        Me.pnlContent.TabIndex = 13
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Enabled = False
        Me.btnAdd.Location = New System.Drawing.Point(297, 12)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Padding = New System.Windows.Forms.Padding(0, 0, 16, 0)
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 13
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'cmsAdd
        '
        Me.cmsAdd.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsiAddCaseSensitive, Me.tsiAddCaseInsensitive})
        Me.cmsAdd.Name = "cmsAdd"
        Me.cmsAdd.Size = New System.Drawing.Size(161, 48)
        '
        'tsiAddCaseSensitive
        '
        Me.tsiAddCaseSensitive.Image = Global.i00SpellCheck.My.Resources.Resources.CaseSensitive
        Me.tsiAddCaseSensitive.Name = "tsiAddCaseSensitive"
        Me.tsiAddCaseSensitive.Size = New System.Drawing.Size(160, 22)
        Me.tsiAddCaseSensitive.Text = "Case sensitive:"
        '
        'tsiAddCaseInsensitive
        '
        Me.tsiAddCaseInsensitive.Image = Global.i00SpellCheck.My.Resources.Resources.CaseInsensitive
        Me.tsiAddCaseInsensitive.Name = "tsiAddCaseInsensitive"
        Me.tsiAddCaseInsensitive.Size = New System.Drawing.Size(160, 22)
        Me.tsiAddCaseInsensitive.Text = "Case insensitive:"
        '
        'SpellCheckDialog
        '
        Me.AcceptButton = Me.btnChange
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnClose
        Me.ClientSize = New System.Drawing.Size(384, 308)
        Me.Controls.Add(Me.pnlContent)
        Me.HelpButton = True
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 342)
        Me.Name = "SpellCheckDialog"
        Me.Text = "Spell Check"
        Me.Panel1.ResumeLayout(False)
        Me.cmsHTMLSpellCheck.ResumeLayout(False)
        Me.cmsHTMLSpellCheck.PerformLayout()
        Me.pnlSuggestions.ResumeLayout(False)
        Me.pnlSuggestions.PerformLayout()
        Me.pnlPleaseWait.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.pnlBottomContent.ResumeLayout(False)
        Me.pnlContent.ResumeLayout(False)
        Me.cmsAdd.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtChangeTo As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnChangeAll As System.Windows.Forms.Button
    Friend WithEvents btnIgnore As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents HtmlSpellCheck1 As i00SpellCheck.HTMLSpellCheck
    Friend WithEvents btnRevertAll As System.Windows.Forms.Button
    Friend WithEvents cmsHTMLSpellCheck As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsiRevertTo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlSuggestions As System.Windows.Forms.Panel
    Friend WithEvents pnlPleaseWait As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents bpIcon As i00SpellCheck.BufferedPanel
    Friend WithEvents pnlBottomContent As System.Windows.Forms.Panel
    Friend WithEvents btnSkip As System.Windows.Forms.Button
    Friend WithEvents mtsChangeTo As i00SpellCheck.MenuTextSeperator
    Friend WithEvents tstbChangeTo As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRevert As System.Windows.Forms.Button
    Friend WithEvents lvSuggestions As System.Windows.Forms.ListView
    Friend WithEvents pnlContent As System.Windows.Forms.Panel
    Friend WithEvents pbChangeAll As System.Windows.Forms.ProgressBar
    Friend WithEvents btnAdd As i00SpellCheck.SplitButton
    Friend WithEvents cmsAdd As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsiAddCaseSensitive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsiAddCaseInsensitive As System.Windows.Forms.ToolStripMenuItem
End Class
