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

#Region "Actual Menu"

#Region "Menu Item Events"

    Private Sub SpellMenuItems_WordAdded(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordAdded
        Try
            DictionaryAddWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error occured adding """ & e.Word & """ to the dictionary:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SpellMenuItems_WordChanged(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordChanged
        'lock window from updating
        extTextBoxCommon.LockWindowUpdate(parentTextBox.Handle)
        parentTextBox.SuspendLayout()

        'If parentRichTextBox IsNot Nothing Then
        '    'rich text box :(... use alternate method ... this will ensure that the formatting isn't lost
        '    parentRichTextBox.Select(extTextBoxContextMenu.LastMenuSpellClickReturn.WordStart, extTextBoxContextMenu.LastMenuSpellClickReturn.Word.Length)
        '    parentRichTextBox.SelectedText = e.Word
        'Else
        '    'standard text box .. can just replace all of the text

        '    'Get old scroll bar position
        '    Dim OldVertPos = extTextBoxCommon.GetScrollBarLocation(parentTextBox)

        '    'replace the text
        '    parentTextBox.Text = Left(parentTextBox.Text, extTextBoxContextMenu.LastMenuSpellClickReturn.WordStart) & _
        '                         e.Word & _
        '                         Right(parentTextBox.Text, Len(parentTextBox.Text) - extTextBoxContextMenu.LastMenuSpellClickReturn.WordEnd)

        '    'Set scroll bars to what they were
        '    extTextBoxCommon.SetScrollBarLocation(parentTextBox, OldVertPos) ' set Vscroll to last saved pos.
        'End If

        parentTextBox.Select(extTextBoxContextMenu.LastMenuSpellClickReturn.WordStart, extTextBoxContextMenu.LastMenuSpellClickReturn.Word.Length)
        parentTextBox.SelectedText = e.Word

        '... and select the replaced text
        parentTextBox.SelectionStart = extTextBoxContextMenu.LastMenuSpellClickReturn.WordStart
        parentTextBox.SelectionLength = Len(e.Word)

        'unlock window updates
        parentTextBox.ResumeLayout()
        extTextBoxCommon.LockWindowUpdate(IntPtr.Zero)
    End Sub

    Private Sub SpellMenuItems_WordUnIgnored(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordUnIgnored
        Try
            DictionaryUnIgnoreWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error ignoring """ & e.Word & """:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SpellMenuItems_WordIgnored(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordIgnored
        Try
            DictionaryIgnoreWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error ignoring """ & e.Word & """:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SpellMenuItems_WordRemoved(ByVal sender As Object, ByVal e As Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordRemoved
        Try
            DictionaryRemoveWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error occured removing """ & e.Word & """ from the dictionary:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region

    Private Sub extTextBoxContextMenu_MenuOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles extTextBoxContextMenu.MenuOpening

        'If ContextMenuStrip.SourceControl Is parentTextBox Then
        'SpellMenuItems.RemoveSpellMenuItems()

        If OKToSpellCheck Then
            SpellMenuItems.ContextMenuStrip = extTextBoxContextMenu.ContextMenuStrip
            If Settings.IgnoreWordOverride(extTextBoxContextMenu.MenuSpellClickReturn.Word) Then
                'word is in caps etc...
            Else
                SpellMenuItems.AddItems(extTextBoxContextMenu.MenuSpellClickReturn.Word, CurrentDictionary, CurrentDefinitions, CurrentSynonyms, Settings)
            End If
        End If
        'Else
        ''no word clicked on
        'End If

    End Sub

    Private WithEvents SpellMenuItems As New Menu.AddSpellItemsToMenu()

    Private Sub extTextBoxContextMenu_MenuClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles extTextBoxContextMenu.MenuClosed
        SpellMenuItems.RemoveSpellMenuItems()
    End Sub

#End Region

End Class