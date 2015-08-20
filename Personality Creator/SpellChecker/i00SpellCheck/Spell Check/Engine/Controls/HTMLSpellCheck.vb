'i00 .Net HTML Spell Check
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

<System.ComponentModel.DesignerCategory("")> _
Public Class HTMLSpellCheck
    Inherits WebBrowser

#Region "Properties"

#Region "HTML"

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
                   ".Error" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    color:" & ColorTranslator.ToHtml(mc_MistakeColor) & ";" & vbCrLf & _
                   "}" & vbCrLf & _
                   ".Case" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    color:" & ColorTranslator.ToHtml(mc_CaseMistakeColor) & ";" & vbCrLf & _
                   "}" & vbCrLf & _
                   ".Pending" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    color:" & ColorTranslator.ToHtml(Color.FromArgb(DrawingFunctions.BlendColor(mc_ForeColor, mc_BackColor).ToArgb)) & ";" & vbCrLf & _
                   "}" & vbCrLf & _
                   ".Changed" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    font-style:italic;" & vbCrLf & _
                   "    /* text-decoration:underline; */" & vbCrLf & _
                   "}" & vbCrLf & _
                   ".OK" & vbCrLf & _
                   "{" & vbCrLf & _
                   "}" & vbCrLf & _
                   ".Selected" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    border: solid 1px " & ColorTranslator.ToHtml(Color.FromKnownColor(KnownColor.Highlight)) & ";" & vbCrLf & _
                   "    background-color:" & ColorTranslator.ToHtml(Color.FromArgb(DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.Highlight), mc_BackColor, 95).ToArgb)) & ";" & vbCrLf & _
                   "    margin: 0px 0px 0px 0px;" & vbCrLf & _
                   "}" & vbCrLf & _
                   "a:hover" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    border: solid 1px " & ColorTranslator.ToHtml(Color.FromKnownColor(KnownColor.Highlight)) & ";" & vbCrLf & _
                   "    background-color:" & ColorTranslator.ToHtml(DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.Highlight), mc_BackColor, 63)) & ";" & vbCrLf & _
                   "    margin: 0px 0px 0px 0px;" & vbCrLf & _
                   "}" & vbCrLf & _
                   "a:active, a:selected" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    ie-dummy: expression(this.hideFocus=true);" & vbCrLf & _
                   "}" & vbCrLf & _
                   "a" & vbCrLf & _
                   "{" & vbCrLf & _
                   "    margin: 1px 1px 1px 1px;" & vbCrLf & _
                   "    text-decoration:none;" & vbCrLf & _
                   "    color:" & ColorTranslator.ToHtml(mc_ForeColor) & ";" & vbCrLf & _
                   "}" & vbCrLf & _
                   "</style>"
        End Get
    End Property

#End Region

    Dim mc_Words As New List(Of SpellCheckDialogWords)

    Public ReadOnly Property Words() As List(Of SpellCheckDialogWords)
        Get
            Return mc_Words
        End Get
    End Property

    Dim mc_BackColor As Color = Color.FromKnownColor(KnownColor.Window)
    <System.ComponentModel.DefaultValue("Window")> _
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)> _
    <System.ComponentModel.Bindable(True)> _
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)> _
    <System.ComponentModel.BrowsableAttribute(True)> _
    Overrides Property BackColor() As Color
        Get
            Return mc_BackColor
        End Get
        Set(ByVal value As Color)
            mc_BackColor = value
        End Set
    End Property

    Dim mc_ForeColor As Color = Color.FromKnownColor(KnownColor.WindowText)
    <System.ComponentModel.DefaultValue("WindowText")> _
    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Always)> _
    <System.ComponentModel.Bindable(True)> _
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)> _
    <System.ComponentModel.BrowsableAttribute(True)> _
    Overrides Property ForeColor() As Color
        Get
            Return mc_ForeColor
        End Get
        Set(ByVal value As Color)
            mc_ForeColor = value
        End Set
    End Property

    Dim mc_MistakeColor As Color = Color.Red
    <System.ComponentModel.DefaultValue("Red")> _
    Public Property MistakeColor() As Color
        Get
            Return mc_MistakeColor
        End Get
        Set(ByVal value As Color)
            mc_MistakeColor = value
        End Set
    End Property

    Dim mc_CaseMistakeColor As Color = Color.Green
    <System.ComponentModel.DefaultValue("Green")> _
    Public Property CaseMistakeColor() As Color
        Get
            Return mc_CaseMistakeColor
        End Get
        Set(ByVal value As Color)
            mc_CaseMistakeColor = value
        End Set
    End Property

    Dim mc_Dictionary As Dictionary = Nothing
    Public Property Dictionary() As Dictionary
        Get
            Return mc_Dictionary
        End Get
        Set(ByVal value As Dictionary)
            mc_Dictionary = value
        End Set
    End Property

