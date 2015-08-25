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

Public MustInherit Class UserDictionaryBase
    Inherits Dictionary

#Region "Add / Remove / Ignore / UnIgnore"

    Public Event WordAdded(ByVal Item As String)
    Public Overrides Sub Add(ByVal Item As String)
        RemoveWordFromList(Item)
        UserWordList.Add(New UserDictItem With {.Word = Item, .PendingChanges = True, .State = SpellCheckWordError.OK})
        RaiseEvent WordAdded(Item)
    End Sub

    Public Event WordUnIgnored(ByVal Item As String)
    Public Overrides Sub UnIgnore(ByVal Item As String)
        RemoveWordFromList(Item)
        UserWordList.Add(New UserDictItem With {.Word = Item, .PendingChanges = True, .State = SpellCheckWordError.SpellError})
        RaiseEvent WordUnIgnored(Item)
    End Sub

    Public Event WordIgnore(ByVal Item As String)
    Public Overrides Sub Ignore(ByVal Item As String)
        'qwetyuiop - 'could be multiple items with the same word different case..?? so only remove words that arn't already ignored?
        RemoveWordFromList(Item)
        UserWordList.Add(New UserDictItem With {.Word = Item, .PendingChanges = True, .State = SpellCheckWordError.Ignore})
        RaiseEvent WordIgnore(Item)
    End Sub

    Public Event WordRemoved(ByVal Item As String)
    Public Overrides Sub Remove(ByVal Item As String)
        RemoveWordFromList(Item)
        UserWordList.Add(New UserDictItem With {.Word = Item, .PendingChanges = True, .State = SpellCheckWordError.SpellError})
        RaiseEvent WordRemoved(Item)
    End Sub

    Private Sub RemoveWordFromList(ByVal Word As String)
        Dim AllSameWords = (From xItem In UserWordList Where LCase(xItem.Word) = LCase(Word)).ToArray
        For Each Item In AllSameWords
            'remove the existing user dictionary items
            UserWordList.Remove(Item)
        Next
    End Sub

