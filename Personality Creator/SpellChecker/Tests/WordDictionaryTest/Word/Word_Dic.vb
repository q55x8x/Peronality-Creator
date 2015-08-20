Partial Class Word

    Public Class Word_Dic
        Inherits i00SpellCheck.Dictionary

#Region "To Make things nicer for the prop grid"

        <System.ComponentModel.Browsable(False)> _
        Public Overrides ReadOnly Property Count() As Integer
            Get
                'so we spell check ...
                '...otherwise i00 Spell check will not bother as it will think that there are 0 words in the dictionary
                Return -1
            End Get
        End Property

        Public ReadOnly Property Version() As String
            Get
                Return WordApp.Version
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return "Word Dictionary"
        End Function

#End Region

        Public Overrides Sub Add(ByVal Item As String)

        End Sub

        Public Overrides ReadOnly Property DicFileFilter() As String
            Get
                Return ""
            End Get
        End Property

        Public Overrides Sub Ignore(ByVal Item As String)

        End Sub

        Public Overrides Sub LoadFromFileInternal(ByVal Filename As String)

        End Sub

        Public Overrides Sub Remove(ByVal Item As String)

        End Sub

        Public Overrides Sub SaveInternal(ByVal Filename As String, Optional ByVal ForceFullSave As Boolean = False)

        End Sub

        Public Overrides Function SpellCheckSuggestionsInternal(ByVal Word As String) As System.Collections.Generic.List(Of i00SpellCheck.Dictionary.SpellCheckSuggestionInfo)
            Dim suggestions = WordApp.GetSpellingSuggestions(Word)
            SpellCheckSuggestionsInternal = New List(Of i00SpellCheck.Dictionary.SpellCheckSuggestionInfo)
            Dim closeness As Integer
            For Each suggestion In suggestions.OfType(Of Microsoft.Office.Interop.Word.SpellingSuggestion)()
                SpellCheckSuggestionsInternal.Add(New i00SpellCheck.Dictionary.SpellCheckSuggestionInfo(closeness, suggestion.Name))
                closeness -= 1
            Next
        End Function

        Public Overrides Function SpellCheckWordInternal(ByVal Word As String) As i00SpellCheck.Dictionary.SpellCheckWordError
            Return If(WordApp.CheckSpelling(Word), SpellCheckWordError.OK, SpellCheckWordError.SpellError)
        End Function

        Public Overrides Sub UnIgnore(ByVal Item As String)

        End Sub
    End Class

End Class