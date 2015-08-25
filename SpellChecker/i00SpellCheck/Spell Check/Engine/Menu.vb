'i00 .Net Spell Check
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

Public Class Menu

    Public Class CustomItems

#Region "Custom Tool Strip Items"

        'empty interface so we can identify our toolstrip item for spelling correction over custom ones
        Friend Interface tsiSpelling
            '
        End Interface

        Friend Class tsiTextSpellingSeparator
            Inherits MenuTextSeperator
            Implements tsiSpelling
        End Class

        Friend Class tsiSpellingSeparator
            Inherits ToolStripSeparator
            Implements tsiSpelling
        End Class

        Friend Class tsiSpellingSuggestion
            Inherits ToolStripMenuItem
            Implements tsiSpelling
            Public UnderlyingValue As String
            Public Sub New(ByVal Text As String, ByVal UnderlyingValue As String, Optional ByVal Image As Image = Nothing)
                Me.Text = Text
                Me.UnderlyingValue = UnderlyingValue
                Me.Image = Image
            End Sub
            Dim LastStateSelected As Boolean = False
            Public Event SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
            Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
                MyBase.OnPaint(e)
                If LastStateSelected <> Me.Selected Then
                    RaiseEvent SelectionChanged(Me, EventArgs.Empty)
                    LastStateSelected = Me.Selected
                End If
            End Sub
        End Class

        Friend Class tsiDefinition
            Inherits HTMLMenuItem
            Implements tsiSpelling
            Public Sub New(ByVal HTMLText As String)
                MyBase.New(HTMLText)
            End Sub
        End Class

#End Region

    End Class

