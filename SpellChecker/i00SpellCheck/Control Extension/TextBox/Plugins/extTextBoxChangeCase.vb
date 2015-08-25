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

Public Class extTextBoxChangeCase
    Inherits ControlExtension

#Region "Ease of access"

    Private WithEvents mc_TextBox As TextBoxBase
    Protected Friend ReadOnly Property parentTextBox() As TextBoxBase
        Get
            Return mc_TextBox
        End Get
    End Property


    Private WithEvents mc_RichTextBox As RichTextBox
    Protected Friend ReadOnly Property parentRichTextBox() As RichTextBox
        Get
            Return mc_RichTextBox
        End Get
    End Property

#End Region

#Region "Underlying Control"

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(TextBoxBase)}
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Overrides Sub Load()
        mc_TextBox = DirectCast(Control, TextBoxBase)
        mc_RichTextBox = TryCast(Control, RichTextBox)
    End Sub

#End Region

#Region "Key Press"

    Private Sub mc_TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles mc_TextBox.KeyDown
        'Ctrl+A
        If e.KeyCode = Keys.A AndAlso My.Computer.Keyboard.CtrlKeyDown Then
            e.Handled = True
            parentTextBox.SelectAll()
        End If
    End Sub

    Private Sub parentTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles mc_TextBox.KeyUp
        Select Case e.KeyCode
            Case Keys.F3
                'cycle through case's ... sentence, propper, upper, lower, origional
                CycleCase()
        End Select
    End Sub

#End Region

#Region "Properties"

    Dim mc_AllowF3ToChangeCase As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow F3 to Change Case")> _
    <System.ComponentModel.Description("Enables the F3 shortcut to change the case of the selected text")> _
    Public Property AllowF3ToChangeCase() As Boolean
        Get
            Return mc_AllowF3ToChangeCase
        End Get
        Set(ByVal value As Boolean)
            mc_AllowF3ToChangeCase = value
        End Set
    End Property

#End Region

