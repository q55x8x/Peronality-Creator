'i00 .Net Control Extensions - KonamiCode
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

Imports Sanford.Multimedia.Midi
Imports Sanford.Multimedia.Midi.UI

Public Class KonamiCode
    Inherits ControlExtension
    Implements iTestHarness

#Region "Ease of access"

    Private WithEvents parentControl As Control

#End Region

#Region "Underlying Control"

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(Control)}
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Overrides Sub Load()
        parentControl = Control
    End Sub

#End Region

#Region "Properties"

    Dim mc_Enabled As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.Description("Enable the magic!")> _
    <System.ComponentModel.Category("Konami")> _
    Public Property Enabled() As Boolean
        Get
            Return mc_Enabled
        End Get
        Set(ByVal value As Boolean)
            mc_Enabled = value
        End Set
    End Property

#End Region

#Region "Konami Code Check"

    Private Shared Code As Keys() = New Keys() {Keys.Up, Keys.Up, Keys.Down, Keys.Down, Keys.Left, Keys.Right, Keys.Left, Keys.Right, Keys.B, Keys.A, Keys.Enter}

    Private Sub parentControl_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles parentControl.KeyUp
        If parentControl.Focused = True AndAlso Enabled Then
            If PressedKeys Is Nothing Then
                PressedKeys = New List(Of Keys)
            End If
            PressedKeys.Add(e.KeyCode)
            'Debug.Print(parentControl.Name & " - " & Join(PressedKeys.Select(Function(x) x.ToString).ToArray, " "))

            For i = 0 To PressedKeys.Count - 1
                Dim iPressedKey = PressedKeys(i)
                If i > UBound(Code) OrElse iPressedKey <> Code(i) Then
                    PressedKeys = Nothing
                    Return
                End If
            Next
            If PressedKeys.Count = UBound(Code) + 1 Then
                PressedKeys = Nothing
                Magic()
            End If
        End If
    End Sub

    'Private Sub parentControl_LostFocus(ByVal sender As Object, ByVal e As EventArgs) Handles parentControl.LostFocus
    '    PressedKeys = Nothing
    'End Sub

    Private Shared PressedKeys As List(Of Keys)

#End Region

