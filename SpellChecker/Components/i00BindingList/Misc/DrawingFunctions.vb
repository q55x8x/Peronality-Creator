'i00 DrawingFunctions
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

    Public Enum BestFitStyle
        Zoom = 1
        Stretch = 2
        DoNotAllowEnlarge = 4
    End Enum

    Public Shared Function GetBestFitRect(ByVal rectFrame As RectangleF, ByVal rect As RectangleF, Optional ByVal BestFitStyle As BestFitStyle = BestFitStyle.Zoom) As RectangleF
        Dim origImageRatio = rect.Width / rect.Height
        Dim canvasRatio = rectFrame.Width / rectFrame.Height
        Dim DrawRect As RectangleF
        If (BestFitStyle And BestFitStyle.DoNotAllowEnlarge) = DrawingFunctions.BestFitStyle.DoNotAllowEnlarge Then
            If rectFrame.Height >= rect.Height Then
                rectFrame.Y += (rectFrame.Height - rect.Height) / 2
                rectFrame.Height = rect.Height
            End If
            If rectFrame.Width >= rect.Width Then
                rectFrame.X += (rectFrame.Width - rect.Width) / 2
                rectFrame.Width = rect.Width
            End If
        End If
        If (origImageRatio > canvasRatio AndAlso (BestFitStyle And BestFitStyle.Stretch) = DrawingFunctions.BestFitStyle.Stretch) OrElse (origImageRatio < canvasRatio AndAlso (BestFitStyle And BestFitStyle.Zoom) = DrawingFunctions.BestFitStyle.Zoom) Then
            DrawRect = New RectangleF(rectFrame.Left, rectFrame.Top, rectFrame.Width, rectFrame.Width * (1 / origImageRatio))
        Else
            DrawRect = New RectangleF(rectFrame.Left, rectFrame.Top, rectFrame.Height * origImageRatio, rectFrame.Height)
        End If
        DrawRect.X += ((rectFrame.Width - DrawRect.Width) / 2)
        DrawRect.Y += ((rectFrame.Height - DrawRect.Height) / 2)
        Return DrawRect
    End Function

    Public Shared Sub DrawImageBestFit(ByVal g As Graphics, ByVal rect As RectangleF, ByVal image As Image, Optional ByVal BestFitStyle As BestFitStyle = BestFitStyle.Zoom)
        Dim oldRegion = g.Clip.Clone
        g.SetClip(rect, CombineMode.Intersect)
        Dim DrawRect = GetBestFitRect(rect, New RectangleF(0, 0, image.Width, image.Height), BestFitStyle)
        g.DrawImage(image, DrawRect)
        g.Clip = oldRegion
    End Sub

    Public Shared Function AlphaColor(ByVal theColor As Color, Optional ByVal AlphaLevel As Byte = 255) As Color
        AlphaColor = Color.FromArgb(AlphaLevel, theColor)
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
