Public Class DrawingFunctions

    Public Shared Sub OutputLineBitmap(ByVal page As System.Web.UI.Page)
        page.Response.Clear()
        page.Response.ContentType = "image/png"

        Using b As New System.Drawing.Bitmap(4, 3)
            Using g = System.Drawing.Graphics.FromImage(b)
                g.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality
                i00SpellCheck.DrawingFunctions.DrawWave(g, New System.Drawing.Point(0, 0), New System.Drawing.Point(4, 1), Drawing.Color.Red)
                Using MemStream As New IO.MemoryStream()
                    b.Save(MemStream, System.Drawing.Imaging.ImageFormat.Png)
                    MemStream.WriteTo(page.Response.OutputStream)
                End Using
            End Using
        End Using

        page.Response.End()
    End Sub

End Class
