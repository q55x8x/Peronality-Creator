Public Class HankDic

    Public Dic As New Dictionary(Of String, Integer)

#Region "Load"

    Public Sub LoadDic(ByVal dicFile As String)
        Dic = New Dictionary(Of String, Integer)()

        Dim lines = System.IO.File.ReadAllLines(dicFile)
        LoadDic(lines)
    End Sub

    Public Sub LoadDic(ByVal lines() As String)
        For Each line In lines
            If line <> "" Then
                Dim dicItem = line.Split(ControlChars.Tab)
                If dicItem.Length = 2 Then
                    Dic.Add(dicItem(0), Integer.Parse(dicItem(1)))
                End If
            End If
        Next
    End Sub

#End Region

#Region "Suggestions"

    Public Function GetSuggestWords(ByVal word As String, Optional ByVal ReturnCount As Integer = 5) As List(Of String)
        Dim result = GetSuggestionsFromCommonMistakes(word).Where(Function(w) Dic.ContainsKey(w)).ToList()

        If result.Count = 0 Then
            result = GetSuggestionsFrom2CommonMistakes(word)
            'qwertyuiop - got rid of the thing that returns the word as it was entered
        End If

        Return result.OrderByDescending(Function(w) Dic(w)).Take(ReturnCount).ToList() 'qwertyuiop - take orig used a math.min??? ... and removed the order by's check to see if it is in the dictionary since the GetSuggestionsFrom2CommonMistakes (previously GetEdits2) now doesn't have all of the words returned

    End Function

    Private Function GetSuggestionsFromCommonMistakes(ByVal word As String) As List(Of String)
        Dim n = word.Length
        Dim tempWord = ""
        Dim editsWords = New List(Of String)()

        'delete
        For i As Integer = 0 To n - 1
            tempWord = word.Substring(0, i) + word.Substring(i + 1)
            If Not editsWords.Contains(tempWord) Then
                editsWords.Add(tempWord)
            End If
        Next

        'transposition
        For i As Integer = 0 To n - 2
            tempWord = word.Substring(0, i) + word.Substring(i + 1, 1) + word.Substring(i, 1) + word.Substring(i + 2)
            If Not editsWords.Contains(tempWord) Then
                editsWords.Add(tempWord)
            End If
        Next

        'replace
        For i As Integer = 0 To n - 1
            Dim t As String = word.Substring(i, 1)
            For ch As Integer = Asc("a"c) To Asc("z"c)
                If ch <> Asc(t) Then
                    tempWord = word.Substring(0, i) + Convert.ToChar(ch) + word.Substring(i + 1)
                    If Not editsWords.Contains(tempWord) Then
                        editsWords.Add(tempWord)
                    End If
                End If
            Next
        Next

        'insert
        For i As Integer = 0 To n
            For ch As Integer = Asc("a"c) To Asc("z"c)
                tempWord = word.Substring(0, i) + Convert.ToChar(ch) + word.Substring(i)
                If Not editsWords.Contains(tempWord) Then
                    editsWords.Add(tempWord)
                End If
            Next
        Next

        Return editsWords
    End Function

    Private Function GetSuggestionsFrom2CommonMistakes(ByVal word As String) As List(Of String)
        Dim words = GetSuggestionsFromCommonMistakes(word)
        Dim result = New List(Of String) 'qwertyuiop - this was cloning words object before??? wtf - means all words are suggested below the good matches??

        For Each edit In words
            GetSuggestionsFromCommonMistakes(edit).ForEach(Function(w) AddWord(w, result))
        Next
        Return result
    End Function

    Private Function AddWord(ByVal w As String, ByRef r As List(Of String)) As Boolean
        If Dic.ContainsKey(w) Then
            r.Add(w)
            Return True
        End If
    End Function

#End Region

End Class