#Region "Magic"

    Private Class KonamiForm
        Inherits Form
        Dim outDevice As OutputDevice
        Private WithEvents sequence As New Sequence
        Private WithEvents sequencer As New Sequencer With {.sequence = sequence}
        Private WithEvents pianoControl As New PianoControl With {.Width = 424, .Height = 48, .Dock = DockStyle.Fill}
        Private WithEvents progress As New ProgressBar With {.Height = 8, .Dock = DockStyle.Bottom}
        Private WithEvents timer As New Timer With {.Interval = 1000, .Enabled = True}

        Private WithEvents toolstrip As New ToolStrip With {.GripStyle = ToolStripGripStyle.Hidden, .Dock = DockStyle.Top}
        Private WithEvents tsiPrevious As New ToolStripButton With {.ToolTipText = "", .DisplayStyle = ToolStripItemDisplayStyle.Image, .Image = My.Resources.Previous}
        Private WithEvents tsiStop As New ToolStripButton With {.ToolTipText = "", .DisplayStyle = ToolStripItemDisplayStyle.Image, .Image = My.Resources._Stop}
        Private WithEvents tsiPlay As New ToolStripButton With {.ToolTipText = "", .DisplayStyle = ToolStripItemDisplayStyle.Image, .Image = My.Resources.Play}
        Private WithEvents tsiNext As New ToolStripButton With {.ToolTipText = "", .DisplayStyle = ToolStripItemDisplayStyle.Image, .Image = My.Resources._Next}

        Private WithEvents pnlImage As New Panel With {.Height = toolstrip.Height + pianoControl.Height + progress.height, .Width = .Height, .Dock = DockStyle.Left, .MaximumSize = .Size, .BackgroundImageLayout = ImageLayout.Zoom}

        Private Sub KonamiForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
            timer.Stop()

            Try
                sequence.Dispose()
            Catch ex As Exception

            End Try
            Try
                sequencer.Dispose()
            Catch ex As Exception

            End Try
            If outDevice IsNot Nothing Then
                Try
                    outDevice.Dispose()
                Catch ex As Exception

                End Try
            End If
        End Sub

        Private Class MusicFileData
            Public Game As String
            Public TrackName As String
            Public FileData As Byte()
            Public AlbumArt As Image
        End Class

        Private Shared ReadOnly Property MusicFiles(Optional ByVal Randomize As Boolean = False) As List(Of MusicFileData)
            Get
                Static mc_MusicFiles As List(Of MusicFileData) = Nothing
                If mc_MusicFiles Is Nothing Then
                    mc_MusicFiles = New List(Of MusicFileData)
                    Dim MusicFileData As MusicFileData

                    MusicFileData = New MusicFileData
                    MusicFileData.Game = "Gradius"
                    MusicFileData.TrackName = "Level 1"
                    MusicFileData.FileData = My.Resources.Gradius___Level_1
                    MusicFileData.AlbumArt = My.Resources.Gradius
                    mc_MusicFiles.Add(MusicFileData)

                    MusicFileData = New MusicFileData
                    MusicFileData.Game = "Contra"
                    MusicFileData.TrackName = "Level 1"
                    MusicFileData.FileData = My.Resources.Contra___Level_1
                    MusicFileData.AlbumArt = My.Resources.Contra
                    mc_MusicFiles.Add(MusicFileData)

                    MusicFileData = New MusicFileData
                    MusicFileData.Game = "Castlevania"
                    MusicFileData.TrackName = "Level 1"
                    MusicFileData.FileData = My.Resources.Castlevania___Level_1
                    MusicFileData.AlbumArt = My.Resources.Castlevania
                    mc_MusicFiles.Add(MusicFileData)

                    MusicFileData = New MusicFileData
                    MusicFileData.Game = "Teenage Mutant Ninja Turtles II: The Arcade Game"
                    MusicFileData.TrackName = "Select Screen"
                    MusicFileData.FileData = My.Resources.TMNT___Select_Screen
                    MusicFileData.AlbumArt = My.Resources.TMNT
                    mc_MusicFiles.Add(MusicFileData)

                End If

                If Randomize Then
                    Dim r As New Random
                    mc_MusicFiles = mc_MusicFiles.OrderBy(Function(x) r.Next).ToList
                End If

                Return mc_MusicFiles
            End Get
        End Property

        Private mc_CurrentMusicFile As MusicFileData
        Private Property CurrentMusicFile() As MusicFileData
            Get
                If mc_CurrentMusicFile Is Nothing Then
                    mc_CurrentMusicFile = MusicFiles.First
                End If
                Return mc_CurrentMusicFile
            End Get
            Set(ByVal value As MusicFileData)
                mc_CurrentMusicFile = value
            End Set
        End Property

        Public Sub Play()
            'play file...
            sequencer.Stop()
            Try
                sequence.LoadAsync(CurrentMusicFile.FileData)
            Catch ex As Exception

            End Try
            pnlImage.BackgroundImage = CurrentMusicFile.AlbumArt
            SetMeText(CurrentMusicFile.Game & " - " & CurrentMusicFile.TrackName)
            EnableDisablePlay(False)
        End Sub

        Public Function SetMeText(ByVal Text As String) As Boolean
            If Me.InvokeRequired Then
                Me.Invoke(Function() SetMeText(Text))
                Return True
            Else
                Me.Text = Text
                Return False
            End If
        End Function

        Public Sub New()
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow
        End Sub

        Private Sub KonamiForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.Width = pnlImage.Width + pianoControl.Width + (Me.Width - Me.ClientSize.Width)
            Me.Height = toolstrip.Height + pianoControl.Height + progress.Height + (Me.Height - Me.ClientSize.Height)
            Me.Text = "Konami Code"
            Me.Controls.Add(pianoControl)
            Me.Controls.Add(toolstrip)
            Me.Controls.Add(progress)
            Me.Controls.Add(pnlImage)
            pianoControl.Visible = True

            toolstrip.Items.Add(tsiPrevious)
            toolstrip.Items.Add(tsiStop)
            toolstrip.Items.Add(tsiPlay)
            toolstrip.Items.Add(tsiNext)

            If OutputDevice.DeviceCount = 0 Then
                'fail no sound card :(
            Else
                Try
                    outDevice = New OutputDevice(0)
                Catch ex As Exception

                End Try
            End If

            'randomize the music each time we open...
            Dim Music = MusicFiles(True)
            Play()

        End Sub

        Private Sub sequence_LoadCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs) Handles sequence.LoadCompleted
            If e.Error Is Nothing Then
                sequencer.Start()
            End If
        End Sub

        Private Sub sequencer_ChannelMessagePlayed(ByVal sender As Object, ByVal e As Sanford.Multimedia.Midi.ChannelMessageEventArgs) Handles sequencer.ChannelMessagePlayed
            outDevice.Send(e.Message)
            pianoControl.Send(e.Message)
        End Sub

        Private Sub sequencer_Chased(ByVal sender As Object, ByVal e As Sanford.Multimedia.Midi.ChasedEventArgs) Handles sequencer.Chased
            For Each Message As ChannelMessage In e.Messages
                outDevice.Send(Message)
            Next
        End Sub

        Private Sub sequencer_PlayingCompleted(ByVal sender As Object, ByVal e As System.EventArgs) Handles sequencer.PlayingCompleted
            'load next song...
            PlayOffset(1)
        End Sub

        Private Sub sequencer_Stopped(ByVal sender As Object, ByVal e As Sanford.Multimedia.Midi.StoppedEventArgs) Handles sequencer.Stopped
            For Each Message As ChannelMessage In e.Messages
                outDevice.Send(Message)
                pianoControl.Send(Message)
            Next

            EnableDisablePlay(True)
        End Sub

        Private Function EnableDisablePlay(ByVal enable As Boolean) As Boolean
            If Me.InvokeRequired Then
                Me.Invoke(Function() EnableDisablePlay(enable))
                Return True
            Else
                tsiStop.Enabled = Not enable
                tsiPlay.Enabled = enable
                Return False
            End If
        End Function

        Private Sub timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles timer.Tick
            Dim len = sequence.GetLength
            If len <> 0 Then
                progress.Value = CInt((sequencer.Position / sequence.GetLength) * progress.Maximum)
            End If
        End Sub

        Private Sub pianoControl_PianoKeyDown(ByVal sender As Object, ByVal e As Sanford.Multimedia.Midi.UI.PianoKeyEventArgs) Handles pianoControl.PianoKeyDown
            outDevice.Send(New ChannelMessage(ChannelCommand.NoteOn, 0, e.NoteID, 127))
        End Sub

        Private Sub pianoControl_PianoKeyUp(ByVal sender As Object, ByVal e As Sanford.Multimedia.Midi.UI.PianoKeyEventArgs) Handles pianoControl.PianoKeyUp
            outDevice.Send(New ChannelMessage(ChannelCommand.NoteOff, 0, e.NoteID, 0))
        End Sub

        Private Sub tsiStop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiStop.Click
            sequencer.Stop()
        End Sub

        Private Sub tsiPlay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiPlay.Click
            If sequencer.Position > sequence.GetLength - 1000 Then
                sequencer.Position = 0
            End If
            sequencer.Continue()
            EnableDisablePlay(False)
        End Sub

        Private Sub PlayOffset(ByVal SongOffset As Integer)
            Dim Index = MusicFiles.IndexOf(CurrentMusicFile)
            If SongOffset < 0 Then
                'for neg numbers
                Index += (MusicFiles.Count) * (CInt(Math.Abs(SongOffset) / MusicFiles.Count) + 1)
            End If
            Index += SongOffset
            Index = Index Mod MusicFiles.Count
            CurrentMusicFile = MusicFiles()(Index)
            Play()
        End Sub

        Private Sub tsiNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiNext.Click
            PlayOffset(1)
        End Sub

        Private Sub tsiPrevious_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiPrevious.Click
            PlayOffset(-1)
        End Sub

    End Class

    Private Sub Magic()
        Using KonamiForm As New KonamiForm
            KonamiForm.ShowDialog()
        End Using
    End Sub

