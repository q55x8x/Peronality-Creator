'i00 .Net Control Extensions - TextBoxSpeechRecognition
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

'The weight is the priority order of the plugins ... if there are multiple plugins that extend the same control ...
'in this case the plugin with the heighest weight gets used first...
'all built in plugins in i00SpellCheck have a weight of 0
<i00SpellCheck.PluginWeight(1)> _
Public Class TextBoxSpeechRecognition
    Inherits i00SpellCheck.ControlExtension
    Implements iTestHarness

#Region "Constructor"

    'called when the control is loaded...
    Public Overrides Sub Load()
        parentTextBoxBase = DirectCast(MyBase.Control, TextBoxBase)

        Me.parentRichTextBox = TryCast(MyBase.Control, RichTextBox)

        extTextBoxContextMenu = parentTextBoxBase.ExtensionCast(Of extTextBoxContextMenu)()
    End Sub

#End Region

#Region "Plugin info"

    Public Overrides ReadOnly Property RequiredExtensions() As System.Collections.Generic.List(Of System.Type)
        Get
            RequiredExtensions = New List(Of System.Type)
            RequiredExtensions.Add(GetType(extTextBoxContextMenu))
        End Get
    End Property

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(TextBoxBase)}
        End Get
    End Property

#End Region

#Region "Ease Of Access"

    Private WithEvents parentTextBoxBase As TextBoxBase
    Private WithEvents parentRichTextBox As RichTextBox
    Private WithEvents extTextBoxContextMenu As extTextBoxContextMenu

#End Region

#Region "Images"

    Friend Shared ReadOnly Property TalkImage() As Bitmap
        Get
            Static b As Bitmap
            If b Is Nothing Then
                b = New Bitmap(16, 16)
                Using g = Graphics.FromImage(b)
                    g.InterpolationMode = Drawing2D.InterpolationMode.High
                    g.DrawImage(My.Resources.Talk, New Rectangle(0, 0, b.Width, b.Height))
                End Using
            End If
            Return b
        End Get
    End Property

    Friend Shared ReadOnly Property StopImage() As Bitmap
        Get
            Static b As Bitmap
            If b Is Nothing Then
                b = New Bitmap(16, 16)
                Using g = Graphics.FromImage(b)
                    g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                    Dim RectMargin As Single = CSng(b.Height / 6)
                    Dim r As New RectangleF(RectMargin, RectMargin, b.Width - (RectMargin * 2), b.Height - (RectMargin * 2))
                    Using lgb As New System.Drawing.Drawing2D.LinearGradientBrush(r, Color.Red, Color.Maroon, Drawing2D.LinearGradientMode.ForwardDiagonal)
                        g.FillRectangle(lgb, r)
                    End Using
                    Using p As New Pen(Color.FromArgb(127, Color.Red))
                        g.DrawRectangle(p, r.X, r.Y, r.Width, r.Height)
                    End Using
                End Using
            End If
            Return b
        End Get
    End Property

    Friend Shared ReadOnly Property MicImage() As Bitmap
        Get
            Static b As Bitmap
            If b Is Nothing Then
                b = New Bitmap(16, 16)
                Using g = Graphics.FromImage(b)
                    g.InterpolationMode = Drawing2D.InterpolationMode.High
                    g.DrawImage(My.Resources.Microphone, New Rectangle(0, 0, b.Width, b.Height))
                End Using
            End If
            Return b
        End Get
    End Property

    Private Shared ReadOnly Property MicImageBW() As Bitmap
        Get
            Static b As Bitmap
            If b Is Nothing Then
                b = DirectCast(MicImage.Clone, Bitmap)
                b.Filters.AlphaMask(Color.Transparent, Color.FromArgb(127, 0, 0, 0))
            End If
            Return b
        End Get
    End Property

#End Region

#Region "For the double click F12"

    Private Sub parentTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles parentTextBoxBase.KeyUp
        Static LastF12Press As New Date
        If mc_AllowF12Dictate AndAlso e.KeyCode = Keys.F12 Then
            If (Now.Subtract(LastF12Press).TotalMilliseconds < SystemInformation.DoubleClickTime) Then
                DoDictate()
                LastF12Press = New Date
            Else
                LastF12Press = Now
            End If
        Else
            LastF12Press = New Date
        End If
    End Sub