#End Region

#Region "Word Object"

#Region "Word Object HTML Updates"

    Private Delegate Sub SetClass_cb(ByVal id As String, ByVal className As String)
    Private Sub SetClass(ByVal id As String, ByVal className As String)
        If MyBase.InvokeRequired Then
            Dim SetClass_cb As New SetClass_cb(AddressOf SetClass)
            MyBase.Invoke(SetClass_cb, id, className)
        Else
            Try
                Dim element = MyBase.Document.GetElementById(id)
                If element IsNot Nothing Then
                    element.SetAttribute("className", className)
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub UpdateClassEvent(ByVal sender As Object, ByVal e As EventArgs)
        Dim oSender = CType(sender, SpellCheckDialogWords)
        If oSender IsNot Nothing Then
            Dim className As String = oSender.SpellCheckState.ToString
            If oSender.Selected Then className &= " Selected"
            If oSender.Changed Then className &= " Changed"
            SetClass(mc_Words.IndexOf(oSender).ToString, className)
        End If
    End Sub

    Private Delegate Sub SetWordText_cb(ByVal id As String, ByVal WordText As String)
    Private Sub SetWordText(ByVal id As String, ByVal WordText As String)
        If MyBase.InvokeRequired Then
            Dim SetWordText_cb As New SetWordText_cb(AddressOf SetWordText)
            MyBase.Invoke(SetWordText_cb, id, WordText)
        Else
            Dim element = MyBase.Document.GetElementById(id)
            If element IsNot Nothing Then
                element.SetAttribute("className", WordText)
                element.InnerHtml = WordText
            End If
        End If
    End Sub

    Private Sub UpdateWordEvent(ByVal sender As Object, ByVal e As EventArgs)
        Dim oSender = CType(sender, SpellCheckDialogWords)
        If oSender IsNot Nothing Then
            SetWordText(mc_Words.IndexOf(oSender).ToString, oSender.NewWord)
        End If
    End Sub

    Private Sub UpdateDocument()
        'Dim DocumentText As String = ""
        Dim DocumentText As New System.Text.StringBuilder
        Dim UpTo = 0
        For Each item In mc_Words
            RemoveHandler item.UpdateClassEvent, AddressOf UpdateClassEvent
            RemoveHandler item.SelectionChangedEvent, AddressOf WordSelectionChangedEvent
            RemoveHandler item.UpdateWordEvent, AddressOf UpdateWordEvent
        Next
        mc_Words.Clear()
        For Each item In Dictionary.Formatting.RemoveWordBreaks(mc_Text).Split(" "c)
            Dim ThisWordItem = New SpellCheckDialogWords(item) With {.StartIndex = UpTo}
            AddHandler ThisWordItem.UpdateClassEvent, AddressOf UpdateClassEvent
            AddHandler ThisWordItem.SelectionChangedEvent, AddressOf WordSelectionChangedEvent
            AddHandler ThisWordItem.UpdateWordEvent, AddressOf UpdateWordEvent
            mc_Words.Add(ThisWordItem)
            If ThisWordItem.OrigWord <> "" Then
                DocumentText.Append("<a href='" & mc_Words.Count - 1 & "' class='Pending' id='" & mc_Words.Count - 1 & "'>" & ThisWordItem.OrigWord & "</a>")
            End If
            UpTo += item.Length + 1
            If UpTo <= mc_Text.Length Then
                DocumentText.Append(mc_Text.Substring(UpTo - 1, 1))
            End If
        Next
        mc_AllowNavigation = True
        MyBase.DocumentText = HTMLStyle() & "<BODY onselectstart='return false;'>" & System.Text.RegularExpressions.Regex.Replace(DocumentText.ToString, "\r\n|\n\r|\r|\n", "<BR>")
        mc_AllowNavigation = False
    End Sub

