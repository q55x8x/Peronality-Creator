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

Public Class frmDictionaryEditor

#Region "Custom dictionary for binding"

    Public Class BoundFlatFileDictionary
        Public WithEvents BindingList As New i00BindingList.BindingListView(Of BoundFlatFileDictionaryEntry)
        Public Shared Function FromFlatFileDictionary(ByVal FlatFileDictionary As FlatFileDictionary) As BoundFlatFileDictionary
            FromFlatFileDictionary = New BoundFlatFileDictionary
            'inbuilt dict
            FromFlatFileDictionary.BindingList.AddRange(From xItem In FlatFileDictionary.IndexedDictionary.GetFullList Order By xItem Select New BoundFlatFileDictionaryEntry With {.ItemState = Dictionary.SpellCheckWordError.OK, .Word = xItem, .User = False})
            'deleted user items
            Dim DeletedItems = (From xItem In FlatFileDictionary.UserWordList Where xItem.State = Dictionary.SpellCheckWordError.SpellError Group Join xItem2 In FromFlatFileDictionary.BindingList.OfType(Of BoundFlatFileDictionaryEntry)() On LCase(xItem.Word) Equals LCase(xItem2.Word) Into Group)
            For Each DeletedItem In DeletedItems
                For Each DeletedGroupItem In DeletedItem.Group
                    DeletedGroupItem.ItemState = Dictionary.SpellCheckWordError.SpellError
                Next
            Next
            'add the user items...
            FromFlatFileDictionary.BindingList.AddRange(From xItem In FlatFileDictionary.UserWordList Where xItem.State <> Dictionary.SpellCheckWordError.SpellError Order By xItem.Word Select New BoundFlatFileDictionaryEntry With {.ItemState = xItem.State, .Word = xItem.Word, .User = True})

        End Function

        Public Function ToFlatFileDictionary() As FlatFileDictionary
            ToFlatFileDictionary = New FlatFileDictionary
            'add the non-user items... (and user removed words)
            Dim WordList = (From xItem In BindingList.FullList Where xItem.Word <> "" AndAlso xItem.User = False Order By xItem.Word Group xItem By xItem.Word Into Group Select Group(0)).ToArray
            For Each item In WordList
                ToFlatFileDictionary.IndexedDictionary.Add(item.Word)
                If item.ItemState = Dictionary.SpellCheckWordError.SpellError Then
                    ToFlatFileDictionary.UserWordList.Add(New UserDictionaryBase.UserDictItem() With {.State = Dictionary.SpellCheckWordError.SpellError, .Word = item.Word})
                End If
            Next

            Dim WordListUser = (From xItem In BindingList.FullList Where xItem.Word <> "" AndAlso xItem.User = True Order By xItem.Word Group xItem By xItem.Word Into Group Select Group(0)).ToArray
            For Each item In WordListUser
                If item.ItemState = Dictionary.SpellCheckWordError.Ignore Then
                    ToFlatFileDictionary.UserWordList.Add(New UserDictionaryBase.UserDictItem() With {.State = Dictionary.SpellCheckWordError.Ignore, .Word = item.Word})
                Else
                    ToFlatFileDictionary.UserWordList.Add(New UserDictionaryBase.UserDictItem() With {.State = Dictionary.SpellCheckWordError.OK, .Word = item.Word})
                End If
            Next

        End Function

        Public Class BoundFlatFileDictionaryEntry
            Public User As Boolean = True
            Public Property Ignore() As Boolean
                Get
                    Return ItemState = Dictionary.SpellCheckWordError.Ignore
                End Get
                Set(ByVal value As Boolean)
                    If True Then
                        ItemState = Dictionary.SpellCheckWordError.Ignore
                    Else
                        ItemState = Dictionary.SpellCheckWordError.SpellError
                    End If
                End Set
            End Property
            Public ItemState As Dictionary.SpellCheckWordError

            Dim mc_Word As String
            Public Property Word() As String
                Get
                    Return mc_Word
                End Get
                Set(ByVal value As String)
                    mc_Word = value
                End Set
            End Property
        End Class

        Public Event FilterChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Private Sub BindingList_FilterChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BindingList.FilterChanged
            RaiseEvent FilterChanged(Me, e)
        End Sub
    End Class

#End Region

