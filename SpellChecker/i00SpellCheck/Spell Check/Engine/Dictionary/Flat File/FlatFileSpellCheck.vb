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

Partial Class FlatFileDictionary

#Region "Check that Word is in the Dictionary"

    Public Overrides Function SpellCheckWordNonUser(ByVal Word As String) As Dictionary.SpellCheckWordError
        If Word = "" Then Return SpellCheckWordError.OK
        'Doing this directly on the word object didnot work????:
        Dim theWord = Word

        'Strip 's
        Dim OldWord = theWord
        theWord = Dictionary.Formatting.RemoveApoS(theWord)

        'ignore numbers
        Dim NumericWord = CStr((From xItem In theWord Select xItem Where xItem <> "$" AndAlso xItem <> "." AndAlso xItem <> "%" AndAlso xItem <> "#").ToArray)
        If IsNumeric(NumericWord) Then
            Return SpellCheckWordError.OK 'not in dic
        End If

        SpellCheckWordNonUser = SpellCheckWordError.SpellError

        'add words that start with that letter only
        Dim DicWords = IndexedDictionary.Item(Word)

        If DicWords Is Nothing Then
            'allow word in caps...
            'If Formatting.AllInCaps(Word) Then
            '    Return SpellCheckWordError.OK
            'End If
            Return SpellCheckWordError.SpellError
        End If

        DicWords = (From xItem In DicWords Where LCase(xItem) = LCase(theWord)).ToList

        For Each iDicWord In DicWords
            'words found
            Dim WordCaseOK = Formatting.CaseOK(Word, iDicWord)
            If WordCaseOK Then
                Return SpellCheckWordError.OK
            Else
                SpellCheckWordNonUser = SpellCheckWordError.CaseError
                'bad case
            End If
        Next

    End Function

#End Region

