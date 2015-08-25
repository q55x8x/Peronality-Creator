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

Partial Public MustInherit Class Dictionary

#Region "Loading / Saving"

    Dim mc_Loading As Boolean
    Public ReadOnly Property Loading() As Boolean
        Get
            Return mc_Loading
        End Get
    End Property

    Public MustOverride Sub LoadFromFileInternal(ByVal Filename As String)

    Public Sub LoadFromFile(ByVal Filename As String)
        mc_Loading = True
        LoadFromFileInternal(Filename)

        mc_Loading = False
        mc_Filename = Filename
    End Sub

    Public MustOverride Sub SaveInternal(ByVal Filename As String, Optional ByVal ForceFullSave As Boolean = False)

    Public Sub Save(Optional ByVal FileName As String = "", Optional ByVal ForceFullSave As Boolean = False)
        If FileName = "" Then FileName = Me.Filename
        If FileName = "" Then
            Throw New Exception("Dictionary filename must be specified as no dictionary has been loaded yet")
        End If

        'if different filename or file doesn't exist then need a full save
        If ForceFullSave = False AndAlso (mc_Filename <> FileName OrElse FileIO.FileSystem.FileExists(FileName) = False) Then
            ForceFullSave = True
        End If

        SaveInternal(FileName, ForceFullSave)
        mc_Filename = FileName
    End Sub

#End Region

#Region "Spelling Suggestions"

    Public Class SpellCheckSuggestionInfo
        Public Closness As Integer
        Public Word As String
        Public Sub New(ByVal Closness As Integer, ByVal Word As String)
            Me.Closness = Closness
            Me.Word = Word
        End Sub
    End Class

    Public Function SpellCheckSuggestions(ByVal Word As String) As List(Of SpellCheckSuggestionInfo)
        Dim pc = DictionaryPerformanceCounter.SuggestionLookupCounter
        If pc IsNot Nothing Then
            pc.Increment()
        End If

        Return SpellCheckSuggestionsInternal(Word)
    End Function

    Public MustOverride Function SpellCheckSuggestionsInternal(ByVal Word As String) As List(Of SpellCheckSuggestionInfo)

    Public Enum SpellCheckWordError
        OK
        SpellError
        CaseError
        Ignore

        NotInDictionary = -1
    End Enum

    Public Function SpellCheckWord(ByVal Word As String) As SpellCheckWordError
        Dim pc = DictionaryPerformanceCounter.WordCheckCounter
        If pc IsNot Nothing Then
            pc.Increment()
        End If

        Return SpellCheckWordInternal(Word)
    End Function

    Public MustOverride Function SpellCheckWordInternal(ByVal Word As String) As SpellCheckWordError

#End Region

    Dim mc_Filename As String

    Protected Friend Sub SetFilename(ByVal Filename As String)
        mc_Filename = Filename
    End Sub

    Public ReadOnly Property Filename() As String
        Get
            Return mc_Filename
        End Get
    End Property

    Public MustOverride ReadOnly Property DicFileFilter() As String

    Public MustOverride Sub Add(ByVal Item As String)

    Public MustOverride Sub Ignore(ByVal Item As String)

    Public MustOverride Sub UnIgnore(ByVal Item As String)

    Public MustOverride Sub Remove(ByVal Item As String)

    Public Shared Event DefaultDictionaryChanged(ByVal sender As Object, ByVal e As EventArgs)
    Private Shared mc_DefaultDictionary As Dictionary
    Public Shared Property DefaultDictionary() As Dictionary
        Get
            Return mc_DefaultDictionary
        End Get
        Set(ByVal value As Dictionary)
            mc_DefaultDictionary = value
            RaiseEvent DefaultDictionaryChanged(Nothing, EventArgs.Empty)
        End Set
    End Property

    Public MustOverride ReadOnly Property Count() As Integer

End Class
