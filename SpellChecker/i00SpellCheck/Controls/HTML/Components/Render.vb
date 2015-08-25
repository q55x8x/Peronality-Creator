'©i00 Productions All rights reserved
'This article is derived from http://www.codeproject.com/Articles/33842/HTMLLabel-An-HTML-Label-for-the-NET-CF
'----------------------------------------------------------------------------------------------------
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Partial Class HTMLParser

    Public Class PaintHTMLReturn
        Public Size As SizeF
        Friend Lines As New List(Of TextLine)()
        Friend HTMLElements As IList(Of Element)
        Public Function GetHTMLText() As String
            Dim sb As New System.Text.StringBuilder
            Dim iElement As Integer = 0
            For Each line In Lines
                Do Until iElement > line.LastElement
                    If HTMLElements(iElement).HTML IsNot Nothing AndAlso HTMLElements(iElement).HTML.Type = PartType.Text Then
                        sb.Append(HTMLElements(iElement).HTML.Value & " ")
                    End If
                    iElement += 1
                Loop
                If line IsNot Lines.Last Then sb.Append(vbCrLf)
            Next
            Return sb.ToString
        End Function
    End Class

    Public Shared Function PaintHTML(ByVal HTML As String, Optional ByVal g As Graphics = Nothing, Optional ByVal MaxWidth As Single = -1, Optional ByVal status As Status = Nothing) As PaintHTMLReturn
        HTML = Replace(HTML, vbCrLf, "<br>")
        Dim _textLines As New List(Of TextLine)()
        Dim currWdth As Single = 0, currHgth As Single = 0
        If status Is Nothing Then
            status = New Status()
            status.Font = New STRFont(System.Drawing.SystemFonts.DefaultFont)
            status.Brush = New STRBrush(Color.FromKnownColor(KnownColor.ControlText))
        End If
        Dim font As Font = status.Font.GetRealFont()
        Dim values As IList(Of Element) = Nothing
        Dim _totalHeight As Single = 0
        Dim _totalWidth As Single = 0

        Dim b As Bitmap = Nothing
        If g Is Nothing Then
            b = New Bitmap(1, 1)
            g = Graphics.FromImage(b)
        End If
        Try
            If MaxWidth = -1 Then
                status.WordWrap = False
            End If

            Dim _HTMLElements As New Elements
            _HTMLElements.Parse(HTML, status)


            values = _HTMLElements.Value

            Dim newLine As Boolean = False
            For i As Integer = 1 To values.Count - 1
                If values(i).Type = ElementType.Status Then
                    If values(i).Status.Image IsNot Nothing AndAlso values(i).Status.Image.Image IsNot Nothing Then
                        If values(i).Status.Image.Size.IsEmpty = False Then
                            'use this size
                            values(i).Size = values(i).Status.Image.Size
                        Else
                            values(i).Size = values(i).Status.Image.Image.Size
                        End If
                        GoTo SizeElement
                    Else
                        newLine = (status.Alignment <> values(i).Status.Alignment) OrElse (status.WordWrap <> values(i).Status.WordWrap)
                        status = values(i).Status
                        font = status.Font.GetRealFont()
                        newLine = newLine Or status.NewLine

                        If newLine Then
                            newLine = False
                            _textLines.Add(New TextLine(currWdth, currHgth, i - 1))
                            _totalHeight += currHgth
                            currWdth = 0
                            currHgth = 0
                        End If
                    End If
                ElseIf values(i).Type = ElementType.HTML Then
                    If values(i).HTML.Type = HTMLParser.PartType.Text Then
                        values(i).Size = g.MeasureString(System.Web.HttpUtility.HtmlDecode(values(i).HTML.Value), font)
                    Else
                        If (values(i).Size.Width < 0) OrElse (values(i).Size.Height < 0) Then
                            Dim txt As String = values(i).HTML.Value
                            If txt = "" Then
                                txt = ""
                            End If
                            Dim size As SizeF = g.MeasureString(System.Web.HttpUtility.HtmlDecode(txt), font)
                            If values(i).Size.Width > 0 Then
                                size.Width = values(i).Size.Width
                            End If
                            If values(i).Size.Height > 0 Then
                                size.Height = values(i).Size.Height
                            End If
                            values(i).Size = size
                        End If
                    End If
