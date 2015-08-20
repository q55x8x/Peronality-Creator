Imports System.Drawing.Drawing2D

Friend Class ctlLogo
    Inherits BufferedPanel

    Public Sub New()
        Me.ClientSize = New Size(288, 115)
    End Sub

    Dim mc_DrawBackground As Boolean = True
    Public Property DrawBackground() As Boolean
        Get
            Return mc_DrawBackground
        End Get
        Set(ByVal value As Boolean)
            mc_DrawBackground = value
            MyBase.Refresh()
        End Set
    End Property

    Private Sub bpLogo_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        If mc_DrawBackground Then
            Using b1 As New LinearGradientBrush(New Point(0, 0), New Point(0, MyBase.ClientSize.Height), Color.White, Color.Black)
                e.Graphics.FillRectangle(b1, New Rectangle(New Point(0, 0), MyBase.ClientSize))
            End Using
            Using b1 As New LinearGradientBrush(New Point(0, 0), New Point(0, MyBase.ClientSize.Height), DrawingFunctions.AlphaColor(Color.White, 128), DrawingFunctions.AlphaColor(Color.Black, 128))
                e.Graphics.FillRectangle(b1, New Rectangle(0, 0, MyBase.ClientSize.Width, CInt(MyBase.ClientSize.Height / 2)))
            End Using
            Using b1 As New LinearGradientBrush(New Point(0, 0), New Point(0, MyBase.ClientSize.Height), DrawingFunctions.BlendColor(Color.Black, Color.White, 64), Color.Black)
                e.Graphics.FillRectangle(b1, New Rectangle(0, CInt((MyBase.ClientSize.Height / 2) - 4), MyBase.ClientSize.Width, CInt((MyBase.ClientSize.Height / 2) + 4)))
            End Using
        End If

        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear

        e.Graphics.DrawImage(My.Resources.Icon, New Rectangle(8, CInt((MyBase.ClientSize.Height - 64) / 2), 64, 64))

        'left point after image is 64+(8*2) = 80




        Using p As GraphicsPath = DrawingFunctions.StringToPath(e.Graphics, "i00 Productions", New Font("Arial", 14, FontStyle.Bold, GraphicsUnit.Pixel))
            Dim m As New Matrix
            m.Translate(80, 4, MatrixOrder.Append)
            p.Transform(m)
            e.Graphics.FillPath(Brushes.DarkGray, p)

        End Using
        Using p As GraphicsPath = DrawingFunctions.StringToPath(e.Graphics, "Binding List", New Font("Arial", 40, FontStyle.Bold, GraphicsUnit.Pixel))
            Dim m As New Matrix
            m.Translate(80, 12, MatrixOrder.Append)
            p.Transform(m)
            e.Graphics.SetClip(New Rectangle(0, 0, MyBase.ClientSize.Width, CInt((MyBase.ClientSize.Height / 2) - 3)))
            e.Graphics.FillPath(New SolidBrush(DrawingFunctions.AlphaColor(Color.Black, 159)), p)
        End Using

    End Sub

End Class
