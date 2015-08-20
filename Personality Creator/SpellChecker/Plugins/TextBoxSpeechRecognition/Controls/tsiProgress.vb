<System.ComponentModel.DesignerCategory("")> _
Public Class tsiProgress
    Inherits ToolStripDropDownItem

    Public Overrides ReadOnly Property HasDropDownItems() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Sub New()
        MyBase.AutoSize = False
        MyBase.Height = 10
        MyBase.Width = 100
    End Sub

    Private Sub tsiProgress_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim rect As New Rectangle(0, 0, Me.Width - 1, Me.Height - 1)
        If System.Windows.Forms.ProgressBarRenderer.IsSupported Then
            System.Windows.Forms.ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rect)
        Else
            Using p As New Pen(Color.FromKnownColor(KnownColor.ControlDark))
                e.Graphics.DrawRectangle(p, rect)
            End Using
        End If

        Dim ScrollMargin As Integer = 2

        rect.X += ScrollMargin
        rect.Y += ScrollMargin
        rect.Width -= ScrollMargin * 2
        rect.Height -= ScrollMargin * 2

        rect = New Rectangle(rect.X, rect.Y, CInt(rect.Width * mc_Progress), rect.Height)

        If System.Windows.Forms.ProgressBarRenderer.IsSupported Then
            System.Windows.Forms.ProgressBarRenderer.DrawHorizontalChunks(e.Graphics, rect)
        Else
            Using sb As New SolidBrush(Color.FromKnownColor(KnownColor.Highlight))
                e.Graphics.FillRectangle(sb, rect)
            End Using
        End If

    End Sub

    Dim mc_Progress As Single
    Public Property Progress() As Single
        Get
            Return mc_Progress
        End Get
        Set(ByVal value As Single)
            If mc_Progress <> value Then
                mc_Progress = value
                Me.Invalidate()
            End If
        End Set
    End Property

End Class
