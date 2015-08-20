Public Class Hunspell_Syn
    Inherits i00SpellCheck.Synonyms

    Dim LoadedFile As String

    Dim Thes As New NHunspell.MyThes

    Public Overrides Property File() As String
        Get
            Return MyBase.File
        End Get
        Set(ByVal value As String)
            MyBase.File = value
            Thes = Nothing
            If mt_LoadSyn IsNot Nothing AndAlso mt_LoadSyn.IsAlive Then
                mt_LoadSyn.Abort()
            End If
            mt_LoadSyn = New System.Threading.Thread(AddressOf mtLoadSyn)
            mt_LoadSyn.IsBackground = True
            mt_LoadSyn.Name = "Loading synonyms file " & value
            mt_LoadSyn.Start()
        End Set
    End Property

    Dim mt_LoadSyn As Threading.Thread
    Private Sub mtLoadSyn()
        Thes = New NHunspell.MyThes(File)
    End Sub

    Public Overrides Function FindWord(ByVal Word As String) As System.Collections.Generic.List(Of i00SpellCheck.Synonyms.FindWordReturn)
        If Thes Is Nothing Then Return Nothing

        Dim Suggestions = Thes.Lookup(Word)
        If Suggestions Is Nothing Then Return Nothing
        Dim ReturnResult As New List(Of i00SpellCheck.Synonyms.FindWordReturn)
        For Each item In Suggestions.Meanings
            Dim FindWordReturn As New i00SpellCheck.Synonyms.FindWordReturn
            FindWordReturn.TypeDescription = item.Description
            'FindWordReturn.WordType = i00SpellCheck.Synonyms.FindWordReturn.WordTypes.Other
            FindWordReturn.AddRange(item.Synonyms)
            ReturnResult.Add(FindWordReturn)
        Next

        If ReturnResult.Count = 0 Then Return Nothing

        Return ReturnResult
    End Function

End Class
