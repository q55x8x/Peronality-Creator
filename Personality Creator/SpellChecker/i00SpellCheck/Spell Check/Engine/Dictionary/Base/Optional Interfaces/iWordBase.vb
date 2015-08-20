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

        Public Interface iWordBase
            Function FindBaseWord(ByVal Word As String) As FindBaseWordReturn
        End Interface

        Public Class FindBaseWordReturn
            Public Found As Boolean
            Public WordBase As String
            Public Enum BaseTypes
                None = 0 'base word
                Plural = 1 's / ies
                Progressive = 2 'ing
                PastTense = 4 'ed
                Comparative = 8 'er
                Superlative = 16 'ist
                PastParticiple = 32 'en
                'tion - suggestion

                'need stuff for??:
                'ian - comedian, politic-ian, utop-ian, austral-ian, as-ian - EnumName: Origin
                'ise, ize - neutralise, plagiarised + ed in this case...
                'ify - mistify, acidify - EnumName: Produce
                'ful - playful
                'ly - playfully - ful+ly, happily
                'able - movable, blamable - drop e
                'ible - corruptible, may also start with in or irr eg incorruptible / irresistible (readd r in this case)
                'ness - happiness
                'less - careless
                'ism - industrialism
                'al - industryal
                'ment - enjoyment
                'ist - perfectionist, artist
                'ish - English

                'prefixes:
                'irr - irregular
                'un - unnatural
                'ab - abnormal
                'de - detoxify, degrade, demote - EnumName: Reverse
                're - redo, reconstitute, reopen - EnumName: Repetitive
            End Enum

            Public BaseType As BaseTypes
            Public ReadOnly Property BaseTypeToArray() As String()
                Get
                    Return (From xItem In [Enum].GetValues(GetType(BaseTypes)).OfType(Of BaseTypes)() Where xItem <> BaseTypes.None AndAlso (BaseType And xItem) = xItem Select System.Text.RegularExpressions.Regex.Replace([Enum].GetName(GetType(BaseTypes), xItem), "(?=(?<!^)[A-Z])", " ")).ToArray
                End Get
            End Property
        End Class

    End Class

End Class