#End Region

    Public MustOverride Function SpellCheckWordNonUser(ByVal Word As String) As i00SpellCheck.Dictionary.SpellCheckWordError

    Public Overrides Function SpellCheckWordInternal(ByVal Word As String) As i00SpellCheck.Dictionary.SpellCheckWordError
        SpellCheckWordInternal = IsInUserDictionary(Word)
        If SpellCheckWordInternal = SpellCheckWordError.NotInDictionary Then
            Return SpellCheckWordNonUser(Word)
        End If
    End Function

    Public MustOverride Function SpellCheckSuggestionsNonUser(ByVal Word As String) As System.Collections.Generic.List(Of Dictionary.SpellCheckSuggestionInfo)

    Public Overrides Function SpellCheckSuggestionsInternal(ByVal Word As String) As System.Collections.Generic.List(Of Dictionary.SpellCheckSuggestionInfo)
        SpellCheckSuggestionsInternal = SpellCheckSuggestionsNonUser(Word)
        'remove items in the user dict that have been removed
        Dim RemovedItems = (From xItem In SpellCheckSuggestionsInternal Join xItemUser In UserWordList On LCase(xItem.Word) Equals LCase(xItemUser.Word) Where xItemUser.State = SpellCheckWordError.SpellError).ToArray
        For Each item In RemovedItems
            SpellCheckSuggestionsInternal.Remove(item.xItem)
        Next
    End Function

    Public Class UserDictItem
        Public Shared Function FromLine(ByVal line As String) As UserDictItem
            If line = "" Then Return Nothing

            Dim UserDictItem As New UserDictItem
            Select Case line.First
                Case "+"c
                    UserDictItem.State = SpellCheckWordError.OK
                Case "-"c
                    UserDictItem.State = SpellCheckWordError.SpellError
                Case "*"c
                    UserDictItem.State = SpellCheckWordError.Ignore
                Case Else
                    Return Nothing
            End Select
            UserDictItem.Word = line.Substring(1)
            Return UserDictItem
        End Function
        Public Word As String
        Public PendingChanges As Boolean
        Public State As Dictionary.SpellCheckWordError
        Public Overrides Function ToString() As String
            Dim Symbol As Char = New Char
            Select Case State
                Case SpellCheckWordError.OK
                    Symbol = "+"c
                Case SpellCheckWordError.Ignore
                    Symbol = "*"c
                Case SpellCheckWordError.SpellError
                    Symbol = "-"c
                Case Else
                    Return ""
            End Select
            Return Symbol & Word
        End Function
    End Class

    Public UserWordList As New List(Of UserDictItem)

    Public Sub LoadUserDictionary(ByVal Filename As String)
        UserWordList.Clear()
        Dim UserDicFile As String = Filename & ".user"
        If FileIO.FileSystem.FileExists(UserDicFile) Then
            Dim fileData = My.Computer.FileSystem.ReadAllText(UserDicFile)
            Dim lines = fileData.Split(New String() {vbCrLf, vbCr, vbLf}, StringSplitOptions.RemoveEmptyEntries).ToArray

            Dim lineData = (From xItem In (From xItem In lines Select UserDictItem.FromLine(xItem)) Where xItem IsNot Nothing).ToArray
            UserWordList.AddRange(lineData)
        End If
    End Sub

    Public Sub SaveUserDictionary(Optional ByVal FileName As String = "", Optional ByVal ForceFullSave As Boolean = False)
        Dim UserDicFile As String = FileName & ".user"
        If ForceFullSave OrElse FileIO.FileSystem.FileExists(UserDicFile) = False Then
            'full save
            My.Computer.FileSystem.WriteAllText(UserDicFile, Join((From xItem In UserWordList Select (xItem.ToString())).ToArray, vbCrLf), False)
        Else
            'only save the items that are new ... this is done so multiple users can modify the user dictionary
            'load the user file
            Dim CompareDict As New BasicUserDictionaryBase
            CompareDict.LoadUserDictionary(FileName)
            Dim PendingItems = (From xItem In UserWordList Where xItem.PendingChanges = True).ToArray
            For Each item In PendingItems
                'find the word in the compare dict .. 
                Dim theItem = item
                Dim CompareDictWords = (From xItem In CompareDict.UserWordList Where LCase(xItem.Word) = LCase(theItem.Word)).ToArray
                For Each CompareItem In CompareDictWords
                    'remove the existing user dictionary items
                    CompareDict.UserWordList.Remove(CompareItem)
                Next
                'add the word back
                CompareDict.UserWordList.Add(item)
            Next

            CompareDict.Save(FileName, True)

            'mark the changes as committed
            For Each item In PendingItems
                item.PendingChanges = False
            Next
        End If

    End Sub

    Public Function IsInUserDictionary(ByVal Word As String) As Dictionary.SpellCheckWordError
        Dim MatchedWord = (From xItem In UserWordList Where LCase(xItem.Word) = LCase(Word)).FirstOrDefault
        'qwetyuiop - 'could be multiple items with the same word different case..??
        If MatchedWord Is Nothing Then
            Return SpellCheckWordError.NotInDictionary
        Else
            'qwertyuiop - check for case
            If MatchedWord.State = SpellCheckWordError.OK Then
                'check the case
                If Formatting.CaseOK(Word, MatchedWord.Word) Then
                    Return SpellCheckWordError.OK
                Else
                    Return SpellCheckWordError.CaseError
                End If
            Else
                Return MatchedWord.State
            End If
        End If
    End Function

    'Dim asd As Dictionary.SpellCheckWordError = SpellCheckWordError.OK
    Public Overrides ReadOnly Property Count() As Integer
        Get
            Return UserWordList.Count
        End Get
    End Property

#Region "Comparer dictionary for saving the user dictionary"

    Private Class BasicUserDictionaryBase
        Inherits UserDictionaryBase

        Public Overrides Function SpellCheckSuggestionsNonUser(ByVal Word As String) As System.Collections.Generic.List(Of Dictionary.SpellCheckSuggestionInfo)
            Return Nothing
        End Function

        Public Overrides Sub LoadFromFileInternal(ByVal Filename As String)

        End Sub

        Public Overloads Overrides Sub SaveInternal(ByVal Filename As String, Optional ByVal ForceFullSave As Boolean = False)
            SaveUserDictionary(Filename, ForceFullSave)
        End Sub

        Public Overrides Function SpellCheckWordNonUser(ByVal Word As String) As Dictionary.SpellCheckWordError

        End Function

        Public Overrides ReadOnly Property DicFileFilter() As String
            Get
                Return ""
            End Get
        End Property
    End Class

#End Region

End Class
