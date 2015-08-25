<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCrosswordGenerator
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
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Across", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Down", System.Windows.Forms.HorizontalAlignment.Left)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCrosswordGenerator))
        Me.bpBackGround = New i00SpellCheck.BufferedPanel
        Me.bpLockableContent = New i00SpellCheck.BufferedPanel
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer
        Me.bpCorsswordBoard = New i00SpellCheck.BufferedPanel
        Me.gcXWordSize = New i00SpellCheck.clsGridCombo
        Me.lstWords = New i00SpellCheck.ExtendedListView
        Me.colNumber = New System.Windows.Forms.ColumnHeader
        Me.colWord = New System.Windows.Forms.ColumnHeader
        Me.colQuestion = New System.Windows.Forms.ColumnHeader
        Me.txtCustomDict = New i00SpellCheck.RTBHighlight
        Me.tsTop = New System.Windows.Forms.ToolStrip
        Me.tsbLoad = New System.Windows.Forms.ToolStripButton
        Me.tsbSave = New System.Windows.Forms.ToolStripButton
        Me.ToolStripLabel2 = New System.Windows.Forms.ToolStripLabel
        Me.cboDict = New System.Windows.Forms.ToolStripComboBox
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.bpBottomHolder = New i00SpellCheck.BufferedPanel
        Me.bpBottom = New i00SpellCheck.BufferedPanel
        Me.btnGenerate = New System.Windows.Forms.Button
        Me.bpStop = New i00SpellCheck.BufferedPanel
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.btnStop = New System.Windows.Forms.Button
        Me.bpBackGround.SuspendLayout()
        Me.bpLockableContent.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.bpCorsswordBoard.SuspendLayout()
        Me.tsTop.SuspendLayout()
        Me.bpBottomHolder.SuspendLayout()
        Me.bpBottom.SuspendLayout()
        Me.bpStop.SuspendLayout()
        Me.SuspendLayout()
        '
        'bpBackGround
        '
        Me.bpBackGround.BackColor = System.Drawing.Color.Transparent
        Me.bpBackGround.Controls.Add(Me.bpLockableContent)
        Me.bpBackGround.Controls.Add(Me.bpBottomHolder)
        Me.bpBackGround.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpBackGround.Location = New System.Drawing.Point(0, 0)
        Me.bpBackGround.Name = "bpBackGround"
        Me.bpBackGround.Size = New System.Drawing.Size(624, 442)
        Me.bpBackGround.TabIndex = 0
        '
        'bpLockableContent
        '
        Me.bpLockableContent.Controls.Add(Me.SplitContainer1)
        Me.bpLockableContent.Controls.Add(Me.txtCustomDict)
        Me.bpLockableContent.Controls.Add(Me.tsTop)
        Me.bpLockableContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpLockableContent.Location = New System.Drawing.Point(0, 0)
        Me.bpLockableContent.Name = "bpLockableContent"
        Me.bpLockableContent.Size = New System.Drawing.Size(624, 384)
        Me.bpLockableContent.TabIndex = 1
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(100, 25)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.bpCorsswordBoard)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lstWords)
        Me.SplitContainer1.Size = New System.Drawing.Size(524, 359)
        Me.SplitContainer1.SplitterDistance = 329
        Me.SplitContainer1.TabIndex = 7
        '
        'bpCorsswordBoard
        '
        Me.bpCorsswordBoard.Controls.Add(Me.gcXWordSize)
        Me.bpCorsswordBoard.Dock = System.Windows.Forms.DockStyle.Fill
        Me.bpCorsswordBoard.Location = New System.Drawing.Point(0, 0)
        Me.bpCorsswordBoard.Name = "bpCorsswordBoard"
        Me.bpCorsswordBoard.Size = New System.Drawing.Size(329, 359)
        Me.bpCorsswordBoard.TabIndex = 3
        '
        'gcXWordSize
        '
        Me.gcXWordSize.CellSize = New System.Drawing.Size(8, 8)
        Me.gcXWordSize.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
        Me.gcXWordSize.DropDownHeight = 402
        Me.gcXWordSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.gcXWordSize.DropDownWidth = 403
        Me.gcXWordSize.FormattingEnabled = True
        Me.gcXWordSize.IntegralHeight = False
        Me.gcXWordSize.ItemHeight = 16
        Me.gcXWordSize.Location = New System.Drawing.Point(60, 91)
        Me.gcXWordSize.MaxGridSize = New System.Drawing.Size(50, 50)
        Me.gcXWordSize.Name = "gcXWordSize"
        Me.gcXWordSize.SelectedCell = New System.Drawing.Point(15, 15)
        Me.gcXWordSize.ShadowOffset = 3
        Me.gcXWordSize.Size = New System.Drawing.Size(120, 22)
        Me.gcXWordSize.TabIndex = 0
        '
        'lstWords
        '
        Me.lstWords.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colNumber, Me.colWord, Me.colQuestion})
        Me.lstWords.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstWords.FullRowSelect = True
        ListViewGroup1.Header = "Across"
        ListViewGroup1.Name = "Across"
        ListViewGroup2.Header = "Down"
        ListViewGroup2.Name = "Down"
        Me.lstWords.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2})
        Me.lstWords.Location = New System.Drawing.Point(0, 0)
        Me.lstWords.Name = "lstWords"
        Me.lstWords.Size = New System.Drawing.Size(191, 359)
        Me.lstWords.TabIndex = 7
        Me.lstWords.UseCompatibleStateImageBehavior = False
        Me.lstWords.View = System.Windows.Forms.View.Details
        '
        'colNumber
        '
        Me.colNumber.Text = "#"
        Me.colNumber.Width = 30
        '
        'colWord
        '
        Me.colWord.Text = "Word"
        '
        'colQuestion
        '
        Me.colQuestion.Text = "Question"
        '
        'txtCustomDict
        '
        Me.txtCustomDict.Dock = System.Windows.Forms.DockStyle.Left
        Me.txtCustomDict.Location = New System.Drawing.Point(0, 25)
        Me.txtCustomDict.Name = "txtCustomDict"
        Me.txtCustomDict.Size = New System.Drawing.Size(100, 359)
        Me.txtCustomDict.TabIndex = 4
        Me.txtCustomDict.Text = resources.GetString("txtCustomDict.Text")
        '
        'tsTop
        '
        Me.tsTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsTop.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbLoad, Me.tsbSave, Me.ToolStripLabel2, Me.cboDict, Me.ToolStripLabel1})
        Me.tsTop.Location = New System.Drawing.Point(0, 0)
        Me.tsTop.Name = "tsTop"
        Me.tsTop.Size = New System.Drawing.Size(624, 25)
        Me.tsTop.TabIndex = 5
        Me.tsTop.Text = "ToolStrip1"
        '
        'tsbLoad
        '
        Me.tsbLoad.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbLoad.Image = CType(resources.GetObject("tsbLoad.Image"), System.Drawing.Image)
        Me.tsbLoad.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbLoad.Name = "tsbLoad"
        Me.tsbLoad.Size = New System.Drawing.Size(23, 22)
        Me.tsbLoad.Text = "Open"
        '
        'tsbSave
        '
        Me.tsbSave.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbSave.Image = CType(resources.GetObject("tsbSave.Image"), System.Drawing.Image)
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(23, 22)
        Me.tsbSave.Text = "Save"
        '
        'ToolStripLabel2
        '
        Me.ToolStripLabel2.Name = "ToolStripLabel2"
        Me.ToolStripLabel2.Size = New System.Drawing.Size(64, 22)
        Me.ToolStripLabel2.Text = "Dictionary:"
        '
        'cboDict
        '
        Me.cboDict.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDict.Name = "cboDict"
        Me.cboDict.Size = New System.Drawing.Size(160, 25)
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(89, 22)
        Me.ToolStripLabel1.Text = "Crossword Size:"
        '
        'bpBottomHolder
        '
        Me.bpBottomHolder.AutoSize = True
        Me.bpBottomHolder.BackColor = System.Drawing.Color.Transparent
        Me.bpBottomHolder.Controls.Add(Me.bpBottom)
        Me.bpBottomHolder.Controls.Add(Me.bpStop)
        Me.bpBottomHolder.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bpBottomHolder.Location = New System.Drawing.Point(0, 384)
        Me.bpBottomHolder.Name = "bpBottomHolder"
        Me.bpBottomHolder.Size = New System.Drawing.Size(624, 58)
        Me.bpBottomHolder.TabIndex = 5
        '
        'bpBottom
        '
        Me.bpBottom.BackColor = System.Drawing.Color.Transparent
        Me.bpBottom.Controls.Add(Me.btnGenerate)
        Me.bpBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bpBottom.Location = New System.Drawing.Point(0, 0)
        Me.bpBottom.Name = "bpBottom"
        Me.bpBottom.Size = New System.Drawing.Size(624, 29)
        Me.bpBottom.TabIndex = 2
        '
        'btnGenerate
        '
        Me.btnGenerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerate.Location = New System.Drawing.Point(546, 3)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerate.TabIndex = 3
        Me.btnGenerate.Text = "Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'bpStop
        '
        Me.bpStop.BackColor = System.Drawing.Color.Transparent
        Me.bpStop.Controls.Add(Me.ProgressBar1)
        Me.bpStop.Controls.Add(Me.btnStop)
        Me.bpStop.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.bpStop.Location = New System.Drawing.Point(0, 29)
        Me.bpStop.Name = "bpStop"
        Me.bpStop.Size = New System.Drawing.Size(624, 29)
        Me.bpStop.TabIndex = 4
        Me.bpStop.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(3, 3)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(100, 23)
        Me.ProgressBar1.TabIndex = 1
        '
        'btnStop
        '
        Me.btnStop.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStop.Location = New System.Drawing.Point(546, 3)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(75, 23)
        Me.btnStop.TabIndex = 0
        Me.btnStop.Text = "Stop"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'frmCrosswordGenerator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 442)
        Me.Controls.Add(Me.bpBackGround)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "frmCrosswordGenerator"
        Me.Text = "Crossword Generator"
        Me.bpBackGround.ResumeLayout(False)
        Me.bpBackGround.PerformLayout()
        Me.bpLockableContent.ResumeLayout(False)
        Me.bpLockableContent.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.bpCorsswordBoard.ResumeLayout(False)
        Me.tsTop.ResumeLayout(False)
        Me.tsTop.PerformLayout()
        Me.bpBottomHolder.ResumeLayout(False)
        Me.bpBottom.ResumeLayout(False)
        Me.bpStop.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gcXWordSize As i00SpellCheck.clsGridCombo
    Friend WithEvents bpBackGround As BufferedPanel
    Friend WithEvents bpBottom As BufferedPanel
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents bpCorsswordBoard As i00SpellCheck.BufferedPanel
    Friend WithEvents bpLockableContent As i00SpellCheck.BufferedPanel
    Friend WithEvents bpStop As i00SpellCheck.BufferedPanel
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents txtCustomDict As RTBHighlight
    Friend WithEvents tsTop As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbLoad As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents ToolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cboDict As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents bpBottomHolder As i00SpellCheck.BufferedPanel
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents lstWords As ExtendedListView
    Friend WithEvents colNumber As System.Windows.Forms.ColumnHeader
    Friend WithEvents colWord As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQuestion As System.Windows.Forms.ColumnHeader
End Class
