'i00 .Net Control Extensions - TextBoxTranslator
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
Public Class TextBoxTranslator
    Inherits i00SpellCheck.ControlExtension
    Implements iTestHarness

#Region "Constructor"

    'called when the control is loaded...
    Public Overrides Sub Load()
        LoadLanguages()
        LoadIcon()
        parentTextBoxBase = DirectCast(MyBase.Control, TextBoxBase)
        extTextBoxContextMenu = parentTextBoxBase.ExtensionCast(Of extTextBoxContextMenu)()
    End Sub

#End Region

#Region "Load Languages and icon"

    Private Shared LoadingIcon As Boolean
    
    Private Shared Sub LoadIcon()
        If LoadingIcon = True OrElse FavIcon IsNot Nothing Then Return
        LoadingIcon = True
        Dim tLoadIcon As New System.Threading.Thread(AddressOf LoadIcon_mt)
        tLoadIcon.IsBackground = True
        tLoadIcon.Name = "Loading Google translator icon"
        tLoadIcon.Start()
    End Sub

    Private Shared FavIcon As Image
    Private Shared Sub LoadIcon_mt()
        Try
            Dim request = System.Net.HttpWebRequest.Create("http://translate.google.com/favicon.ico")
            Dim response = request.GetResponse()
            Dim Image As Image = Nothing
            'Try standard Image
            Using stream = response.GetResponseStream()
                Try
                    Image = Image.FromStream(stream)
                Catch ex As Exception
                End Try
            End Using
            'Try icon
            If Image Is Nothing Then
                Using stream = response.GetResponseStream()
                    Try
                        Using tempStream As New IO.MemoryStream
                            Const BUFFER_SIZE As Integer = 1024
                            Dim buffer(BUFFER_SIZE - 1) As Byte
                            Dim byteCount As Integer
                            Do
                                byteCount = stream.Read(buffer, 0, BUFFER_SIZE)
                                tempStream.Write(buffer, 0, byteCount)
                            Loop While byteCount = BUFFER_SIZE

                            tempStream.Seek(0, IO.SeekOrigin.Begin)
                            Image = New Icon(tempStream).ToBitmap
                        End Using
                    Catch ex As Exception
                    End Try
                End Using
            End If
            If Image IsNot Nothing Then
                FavIcon = Image
            End If
        Catch ex As Exception

        End Try
        LoadingIcon = False
    End Sub

    Private Shared LoadingLanguages As Boolean
    Private Shared LoadedLanguages As Boolean

    Private Shared Sub LoadLanguages()
        If LoadingLanguages = True OrElse LoadedLanguages = True Then Return
        LoadingLanguages = True
        Dim tLoadLanguages As New System.Threading.Thread(AddressOf LoadLanguages_mt)
        tLoadLanguages.IsBackground = True
        tLoadLanguages.Name = "Loading Google translator languages"
        tLoadLanguages.Start()
    End Sub

    Private Shared Sub LoadLanguages_mt()
        Try
            Dim Languages = GetSupportedLanguages()
            If Languages.Count > 0 Then
                LoadedLanguages = True
            End If
        Catch ex As Exception

        End Try
        LoadingLanguages = False
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
    Private WithEvents extTextBoxContextMenu As extTextBoxContextMenu

#End Region

#Region "For the test control"

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As System.Windows.Forms.Control Implements i00SpellCheck.iTestHarness.SetupControl
        If Control.GetType Is GetType(TextBox) Then
            Dim TextBox = DirectCast(Control, TextBox)

            TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12)
            TextBox.Multiline = True
            TextBox.ScrollBars = ScrollBars.Vertical
            TextBox.AppendText(If(TextBox.Text = "", "", vbCrLf & vbCrLf) & "The TextBoxTranslator project allows you to translate text to different languages using Google Translate.  To translate, right click and press Translate!")

            TextBox.SelectionStart = 0
            TextBox.SelectionLength = 0

            Return TextBox
        ElseIf Control.GetType Is GetType(RichTextBox) Then
            Dim RichTextBox = DirectCast(Control, RichTextBox)

            RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!)

            RichTextBox.AppendText(If(RichTextBox.Text = "", "", vbCrLf & vbCrLf) & "The TextBoxTranslator project allows you to translate text to different languages using Google Translate.  To translate, right click and press Translate!")

            Dim HighlightKeyWordFormat As New extTextBoxCommon.HighlightKeyWordFormat
            HighlightKeyWordFormat.Color = Color.FromKnownColor(KnownColor.HotTrack)

            extTextBoxCommon.HighlightKeyWord(RichTextBox, "TextBoxTranslator", HighlightKeyWordFormat)

            RichTextBox.Select(0, 0)
            RichTextBox.ClearUndo()

            Return RichTextBox
        Else
            Return Nothing
        End If

    End Function

