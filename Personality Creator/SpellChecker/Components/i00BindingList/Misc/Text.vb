'i00 DrawingText Class
'©i00 Productions All rights reserved
'Created by Kris Bennett
'----------------------------------------------------------------------------------------------------
'All property in this file is and remains the property of i00 Productions, regardless of its usage,
'unless stated otherwise in writing from i00 Productions.
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Public Class DrawingText

    Public Class TextRendererMeasure

        Public Shared Function TextFlagsFrom(ByVal CellStyle As DataGridViewCellStyle) As TextFormatFlags

            Dim Flags As TextFormatFlags = TextFormatFlags.PreserveGraphicsClipping Or TextFormatFlags.PreserveGraphicsTranslateTransform Or TextFormatFlags.NoPrefix

            Select Case CellStyle.Alignment
                Case DataGridViewContentAlignment.BottomCenter
                    Flags = Flags Or TextFormatFlags.Bottom
                    Flags = Flags Or TextFormatFlags.HorizontalCenter
                Case DataGridViewContentAlignment.BottomLeft
                    Flags = Flags Or TextFormatFlags.Bottom
                    Flags = Flags Or TextFormatFlags.Left
                Case DataGridViewContentAlignment.BottomRight
                    Flags = Flags Or TextFormatFlags.Bottom
                    Flags = Flags Or TextFormatFlags.Right
                Case DataGridViewContentAlignment.MiddleCenter
                    Flags = Flags Or TextFormatFlags.VerticalCenter
                    Flags = Flags Or TextFormatFlags.HorizontalCenter
                Case DataGridViewContentAlignment.MiddleLeft
                    Flags = Flags Or TextFormatFlags.VerticalCenter
                    Flags = Flags Or TextFormatFlags.Left
                Case DataGridViewContentAlignment.MiddleRight
                    Flags = Flags Or TextFormatFlags.VerticalCenter
                    Flags = Flags Or TextFormatFlags.Right
                Case DataGridViewContentAlignment.TopCenter
                    Flags = Flags Or TextFormatFlags.Top
                    Flags = Flags Or TextFormatFlags.HorizontalCenter
                Case DataGridViewContentAlignment.TopLeft
                    Flags = Flags Or TextFormatFlags.Top
                    Flags = Flags Or TextFormatFlags.Left
                Case DataGridViewContentAlignment.TopRight
                    Flags = Flags Or TextFormatFlags.Top
                    Flags = Flags Or TextFormatFlags.Right
            End Select

            If CellStyle.WrapMode = DataGridViewTriState.True Then
                Flags = Flags Or TextFormatFlags.WordBreak
            Else
                Flags = Flags Or TextFormatFlags.WordEllipsis
            End If

            Return Flags
        End Function

        Public Class WordBounds
            Inherits List(Of WordBound)
            Public TextMargin As Integer
            Public Class WordBound
                Public Bounds As Rectangle
                Public Word As String
                Public WordPosition As Point
                Public LetterIndex As Integer
            End Class
        End Class

        Public Shared Function Measure(ByVal text As String, ByVal font As Font, ByVal proposedSize As Size, ByVal flags As TextFormatFlags, Optional ByVal OnlyReturnRenderedWords As Boolean = True, Optional ByVal PerWord As Boolean = True) As WordBounds
            Measure = New WordBounds
            If text = "" Then Exit Function
            text = Replace(text, vbCrLf, vbCr)
            text = Replace(text, vbLf, vbCr)

            Dim textSize = TextRenderer.MeasureText(text, font, proposedSize, flags)

            Dim Words() As String
            If PerWord Then
                Words = text.Split(" "c, CChar(vbCr))
            Else
                'split every char...
                Words = (From xItem In text.OfType(Of Char)() Select CStr(xItem)).ToArray
            End If



            Dim SlashWidth = TextRenderer.MeasureText("--", font, proposedSize, flags).Width
            Dim SpaceWidth = TextRenderer.MeasureText("- -", font, proposedSize, flags).Width

            Dim TextMargin = SpaceWidth
            TextMargin -= SlashWidth
            TextMargin = TextRenderer.MeasureText(" ", font, proposedSize, flags).Width - TextMargin
            TextMargin = CInt(Math.Round(TextMargin / 2, 0, MidpointRounding.AwayFromZero))

            Measure.TextMargin = TextMargin

            SpaceWidth -= SlashWidth

            Dim NextLeft = 0
            Dim NextTop = 0
            Dim PreText = ""
            Dim LastPreTextHeight = -1
            Dim Lines = 0
            Dim TextX = 0

            Dim FalseFlags = flags

            If (FalseFlags And TextFormatFlags.Right) = TextFormatFlags.Right Then
                FalseFlags = FalseFlags Xor TextFormatFlags.Right
            End If
            If (FalseFlags And TextFormatFlags.Left) = TextFormatFlags.Left Then
                FalseFlags = FalseFlags Xor TextFormatFlags.Left
            End If

            Dim LetterIndex = 0

            Dim ThisIndex As Integer = 0
            For i = 0 To Words.Count - 1
                'to make enters work...
                Dim LineBreakChr = False
                Dim BreakCharIndex = ThisIndex - 1
                If BreakCharIndex >= 0 Then
                    LineBreakChr = text(BreakCharIndex) = vbCr
                End If
                ThisIndex += Len(Words(i)) + If(PerWord, 1, 0)

                PreText &= Words(i) & If(PerWord, " ", "")
                If LineBreakChr Then PreText &= vbCr
                Dim PreTextSize = TextRenderer.MeasureText(If(PreText = "", " ", PreText), font, proposedSize, FalseFlags)
                If LastPreTextHeight = -1 OrElse PreTextSize.Height <> LastPreTextHeight Then
                    If LastPreTextHeight <> -1 Then
                        Lines += 1
                        PreText = Replace(New String(" "c, Lines), " ", vbCrLf) & Words(i) & If(PerWord, " ", "")
                        PreTextSize = TextRenderer.MeasureText(If(PreText = "", " ", PreText), font, proposedSize, FalseFlags)
                    End If
                    TextX = 0
                    NextLeft = 0
                    LastPreTextHeight = PreTextSize.Height
                End If
                Dim WordPosition As New Point(TextX, Lines)

                Dim WordSize = TextRenderer.MeasureText("-" & Words(i) & "-", font, proposedSize, flags)
                WordSize.Width -= SlashWidth

                Dim pFrom = New Point(NextLeft, PreTextSize.Height - WordSize.Height)
                Dim YDif = 0
                If (flags And TextFormatFlags.VerticalCenter) = TextFormatFlags.VerticalCenter Then
                    YDif = (proposedSize.Height - textSize.Height) \ 2
                ElseIf (flags And TextFormatFlags.Bottom) = TextFormatFlags.Bottom Then
                    YDif = proposedSize.Height - textSize.Height
                End If
                If YDif > 0 Then
                    pFrom.Y += YDif
                End If

                If Words(i) <> "" Then
                    Dim WordRect = New Rectangle(pFrom, WordSize)
                    If (OnlyReturnRenderedWords AndAlso New Rectangle(New Point, proposedSize).IntersectsWith(WordRect)) _
                    OrElse OnlyReturnRenderedWords = False Then
                        Measure.Add(New WordBounds.WordBound() With {.Bounds = WordRect, .Word = Words(i), .WordPosition = WordPosition, .LetterIndex = LetterIndex})
                    Else
                        If OnlyReturnRenderedWords Then GoTo Finish
                    End If
                End If

                NextLeft += WordSize.Width + If(PerWord, SpaceWidth, 0)
                TextX += Len(Words(i)) + If(PerWord, 1, 0)
                LetterIndex += Len(Words(i)) + If(PerWord, 1, 0)
            Next

Finish:

            Dim AlignRight = (flags And TextFormatFlags.Right) = TextFormatFlags.Right
            Dim AlignCenter = (flags And TextFormatFlags.HorizontalCenter) = TextFormatFlags.HorizontalCenter
            If AlignCenter OrElse AlignRight Then
                Dim TextRowData = (From n In Measure Group n By Row = n.WordPosition.Y Into Group).ToList
                For Each row In TextRowData
                    Dim FarRight = row.Group.Max(Function(x As WordBounds.WordBound) x.Bounds.Right)
                    Dim xOffset As Integer
                    If AlignCenter Then
                        xOffset = ((proposedSize.Width - FarRight) \ 2)
                    ElseIf AlignRight Then
                        xOffset = ((proposedSize.Width - FarRight) - TextMargin)
                    End If
                    For Each word In row.Group
                        word.Bounds.X += xOffset
                    Next
                Next
            Else
                For Each word In Measure
                    word.Bounds.X += TextMargin
                Next
            End If
        End Function

    End Class

End Class
