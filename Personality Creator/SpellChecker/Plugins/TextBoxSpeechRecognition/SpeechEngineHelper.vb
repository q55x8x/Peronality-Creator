'i00 .Net Spell Check - TextBoxSpeechRecognition
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

Imports i00SpellCheck

Partial Class TextBoxSpeechRecognition

    Public Class SpeechEngineHelper

        Private Sub New()
            'This is a shared class ... so this prevents new instances being created...
            Throw New NotSupportedException
        End Sub

        Private Shared WithEvents RecognitionTextBox As TextBoxBase

        Private Shared Sub TextBox_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles RecognitionTextBox.KeyPress
            Select Case Asc(e.KeyChar)
                Case Keys.Enter, Keys.Return
                    'doesnt work if PreviewKey handles this
                    e.Handled = True
                    CommitRecognition()
                Case Keys.Escape
                    CancelRecognition()
            End Select
        End Sub

        Private Shared Sub RecognitionTextBox_PreviewKeyDown(ByVal sender As Object, ByVal e As PreviewKeyDownEventArgs) Handles RecognitionTextBox.PreviewKeyDown
            Select Case e.KeyCode
                Case Keys.Enter
                    'qwertyuiop - doesnt work properly ????
                    'places an enter after the recognition is commited ... don't know how to work around this
                    CommitRecognition()
            End Select
        End Sub

