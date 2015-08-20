'i00 .Net Control Extensions
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

Public Class extTextBoxCommon

    Private Sub New()
        'This is a shared class ... so this prevents new instances being created...
        Throw New NotSupportedException
    End Sub

    <Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    End Function

#Region "Scrollbar location"

    <Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function GetScrollPos(ByVal hwnd As IntPtr, ByVal nBar As Integer) As Integer
    End Function

    Private Const SB_HORZ As Integer = 0
    Private Const SB_VERT As Integer = 1
    Private Const SB_TOP As Integer = 6
    Private Const EM_SCROLL As Integer = &HB5
    Private Const EM_LINESCROLL As Integer = &HB6

    Public Shared Function GetScrollBarLocation(ByVal TextBox As TextBoxBase, Optional ByVal Vertical As Boolean = True) As Integer
        Return GetScrollPos(TextBox.Handle, If(Vertical, SB_VERT, SB_HORZ))
    End Function

    Public Shared Sub SetScrollBarLocation(ByVal TextBox As TextBoxBase, ByVal ScrollPos As Integer)
        'SendMessage(TextBox.Handle, EM_SCROLL, SB_TOP, 0) ' reset Vscroll to top
        SendMessage(TextBox.Handle, EM_LINESCROLL, 0, ScrollPos) ' set Vscroll to last saved pos.
    End Sub

#End Region

#Region "Scroll to caret"

    Private Const EM_SCROLLCARET As Integer = &HB7

    'scroll the caret into view
    Public Shared Sub ScrollToCaret(ByVal TextBox As TextBoxBase)
        extTextBoxCommon.SendMessage(TextBox.Handle, extTextBoxCommon.EM_SCROLLCARET, 0, 0)
    End Sub

#End Region

#Region "Line Height"

    'For the below function to work properly will also need to use (if using RTB's):
    'Dim RTBContents As Rectangle
    'Private Sub parentRichTextBox_ContentsResized(ByVal sender As Object, ByVal e As System.Windows.Forms.ContentsResizedEventArgs)
    '    RTBContents = e.NewRectangle
    'End Sub
    Public Shared Function GetLineHeightFromCharPosition(ByVal TextBox As TextBoxBase, ByVal LetterIndex As Integer, ByVal RTBContentSize As Rectangle) As Integer
        Dim TextHeight As Integer = System.Windows.Forms.TextRenderer.MeasureText("Ag", TextBox.Font).Height
        Dim RichTextBox = TryCast(TextBox, RichTextBox)
        If RichTextBox IsNot Nothing Then
            'rich text box :(... need to get the height for each bit...
            Dim OldStart = RichTextBox.SelectionStart
            Dim OldLength = RichTextBox.SelectionLength
            Dim ThisLine = RichTextBox.GetLineFromCharIndex(LetterIndex)
            Dim LineBelow = RichTextBox.GetPositionFromCharIndex(RichTextBox.GetFirstCharIndexFromLine(ThisLine + 1))
            If LineBelow.IsEmpty Then
                If RTBContentSize.IsEmpty Then
                    'qwertyuiop ....
                    'TODO: hrm ... need some way to make the RTB fire the ContentsResized event without interfering with the user...
                    'use the standard text height 4 now...
                    Return TextHeight
                Else
                    Return (RTBContentSize.Height + RichTextBox.GetPositionFromCharIndex(0).Y) - RichTextBox.GetPositionFromCharIndex(LetterIndex).Y
                End If
            Else
                Return LineBelow.Y - RichTextBox.GetPositionFromCharIndex(LetterIndex).Y
            End If
        Else
            Return TextHeight
        End If
    End Function

#End Region

    Public Class HighlightKeyWordFormat
        Public Color As Color
        Public Font As Font
    End Class

    Public Shared Sub HighlightKeyWord(ByVal RichTextBox As RichTextBox, ByVal Find As String, ByVal Format As HighlightKeyWordFormat, Optional ByVal RichTextBoxFinds As RichTextBoxFinds = RichTextBoxFinds.WholeWord, Optional ByVal StartIndex As Integer = 0)
        Dim Index As Integer = StartIndex
        Dim FirstRun = True
        Do Until Index = -1
            Index = RichTextBox.Find(Find, If(Index = 0 AndAlso FirstRun = True, 0, Index + 1), RichTextBoxFinds)
            FirstRun = False
            If Format.Color.IsEmpty = False Then RichTextBox.SelectionColor = Format.Color
            If Format.Font IsNot Nothing Then RichTextBox.SelectionFont = Format.Font
        Loop
    End Sub


    'I wrote this to replace the standard GetPositionFromCharIndex function
    'as the inbuilt one cannot return the position after the very last char
    Public Shared Function GetPositionFromCharIndex(ByVal TextBox As TextBoxBase, ByVal index As Integer) As Point
        If index >= TextBox.TextLength Then
            'we are the last char ... need to add the char width on to the previous char :(
            GetPositionFromCharIndex = TextBox.GetPositionFromCharIndex(index - 1)
            If TextBox.TextLength > 0 Then
                Dim LastLine = TextBox.Lines.Last
                If LastLine = "" Then
                    'hrm ... this is the last letter of the previous line :(
                    GetPositionFromCharIndex.X = 0
                    'qwertyuiop - this should really use the text size of the last bit of text for the 
                    GetPositionFromCharIndex.Y += System.Windows.Forms.TextRenderer.MeasureText("Ag", TextBox.Font).Height
                Else
                    LastLine = Left(LastLine, Len(LastLine) - 1)
                    If LastLine = "" Then LastLine = "Ag"
                    Dim LineSize = TextRenderer.MeasureText(LastLine, TextBox.Font).Width

                    Dim TextWithLineSize = TextRenderer.MeasureText(LastLine & TextBox.Text.Last, TextBox.Font).Width
                    GetPositionFromCharIndex.X += TextWithLineSize - LineSize
                End If
            End If
        Else
            GetPositionFromCharIndex = TextBox.GetPositionFromCharIndex(index)
        End If

    End Function

    <Runtime.InteropServices.DllImport("user32.dll")> _
    Public Shared Function LockWindowUpdate(ByVal hWndLock As IntPtr) As Boolean
    End Function

End Class