#End Region

#Region "iTestHarness"

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As System.Windows.Forms.Control Implements i00SpellCheck.iTestHarness.SetupControl

        If Control.GetType Is GetType(TextBox) Then
            Dim TextBox = DirectCast(Control, TextBox)

            TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12)
            TextBox.Multiline = True
            TextBox.ScrollBars = ScrollBars.Vertical
            TextBox.AppendText(If(TextBox.Text = "", "", vbCrLf & vbCrLf) & "KONAMI" & vbCrLf & "The source code for the midi player used in the Konami player can be downloaded from http://www.codeproject.com/Articles/6228/C-MIDI-Toolkit and is the property of Leslie Sanford.")

            TextBox.SelectionStart = 0
            TextBox.SelectionLength = 0
        ElseIf Control.GetType Is GetType(RichTextBox) Then
            Dim RichTextBox = DirectCast(Control, RichTextBox)
            RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!)
            RichTextBox.AppendText(If(RichTextBox.Text = "", "", vbCrLf & vbCrLf) & "KONAMI" & vbCrLf & "The source code for the midi player used in the Konami player can be downloaded from http://www.codeproject.com/Articles/6228/C-MIDI-Toolkit and is the property of Leslie Sanford.")
            Dim HighlightKeyWordFormat As New extTextBoxCommon.HighlightKeyWordFormat
            HighlightKeyWordFormat.Color = Color.FromArgb(255, 229, 30, 43)
            If RichTextBox.Font.FontFamily.IsStyleAvailable(FontStyle.Bold) Then
                HighlightKeyWordFormat.Font = New Font(RichTextBox.Font, FontStyle.Bold)
            End If
            extTextBoxCommon.HighlightKeyWord(RichTextBox, "KONAMI", HighlightKeyWordFormat)
        End If

        Return Control
    End Function

#End Region

End Class