#End Region

    Public Class SpellCheckDialogWords

        Public Sub New(ByVal OrigWord As String)
            Me.OrigWord = OrigWord
            mc_NewWord = OrigWord
        End Sub

        Public OrigWord As String
        Dim mc_NewWord As String
        Public Property NewWord(Optional ByVal Autoupdate As Boolean = True) As String
            Get
                Return mc_NewWord
            End Get
            Set(ByVal value As String)
                mc_NewWord = value
                If Changed = False Then
                    Changed = True
                End If
                If Autoupdate Then
                    UpdateWord()
                End If
            End Set
        End Property
        Public StartIndex As Integer
        Public Enum SpellCheckStates
            Pending
            [Error]
            OK
            [Case]
        End Enum

        Dim mc_Changed As Boolean
        Public Property Changed() As Boolean
            Get
                Return mc_Changed
            End Get
            Set(ByVal value As Boolean)
                If mc_Changed <> value Then
                    mc_Changed = value
                    'update the class
                    UpdateClass()
                End If
            End Set
        End Property
        Private mc_SpellCheckState As SpellCheckStates = SpellCheckStates.Pending
        Public Property SpellCheckState(Optional ByVal Autoupdate As Boolean = True) As SpellCheckStates
            Get
                Return mc_SpellCheckState
            End Get
            Set(ByVal value As SpellCheckStates)
                mc_SpellCheckState = value
                If Autoupdate Then
                    UpdateClass()
                End If
            End Set
        End Property
        Private mc_Selected As Boolean = False
        Public Property Selected(Optional ByVal Autoupdate As Boolean = True) As Boolean
            Get
                Return mc_Selected
            End Get
            Set(ByVal value As Boolean)
                mc_Selected = value
                If Autoupdate Then
                    SelectionChanged()
                    UpdateClass()
                End If
            End Set
        End Property
        Public Event UpdateWordEvent(ByVal sender As Object, ByVal e As EventArgs)
        Public Sub UpdateWord()
            RaiseEvent UpdateWordEvent(Me, EventArgs.Empty)
        End Sub
        Public Event UpdateClassEvent(ByVal sender As Object, ByVal e As EventArgs)
        Public Sub UpdateClass()
            RaiseEvent UpdateClassEvent(Me, EventArgs.Empty)
        End Sub
        Public Event SelectionChangedEvent(ByVal sender As Object, ByVal e As EventArgs)
        Public Sub SelectionChanged()
            RaiseEvent SelectionChangedEvent(Me, EventArgs.Empty)
        End Sub
    End Class

#End Region

#Region "Events"

    Public Event SelectionChanged(ByVal sender As Object, ByVal e As HTMLWordEventArgs)

    Public Class HTMLWordEventArgs
        Inherits EventArgs
        Public Word As SpellCheckDialogWords
    End Class

    Public Event WordSpellChecked(ByVal sender As Object, ByVal e As HTMLWordEventArgs)

    Public Event SpellCheckComplete(ByVal sender As Object, ByVal e As EventArgs)

#End Region

#Region "Constructors"

    Public Sub New()
        MyBase.AllowWebBrowserDrop = False
        MyBase.IsWebBrowserContextMenuEnabled = False
    End Sub

#End Region

#Region "Other Public Subs"

    'Sets the HTML to the document string
    Dim mc_Text As String

    Private Settings As New SpellCheckSettings

    Public Sub SetText(ByVal Text As String, Optional ByVal Settings As SpellCheckSettings = Nothing)
        If Settings IsNot Nothing Then Me.Settings = Settings
        mc_Text = Text
        UpdateDocument()
    End Sub

    'Ensures that a specific word is visible in the HTML document
    Private Delegate Sub EnsureVisible_cb(ByVal Word As SpellCheckDialogWords)
    Public Sub EnsureVisible(ByVal Word As SpellCheckDialogWords)
        If Me.InvokeRequired Then
            Dim EnsureVisible_cb As New EnsureVisible_cb(AddressOf EnsureVisible)
            Me.Invoke(EnsureVisible_cb, Word)
        Else
            Dim Element = MyBase.Document.GetElementById(Words.IndexOf(Word).ToString)
            If Element.OffsetRectangle.Bottom > MyBase.Document.Body.OffsetRectangle.Height + MyBase.Document.Body.ScrollTop Then
                MyBase.Document.Body.ScrollTop = Element.OffsetRectangle.Bottom - MyBase.Document.Body.OffsetRectangle.Height
            ElseIf Element.OffsetRectangle.Top < MyBase.Document.Body.ScrollTop Then
                MyBase.Document.Body.ScrollTop = Element.OffsetRectangle.Top
            End If
        End If
    End Sub

