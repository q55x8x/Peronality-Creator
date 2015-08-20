Imports i00SpellCheck

Public Class frmTest

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load the extension
        i00SpellCheck.ControlExtensions.LoadSingleControlExtension(RichTextBox1, New TextBoxSpeechRecognition)
        'load the test data
        RichTextBox1.ExtensionCast(Of TextBoxSpeechRecognition)().SetupControl(RichTextBox1)

        'set the property grid to the extension
        PropertyGrid1.SelectedObject = RichTextBox1.ExtensionCast(Of TextBoxSpeechRecognition)()

        'setup the toolstrip items
        tsbDictate.Image = TextBoxSpeechRecognition.MicImage
        tsbSpeak.Image = TextBoxSpeechRecognition.TalkImage
        tsbStopSpeaking.Image = TextBoxSpeechRecognition.StopImage

        'form icon
        Using b As Bitmap = My.Resources.Talk
            Me.Icon = Icon.FromHandle(b.GetHicon)
        End Using

        'wire up events
        AddHandler TextBoxSpeechRecognition.SpeechEngineHelper.SynthesisStarted, AddressOf SpeechEngineHelper_SynthesisStarted
        AddHandler TextBoxSpeechRecognition.SpeechEngineHelper.SynthesisStoped, AddressOf SpeechEngineHelper_SynthesisStoped
        AddHandler TextBoxSpeechRecognition.SpeechEngineHelper.SpeakProgress, AddressOf SpeechEngineHelper_SpeakProgress

        'setup the "karaoke" web browser
        webKaraoke.AllowWebBrowserDrop = False
        webKaraoke.IsWebBrowserContextMenuEnabled = False
    End Sub

#Region "Button Events"

    Private Sub tsbDictate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbDictate.Click
        RichTextBox1.ExtensionCast(Of TextBoxSpeechRecognition)().DoDictate()
    End Sub

    Private Sub tsbSpeak_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbSpeak.Click
        RichTextBox1.ExtensionCast(Of TextBoxSpeechRecognition)().DoSynthesis()
    End Sub

    Private Sub tsbStopSpeaking_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbStopSpeaking.Click
        'SpeechEngineHelper is used below as the speech is global (not just specific to our text box...)
        TextBoxSpeechRecognition.SpeechEngineHelper.CancelSynthesis()
    End Sub

#End Region

#Region "Handled Events"

    Private Sub SpeechEngineHelper_SpeakProgress(ByVal sender As Object, ByVal e As System.Speech.Synthesis.SpeakProgressEventArgs)
        pbSpeechProgress.Value = CInt(TextBoxSpeechRecognition.SpeechEngineHelper.SpeechProgress * pbSpeechProgress.Maximum)

        UpdateKaraoke(e)
    End Sub

    Private Sub SpeechEngineHelper_SynthesisStoped(ByVal sender As Object, ByVal e As System.EventArgs)
        tsbSpeak.Visible = True
        tsbStopSpeaking.Visible = False
        pbSpeechProgress.Visible = False
        webKaraoke.Visible = False
    End Sub

    Private Sub SpeechEngineHelper_SynthesisStarted(ByVal sender As Object, ByVal e As System.EventArgs)
        tsbSpeak.Visible = False
        tsbStopSpeaking.Visible = True
        pbSpeechProgress.Visible = True

        RenderKaraokeHTML(TextBoxSpeechRecognition.SpeechEngineHelper.SpeechText)
        webKaraoke.Visible = True
    End Sub

#End Region

#Region "Karaoke Rendering"

    Private Sub RenderKaraokeHTML(ByVal Text As String)
        LastElement = Nothing
        Dim sb As New System.Text.StringBuilder
        Dim itemNo = 0
        For Each item In Text.Split(" "c, CChar(vbCr), CChar(vbLf))
            Dim CharNextGap = (itemNo + item.Length) 'next breaking char position
            Dim BreakChar = If(CharNextGap >= Text.Length, "", Replace(Text(CharNextGap), vbCr, "<br>"))
            sb.Append("<span id=" & itemNo & ">" & item & "</span>" & BreakChar)
            itemNo += item.Length + 1
        Next
        Dim DocumentText = HTMLStyle() & "<BODY onselectstart='return false;'><font face='" & SystemFonts.DefaultFont.Name & "' style='font-size:" & SystemFonts.DefaultFont.Size & "pt'>" & sb.ToString

        webKaraoke.DocumentText = DocumentText
    End Sub

    Dim LastElement As HtmlElement
    Private Sub UpdateKaraoke(ByVal SpeakProgressEventArgs As System.Speech.Synthesis.SpeakProgressEventArgs)
        If webKaraoke.IsBusy = False Then

            If LastElement IsNot Nothing Then
                LastElement.SetAttribute("className", "Normal")
            End If

            Dim ElementPosition = SpeakProgressEventArgs.CharacterPosition
            Dim Element As HtmlElement = Nothing
            Do Until Element IsNot Nothing OrElse ElementPosition = -1
                Element = webKaraoke.Document.GetElementById(CStr(ElementPosition))
                ElementPosition -= 1
            Loop
            If Element IsNot Nothing Then
                If Element.OffsetRectangle.Bottom > webKaraoke.Document.Body.OffsetRectangle.Height + webKaraoke.Document.Body.ScrollTop Then
                    webKaraoke.Document.Body.ScrollTop = Element.OffsetRectangle.Bottom - webKaraoke.Document.Body.OffsetRectangle.Height
                ElseIf Element.OffsetRectangle.Top < webKaraoke.Document.Body.ScrollTop Then
                    webKaraoke.Document.Body.ScrollTop = Element.OffsetRectangle.Top
                End If
                Element.SetAttribute("className", "Selected")
                LastElement = Element
            End If
        End If
    End Sub

    Dim mc_BackColor As Color = Color.FromKnownColor(KnownColor.Window)
    Dim mc_ForeColor As Color = Color.FromKnownColor(KnownColor.WindowText)
    Private ReadOnly Property HTMLStyle() As String
        Get
            Return "<style>" & vbCrLf & _
                   "body" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    margin:0px;" & vbCrLf & _
                   "    background-color:" & ColorTranslator.ToHtml(mc_BackColor) & ";" & vbCrLf & _
                   "    color:" & ColorTranslator.ToHtml(mc_ForeColor) & ";" & vbCrLf & _
                   "    font-family:Tahoma;" & vbCrLf & _
                   "    font-size:10pt;" & vbCrLf & _
                   "    cursor:default;" & vbCrLf & _
                   "    outline: none;" & vbCrLf & _
                   "}" & vbCrLf & _
                   ".Normal" & vbCrLf & _
                   "{" & vbCrLf & _
                   "}" & vbCrLf & _
                   ".Selected" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    border: solid 1px " & ColorTranslator.ToHtml(Color.FromKnownColor(KnownColor.Highlight)) & ";" & vbCrLf & _
                   "    background-color:" & ColorTranslator.ToHtml(Color.FromArgb(DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.Highlight), mc_BackColor, 95).ToArgb)) & ";" & vbCrLf & _
                   "    margin: 0px 0px 0px 0px;" & vbCrLf & _
                   "}" & vbCrLf & _
                   "</style>"
        End Get
    End Property

#End Region

End Class