#Region "Spelling Suggestions"

    Private Function GetUserAddedWords() As String()
        Return (From xItem In UserWordList Where xItem.State = SpellCheckWordError.OK Select xItem.Word).ToArray
    End Function

    Public Overrides Function SpellCheckSuggestionsNonUser(ByVal Word As String) As List(Of SpellCheckSuggestionInfo)
        Dim leewaynum As Integer
        Dim leewaypct As Double

        Dim theWord = Word
        Dim OldWord = theWord
        theWord = System.Text.RegularExpressions.Regex.Replace(theWord, "'s$", "") '.. remove 's ... can't use SpellCheckTextBox.RemoveApoS(theWord) as we also want to remove them if we have chris's
        Dim ApoSRemoved = False
        If OldWord <> theWord Then
            ApoSRemoved = True
        End If

        Dim txtlen As Integer = theWord.Length
        Select Case txtlen
            Case Is < 5
                leewaynum = 2
                leewaypct = 0.75
            Case 5 To 7
                leewaynum = 3
                leewaypct = 0.6
            Case 8 To 11
                leewaynum = 4
                leewaypct = 0.5
            Case Else
                leewaynum = 5
                leewaypct = 0.45
        End Select

        'this makes words such as runnning match running 1st then everything else
        Dim theWordNoDups = System.Text.RegularExpressions.Regex.Replace(theWord.ToLower, "(.)(\1)+", "$1")

        'Dim DicWords() As String = Nothing

        'add words that start with that letter only
        Dim DicWords As List(Of String) = IndexedDictionary.Item(Word)
        If DicWords Is Nothing Then Return New List(Of SpellCheckSuggestionInfo)

        'clone the dictword list ... as we don't want to append the user words to the origional list
        DicWords = DicWords.ToList

        'add words from user dict
        DicWords.AddRange(GetUserAddedWords)

        Dim CutDownDict = (From xItem In DicWords Where xItem.ToLower.StartsWith(Left(theWord.ToLower, 1)) AndAlso Len(xItem) > txtlen - leewaynum AndAlso Len(xItem) < txtlen + leewaynum).ToArray

        SpellCheckSuggestionsNonUser = New List(Of SpellCheckSuggestionInfo)

        'Dim StartTime = Environment.TickCount
        For Each iWord In CutDownDict
            Dim nummat As Integer = 0
            Dim allmat As Integer = 0
            Dim firstfewmat As Integer = 0

            'If iWord.StartsWith(Left(theWord, 1), StringComparison.OrdinalIgnoreCase) Then
            If theWord.Contains(Left$(iWord, CInt(leewaypct * txtlen))) Then
                '1st leewaypct of characters match (Sliding scale percentage based on theWord len)
                firstfewmat = CInt(4 * txtlen)    'if first 3 of 4 letters matches, weighting would be an extra 12
            End If
            'If txtlen > 5 And (InStr(1, theWord, Left$(iWord, 3), CompareMethod.Text)) > 0 Then
            '    '1st leewaypct of characters match (Sliding scale percentage based on theWord len)
            '    firstfewmat = firstfewmat + 5    'if first 3 of 4 letters matches, weighting would be an extra 12
            'End If
            If iWord.StartsWith(Left(theWord, 3), StringComparison.OrdinalIgnoreCase) AndAlso iWord.EndsWith(Right(theWord, 2), StringComparison.OrdinalIgnoreCase) Then
                '1st leewaypct of characters match (Sliding scale percentage based on theWord len)
                firstfewmat = firstfewmat + 10    'if first 3 of 4 letters matches, weighting would be an extra 12
            End If
            If iWord.EndsWith("cause", StringComparison.OrdinalIgnoreCase) AndAlso theWord.EndsWith("cose", StringComparison.OrdinalIgnoreCase) Then
                'give extra weight to this common mis-spelling
                firstfewmat = firstfewmat + 20    'if first 3 of 4 letters matches, weighting would be an extra 12
            End If
            If iWord.EndsWith("ds", StringComparison.OrdinalIgnoreCase) AndAlso theWord.EndsWith("des", StringComparison.OrdinalIgnoreCase) Then
                'give extra weight to this common mis-spelling
                firstfewmat = firstfewmat + 20    'if first 3 of 4 letters matches, weighting would be an extra 12
            End If
            If txtlen > 5 AndAlso iWord.EndsWith(Right(theWord, 3), StringComparison.OrdinalIgnoreCase) Then
                'last 3 letters match, give this a bit more weight
                firstfewmat = firstfewmat + txtlen    'if first 3 of 4 letters matches, weighting would be an extra 12
            End If

            For i = 1 To Len(theWord)
                If InStr(If(i - 1 > 1, i - 1, i), iWord, Mid(theWord, i, 1), CompareMethod.Text) > 0 Then 'i-1 to cover transpositions
                    'If InStr(IIf(i - 1 > 1, i - 1, i), theWord, Mid$(iWord, i, 1), 1) > 0 Then 'i-1 to cover transpositions
                    nummat = nummat + 1
                End If
            Next i
            If nummat = txtlen Then
                If txtlen = iWord.Length Then
                    allmat = 100    'extra extra weight for all matches, this would probably be a transposition
                Else
                    allmat = 50 'was 20
                End If
            ElseIf Math.Abs(nummat - txtlen) = 1 Then
                'almost all characters were mached
                allmat = 25
            ElseIf Math.Abs(nummat - txtlen) = 2 Then
                allmat = 15
            ElseIf Math.Abs(nummat - txtlen) = 3 Then
                allmat = 10
            End If
            If nummat + allmat + firstfewmat > 0 Then
                Dim SuggestionTxt = iWord
                If ApoSRemoved Then
                    'add the 's back...
                    If SuggestionTxt.EndsWith("'s", StringComparison.OrdinalIgnoreCase) = False OrElse SuggestionTxt.EndsWith("'") = False Then
                        If SuggestionTxt.EndsWith("s", StringComparison.OrdinalIgnoreCase) Then
                            SuggestionTxt &= "'"
                        Else
                            SuggestionTxt &= "'s"
                        End If
                    End If
                End If
                Dim Closeness = nummat + allmat + firstfewmat
                If System.Text.RegularExpressions.Regex.Replace(iWord.ToLower, "(.)(\1)+", "$1") = theWordNoDups Then
                    Closeness = -1
                End If
                SpellCheckSuggestionsNonUser.Add(New SpellCheckSuggestionInfo(Closeness, SuggestionTxt))
            End If

            'End If
        Next
        If SpellCheckSuggestionsNonUser.Count > 0 Then
            Dim MaxCloseness = SpellCheckSuggestionsNonUser.Max(Function(x As SpellCheckSuggestionInfo) x.Closness)
            For Each iSuggest In (From xItem In SpellCheckSuggestionsNonUser Where xItem.Closness = -1).ToArray
                iSuggest.Closness = MaxCloseness + 1
            Next
        End If

        'Debug.Print((Environment.TickCount - StartTime).ToString)

    End Function

#End Region

End Class