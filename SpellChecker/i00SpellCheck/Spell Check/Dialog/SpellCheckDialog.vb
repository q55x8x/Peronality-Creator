'i00 .Net Spell Check Dialog
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

Public Class SpellCheckDialog

    Dim SpellCheckControlBase As SpellCheckControlBase

#Region "Menu"

    Private WithEvents SpellMenuItems As New Menu.AddSpellItemsToMenu()

    Private Sub cmsHTMLSpellCheck_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles cmsHTMLSpellCheck.Closed
        SpellMenuItems.RemoveSpellMenuItems()
    End Sub

    Dim MenuCurrentWord As HTMLSpellCheck.SpellCheckDialogWords

    Private Sub cmsHTMLSpellCheck_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmsHTMLSpellCheck.LocationChanged
        If HtmlSpellCheck1.Document Is Nothing Then Return

        Static LocationChanging As Boolean
        If LocationChanging Then Exit Sub
        LocationChanging = True

        Dim Element = HtmlSpellCheck1.Document.GetElementById(HtmlSpellCheck1.Words.IndexOf(MenuCurrentWord).ToString)
        If Element IsNot Nothing Then
            Dim OffsetPos = HtmlSpellCheck1.PointToScreen(Point.Empty)
            Dim OffsetX As Integer = Element.OffsetRectangle.Left - HtmlSpellCheck1.Document.Body.ScrollLeft
            Dim OffsetY As Integer = Element.OffsetRectangle.Bottom - HtmlSpellCheck1.Document.Body.ScrollTop

            cmsHTMLSpellCheck.Top = OffsetPos.Y + OffsetY
            cmsHTMLSpellCheck.Left = OffsetPos.X + OffsetX
        End If


        LocationChanging = False
    End Sub

    Dim OpenedByClick As Boolean = True

    Private Sub cmsHTMLSpellCheck_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmsHTMLSpellCheck.Opening
        If OpenedByClick Then
            Dim pCursor = Cursor.Position
            Dim pHtml = HtmlSpellCheck1.PointToScreen(New Point(0, 0))
            pCursor.X = pCursor.X - pHtml.X
            pCursor.Y = pCursor.Y - pHtml.Y

            Dim element = HtmlSpellCheck1.Document.GetElementFromPoint(pCursor)
            If element IsNot Nothing AndAlso element.Id <> "" Then
                Dim ElementNo As Integer
                If IsNumeric(Integer.TryParse(element.Id, ElementNo)) AndAlso HtmlSpellCheck1.Words.Count > ElementNo Then
                    MenuCurrentWord = HtmlSpellCheck1.Words.Item(ElementNo)
                    UpdateMenuItems()
                Else
                    e.Cancel = True
                End If
            Else
                e.Cancel = True
            End If
        Else
            OpenedByClick = True
        End If
    End Sub

    Private Sub SpellMenuItems_WordAdded(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordAdded
        AddWordToDict(e.Word, MenuCurrentWord)
    End Sub

    Private Sub SpellMenuItems_WordUnIgnored(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordUnIgnored
        UnIgnoreWord(MenuCurrentWord)
    End Sub

    Private Sub UnIgnoreWord(ByVal theSelectedWord As HTMLSpellCheck.SpellCheckDialogWords)
        If theSelectedWord IsNot Nothing Then
            Dim theWord = theSelectedWord.OrigWord
            SpellCheckControlBase.DictionaryUnIgnoreWord(theWord)

            theSelectedWord.Changed = True
            theSelectedWord.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK

            For Each item In (From xItem In HtmlSpellCheck1.Words Where xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case).ToArray
                item.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Pending
            Next
            HtmlSpellCheck1.StartSpellCheck()

            btnSkip_Click(btnSkip, EventArgs.Empty)
        End If
    End Sub

    Private Sub SpellMenuItems_WordIgnored(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordIgnored
        IgnoreWord(MenuCurrentWord)
    End Sub

    Private Sub SpellMenuItems_WordRemoved(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordRemoved
        Dim NoApoS = Dictionary.Formatting.RemoveApoS(e.Word)
        Try
            SpellCheckControlBase.DictionaryRemoveWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error occured removing """ & e.Word & """ from the dictionary:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
        WordUpdatedFromMenu()

        For Each item In (From xItem In HtmlSpellCheck1.Words Where InStr(xItem.NewWord, NoApoS, CompareMethod.Text) > 0 AndAlso Not (xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case)).ToArray
            item.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Pending
        Next
        HtmlSpellCheck1.StartSpellCheck()
    End Sub

    Private Sub SpellMenuItems_WordChanged(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordChanged
        MenuCurrentWord.NewWord = e.Word
        MenuCurrentWord.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK
        WordUpdatedFromMenu()
    End Sub

    Private Sub tsiRevertTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsiRevertTo.Click
        MenuCurrentWord.NewWord = MenuCurrentWord.OrigWord
        MenuCurrentWord.Changed = False
        MenuCurrentWord.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Pending
        HtmlSpellCheck1.StartSpellCheck()
        WordUpdatedFromMenu()
    End Sub

    Private Sub tstbChangeTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles tstbChangeTo.KeyPress
        If e.KeyChar = vbCr Then
            MenuCurrentWord.NewWord = tstbChangeTo.Text
            'assume this word is correct since it is user-entered...
            MenuCurrentWord.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK
            UpdateMenuItems()
            WordUpdatedFromMenu()
        End If
    End Sub

    Private Sub WordUpdatedFromMenu()
        If SelectedWord() Is MenuCurrentWord Then
            HtmlSpellCheck1_SelectionChanged(HtmlSpellCheck1, New HTMLSpellCheck.HTMLWordEventArgs() With {.Word = MenuCurrentWord})
        End If
    End Sub

    Private Sub UpdateMenuItems()
        If Replace(Dictionary.Formatting.RemoveWordBreaks(MenuCurrentWord.NewWord), " ", "") = MenuCurrentWord.NewWord Then
            SpellMenuItems.ContextMenuStrip = cmsHTMLSpellCheck
            SpellMenuItems.RemoveSpellMenuItems()
            SpellMenuItems.AddItems(MenuCurrentWord.NewWord, SpellCheckControlBase.CurrentDictionary, SpellCheckControlBase.CurrentDefinitions, SpellCheckControlBase.CurrentSynonyms, SpellCheckControlBase.Settings)
        End If
        mtsChangeTo.Text = "Change " & MenuCurrentWord.NewWord & " to:"
        tstbChangeTo.Text = MenuCurrentWord.NewWord
        tsiRevertTo.Text = "Revert to " & MenuCurrentWord.OrigWord
        tsiRevertTo.Visible = MenuCurrentWord.Changed
        'hrm... had to put this in as the location did not get updated the first go otherwise???
        cmsHTMLSpellCheck_LocationChanged(cmsHTMLSpellCheck, EventArgs.Empty)
    End Sub

#End Region

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        For Each ctl In pnlBottomContent.Controls.OfType(Of Control)()
            ctl.Visible = False
            ctl.Dock = DockStyle.Fill
        Next
        ShowHideSuggestions(False)
        pnlPleaseWait.SendToBack()
        pnlPleaseWait.Visible = True

        Dim ShortcutKeyColor = System.Drawing.ColorTranslator.ToHtml(DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.ControlText), Color.FromKnownColor(KnownColor.Control)))
        btnAdd.Tag = "Adds the word to the dictionary"
        btnIgnore.Tag = "Ignores the word and continues"
        btnSkip.Tag = "Skips the current correction and goes to the next one <i><font color=" & ShortcutKeyColor & ">&lt;F7&gt;</font></i>"
        btnRevert.Tag = "Reverts the changes for the selected word <i><font color=" & ShortcutKeyColor & ">&lt;Ctrl + Z&gt;</font></i>"

        btnChange.Tag = "Changes the word to the text in ""Change to"" <i><font color=" & ShortcutKeyColor & ">&lt;Enter&gt;</font></i>"
        btnChangeAll.Tag = "Changes all misspelled words to the first suggestion <i><font color=" & ShortcutKeyColor & ">&lt;Shift + Enter&gt;</font></i>"
        btnRevertAll.Tag = "Reverts all words to their original values<i><font color=" & ShortcutKeyColor & ">&lt;Ctrl + Shift + Z&gt;</font></i>"
        btnClose.Tag = "Closes the spell check and saves all changes<i><font color=" & ShortcutKeyColor & ">&lt;Esc&gt;</font></i>"

    End Sub

    Public Overloads Function ShowDialog(ByVal owner As IWin32Window, ByVal SpellCheckControlBase As SpellCheckControlBase, ByVal Text As String) As List(Of HTMLSpellCheck.SpellCheckDialogWords)
        Me.SpellCheckControlBase = SpellCheckControlBase
        HtmlSpellCheck1.Dictionary = SpellCheckControlBase.CurrentDictionary
        HtmlSpellCheck1.SetText(Text)
        pbChangeAll.Maximum = HtmlSpellCheck1.Words.Count - 1
        MyBase.StartPosition = FormStartPosition.CenterParent
        MyBase.ShowDialog(owner)
        Return HtmlSpellCheck1.Words
    End Function

    Private Sub bpIcon_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles bpIcon.Paint
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.High
        e.Graphics.DrawImage(My.Resources.Icon, New Rectangle(0, 0, 32, 32))
    End Sub

    Private Sub HtmlSpellCheck1_WordSpellChecked(ByVal sender As Object, ByVal e As HTMLSpellCheck.HTMLWordEventArgs) Handles HtmlSpellCheck1.WordSpellChecked

        If pbChangeAll.Visible Then
            Dim Progress As Integer = HtmlSpellCheck1.Words.IndexOf(e.Word)
            pbChangeAll.SafeInvoke(Function(x As ProgressBar) InlineAssignHelper(x.Value, Progress))
        End If

        pnlPleaseWait.SafeInvoke(Function(x As Panel) InlineAssignHelper(x.Visible, True))
        If e.Word.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error OrElse e.Word.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case Then
            If ChangeingAll Then
                Dim Suggestions = GetSuggestions(e.Word.NewWord)
                If Suggestions.Count > 0 Then
                    'change to first
                    e.Word.NewWord = Suggestions.First
                    e.Word.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK
                    If SelectedWord() Is e.Word Then
                        HtmlSpellCheck1_SelectionChanged(HtmlSpellCheck1, New HTMLSpellCheck.HTMLWordEventArgs() With {.Word = e.Word})
                    End If
                End If
                Exit Sub
            End If
            If pnlSuggestions.Visible = False Then
                'select this word :)
                e.Word.Selected = True
            End If
            ShowHideSuggestions(pnlSuggestions.Visible)

            'If btnSkip.Enabled = False Then
            '    Dim SelectedWord = Me.SelectedWord()
            '    Dim btnSkipEnabled = (From xItem In HtmlSpellCheck1.Words Where (xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case) AndAlso xItem IsNot SelectedWord).Count > 0
            '    If btnSkip.Enabled <> btnSkipEnabled Then
            '        btnSkip.SafeInvoke(Function(x As Button) InlineAssignHelper(x.Enabled, btnSkipEnabled))
            '    End If
            'End If
        End If
    End Sub

    Private Delegate Sub HtmlSpellCheck1_SelectionChanged_cb(ByVal sender As Object, ByVal e As HTMLSpellCheck.HTMLWordEventArgs)
    Private Sub HtmlSpellCheck1_SelectionChanged(ByVal sender As Object, ByVal e As HTMLSpellCheck.HTMLWordEventArgs) Handles HtmlSpellCheck1.SelectionChanged
        If pnlSuggestions.InvokeRequired Then
            Dim HtmlSpellCheck1_SelectionChanged_cb As New HtmlSpellCheck1_SelectionChanged_cb(AddressOf HtmlSpellCheck1_SelectionChanged)
            pnlSuggestions.Invoke(HtmlSpellCheck1_SelectionChanged_cb, sender, e)
        Else
            FillSuggestions()
            ShowHideSuggestions(True)
            txtChangeTo.Focus()
            ChangeToUseOldWord = True
            txtChangeTo_TextChanged(txtChangeTo, EventArgs.Empty)
            ChangeToChanged = False
        End If
    End Sub

    Dim ChangeToChanged As Boolean

    Private Function SelectedWord() As HTMLSpellCheck.SpellCheckDialogWords
        Return (From xItem In HtmlSpellCheck1.Words Where xItem.Selected).FirstOrDefault
    End Function

    Private Function GetSuggestions(ByVal theWord As String) As List(Of String)
        Dim Ucase1stLetter As Boolean
        If theWord.Length > 0 AndAlso Char.IsUpper(theWord(0)) Then
            'upper case - so lets make suggestions 1st letter in ucase :)
            Ucase1stLetter = True
        End If
        Dim Suggestions = SpellCheckControlBase.CurrentDictionary.SpellCheckSuggestions(theWord)
        If Suggestions Is Nothing OrElse Suggestions.Count = 0 Then
            Return New List(Of String)
        End If
        Dim TopCloseness = Suggestions.Max(Function(x As i00SpellCheck.Dictionary.SpellCheckSuggestionInfo) x.Closness)
        Dim FilteredSuggestions = (From xItem In Suggestions Order By xItem.Closness Descending, xItem.Word Ascending Where xItem.Closness >= TopCloseness * 0.75 Select xItem.Word).ToArray
        FilteredSuggestions = (From xItem In FilteredSuggestions Where Array.IndexOf(FilteredSuggestions, xItem) < 15).ToArray
        Dim lstSuggestions As New List(Of String)
        If Ucase1stLetter Then
            For Each item In FilteredSuggestions
                Dim arrWord = item.ToArray
                arrWord(0) = Char.ToUpper(arrWord(0))
                lstSuggestions.Add(CStr(arrWord))
            Next
        Else
            lstSuggestions.AddRange(FilteredSuggestions)
        End If
        Return lstSuggestions
    End Function

    Private Sub FillSuggestions()
        ChangeToChangedTrigger = False
        lvSuggestions.Groups.Clear()
        lvSuggestions.Items.Clear()
        Dim theSelectedWord = SelectedWord()
        If theSelectedWord IsNot Nothing Then
            txtChangeTo.Text = theSelectedWord.NewWord
            If Replace(Dictionary.Formatting.RemoveWordBreaks(theSelectedWord.NewWord), " ", "") = theSelectedWord.NewWord Then
                'we have a revert button so removed this from the list
                'If theSelectedWord.NewWord <> theSelectedWord.OrigWord Then
                '    Dim RevertGroup = New ListViewGroup("Revert", "Revert")
                '    lvSuggestions.Groups.Add(RevertGroup)
                '    lvSuggestions.Items.Add(theSelectedWord.OrigWord).Group = RevertGroup
                'End If
                Select Case SpellCheckControlBase.CurrentDictionary.SpellCheckWord(theSelectedWord.NewWord)
                    Case i00SpellCheck.Dictionary.SpellCheckWordError.SpellError, i00SpellCheck.Dictionary.SpellCheckWordError.CaseError
                        'add Suggestions...
                        If theSelectedWord.Changed = False Then
                            Dim KeepAsGroup = New ListViewGroup("Keep", "Keep as:")
                            lvSuggestions.Groups.Add(KeepAsGroup)
                            lvSuggestions.Items.Add(theSelectedWord.OrigWord).Group = KeepAsGroup
                        End If

                        Dim Suggestions = GetSuggestions(theSelectedWord.NewWord)
                        If Suggestions.Count > 0 Then
                            Dim Group = New ListViewGroup("Suggestions", "Suggestions")
                            lvSuggestions.Groups.Add(Group)
                            lvSuggestions.Items.AddRange((From xItem In Suggestions Select New ListViewItem(xItem) With {.Group = Group}).ToArray)
                            Group.Items(0).Selected = True
                            'selecting the item with the line above also does :):
                            'txtChangeTo.Text = FilteredSuggestions(0)
                        End If
                    Case i00SpellCheck.Dictionary.SpellCheckWordError.OK
                        'add synonyms
                        If SpellCheckControlBase.Settings.AllowChangeTo Then
                            Dim MatchedSynonyms = SpellCheckControlBase.CurrentSynonyms.FindWord(theSelectedWord.NewWord)
                            If MatchedSynonyms IsNot Nothing Then
                                'Add change to...
                                For Each item In MatchedSynonyms
                                    Dim GroupName = item.TypeDescription & If(item.WordType <> i00SpellCheck.Synonyms.FindWordReturn.WordTypes.Other, " (" & item.WordType.ToString & ")", "")
                                    Dim Group = New ListViewGroup(MatchedSynonyms.IndexOf(item).ToString, GroupName)
                                    lvSuggestions.Groups.Add(Group)
                                    lvSuggestions.Items.AddRange((From xItem In item Select New ListViewItem(xItem) With {.Group = Group}).ToArray)
                                Next
                            End If
                        End If
                End Select

                lvSuggestions.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If
        End If
        ChangeToChangedTrigger = True
    End Sub

    Private Function MoveToNextWordError(Optional ByVal DoRecheck As Boolean = True) As Boolean
        'find the selected word
        Dim StartIndex = 0
        Dim theSelectedWord = SelectedWord()
        If theSelectedWord IsNot Nothing Then
            StartIndex = HtmlSpellCheck1.Words.IndexOf(theSelectedWord) + 1
        End If
ReCheck:
        For i = StartIndex To HtmlSpellCheck1.Words.Count - 1
            Select Case HtmlSpellCheck1.Words(i).SpellCheckState
                Case HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error, HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case
                    'select
                    HtmlSpellCheck1.Words(i).Selected = True
                    MoveToNextWordError = True
                    Exit Function
            End Select
        Next
        'if we got here ... start over?
        If DoRecheck Then
            If StartIndex = 0 Then Exit Function 'already checked from the start
            StartIndex = 0
            GoTo ReCheck
        End If
    End Function

    Private Delegate Sub btnSkip_Click_cb(ByVal sender As System.Object, ByVal e As System.EventArgs)
    Private Sub btnSkip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSkip.Click
        If btnSkip.InvokeRequired Then
            Dim btnSkip_Click_cb As New btnSkip_Click_cb(AddressOf btnSkip_Click)
            btnSkip.Invoke(btnSkip_Click_cb, sender, e)
        Else
            If MoveToNextWordError(False) = False Then
                If HtmlSpellCheck1.mt_SpellCheck IsNot Nothing AndAlso HtmlSpellCheck1.mt_SpellCheck.IsAlive Then
                    'we are still spell checking... so wait to move next
                    Dim theSelectedWord = SelectedWord()
                    If theSelectedWord IsNot Nothing Then
                        theSelectedWord.Selected = False
                    End If
                    ShowHideSuggestions(False)
                Else
                    If MoveToNextWordError() Then
                        'found a word... 
                    Else
                        'all correct :)
                        CompleteSpellCheck()
                    End If
                End If
            End If
        End If
    End Sub

    Public WithEvents CompleteTooltip As New HTMLToolTip
    Private Delegate Sub CompleteSpellCheck_cb()
    Private Sub CompleteSpellCheck()
        If Me.InvokeRequired Then
            Dim CompleteSpellCheck_cb As New CompleteSpellCheck_cb(AddressOf CompleteSpellCheck)
            Me.Invoke(CompleteSpellCheck_cb)
        Else
            Try 'Doesn't really matter if it fails ... and some objects = nothing if the form is closed

                'remove the opening animation
                If (Me.AnimationEnabled And AnimationState.Opening) = AnimationState.Opening Then
                    Me.AnimationEnabled = DirectCast(Me.AnimationEnabled - AnimationState.Opening, AnimationState)
                End If
                If Me.WindowAnimationState = AnimationState.Opening Then
                    Me.StopAnimation()
                    Application.DoEvents()
                End If

                btnClose.Focus()
                pnlPleaseWait.Visible = False
                'MsgBox("Spell check is complete")
                My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Exclamation)
                CompleteTooltip.Dispose()
                CompleteTooltip = New HTMLToolTip With {.IsBalloon = True, .ToolTipTitle = "Spell check is complete", .ToolTipIcon = ToolTipIcon.Info}
                CompleteTooltip.ShowHTML("Click close to confirm changes", btnClose, New Point(CInt(btnClose.Width / 2), CInt(btnClose.Height / 2)), 5000)
            Catch ex As Exception

            End Try
         End If
    End Sub

    Private Sub HtmlSpellCheck1_SpellCheckComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles HtmlSpellCheck1.SpellCheckComplete
        'see if there is any words with errors...
        If pbChangeAll.Visible Then pbChangeAll.SafeInvoke(Function(x As ProgressBar) InlineAssignHelper(x.Visible, False))
        If ChangeingAll Then
            ChangeingAll = False
            ShowHideSuggestions(pnlSuggestions.Visible)
        End If
        If SelectedWord() Is Nothing AndAlso MoveToNextWordError() = False Then
            CompleteSpellCheck()
        End If
    End Sub

    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Dim theSelectedWord = SelectedWord()
        If theSelectedWord IsNot Nothing Then
            theSelectedWord.NewWord = txtChangeTo.Text
            theSelectedWord.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK
        End If
        btnSkip_Click(btnSkip, EventArgs.Empty)
        ShowHideSuggestions(pnlSuggestions.Visible)
    End Sub

    Private Delegate Sub ShowHideSuggestions_cb(ByVal Show As Boolean)
    Private Sub ShowHideSuggestions(ByVal Show As Boolean)
        If pnlSuggestions.InvokeRequired Then
            Dim ShowHideSuggestions_cb As New ShowHideSuggestions_cb(AddressOf ShowHideSuggestions)
            pnlSuggestions.Invoke(ShowHideSuggestions_cb, Show)
        Else
            Dim theSelectedWord = SelectedWord()

            pnlSuggestions.Visible = Show

            btnIgnore.Enabled = Show AndAlso theSelectedWord IsNot Nothing AndAlso theSelectedWord.Changed = False AndAlso theSelectedWord.SpellCheckState <> HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK

            btnSkip.Enabled = Show AndAlso (From xItem In HtmlSpellCheck1.Words Where (xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case) AndAlso xItem IsNot theSelectedWord).Count > 0
            btnChange.Enabled = Show
            btnRevert.Enabled = Show AndAlso theSelectedWord IsNot Nothing AndAlso theSelectedWord.Changed = True
            btnChangeAll.Enabled = Not ChangeingAll AndAlso (From xItem In HtmlSpellCheck1.Words Where (xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error)).Count > 0
            btnRevertAll.Enabled = (From xItem In HtmlSpellCheck1.Words Where xItem.Changed = True).Count > 0
        End If

    End Sub

    Private Sub lvSuggestions_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawListViewItemEventArgs) Handles lvSuggestions.DrawItem
        Dim newBounds = New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1)
        Dim Forecolor As Color = e.Item.ForeColor
        Dim Backcolor As Color = e.Item.BackColor
        If e.Item.Selected Then
            Backcolor = DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.Highlight), Backcolor, If(lvSuggestions.Focused, 95, 63))
        End If

        Using b As New SolidBrush(Backcolor)
            e.Graphics.FillRectangle(b, newBounds)
            e.DrawText(TextFormatFlags.VerticalCenter)
        End Using
        If e.Item.Selected Then
            Using p As New Pen(Color.FromKnownColor(KnownColor.Highlight)) 'DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.Highlight), 127))
                e.Graphics.DrawRectangle(p, newBounds)
            End Using
        End If
    End Sub

    Private Sub lvSuggestions_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvSuggestions.MouseDoubleClick
        btnChange_Click(btnChange, EventArgs.Empty)
    End Sub