#Region "For Alt to trigger toolstrip"

    Const WM_SYSCOMMAND As Integer = &H112
    Const SC_KEYMENU As Integer = &HF100

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

        Select Case m.Msg
            Case WM_SYSCOMMAND
                Select Case m.WParam.ToInt32
                    Case SC_KEYMENU
                        Static LastControl As Control
                        If tsbSave.GetCurrentParent.Focused() Then
                            If LastControl IsNot Nothing Then
                                Try
                                    LastControl.Focus()
                                Catch ex As Exception

                                End Try
                            End If
                        Else
                            LastControl = Me.ActiveControl
                            tsbSave.GetCurrentParent.Focus()
                            tsbSave.Select()
                        End If
                    Case Else
                        MyBase.WndProc(m)
                End Select
            Case Else
                MyBase.WndProc(m)
        End Select

    End Sub

#End Region

#Region "Key"

    Private Sub blDictItems_FilterChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Dict.FilterChanged
        Keys.FilterSet(Dict.BindingList.Filter)
    End Sub

    Private WithEvents Keys As New KeyToolbarHelper

    Private Function GetEnumFullPath(ByVal [Enum] As [Enum]) As String
        Return Replace([Enum].GetType.FullName, "+", ".") & "." & [Enum].ToString
    End Function

    Private Sub frmDictionaryEditor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ShortcutKeyColor = System.Drawing.ColorTranslator.ToHtml(DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.ControlText), Color.FromKnownColor(KnownColor.Control)))

        Keys.Add(New KeyToolbarHelper.KeyItems("Builtin Word", DrawingFunctions.BlendColor(Color.Black, Color.FromKnownColor(KnownColor.Window), 31), "These words are in the non-user dictionary, items cannot be edited in it, they can only be marked as deleted.", "[ItemState] = " & GetEnumFullPath(Dictionary.SpellCheckWordError.OK) & " AndAlso [User]=False"))
        Keys.Add(New KeyToolbarHelper.KeyItems("Removed Word", DrawingFunctions.BlendColor(Color.Red, Color.FromKnownColor(KnownColor.Window), 31), "These words are in the non-user dictionary that have been marked as removed by the user dictionary." & vbCrLf & vbCrLf & "Select the row selectors and press <i><font color=" & ShortcutKeyColor & ">&lt;Delete&gt;</font></i> to mark the item as removed" & vbCrLf & "To unmark an item as removed press <i><font color=" & ShortcutKeyColor & ">&lt;Shift&gt;</font></i> + <i><font color=" & ShortcutKeyColor & ">&lt;Delete&gt;</font></i>", "[ItemState] = " & GetEnumFullPath(Dictionary.SpellCheckWordError.SpellError)))
        Keys.Add(New KeyToolbarHelper.KeyItems("User Word", Color.FromKnownColor(KnownColor.Window), "These are words have been added to the user dictionary.", "[User] = True AndAlso [ItemState] <> " & GetEnumFullPath(Dictionary.SpellCheckWordError.Ignore)))
        Keys.Add(New KeyToolbarHelper.KeyItems("Ignored Word", DrawingFunctions.BlendColor(Color.Blue, Color.FromKnownColor(KnownColor.Window), 31), "These are words have been marked in the user dictionary to be ignored by the i00 Spell Check engine.", "[User] = True AndAlso [ItemState] = " & GetEnumFullPath(Dictionary.SpellCheckWordError.Ignore)))

        Keys.FillToolBarWithKeys(tsKeys)

    End Sub

    Private Sub Keys_FilterClicked(ByVal sender As Object, ByVal e As KeyToolbarHelper.KeyListEventArgs) Handles Keys.FilterClicked
        Dict.BindingList.BindingSource.Filter = e.KeyItem.Filter
    End Sub

