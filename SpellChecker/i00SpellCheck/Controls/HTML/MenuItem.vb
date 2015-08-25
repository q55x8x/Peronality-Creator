'i00 .Net HTML Menu Item
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
Public Class HTMLMenuItem
    Inherits ToolStripDropDownItem

    Dim mc_HTMLText As String
    Public Property HTMLText() As String
        Get
            Return mc_HTMLText
        End Get
        Set(ByVal value As String)
            If value = "" Then value = " "
            Dim ToBeWidth As Integer
            Dim ToBeHeight As Integer

            mc_HTMLText = value
            Me.AutoSize = False
            Dim theSize = GetHTMLInfo.Size
            If theSize.Width > 400 Then
                ToBeWidth = 400
                theSize = GetHTMLInfo(400).Size
            Else
                ToBeWidth = CInt(theSize.Width) + 1
            End If
            If theSize.Height > SmallHeight + SmallHeightTolerance Then
                ToBeHeight = SmallHeight
                BigHeight = CInt(theSize.Height) + 1
            Else
                ToBeHeight = CInt(theSize.Height) + 1
                BigHeight = ToBeHeight
            End If

            If Me.Width <> ToBeWidth Then
                Me.Width = ToBeWidth
            End If
            If Me.Height <> ToBeHeight Then
                Me.Height = ToBeHeight
            End If

        End Set
    End Property
    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = ""
        End Set
    End Property

    Public Overrides ReadOnly Property HasDropDownItems() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Sub New(ByVal HTMLText As String)
        Me.HTMLText = HTMLText
    End Sub

    Const SmallHeightTolerance As Integer = 16
    Public SmallHeight As Integer = 100
    Dim BigHeight As Integer

    <System.ComponentModel.Browsable(False)> _
    Overrides ReadOnly Property CanSelect() As Boolean
        Get
            Return BigHeight > SmallHeight AndAlso Me.Height <> BigHeight
        End Get
    End Property

    Private Sub tsiDefinition_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click
        Expand()
    End Sub

    Public Sub Expand()
        If CanSelect Then
            'means we have too much stuff
            If Me.Height = SmallHeight Then
                Me.Height = BigHeight
            Else
                'make it grow back?
                'qwertyuiop - put on timer to prevent close if after shrink cursor location is off the menu?
                'Me.Height = SmallHeight
            End If
        End If
    End Sub

    Private Sub tsiDefinition_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDownOpening
        'pressed enter :)
        Expand()
    End Sub

    Dim MouseOver As Boolean
    Private Sub tsiDefinition_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
        If MouseOver = False Then
            MouseOver = True
            Me.Invalidate()
        End If
    End Sub

    Private Sub tsiDefinition_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        If MouseOver = True Then
            MouseOver = False
            Me.Invalidate()
        End If
    End Sub

    Public Function GetHTMLInfo(Optional ByVal MaxWidth As Single = -1) As HTMLParser.PaintHTMLReturn
        Dim status As New HTMLParser.Status
        status.Font = New HTMLParser.STRFont(System.Drawing.SystemFonts.MenuFont)
        status.Brush = New HTMLParser.STRBrush(Color.FromKnownColor(KnownColor.MenuText))

        Return HTMLParser.PaintHTML(HTMLText, , MaxWidth, status)
    End Function

    Private Sub tsiDefinition_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim status As New HTMLParser.Status
        status.Font = New HTMLParser.STRFont(System.Drawing.SystemFonts.MenuFont)
        status.Brush = New HTMLParser.STRBrush(Color.FromKnownColor(KnownColor.MenuText))

        If CanSelect Then
            'draw the expander ...checked in the CanSelect :)
            Using tsi As New ToolStripMenuItem 'ToolStripButton
                tsi.AutoSize = False
                tsi.Width = Me.Width
                tsi.Height = CInt(tsi.Height / 2)
                tsi.Select()
                Dim ExpandBarRect As New Rectangle(0, Me.Height - tsi.Height, tsi.Width, tsi.Height)
                Using r As New Region
                    r.Exclude(ExpandBarRect)
                    e.Graphics.Clip = r
                    HTMLParser.PaintHTML(HTMLText, e.Graphics, MyBase.Width, status)
                    e.Graphics.ResetClip()
                End Using

                Using b As New Bitmap(tsi.Width, tsi.Height)
                    Using g = Graphics.FromImage(b)
                        Me.Parent.Renderer.DrawMenuItemBackground(New ToolStripItemRenderEventArgs(g, tsi))

                        Using tsmi As New ToolStripMenuItem
                            tsmi.Width = tsi.Width
                            tsmi.Height = tsi.Height
                            tsmi.AutoSize = False
                            Me.Parent.Renderer.DrawArrow(New ToolStripArrowRenderEventArgs(g, tsmi, New Rectangle(0, 0, tsi.Width, tsi.Height), Color.FromKnownColor(KnownColor.MenuText), ArrowDirection.Down))
                        End Using
                        If MouseOver OrElse Me.Selected Then
                            e.Graphics.DrawImage(b, ExpandBarRect)
                        Else
                            'DrawingFunctions.Grayscale(b)
                            DrawingFunctions.AlphaImage(e.Graphics, b, ExpandBarRect)
                        End If
                    End Using
                End Using
            End Using
        Else
            'just draw
            HTMLParser.PaintHTML(HTMLText, e.Graphics, MyBase.Width, status)
        End If

    End Sub
End Class