#Region "Change case"

    Public Shared Function SentenceCase(ByVal Input As String) As String
        Dim SentenceBreakers As String = System.Text.RegularExpressions.Regex.Escape(".!?" & vbCrLf)
        Dim Pattern As String = "((?<=^[^" & SentenceBreakers & "\w]{0,})[a-z]|(?<![" & SentenceBreakers & "])(?<=[" & SentenceBreakers & "][^" & SentenceBreakers & "\w]{0,})[a-z])"
        Return System.Text.RegularExpressions.Regex.Replace(Input, Pattern, Function(m) m.Value(0).ToString().ToUpper() & m.Value.Substring(1))
    End Function

    Private Enum Cases
        Origional
        Sentence
        Propper
        Upper
        Lower
    End Enum

    Dim TipCase As New HTMLToolTip With {.IsBalloon = True, .ToolTipIcon = ToolTipIcon.Info, .ToolTipTitle = "Case changed", .ToolTipOrientation = HTMLToolTip.ToolTipOrientations.LowRight}

    Private Sub parentTextBox_ForChangeCase_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mc_TextBox.Disposed
        TipCase.Dispose()
    End Sub

    Private Sub CycleCase()
        If parentTextBox.SelectedText = "" Then Return

        Static LastSelectedText As String = ""
        Static CurrentCase As Cases
        Static OrigionalRTF As String
        If UCase(LastSelectedText) <> UCase(parentTextBox.SelectedText) Then
            'new text selected
            LastSelectedText = parentTextBox.SelectedText
            CurrentCase = Cases.Origional
            LastSelectedText = parentTextBox.SelectedText
            If parentRichTextBox IsNot Nothing Then OrigionalRTF = parentRichTextBox.SelectedRtf
        End If

        CurrentCase = CType((CInt(CurrentCase) + 1) Mod (UBound([Enum].GetValues(GetType(Cases))) + 1), Cases)

        If parentRichTextBox IsNot Nothing Then
            'this section is used to preserve RTB formatting when changing case...
            Dim ToBeText As String = OrigionalRTF
            Dim RTFMatchPatern = "(?<=\s).(?<![\s|\\]).*?((?=\s)|(?=((?<!\\)\\[^\\]))(?=((?<!\\)\\[^}]))(?=((?<!\\)\\[^{])))"
            'oldregex = "(?<=( |\t|\r|\n)).(?<!( |\t|\r|\n|\\)).*?((?=})|(?=\r)|(?=\n)|(?= )|(?=((?<!\\)\\[^\\]))(?=((?<!\\)\\[^}]))(?=((?<!\\)\\[^{])))"
            Dim EndSentenceChars() As Char = {"."c, "!"c, "?"c}
            Select Case CurrentCase
                Case Cases.Origional

                Case Cases.Sentence
                    Dim mc = System.Text.RegularExpressions.Regex.Matches(ToBeText, "(\r|\n|" & RTFMatchPatern & ")")

                    Dim NewSentence As Boolean = True
                    For Each match In mc.OfType(Of System.Text.RegularExpressions.Match)()
                        If isInRTFTextSegment(ToBeText, match) Then
                            'we can change this as we are true text...
                            If NewSentence Then
                                'capitalize the 1st letter...
                                Dim CapMatch = System.Text.RegularExpressions.Regex.Match(match.Value, "\w")
                                If CapMatch.Success Then
                                    'yay
                                    Dim NewWord = match.Value.Substring(0, CapMatch.Index) & CapMatch.Value.ToUpper & match.Value.Substring(CapMatch.Index + CapMatch.Length)
                                    ToBeText = ToBeText.Substring(0, match.Index) & NewWord & ToBeText.Substring(match.Index + match.Length)
                                    NewSentence = False
                                Else
                                    'this word needs cap... but no word start found...
                                    Continue For
                                End If
                            End If
                            If match.Value = vbCr OrElse match.Value = vbLf Then
                                NewSentence = True
                            Else
                                If match.Value <> "" AndAlso EndSentenceChars.Contains(match.Value.Last) Then
                                    NewSentence = True
                                End If
                            End If
                        End If
                    Next
                Case Cases.Propper
                    ToBeText = System.Text.RegularExpressions.Regex.Replace(ToBeText, RTFMatchPatern, Function(m) If(isInRTFTextSegment(ToBeText, m), StrConv(m.Value, VbStrConv.ProperCase), m.Value))
                Case Cases.Upper
                    ToBeText = System.Text.RegularExpressions.Regex.Replace(ToBeText, RTFMatchPatern, Function(m) If(isInRTFTextSegment(ToBeText, m), m.Value.ToUpper(), m.Value))
                Case Cases.Lower
                    ToBeText = System.Text.RegularExpressions.Regex.Replace(ToBeText, RTFMatchPatern, Function(m) If(isInRTFTextSegment(ToBeText, m), m.Value.ToLower(), m.Value))
            End Select
            Dim OldSelectionStart = parentTextBox.SelectionStart
            Dim OldSelectionLen = parentTextBox.SelectionLength
            Dim SetWholeText As Boolean  'gets round the empty line rtb bug...
            '                qwertyuiop - unfortunatly it creates another... the undo stack is cleared :(
            If OldSelectionStart = 0 AndAlso OldSelectionLen = parentTextBox.TextLength Then SetWholeText = True
            If SetWholeText Then
                parentRichTextBox.Rtf = ToBeText.TrimEnd(CChar(vbCr), CChar(vbLf))
            Else
                parentRichTextBox.SelectedRtf = ToBeText
            End If
            parentTextBox.SelectionStart = OldSelectionStart
            parentTextBox.SelectionLength = OldSelectionLen
        Else
            Dim ToBeText As String = LastSelectedText
            Select Case CurrentCase
                Case Cases.Origional

                Case Cases.Sentence
                    ToBeText = SentenceCase(ToBeText)
                Case Cases.Propper
                    ToBeText = StrConv(ToBeText, VbStrConv.ProperCase)
                Case Cases.Upper
                    ToBeText = UCase(ToBeText)
                Case Cases.Lower
                    ToBeText = LCase(ToBeText)
            End Select
            Dim OldSelectionStart = parentTextBox.SelectionStart
            Dim OldSelectionLen = parentTextBox.SelectionLength
            parentTextBox.SelectedText = ToBeText
            parentTextBox.SelectionStart = OldSelectionStart
            parentTextBox.SelectionLength = OldSelectionLen
        End If

        'tooltip

        'no longer to dispose 1st (to stop the dissapearing not being 5000 if they press it multiple times quickly) due to my new tooltip

        'scroll the caret into view
        extTextBoxCommon.ScrollToCaret(parentTextBox)
        Dim ChrPos = Me.parentTextBox.GetPositionFromCharIndex(parentTextBox.SelectionStart)
        Dim LineHeight As Integer = extTextBoxCommon.GetLineHeightFromCharPosition(parentTextBox, parentTextBox.SelectionStart, RTBContents)
        ChrPos.Y += LineHeight
        ChrPos.X += 8

        If ChrPos.Y < 0 Then ChrPos.Y = 0
        If ChrPos.Y > parentTextBox.Height Then ChrPos.Y = parentTextBox.Height
        TipCase.ShowHTML("to " & CurrentCase.ToString & " " & If(CurrentCase = Cases.Origional, "state", "case"), parentTextBox, ChrPos, 2500)

    End Sub

    Private Function isInRTFTextSegment(ByVal CompleteString As String, ByVal m As System.Text.RegularExpressions.Match) As Boolean
        Return System.Text.RegularExpressions.Regex.Matches(Left(CompleteString, m.Index), "(?<!\\){").Count - System.Text.RegularExpressions.Regex.Matches(Left(CompleteString, m.Index), "(?<!\\)}").Count = 1
    End Function

    Dim RTBContents As Rectangle
    Private Sub mc_RichTextBox_ContentsResized(ByVal sender As Object, ByVal e As System.Windows.Forms.ContentsResizedEventArgs) Handles mc_RichTextBox.ContentsResized
        RTBContents = e.NewRectangle
    End Sub

#End Region

End Class