#End Region

    Private Filename As String
    Private WithEvents Dict As BoundFlatFileDictionary

    Public Overloads Function ShowDialog(ByVal Dict As FlatFileDictionary) As Dictionary
        Me.StartPosition = FormStartPosition.CenterParent

        Me.Controls.Add(bn)

        LoadDictionary(Dict)

        MyBase.ShowDialog()

        Dim NewDict = Me.Dict.ToFlatFileDictionary()
        NewDict.SetFilename(Filename)
        Return NewDict

        ''rebuild the dictionary from the new data
        'ReBuildDictionary()
        'Return dict
    End Function

    Dim bn As New i00BindingList.BindingNavigatorWithFilter With {.Dock = DockStyle.Bottom, .GripStyle = ToolStripGripStyle.Hidden}

    Private Sub LoadDictionary(ByVal FlatFileDictionary As FlatFileDictionary)
        If FlatFileDictionary Is Nothing Then
            Dict = New BoundFlatFileDictionary
        Else
            Dict = BoundFlatFileDictionary.FromFlatFileDictionary(FlatFileDictionary)
        End If

        Me.Filename = FlatFileDictionary.Filename
        If FlatFileDictionary.Filename = "" Then
            Me.Text = "Dictionary Editor"
        Else
            Me.Text = "Dictionary Editor " & " - " & FlatFileDictionary.Filename
        End If

        dgvDictItems.DataSource = Dict.BindingList.BindingSource
        bn.BindingSource = Dict.BindingList.BindingSource

        'clear filter
        Dict.BindingList.BindingSource.Filter = ""

    End Sub

    'prevent changing of non-user items
    Private Sub dgvDictItems_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgvDictItems.CellBeginEdit
        If e.RowIndex <> -1 AndAlso e.ColumnIndex <> -1 Then
            Dim DictItem = TryCast(dgvDictItems.Rows(e.RowIndex).DataBoundItem, BoundFlatFileDictionary.BoundFlatFileDictionaryEntry)
            If DictItem IsNot Nothing Then
                If DictItem.User = False Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    'color rows / don't paint checkboxes on non-user rows
    Private Sub dgvDictItems_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvDictItems.CellPainting
        If e.RowIndex <> -1 AndAlso e.ColumnIndex <> -1 Then
            Dim DictItem = TryCast(dgvDictItems.Rows(e.RowIndex).DataBoundItem, BoundFlatFileDictionary.BoundFlatFileDictionaryEntry)
            If DictItem IsNot Nothing Then
                If DictItem.User Then
                    If DictItem.ItemState = Dictionary.SpellCheckWordError.Ignore Then
                        e.CellStyle.BackColor = DrawingFunctions.BlendColor(Color.Blue, Color.FromKnownColor(KnownColor.Window), 31)
                    End If
                Else
                    If DictItem.ItemState = Dictionary.SpellCheckWordError.SpellError Then
                        e.CellStyle.BackColor = DrawingFunctions.BlendColor(Color.Red, Color.FromKnownColor(KnownColor.Window), 31)
                    Else
                        e.CellStyle.BackColor = DrawingFunctions.BlendColor(Color.Black, Color.FromKnownColor(KnownColor.Window), 31)
                    End If
                    If dgvDictItems.Columns(e.ColumnIndex).DataPropertyName = "Ignore" Then
                        e.Handled = True
                        e.PaintBackground(e.ClipBounds, True)
                    End If
                End If
            Else
                If dgvDictItems.Columns(e.ColumnIndex).DataPropertyName = "Ignore" Then
                    e.Handled = True
                    e.PaintBackground(e.ClipBounds, True)
                End If
            End If
        End If
    End Sub

    Private Sub DictionaryEditor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        dgvDictItems.EndEdit()
    End Sub

    Private Sub tsbSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSave.Click
        Using sfd As New SaveFileDialog
            sfd.FileName = Filename
            sfd.Filter = "Dictionary Files (*.dic)|*.dic|All Files (*.*)|*.*"
            sfd.FilterIndex = 0
            If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.Text = "Dictionary Editor " & " - " & sfd.FileName
                dgvDictItems.EndEdit()
                Dim FFDictionary = Dict.ToFlatFileDictionary
                FFDictionary.Save(sfd.FileName, True)
            End If
        End Using
    End Sub

    Private Sub tsbOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbOpen.Click
        Using ofd As New OpenFileDialog
            ofd.FileName = Filename
            ofd.Filter = "Dictionary Files (*.dic)|*.dic|All Files (*.*)|*.*"
            ofd.FilterIndex = 0
            If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                LoadDictionary(New FlatFileDictionary(ofd.FileName))
            End If
        End Using
    End Sub

    Private Sub dgvDictItems_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvDictItems.Resize
        'qwertyuiop - what the? can't get the word column width to change?
        'tbcWord.Width = dgvDictItems.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - cbcIgnore.Width - dgvDictItems.RowHeadersWidth
    End Sub

    'Prevent user from deleting non-user items
    Private Sub dgvDictItems_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles dgvDictItems.UserDeletingRow
        Dim DictItem = DirectCast(e.Row.DataBoundItem, BoundFlatFileDictionary.BoundFlatFileDictionaryEntry)
        If DictItem.User = False Then
            If My.Computer.Keyboard.ShiftKeyDown Then
                DictItem.ItemState = Dictionary.SpellCheckWordError.OK
            Else
                DictItem.ItemState = Dictionary.SpellCheckWordError.SpellError
            End If
            e.Cancel = True

        End If
    End Sub

End Class