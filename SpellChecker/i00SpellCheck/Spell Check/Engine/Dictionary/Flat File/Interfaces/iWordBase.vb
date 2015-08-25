Partial Class FlatFileDictionary
    Implements Dictionary.Interfaces.iWordBase

    Private Function ContainsBaseWord(ByVal Word As String, ByVal CaseInsensitive As Boolean) As Boolean
        Return (From xItem In IndexedDictionary.GetFullList Where If(CaseInsensitive = False, xItem = Word, xItem.ToLower = Word.ToLower)).Count > 0
    End Function

    'makes countries becomes country etc
    Public Function FindBaseWord(ByVal Word As String) As Dictionary.Interfaces.FindBaseWordReturn Implements Dictionary.Interfaces.iWordBase.FindBaseWord
        Word = Word.Trim("'"c)

        Dim FindBaseWordReturn As New Dictionary.Interfaces.FindBaseWordReturn
        FindBaseWordReturn.Found = False
        FindBaseWordReturn.WordBase = Word

        If Word.EndsWith("ing", StringComparison.OrdinalIgnoreCase) Then
            FindBaseWordReturn.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.Progressive
        ElseIf Word.EndsWith("er", StringComparison.OrdinalIgnoreCase) Then
            FindBaseWordReturn.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.Comparative
        ElseIf Word.EndsWith("ed", StringComparison.OrdinalIgnoreCase) Then
            FindBaseWordReturn.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.PastTense
        ElseIf Word.EndsWith("ist", StringComparison.OrdinalIgnoreCase) Then
            FindBaseWordReturn.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.Superlative
        ElseIf Word.EndsWith("en", StringComparison.OrdinalIgnoreCase) Then
            FindBaseWordReturn.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.PastParticiple
        End If

        Dim ReTryWord As String = ""
        If Word.EndsWith("ing", StringComparison.OrdinalIgnoreCase) OrElse Word.EndsWith("ed", StringComparison.OrdinalIgnoreCase) OrElse Word.EndsWith("er", StringComparison.OrdinalIgnoreCase) OrElse Word.EndsWith("est", StringComparison.OrdinalIgnoreCase) OrElse Word.EndsWith("en", StringComparison.OrdinalIgnoreCase) Then
            Dim theWord = System.Text.RegularExpressions.Regex.Replace(Word, "(ing|ed|er|est|en)$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            'see if the word exists in the dictionary
            If ContainsBaseWord(theWord, True) Then
                'found
                FindBaseWordReturn.Found = True
                FindBaseWordReturn.WordBase = theWord
            Else
                'drop 1 letter if double letters at the back eg cancelled... on ed/ing words
                If Word.EndsWith("ing", StringComparison.OrdinalIgnoreCase) OrElse Word.EndsWith("ed", StringComparison.OrdinalIgnoreCase) Then
                    If theWord.Length >= 2 AndAlso theWord.ToLower.EndsWith(New String(theWord.ToLower.Last, 2)) Then
                        Dim OrigTheWord = theWord
                        theWord = theWord.Remove(theWord.Length - 1, 1)
                        If ContainsBaseWord(theWord, True) Then
                            'found
                            FindBaseWordReturn.Found = True
                            FindBaseWordReturn.WordBase = theWord
                        Else
                            'not found put the letter back and continue
                            theWord = OrigTheWord
                        End If
                    End If
                End If

                If FindBaseWordReturn.Found = False Then
                    'add the e back? - for words like ignoring/ignored = ignore, wider = wide, shaven = shave
                    ReTryWord = theWord
                    theWord &= "e"
                    If ContainsBaseWord(theWord, True) Then
                        'found
                        FindBaseWordReturn.Found = True
                        FindBaseWordReturn.WordBase = theWord
                    Else
                        If Word.EndsWith("ier", StringComparison.OrdinalIgnoreCase) OrElse Word.EndsWith("iest", StringComparison.OrdinalIgnoreCase) Then
                            'for words like happier/happiest = happy
                            theWord = System.Text.RegularExpressions.Regex.Replace(Word, "(ier|iest)$", "y", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                            If ContainsBaseWord(theWord, True) Then
                                FindBaseWordReturn.Found = True
                                FindBaseWordReturn.WordBase = theWord
                            Else
                                'not found...
                                'cannot extend off ist or ier
                                theWord = ""
                            End If
                        Else
                            'not found...
                        End If
                    End If
                End If
            End If
        ElseIf Word.EndsWith("ies", StringComparison.OrdinalIgnoreCase) Then
            Dim theWord = System.Text.RegularExpressions.Regex.Replace(Word, "ies$", "y", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            If ContainsBaseWord(theWord, True) Then
                FindBaseWordReturn.Found = True
                FindBaseWordReturn.WordBase = theWord
                FindBaseWordReturn.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.Plural
            Else
                'not found...
                ReTryWord = theWord
            End If
        ElseIf Word.EndsWith("s", StringComparison.OrdinalIgnoreCase) OrElse Word.EndsWith("'s", StringComparison.OrdinalIgnoreCase) Then ' OrElse Word.EndsWith("'", StringComparison.OrdinalIgnoreCase) Then
            Dim theWord = System.Text.RegularExpressions.Regex.Replace(Word, "('?s|')$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase) 'Old match for words ending in ' "('?s|')$"
            If ContainsBaseWord(theWord, True) Then
                FindBaseWordReturn.Found = True
                FindBaseWordReturn.WordBase = theWord
                FindBaseWordReturn.BaseType = Interfaces.FindBaseWordReturn.BaseTypes.Plural
            Else
                FindBaseWordReturn = FindBaseWord(theWord)
                FindBaseWordReturn.BaseType = FindBaseWordReturn.BaseType Or Interfaces.FindBaseWordReturn.BaseTypes.Plural
            End If
        Else
            'not found... and no special pre/suf-ixes
        End If
        'do multiple times ... for words like quickened
        If ReTryWord <> "" AndAlso FindBaseWordReturn.Found = False Then
            Dim OldBaseWordReturn = FindBaseWordReturn
            FindBaseWordReturn = FindBaseWord(ReTryWord)
            FindBaseWordReturn.BaseType = FindBaseWordReturn.BaseType Or OldBaseWordReturn.BaseType
        End If
        If FindBaseWordReturn.Found = False Then FindBaseWordReturn.BaseType = Nothing
        Return FindBaseWordReturn
    End Function


End Class
