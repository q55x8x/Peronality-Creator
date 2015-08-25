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

Partial Public Class Dictionary

    Partial Public Class Interfaces

#Region "Scrabble result"

        Public Class ScrabbleResult
            Public Word As String
            Public Shared ReadOnly Property ScrabbleScoreLetters() As Dictionary(Of Char, Integer)
                Get
                    Static mc_ScrabbleScoreLetters As Dictionary(Of Char, Integer)
                    If mc_ScrabbleScoreLetters Is Nothing Then
                        mc_ScrabbleScoreLetters = New Dictionary(Of Char, Integer)()
                        mc_ScrabbleScoreLetters.Add("A"c, 1)
                        mc_ScrabbleScoreLetters.Add("B"c, 3)
                        mc_ScrabbleScoreLetters.Add("C"c, 3)
                        mc_ScrabbleScoreLetters.Add("D"c, 2)
                        mc_ScrabbleScoreLetters.Add("E"c, 1)
                        mc_ScrabbleScoreLetters.Add("F"c, 4)
                        mc_ScrabbleScoreLetters.Add("G"c, 2)
                        mc_ScrabbleScoreLetters.Add("H"c, 4)
                        mc_ScrabbleScoreLetters.Add("I"c, 1)
                        mc_ScrabbleScoreLetters.Add("J"c, 8)
                        mc_ScrabbleScoreLetters.Add("K"c, 5)
                        mc_ScrabbleScoreLetters.Add("L"c, 1)
                        mc_ScrabbleScoreLetters.Add("M"c, 3)
                        mc_ScrabbleScoreLetters.Add("N"c, 1)
                        mc_ScrabbleScoreLetters.Add("O"c, 1)
                        mc_ScrabbleScoreLetters.Add("P"c, 3)
                        mc_ScrabbleScoreLetters.Add("Q"c, 10)
                        mc_ScrabbleScoreLetters.Add("R"c, 1)
                        mc_ScrabbleScoreLetters.Add("S"c, 1)
                        mc_ScrabbleScoreLetters.Add("T"c, 1)
                        mc_ScrabbleScoreLetters.Add("U"c, 1)
                        mc_ScrabbleScoreLetters.Add("V"c, 4)
                        mc_ScrabbleScoreLetters.Add("W"c, 4)
                        mc_ScrabbleScoreLetters.Add("X"c, 8)
                        mc_ScrabbleScoreLetters.Add("Y"c, 4)
                        mc_ScrabbleScoreLetters.Add("Z"c, 10)
                    End If
                    Return mc_ScrabbleScoreLetters
                End Get
            End Property
            Public ReadOnly Property Score() As Integer
                Get
                    Return (From xItem In Word.ToUpper Where ScrabbleResult.ScrabbleScoreLetters.ContainsKey(xItem) Select ScrabbleResult.ScrabbleScoreLetters(xItem)).Sum
                End Get
            End Property
            Public Sub New(ByVal Word As String)
                Me.Word = Word
            End Sub
        End Class

#End Region

        Public Interface iScrabble
            Function ScrabbleLookup(ByVal Letters As String) As List(Of ScrabbleResult)
        End Interface

    End Class

End Class