#End Region

#Region "For the test control"

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As System.Windows.Forms.Control Implements i00SpellCheck.iTestHarness.SetupControl
        If Control.GetType Is GetType(TextBox) Then
            Dim TextBox = DirectCast(Control, TextBox)

            TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12)
            TextBox.Multiline = True
            TextBox.ScrollBars = ScrollBars.Vertical
            TextBox.AppendText(If(TextBox.Text = "", "", vbCrLf & vbCrLf) & "The TextBoxSpeechRecognition project now adds speech recognition functionality to any TextBox, simply press F12 twice (quickly), or right click and press Dictate to dictate what you want written!")

            TextBox.SelectionStart = 0
            TextBox.SelectionLength = 0

            Return TextBox
        ElseIf Control.GetType Is GetType(RichTextBox) Then
            Dim RichTextBox = DirectCast(Control, RichTextBox)

            RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!)

            RichTextBox.AppendText(If(RichTextBox.Text = "", "", vbCrLf & vbCrLf) & "The TextBoxSpeechRecognition project now adds speech recognition functionality to any TextBox, simply press F12 twice (quickly), or right click and press Dictate to dictate what you want written!")

            Dim HighlightKeyWordFormat As New extTextBoxCommon.HighlightKeyWordFormat
            HighlightKeyWordFormat.Color = Color.FromKnownColor(KnownColor.HotTrack)

            extTextBoxCommon.HighlightKeyWord(RichTextBox, "TextBoxSpeechRecognition", HighlightKeyWordFormat)

            RichTextBox.Select(0, 0)
            RichTextBox.ClearUndo()

            Return RichTextBox
        Else
            Return Nothing
        End If

    End Function
    'Public Overrides Function SetupControl(ByVal Control As System.Windows.Forms.Control) As Control
    '    SetupControl = MyBase.SetupControl(Control)
    '    Dim TextBoxBase = TryCast(SetupControl, TextBoxBase)
    '    If TextBoxBase IsNot Nothing Then
    '        TextBoxBase.AppendText(vbCrLf & vbCrLf & "The TextBoxSpeechRecognition project now adds speech recognition functionality to any TextBox, simply press F12 twice (quickly), or right click and press Dictate to dictate what you want written!")
    '    End If
    'End Function

#End Region

#Region "Properties"

    Dim mc_AllowF12Dictate As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow F12 x2 to Dictate")> _
    <System.ComponentModel.Description("Enables the double pressing of the F12 key to initialise dictation")> _
    Public Property AllowF12Dictate() As Boolean
        Get
            Return mc_AllowF12Dictate
        End Get
        Set(ByVal value As Boolean)
            mc_AllowF12Dictate = value
        End Set
    End Property

    Dim mc_ShowSpeakInMenu As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Show Speak in Menu")> _
    <System.ComponentModel.Description("Shows the option to speak the text in the right click context menu")> _
    Public Property ShowSpeakInMenu() As Boolean
        Get
            Return mc_ShowSpeakInMenu
        End Get
        Set(ByVal value As Boolean)
            mc_ShowSpeakInMenu = value
        End Set
    End Property


    Dim mc_ShowSpeakInSystemTray As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Show Speak in System Tray")> _
    <System.ComponentModel.Description("Determines if the speech icon is shown in the system tray while speaking")> _
    Public Property ShowSpeakInSystemTray() As Boolean
        Get
            Return mc_ShowSpeakInSystemTray
        End Get
        Set(ByVal value As Boolean)
            mc_ShowSpeakInSystemTray = value
        End Set
    End Property


    Dim mc_ShowDictateInMenu As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Show Dictate in Menu")> _
    <System.ComponentModel.Description("Shows the option to allow the user to dictate what they want typed in the right click context menu")> _
    Public Property ShowDictateInMenu() As Boolean
        Get
            Return mc_ShowDictateInMenu
        End Get
        Set(ByVal value As Boolean)
            mc_ShowDictateInMenu = value
        End Set
    End Property