SizeElement:
                    If i = 0 Then
                        currWdth = values(i).Size.Width
                        currHgth = values(i).Size.Height
                    Else

                        If ((status.WordWrap) AndAlso (currWdth + values(i).Size.Width >= MaxWidth)) Then
                            newLine = False
                            _textLines.Add(New TextLine(currWdth, currHgth, i - 1))
                            _totalHeight += currHgth
                            currWdth = values(i).Size.Width
                            If currWdth > _totalWidth Then _totalWidth = currWdth
                            currHgth = values(i).Size.Height
                        Else
                            currWdth += values(i).Size.Width
                            If currWdth > _totalWidth Then _totalWidth = currWdth
                            currHgth = Math.Max(currHgth, values(i).Size.Height)
                        End If
                    End If
                End If
            Next
            _textLines.Add(New TextLine(currWdth, currHgth, values.Count - 1))
            _totalHeight += currHgth

        Catch ex As Exception
        Finally
            If b IsNot Nothing Then
                'we were using a graphics buffer
                g.Dispose()
                b.Dispose()
            End If
        End Try

        PaintHTML = New PaintHTMLReturn
        PaintHTML.Lines = _textLines
        PaintHTML.Size = New SizeF(_totalWidth, _totalHeight)
        PaintHTML.HTMLElements = values

        If b IsNot Nothing Then
            'don't paint
            Exit Function
        End If

        If values Is Nothing Then Exit Function


        Dim brush As Brush = status.Brush.GetRealBrush()
        font = status.Font.GetRealFont()

        Dim currElement As Integer = 1
        Dim left As Single = 0, top As Single = 0

        For Each line As TextLine In _textLines
            If values(currElement).Type = ElementType.Status Then
                status = values(currElement).Status
                brush = status.Brush.GetRealBrush()
                font = status.Font.GetRealFont()
            End If

            Select Case status.Alignment
                Case ContentAlignment.TopCenter
                    left = (MaxWidth - line.Width) / 2
                    Exit Select
                Case ContentAlignment.TopRight
                    left = MaxWidth - line.Width
                    Exit Select
                Case Else
                    left = 0
                    Exit Select
            End Select

            While currElement <= line.LastElement
                If values(currElement).Type = ElementType.Status Then
                    If values(currElement).Status.Image IsNot Nothing AndAlso values(currElement).Status.Image.Image IsNot Nothing Then
                        Dim irect As New Rectangle(CInt(Math.Truncate(left)), CInt(Math.Truncate(top + line.Height - values(currElement).Size.Height)), CInt(Math.Truncate(values(currElement).Size.Width)), CInt(Math.Truncate(values(currElement).Size.Height)))
                        values(currElement).DisplayedRect = irect
                        g.DrawImage(values(currElement).Status.Image.Image, values(currElement).DisplayedRect)
                        left += values(currElement).Size.Width
                    Else
                        status = values(currElement).Status
                        brush = status.Brush.GetRealBrush()
                        font = status.Font.GetRealFont()
                    End If
                End If

                If values(currElement).Type = ElementType.HTML Then
                    If values(currElement).HTML.Type = HTMLParser.PartType.Text Then
                        g.DrawString(System.Web.HttpUtility.HtmlDecode(values(currElement).HTML.Value), font, brush, left, top + line.Height - values(currElement).Size.Height)
                    End If

                    left += values(currElement).Size.Width
                End If
                currElement += 1
            End While
            top += line.Height
        Next
    End Function

End Class