#Region "Dictate"

        Friend Shared WithEvents Recogniser As Speech.Recognition.SpeechRecognitionEngine

        Friend Shared WithEvents DictateToolTip As i00SpellCheck.HTMLToolTip

        Friend Shared DictateCancel As Boolean

        Private Shared LastAudioLevel As Integer

        Friend Shared Sub CancelRecognition()
            RecognitionTextBox = Nothing
            DictateCancel = True
            If DictateToolTip IsNot Nothing Then DictateToolTip.Hide()
        End Sub

        Private Shared Sub Recogniser_AudioLevelUpdated(ByVal sender As Object, ByVal e As System.Speech.Recognition.AudioLevelUpdatedEventArgs) Handles Recogniser.AudioLevelUpdated
            If DictateCancel Then Exit Sub
            Dim AudioLevel = CInt(Int((e.AudioLevel / 100) * 16))

            If LastAudioLevel <> AudioLevel Then
                'redraw...
                Dim b As New Bitmap(16, 16)
                Using g = Graphics.FromImage(b)
                    g.DrawImageUnscaled(MicImageBW, New Point(0, 0))
                    g.SetClip(New Rectangle(0, 16 - AudioLevel, 16, AudioLevel))
                    g.DrawImageUnscaled(MicImage, New Point(0, 0))
                End Using

                If DictateToolTip.Image IsNot Nothing Then
                    DictateToolTip.Image.Dispose()
                    DictateToolTip.Image = Nothing
                End If

                DictateToolTip.Image = b
                DictateToolTip.ShowHTML(DictateToolTip.LastText, RecognitionTextBox, ToolTipLocation, 3000)

                LastAudioLevel = AudioLevel
            End If
        End Sub

        Private Shared ToolTipLocation As Point
        Private Shared ExistingText As String
        Private Shared tmpText As String
        Private Shared ListeningColor As String = System.Drawing.ColorTranslator.ToHtml(i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.ControlText), Color.FromKnownColor(KnownColor.Control)))

        Friend Shared Sub DoDictate(ByVal TextBox As TextBoxBase, ByVal RTBContentSize As Rectangle)
            StopOtherInstancesFromSpeaking()

            'do dictate
            '... unless we are already...
            If SpeechEngineHelper.DictateToolTip Is Nothing Then
                'cancel speech
                CancelSynthesis()

                'speech balloon pos...
                extTextBoxCommon.ScrollToCaret(TextBox)
                SpeechEngineHelper.ToolTipLocation = extTextBoxCommon.GetPositionFromCharIndex(TextBox, TextBox.SelectionStart)
                Dim lineHeight = i00SpellCheck.extTextBoxCommon.GetLineHeightFromCharPosition(TextBox, TextBox.SelectionStart, RTBContentSize)
                ToolTipLocation.Y += lineHeight


                'dictate
                SpeechEngineHelper.RecognitionTextBox = TextBox
                'SpeechEngineHelper.ToolTipLocation = ToolTipLocation
                tmpText = ""
                ExistingText = ""
                Dim [Error] As Exception = Nothing
                Try
                    Recogniser = New Speech.Recognition.SpeechRecognitionEngine

                    Try
                        Recogniser.SetInputToDefaultAudioDevice()
                    Catch ex As InvalidOperationException
                        Throw New Exception("Could not set the default audio device" & vbCrLf & "Please make sure your soundcard and microphone are working correctly")
                    End Try
                    Recogniser.LoadGrammar(New System.Speech.Recognition.DictationGrammar)
                    Recogniser.RecognizeAsync(Speech.Recognition.RecognizeMode.Multiple)
                Catch ex As Exception
                    [Error] = ex
                End Try

                If [Error] Is Nothing Then
                    'tooltip
                    DictateCancel = False
                    LastAudioLevel = 0
                    DictateToolTip = New i00SpellCheck.HTMLToolTip With {.IsBalloon = True, .ToolTipTitle = "Dictate what you want typed", .ToolTipIcon = ToolTipIcon.Info}
                    DictateToolTip.ToolTipOrientation = i00SpellCheck.HTMLToolTip.ToolTipOrientations.LowRight
                    DictateToolTip.Image = DirectCast(MicImageBW.Clone, Bitmap)
                    DictateToolTip.ShowHTML("<i><font color=" & ListeningColor & ">Listening<br></i>...click this balloon or wait after you are done talking to confirm<br>...click on the textbox or press <i>&lt;Escape&gt;</i> to cancel</font>", TextBox, ToolTipLocation, 5000)
                    'do something like?: '<br>...or right click the balloon when finished for corrections
                Else
                    'show the error
                    LastAudioLevel = 0
                    DictateToolTip = New i00SpellCheck.HTMLToolTip With {.IsBalloon = True, .ToolTipTitle = "Error starting dictate", .ToolTipIcon = ToolTipIcon.Error}
                    DictateToolTip.ToolTipOrientation = i00SpellCheck.HTMLToolTip.ToolTipOrientations.LowRight
                    DictateToolTip.Image = Nothing
                    DictateToolTip.ShowHTML("<b>Error: </b>" & [Error].Message, TextBox, ToolTipLocation, 5000)
                End If

            End If
        End Sub

        Private Shared Sub CommitRecognition()
            If DictateCancel = False Then
                'write the spoken text
                If tmpText <> "" Then
                    ExistingText &= " " & tmpText
                End If
                RecognitionTextBox.SelectedText = Trim(ExistingText) & " "
                DictateCancel = True
            End If
            RecognitionTextBox = Nothing
        End Sub

        Private Shared Sub TextBox_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles RecognitionTextBox.LostFocus
            CancelRecognition()
        End Sub

        Private Shared Sub TextBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RecognitionTextBox.TextChanged
            CancelRecognition()
        End Sub

        Private Shared Sub TextBox_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RecognitionTextBox.MouseDown
            CancelRecognition()
        End Sub

        Private Shared Sub DictateToolTip_TipClick(ByVal sender As Object, ByVal e As i00SpellCheck.HTMLToolTip.TipClickEventArgs) Handles DictateToolTip.TipClick
            CommitRecognition()
        End Sub

        Private Shared Sub DictateToolTip_TipClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles DictateToolTip.TipClosed
            CommitRecognition()

            If DictateToolTip IsNot Nothing Then
                Try
                    DictateToolTip.Dispose()
                Catch ex As Exception

                End Try
            End If
            DictateToolTip = Nothing

            If Recogniser IsNot Nothing Then
                Try
                    Recogniser.RecognizeAsyncStop()
                Catch ex As Exception

                End Try
                Recogniser = Nothing
            End If
        End Sub

        Private Shared strYouSaid As String = "<i><font color=" & ListeningColor & ">You Said:</font></i> "

        Private Shared Sub Recogniser_SpeechHypothesized(ByVal sender As Object, ByVal e As System.Speech.Recognition.SpeechHypothesizedEventArgs) Handles Recogniser.SpeechHypothesized
            If DictateCancel Then Exit Sub
            tmpText = e.Result.Text
            DictateToolTip.ShowHTML(strYouSaid & ExistingText & "<font color=" & ListeningColor & ">" & tmpText & "</font>", RecognitionTextBox, ToolTipLocation)
        End Sub

        Private Shared Sub Recogniser_SpeechRecognized(ByVal sender As Object, ByVal e As System.Speech.Recognition.SpeechRecognizedEventArgs) Handles Recogniser.SpeechRecognized
            If DictateCancel Then Exit Sub
            tmpText = ""
            If Trim(e.Result.Text) <> "" Then
                ''for error coloring
                'For Each item In e.Result.Words
                '    If (item.DisplayAttributes And Speech.Recognition.DisplayAttributes.ConsumeLeadingSpaces) = Speech.Recognition.DisplayAttributes.ConsumeLeadingSpaces AndAlso ExistingText <> "" Then
                '        ExistingText = ExistingText.TrimEnd()
                '    End If
                '    ExistingText &= If(item.Confidence < 0.25, "<font color=" & InCorrectColor & "><b>", "") & item.Text & If(item.Confidence < 0.25, "</b></font>", "")
                '    If (item.DisplayAttributes And Speech.Recognition.DisplayAttributes.OneTrailingSpace) = Speech.Recognition.DisplayAttributes.OneTrailingSpace Then
                '        ExistingText &= " "
                '    ElseIf (item.DisplayAttributes And Speech.Recognition.DisplayAttributes.TwoTrailingSpaces) = Speech.Recognition.DisplayAttributes.TwoTrailingSpaces Then
                '        ExistingText &= "  "
                '    End If
                'Next

                ExistingText &= e.Result.Text & " "

                ''to play audio
                'Dim s As New IO.MemoryStream
                'e.Result.Audio.WriteToWaveStream(s)
                's.Position = 0
                'My.Computer.Audio.Play(s, AudioPlayMode.Background)

                If DictateToolTip IsNot Nothing Then
                    DictateToolTip.ShowHTML(strYouSaid & ExistingText, RecognitionTextBox, ToolTipLocation, 3000)
                End If
            End If
        End Sub

