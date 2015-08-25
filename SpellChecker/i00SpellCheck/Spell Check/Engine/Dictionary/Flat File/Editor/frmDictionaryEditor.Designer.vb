<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDictionaryEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDictionaryEditor))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsbSave = New System.Windows.Forms.ToolStripButton
        Me.tsbOpen = New System.Windows.Forms.ToolStripButton
        Me.dgvDictItems = New i00BindingList.DataGridView
        Me.cbcIgnore = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.tbcWord = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.tsKeys = New System.Windows.Forms.ToolStrip
        Me.ToolStripLabel1 = New System.Windows.Forms.ToolStripLabel
        Me.ToolStrip1.SuspendLayout()
        CType(Me.dgvDictItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsKeys.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbSave, Me.tsbOpen})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(624, 25)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbSave
        '
        Me.tsbSave.AutoToolTip = False
        Me.tsbSave.Image = CType(resources.GetObject("tsbSave.Image"), System.Drawing.Image)
        Me.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSave.Name = "tsbSave"
        Me.tsbSave.Size = New System.Drawing.Size(51, 22)
        Me.tsbSave.Text = "Save"
        Me.tsbSave.ToolTipText = "Save"
        '
        'tsbOpen
        '
        Me.tsbOpen.AutoToolTip = False
        Me.tsbOpen.Image = CType(resources.GetObject("tsbOpen.Image"), System.Drawing.Image)
        Me.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbOpen.Name = "tsbOpen"
        Me.tsbOpen.Size = New System.Drawing.Size(56, 22)
        Me.tsbOpen.Text = "Open"
        Me.tsbOpen.ToolTipText = "Open"
        '
        'dgvDictItems
        '
        Me.dgvDictItems.AllowUserToResizeRows = False
        Me.dgvDictItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDictItems.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cbcIgnore, Me.tbcWord})
        Me.dgvDictItems.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDictItems.Location = New System.Drawing.Point(0, 25)
        Me.dgvDictItems.Name = "dgvDictItems"
        Me.dgvDictItems.Size = New System.Drawing.Size(624, 392)
        Me.dgvDictItems.TabIndex = 2
        '
        'cbcIgnore
        '
        Me.cbcIgnore.DataPropertyName = "Ignore"
        Me.cbcIgnore.Frozen = True
        Me.cbcIgnore.HeaderText = "Ignore"
        Me.cbcIgnore.MinimumWidth = 43
        Me.cbcIgnore.Name = "cbcIgnore"
        Me.cbcIgnore.Width = 43
        '
        'tbcWord
        '
        Me.tbcWord.DataPropertyName = "Word"
        Me.tbcWord.HeaderText = "Word"
        Me.tbcWord.MinimumWidth = 100
        Me.tbcWord.Name = "tbcWord"
        Me.tbcWord.Width = 250
        '
        'tsKeys
        '
        Me.tsKeys.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tsKeys.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsKeys.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripLabel1})
        Me.tsKeys.Location = New System.Drawing.Point(0, 417)
        Me.tsKeys.Name = "tsKeys"
        Me.tsKeys.Size = New System.Drawing.Size(624, 25)
        Me.tsKeys.TabIndex = 4
        Me.tsKeys.Text = "ToolStrip2"
        '
        'ToolStripLabel1
        '
        Me.ToolStripLabel1.Name = "ToolStripLabel1"
        Me.ToolStripLabel1.Size = New System.Drawing.Size(29, 22)
        Me.ToolStripLabel1.Text = "Key:"
        '
        'frmDictionaryEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 442)
        Me.Controls.Add(Me.dgvDictItems)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.tsKeys)
        Me.MinimumSize = New System.Drawing.Size(640, 480)
        Me.Name = "frmDictionaryEditor"
        Me.ShowInTaskbar = False
        Me.Text = "Dictionary Editor"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.dgvDictItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsKeys.ResumeLayout(False)
        Me.tsKeys.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbSave As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbOpen As System.Windows.Forms.ToolStripButton
    Friend WithEvents dgvDictItems As i00BindingList.DataGridView
    Friend WithEvents tsKeys As System.Windows.Forms.ToolStrip
    Friend WithEvents ToolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Friend WithEvents cbcIgnore As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents tbcWord As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
