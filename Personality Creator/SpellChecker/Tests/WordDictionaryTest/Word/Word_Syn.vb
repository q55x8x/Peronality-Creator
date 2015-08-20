Partial Class Word

    Public Class Word_Syn
        Inherits i00SpellCheck.Synonyms

#Region "To Make things nicer for the prop grid"

        <System.ComponentModel.Browsable(False)> _
            Public Overrides Property File() As String
            Get
                Return Nothing
            End Get
            Set(ByVal value As String)

            End Set
        End Property

        Public ReadOnly Property Version() As String
            Get
                Return WordApp.Version
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return "Word Synonyms"
        End Function

        <System.ComponentModel.Browsable(False)> _
        Public Overrides ReadOnly Property SynonymCount() As Long
            Get

            End Get
        End Property

#End Region

        Public Overrides Function FindWord(ByVal Word As String) As System.Collections.Generic.List(Of i00SpellCheck.Synonyms.FindWordReturn)
            Dim ThisSyn = WordApp.SynonymInfo(Word)
            FindWord = New List(Of i00SpellCheck.Synonyms.FindWordReturn)
            For Each Meaning In ThisSyn.MeaningList
                Dim FindWordReturn = New i00SpellCheck.Synonyms.FindWordReturn
                FindWordReturn.TypeDescription = Meaning.ToString
                For Each Synonym In ThisSyn.SynonymList(Meaning)
                    FindWordReturn.Add(Synonym.ToString())
                Next
                FindWord.Add(FindWordReturn)
            Next
            If FindWord.Count = 0 Then Return Nothing
        End Function

    End Class

End Class