Partial Class FlatFileDictionary
    Implements Dictionary.Interfaces.iAnagram

    Public Function AnagramLookup(ByVal Letters As String) As List(Of String) Implements Dictionary.Interfaces.iAnagram.AnagramLookup
        AnagramLookup = New List(Of String)

        Letters = Letters.ToLower

        Dim Dict() As String = (From xItem In IndexedDictionary.GetFullList Select xItem.ToLower).ToArray 'fill dict here

        Dim WordsWithMatchingChars = (From xItem In Dict Where xItem.Length = Letters.Length Select New With {.Word = xItem, .LetterCount = (From xItemWordMatch In xItem Where Letters.Contains(xItemWordMatch)).Count}).ToArray

        Dim MatchingLenthWords = (From xItem In WordsWithMatchingChars Where xItem.LetterCount = Letters.Count Select xItem.Word).ToArray

        'final check to check each char occurs only for each char:
        For Each iMatchedWord In MatchingLenthWords
            Dim CheckLetters = Letters
            For Each iMatchedWordLetter In iMatchedWord
                'find and remove the letter from check
                Dim index = CheckLetters.IndexOf(iMatchedWordLetter)
                If index <> -1 Then
                    CheckLetters = CheckLetters.Remove(index, 1)
                End If
            Next
            If CheckLetters.Count = 0 Then
                'all matched
                AnagramLookup.Add(iMatchedWord)
            End If
        Next
    End Function
End Class