#Region "Add spell items class"

    Public Class AddSpellItemsToMenu
        Implements IDisposable

        Public Sub RemoveSpellMenuItems()
            'remove all the previous Suggestions from a menu
            If ContextMenuStrip IsNot Nothing Then
                For Each item In ContextMenuStrip.Items.OfType(Of CustomItems.tsiSpelling)().ToArray
                    ContextMenuStrip.Items.Remove(TryCast(item, ToolStripItem))
                Next
            End If
        End Sub

        Private WithEvents SpellToolTip As New HTMLToolTip

        Dim DefinitionSet As Definitions
        Dim Dictionary As Dictionary

        Dim mtTip As System.Threading.Thread

        Private Sub LookupDefForTip(ByVal oTsi As Object)
            Dim tsi = TryCast(oTsi, CustomItems.tsiSpellingSuggestion)
            If tsi IsNot Nothing Then
                Dim WordDef = DefinitionSet.FindWord(tsi.UnderlyingValue, Dictionary).ToString
                If WordDef <> "" Then
                    'had to create a new one each time ... otherwise it doesn't fade when moving between items
                    SpellToolTip.Dispose()
                    SpellToolTip = New HTMLToolTip

                    ShowTip(WordDef, tsi)
                Else
                    HideTip(tsi)
                End If
            End If
        End Sub

        Private Delegate Sub ShowTip_cb(ByVal WordDef As String, ByVal tsi As CustomItems.tsiSpellingSuggestion)
        Private Sub ShowTip(ByVal WordDef As String, ByVal tsi As CustomItems.tsiSpellingSuggestion)
            If tsi.Owner IsNot Nothing Then
                If tsi.Owner.InvokeRequired Then
                    Dim ShowTip_cb As New ShowTip_cb(AddressOf ShowTip)
                    tsi.Owner.Invoke(ShowTip_cb, WordDef, tsi)
                Else
                    Dim ToolTipPoint = New Point(tsi.Bounds.Right, tsi.Bounds.Top)
                    SpellToolTip.ShowHTML(WordDef, tsi.Owner, ToolTipPoint)
                End If
            End If
        End Sub

        Private Delegate Sub HideTip_cb(ByVal tsi As CustomItems.tsiSpellingSuggestion)
        Private Sub HideTip(ByVal tsi As CustomItems.tsiSpellingSuggestion)
            If tsi.Owner IsNot Nothing Then
                If tsi.Owner.InvokeRequired Then
                    Dim HideTip_cb As New HideTip_cb(AddressOf HideTip)
                    tsi.Owner.Invoke(HideTip_cb, tsi)
                Else
                    Dim ToolTipPoint = New Point(tsi.Bounds.Right, tsi.Bounds.Top)
                    SpellToolTip.Hide(tsi.Owner)
                End If
            End If
        End Sub

        Private Sub SpellToolTip_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsi = TryCast(sender, CustomItems.tsiSpellingSuggestion)
            If tsi IsNot Nothing Then
                If mtTip IsNot Nothing AndAlso mtTip.IsAlive Then
                    mtTip.Abort()
                End If
                mtTip = New System.Threading.Thread(AddressOf LookupDefForTip)
                mtTip.Name = "Definition Tooltip - " & tsi.UnderlyingValue
                mtTip.IsBackground = True
                mtTip.Start(tsi)
            End If
        End Sub

        Private Sub ContextMenuStrip_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles ContextMenuStrip.Closed
            Dim ContextMenuStrip = TryCast(sender, ContextMenuStrip)
            If ContextMenuStrip IsNot Nothing Then
                SpellToolTip.Hide(ContextMenuStrip)
            End If
        End Sub

        Private Sub SpellToolTip_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)
            If mtTip IsNot Nothing AndAlso mtTip.IsAlive Then
                mtTip.Abort()
            End If
            Dim tsi = TryCast(sender, ToolStripItem)
            If tsi IsNot Nothing Then
                SpellToolTip.Hide(tsi.Owner)
            End If
        End Sub

        Public WithEvents ContextMenuStrip As ContextMenuStrip

        Public Sub New()

        End Sub

        Public Sub New(ByVal ContextMenuStrip As ContextMenuStrip)
            Me.ContextMenuStrip = ContextMenuStrip
        End Sub

        Public Class SpellItemEventArgs
            Inherits EventArgs
            Public Word As String
        End Class

        Public Event WordChanged(ByVal sender As Object, ByVal e As SpellItemEventArgs)
        Public Event WordRemoved(ByVal sender As Object, ByVal e As SpellItemEventArgs)
        Public Event WordAdded(ByVal sender As Object, ByVal e As SpellItemEventArgs)
        Public Event WordIgnored(ByVal sender As Object, ByVal e As SpellItemEventArgs)
        Public Event WordUnIgnored(ByVal sender As Object, ByVal e As SpellItemEventArgs)

        Private Sub SpellItemClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsiSpellingSuggestion = TryCast(sender, CustomItems.tsiSpellingSuggestion)
            If tsiSpellingSuggestion IsNot Nothing Then
                RaiseEvent WordChanged(Me, New SpellItemEventArgs() With {.Word = tsiSpellingSuggestion.UnderlyingValue})
            End If
        End Sub

        Private Sub SpellRemoveWordClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsiSpellingSuggestion = TryCast(sender, CustomItems.tsiSpellingSuggestion)
            If tsiSpellingSuggestion IsNot Nothing Then
                RaiseEvent WordRemoved(Me, New SpellItemEventArgs() With {.Word = tsiSpellingSuggestion.UnderlyingValue})
            End If
        End Sub

        Private Sub SpellAddNewWordClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsiSpellingSuggestion = TryCast(sender, CustomItems.tsiSpellingSuggestion)
            If tsiSpellingSuggestion IsNot Nothing Then
                RaiseEvent WordAdded(Me, New SpellItemEventArgs() With {.Word = tsiSpellingSuggestion.UnderlyingValue})
            End If
        End Sub

        Private Sub SpellUnIgnoreWordClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsiSpellingSuggestion = TryCast(sender, CustomItems.tsiSpellingSuggestion)
            If tsiSpellingSuggestion IsNot Nothing Then
                RaiseEvent WordUnIgnored(Me, New SpellItemEventArgs() With {.Word = tsiSpellingSuggestion.UnderlyingValue})
            End If
        End Sub

        Private Sub SpellIgnoreWordClick(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsiSpellingSuggestion = TryCast(sender, CustomItems.tsiSpellingSuggestion)
            If tsiSpellingSuggestion IsNot Nothing Then
                RaiseEvent WordIgnored(Me, New SpellItemEventArgs() With {.Word = tsiSpellingSuggestion.UnderlyingValue})
            End If
        End Sub

        Private Sub AddSeperatorIfNeeded()
            Dim LastItem = ContextMenuStrip.Items.OfType(Of ToolStripItem)().LastOrDefault
            If LastItem Is Nothing OrElse TypeOf (LastItem) Is CustomItems.tsiSpellingSeparator Then
                'don't add
            Else
                ContextMenuStrip.Items.Add(New CustomItems.tsiSpellingSeparator())
            End If
        End Sub

        Public Sub AddItems(ByVal Word As String, ByVal Dictionary As Dictionary, ByVal DefinitionSet As Definitions, ByVal Synonyms As Synonyms, ByVal Settings As SpellCheckSettings)
            If Dictionary IsNot Nothing AndAlso Dictionary.Loading Then Exit Sub
            Me.DefinitionSet = DefinitionSet
            Me.Dictionary = Dictionary

            Dim Result = Dictionary.SpellCheckWord(Word)
            Dim NiceWord As String = Dictionary.Formatting.RemoveApoS(Word)
            If NiceWord = "" Then Exit Sub 'shouldn't happen
            If Result = Dictionary.SpellCheckWordError.OK OrElse Result = Dictionary.SpellCheckWordError.CaseError OrElse Dictionary.Count = 0 Then
                'word is in the dictionary... regardless of case...
                'Lookup the def
                If Settings.AllowInMenuDefs Then
                    Dim WordDef = DefinitionSet.FindWord(Word, Dictionary, System.Drawing.ColorTranslator.ToHtml(DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.MenuText), Color.FromKnownColor(KnownColor.Menu)))).ToString
                    If WordDef <> "" Then
                        AddSeperatorIfNeeded()
                        Dim tsib As New CustomItems.tsiDefinition(WordDef)
                        ContextMenuStrip.Items.Add(tsib)
                    End If
                End If

                'Suggest other Synonyms
                If Settings.AllowChangeTo Then
                    Dim MatchedSynonyms = Synonyms.FindWord(Word)
                    If MatchedSynonyms IsNot Nothing Then
                        'Add change to...
                        AddSeperatorIfNeeded()
                        Dim tsiMain = New CustomItems.tsiSpellingSuggestion("Change to ...", "")
                        ContextMenuStrip.Items.Add(tsiMain)
                        For Each item In MatchedSynonyms
                            Dim tsiSeparator As New CustomItems.tsiTextSpellingSeparator() With {.Text = item.TypeDescription & If(item.WordType <> Synonyms.FindWordReturn.WordTypes.Other, " (" & item.WordType.ToString & ")", "")}
                            tsiMain.DropDownItems.Add(tsiSeparator)
                            For Each iSyn In item
                                Dim tsi = New CustomItems.tsiSpellingSuggestion(iSyn, iSyn)
                                AddHandler tsi.Click, AddressOf SpellItemClick
                                AddHandler tsi.MouseEnter, AddressOf SpellToolTip_MouseEnter
                                AddHandler tsi.MouseLeave, AddressOf SpellToolTip_MouseLeave
                                'AddHandler tsi.SelectionChanged, AddressOf SpellToolTip_SelectionChanged
                                tsiMain.DropDownItems.Add(tsi)
                            Next
                        Next
                    End If
                End If

            End If

            If Dictionary.Count = 0 Then Exit Sub

            Select Case Result
                Case Dictionary.SpellCheckWordError.OK
                    If Settings.AllowRemovals Then
                        AddSeperatorIfNeeded()
                        Dim tsi = New CustomItems.tsiSpellingSuggestion("Remove " & NiceWord & " from dictionary", Word, My.Resources.RemoveWord)
                        AddHandler tsi.Click, AddressOf SpellRemoveWordClick
                        ContextMenuStrip.Items.Add(tsi)
                    End If
                Case Else
                    Dim Suggestions As List(Of Dictionary.SpellCheckSuggestionInfo) = Nothing

                    Dim Ucase1stLetter As Boolean
                    If Char.IsUpper(NiceWord(0)) Then
                        'upper case - so lets make suggestions 1st letter in ucase :)
                        Ucase1stLetter = True
                    End If
                    Select Case Result
                        Case i00SpellCheck.Dictionary.SpellCheckWordError.Ignore
                            'unignore
                            AddSeperatorIfNeeded()
                            Dim tsi = New CustomItems.tsiSpellingSuggestion("Don't Ignore " & NiceWord, NiceWord, My.Resources.Ignore)
                            AddHandler tsi.Click, AddressOf SpellUnIgnoreWordClick
                            ContextMenuStrip.Items.Add(tsi)
                        Case Dictionary.SpellCheckWordError.SpellError, Dictionary.SpellCheckWordError.CaseError
                            Suggestions = (From xItem In Dictionary.SpellCheckSuggestions(Word) Order By xItem.Closness Descending).ToList
                            If Result = i00SpellCheck.Dictionary.SpellCheckWordError.CaseError Then
                                'only show suggestions with the changed case
                                Dim newSuggestions = (From xItem In Suggestions Where LCase(xItem.Word) = LCase(Word)).ToList
                                If newSuggestions.Count > 0 Then
                                    Suggestions = newSuggestions
                                End If
                            End If
                    End Select

                    If Ucase1stLetter AndAlso Suggestions IsNot Nothing Then
                        For Each item In Suggestions
                            Dim arrWord = item.Word.ToArray
                            arrWord(0) = Char.ToUpper(arrWord(0))
                            item.Word = CStr(arrWord)
                        Next
                    End If

                    If Suggestions IsNot Nothing Then
                        'word is not in the dictionary ... so suggest
                        AddSeperatorIfNeeded()

                        If Settings.AllowAdditions = True AndAlso Result <> Dictionary.SpellCheckWordError.CaseError Then
                            Dim theWord As String = Word
                            If NiceWord = LCase(NiceWord) Then
                                'not case sensitive
                                Dim tsi = New CustomItems.tsiSpellingSuggestion("Add " & NiceWord & " to dictionary", NiceWord, My.Resources.AddWord)
                                AddHandler tsi.Click, AddressOf SpellAddNewWordClick
                                ContextMenuStrip.Items.Add(tsi)
                            Else
                                'may be case sensitive
                                Dim tsi = New CustomItems.tsiSpellingSuggestion("Add to dictionary", "", My.Resources.AddWord)
                                Dim tsiCase = New CustomItems.tsiSpellingSuggestion("Case sensitive: " & NiceWord, NiceWord, My.Resources.CaseSensitive)
                                AddHandler tsiCase.Click, AddressOf SpellAddNewWordClick
                                tsi.DropDownItems.Add(tsiCase)
                                tsiCase = New CustomItems.tsiSpellingSuggestion("Case insensitive: " & LCase(NiceWord), LCase(NiceWord), My.Resources.CaseInsensitive)
                                AddHandler tsiCase.Click, AddressOf SpellAddNewWordClick
                                tsi.DropDownItems.Add(tsiCase)
                                ContextMenuStrip.Items.Add(tsi)
                            End If
                        End If

                        If Settings.AllowIgnore = True Then
                            Dim tsi = New CustomItems.tsiSpellingSuggestion("Ignore " & NiceWord, NiceWord, My.Resources.Ignore)
                            AddHandler tsi.Click, AddressOf SpellIgnoreWordClick
                            ContextMenuStrip.Items.Add(tsi)
                        End If

                        If Suggestions.Count = 0 Then
                            Dim tsi = New CustomItems.tsiSpellingSuggestion("No suggestions", "", My.Resources.SpellCheck)
                            tsi.Enabled = False
                            ContextMenuStrip.Items.Add(tsi)
                        Else
                            Dim TopCloseness = Suggestions.Max(Function(x As Dictionary.SpellCheckSuggestionInfo) x.Closness)
                            Dim FilteredSuggestions As Dictionary.SpellCheckSuggestionInfo()
                            If TopCloseness = 0 Then
                                'this dictionary doesn't prioritize words on a closeness rating... just do it in order
                                FilteredSuggestions = Suggestions.ToArray
                            Else
                                FilteredSuggestions = (From xItem In Suggestions Order By xItem.Closness Descending, xItem.Word Ascending Where xItem.Closness >= TopCloseness * 0.75).ToArray
                            End If

                            FilteredSuggestions = (From xItem In FilteredSuggestions Where Array.IndexOf(FilteredSuggestions, xItem) < 15).ToArray

                            For Each item In FilteredSuggestions
                                'hrm interesting ... had in the line below If(item Is Suggestions.First.... but this wouldn't work for capitalisation
                                Dim tsi = New CustomItems.tsiSpellingSuggestion(item.Word, item.Word, If(item.Word = Suggestions.First.Word, My.Resources.SpellCheck, Nothing))
                                AddHandler tsi.Click, AddressOf SpellItemClick
                                AddHandler tsi.MouseEnter, AddressOf SpellToolTip_MouseEnter
                                AddHandler tsi.MouseLeave, AddressOf SpellToolTip_MouseLeave
                                'AddHandler tsi.SelectionChanged, AddressOf SpellToolTip_SelectionChanged
                                ContextMenuStrip.Items.Add(tsi)
                            Next
                            End If
                    End If
            End Select
        End Sub


        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    'clean up objects
                    SpellToolTip.Dispose()
                    If mtTip IsNot Nothing AndAlso mtTip.IsAlive Then
                        mtTip.Abort()
                    End If
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class

#End Region

End Class
