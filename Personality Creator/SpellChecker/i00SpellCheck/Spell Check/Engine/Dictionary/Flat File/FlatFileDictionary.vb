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

Public Class FlatFileDictionary
    Inherits UserDictionaryBase

    Public Overrides ReadOnly Property Count() As Integer
        Get
            Return IndexedDictionary.GetCount
        End Get
    End Property

    Public Overloads Overrides Function ToString() As String
        Dim WordCount = IndexedDictionary.GetCount
        Return WordCount.ToString & " word" & If(WordCount = 1, "", "s")
    End Function

#Region "Default dic"

    Friend Shared ReadOnly Property DefaultDictFile() As String
        Get
            Return IO.Path.Combine(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location()), "dic.dic")
        End Get
    End Property

    Public Delegate Sub DictionaryLoaded_cb()

    Public Shared Sub LoadDefaultDictionary(Optional ByVal DictionaryLoaded_cb As DictionaryLoaded_cb = Nothing)
        Dim ThreadLoaded As Boolean = False
        If DefaultDictionary Is Nothing Then
            'load from file
            If FileIO.FileSystem.FileExists(DefaultDictFile) Then
                'load this
                Try
                    DefaultDictionary = New FlatFileDictionary()
                    If DictionaryLoaded_cb IsNot Nothing Then
                        ThreadLoaded = True
                        Dim t As New Threading.Thread(AddressOf mtLoadDictionary)
                        t.Name = "Load spell check dictionary"

                        t.IsBackground = True 'make it abort when the app is killed
                        t.Start(DictionaryLoaded_cb)

                    Else
                        'load it in this thread
                        DefaultDictionary.LoadFromFile(DefaultDictFile)
                    End If
                Catch ex As Exception
                    'failed ... load blank one 
                    DefaultDictionary = New FlatFileDictionary(DefaultDictFile, True)
                End Try
            Else
                'file not found ... load blank one
                DefaultDictionary = New FlatFileDictionary(DefaultDictFile, True)
            End If
        End If
        If DictionaryLoaded_cb IsNot Nothing Then
            If ThreadLoaded = False Then
                'we didn't need to load this in a thread... so invoke the cb here...
                DictionaryLoaded_cb.Invoke()
            End If
        End If
    End Sub

    Private Shared Sub mtLoadDictionary(ByVal o As Object)
        DefaultDictionary.LoadFromFile(DefaultDictFile)
        Dim DictionaryLoaded_cb = DirectCast(o, DictionaryLoaded_cb)
        If DictionaryLoaded_cb IsNot Nothing Then
            DictionaryLoaded_cb.Invoke()
        End If
    End Sub

#End Region

    Public Sub New()

    End Sub

    Public Overrides Sub LoadFromFileInternal(ByVal Filename As String)
        IndexedDictionary.Clear()

        'try loading from a flat file
        Dim fileData = My.Computer.FileSystem.ReadAllText(Filename)
        Dim lines = fileData.Split(New String() {vbCrLf, vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries).ToArray  'fileData.Split(CChar(vbLf))

        '>>>>                                                                rid duplicates
        Dim WordList = (From xItem In lines Where xItem <> "" Order By xItem Group xItem By xItem Into Group Select Group(0)).ToArray

        For Each item In WordList
            IndexedDictionary.Add(item)
        Next

        'now load the user dictionary
        LoadUserDictionary(Filename)

    End Sub

    Public IndexedDictionary As New IndexedDictionaryItems

    Public Class IndexedDictionaryItems
        Dim UnderlyingDict As New Hashtable ' Generic.Dictionary(Of Char, List(Of String))

        Public Sub Clear()
            UnderlyingDict.Clear()
        End Sub

        Public Function GetFullList() As List(Of String)
            GetFullList = New List(Of String)
            For Each theItem In (From xItem In UnderlyingDict.OfType(Of System.Collections.DictionaryEntry)() Order By xItem.Key Select TryCast(xItem.Value, IEnumerable(Of String)))
                If theItem IsNot Nothing Then
                    GetFullList.AddRange(theItem)
                End If
            Next
        End Function

        Public Function Item(ByVal Word As String) As List(Of String)
            Return DirectCast(UnderlyingDict(Word.ToLower.First), List(Of String))
            'If UnderlyingDict.ContainsKey(Word.ToLower.First) Then
            '    Return UnderlyingDict.Item(Word.ToLower.First)
            'Else
            '    Return Nothing
            'End If
        End Function

        Public Function GetCount() As Integer
            Return (From xItem In UnderlyingDict.Values.OfType(Of List(Of String))() Select xItem.Count).Sum
        End Function

        Public Sub Add(ByVal Word As String)
            Dim firstLetter = Word.ToLower.First
            If UnderlyingDict.ContainsKey(firstLetter) = False Then
                Dim List As New List(Of String)
                UnderlyingDict.Add(firstLetter, List)
            End If
            DirectCast(UnderlyingDict(firstLetter), List(Of String)).Add(Word)
        End Sub

        Public Function Contains(ByVal Word As String) As Boolean
            Dim firstLetter = Word.ToLower.First
            If UnderlyingDict.ContainsKey(firstLetter) Then
                Return DirectCast(UnderlyingDict(firstLetter), List(Of String)).Contains(Word)
            End If
        End Function

    End Class

    Public Sub New(ByVal Filename As String, ByVal CreateNewDict As Boolean)
        If CreateNewDict Then
            Me.SetFilename(Filename)
        Else
            LoadFromFile(Filename)
        End If
    End Sub

    Public Sub New(ByVal Filename As String)
        LoadFromFile(Filename)
    End Sub

    Public Overrides Sub SaveInternal(ByVal Filename As String, Optional ByVal ForceFullSave As Boolean = False)
        If ForceFullSave Then
            'we need to save the non-user dictionary too...
            Dim FileData = Join(IndexedDictionary.GetFullList.ToArray, vbCrLf)
            My.Computer.FileSystem.WriteAllText(Filename, FileData, False)
        End If

        'user dictionary 
        SaveUserDictionary(Filename, ForceFullSave)
    End Sub

    Public Overrides ReadOnly Property DicFileFilter() As String
        Get
            Return "*.dic"
        End Get
    End Property
End Class