#End Region

#Region "Properties"

    Dim mc_ShowTranslateInMenu As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Show Translate In Menu")> _
    <System.ComponentModel.Description("Shows the option to translate in the right click context menu")> _
    Public Property ShowTranslateInMenu() As Boolean
        Get
            Return mc_ShowTranslateInMenu
        End Get
        Set(ByVal value As Boolean)
            mc_ShowTranslateInMenu = value
        End Set
    End Property

#End Region

#Region "Add menu items"

    Private WithEvents tsiTranslate As i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem

    Dim tsiTranslateSubItemHolder As New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem("Google Translate is not avaliable at this time.", Nothing) With {.Enabled = False}

    Private Sub tsiTranslate_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiTranslate.DropDownOpening
        If tsiTranslate.DropDownItems.Contains(tsiTranslateSubItemHolder) Then
            Dim SupportedLanguages As Dictionary(Of String, String) = Nothing
            Try
                SupportedLanguages = GetSupportedLanguages()
            Catch ex As Exception

            End Try
            If SupportedLanguages IsNot Nothing AndAlso SupportedLanguages.Count > 0 Then
                For Each item In SupportedLanguages
                    Dim tsi = New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem(item.Value, Nothing)
                    tsi.Tag = item.Key
                    tsi.Image = GetFlagImage(item.Key)
                    AddHandler tsi.Click, AddressOf tsiTranslate_Click
                    tsiTranslate.DropDownItems.Add(tsi)
                Next
                tsiTranslate.DropDownItems.Remove(tsiTranslateSubItemHolder)
            End If
        End If
    End Sub

    Private Sub tsiTranslate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim StandardToolStripMenuItem = DirectCast(sender, i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem)
        Dim LanguageTo = StandardToolStripMenuItem.Tag.ToString()
        If parentTextBoxBase.SelectionLength = 0 Then
            'doing it this way rather setting .Text allows undo
            Try
                Dim TranslatedText = Translate(parentTextBoxBase.Text, LanguageTo)
                parentTextBoxBase.SelectAll()
                parentTextBoxBase.SelectedText = TranslatedText
            Catch ex As Exception
                MsgBox("Translation failed, please make sure you are connected to the internet." & vbCrLf & "Error: " & ex.Message, MsgBoxStyle.Critical)
            End Try
        Else
            parentTextBoxBase.SelectedText = Translate(parentTextBoxBase.SelectedText, LanguageTo)
        End If
    End Sub

    Private Shared Function GetFlagAlias(ByVal TranslateCode As String) As String
        TranslateCode = LCase(TranslateCode)
        Dim arrTranslateCode = Split(TranslateCode, "-", 2)
        If UBound(arrTranslateCode) >= 1 Then
            TranslateCode = arrTranslateCode(1)
        End If
        Static FlagAlias As Dictionary(Of String, String)
        FlagAlias = Nothing
        If FlagAlias Is Nothing Then
            FlagAlias = New Dictionary(Of String, String)
            FlagAlias.Add("af", "za") 'afrikaans
            FlagAlias.Add("sq", "al") 'albanian
            FlagAlias.Add("ar", "sa") 'arabic
            FlagAlias.Add("hy", "am") 'Armenian
            FlagAlias.Add("be", "by") 'Belarusian
            FlagAlias.Add("bn", "bd") 'Bengali
            FlagAlias.Add("ca", "catalonia") 'Catalan
            FlagAlias.Add("cs", "cz") 'Czech
            FlagAlias.Add("da", "dk") 'Danish
            FlagAlias.Add("en", "gb") 'English
            FlagAlias.Add("et", "ee") 'Estonian
            FlagAlias.Add("tl", "ph") 'Filipino
            FlagAlias.Add("ka", "ge") 'Georgian
            FlagAlias.Add("el", "gr") 'Greek
            FlagAlias.Add("gu", "in") 'Gujarati
            FlagAlias.Add("iw", "il") 'Hebrew
            FlagAlias.Add("hi", "in") 'Hindi
            FlagAlias.Add("ga", "ie") 'Irish
            FlagAlias.Add("ja", "jp") 'Japanese
            FlagAlias.Add("ko", "kr") 'Korean
            FlagAlias.Add("lo", "la") 'Lao
            FlagAlias.Add("ms", "my") 'Malay
            FlagAlias.Add("fa", "ir") 'Persian
            FlagAlias.Add("sr", "rs") 'Serbian
            FlagAlias.Add("sl", "si") 'Slovenian
            FlagAlias.Add("sw", "ke") 'Swahili
            FlagAlias.Add("sv", "se") 'Swedish
            FlagAlias.Add("uk", "ua") 'Ukrainian
            FlagAlias.Add("ur", "pk") 'Urdu
            FlagAlias.Add("vi", "vn") 'Vietnamese
            FlagAlias.Add("cy", "wales") 'Welsh
            '----------------------------------------
            FlagAlias.Add("eu", "zz") 'Basque
            FlagAlias.Add("eo", "zz") 'Esperanto
            FlagAlias.Add("gl", "zz") 'Galician
            FlagAlias.Add("kn", "zz") 'Kannada
            FlagAlias.Add("la", "zz") 'Latin
            FlagAlias.Add("ta", "zz") 'Tamil
            FlagAlias.Add("te", "zz") 'Telugu
            FlagAlias.Add("yi", "zz") 'Yiddish
        End If
        If FlagAlias.ContainsKey(TranslateCode) Then
            TranslateCode = FlagAlias(TranslateCode)
        End If
        Return TranslateCode
    End Function

    Private Shared Function GetFlagImage(ByVal FlagName As String) As Image
        FlagName = GetFlagAlias(FlagName)
        Static FlagImageCache As New Dictionary(Of String, Image)
        If FlagImageCache.ContainsKey(FlagName) Then
            Return FlagImageCache(FlagName)
        Else
            Dim File = "Flags\" & FlagName & ".png"
            If FileIO.FileSystem.FileExists(File) Then
                Try
                    Dim FlagImage = New Bitmap(16, 16)
                    Using g = Graphics.FromImage(FlagImage)
                        g.InterpolationMode = Drawing2D.InterpolationMode.High
                        Using b As New Bitmap(File)
                            g.DrawImage(b, Rectangle.Round(i00SpellCheck.DrawingFunctions.GetBestFitRect(New RectangleF(0, 0, 16, 16), New RectangleF(0, 0, b.Width, b.Height), DrawingFunctions.BestFitStyle.Stretch)))
                        End Using
                    End Using
                    FlagImageCache.Add(FlagName, FlagImage)
                    Return FlagImage
                Catch ex As Exception

                End Try
            End If
        End If

        Return Nothing
    End Function

    Private Sub extTextBoxContextMenu_MenuOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles extTextBoxContextMenu.MenuOpening
        Dim MenuItems As New List(Of ToolStripItem)

        'incase the loading of the languages failed ... eg no internet
        LoadLanguages()
        LoadIcon()

        If mc_ShowTranslateInMenu AndAlso LoadedLanguages Then
            tsiTranslate = New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem("Translate", Nothing)
            tsiTranslate.DropDownItems.Add(tsiTranslateSubItemHolder)
            tsiTranslate.Image = FavIcon
            MenuItems.Add(tsiTranslate)
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

