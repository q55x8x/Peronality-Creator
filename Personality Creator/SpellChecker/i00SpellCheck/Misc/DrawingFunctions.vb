'i00 Misc Drawing Functions
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

Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Public Class DrawingFunctions

#Region "i00 Logo"

    '26x26 is the origional logo's canvas size :)
    Public Shared ReadOnly Property OrigLogoCanvasSize() As Size
        Get
            Return New Size(26, 26)
        End Get
    End Property

    Private Shared Function LogoRatio(ByVal xy As Single, ByVal rect As RectangleF, Optional ByVal x As Boolean = True) As Single
        If x Then
            'x
            Return (xy * (rect.Width / OrigLogoCanvasSize.Width)) + rect.X
        Else
            'y
            Return (xy * (rect.Height / OrigLogoCanvasSize.Height)) + rect.Y
        End If
    End Function

    Public Shared Sub DrawLogo(ByVal g As Graphics, ByVal Brush As Brush, ByVal Rect As RectangleF)
        'Dim Rect As New Rectangle(0, 0, 26, 26)
        Using p As New Pen(Brush, (4 * (Rect.Width / OrigLogoCanvasSize.Width)))
            g.DrawLine(p, LogoRatio(13, Rect), LogoRatio(4, Rect, False), LogoRatio(13, Rect), LogoRatio(8, Rect, False))
            g.DrawLine(p, LogoRatio(13, Rect), LogoRatio(10, Rect, False), LogoRatio(13, Rect), LogoRatio(23.5, Rect, False))
        End Using
        Dim RectOrigCanvas As New Rectangle(New Point(0, 0), OrigLogoCanvasSize)
        Using p As New GraphicsPath
            p.AddArc(8, 0, 5, 5, 90, 180)
            p.AddArc(RectOrigCanvas, -90, 135)
            p.AddArc(New RectangleF(3.3, 5, 21, 21), 45, -135)
            p.CloseFigure()
            Using m As New Matrix
                m.Translate(Rect.X, Rect.Y)
                m.Scale(Rect.Width / RectOrigCanvas.Width, Rect.Height / RectOrigCanvas.Height)
                p.Transform(m)
            End Using
            g.FillPath(Brush, p)

            Using m As New Matrix
                m.RotateAt(180, New PointF(Rect.X + (Rect.Width / 2), Rect.Y + (Rect.Height / 2)))
                p.Transform(m)
            End Using
            g.FillPath(Brush, p)

        End Using

    End Sub

#End Region

#Region "Best Fit Rect"

    Public Enum BestFitStyle
        Zoom
        Stretch
    End Enum

    Public Shared Function GetBestFitRect(ByVal rectFrame As RectangleF, ByVal rect As RectangleF, Optional ByVal BestFitStyle As BestFitStyle = BestFitStyle.Zoom) As RectangleF
        Dim origImageRatio = rect.Width / rect.Height
        Dim canvasRatio = rectFrame.Width / rectFrame.Height
        Dim DrawRect As RectangleF
        If (origImageRatio > canvasRatio AndAlso BestFitStyle = BestFitStyle.Stretch) OrElse (origImageRatio < canvasRatio AndAlso BestFitStyle = BestFitStyle.Zoom) Then
            DrawRect = New RectangleF(rectFrame.Left, rectFrame.Top, rectFrame.Width, rectFrame.Width * (1 / origImageRatio))
        Else
            DrawRect = New RectangleF(rectFrame.Left, rectFrame.Top, rectFrame.Height * origImageRatio, rectFrame.Height)
        End If
        DrawRect.X += ((rectFrame.Width - DrawRect.Width) / 2)
        DrawRect.Y += ((rectFrame.Height - DrawRect.Height) / 2)
        Return DrawRect
    End Function

#End Region

