Public Class Hanks_Dic
    Inherits i00SpellCheck.UserDictionaryBase

    Dim HankDic As New HankDic

    Public Overrides ReadOnly Property Count() As Integer
        Get
            Return HankDic.Dic.Count
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return LoadedFileName
    End Function

    Dim LoadedFileName As String

    Public Overrides Sub LoadFromFileInternal(ByVal Filename As String)
        HankDic.LoadDic(Filename)
        LoadUserDictionary(Filename)

        LoadedFileName = Filename
    End Sub

    Public Overrides Function SpellCheckSuggestionsNonUser(ByVal Word As String) As System.Collections.Generic.List(Of i00SpellCheck.Dictionary.SpellCheckSuggestionInfo)
        'only spell check the bit before the '
        Dim AposIndex = Word.IndexOf("'")
        Dim Suffix As String = ""
        If AposIndex > 0 Then
            Suffix = Word.Substring(AposIndex)
            Word = Word.Substring(0, AposIndex)
        End If

        Dim DicKeys = (From xItem In HankDic.Dic Where LCase(xItem.Key) = LCase(Word) Select xItem)
        If DicKeys.Count > 0 Then
            Return (From xItem In DicKeys Order By xItem.Value Select New i00SpellCheck.Dictionary.SpellCheckSuggestionInfo(0, xItem.Key & Suffix)).ToList
        Else
            Dim Suggestions = HankDic.GetSuggestWords(Word)
            Return (From xItem In Suggestions Select New i00SpellCheck.Dictionary.SpellCheckSuggestionInfo(0, xItem & Suffix)).ToList
        End If
    End Function

    Public Overrides Function SpellCheckWordNonUser(ByVal Word As String) As i00SpellCheck.Dictionary.SpellCheckWordError
        'only spell check the bit before the '
        Dim AposIndex = Word.IndexOf("'")
        If AposIndex > 0 Then
            Word = Word.Substring(0, AposIndex)
        End If

        If HankDic.Dic.ContainsKey(Word) Then
            Return SpellCheckWordError.OK
        Else
            If (From xItem In HankDic.Dic Where LCase(xItem.Key) = LCase(Word)).Count > 0 Then
                'may be case error?
                If HankDic.Dic.ContainsKey(LCase(Word)) Then
                    'ok
                    Return SpellCheckWordError.OK
                Else
                    Return SpellCheckWordError.CaseError
                End If
            Else
                Return SpellCheckWordError.SpellError
            End If
        End If
    End Function

    Public Overrides Sub SaveInternal(ByVal Filename As String, Optional ByVal ForceFullSave As Boolean = False)
        SaveUserDictionary(Filename, ForceFullSave)
    End Sub

    Public Overrides ReadOnly Property DicFileFilter() As String
        Get
            Return "*.txt"
        End Get
    End Property

End Class