#End Region

#Region "Add menu items"

    Public Class tsiStandardProgress
        Inherits tsiProgress
        Implements i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripItem
    End Class

    Public Class tsiStandardHTMLMenuItem
        Inherits i00SpellCheck.HTMLMenuItem
        Implements i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripItem

        Public Sub New()
            MyBase.New("")
        End Sub
    End Class

    Private Sub extTextBoxContextMenu_MenuOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles extTextBoxContextMenu.MenuOpening
        Dim MenuItems As New List(Of ToolStripItem)

        If mc_ShowDictateInMenu Then
            Dim tsiDictate = New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem("Dictate", MicImage)
            AddHandler tsiDictate.Click, AddressOf tsiDictate_Click
            MenuItems.Add(tsiDictate)
        End If

        If mc_ShowSpeakInMenu Then
            Dim tsiSpeak = New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem("Speak", TalkImage)
            AddHandler tsiSpeak.Click, AddressOf tsiSpeak_Click
            MenuItems.Add(tsiSpeak)

            If SpeechEngineHelper.Synthesizer IsNot Nothing AndAlso SpeechEngineHelper.Synthesizer.State = Speech.Synthesis.SynthesizerState.Speaking Then

                'stop option
                Dim tsiStopSpeak = New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem("Stop Speaking", StopImage)
                AddHandler tsiStopSpeak.Click, AddressOf tsiStopSpeak_Click
                MenuItems.Add(tsiStopSpeak)
                SpeechEngineHelper.tsiStopSpeak = tsiStopSpeak

                'progress
                Dim tsiSpeakProgress = New tsiStandardProgress()
                tsiSpeakProgress.Progress = SpeechEngineHelper.SpeechProgress
                tsiSpeakProgress.Width = 400
                MenuItems.Add(tsiSpeakProgress)
                SpeechEngineHelper.tsiSpeechProgress = tsiSpeakProgress

                'content
                Dim tsiSpeakingContent = New tsiStandardHTMLMenuItem
                tsiSpeakingContent.HTMLText = SpeechEngineHelper.Top3LinesOfSpokenText
                MenuItems.Add(tsiSpeakingContent)
                SpeechEngineHelper.tsiSpeakingContent = tsiSpeakingContent

            End If
        End If

        If MenuItems.Count > 0 Then
            If extTextBoxContextMenu.ContextMenuStrip.Items.Count > 0 Then
                extTextBoxContextMenu.ContextMenuStrip.Items.Add(New extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripSeparator)
            End If
            For Each item In MenuItems
                extTextBoxContextMenu.ContextMenuStrip.Items.AddRange(MenuItems.ToArray)
            Next
        End If

    End Sub

    Private Shared Sub tsiStopSpeak_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        CancelSynthesis()
    End Sub

    Private Sub tsiSpeak_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        DoSynthesis()
    End Sub

    Private Sub tsiDictate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If extTextBoxContextMenu.LastMenuSpellClickReturn IsNot Nothing AndAlso extTextBoxContextMenu.LastMenuSpellClickReturn.UseMouseLocation Then
            parentTextBoxBase.SelectionStart = extTextBoxContextMenu.LastMenuSpellClickReturn.WordStart
            parentTextBoxBase.SelectionLength = 0
        End If
        DoDictate()
    End Sub

    Public Shared Sub CancelSynthesis()
        SpeechEngineHelper.CancelSynthesis()
    End Sub

    Public Sub DoSynthesis()
        SpeechEngineHelper.DoSynthesis(parentTextBoxBase)
    End Sub

    Public Sub DoDictate()
        SpeechEngineHelper.DoDictate(parentTextBoxBase, RTBContents)
    End Sub

#End Region

    Dim RTBContents As Rectangle
    Private Sub parentRichTextBox_ContentsResized(ByVal sender As Object, ByVal e As System.Windows.Forms.ContentsResizedEventArgs) Handles parentRichTextBox.ContentsResized
        RTBContents = e.NewRectangle
    End Sub

End Class
