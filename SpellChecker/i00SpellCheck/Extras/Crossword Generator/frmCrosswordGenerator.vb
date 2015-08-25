'i00 .Net Spell Check - Crossword Generator
'©i00 Productions All rights reserved
'Created by Kris Bennett
'----------------------------------------------------------------------------------------------------
'All property in this file is and remains the property of i00 Productions, regardless of its usage,
'unless stated otherwise in writing from i00 Productions.
'
'Anyone wishing to use this code in their projects may do so, however are required to leave a post on
'VBForums (under: http://www.vbforums.com/showthread.php?p=4075093) stating that they are doing so.
'A simple "I am using i00 Spell check in my project" will surffice.
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Public Class frmCrosswordGenerator

    Private Sub bpBackGround_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles bpBackGround.Paint
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Using gb As New Drawing2D.LinearGradientBrush(New Point(0, 0), New Point(0, bpBackGround.ClientSize.Height), Color.Transparent, i00SpellCheck.DrawingFunctions.AlphaColor(Color.Black, 31))
            e.Graphics.FillRectangle(gb, bpBackGround.ClientRectangle)
        End Using
        'e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.High
        'e.Graphics.DrawImage(My.Resources.Icon, New Point(CInt((bpLogo.ClientSize.Width - My.Resources.Icon.Width) / 2), CInt((bpLogo.ClientSize.Height - My.Resources.Icon.Height) / 2)))
        Using b As New SolidBrush(Color.FromArgb(255, 120, 154, 255))
            'Dim LogoSize As Single = Me.ClientSize.Width
            Dim LogoSize = CSng(Me.ClientSize.Height * 1.25)
            Dim LeftPos = CSng(Me.ClientSize.Width - (LogoSize * 0.75))
            If LeftPos < -(LogoSize * 0.25) Then LeftPos = CSng(-(LogoSize * 0.25))
            i00SpellCheck.DrawingFunctions.DrawLogo(e.Graphics, b, New RectangleF(LeftPos, CSng((bpBackGround.ClientSize.Height - LogoSize) / 2), LogoSize, LogoSize))
        End Using
    End Sub

    Private Sub frmCrosswordGenerator_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If MT_GenerateXWord IsNot Nothing AndAlso MT_GenerateXWord.IsAlive Then
            MT_GenerateXWord.Abort()
        End If
    End Sub

    Private Sub frmCrosswordGenerator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim BottomHolderSize = bpBottomHolder.Size
        bpBottomHolder.AutoSize = False
        bpBottomHolder.Size = BottomHolderSize

        Dim item As New ToolStripControlHost(gcXWordSize)
        tsTop.Items.Add(item)

        tsTop.Renderer = New TransparentToolStripRender

        Dim ControlColor = i00SpellCheck.DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.AppWorkspace), 127)
        bpCorsswordBoard.BackColor = ControlColor

        Dim ControlColorBot = i00SpellCheck.DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.Control), 127)
        tsTop.BackColor = ControlColorBot
        bpBottom.BackColor = ControlColorBot
        bpStop.BackColor = ControlColorBot

        cboDict.Items.Add("Default (definitions only)")
        cboDict.Items.Add("Default")
        cboDict.Items.Add("Custom")
        cboDict.SelectedIndex = 0

        txtCustomDict.SpellCheck.Settings.ShowMistakes = False
    End Sub

    Private WithEvents CrossWordGenerator As New CrossWordGenerator

    Private Sub CreateXWord()
        CrossWordGenerator.CrossWordSize = New Size(gcXWordSize.SelectedCell.X, gcXWordSize.SelectedCell.Y)
        bpCorsswordBoard.Invalidate()
    End Sub

    Private Sub bpCorsswordBoard_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles bpCorsswordBoard.Paint
        Try
            Dim XWordDraw = New CrossWordGenerator.CrossWordDrawingFunctions(CrossWordGenerator)
            Dim XWordSize = XWordDraw.BoardSize
            Dim DrawRect = DrawingFunctions.GetBestFitRect(bpCorsswordBoard.ClientRectangle, New Rectangle(New Point(0, 0), XWordSize), DrawingFunctions.BestFitStyle.Stretch)

            e.Graphics.TranslateTransform(DrawRect.X, DrawRect.Y)
            e.Graphics.ScaleTransform(DrawRect.Width / XWordSize.Width, DrawRect.Height / XWordSize.Height)

            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

            ''paint on canvas 1st then output to make clip regions "nicer"
            'Using b As New Bitmap(XWordSize.Width, XWordSize.Height)
            '    Using g = Graphics.FromImage(b)
            '        XWordDraw.Draw(g)
            '    End Using
            '    e.Graphics.DrawImage(b, 0, 0)
            'End Using
            XWordDraw.Draw(e.Graphics)

            e.Graphics.ResetTransform()

            'draw loading
            If bpLockableContent.Enabled = False Then
                Using f As New Font(Drawing.SystemFonts.DefaultFont.Name, 20, Drawing.SystemFonts.DefaultFont.Style)
                    Dim FontSize = e.Graphics.MeasureString("Loading...", f)
                    Dim FontLocation As New PointF((bpCorsswordBoard.ClientRectangle.Width - FontSize.Width) / 2, (bpCorsswordBoard.ClientRectangle.Height - FontSize.Height) / 2)
                    Using b As New SolidBrush(DrawingFunctions.AlphaColor(Color.Black, 127))
                        e.Graphics.FillRectangle(b, bpCorsswordBoard.ClientRectangle)
                        'make the loading bg darker still
                        e.Graphics.FillRectangle(b, New RectangleF(FontLocation, FontSize))
                    End Using
                    e.Graphics.DrawString("Loading...", f, Brushes.White, FontLocation)
                End Using
            End If

        Catch ex As Exception

        End Try
    End Sub

    Dim MT_GenerateXWord As Threading.Thread

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Dim dict As FlatFileDictionary
        Select Case cboDict.SelectedItem.ToString
            Case "Default (definitions only)"
                dict = New FlatFileDictionary
                For Each item In (From xItem In Definitions.DefaultDefinitions.GetWordList).ToArray
                    dict.IndexedDictionary.Add(item)
                Next
            Case "Default"
                dict = TryCast(Dictionary.DefaultDictionary, FlatFileDictionary)
            Case Else 'custom
                dict = New FlatFileDictionary
                For Each item In (From xItem In System.Text.RegularExpressions.Regex.Matches(txtCustomDict.Text, "(?<=\b)\w+?(?=\b)").OfType(Of System.Text.RegularExpressions.Match)() Where xItem.Value <> "" Select xItem.Value).ToArray
                    dict.IndexedDictionary.Add(item)
                Next
        End Select

        If dict Is Nothing Then
            'dict not loaded yet
            FlatFileDictionary.LoadDefaultDictionary()
            dict = DirectCast(Dictionary.DefaultDictionary, FlatFileDictionary)
        End If

        If MT_GenerateXWord IsNot Nothing AndAlso MT_GenerateXWord.IsAlive Then
            MT_GenerateXWord.Abort()
        End If

        LVComboBox.Visible = False
        lstWords.Items.Clear()

        txtCustomDict.HighlightWords.Clear()
        ProgressBar1.Value = 0
        MT_GenerateXWord = New Threading.Thread(AddressOf GenerateXWord)
        MT_GenerateXWord.IsBackground = True
        MT_GenerateXWord.Name = "Generating crossword"
        MT_GenerateXWord.Start(dict)
    End Sub

    Public Sub GenerateXWord(ByVal dict As Object)
        EnableDisableControls(True)
        CrossWordGenerator.Generate(DirectCast(dict, FlatFileDictionary))
        EnableDisableControls(False)
        RefreshHighlights()
        ResizeColumns()
    End Sub

    Private Sub EnableDisableControls(ByVal Lock As Boolean)
        bpStop.SafeInvoke(Function(t As BufferedPanel) InlineAssignHelper(t.Visible, Lock))
        bpBottom.SafeInvoke(Function(t As BufferedPanel) InlineAssignHelper(t.Visible, Not Lock))
        'bpLockableContent.SafeInvoke(Function(t As BufferedPanel) InlineAssignHelper(t.Cursor, If(Lock, Cursors.WaitCursor, Cursors.Default)))
        bpLockableContent.SafeInvoke(Function(t As BufferedPanel) InlineAssignHelper(t.Enabled, Not Lock))
    End Sub

    Private Sub gcXWordSize_SelectedCellChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gcXWordSize.SelectedCellChanged
        CreateXWord()
    End Sub

    Private Sub cboDict_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDict.SelectedIndexChanged
        txtCustomDict.Visible = cboDict.SelectedItem.ToString.ToLower.Contains("default") = False
    End Sub

    Private Sub CrossWordGenerator_FilledCellsWithWord(ByVal sender As Object, ByVal e As CrossWordGenerator.WordLocationEventArgs) Handles CrossWordGenerator.FilledCellsWithWord

        Static LastDrawTime As Integer
        Dim ThisDrawTime = Environment.TickCount
        If ThisDrawTime > LastDrawTime + 1000 Then
            bpCorsswordBoard.Invalidate()
            LastDrawTime = ThisDrawTime
        End If
        txtCustomDict.HighlightWords.Add(e.Word)

        AddWordToList(sender, e)
    End Sub

    Private Delegate Sub cb_AddWordToList(ByVal sender As Object, ByVal e As CrossWordGenerator.WordLocationEventArgs)
    Private Sub AddWordToList(ByVal sender As Object, ByVal e As CrossWordGenerator.WordLocationEventArgs)
        If Me.InvokeRequired Then
            Dim cb_AddWordToList As New cb_AddWordToList(AddressOf AddWordToList)
            Me.Invoke(cb_AddWordToList, sender, e)
        Else
            'If cboDict.SelectedItem.ToString.ToLower.Contains("default") Then
            'load the word to the grid
            Dim lvi = lstWords.Items.Add("")
            lvi.Group = lstWords.Groups(If(e.Across, "Across", "Down"))

            lvi.SubItems.Add(e.Word)
            'find the definition
            Dim WordDefs As New List(Of String)
            'qwertyuiop - thread this?
            For Each item In Definitions.DefaultDefinitions.FindWord(e.Word)
                WordDefs.AddRange(item.GetDefs)
            Next
            If WordDefs.Count > 0 Then
                'pick one of the definitions...
                '...pref one without the word in the definition itself...
                Randomize()
                Dim arrSplitString = New String() {" - "}
                Dim defList = (From xItem In WordDefs Select xItem.Split(arrSplitString, 2, StringSplitOptions.None)(0))
                Dim SelectedDef = (From xItem In defList Order By xItem.ToLower.Contains(e.Word.ToLower), ReplaceDefWordWithLine(xItem, e.Word) <> xItem, Rnd()).FirstOrDefault
                If SelectedDef <> "" Then
                    lvi.SubItems.Add(ReplaceDefWordWithLine(SelectedDef, e.Word))
                End If
            Else
                lvi.SubItems.Add("")
            End If

        End If
    End Sub

    Private Function ReplaceDefWordWithLine(ByVal Def As String, ByVal Word As String) As String
        Word = Join((From xItem In Word Where Char.IsLetterOrDigit(xItem) OrElse xItem = " "c Select CStr(xItem)).ToArray, "")
        Return System.Text.RegularExpressions.Regex.Replace(Def, "\b" & Word & "\b", "____", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    End Function

    Private Sub ResizeColumns()
        If lstWords.InvokeRequired Then
            lstWords.Invoke(New Action(AddressOf ResizeColumns))
        Else
            For Each item In lstWords.Columns.OfType(Of ColumnHeader)()
                lstWords.Columns(lstWords.Columns.IndexOf(item)).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            Next
        End If
    End Sub

    Private Sub RefreshHighlights()
        If txtCustomDict.InvokeRequired Then
            txtCustomDict.Invoke(New Action(AddressOf RefreshHighlights))
        Else
            txtCustomDict.UpdateHighlights()
        End If
    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click
        If MT_GenerateXWord IsNot Nothing AndAlso MT_GenerateXWord.IsAlive Then
            MT_GenerateXWord.Abort()
        End If
        EnableDisableControls(False)
    End Sub

    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Using sfd As New SaveFileDialog
            sfd.Filter = "Crossword Files (*.crossword.txt)|*.crossword.txt|All Files (*.*)|*.*"
            If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                My.Computer.FileSystem.WriteAllText(sfd.FileName, CrossWordGenerator.ToString, False)
            End If
        End Using
    End Sub

    Private Sub tsbLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbLoad.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "Crossword Files (*.crossword.txt)|*.crossword.txt|All Files (*.*)|*.*"
            If ofd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                CrossWordGenerator.FromString(My.Computer.FileSystem.ReadAllText(ofd.FileName))
                gcXWordSize.SelectedCell = New Point(CrossWordGenerator.CrossWordSize)
                bpCorsswordBoard.Invalidate()
            End If
        End Using
    End Sub

    Private Sub CrossWordGenerator_Progress(ByVal sender As Object, ByVal e As CrossWordGenerator.ProgressEventArgs) Handles CrossWordGenerator.Progress
        ProgressBar1.SafeInvoke(Function(t As ProgressBar) InlineAssignHelper(t.Value, CInt(e.Progress * 100)))
    End Sub

    Private Sub txtCustomDict_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCustomDict.TextChanged
        txtCustomDict.ClearHighlights()
    End Sub

#Region "Word List"

    Private WithEvents LVComboBox As New ComboBox

    Private Sub lstWords_ColumnWidthChanging(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnWidthChangingEventArgs) Handles lstWords.ColumnWidthChanging
        UpdateLVControlLocation()
    End Sub

    Private Sub UpdateLVControlLocation(Optional ByVal ForceLocationChange As Boolean = False)
        If LVComboBox.Parent IsNot Nothing AndAlso (LVComboBox.Visible OrElse ForceLocationChange) Then
            Dim ThisItem = lstWords.Items(SelectedLVCell.Y).SubItems(SelectedLVCell.X)
            Dim Bounds = ThisItem.Bounds
            If Bounds.X < 0 Then Bounds.X = 0
            If Bounds.Right > lstWords.ClientRectangle.Width Then
                Bounds.Width = lstWords.ClientRectangle.Width - Bounds.Left
            End If
            LVComboBox.Bounds = Bounds

            If LVComboBox.Width < 400 Then
                LVComboBox.DropDownWidth = 400
            Else
                LVComboBox.DropDownWidth = LVComboBox.Width
            End If
        End If
    End Sub

    Dim SelectedLVCell As Point

    Private Sub lstWords_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstWords.MouseClick
        Dim ListViewItem = lstWords.GetItemAt(e.Location.X, e.Location.Y)
        Dim XOffset = 0

        Dim arrSplitString = New String() {" - "}
        For Each item In lstWords.Columns.OfType(Of ColumnHeader)()
            If e.X > XOffset AndAlso e.X < XOffset + item.Width Then
                SelectedLVCell = New Point(lstWords.Columns.IndexOf(item), lstWords.Items.IndexOf(ListViewItem))
                Dim ThisCol = lstWords.Columns(SelectedLVCell.X)
                Dim ThisItem = ListViewItem.SubItems(SelectedLVCell.X)
                If ThisCol Is colQuestion Then
                    LVComboBox.Parent = lstWords
                    LVComboBox.Font = ThisItem.Font
                    LVComboBox.ForeColor = ThisItem.ForeColor
                    UpdateLVControlLocation(True)
                    LVComboBox.Items.Clear()
                    LVComboBox.Text = ThisItem.Text
                    LVComboBox.SelectAll()
                    Dim ThisWord = ListViewItem.SubItems(colWord.Index).Text
                    Dim WordDefs As New List(Of String)
                    For Each iDef In Definitions.DefaultDefinitions.FindWord(ThisWord)
                        WordDefs.AddRange(iDef.GetDefs)
                    Next
                    LVComboBox.Items.AddRange((From xItem In WordDefs Select ReplaceDefWordWithLine(xItem.Split(arrSplitString, 2, StringSplitOptions.None)(0), ThisWord)).ToArray)
                    LVComboBox.Visible = True
                    LVComboBox.Focus()
                End If
                Exit Sub
            End If
            XOffset += item.Width
        Next
    End Sub

    Private Sub lstWords_Scroll(ByVal sebder As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles lstWords.Scroll
        UpdateLVControlLocation()
    End Sub

    Private Sub LVComboBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles LVComboBox.LostFocus
        'update and hide
        Dim ThisItem = lstWords.Items(SelectedLVCell.Y).SubItems(SelectedLVCell.X)
        ThisItem.Text = LVComboBox.Text
        LVComboBox.Visible = False
    End Sub

    Private Sub lstWords_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstWords.SizeChanged
        UpdateLVControlLocation()
    End Sub

#End Region

End Class