#Region "Tooltip"

    Dim ToolTip As New HTMLToolTip

    Dim lvItemToolTip As ListViewItem

    Private Sub lvSuggestions_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvSuggestions.MouseLeave
        ToolTip.Hide(lvSuggestions)
    End Sub
    Private Sub lvSuggestions_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lvSuggestions.MouseMove
        Dim LastToolTip = lvItemToolTip
        lvItemToolTip = lvSuggestions.GetItemAt(e.X, e.Y)
        If LastToolTip IsNot lvItemToolTip Then
            If lvItemToolTip IsNot Nothing Then
                'had to create a new one each time ... otherwise it doesn't fade when moving between items
                ToolTip.Dispose()
                ToolTip = New HTMLToolTip
                Dim WordDef = SpellCheckControlBase.CurrentDefinitions.FindWord(lvItemToolTip.Text, SpellCheckControlBase.CurrentDictionary).ToString
                If WordDef <> "" Then
                    ToolTip.ShowHTML(WordDef, lvSuggestions, New Point(lvSuggestions.GetItemRect(lvItemToolTip.Index).Right, lvSuggestions.GetItemRect(lvItemToolTip.Index).Top))
                End If
            Else
                ToolTip.Hide(lvSuggestions)
            End If
        End If
    End Sub

