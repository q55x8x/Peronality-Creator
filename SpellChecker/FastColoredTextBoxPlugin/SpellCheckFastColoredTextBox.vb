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

Imports i00SpellCheck
Imports FastColoredTextBoxNS

Public Class SpellCheckFastColoredTextBox
    Inherits i00SpellCheck.SpellCheckControlBase
    Implements iTestHarness

#Region "Setup"

    'Called when the control is loaded
    Overrides Sub Load()
        parentFastColoredTextBox = DirectCast(Me.Control, FastColoredTextBox)

        SpellErrorStyle.FastColoredTextBox = parentFastColoredTextBox
        CaseErrorStyle.FastColoredTextBox = parentFastColoredTextBox
        IgnoreErrorStyle.FastColoredTextBox = parentFastColoredTextBox

        LoadSettingsToObjects()
    End Sub

    'Lets the EnableSpellCheck() know what ControlTypes we can spellcheck
    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(FastColoredTextBox)}
        End Get
    End Property

    Private Sub LoadSettingsToObjects()
        SpellErrorStyle.Color = Settings.MistakeColor
        IgnoreErrorStyle.Color = Settings.IgnoreColor
        CaseErrorStyle.Color = Settings.CaseMistakeColor
    End Sub

    'Repaint control when settings are changed
    Private Sub SpellCheckFastColoredTextBox_SettingsChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SettingsChanged
        LoadSettingsToObjects()
        RepaintControl()
    End Sub

    Dim mc_SpellCheckMatch As String = ""

    <System.ComponentModel.Description("This regular expression is used to set what text gets spell checked in the FastColoredTextBox")> _
    <System.ComponentModel.DisplayName("Spell Check Match")> _
    Public Property SpellCheckMatch() As String
        Get
            Return mc_SpellCheckMatch
        End Get
        Set(ByVal value As String)
            If mc_SpellCheckMatch <> value Then
                mc_SpellCheckMatch = value
                RepaintControl()
            End If
        End Set
    End Property


#End Region

#Region "Underlying Control"

    'Make underlying control appear nicer in property grids
    <System.ComponentModel.Category("Control")> _
    <System.ComponentModel.Description("The FastColoredTextBox associated with the SpellCheckFastColoredTextBox object")> _
    <System.ComponentModel.DisplayName("FastColoredTextBox")> _
    Public Overrides ReadOnly Property Control() As System.Windows.Forms.Control
        Get
            Return MyBase.Control
        End Get
    End Property

    'Quick access to underlying FastColoredTextBox object... with events...
    Private WithEvents parentFastColoredTextBox As FastColoredTextBox

#End Region

