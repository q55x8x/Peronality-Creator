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

Partial Class SpellCheckTextBox

#Region "Show spellcheck dialog"

    Public Sub ShowDialog() Implements iSpellCheckDialog.ShowDialog
        If CurrentDictionary IsNot Nothing AndAlso CurrentDictionary.Loading = False Then
            Using SpellCheckDialog As New SpellCheckDialog
                Dim SpellCheckResults = SpellCheckDialog.ShowDialog(parentTextBox, Me, parentTextBox.Text)

                'update the text box...

                'lock window from updating
                DrawSpellingErrors = False
                extTextBoxCommon.LockWindowUpdate(Control.Handle)
                Control.SuspendLayout()
                Dim SelStart = parentTextBox.SelectionStart
                Dim SellLength = parentTextBox.SelectionLength


                'Dim mc_parentRichTextBox = TryCast(parentTextBox, RichTextBox)
                If parentRichTextBox IsNot Nothing Then
                    'rich text box :(... use alternate method ... this will ensure that the formatting isn't lost
                    'qwertyuiop - would be nicer if this did this in one undo move...
                    For Each word In (From xItem In SpellCheckResults Where xItem.Changed = True AndAlso xItem.NewWord <> xItem.OrigWord).ToArray.Reverse
                        parentRichTextBox.Select(word.StartIndex, word.OrigWord.Length)
                        parentRichTextBox.SelectedText = word.NewWord
                    Next
                    CType(parentRichTextBox, TextBoxBase).ClearUndo()
                Else
                    'standard text box .. can just replace all of the text

                    'Get old scroll bar position
                    Dim OldVertPos = extTextBoxCommon.GetScrollBarLocation(parentTextBox)

                    Dim NewText As String = parentTextBox.Text
                    For Each word In (From xItem In SpellCheckResults Where xItem.Changed = True AndAlso xItem.NewWord <> xItem.OrigWord).ToArray.Reverse
                        NewText = Strings.Left(NewText, word.StartIndex) & _
                                  word.NewWord & _
                                  Strings.Right(NewText, Len(NewText) - (word.StartIndex + word.OrigWord.Length))
                    Next

                    'replace the text
                    parentTextBox.Text = NewText

                    'Set scroll bars to what they were
                    extTextBoxCommon.SetScrollBarLocation(parentTextBox, OldVertPos) ' set Vscroll to last saved pos.
                End If

                parentTextBox.SelectionStart = SelStart
                parentTextBox.SelectionLength = SellLength

                'unlock window updates
                DrawSpellingErrors = True
                Control.ResumeLayout()
                extTextBoxCommon.LockWindowUpdate(IntPtr.Zero)

            End Using
        End If
    End Sub

#End Region

#Region "Ease of access"

    Private WithEvents extTextBoxContextMenu As extTextBoxContextMenu

    Private WithEvents parentTextBox As TextBoxBase

    Private WithEvents parentRichTextBox As RichTextBox

#End Region

#Region "Misc"

    <System.ComponentModel.Category("Control")> _
    <System.ComponentModel.Description("The TextBox associated with the SpellCheckTextBox object")> _
    <System.ComponentModel.DisplayName("Text Box")> _
    Public Overrides ReadOnly Property Control() As System.Windows.Forms.Control
        Get
            Return MyBase.Control
        End Get
    End Property

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(TextBoxBase)}
        End Get
    End Property

    Public Overrides ReadOnly Property RequiredExtensions() As System.Collections.Generic.List(Of System.Type)
        Get
            RequiredExtensions = New List(Of System.Type)
            RequiredExtensions.Add(GetType(extTextBoxContextMenu))
        End Get
    End Property

#End Region

End Class