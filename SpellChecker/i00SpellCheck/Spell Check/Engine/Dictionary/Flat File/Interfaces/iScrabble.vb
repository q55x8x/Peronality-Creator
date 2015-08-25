Partial Class FlatFileDictionary
    Implements Dictionary.Interfaces.iScrabble

    Public Function ScrabbleLookup(ByVal Letters As String) As System.Collections.Generic.List(Of Dictionary.Interfaces.ScrabbleResult) Implements Dictionary.Interfaces.iScrabble.ScrabbleLookup
        ScrabbleLookup = New List(Of Interfaces.ScrabbleResult)

        Dim Dict() As String = (From xItem In IndexedDictionary.GetFullList Select xItem.ToLower).ToArray

        For Each iDict In Dict
            Dim CheckLetters = Letters
            For Each iDictLetter In iDict 'for each letter in our dictionary
                Dim index = CheckLetters.IndexOf(iDictLetter)
                If index = -1 Then
                    'this letter is not in our Letters
                    GoTo NextWord
                Else
                    'take the letter out
                    CheckLetters = CheckLetters.Remove(index, 1)
                End If
            Next
            'if we got this far we were can add the word to our return :)
            ScrabbleLookup.Add(New Interfaces.ScrabbleResult(iDict))
NextWord:
        Next
    End Function

End Class
