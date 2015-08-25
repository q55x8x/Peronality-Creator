'i00 .Net Split Button
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

<System.ComponentModel.DesignerCategory("")> _
Public Class SplitButton
    Inherits Button

    Private Sub SplitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click

    End Sub

    Protected Overridable Sub OnClickDropdown(ByVal e As System.EventArgs)
        RaiseEvent ClickDropdown(Me, e)
    End Sub

    Public Event ClickDropdown(ByVal sender As Object, ByVal e As EventArgs)

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        Dim location = Cursor.Position
        Dim ButtonLocation = Me.PointToScreen(Point.Empty)
        location.X -= ButtonLocation.X
        location.Y -= ButtonLocation.Y
        If location.X >= Me.ClientRectangle.Width - Me.Padding.Right Then
            'clicked the drop down
            OnClickDropdown(e)
        Else
            MyBase.OnClick(e)
        End If
    End Sub

    Private Sub SplitButton_PaddingChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PaddingChanged
        If Me.Padding.Right < 16 Then
            Me.Padding = New Padding(Me.Padding.Left, Me.Padding.Top, 16, Me.Padding.Bottom)
        End If
    End Sub

    Private Sub SplitButton_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Using tsi As New ToolStripButton
            tsi.Enabled = Me.Enabled
            tsi.Height = Me.ClientSize.Height
            tsi.Width = Me.Padding.Right
            ToolStripRenderer.DrawArrow(New ToolStripArrowRenderEventArgs(e.Graphics, tsi, New Rectangle(Me.ClientRectangle.Width - Me.Padding.Right, 0, Me.Padding.Right, Me.ClientRectangle.Height), Me.ForeColor, ArrowDirection.Down))
        End Using
        Using tsi As New ToolStripSeparator
            tsi.Height = Me.ClientSize.Height
            e.Graphics.TranslateTransform(Me.ClientSize.Width - Me.Padding.Right - CInt(tsi.Width / 2), 0)
            ToolStripRenderer.DrawSeparator(New ToolStripSeparatorRenderEventArgs(e.Graphics, tsi, True))
            e.Graphics.ResetTransform()
        End Using
    End Sub

    Dim mc_ToolStripRenderer As ToolStripRenderer
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
    Public Property ToolStripRenderer() As ToolStripRenderer
        Get
            If mc_ToolStripRenderer Is Nothing Then
                mc_ToolStripRenderer = New System.Windows.Forms.ToolStripProfessionalRenderer
            End If
            Return mc_ToolStripRenderer
        End Get
        Set(ByVal value As ToolStripRenderer)
            mc_ToolStripRenderer = value
            Me.Invalidate()
        End Set
    End Property

    Public Sub New()
        Me.Padding = New Padding(0, 0, 16, 0)
    End Sub

End Class
