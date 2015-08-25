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

<System.ComponentModel.TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> _
Public Class Definitions

#Region "Default Definitions"

    Public Shared ReadOnly Property DefaultDefinitionsFile() As String
        Get
            Return IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location()), "def.def")
        End Get
    End Property

    Public Shared DefaultDefinitions As Definitions
    Public Shared Sub LoadDefaultDefinitions()
        If DefaultDefinitions Is Nothing Then
            DefaultDefinitions = New Definitions With {.File = DefaultDefinitionsFile}
        End If
    End Sub

#End Region

    Public Overrides Function ToString() As String
        Return If(File <> "", File, "No File")
    End Function

    Dim mc_File As String
    <System.ComponentModel.DisplayName("Filename")> _
    <System.ComponentModel.Editor(GetType(defFile_UITypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property File() As String
        Get
            Return mc_File
        End Get
        Set(ByVal value As String)
            mc_File = value
        End Set
    End Property

#Region "PropertyGrid UI Def File Selector"
    Public Class defFile_UITypeEditor
        Inherits System.Drawing.Design.UITypeEditor

        Public Overloads Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Using ofd As New OpenFileDialog
                ofd.Filter = "Definition Files (*.def)|*.def|All Files (*.*)|*.*"
                If ofd.ShowDialog() = DialogResult.OK Then
                    value = ofd.FileName
                End If
                Return value
            End Using
        End Function

        Public Overloads Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
            Return System.Drawing.Design.UITypeEditorEditStyle.Modal
        End Function
    End Class
#End Region

    <System.ComponentModel.DisplayName("Count")> _
    <System.ComponentModel.Description("The count of words that have definitions specified in the selected def file")> _
    Public ReadOnly Property DefinitionCount() As Long
        Get
            If FileIO.FileSystem.FileExists(File) Then
                Using sr As New IO.StreamReader(File)
                    '    Return System.Text.RegularExpressions.Regex.Matches(sr.ReadToEnd(), "\r\n|\n\r|\r|\n").Count
                    Do Until sr.EndOfStream
                        Dim ThisLine = sr.ReadLine
                        DefinitionCount += 1
                    Loop
                End Using
            End If
        End Get
    End Property

    Public Function GetWordList() As List(Of String)
        GetWordList = New List(Of String)
        If FileIO.FileSystem.FileExists(File) Then
            Using sr As New IO.StreamReader(File)
                Do Until sr.EndOfStream
                    Dim ThisLine = sr.ReadLine
                    If ThisLine <> "" AndAlso ThisLine.Contains("|") Then
                        GetWordList.Add(ThisLine.Split(New Char() {"|"c}, 2)(0))
                    End If
                Loop
            End Using
        End If
    End Function

    Public Function FindWord(ByVal Word As String, Optional ByVal DictionaryForBaseWord As Dictionary = Nothing, Optional ByVal HTMLColorForQuotes As String = "") As WordDefinition
        FindWord = New WordDefinition
        FindWord.Word = Word

        If FileIO.FileSystem.FileExists(File) Then
            If HTMLColorForQuotes = "" Then HTMLColorForQuotes = System.Drawing.ColorTranslator.ToHtml(DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.ControlText), Color.FromKnownColor(KnownColor.Control)))
            AddWordDefs(Word, HTMLColorForQuotes, FindWord)
            If FindWord.Count = 0 Then
                'find base word... countries becomes country etc...
                If DictionaryForBaseWord Is Nothing OrElse DictionaryForBaseWord.Loading Then
                    'don't bother dict is not specified or still loading...
                Else
                    Dim iWordBase = TryCast(DictionaryForBaseWord, Dictionary.Interfaces.iWordBase)
                    If iWordBase IsNot Nothing Then
                        Dim BaseWord = iWordBase.FindBaseWord(Word.ToLower)
                        If BaseWord.Found Then
                            'Try again with the new word...
                            FindWord.Word = BaseWord.WordBase
                            FindWord.BaseType = BaseWord.BaseType
                            AddWordDefs(BaseWord.WordBase, HTMLColorForQuotes, FindWord)
                        End If
                    End If
                End If
            End If
        End If
    End Function

    Private Sub AddWordDefs(ByVal Word As String, ByVal HTMLColorForQuotes As String, ByVal WordDefinitionList As WordDefinition)
        Using sr As New IO.StreamReader(File)
            Do Until sr.EndOfStream
                Dim ThisLine = sr.ReadLine
                If ThisLine.ToLower.StartsWith(Word.ToLower & "|") Then
                    'found what we want
                    WordDefinitionList.Add(New WordDefinition.SingleDefinition(ThisLine, HTMLColorForQuotes))
                End If
            Loop
        End Using
    End Sub

    Public Class WordDefinition
        Inherits List(Of SingleDefinition)
        Public Word As String
        Public BaseType As Dictionary.Interfaces.FindBaseWordReturn.BaseTypes = Dictionary.Interfaces.FindBaseWordReturn.BaseTypes.None
        Public Class SingleDefinition
            Public Sub New(ByVal FileLine As String, ByVal HTMLColorForQuotes As String)
                Me.HTMLColorForQuotes = HTMLColorForQuotes
                Me.Line = FileLine
            End Sub
            Public ReadOnly Property GetDefs() As String()
                Get
                    Dim LineData = Line.Split(New Char() {"|"c}, 3)
                    If LineData.Length = 3 Then
                        Dim WordType = LineData(1)
                        Return LineData(2).Split(";"c)
                    End If
                    Return New String() {}
                End Get
            End Property
            Public ReadOnly Property HTMLLine() As String
                Get
                    Dim LineData = Line.Split(New Char() {"|"c}, 3)
                    If LineData.Length = 3 Then
                        Dim WordType = LineData(1)
                        Dim Defs = LineData(2).Split(";"c)

                        HTMLLine = "<i>" & WordType & "</i>" & vbCrLf
                        For Each iDef In Defs
                            Dim arrDef = Split(iDef, " - ", 2)
                            If UBound(arrDef) = 1 Then
                                arrDef(1) = "<font color=" & HTMLColorForQuotes & ">" & arrDef(1) & "</font>"
                            End If
                            If FileIO.FileSystem.DirectoryExists("def") Then
                                arrDef(0) = System.Text.RegularExpressions.Regex.Replace(arrDef(0), "(?<=\()[\w\s]{1,}?(?=\))", Function(m) If(FileIO.FileSystem.FileExists("def\" & m.Value & ".png"), "<img src=def\" & m.Value & ".png>" & m.Value, m.Value))
                            End If
                            HTMLLine &= "- " & Strings.Join(arrDef, " - ") & vbCrLf
                        Next
                    Else
                        Return Line
                    End If
                End Get
            End Property
            Public HTMLColorForQuotes As String
            Public Line As String
        End Class
        Public Overrides Function ToString() As String
            If Me.Count > 0 Then
                Dim BaseTypeText As String = ""
                If BaseType <> Dictionary.Interfaces.FindBaseWordReturn.BaseTypes.None Then
                    Dim FindBaseWordReturn As New Dictionary.Interfaces.FindBaseWordReturn
                    FindBaseWordReturn.BaseType = BaseType
                    BaseTypeText = Microsoft.VisualBasic.Join(FindBaseWordReturn.BaseTypeToArray, " / ")
                End If
                Return "<b>" & Word & "</b>" & If(BaseTypeText = "", "", " - " & BaseTypeText) & vbCrLf & _
                       Microsoft.VisualBasic.Join((From xItem In Me Select xItem.HTMLLine).ToArray, vbCrLf)
            Else
                Return ""
            End If
        End Function
    End Class

End Class
