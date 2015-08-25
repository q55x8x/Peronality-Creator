'i00 PersistentToolStripMenuItem
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
Public Class PersistentToolStripMenuItem
    Inherits ToolStripMenuItem
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        'draw bg
        Me.Parent.Renderer.DrawMenuItemBackground(New ToolStripItemRenderEventArgs(e.Graphics, Me))


        'draw check
        If Me.Checked Then
            Dim CheckRect = e.ClipRectangle
            CheckRect.Width = Me.Parent.Padding.Left
            CheckRect.Y += 1
            CheckRect.X += 3
            CheckRect.Width = CheckRect.Height - 3
            CheckRect.Height -= 3

            'should use something like thiis to draw the check ... but it doesn't work properly.. :(
            'If Checked Then
            '    Me.Parent.Renderer.DrawItemCheck(New ToolStripItemImageRenderEventArgs(e.Graphics, Me, Me.Image, CheckRect))
            'Else
            '    Me.Parent.Renderer.DrawItemImage(New ToolStripItemImageRenderEventArgs(e.Graphics, Me, Me.Image, CheckRect))
            'End If        If Checked Then

            Using sb As New SolidBrush(DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.MenuHighlight), 63))
                e.Graphics.FillRectangle(sb, CheckRect)
            End Using
            Using p As New Pen(Color.FromKnownColor(KnownColor.MenuHighlight))
                e.Graphics.DrawRectangle(p, CheckRect)
            End Using

            'we know the tick is 6x5... so position it in the center
            Dim yOffset = CInt(Int(CheckRect.Top + ((CheckRect.Height - 5) / 2))) - 1
            Dim xOffset As Integer = CInt(Int(CheckRect.Left + ((CheckRect.Width - 6) / 2))) + 1

            For yOffset = yOffset To yOffset + 1
                Using p As New Pen(Color.FromKnownColor(KnownColor.MenuText))
                    e.Graphics.DrawLine(p, xOffset, 2 + yOffset, 2 + xOffset, 4 + yOffset)
                    e.Graphics.DrawLine(p, 2 + xOffset, 4 + yOffset, 6 + xOffset, yOffset)
                End Using
            Next
        End If

        'draw text
        Dim TextFormatFlags As TextFormatFlags
        Select Case Me.Alignment
            Case ToolStripItemAlignment.Left
                TextFormatFlags = TextFormatFlags Or Windows.Forms.TextFormatFlags.VerticalCenter
                TextFormatFlags = TextFormatFlags Or Windows.Forms.TextFormatFlags.Left
            Case ToolStripItemAlignment.Right
                TextFormatFlags = TextFormatFlags Or Windows.Forms.TextFormatFlags.VerticalCenter
                TextFormatFlags = TextFormatFlags Or Windows.Forms.TextFormatFlags.Right
        End Select

        Dim TextRect = e.ClipRectangle
        TextRect.X += Me.Parent.Padding.Left
        TextRect.Width -= Me.Parent.Padding.Left
        TextRect.Width -= Me.Parent.Padding.Right

        TextRenderer.DrawText(e.Graphics, Me.Text, Me.Font, TextRect, Me.ForeColor, TextFormatFlags)
    End Sub

    Public Persistent As Boolean = True

    Public Overrides ReadOnly Property HasDropDownItems() As Boolean
        Get
            Return Persistent
        End Get
    End Property
End Class