#Region "Gray scale image"

    Private Shared Function draw_adjusted_image(ByVal img As Image, ByVal cm As ColorMatrix) As Boolean
        Try
            Dim bmp As New Bitmap(img) ' create a copy of the source image 
            Dim imgattr As New ImageAttributes()
            Dim rc As New Rectangle(0, 0, img.Width, img.Height)
            Using g As Graphics = Graphics.FromImage(img)
                imgattr.SetColorMatrix(cm)
                g.DrawImage(bmp, rc, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgattr)
            End Using
            Return True
        Catch
            Return False
        End Try

    End Function

    Public Shared Function Grayscale(ByVal img As Image) As Boolean
        Dim cm As New ColorMatrix(New Single()() _
                               {New Single() {0.299, 0.299, 0.299, 0, 0}, _
                                New Single() {0.587, 0.587, 0.587, 0, 0}, _
                                New Single() {0.114, 0.114, 0.114, 0, 0}, _
                                New Single() {0, 0, 0, 1, 0}, _
                                New Single() {0, 0, 0, 0, 1}})

        Return draw_adjusted_image(img, cm)

    End Function

#End Region

#Region "Alpha Image"

    Public Shared Sub AlphaImage(ByVal g As Graphics, ByVal b As Bitmap, ByVal Rect As Rectangle, Optional ByVal Alpha As Byte = 127)
        Dim cm As New System.Drawing.Imaging.ColorMatrix()
        cm.Matrix33 = CSng(Alpha / 255)
        Using ia As New System.Drawing.Imaging.ImageAttributes
            ia.SetColorMatrix(cm)
            g.DrawImage(b, Rect, 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, ia)
        End Using
    End Sub

