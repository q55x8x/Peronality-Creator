Public Class Hunspell_Dic
    Inherits i00SpellCheck.UserDictionaryBase
    Implements i00SpellCheck.Dictionary.Interfaces.iWordBase

#Region "i00SpellCheck.Dictionary.Interfaces.iWordBase"

    Public Function FindBaseWord(ByVal Word As String) As i00SpellCheck.Dictionary.Interfaces.FindBaseWordReturn Implements i00SpellCheck.Dictionary.Interfaces.iWordBase.FindBaseWord
        FindBaseWord = New i00SpellCheck.Dictionary.Interfaces.FindBaseWordReturn
        Dim list = NHunspell.Stem(Word)
        If list.Count > 0 Then
            FindBaseWord.WordBase = list.First
            FindBaseWord.Found = True
        Else
            FindBaseWord.WordBase = Word
            FindBaseWord.Found = False
            'FindBaseWord.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.None
        End If
    End Function

#End Region

    Dim NHunspell As NHunspell.Hunspell

    Public Overrides ReadOnly Property Count() As Integer
        Get
            If NHunspell Is Nothing Then
                Return 0
            Else
                Return -1
            End If
        End Get
    End Property

    Private Function GetAffFile(ByVal Filename As String) As String
        Return IO.Path.Combine(IO.Path.GetDirectoryName(Filename), IO.Path.GetFileNameWithoutExtension(Filename)) & ".aff"
    End Function

    Public Overrides Function ToString() As String
        Return LoadedFileName
    End Function

    Dim LoadedFileName As String

    Public Overrides Sub LoadFromFileInternal(ByVal Filename As String)
        NHunspell = New NHunspell.Hunspell(GetAffFile(Filename), Filename)

        'user dict
        LoadUserDictionary(Filename)
        'add to the suggestions...
        For Each item In (From xItem In UserWordList Where xItem.State = SpellCheckWordError.OK).ToArray
            NHunspell.Add(item.Word)
        Next

        LoadedFileName = Filename
    End Sub

    Public Overrides Sub SaveInternal(ByVal Filename As String, Optional ByVal ForceFullSave As Boolean = False)
        'copy file elsewhere?
        SaveUserDictionary(Filename, ForceFullSave)
    End Sub

    Public Overrides Function SpellCheckSuggestionsNonUser(ByVal Word As String) As System.Collections.Generic.List(Of i00SpellCheck.Dictionary.SpellCheckSuggestionInfo)
        Return (From xItem In NHunspell.Suggest(Word) Select New i00SpellCheck.Dictionary.SpellCheckSuggestionInfo(0, xItem)).ToList
    End Function

    Public Overrides Function SpellCheckWordNonUser(ByVal Word As String) As i00SpellCheck.Dictionary.SpellCheckWordError
        If NHunspell.Spell(Word) Then
            Return SpellCheckWordError.OK
        Else
            'see if the word is a case error 
            Dim CaseError = (From xItem In NHunspell.Suggest(Word) Where LCase(xItem) = LCase(Word) Select New i00SpellCheck.Dictionary.SpellCheckSuggestionInfo(0, xItem)).Count > 0
            If CaseError Then
                Return SpellCheckWordError.CaseError
            Else
                Return SpellCheckWordError.SpellError
            End If
        End If
    End Function

    Private Sub Hunspell_WordAdded(ByVal Item As String) Handles Me.WordAdded
        NHunspell.Add(Item)
    End Sub

    Private Sub Hunspell_WordRemoved(ByVal Item As String) Handles Me.WordRemoved
        'NHunspell - doesn't support removal :(
    End Sub

    Public Overrides ReadOnly Property DicFileFilter() As String
        Get
            Return "*_*.dic"
        End Get
    End Property

End Class