#End Region

#Region "Translate"

    Private Shared Function getValueFromString(ByVal theString As String, ByVal theField As String, Optional ByVal DelimiterMajor As String = ";", Optional ByVal DelimiterMinor As String = ":", Optional ByVal returnNullValue As Boolean = False) As String
        Dim mc As System.Text.RegularExpressions.MatchCollection
        theString = DelimiterMajor & theString & DelimiterMajor

        DelimiterMinor = "\s{0,}" & System.Text.RegularExpressions.Regex.Escape(DelimiterMinor) & "\s{0,}"
        DelimiterMajor = "\s{0,}" & System.Text.RegularExpressions.Regex.Escape(DelimiterMajor) & "\s{0,}"

        mc = System.Text.RegularExpressions.Regex.Matches(theString, "(?<=(^|" & DelimiterMajor & ")" & theField & "($|" & DelimiterMinor & ")).*?(?=" & DelimiterMajor & ")", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        If mc.Count = 0 Then
            getValueFromString = ""
        Else
            getValueFromString = mc(0).Value
            If returnNullValue = True AndAlso getValueFromString = "" Then
                getValueFromString = "Null"
            End If
        End If
    End Function

    Public Shared Function Translate(ByVal TranslateText As String, ByVal destLangCode As String) As String
        Dim url As String = "http://translate.google.com"

        Dim request = System.Net.HttpWebRequest.Create(url)
        request.Timeout = 2500
        request.Method = "POST"
        Dim Post As String = "langpair=auto|" & destLangCode & "&text=" & System.Web.HttpUtility.UrlEncode(TranslateText)
        request.ContentLength = Post.Length
        Using sw As New IO.StreamWriter(request.GetRequestStream)
            sw.Write(Post)
        End Using

        Dim response = request.GetResponse()
        Dim strEncoding = Trim(getValueFromString(response.ContentType, "charset", ";", "="))
        Dim Encoding = System.Text.Encoding.GetEncoding(strEncoding)
        Using stream = response.GetResponseStream()
            Using sr As New IO.StreamReader(stream, Encoding)
                Dim PageContent = sr.ReadToEnd()

                Translate = System.Text.RegularExpressions.Regex.Match(PageContent, "(?<=<span\s[^>]*(?<=\s)id\s{0,}\=[\s""']{0,}result_box[^>]*>).+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value
                'go until we find the closing Span tag
                Dim SpanCounter = 0
                For Each item In System.Text.RegularExpressions.Regex.Matches(Translate, "(?<=<)/{0,}span(?=[\s>])", System.Text.RegularExpressions.RegexOptions.IgnoreCase).OfType(Of System.Text.RegularExpressions.Match)()
                    If item.Value.First = "/"c Then
                        SpanCounter -= 1
                    Else
                        SpanCounter += 1
                    End If
                    If SpanCounter < 0 Then
                        Translate = Strings.Left(Translate, item.Index - 1)
                        Exit For
                    End If
                Next
                Translate = System.Text.RegularExpressions.Regex.Replace(Translate, "<br[^>]*>", vbCrLf)
                Translate = System.Text.RegularExpressions.Regex.Replace(Translate, "(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>", "")
                Translate = System.Web.HttpUtility.HtmlDecode(Translate)

            End Using
        End Using
    End Function

    Public Shared Function GetSupportedLanguages() As Dictionary(Of String, String)
        Static mc_GetSupportedLanguages As Dictionary(Of String, String)
        If mc_GetSupportedLanguages Is Nothing Then
            mc_GetSupportedLanguages = New Dictionary(Of String, String)
            Using client As New System.Net.WebClient()
                Dim url As String = "http://translate.google.com"
                Dim PageContent As String = client.DownloadString(url)
                '<select id=gt-sl
                Dim SelectText = System.Text.RegularExpressions.Regex.Match(PageContent, "(?<=<select\s[^>]*(?<=\s)id\s{0,}\=[\s""']{0,}gt-sl[^>]*>).+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value
                SelectText = System.Text.RegularExpressions.Regex.Match(SelectText, ".+?(?=</select[\s>])", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value
                For Each item In System.Text.RegularExpressions.Regex.Matches(SelectText, "(?<=<option[\s>]).+?(?=</option[\s>])", System.Text.RegularExpressions.RegexOptions.IgnoreCase).OfType(Of System.Text.RegularExpressions.Match)()
                    If InStr(item.Value, "disabled", CompareMethod.Text) = 0 Then
                        Dim Key = System.Text.RegularExpressions.Regex.Match(item.Value, "(?<=value\s{0,}=\s{0,})[""']?((?:.(?![""']?\s+(?:\S+)=|[>""']))+.)[""']?", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value
                        Key = Trim(Key)
                        If Key <> "" Then
                            If Key.StartsWith(""""c) OrElse Key.StartsWith("'"c) Then
                                'just trim
                                Key = Key.Trim(""""c, "'"c)
                            Else
                                'until the first space
                                Key = Split(Key, " ", 2)(0)
                            End If
                        End If
                        If Key <> "" Then
                            'do the value
                            Dim Value = System.Text.RegularExpressions.Regex.Match(item.Value, "(?<=>).+", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value
                            If Value <> "" Then
                                mc_GetSupportedLanguages.Add(Key, Value)
                            End If
                        End If
                    End If
                Next
                mc_GetSupportedLanguages.Remove("auto")
            End Using
        End If
        Return mc_GetSupportedLanguages
    End Function

#End Region

End Class
