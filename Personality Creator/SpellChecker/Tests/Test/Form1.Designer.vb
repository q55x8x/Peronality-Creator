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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tslStatus = New System.Windows.Forms.ToolStripLabel
        Me.tsbDonate = New System.Windows.Forms.ToolStripButton
        Me.tsbi00Productions = New System.Windows.Forms.ToolStripButton
        Me.tsbCodeProject = New System.Windows.Forms.ToolStripButton
        Me.tsbVBForums = New System.Windows.Forms.ToolStripButton
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip
        Me.tsbExamples = New System.Windows.Forms.ToolStripDropDownButton
        Me.CrosswordSolverToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.MenuTextSeperator3 = New i00SpellCheck.MenuTextSeperator
        Me.tsbSuggestionLookup = New System.Windows.Forms.ToolStripTextBox
        Me.MenuTextSeperator1 = New i00SpellCheck.MenuTextSeperator
        Me.tstbAnagramLookup = New System.Windows.Forms.ToolStripTextBox
        Me.MenuTextSeperator2 = New i00SpellCheck.MenuTextSeperator
        Me.tstbScrabbleHelper = New System.Windows.Forms.ToolStripTextBox
        Me.tsbAbout = New System.Windows.Forms.ToolStripButton
        Me.ToolStripDropDownButton2 = New System.Windows.Forms.ToolStripDropDownButton
        Me.MenuTextSeperator4 = New i00SpellCheck.MenuTextSeperator
        Me.SpellingErrorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tsiCPSpellingError = New i00SpellCheck.tsiColorPicker
        Me.CaseErrorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tsiCPCaseError = New i00SpellCheck.tsiColorPicker
        Me.IgnoredWordToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tsiCPIgnoreColor = New i00SpellCheck.tsiColorPicker
        Me.MenuTextSeperator5 = New i00SpellCheck.MenuTextSeperator
        Me.tsiDrawStyle = New System.Windows.Forms.ToolStripComboBox
        Me.MenuTextSeperator6 = New i00SpellCheck.MenuTextSeperator
        Me.ShowErrorsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ShowIgnoredToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.WhenCtrlIsPressedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AlwaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NeverToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.tsbPerformanceMonitor = New System.Windows.Forms.ToolStripButton
        Me.tsbSpellCheck = New System.Windows.Forms.ToolStripButton
        Me.tsbProperties = New System.Windows.Forms.ToolStripButton
        Me.tabSpellControls = New System.Windows.Forms.TabControl
        Me.ilTabSpellControls = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStrip3 = New System.Windows.Forms.ToolStrip
        Me.tsiEnabled = New System.Windows.Forms.ToolStripButton
        Me.tsbEditDictionary = New System.Windows.Forms.ToolStripButton
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AsdToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStrip1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.ToolStrip3.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslStatus, Me.tsbDonate, Me.tsbi00Productions, Me.tsbCodeProject, Me.tsbVBForums})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 417)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(624, 25)
        Me.ToolStrip1.TabIndex = 3
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tslStatus
        '
        Me.tslStatus.Name = "tslStatus"
        Me.tslStatus.Size = New System.Drawing.Size(86, 22)
        Me.tslStatus.Text = "i00 Spell Check"
        '
        'tsbDonate
        '
        Me.tsbDonate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbDonate.AutoToolTip = False
        Me.tsbDonate.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDonate.Name = "tsbDonate"
        Me.tsbDonate.Size = New System.Drawing.Size(49, 22)
        Me.tsbDonate.Text = "Donate"
        '
        'tsbi00Productions
        '
        Me.tsbi00Productions.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbi00Productions.AutoToolTip = False
        Me.tsbi00Productions.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbi00Productions.Name = "tsbi00Productions"
        Me.tsbi00Productions.Size = New System.Drawing.Size(93, 22)
        Me.tsbi00Productions.Text = "i00 Productions"
        '
        'tsbCodeProject
        '
        Me.tsbCodeProject.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbCodeProject.AutoToolTip = False
        Me.tsbCodeProject.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCodeProject.Name = "tsbCodeProject"
        Me.tsbCodeProject.Size = New System.Drawing.Size(79, 22)
        Me.tsbCodeProject.Text = "Code Project"
        '
        'tsbVBForums
        '
        Me.tsbVBForums.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbVBForums.AutoToolTip = False
        Me.tsbVBForums.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbVBForums.Name = "tsbVBForums"
        Me.tsbVBForums.Size = New System.Drawing.Size(68, 22)
        Me.tsbVBForums.Text = "VB Forums"
        '
        'ToolStrip2
        '
        Me.ToolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbExamples, Me.tsbAbout, Me.ToolStripDropDownButton2, Me.ToolStripSeparator1, Me.tsbPerformanceMonitor})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(624, 25)
        Me.ToolStrip2.TabIndex = 5
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'tsbExamples
        '
        Me.tsbExamples.AutoToolTip = False
        Me.tsbExamples.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CrosswordSolverToolStripMenuItem, Me.MenuTextSeperator3, Me.tsbSuggestionLookup, Me.MenuTextSeperator1, Me.tstbAnagramLookup, Me.MenuTextSeperator2, Me.tstbScrabbleHelper})
        Me.tsbExamples.Image = CType(resources.GetObject("tsbExamples.Image"), System.Drawing.Image)
        Me.tsbExamples.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbExamples.Name = "tsbExamples"
        Me.tsbExamples.Size = New System.Drawing.Size(127, 22)
        Me.tsbExamples.Text = "Other Examples..."
        '
        'CrosswordSolverToolStripMenuItem
        '
        Me.CrosswordSolverToolStripMenuItem.Name = "CrosswordSolverToolStripMenuItem"
        Me.CrosswordSolverToolStripMenuItem.Size = New System.Drawing.Size(185, 22)
        Me.CrosswordSolverToolStripMenuItem.Text = "Crossword Generator"
        '
        'MenuTextSeperator3
        '
        Me.MenuTextSeperator3.AutoSize = False
        Me.MenuTextSeperator3.BackColor = System.Drawing.SystemColors.Control
        Me.MenuTextSeperator3.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.MenuTextSeperator3.Name = "MenuTextSeperator3"
        Me.MenuTextSeperator3.Size = New System.Drawing.Size(1920, 18)
        Me.MenuTextSeperator3.Text = "Suggestion Lookup:"
        '
        'tsbSuggestionLookup
        '
        Me.tsbSuggestionLookup.Name = "tsbSuggestionLookup"
        Me.tsbSuggestionLookup.Size = New System.Drawing.Size(100, 23)
        '
        'MenuTextSeperator1
        '
        Me.MenuTextSeperator1.AutoSize = False
        Me.MenuTextSeperator1.BackColor = System.Drawing.SystemColors.Control
        Me.MenuTextSeperator1.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.MenuTextSeperator1.Name = "MenuTextSeperator1"
        Me.MenuTextSeperator1.Size = New System.Drawing.Size(1920, 18)
        Me.MenuTextSeperator1.Text = "Anagram Lookup:"
        '
        'tstbAnagramLookup
        '
        Me.tstbAnagramLookup.Name = "tstbAnagramLookup"
        Me.tstbAnagramLookup.Size = New System.Drawing.Size(100, 23)
        '
        'MenuTextSeperator2
        '
        Me.MenuTextSeperator2.AutoSize = False
        Me.MenuTextSeperator2.BackColor = System.Drawing.SystemColors.Control
        Me.MenuTextSeperator2.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.MenuTextSeperator2.Name = "MenuTextSeperator2"
        Me.MenuTextSeperator2.Size = New System.Drawing.Size(1920, 18)
        Me.MenuTextSeperator2.Text = "Scrabble Helper:"
        '
        'tstbScrabbleHelper
        '
        Me.tstbScrabbleHelper.Name = "tstbScrabbleHelper"
        Me.tstbScrabbleHelper.Size = New System.Drawing.Size(100, 23)
        '
        'tsbAbout
        '
        Me.tsbAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbAbout.AutoToolTip = False
        Me.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAbout.Name = "tsbAbout"
        Me.tsbAbout.Size = New System.Drawing.Size(44, 22)
        Me.tsbAbout.Text = "About"
        '
        'ToolStripDropDownButton2
        '
        Me.ToolStripDropDownButton2.AutoToolTip = False
        Me.ToolStripDropDownButton2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuTextSeperator4, Me.SpellingErrorToolStripMenuItem, Me.CaseErrorToolStripMenuItem, Me.IgnoredWordToolStripMenuItem, Me.MenuTextSeperator5, Me.tsiDrawStyle, Me.MenuTextSeperator6, Me.ShowErrorsToolStripMenuItem, Me.ShowIgnoredToolStripMenuItem})
        Me.ToolStripDropDownButton2.Image = CType(resources.GetObject("ToolStripDropDownButton2.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton2.Name = "ToolStripDropDownButton2"
        Me.ToolStripDropDownButton2.Size = New System.Drawing.Size(92, 22)
        Me.ToolStripDropDownButton2.Text = "Customize"
        '
        'MenuTextSeperator4
        '
        Me.MenuTextSeperator4.AutoSize = False
        Me.MenuTextSeperator4.BackColor = System.Drawing.SystemColors.Control
        Me.MenuTextSeperator4.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.MenuTextSeperator4.Name = "MenuTextSeperator4"
        Me.MenuTextSeperator4.Size = New System.Drawing.Size(1920, 18)
        Me.MenuTextSeperator4.Text = "Colors:"
        '
        'SpellingErrorToolStripMenuItem
        '
        Me.SpellingErrorToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsiCPSpellingError})
        Me.SpellingErrorToolStripMenuItem.Name = "SpellingErrorToolStripMenuItem"
        Me.SpellingErrorToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.SpellingErrorToolStripMenuItem.Text = "Spelling Error"
        '
        'tsiCPSpellingError
        '
        Me.tsiCPSpellingError.AutoSize = False
        Me.tsiCPSpellingError.Colors = CType(resources.GetObject("tsiCPSpellingError.Colors"), System.Collections.Generic.List(Of System.Drawing.Color))
        Me.tsiCPSpellingError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None
        Me.tsiCPSpellingError.Name = "tsiCPSpellingError"
        Me.tsiCPSpellingError.Persistent = True
        Me.tsiCPSpellingError.SelectedColor = System.Drawing.Color.Red
        '
        'CaseErrorToolStripMenuItem
        '
        Me.CaseErrorToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsiCPCaseError})
        Me.CaseErrorToolStripMenuItem.Name = "CaseErrorToolStripMenuItem"
        Me.CaseErrorToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.CaseErrorToolStripMenuItem.Text = "Case Error"
        '
        'tsiCPCaseError
        '
        Me.tsiCPCaseError.AutoSize = False
        Me.tsiCPCaseError.Colors = CType(resources.GetObject("tsiCPCaseError.Colors"), System.Collections.Generic.List(Of System.Drawing.Color))
        Me.tsiCPCaseError.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None
        Me.tsiCPCaseError.Name = "tsiCPCaseError"
        Me.tsiCPCaseError.Persistent = True
        Me.tsiCPCaseError.SelectedColor = System.Drawing.Color.Green
        '
        'IgnoredWordToolStripMenuItem
        '
        Me.IgnoredWordToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsiCPIgnoreColor})
        Me.IgnoredWordToolStripMenuItem.Name = "IgnoredWordToolStripMenuItem"
        Me.IgnoredWordToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.IgnoredWordToolStripMenuItem.Text = "Ignored Word"
        '
        'tsiCPIgnoreColor
        '
        Me.tsiCPIgnoreColor.AutoSize = False
        Me.tsiCPIgnoreColor.Colors = CType(resources.GetObject("tsiCPIgnoreColor.Colors"), System.Collections.Generic.List(Of System.Drawing.Color))
        Me.tsiCPIgnoreColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None
        Me.tsiCPIgnoreColor.Name = "tsiCPIgnoreColor"
        Me.tsiCPIgnoreColor.Persistent = True
        Me.tsiCPIgnoreColor.SelectedColor = System.Drawing.Color.Blue
        '
        'MenuTextSeperator5
        '
        Me.MenuTextSeperator5.AutoSize = False
        Me.MenuTextSeperator5.BackColor = System.Drawing.SystemColors.Control
        Me.MenuTextSeperator5.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.MenuTextSeperator5.Name = "MenuTextSeperator5"
        Me.MenuTextSeperator5.Size = New System.Drawing.Size(1920, 18)
        Me.MenuTextSeperator5.Text = "Style:"
        '
        'tsiDrawStyle
        '
        Me.tsiDrawStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.tsiDrawStyle.Items.AddRange(New Object() {"Default", "Boxed In", "Opera", "Draft Plan"})
        Me.tsiDrawStyle.Name = "tsiDrawStyle"
        Me.tsiDrawStyle.Size = New System.Drawing.Size(121, 23)
        '
        'MenuTextSeperator6
        '
        Me.MenuTextSeperator6.AutoSize = False
        Me.MenuTextSeperator6.BackColor = System.Drawing.SystemColors.Control
        Me.MenuTextSeperator6.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.MenuTextSeperator6.Name = "MenuTextSeperator6"
        Me.MenuTextSeperator6.Size = New System.Drawing.Size(1920, 18)
        Me.MenuTextSeperator6.Text = "Misc:"
        '
        'ShowErrorsToolStripMenuItem
        '
        Me.ShowErrorsToolStripMenuItem.Checked = True
        Me.ShowErrorsToolStripMenuItem.CheckOnClick = True
        Me.ShowErrorsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ShowErrorsToolStripMenuItem.Name = "ShowErrorsToolStripMenuItem"
        Me.ShowErrorsToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.ShowErrorsToolStripMenuItem.Text = "Show Mistakes"
        '
        'ShowIgnoredToolStripMenuItem
        '
        Me.ShowIgnoredToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WhenCtrlIsPressedToolStripMenuItem, Me.AlwaysToolStripMenuItem, Me.NeverToolStripMenuItem})
        Me.ShowIgnoredToolStripMenuItem.Name = "ShowIgnoredToolStripMenuItem"
        Me.ShowIgnoredToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.ShowIgnoredToolStripMenuItem.Text = "Show Ignored"
        '
        'WhenCtrlIsPressedToolStripMenuItem
        '
        Me.WhenCtrlIsPressedToolStripMenuItem.Checked = True
        Me.WhenCtrlIsPressedToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.WhenCtrlIsPressedToolStripMenuItem.Name = "WhenCtrlIsPressedToolStripMenuItem"
        Me.WhenCtrlIsPressedToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.WhenCtrlIsPressedToolStripMenuItem.Tag = "2"
        Me.WhenCtrlIsPressedToolStripMenuItem.Text = "When Ctrl is Pressed"
        '
        'AlwaysToolStripMenuItem
        '
        Me.AlwaysToolStripMenuItem.Name = "AlwaysToolStripMenuItem"
        Me.AlwaysToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.AlwaysToolStripMenuItem.Tag = "0"
        Me.AlwaysToolStripMenuItem.Text = "Always"
        '
        'NeverToolStripMenuItem
        '
        Me.NeverToolStripMenuItem.Name = "NeverToolStripMenuItem"
        Me.NeverToolStripMenuItem.Size = New System.Drawing.Size(181, 22)
        Me.NeverToolStripMenuItem.Tag = "1"
        Me.NeverToolStripMenuItem.Text = "Never"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'tsbPerformanceMonitor
        '
        Me.tsbPerformanceMonitor.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbPerformanceMonitor.AutoToolTip = False
        Me.tsbPerformanceMonitor.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPerformanceMonitor.Name = "tsbPerformanceMonitor"
        Me.tsbPerformanceMonitor.Size = New System.Drawing.Size(125, 22)
        Me.tsbPerformanceMonitor.Text = "Performance Monitor"
        '
        'tsbSpellCheck
        '
        Me.tsbSpellCheck.AutoToolTip = False
        Me.tsbSpellCheck.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbSpellCheck.Name = "tsbSpellCheck"
        Me.tsbSpellCheck.Size = New System.Drawing.Size(72, 22)
        Me.tsbSpellCheck.Text = "Spell Check"
        '
        'tsbProperties
        '
        Me.tsbProperties.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbProperties.AutoToolTip = False
        Me.tsbProperties.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbProperties.Name = "tsbProperties"
        Me.tsbProperties.Size = New System.Drawing.Size(64, 22)
        Me.tsbProperties.Text = "Properties"
        '
        'tabSpellControls
        '
        Me.tabSpellControls.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabSpellControls.ImageList = Me.ilTabSpellControls
        Me.tabSpellControls.Location = New System.Drawing.Point(0, 50)
        Me.tabSpellControls.Name = "tabSpellControls"
        Me.tabSpellControls.SelectedIndex = 0
        Me.tabSpellControls.Size = New System.Drawing.Size(624, 367)
        Me.tabSpellControls.TabIndex = 6
        '
        'ilTabSpellControls
        '
        Me.ilTabSpellControls.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ilTabSpellControls.ImageSize = New System.Drawing.Size(16, 16)
        Me.ilTabSpellControls.TransparentColor = System.Drawing.Color.Transparent
        '
        'ToolStrip3
        '
        Me.ToolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip3.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbSpellCheck, Me.tsbProperties, Me.tsiEnabled, Me.tsbEditDictionary})
        Me.ToolStrip3.Location = New System.Drawing.Point(0, 25)
        Me.ToolStrip3.Name = "ToolStrip3"
        Me.ToolStrip3.Size = New System.Drawing.Size(624, 25)
        Me.ToolStrip3.TabIndex = 7
        Me.ToolStrip3.Text = "ToolStrip3"
        '
        'tsiEnabled
        '
        Me.tsiEnabled.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsiEnabled.AutoToolTip = False
        Me.tsiEnabled.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsiEnabled.Name = "tsiEnabled"
        Me.tsiEnabled.Size = New System.Drawing.Size(53, 22)
        Me.tsiEnabled.Text = "Enabled"
        '
        'tsbEditDictionary
        '
        Me.tsbEditDictionary.AutoToolTip = False
        Me.tsbEditDictionary.Image = CType(resources.GetObject("tsbEditDictionary.Image"), System.Drawing.Image)
        Me.tsbEditDictionary.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbEditDictionary.Name = "tsbEditDictionary"
        Me.tsbEditDictionary.Size = New System.Drawing.Size(104, 22)
        Me.tsbEditDictionary.Text = "Edit Dictionary"
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "Column1"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AsdToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(93, 26)
        '
        'AsdToolStripMenuItem
        '
        Me.AsdToolStripMenuItem.Name = "AsdToolStripMenuItem"
        Me.AsdToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.AsdToolStripMenuItem.Text = "asd"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 442)
        Me.Controls.Add(Me.tabSpellControls)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.ToolStrip3)
        Me.Controls.Add(Me.ToolStrip2)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Spell Check"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ToolStrip3.ResumeLayout(False)
        Me.ToolStrip3.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tslStatus As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tsbVBForums As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbi00Productions As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip2 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbExamples As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents MenuTextSeperator1 As i00SpellCheck.MenuTextSeperator
    Friend WithEvents tstbAnagramLookup As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents MenuTextSeperator2 As i00SpellCheck.MenuTextSeperator
    Friend WithEvents tstbScrabbleHelper As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tsbAbout As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCodeProject As System.Windows.Forms.ToolStripButton
    Friend WithEvents MenuTextSeperator3 As i00SpellCheck.MenuTextSeperator
    Friend WithEvents tsbSuggestionLookup As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tabSpellControls As System.Windows.Forms.TabControl
    Friend WithEvents CrosswordSolverToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripDropDownButton2 As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents MenuTextSeperator4 As i00SpellCheck.MenuTextSeperator
    Friend WithEvents SpellingErrorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsiCPSpellingError As i00SpellCheck.tsiColorPicker
    Friend WithEvents CaseErrorToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsiCPCaseError As i00SpellCheck.tsiColorPicker
    Friend WithEvents IgnoredWordToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsiCPIgnoreColor As i00SpellCheck.tsiColorPicker
    Friend WithEvents MenuTextSeperator5 As i00SpellCheck.MenuTextSeperator
    Friend WithEvents tsiDrawStyle As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents MenuTextSeperator6 As i00SpellCheck.MenuTextSeperator
    Friend WithEvents ShowErrorsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowIgnoredToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AlwaysToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NeverToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WhenCtrlIsPressedToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbSpellCheck As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbProperties As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStrip3 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsiEnabled As System.Windows.Forms.ToolStripButton
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents tsbDonate As System.Windows.Forms.ToolStripButton
    Friend WithEvents ilTabSpellControls As System.Windows.Forms.ImageList
    Friend WithEvents tsbEditDictionary As System.Windows.Forms.ToolStripButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AsdToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsbPerformanceMonitor As System.Windows.Forms.ToolStripButton

End Class