#End Region

#Region "Synthesis"

#Region "Pipe server/client to stop synthesis across applications that use i00 TextBoxSpeechRecognition"

        Private Shared WithEvents PipeServer As New Controls.PipeServer("TextBoxSpeechRecognition")
        Private Shared WithEvents PipeClient As New Controls.PipeClient("TextBoxSpeechRecognition")

        Private Shared Sub PipeServer_CommandRecieved(ByVal Command As String) Handles PipeServer.CommandRecieved
            Select Case Command
                Case "STOP"
                    'StopTalking
                    CancelSynthesis()
            End Select
        End Sub

#End Region

        Friend Shared WithEvents Synthesizer As Speech.Synthesis.SpeechSynthesizer
        Friend Shared SynthesisTextBox As TextBoxBase
        Friend Shared Sub CancelSynthesis()
            StopSpeaking()
            If SpeechEngineHelper.Synthesizer IsNot Nothing Then
                SpeechEngineHelper.Synthesizer.SpeakAsyncCancelAll()
            End If
        End Sub

        Shared SystemTray As New clsSystemTray
        Private Class clsSystemTray
            Inherits ContextMenuStrip

            Public TrayIcon As New NotifyIcon With {.Text = "Speech - " & System.Reflection.Assembly.GetEntryAssembly.GetName.Name, .ContextMenuStrip = Me}

            'Controls
            Public WithEvents tsiHeading As New i00SpellCheck.MenuTextSeperator

            Public WithEvents tsiSpeakingContent As New i00SpellCheck.HTMLMenuItem("")

            Public tsiStopSpeak As New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem("Stop Speaking", StopImage)

            Public tsiSpeechProgress As New TextBoxSpeechRecognition.tsiStandardProgress With {.Width = 400}

            Public Sub New()

                TrayIcon.Icon = Icon.FromHandle(TalkImage.GetHicon)

                tsiHeading.Text = "Speech - " & System.Reflection.Assembly.GetEntryAssembly.GetName.Name
                Me.Items.Add(tsiHeading)

                AddHandler tsiStopSpeak.Click, AddressOf tsiStopSpeak_Click
                Me.Items.Add(tsiStopSpeak)
                SpeechEngineHelper.tsiStopSpeak = tsiStopSpeak

                Me.Items.Add(tsiSpeechProgress)

                Me.Items.Add(tsiSpeakingContent)

            End Sub

        End Class

        Friend Shared Sub DoSynthesis(ByVal TextBox As TextBoxBase)
            StopOtherInstancesFromSpeaking()

            'If ShowSpeakInSystemTray Then
            If SpeechEngineHelper.Synthesizer Is Nothing Then
                SpeechEngineHelper.Synthesizer = New Speech.Synthesis.SpeechSynthesizer
            Else
                SpeechEngineHelper.Synthesizer.SpeakAsyncCancelAll()
            End If
            mc_SpeechProgress = 0
            If TextBox.SelectionLength = 0 Then
                mc_LastSpeechText = TextBox.Text
            Else
                mc_LastSpeechText = TextBox.SelectedText
            End If

            Dim SpeekThis = mc_LastSpeechText

            'this makes the text formated how it will be in the tsiSpeakingContent object... for nice spacing...
            SystemTray.tsiSpeakingContent.HTMLText = Replace(Replace(Replace(mc_LastSpeechText, vbCrLf, "<br>"), vbCr, "<br>"), vbLf, "<br>")
            mc_LastSpeechText = Join((From xItem In Split(SystemTray.tsiSpeakingContent.GetHTMLInfo(400).GetHTMLText, vbCrLf) Where Trim(xItem) <> "").ToArray, vbCrLf)
            SystemTray.tsiSpeakingContent.HTMLText = Top3LinesOfSpokenText()

            If mc_LastSpeechText Is Nothing Then Exit Sub
            SynthesisTextBox = TextBox

            SpeechEngineHelper.Synthesizer.SpeakAsync(Replace(mc_LastSpeechText, vbCrLf, "  "))

        End Sub

        Public Shared Function Top3LinesOfSpokenText() As String
            Top3LinesOfSpokenText = "<font color=" & NotSpeakingColor & ">"
            Dim arr = Split(mc_LastSpeechText, vbCrLf, 4)
            If arr.Length > 3 Then
                arr(UBound(arr)) = ""
                Top3LinesOfSpokenText &= Join(arr, vbCrLf).TrimEnd(CChar(vbCr), CChar(vbLf))
            Else
                Top3LinesOfSpokenText &= mc_LastSpeechText
            End If
            Top3LinesOfSpokenText &= "</font>"
        End Function

        Private Shared mc_LastSpeechText As String
        Private Shared mc_SpeechProgress As Single

        Public Shared ReadOnly Property SpeechText() As String
            Get
                Return mc_LastSpeechText
            End Get
        End Property

        Public Shared ReadOnly Property SpeechProgress() As Single
            Get
                Return mc_SpeechProgress
            End Get
        End Property

        Private Shared Sub Synthesizer_SpeakCompleted(ByVal sender As Object, ByVal e As System.Speech.Synthesis.SpeakCompletedEventArgs) Handles Synthesizer.SpeakCompleted
            StopSpeaking()
        End Sub

        Public Shared Event SynthesisStarted(ByVal sender As Object, ByVal e As EventArgs)
        Private Shared Sub Synthesizer_SpeakStarted(ByVal sender As Object, ByVal e As System.Speech.Synthesis.SpeakStartedEventArgs) Handles Synthesizer.SpeakStarted
            RaiseEvent SynthesisStarted(Nothing, EventArgs.Empty)
            If SynthesisTextBox IsNot Nothing AndAlso SynthesisTextBox.IsDisposed = False Then
                Dim TextBoxSpeechRecognition = SynthesisTextBox.ExtensionCast(Of TextBoxSpeechRecognition)()
                If TextBoxSpeechRecognition.ShowSpeakInSystemTray Then
                    'show in system tray...
                    SystemTray.TrayIcon.Visible = True
                End If
            End If
        End Sub

        Private Shared Sub StopOtherInstancesFromSpeaking()
            PipeClient.SendCommand("STOP")
        End Sub

        Public Shared Event SynthesisStoped(ByVal sender As Object, ByVal e As EventArgs)
        Private Shared Sub StopSpeaking()
            RaiseEvent SynthesisStoped(Nothing, EventArgs.Empty)
            SystemTray.TrayIcon.Visible = False
            SystemTray.Visible = False
            mc_SpeechProgress = 0
            If tsiSpeakingContent IsNot Nothing AndAlso tsiSpeakingContent.IsDisposed = False Then
                tsiSpeakingContent.Visible = False
            End If
            If tsiStopSpeak IsNot Nothing AndAlso tsiStopSpeak.IsDisposed = False Then
                tsiStopSpeak.Visible = False
            End If
            If tsiSpeechProgress IsNot Nothing AndAlso tsiSpeechProgress.IsDisposed = False Then
                tsiSpeechProgress.Visible = False
            End If
        End Sub

        Private Shared SpeakingColor As String = System.Drawing.ColorTranslator.ToHtml(Color.FromArgb(255, Color.FromKnownColor(KnownColor.HotTrack)))
        Private Shared NotSpeakingColor As String = System.Drawing.ColorTranslator.ToHtml(i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.MenuText), Color.FromKnownColor(KnownColor.Menu), 95))

        Friend Shared Function GetDisplaySpeakContentText(ByVal e As System.Speech.Synthesis.SpeakProgressEventArgs) As String
            'just show the current line that we are on ...
            Dim Lines As Integer = 1
            Dim CurrentText = Left(mc_LastSpeechText, e.CharacterPosition)

            Dim arrStartLines = Split(CurrentText, vbCrLf)
            CurrentText = ""
            If arrStartLines.Count > 1 Then
                CurrentText &= arrStartLines(UBound(arrStartLines) - 1) & "<br>"
                Lines += 1
            End If

            'This Line
            Dim thisLine = arrStartLines.Last
            Dim CurrentText2 = mc_LastSpeechText.Substring(e.CharacterPosition, e.CharacterCount)
            'go back to the last space
            Dim SpaceIndex = thisLine.LastIndexOf(" ")
            If SpaceIndex = -1 Then
                'no space
                thisLine = "<font color=" & SpeakingColor & ">" & thisLine
            Else
                'go back to the previous space
                thisLine = Left(thisLine, SpaceIndex + 1) & "<font color=" & SpeakingColor & ">" & thisLine.Substring(SpaceIndex + 1)
            End If
            thisLine &= CurrentText2 'the current spoken word
            CurrentText2 = mc_LastSpeechText.Substring(e.CharacterPosition + e.CharacterCount)
            Dim arrEndLines = Split(CurrentText2, vbCrLf)
            Dim thisLineEnd = arrEndLines.First
            'go to the next space
            SpaceIndex = thisLineEnd.IndexOf(" ")
            If SpaceIndex = -1 Then
                'no space
                thisLineEnd = thisLineEnd & "</font>"
            Else
                'go to the next space
                thisLineEnd = Left(thisLineEnd, SpaceIndex) & "</font>" & thisLineEnd.Substring(SpaceIndex)
            End If
            thisLine &= thisLineEnd

            CurrentText &= thisLine

            If arrEndLines.Count > 1 Then
                CurrentText &= "<br>" & arrEndLines(1)
                Lines += 1
            End If

            If Lines < 3 Then
                If arrStartLines.Count > 2 Then
                    'add another start line...
                    CurrentText = arrStartLines(UBound(arrStartLines) - 2) & "<br>" & CurrentText
                    Lines += 1
                Else
                    'add another end line...
                    If arrEndLines.Count > 2 Then
                        CurrentText &= "<br>" & arrEndLines(2)
                        Lines += 1
                    End If
                End If
            End If
            Return "<font color=" & NotSpeakingColor & ">" & CurrentText & "</font>"
        End Function

        Public Shared Event SpeakProgress(ByVal sender As Object, ByVal e As System.Speech.Synthesis.SpeakProgressEventArgs)
        Private Shared Sub Synthesizer_SpeakProgress(ByVal sender As Object, ByVal e As System.Speech.Synthesis.SpeakProgressEventArgs) Handles Synthesizer.SpeakProgress
            mc_SpeechProgress = CSng(e.CharacterPosition / mc_LastSpeechText.Length)
            RaiseEvent SpeakProgress(Nothing, e)

            If SystemTray.Visible Then
                'menu is showing
                SystemTray.tsiSpeechProgress.Progress = mc_SpeechProgress

                SystemTray.tsiSpeakingContent.HTMLText = GetDisplaySpeakContentText(e)
                SystemTray.tsiSpeakingContent.Invalidate()
            End If

            If tsiSpeechProgress IsNot Nothing AndAlso tsiSpeechProgress.IsDisposed = False AndAlso tsiSpeechProgress.Visible Then
                tsiSpeechProgress.Progress = mc_SpeechProgress
            End If

            If tsiSpeakingContent IsNot Nothing AndAlso tsiSpeakingContent.IsDisposed = False AndAlso tsiSpeakingContent.Visible Then
                tsiSpeakingContent.HTMLText = GetDisplaySpeakContentText(e)
                tsiSpeakingContent.Invalidate()
            End If

            ''estimate time remaining
            'If SpeechProgress <> 0 Then
            '    Dim ts = New TimeSpan(CLng(TimeSpan.TicksPerMillisecond * e.AudioPosition.TotalMilliseconds * (1 / SpeechProgress)))
            '    Debug.Print(e.AudioPosition.ToString & " / " & ts.ToString)
            'End If
        End Sub

        Friend Shared tsiStopSpeak As i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem
        Friend Shared tsiSpeechProgress As TextBoxSpeechRecognition.tsiStandardProgress
        Friend Shared tsiSpeakingContent As tsiStandardHTMLMenuItem

#End Region

    End Class

End Class
