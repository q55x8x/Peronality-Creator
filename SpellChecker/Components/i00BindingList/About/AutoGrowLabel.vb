Imports System.ComponentModel
Imports System.Windows.Forms.Design
Imports System.Drawing.Design  'Must reference System.Design.Dll
Imports System.Globalization

<System.ComponentModel.DesignerCategory("")> _
Public Class AutoGrowLabel
    Inherits BufferedPanel
    Private mc_Text As String
    <System.ComponentModel.Browsable(True), Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(UITypeEditor))> _
    Public Overrides Property Text() As String
        Get
            Return mc_Text
        End Get
        Set(ByVal value As String)
            If mc_Text <> value Then
                mc_Text = value
                Repaint()
            End If
        End Set
    End Property

    Private Sub AutoGrowLabel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        PaintOn(e.Graphics)
    End Sub

    Public Sub PaintOn(ByVal g As Graphics)
        Dim TextSize As Size = TextRenderer.MeasureText(mc_Text, MyBase.Font, New Size(MyBase.Width - Padding.Left - Padding.Right, Integer.MaxValue), TextFormatFlags.WordBreak)
        Me.Height = TextSize.Height + Padding.Bottom + Padding.Top + (Me.Height - Me.ClientSize.Height)
        If g IsNot Nothing Then
            g.FillRectangle(New SolidBrush(Me.BackColor), New Rectangle(0, 0, Me.Width, Me.Height))
            TextRenderer.DrawText(g, mc_Text, MyBase.Font, New Rectangle(Padding.Left, Padding.Top, MyBase.Width - Padding.Left - Padding.Right, Integer.MaxValue), MyBase.ForeColor, TextFormatFlags.WordBreak)
        End If
    End Sub

    Public Sub Repaint()
        MyBase.RaisePaintEvent(Me, New PaintEventArgs(MyBase.CreateGraphics, New Rectangle(0, 0, MyBase.ClientSize.Width, MyBase.ClientSize.Height)))
    End Sub

    Private Sub AutoGrowTextBox_ReRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged, Me.FontChanged, Me.PaddingChanged
        Repaint()
    End Sub


    Protected Overrides Sub OnPaddingChanged(ByVal e As System.EventArgs)
        Repaint()
        MyBase.OnPaddingChanged(e)
    End Sub
End Class