#End Region

    Public Shared Sub DrawWave(ByVal g As Graphics, ByVal startPoint As Point, ByVal endPoint As Point, ByVal Color As Color)
        If endPoint.X - startPoint.X < 4 Then
            endPoint.X = startPoint.X + 4
        End If
        Dim points() As PointF = Nothing  'New List(Of PointF)
        For i = startPoint.X To endPoint.X Step 2
            If points Is Nothing Then
                ReDim Preserve points(0)
            Else
                ReDim Preserve points(UBound(points) + 1)
            End If
            If (i - startPoint.X) Mod 4 = 0 Then
                points(UBound(points)) = New PointF(i, endPoint.Y + 1)
                'points.Add(New PointF(i, startPoint.Y))
            Else
                points(UBound(points)) = New PointF(i, endPoint.Y - 1)
                'points.Add(New PointF(i, startPoint.Y + 2))
            End If
        Next
        Using p As New System.Drawing.Drawing2D.GraphicsPath
            p.AddCurve(points)
            Using pen As Pen = New Pen(Color)
                g.DrawPath(pen, p)
            End Using
        End Using
    End Sub

    Public Shared Sub DrawString(ByVal g As Graphics, ByVal s As String, ByVal font As System.Drawing.Font, ByVal brush As System.Drawing.Brush, ByVal x As Single, ByVal y As Single, Optional ByVal Ratio As Single = 1)
        Using gp = GetStringPath(s, font, x, y, Ratio, g)
            g.FillPath(brush, gp)
        End Using
    End Sub

    Public Shared Function GetStringPath(ByVal s As String, ByVal font As System.Drawing.Font, ByVal x As Single, ByVal y As Single, Optional ByVal Ratio As Single = 1, Optional ByVal g As Graphics = Nothing) As GraphicsPath
        Dim FontSize As SizeF
        If g Is Nothing Then
            Using b As New Bitmap(1, 1)
                Using g2 = Graphics.FromImage(b)
                    FontSize = g2.MeasureString(s, font, New PointF(0, 0), System.Drawing.StringFormat.GenericDefault)
                End Using
            End Using
        Else
            FontSize = g.MeasureString(s, font, New PointF(0, 0), System.Drawing.StringFormat.GenericDefault)
        End If
        GetStringPath = New GraphicsPath
        GetStringPath.AddString(s, font.FontFamily, font.Style, font.Size, New PointF(0, 0), System.Drawing.StringFormat.GenericDefault)
        Dim scale As Single = FontSize.Width / (GetStringPath.GetBounds.Right + GetStringPath.GetBounds.Left)
        Using m As New Matrix
            m.Translate(x, y, MatrixOrder.Append)
            scale = scale * Ratio
            m.Scale(scale, scale, MatrixOrder.Prepend)
            GetStringPath.Transform(m)
        End Using
    End Function

    Public Shared Sub DrawElipseText(ByVal g As Graphics, ByVal RequiredLocation As PointF, ByVal Text As String, ByVal font As Font, ByVal requiredWidth As Integer, ByVal fontColor As Color)
        Dim StringSize As SizeF = g.MeasureString(Text, font)
        If StringSize.Width > requiredWidth Then

            Dim ElipseWidth As Single = g.MeasureString("...", font).Width
            Dim TweenCurrentRectWithFont As New RectangleF(RequiredLocation.X, RequiredLocation.Y, StringSize.Width, StringSize.Height)

            Dim lgb As New LinearGradientBrush(TweenCurrentRectWithFont, Color.Magenta, Color.Magenta, LinearGradientMode.Horizontal)
            Dim InterpolationColors As New ColorBlend(3)
            Dim ElipseGradRatio As Single = ElipseWidth / StringSize.Width
            Dim ShowTextGradRatio As Single = requiredWidth / StringSize.Width
            Dim FadeStartRatio As Single = ShowTextGradRatio - (ElipseGradRatio * 2)
            If FadeStartRatio < 0 Then FadeStartRatio = 0
            InterpolationColors.Positions = New Single() {0, FadeStartRatio, ShowTextGradRatio, 1}
            InterpolationColors.Colors = New Color() {fontColor, AlphaColor(fontColor, 191), Color.Transparent, Color.Transparent}

            lgb.InterpolationColors = InterpolationColors

            DrawString(g, Text, font, lgb, RequiredLocation.X, RequiredLocation.Y)
            DrawString(g, "...", font, New SolidBrush(AlphaColor(fontColor, 191)), (RequiredLocation.X + requiredWidth) - ElipseWidth, RequiredLocation.Y)

        Else

            Dim lgb As New LinearGradientBrush(New RectangleF(RequiredLocation.X, RequiredLocation.Y, RequiredLocation.X + requiredWidth, 1), fontColor, AlphaColor(fontColor, 191), LinearGradientMode.Horizontal)

            DrawString(g, Text, font, lgb, RequiredLocation.X, RequiredLocation.Y)

        End If
    End Sub

    Public Shared Function AlphaColor(ByVal theColor As Color, Optional ByVal AlphaLevel As Byte = 255) As Color
        AlphaColor = Color.FromArgb(AlphaLevel, theColor.R, theColor.G, theColor.B)
    End Function

    Public Shared Function BlendColor(ByVal FromColor As Color, ByVal ToColor As Color, Optional ByVal alpha As Integer = 127) As Color
        BlendColor = Color.FromArgb(CInt(((FromColor.A * alpha) / 255) + ((ToColor.A * (255 - alpha)) / 255)), _
                                    CInt(((FromColor.R * alpha) / 255) + ((ToColor.R * (255 - alpha)) / 255)), _
                                    CInt(((FromColor.G * alpha) / 255) + ((ToColor.G * (255 - alpha)) / 255)), _
                                    CInt(((FromColor.B * alpha) / 255) + ((ToColor.B * (255 - alpha)) / 255)))
    End Function

    Public Shared Function StringToPath(ByVal g As Graphics, ByVal s As String, ByVal font As System.Drawing.Font) As GraphicsPath
        Dim FontSize As SizeF = g.MeasureString(s, font, New PointF(0, 0), System.Drawing.StringFormat.GenericDefault)
        Dim gp As New GraphicsPath
        gp.AddString(s, font.FontFamily, font.Style, font.Size, New PointF(0, 0), System.Drawing.StringFormat.GenericDefault)
        Dim scale As Single = FontSize.Width / (gp.GetBounds.Right + gp.GetBounds.Left)
        Return gp
    End Function

    Public Class Text

        Partial Public Class TextRendererMeasure

            Public Shared Function TextFlagsFrom(ByVal CellStyle As DataGridViewCellStyle) As TextFormatFlags

                Dim Flags As TextFormatFlags
                Flags = TextFormatFlags.NoPrefix

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

        Public Enum TextRenderMode
            Auto
            Windows
            i00
        End Enum

        'sets a new size for a given font
        Public Shared Function FontSetNewSize(ByVal theFont As Font, ByVal NewSize As Single) As Font
            FontSetNewSize = New Font(theFont.Name, NewSize, theFont.Style)
        End Function

        'allows you to specify width and/or height bounds for a given string and it will return the maximum 
        'font size to fit within that region
        Public Shared Function DetermineFontSizeForBounds(ByVal Text As String, ByVal theFont As Drawing.Font, Optional ByVal RequiredWidth As Single = 0, Optional ByVal RequiredHeight As Single = 0, Optional ByVal UseTextRenderer As Boolean = False) As Single
            Using b As New Bitmap(1, 1)
                Using g As Graphics = Graphics.FromImage(b)
                    If Text = "" Then
                        Return 8
                    End If

                    If RequiredWidth <> 0 Then
                        DetermineFontSizeForBounds = 8
                        If UseTextRenderer Then
                            Do While TextRenderer.MeasureText(Text, FontSetNewSize(theFont, DetermineFontSizeForBounds)).Width < RequiredWidth
                                DetermineFontSizeForBounds += 1
                            Loop
                        Else
                            Do While g.MeasureString(Text, FontSetNewSize(theFont, DetermineFontSizeForBounds)).Width < RequiredWidth
                                DetermineFontSizeForBounds += 1
                            Loop
                        End If
                        DetermineFontSizeForBounds = DetermineFontSizeForBounds - 1 'because we are one more than the size we want
                        If RequiredHeight = 0 Then
                            'just use the width
                            Return DetermineFontSizeForBounds
                        End If
                    End If

                    Dim FontSizeHeight As Single = 8
                    If RequiredHeight <> 0 Then
                        If UseTextRenderer Then
                            Do While TextRenderer.MeasureText(Text, FontSetNewSize(theFont, FontSizeHeight)).Height < RequiredHeight
                                FontSizeHeight += 1
                            Loop
                        Else
                            Do While g.MeasureString(Text, FontSetNewSize(theFont, FontSizeHeight)).Height < RequiredHeight
                                FontSizeHeight += 1
                            Loop
                        End If
                        FontSizeHeight -= 1 'because we are one more than the size we want
                        If RequiredWidth = 0 Then
                            'just use the height
                            Return FontSizeHeight
                        End If
                    End If

                    If RequiredWidth <> 0 AndAlso RequiredHeight <> 0 Then
                        'we have both a width and height so lets pick the smallest
                        If FontSizeHeight < DetermineFontSizeForBounds Then
                            Return FontSizeHeight
                        Else
                            Return DetermineFontSizeForBounds
                        End If
                    End If
                End Using
            End Using
        End Function

        Public Shared Sub DrawString(ByVal g As Graphics, ByVal s As String, ByVal font As System.Drawing.Font, ByVal brush As System.Drawing.Brush, ByVal x As Single, ByVal y As Single, Optional ByVal TextRenderMode As TextRenderMode = TextRenderMode.Auto)
            If Trim(s) = "" Then Exit Sub
            If (TextRenderMode = Text.TextRenderMode.Windows OrElse (TextRenderMode = Text.TextRenderMode.Auto AndAlso Screen.PrimaryScreen.BitsPerPixel < 24)) AndAlso TypeOf brush Is SolidBrush Then
                'also only works for solid brushes
                Dim ForeColor As Color = Color.FromKnownColor(KnownColor.ControlText)
                If TypeOf brush Is SolidBrush Then
                    ForeColor = CType(brush, SolidBrush).Color
                End If
                'this adds the transform location as the text renderer does not take this into consideration :S
                TextRenderer.DrawText(g, s, font, New Rectangle(CInt(x + g.Transform.OffsetX), CInt(y + g.Transform.OffsetY), Integer.MaxValue, Integer.MaxValue), ForeColor, TextFormatFlags.Default)
            Else
                Dim FontSize As SizeF = g.MeasureString(s, font, New PointF(0, 0), System.Drawing.StringFormat.GenericDefault)
                Using gp As New GraphicsPath

                    gp.AddString(s, font.FontFamily, font.Style, font.Size, New PointF(0, 0), System.Drawing.StringFormat.GenericDefault)
                    Dim scale As Single = FontSize.Width / (gp.GetBounds.Right + gp.GetBounds.Left)
                    Using m As New Matrix
                        m.Translate(x, y, MatrixOrder.Append)
                        m.Scale(scale, scale, MatrixOrder.Prepend)
                        gp.Transform(m)
                    End Using
                    Try
                        g.FillPath(brush, gp)
                    Catch ex As Exception
                        'Debug.Print("error modDrawing.DrawString")
                    End Try
                End Using
            End If
        End Sub

    End Class

End Class