#End Region

#Region "Spell Check"

    Friend mt_SpellCheck As Threading.Thread

    Private Sub DoSpellCheck()
        'Dim st = Now
        For Each item In (From xItem In mc_Words Where xItem.SpellCheckState = SpellCheckDialogWords.SpellCheckStates.Pending).ToArray
            Dim className As String = ""
            If Settings.IgnoreWordOverride(item.NewWord) Then
                'ok as all in caps etc
                item.SpellCheckState(False) = SpellCheckDialogWords.SpellCheckStates.OK
            Else
                Select Case mc_Dictionary.SpellCheckWord(item.NewWord)
                    Case Dictionary.SpellCheckWordError.OK
                        item.SpellCheckState(False) = SpellCheckDialogWords.SpellCheckStates.OK
                    Case Dictionary.SpellCheckWordError.SpellError
                        item.SpellCheckState(False) = SpellCheckDialogWords.SpellCheckStates.Error
                    Case Dictionary.SpellCheckWordError.CaseError
                        item.SpellCheckState(False) = SpellCheckDialogWords.SpellCheckStates.Case
                    Case Dictionary.SpellCheckWordError.Ignore
                        item.SpellCheckState(False) = SpellCheckDialogWords.SpellCheckStates.OK
                End Select
            End If
            RaiseEvent WordSpellChecked(Me, New HTMLWordEventArgs() With {.Word = item})
            item.UpdateClass()
        Next
        'MsgBox(Now.Subtract(st).TotalMilliseconds)
        RaiseEvent SpellCheckComplete(Me, EventArgs.Empty)
    End Sub

    Private Sub HTMLSpellCheck_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If mt_SpellCheck IsNot Nothing AndAlso mt_SpellCheck.IsAlive Then
            mt_SpellCheck.Abort()
        End If
    End Sub

    Private Sub HTMLSpellCheck_DocumentCompleted(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles Me.DocumentCompleted
        'check the words in a thread here...
        StartSpellCheck()
    End Sub
    Public Sub StartSpellCheck()
        If mc_Dictionary IsNot Nothing Then
            'qwertyuiop - was going to do this in a threadpool... but no way to abort threads ... easily???
            'System.Threading.ThreadPool.SetMaxThreads(10, 10)
            'For Each item In Words
            '    Dim w As New System.Threading.WaitCallback(AddressOf DoSpellCheck)
            '    Threading.ThreadPool.QueueUserWorkItem(w, item)
            'Next
            If mt_SpellCheck IsNot Nothing AndAlso mt_SpellCheck.IsAlive Then
                mt_SpellCheck.Abort()
            End If
            mt_SpellCheck = New System.Threading.Thread(AddressOf DoSpellCheck)
            mt_SpellCheck.Name = If(Me.Name = "", "HTMLSpellCheck", Me.Name) & " - Spell Checking"
            mt_SpellCheck.IsBackground = True
            mt_SpellCheck.Start()
        End If
    End Sub

#End Region

#Region "Word Selection"

    Private Sub WordSelectionChangedEvent(ByVal sender As Object, ByVal e As EventArgs)
        Dim oSender = CType(sender, SpellCheckDialogWords)
        If oSender IsNot Nothing Then
            If oSender.Selected = True Then
                'unselect the others
                For Each item In (From xItem In Words Where xItem IsNot oSender AndAlso xItem.Selected = True).ToArray
                    item.Selected = False
                Next
                EnsureVisible(oSender)
                RaiseEvent SelectionChanged(Me, New HTMLWordEventArgs() With {.Word = oSender})
            End If
        End If
    End Sub

    Dim mc_AllowNavigation As Boolean
    Private Sub HTMLSpellCheck_Navigating(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles Me.Navigating
        If mc_AllowNavigation = False Then
            e.Cancel = True
            Dim elementNo As Integer
            If Integer.TryParse(e.Url.AbsolutePath, elementNo) Then
                Words(elementNo).Selected = True
            End If
        End If
    End Sub

#End Region

End Class