#End Region

    Private Sub lvSuggestions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvSuggestions.SelectedIndexChanged
        If lvSuggestions.SelectedItems.Count > 0 Then
            txtChangeTo.Text = lvSuggestions.SelectedItems(0).Text
            txtChangeTo.SelectAll()
        End If
    End Sub

    Private Sub SpellCheckDialog_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        ToolTip.Dispose()
        'CompleteTooltip.Dispose()
        If mt_ChangeAll IsNot Nothing AndAlso mt_ChangeAll.IsAlive Then
            mt_ChangeAll.Abort()
        End If
    End Sub

    Dim F1Down As Boolean

    Private Sub SpellCheckDialog_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'need to dispose of this here for the window animation to be nice
        CompleteTooltip.Dispose()
        CompleteTooltip = Nothing
    End Sub

    Private Const WM_SYSCOMMAND As Integer = &H112
    Private Const SC_CONTEXTHELP As Integer = &HF180

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Select Case m.Msg
            Case WM_SYSCOMMAND
                Select Case m.WParam.ToInt32
                    Case SC_CONTEXTHELP
                        'close tool tips...
                        For Each item In ControlToolTips.ToArray
                            item.Hide(pnlContent)
                            ControlToolTips.Remove(item)
                            item.Dispose()
                        Next

                        For Each ctl In (From xItem In pnlContent.Controls.OfType(Of Button)() Where xItem.Tag IsNot Nothing AndAlso TypeOf xItem.Tag Is String AndAlso xItem.Tag.ToString <> "" Order By xItem.Top Ascending)
                            'we have a tool tip
                            Dim toolTip As New HTMLToolTip
                            ControlToolTips.Add(toolTip)

                            Dim TipSizeF = HTMLParser.PaintHTML(ctl.Tag.ToString, Nothing, , toolTip.HTMLRenderStatus).Size
                            toolTip.ShowHTML(ctl.Tag.ToString, pnlContent, New Point(ctl.Bounds.Right, CInt(Int(ctl.Bounds.Top + ((ctl.Bounds.Height - TipSizeF.Height) / 2)))), 10000)
                        Next
                        Return
                End Select
        End Select
        MyBase.WndProc(m)
    End Sub

    Private Sub SpellCheckDialog_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.F1
                If F1Down = False Then
                    F1Down = True
                    For Each ctl In (From xItem In pnlContent.Controls.OfType(Of Button)() Where xItem.Tag IsNot Nothing AndAlso TypeOf xItem.Tag Is String AndAlso xItem.Tag.ToString <> "" Order By xItem.Top Ascending)
                        'we have a tool tip
                        Dim toolTip As New HTMLToolTip
                        ControlToolTips.Add(toolTip)

                        Dim TipSizeF = HTMLParser.PaintHTML(ctl.Tag.ToString, Nothing, , toolTip.HTMLRenderStatus).Size
                        toolTip.ShowHTML(ctl.Tag.ToString, pnlContent, New Point(ctl.Bounds.Right, CInt(Int(ctl.Bounds.Top + ((ctl.Bounds.Height - TipSizeF.Height) / 2)))))
                    Next
                End If
            Case Keys.Shift, Keys.ShiftKey
                Me.AcceptButton = btnChangeAll
        End Select
    End Sub

    Private Sub SpellCheckDialog_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress

    End Sub

    Dim ControlToolTips As New List(Of HTMLToolTip)

    Private Sub SpellCheckDialog_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Keys.F7
                If btnSkip.Enabled Then
                    btnSkip_Click(btnSkip, EventArgs.Empty)
                End If
            Case Keys.F1
                For Each item In ControlToolTips.ToArray
                    item.Hide(pnlContent)
                    ControlToolTips.Remove(item)
                    item.Dispose()
                Next
                F1Down = False
            Case Keys.Z
                If e.Control Then
                    If e.Shift Then
                        If btnRevertAll.Enabled Then
                            btnRevertAll_Click(btnRevertAll, EventArgs.Empty)
                        End If
                    Else
                        If btnRevert.Enabled Then
                            btnRevert_Click(btnRevert, EventArgs.Empty)
                        End If
                    End If
                End If
            Case Keys.Shift, Keys.ShiftKey
                Me.AcceptButton = btnChange
        End Select
    End Sub

    Private Sub btnRevert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevert.Click
        Dim theSelectedWord = SelectedWord()
        If theSelectedWord IsNot Nothing Then
            theSelectedWord.NewWord = theSelectedWord.OrigWord
            theSelectedWord.Changed = False
            theSelectedWord.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Pending
            HtmlSpellCheck1.StartSpellCheck()
            HtmlSpellCheck1_SelectionChanged(HtmlSpellCheck1, New HTMLSpellCheck.HTMLWordEventArgs() With {.Word = theSelectedWord})
        End If
    End Sub

    Private Sub btnRevertAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRevertAll.Click
        ChangeingAll = False
        For Each item In (From xItem In HtmlSpellCheck1.Words Where xItem.Changed).ToArray
            item.NewWord = item.OrigWord
            item.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Pending
            item.Changed = False
            If SelectedWord() Is item Then
                HtmlSpellCheck1_SelectionChanged(HtmlSpellCheck1, New HTMLSpellCheck.HTMLWordEventArgs() With {.Word = item})
            End If
        Next
        btnRevertAll.Enabled = False
        ShowHideSuggestions(pnlSuggestions.Visible)

        HtmlSpellCheck1.StartSpellCheck()
    End Sub

    Dim ChangeingAll As Boolean
    Dim mt_ChangeAll As System.Threading.Thread
    Private Sub StartChangeAll()
        btnAdd.Enabled = False
        ChangeingAll = True
        If mt_ChangeAll IsNot Nothing AndAlso mt_ChangeAll.IsAlive Then
            mt_ChangeAll.Abort()
        End If

        Dim SelectedWord = Me.SelectedWord
        If SelectedWord IsNot Nothing Then SelectedWord.Selected = False

        ShowHideSuggestions(False)

        btnChangeAll.Enabled = False
        mt_ChangeAll = New System.Threading.Thread(AddressOf ChangeAll)
        mt_ChangeAll.Name = "Spell Check - Change all"
        mt_ChangeAll.IsBackground = True
        mt_ChangeAll.Start()
    End Sub
    Private Sub ChangeAll()

        'Dim st = Now

        Dim WordErrors = (From xItem In HtmlSpellCheck1.Words Where (xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error))
        For Each item In WordErrors.ToArray
            Dim Suggestions = GetSuggestions(item.NewWord)
            If Suggestions.Count > 0 Then
                'change to first
                item.NewWord = Suggestions.First
                item.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK
                If SelectedWord() Is item Then
                    HtmlSpellCheck1_SelectionChanged(HtmlSpellCheck1, New HTMLSpellCheck.HTMLWordEventArgs() With {.Word = item})
                End If
            End If
        Next
        btnRevertAll.SafeInvoke(Function(x As Button) InlineAssignHelper(x.Enabled, True))

        'complete... unless...
        If HtmlSpellCheck1.mt_SpellCheck IsNot Nothing AndAlso HtmlSpellCheck1.mt_SpellCheck.IsAlive Then
            '... we are still spell checking :(
        Else
            Dim btnChangeAllEnabled = (From xItem In HtmlSpellCheck1.Words Where (xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error)).Count > 0
            ChangeingAll = False
            If btnChangeAllEnabled = True Then
                '...or we have words left that suggestions could not be found for...
                btnChangeAll.SafeInvoke(Function(x As Button) InlineAssignHelper(x.Enabled, btnChangeAllEnabled))
                btnSkip_Click(btnSkip, EventArgs.Empty)
            Else
                CompleteSpellCheck()
            End If
        End If

        'MsgBox(Now.Subtract(st).TotalMilliseconds)

    End Sub

    Private Sub btnChangeAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeAll.Click
        StartChangeAll()
    End Sub

    Private Sub btnIgnore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIgnore.Click
        IgnoreWord(SelectedWord)
    End Sub

    Private Sub IgnoreWord(ByVal theSelectedWord As HTMLSpellCheck.SpellCheckDialogWords)
        If theSelectedWord IsNot Nothing Then
            Dim theWord = theSelectedWord.OrigWord
            SpellCheckControlBase.DictionaryIgnoreWord(theWord)

            theSelectedWord.Changed = True
            theSelectedWord.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.OK

            For Each item In (From xItem In HtmlSpellCheck1.Words Where xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error OrElse xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Case).ToArray
                item.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Pending
            Next
            HtmlSpellCheck1.StartSpellCheck()

            btnSkip_Click(btnSkip, EventArgs.Empty)
        End If
    End Sub

    Dim ChangeToUseOldWord As Boolean
    Private Sub AddWordButton(ByVal CaseSensitive As Boolean)
        Dim theSelectedWord = SelectedWord()
        If theSelectedWord IsNot Nothing Then
            'change the word...
            Dim Word As String = txtChangeTo.Text
            If ChangeToChanged = False Then
                Word = theSelectedWord.NewWord
            Else
                theSelectedWord.NewWord = Word
            End If
            If SpellCheckControlBase.CurrentDictionary.SpellCheckWord(Word) <> i00SpellCheck.Dictionary.SpellCheckWordError.OK Then
                If CaseSensitive = False Then
                    Word = LCase(Word)
                End If
                AddWordToDict(Word, theSelectedWord)
            End If
        End If
        'move next
        btnSkip_Click(btnSkip, EventArgs.Empty)
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        AddWordButton(False)
    End Sub

    Private Sub tsiAddCaseSensitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsiAddCaseSensitive.Click
        AddWordButton(True)
    End Sub

    Private Sub tsiAddCaseInsensitive_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsiAddCaseInsensitive.Click
        AddWordButton(False)
    End Sub

    Private Sub btnAdd_ClickDropdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.ClickDropdown
        Dim theSelectedWord = SelectedWord()
        If theSelectedWord IsNot Nothing Then
            Dim Word As String = txtChangeTo.Text
            If ChangeToChanged = False Then
                Word = theSelectedWord.NewWord
            End If
            tsiAddCaseSensitive.Text = "Case sensitive: " & Word
            tsiAddCaseInsensitive.Text = "Case insensitive: " & LCase(Word)
            tsiAddCaseSensitive.Visible = Word <> LCase(Word)
            cmsAdd.Show(btnAdd, New Point(0, btnAdd.Height))
        End If
    End Sub

    Private Sub AddWordToDict(ByVal theWord As String, ByVal WordObject As HTMLSpellCheck.SpellCheckDialogWords)
        Try
            SpellCheckControlBase.DictionaryAddWord(theWord)
        Catch ex As Exception
            MsgBox("The following error occured saving the dictionary:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try

        'mark all of the other matching words as ok :)
        For Each item In (From xItem In HtmlSpellCheck1.Words Where xItem.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Error)
            item.SpellCheckState = HTMLSpellCheck.SpellCheckDialogWords.SpellCheckStates.Pending
        Next
        HtmlSpellCheck1.StartSpellCheck()

        WordUpdatedFromMenu()
    End Sub

    Private Sub txtChangeTo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChangeTo.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                e.Handled = True
                If lvSuggestions.Items.Count > 0 Then
                    If lvSuggestions.SelectedItems.Count > 0 Then
                        Dim SelectedItem = lvSuggestions.SelectedItems(0)
                        If SelectedItem.Index > 0 Then
                            lvSuggestions.Items(SelectedItem.Index - 1).Selected = True
                            lvSuggestions.EnsureVisible(SelectedItem.Index - 1)
                        End If
                    Else
                        lvSuggestions.Items(0).Selected = True
                        lvSuggestions.EnsureVisible(0)
                    End If
                End If
            Case Keys.Down
                e.Handled = True
                If lvSuggestions.Items.Count > 0 Then
                    If lvSuggestions.SelectedItems.Count > 0 Then
                        Dim SelectedItem = lvSuggestions.SelectedItems(0)
                        If SelectedItem.Index < lvSuggestions.Items.Count - 1 Then
                            lvSuggestions.Items(SelectedItem.Index + 1).Selected = True
                            lvSuggestions.EnsureVisible(SelectedItem.Index + 1)
                        End If
                    Else
                        lvSuggestions.Items(0).Selected = True
                        lvSuggestions.EnsureVisible(0)
                    End If
                End If
        End Select
    End Sub

    Private Sub txtChangeTo_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtChangeTo.KeyUp
        Select Case e.KeyCode
            Case Keys.Apps
                e.Handled = True
                'had to do this as the line above doesn't work for app button :(...
                If txtChangeTo.ContextMenu Is Nothing Then
                    txtChangeTo.ContextMenu = New ContextMenu
                End If
                'now show the html spell check context menu!
                Dim theSelectedWord = SelectedWord()
                If theSelectedWord IsNot Nothing Then
                    MenuCurrentWord = theSelectedWord
                    UpdateMenuItems()
                    OpenedByClick = False
                    cmsHTMLSpellCheck.Show(HtmlSpellCheck1, Point.Empty)
                End If
        End Select
    End Sub

    Private Sub btnClose_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.LostFocus
        If CompleteTooltip IsNot Nothing Then
            CompleteTooltip.Hide(btnClose)
        End If
    End Sub

    Dim ChangeToChangedTrigger As Boolean = True
    Private Sub txtChangeTo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtChangeTo.TextChanged
        If ChangeToChangedTrigger = False Then Exit Sub
        'qwertyuiop - should really mt this?:
        Dim theSelectedWord = SelectedWord()
        If theSelectedWord Is Nothing Then
            btnAdd.Enabled = False
        Else
            Dim Word = txtChangeTo.Text
            If ChangeToUseOldWord Then
                Word = theSelectedWord.NewWord
                ChangeToUseOldWord = False
            End If
            btnAdd.Enabled = txtChangeTo.Visible AndAlso txtChangeTo.Text <> "" AndAlso txtChangeTo.Text.Contains(" ") = False AndAlso SpellCheckControlBase.CurrentDictionary.SpellCheckWord(Word) <> i00SpellCheck.Dictionary.SpellCheckWordError.OK
        End If
        ChangeToChanged = True
    End Sub

    Private Sub txtChangeTo_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChangeTo.VisibleChanged
        txtChangeTo_TextChanged(txtChangeTo, EventArgs.Empty)
    End Sub

End Class