#Region "Click"

    Private WithEvents ErrorMenu As New ContextMenuStrip
    'Menu
    Private WithEvents SpellMenuItems As New Menu.AddSpellItemsToMenu() With {.ContextMenuStrip = ErrorMenu}

    'Error click
    Private Sub FastColoredTextBox_VisualMarkerClick(ByVal sender As Object, ByVal e As FastColoredTextBoxNS.VisualMarkerEventArgs) Handles parentFastColoredTextBox.VisualMarkerClick
        If parentFastColoredTextBox.ReadOnly Then Exit Sub
        Dim ErrorStyleMarker = TryCast(e.Marker, ErrorStyle.ErrorStyleMarker)
        If ErrorStyleMarker IsNot Nothing Then
            parentFastColoredTextBox.Selection = ErrorStyleMarker.Range

            SpellMenuItems.RemoveSpellMenuItems()
            SpellMenuItems.AddItems(ErrorStyleMarker.Range.Text, CurrentDictionary, CurrentDefinitions, CurrentSynonyms, Settings)
            ErrorMenu.Show(parentFastColoredTextBox, New Point(ErrorStyleMarker.rectangle.X, ErrorStyleMarker.rectangle.Bottom))
        End If
    End Sub

    Private Sub ErrorMenu_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles ErrorMenu.Closed
        SpellMenuItems.RemoveSpellMenuItems()
    End Sub

    Private Sub SpellMenuItems_WordAdded(ByVal sender As Object, ByVal e As i00SpellCheck.Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordAdded
        Try
            DictionaryAddWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error occured adding """ & e.Word & """ to the dictionary:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SpellMenuItems_WordChanged(ByVal sender As Object, ByVal e As i00SpellCheck.Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordChanged
        Dim Start = parentFastColoredTextBox.Selection.Start
        parentFastColoredTextBox.InsertText(e.Word)
        parentFastColoredTextBox.Selection = New Range(parentFastColoredTextBox, Start, parentFastColoredTextBox.Selection.End)
    End Sub

    Private Sub SpellMenuItems_WordIgnored(ByVal sender As Object, ByVal e As i00SpellCheck.Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordIgnored
        Try
            DictionaryIgnoreWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error ignoring """ & e.Word & """:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub SpellMenuItems_WordRemoved(ByVal sender As Object, ByVal e As i00SpellCheck.Menu.AddSpellItemsToMenu.SpellItemEventArgs) Handles SpellMenuItems.WordRemoved
        Try
            DictionaryRemoveWord(e.Word)
        Catch ex As Exception
            MsgBox("The following error occured removing """ & e.Word & """ from the dictionary:" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

#End Region

#Region "Painting"

#Region "Error Styles"

    Private WithEvents SpellErrorStyle As New ErrorStyle With {.WordState = Dictionary.SpellCheckWordError.SpellError}
    Private WithEvents IgnoreErrorStyle As New ErrorStyle With {.WordState = Dictionary.SpellCheckWordError.Ignore}
    Private WithEvents CaseErrorStyle As New ErrorStyle With {.WordState = Dictionary.SpellCheckWordError.CaseError}

    'Owner Draw Errors
    Private Sub ErrorStyle_SpellCheckErrorPaint(ByVal sender As Object, ByRef e As i00SpellCheck.SpellCheckControlBase.SpellCheckCustomPaintEventArgs) Handles SpellErrorStyle.SpellCheckErrorPaint, CaseErrorStyle.SpellCheckErrorPaint, IgnoreErrorStyle.SpellCheckErrorPaint
        OnSpellCheckErrorPaint(e)
    End Sub

#Region "Error Style"

    Private Class ErrorStyle
        Inherits Style

#Region "Marker"

        Public Class ErrorStyleMarker
            Inherits FastColoredTextBoxNS.StyleVisualMarker

            Public Overrides ReadOnly Property Cursor() As System.Windows.Forms.Cursor
                Get
                    Return Cursors.Default
                End Get
            End Property

            Public Range As Range

            Public Sub New(ByVal Range As Range, ByVal rectangle As Rectangle, ByVal style As FastColoredTextBoxNS.Style)
                MyBase.New(rectangle, style)
                Me.Range = Range
            End Sub
        End Class

#End Region

        Public Overrides Sub OnVisualMarkerClick(ByVal tb As FastColoredTextBoxNS.FastColoredTextBox, ByVal args As FastColoredTextBoxNS.VisualMarkerEventArgs)
            MyBase.OnVisualMarkerClick(tb, args)
        End Sub

        Public FastColoredTextBox As FastColoredTextBoxNS.FastColoredTextBox

        Public Event SpellCheckErrorPaint(ByVal sender As Object, ByRef e As SpellCheckCustomPaintEventArgs)
        Protected Sub OnSpellCheckErrorPaint(ByRef e As SpellCheckCustomPaintEventArgs)
            RaiseEvent SpellCheckErrorPaint(Me, e)
        End Sub

        Public WordState As Dictionary.SpellCheckWordError

        Public Overrides Sub Draw(ByVal gr As Graphics, ByVal position As Point, ByVal range As Range)
            Dim size As Size = Style.GetSizeOfRange(range)
            Dim rect As Rectangle = New Rectangle(position, size)

            Dim eArgs = New SpellCheckCustomPaintEventArgs With {.Graphics = gr, .Word = range.Text, .Bounds = rect, .WordState = WordState}
            OnSpellCheckErrorPaint(eArgs)
            Select Case WordState
                Case Dictionary.SpellCheckWordError.CaseError, Dictionary.SpellCheckWordError.SpellError
                    If eArgs.DrawDefault Then
                        DrawingFunctions.DrawWave(gr, New Point(rect.Left, rect.Bottom), New Point(rect.Right, rect.Bottom), Color)
                    End If
                    AddVisualMarker(FastColoredTextBox, New ErrorStyleMarker(range, rect, Me))
                Case Dictionary.SpellCheckWordError.Ignore
                    If eArgs.DrawDefault Then
                        Using p As New Pen(Color)
                            gr.DrawLine(p, New Point(rect.Left, rect.Bottom), New Point(rect.Right, rect.Bottom))
                        End Using
                    End If
                    AddVisualMarker(FastColoredTextBox, New ErrorStyleMarker(range, rect, Me))
            End Select
        End Sub

        Public Color As Color
    End Class

#End Region

#End Region

    Private WithEvents tmrRepaint As New Timer With {.Interval = 1, .Enabled = False}
    Public Overrides Sub RepaintControl()
        'qwertyuiop - will probably look at a way to pass back new errors eventually so we can just paint those as this would be able to SetStyle on those new errors ...
        'this has not been an issue before... as most controls repaint as a whole...
        If parentFastColoredTextBox IsNot Nothing Then
            UpdateErrors(parentFastColoredTextBox.VisibleRange)
        End If
    End Sub

    Private Sub tmrRepaint_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrRepaint.Tick
        UpdateErrors(parentFastColoredTextBox.VisibleRange)
        tmrRepaint.Enabled = False
    End Sub

    Private Sub FastColoredTextBox_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentFastColoredTextBox.Disposed
        tmrRepaint.Dispose()
        SpellErrorStyle.Dispose()
        IgnoreErrorStyle.Dispose()
        CaseErrorStyle.Dispose()
    End Sub

    Private Sub FastColoredTextBox_VisibleRangeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentFastColoredTextBox.VisibleRangeChanged
        tmrRepaint.Stop()
        tmrRepaint.Start()
    End Sub

    Private Sub FastColoredTextBox_TextChanged(ByVal sender As System.Object, ByVal e As FastColoredTextBoxNS.TextChangedEventArgs) Handles parentFastColoredTextBox.TextChanged
        'qwertyuiop - don't need this as the VisibleRangeChanged event gets called when text is changed :)
        'UpdateErrors(e.ChangedRange)
    End Sub

    Private Sub UpdateErrors(ByVal Range As FastColoredTextBoxNS.Range)
        Range.ClearStyle(IgnoreErrorStyle, CaseErrorStyle, SpellErrorStyle)
        If OKToDraw = False Then Exit Sub
        'clear errors...

        Dim text = Range.Text
        Dim RangesToSpellCheck As New List(Of Range)
        RangesToSpellCheck.Add(Range)
        If mc_SpellCheckMatch <> "" Then
            Dim matches = System.Text.RegularExpressions.Regex.Matches(text, mc_SpellCheckMatch, System.Text.RegularExpressions.RegexOptions.Multiline)
            text = Join((From xItem In matches.OfType(Of System.Text.RegularExpressions.Match)() Select xItem.Value).ToArray, " ")
            RangesToSpellCheck = Range.GetRanges(mc_SpellCheckMatch, System.Text.RegularExpressions.RegexOptions.Multiline).ToList
        End If

        text = Dictionary.Formatting.RemoveWordBreaks(text)
        Dim Words = Split(text, " ")

        Dim NewWords As New Dictionary(Of String, Dictionary.SpellCheckWordError)
        For Each word In Words
            If word <> "" Then
                Dim WordState As Dictionary.SpellCheckWordError = Dictionary.SpellCheckWordError.SpellError
                If dictCache.ContainsKey(word) Then
                    'load from cache
                    WordState = dictCache(word)
                Else
                    If NewWords.ContainsKey(word) = False Then
                        NewWords.Add(word, Dictionary.SpellCheckWordError.OK)
                    End If

                    WordState = Dictionary.SpellCheckWordError.OK
                End If
                Dim style As Style = Nothing
                Select Case WordState
                    Case Dictionary.SpellCheckWordError.Ignore
                        If DrawIgnored() Then
                            style = IgnoreErrorStyle
                        End If
                    Case Dictionary.SpellCheckWordError.CaseError
                        style = CaseErrorStyle
                    Case Dictionary.SpellCheckWordError.SpellError
                        style = SpellErrorStyle
                End Select
                If style IsNot Nothing Then
                    For Each RangeToSpellCheck In RangesToSpellCheck
                        RangeToSpellCheck.SetStyle(style, "\b" & System.Text.RegularExpressions.Regex.Escape(word) & "\b")
                    Next
                End If
            End If
        Next
        If NewWords.Count > 0 Then
            AddWordsToCache(NewWords)
        End If
    End Sub

#End Region

#Region "Test Harness"

#Region "HTML Color Highlighting"

    Private Class HTMLColorMap
        Inherits Dictionary(Of String, String)

        Public Shared ReadOnly Property ColorMapList() As HTMLColorMap
            Get
                Static mc_ColorMapList As New HTMLColorMap
                Return mc_ColorMapList
            End Get
        End Property

        Public Function GetDotNetColorName(ByVal HTMLColorName As String) As String
            GetDotNetColorName = (From xItem In Me Where LCase(xItem.Value) = LCase(HTMLColorName) Select xItem.Key).FirstOrDefault
            If GetDotNetColorName = "" Then
                GetDotNetColorName = HTMLColorName
            End If
        End Function

        Public Sub New()

            'color mapping
            MyBase.Add("ActiveCaptionText", "CaptionText")
            MyBase.Add("ControlText", "ButtonText")
            MyBase.Add("Desktop", "Background")
            MyBase.Add("ControlLight", "ThreeDLightShadow")
            MyBase.Add("ControlLightLight", "ThreeDHighlight")
            MyBase.Add("Info", "InfoBackground")

            'depriciated
            MyBase.Add("GradientActiveCaption", "")
            MyBase.Add("GradientInactiveCaption", "")
            MyBase.Add("MenuBar", "")
            MyBase.Add("MenuHighlight", "")
            MyBase.Add("Control", "")
            MyBase.Add("ControlDarkDark", "")
            MyBase.Add("ControlDark", "")

        End Sub
    End Class

    Private Sub TestHarness_TextChanged(ByVal sender As System.Object, ByVal e As FastColoredTextBoxNS.TextChangedEventArgs)
        If parentFastColoredTextBox.Language = Language.HTML Then

            Static cs As New ColorStyle(parentFastColoredTextBox)
            If parentFastColoredTextBox.Styles(0) Is Nothing OrElse Not (TypeOf parentFastColoredTextBox.Styles(0) Is ColorStyle) Then
                'make it first...
                parentFastColoredTextBox.ClearStylesBuffer()
                parentFastColoredTextBox.AddStyle(cs)
                'Dim Styles = (From xItem In parentFastColoredTextBox.Styles Where xItem IsNot cs).ToList
                'Styles.Insert(0, cs)
                'For i = LBound(parentFastColoredTextBox.Styles) To UBound(parentFastColoredTextBox.Styles)
                '    parentFastColoredTextBox.Styles(i) = Styles(i)
                'Next
            End If

            Static ColorMatch As String
            If ColorMatch = "" Then
                Dim lstColorMatch As New List(Of String)
                lstColorMatch.Add("#[a-f0-9]{6}\b") 'match "#ff00ff"
                lstColorMatch.Add("\brgb\(\s*?\d+?\s*?\,\s*?\d+?\s*?\,\s*?\d+?\s*?\)") 'match "rgb(255, 0, 255)"
                '\s.*?

                'match known colors (such as "red")
                For Each item In [Enum].GetValues(GetType(KnownColor))
                    If HTMLColorMap.ColorMapList.ContainsKey(item.ToString) Then
                        If HTMLColorMap.ColorMapList(item.ToString) = "" Then
                            'depriciated color
                        Else
                            lstColorMatch.Add("\b" & HTMLColorMap.ColorMapList(item.ToString) & "\b")
                        End If
                    Else
                        lstColorMatch.Add("\b" & item.ToString & "\b")
                    End If
                Next
                ColorMatch = "(" & Join(lstColorMatch.ToArray, "|") & ")"
            End If

            e.ChangedRange.ClearStyle(cs)
            e.ChangedRange.SetStyle(cs, ColorMatch, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        End If
    End Sub

    Private Class ColorStyle
        Inherits TextStyle

        Dim parent As FastColoredTextBox
        Public Sub New(ByVal parent As FastColoredTextBox)
            MyBase.New(Nothing, Nothing, Drawing.FontStyle.Regular)
            Me.parent = parent
        End Sub

        Public Overrides Sub Draw(ByVal gr As Graphics, ByVal position As Point, ByVal range As Range)
            Dim rect As Rectangle = New Rectangle(position, Style.GetSizeOfRange(range))
            rect.Inflate(1, 1)
            Using path As System.Drawing.Drawing2D.GraphicsPath = Style.GetRoundedRectangle(rect, 7)

                'try is here as otherwise if the word is "red" for eg and the "ed" part is off the visible area, the range.Text will be "r"... this does not translate to a html color
                Dim DrawBorder As Boolean

                Try
                    Dim BGColor As Color
                    Try
                        BGColor = ColorTranslator.FromHtml(HTMLColorMap.ColorMapList.GetDotNetColorName(range.Text))
                    Catch ex As Exception
                        'try the css rgb()
                        Dim TextColorSection = range.Text.Substring(range.Text.IndexOf("(") + 1)
                        TextColorSection = TextColorSection.Substring(0, TextColorSection.IndexOf(")"))
                        TextColorSection = System.Text.RegularExpressions.Regex.Replace(TextColorSection, "\s", "")
                        Dim arrColors = (From xItem In Split(TextColorSection, ",") Select CInt(Math.Min(Val(xItem), 255))).ToArray
                        BGColor = Color.FromArgb(255, arrColors(0), arrColors(1), arrColors(2))
                    End Try
                    Using sb As New SolidBrush(BGColor)
                        'MyBase.BackgroundBrush = sb
                        gr.FillPath(sb, path)
                    End Using

                    If CInt(BGColor.R) + CInt(BGColor.G) + CInt(BGColor.B) < 383 Then
                        'color is dark so make text light :)
                        MyBase.ForeBrush = New SolidBrush(Color.White)
                    Else
                        'color is light so make text dark :)
                        MyBase.ForeBrush = New SolidBrush(Color.Black)
                    End If

                    DrawBorder = (255 * 3) - (CInt(BGColor.R) + CInt(BGColor.G) + CInt(BGColor.B)) < 64

                Catch ex As Exception
                    MyBase.ForeBrush = New SolidBrush(Color.Black)
                End Try
                MyBase.Draw(gr, position, range)

                If DrawBorder Then
                    'color is nearly invisible ... so add a border...
                    Using p As New Pen(Color.LightGray)
                        gr.DrawPath(p, path)
                    End Using
                End If
            End Using

        End Sub
    End Class

#End Region

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As Control Implements i00SpellCheck.iTestHarness.SetupControl

        Dim FastColoredTextBox = TryCast(Control, FastColoredTextBox)
        If FastColoredTextBox IsNot Nothing Then
            Dim pnlHolder As New Panel
            Dim pnlOptions As New Panel
            pnlOptions.Dock = DockStyle.Bottom
            pnlHolder.Controls.Add(pnlOptions)

            Dim CodeRadioButtonNames() As String = {"VB.Net", "HTML"}
            Dim OptionLeftPos As Integer
            For Each item In CodeRadioButtonNames
                Dim CodeRadioButton As New RadioButton
                CodeRadioButton.Text = item
                CodeRadioButton.Left = OptionLeftPos
                CodeRadioButton.AutoSize = True
                OptionLeftPos += CodeRadioButton.Width
                pnlOptions.Controls.Add(CodeRadioButton)
                AddHandler CodeRadioButton.CheckedChanged, AddressOf CodeRadioButton_CheckedChanged
            Next
            pnlOptions.Height = pnlOptions.Controls(0).Height
            pnlHolder.Dock = DockStyle.Fill

            'AddHandler FastColoredTextBox.TextChanged, AddressOf TestHarness_TextChanged
            FastColoredTextBox.Dock = DockStyle.Fill

            TestWebBrowser = New WebBrowser
            TestWebBrowser.AllowWebBrowserDrop = False
            TestWebBrowser.IsWebBrowserContextMenuEnabled = False
            TestWebBrowser.Dock = DockStyle.Fill
            TestWebBrowser.DocumentText = "<body></body>"
            AddHandler TestWebBrowser.Navigating, AddressOf TestWebBrowser_Navigating

            TestSplitContainer = New SplitContainer
            TestSplitContainer.BackColor = i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.Highlight), Color.FromKnownColor(KnownColor.Info), 63)
            TestSplitContainer.Orientation = Orientation.Horizontal
            TestSplitContainer.Dock = DockStyle.Fill
            TestSplitContainer.Panel1.Controls.Add(FastColoredTextBox)
            TestSplitContainer.Panel2.Controls.Add(TestWebBrowser)

            pnlHolder.Controls.Add(TestSplitContainer)
            TestSplitContainer.SplitterDistance = CInt((TestSplitContainer.Height - TestSplitContainer.SplitterWidth) * 0.75)
            TestSplitContainer.BringToFront()

            'select an option
            pnlOptions.Controls.OfType(Of RadioButton).First.Checked = True

            AddHandler FastColoredTextBox.TextChanged, AddressOf TestHarness_TextChanged

            Return pnlHolder
        Else
            Return Nothing
        End If


    End Function

    Private Sub TestFastColoredTextBox_TextChanged(ByVal sender As System.Object, ByVal e As FastColoredTextBoxNS.TextChangedEventArgs) Handles parentFastColoredTextBox.TextChanged
        If TestWebBrowser IsNot Nothing AndAlso TestWebBrowser.Document IsNot Nothing AndAlso TestWebBrowser.Document.Body IsNot Nothing Then
            TestWebBrowser.Document.Body.InnerHtml = parentFastColoredTextBox.Text
        End If
    End Sub

    Dim TestWebBrowser As WebBrowser
    Dim TestSplitContainer As SplitContainer

    Private Sub TestWebBrowser_Navigating(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs)
        e.Cancel = True
        Try
            System.Diagnostics.Process.Start(e.Url.AbsoluteUri)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CodeRadioButton_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim oSender = DirectCast(sender, RadioButton)

        If oSender.Checked = True Then
            Select Case oSender.Text
                Case "VB.Net"
                    'parentFastColoredTextBox.LeftBracket = "("c
                    'parentFastColoredTextBox.RightBracket = ")"c

                    parentFastColoredTextBox.Language = Language.VB

                    SpellCheckMatch = "('.*$|"".*?"")"

                    TestSplitContainer.Panel2Collapsed = True

                    parentFastColoredTextBox.Text = "'Simple test to check spelling with 3rd party controls" & vbCrLf & _
                           "'This test is done on the FastColoredTextBox (open source control) that is hosted on CodeProject" & vbCrLf & _
                           "'The article can be found at: http://www.codeproject.com/Articles/161871/Fast-Colored-TextBox-for-syntax-highlighting" & vbCrLf & _
                           "" & vbCrLf & _
                           "'i00 Does not take credit for any work in the FastColoredTextBox.dll" & vbCrLf & _
                           "'i00 is however solely responsible for the spellcheck plugin to interface with FastColoredTextBox" & vbCrLf & _
                           "" & vbCrLf & _
                           "'As you can see only comments and string blocks are corrected" & vbCrLf & _
                           "'This is due to the SpellCheckMatch property being set" & vbCrLf & _
                           "" & vbCrLf & _
                           "'Click on a missspelled word to correct..." & vbCrLf & _
                           "" & vbCrLf & _
                           "Dim test = ""Test with some bad spellling!""" & vbCrLf & _
                           "" & vbCrLf & _
                           "#Region ""Char""" & vbCrLf & _
                           "   " & vbCrLf & _
                           "   ''' <summary>" & vbCrLf & _
                           "   ''' Char and style" & vbCrLf & _
                           "   ''' </summary>" & vbCrLf & _
                           "   Public Structure CharStyle" & vbCrLf & _
                           "       Public c As Char" & vbCrLf & _
                           "       Public style As StyleIndex" & vbCrLf & _
                           "   " & vbCrLf & _
                           "       Public Sub CharStyle(ByVal ch As Char)" & vbCrLf & _
                           "           c = ch" & vbCrLf & _
                           "           Style = StyleIndex.None" & vbCrLf & _
                           "       End Sub" & vbCrLf & _
                           "   " & vbCrLf & _
                           "   End Structure" & vbCrLf & _
                           "   " & vbCrLf & _
                           "#End Region"
                Case "HTML"
                    'parentFastColoredTextBox.LeftBracket = "<"c
                    'parentFastColoredTextBox.RightBracket = ">"c

                    TestSplitContainer.Panel2Collapsed = False


                    parentFastColoredTextBox.Language = Language.HTML

                    SpellCheckMatch = "(?<!<[^>]*)[^<^>]*"

                    'LinkColor here is used to demo css rgb()
                    Dim LinkColor As Color = i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.HotTrack), Color.FromKnownColor(KnownColor.WindowText))
                    Dim LinkColorHTML = "rgb(" & LinkColor.R & ", " & LinkColor.G & ", " & LinkColor.B & ")"

                    Dim BGColorHTML = ColorTranslator.ToHtml(i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.Highlight), Color.FromKnownColor(KnownColor.Window), 63))

                    parentFastColoredTextBox.Text = "<table style=""border:1px solid highlight;font-family:Arial;font-size:10 pt"" bgcolor=""" & BGColorHTML & """>" & vbCrLf & _
                           "    <tr>" & vbCrLf & _
                           "        <td valign=top>" & vbCrLf & _
                           "            <img src=""http://i00productions.org/i00logo/?size=48"">" & vbCrLf & _
                           "        </td>" & vbCrLf & _
                           "        <td>" & vbCrLf & _
                           "            This is a test done on the <i>FastColoredTextBox</i> (open source control) that is hosted on CodeProject<br>" & vbCrLf & _
                           "            The article can be found <a style=""color:" & LinkColorHTML & """ href=""http://www.codeproject.com/Articles/161871/Fast-Colored-TextBox-for-syntax-highlighting"">here</a><br>" & vbCrLf & _
                           "            <br>" & vbCrLf & _
                           "            i00 <b>does not</b> take credit for any work in the <i>FastColoredTextBox.dll</i> used in this project<br>" & vbCrLf & _
                           "            i00 is however solely responsible for the spellcheck plugin to interface with <i>FastColoredTextBox</i><br>" & vbCrLf & _
                           "            <br>" & vbCrLf & _
                           "            As you can see HTML tags are not corrected<br>" & vbCrLf & _
                           "            This is due to the <i>SpellCheckMatch</i> property being set<br>" & vbCrLf & _
                           "            <br>" & vbCrLf & _
                           "            Click on a missspelled word to correct...<br>" & vbCrLf & _
                           "            <br>" & vbCrLf & _
                           "            Test with some bad spellling!" & vbCrLf & _
                           "        </td>" & vbCrLf & _
                           "    </tr>" & vbCrLf & _
                           "</table>"
            End Select

            parentFastColoredTextBox.SelectionStart = 0
            parentFastColoredTextBox.SelectionLength = 0
            parentFastColoredTextBox.DoCaretVisible()

        End If

    End Sub

#End Region

End Class
