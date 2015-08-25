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

Public NotInheritable Class DictionaryPerformanceCounter

    Public Const CatName As String = "i00 SpellCheck"
    Public Const WordCheckCounterName As String = "Word check"
    Public Const SuggestionLookupCounterName As String = "Suggestion lookup"

    Public Shared Sub Remove()
        'delete the counter
        Try
            If PerformanceCounterCategory.Exists(CatName) Then
                PerformanceCounterCategory.Delete(CatName)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Shared Sub Instate()
        'create the counter
        Static TriedToCreate As Boolean
        Try
            If TriedToCreate = False AndAlso PerformanceCounterCategory.Exists(CatName) = False Then
                TriedToCreate = True
                Dim counterDataCollection As New CounterCreationDataCollection()

                Dim averageCount64 As New CounterCreationData()
                averageCount64.CounterType = PerformanceCounterType.RateOfCountsPerSecond32
                averageCount64.CounterName = WordCheckCounterName
                counterDataCollection.Add(averageCount64)

                averageCount64 = New CounterCreationData()
                averageCount64.CounterType = PerformanceCounterType.RateOfCountsPerSecond32
                averageCount64.CounterName = SuggestionLookupCounterName
                counterDataCollection.Add(averageCount64)

                PerformanceCounterCategory.Create(CatName, _
                                                  "", _
                                                  PerformanceCounterCategoryType.SingleInstance, _
                                                  counterDataCollection)
            End If
        Catch ex As Exception

        End Try
        TriedToCreate = True
    End Sub

    Private Shared mc_WordCheckCounter As PerformanceCounter
    Public Shared ReadOnly Property WordCheckCounter() As PerformanceCounter
        Get
            Static TriedToCreate As Boolean
            If TriedToCreate = False AndAlso mc_WordCheckCounter Is Nothing Then
                TriedToCreate = True
                Try
                    Instate()
                    If PerformanceCounterCategory.Exists(CatName) Then
                        mc_WordCheckCounter = New PerformanceCounter(CatName, WordCheckCounterName, False)
                    End If
                Catch ex As Exception

                End Try
            End If
            Return mc_WordCheckCounter
        End Get
    End Property

    Private Shared mc_SuggestionLookupCounter As PerformanceCounter
    Public Shared ReadOnly Property SuggestionLookupCounter() As PerformanceCounter
        Get
            Static TriedToCreate As Boolean
            If TriedToCreate = False AndAlso mc_SuggestionLookupCounter Is Nothing Then
                TriedToCreate = True
                Try
                    Instate()
                    If PerformanceCounterCategory.Exists(CatName) Then
                        mc_SuggestionLookupCounter = New PerformanceCounter(CatName, SuggestionLookupCounterName, False)
                    End If
                Catch ex As Exception

                End Try
            End If
            Return mc_SuggestionLookupCounter
        End Get
    End Property

End Class
