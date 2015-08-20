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

'NativeWindow thinks it can be designed... so disable the designer 
Public Class SpellCheckTextBox
    Inherits SpellCheckControlBase
    Implements iSpellCheckDialog
    Implements iTestHarness

#Region "Text Box"

#Region "To get the height of a given line / chr position"

    Dim RTBContents As Rectangle
    Private Sub parentRichTextBox_ContentsResized(ByVal sender As Object, ByVal e As System.Windows.Forms.ContentsResizedEventArgs) Handles parentRichTextBox.ContentsResized
        RTBContents = e.NewRectangle
    End Sub

#End Region

#Region "Key handlers"

    Private Sub parentTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles parentTextBox.KeyUp
        If parentTextBox.IsSpellCheckEnabled = False Then Return

        Select Case e.KeyCode
            Case Keys.F7
                'spell check dialogue
                If Settings.AllowF7 Then
                    ShowDialog()
                End If
        End Select
    End Sub

#End Region

#Region "Custom Settings"

    Private mc_RenderCompatibility As Boolean = False
    <System.ComponentModel.DefaultValue(False)> _
    <System.ComponentModel.DisplayName("Compatible Rendering")> _
    <System.ComponentModel.Description("Compatible rendering increases drawing compatibility but also adds flickering upon redraw, only enable if there are redraw/layering problems")> _
    Public Property RenderCompatibility() As Boolean
        Get
            Return mc_RenderCompatibility
        End Get
        Set(ByVal value As Boolean)
            If mc_RenderCompatibility <> value Then
                mc_RenderCompatibility = value
                RepaintControl() 'parentTextBox.Invalidate() 'OnRedraw()
            End If
        End Set
    End Property


#End Region

    Private Sub parentTextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentTextBox.TextChanged
        'Standard TextBoxes do not fire the WM_PAINT event when adding text so need to do it this way
        If TypeOf parentTextBox Is TextBox Then
            'parentTextBox.Invalidate()
            RepaintControl()
        End If
    End Sub

    Private Sub parentTextBox_MultilineChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentTextBox.MultilineChanged
        Dim senderTextBox = TryCast(sender, TextBoxBase)
        If senderTextBox IsNot Nothing Then
            If TypeOf senderTextBox Is DataGridViewTextBoxEditingControl Then
                'always enabled 
                Return
            End If
            If senderTextBox.Multiline Then
                senderTextBox.EnableSpellCheck()
            Else
                senderTextBox.DisableSpellCheck()
            End If
        End If
    End Sub


    Private Const WM_VSCROLL As Integer = &H115
    Private Const WM_MOUSEWHEEL As Integer = &H20A
    Private Const WM_PAINT As Integer = &HF

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case WM_PAINT
                If OKToDraw Then
                    If Me.RenderCompatibility Then
                        'old draw method...
                        parentTextBox.Invalidate()
                        CloseOverlay()
                        MyBase.WndProc(m)
                        Me.CustomPaint()
                    Else
                        MyBase.WndProc(m)
                        RepaintControl()
                        'OpenOverlay()
                        'Me.CustomPaint()
                    End If
                Else
                    MyBase.WndProc(m)
                End If
            Case Else
                MyBase.WndProc(m)
        End Select
    End Sub

#End Region

#Region "Constructor"

    Overrides Sub Load()
        parentTextBox = DirectCast(Control, TextBoxBase)
        parentRichTextBox = TryCast(Control, RichTextBox)
        extTextBoxContextMenu = Control.ExtensionCast(Of extTextBoxContextMenu)()
        If parentTextBox.Multiline = False AndAlso Not (TypeOf parentTextBox Is DataGridViewTextBoxEditingControl) Then parentTextBox.DisableSpellCheck()
        RepaintControl()

    End Sub

#End Region

#Region "Test Harness"

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As Control Implements iTestHarness.SetupControl
        If Control.GetType Is GetType(TextBox) Then
            Dim TextBox = DirectCast(Control, TextBox)

            TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12)
            TextBox.Multiline = True
            TextBox.ScrollBars = ScrollBars.Vertical
            TextBox.AppendText(If(TextBox.Text = "", "", vbCrLf & vbCrLf) & "Ths is a standrd text field that uses a dictionary to spel check the its contents ...  as you can se errors are underlnied in red!")

            TextBox.SelectionStart = 0
            TextBox.SelectionLength = 0

            Return TextBox
        ElseIf Control.GetType Is GetType(RichTextBox) Then
            Dim RichTextBox = DirectCast(Control, RichTextBox)

            RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!)

            Dim StartIndex = RichTextBox.TextLength

            RichTextBox.AppendText(If(RichTextBox.Text = "", "", vbCrLf & vbCrLf) & "i00SpellCheck has built in support for RichTextBoxes!" & vbCrLf & _
                               "The quic brown fox junped ovr the lazy dog!" & vbCrLf & _
                               "You can right click to see spelling suggestions for words and to add/ignore/remove words from the dictionary." & vbCrLf & _
                               "If you ignre a word you can hold ctrl down to underlne all ignored words!" & vbCrLf & _
                               "The initial dictionary may take a little while to load ... it holds more than 150 000 words!")

            Dim HighlightKeyWordFormat As New extTextBoxCommon.HighlightKeyWordFormat

            HighlightKeyWordFormat.Color = Color.Red
            extTextBoxCommon.HighlightKeyWord(RichTextBox, "Rich", HighlightKeyWordFormat, RichTextBoxFinds.None, StartIndex)

            HighlightKeyWordFormat.Color = Color.Green
            extTextBoxCommon.HighlightKeyWord(RichTextBox, "Text", HighlightKeyWordFormat, RichTextBoxFinds.None, StartIndex)

            HighlightKeyWordFormat.Color = Color.Blue
            extTextBoxCommon.HighlightKeyWord(RichTextBox, "Boxes", HighlightKeyWordFormat, RichTextBoxFinds.None, StartIndex)

            HighlightKeyWordFormat.Color = Color.FromKnownColor(KnownColor.HotTrack)
            extTextBoxCommon.HighlightKeyWord(RichTextBox, "i00SpellCheck", HighlightKeyWordFormat, RichTextBoxFinds.None, StartIndex)

            HighlightKeyWordFormat.Color = Color.Empty
            HighlightKeyWordFormat.Font = New Font(RichTextBox.Font.Name, CSng(RichTextBox.Font.Size * 1.5), FontStyle.Bold)
            extTextBoxCommon.HighlightKeyWord(RichTextBox, "RichTextBoxes!", HighlightKeyWordFormat, RichTextBoxFinds.None, StartIndex)

            RichTextBox.Select(0, 0)
            RichTextBox.ClearUndo()

            Return RichTextBox
        Else
            Return Nothing
        End If
    End Function

#End Region

End Class