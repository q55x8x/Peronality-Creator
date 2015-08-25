'i00 .Net ToolStrip Color Picker
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

<System.ComponentModel.DefaultEvent("ColorChanged")> _
<System.Windows.Forms.Design.ToolStripItemDesignerAvailability(Windows.Forms.Design.ToolStripItemDesignerAvailability.All)> _
<System.ComponentModel.DesignerCategory("")> _
Public Class tsiColorPicker
    Inherits ToolStripDropDownItem

    Dim OrigWidth As Integer
    Dim OrigHeight As Integer

    Public Sub New()
        Using tsiSizeCheck As New ToolStripMenuItem
            OrigWidth = tsiSizeCheck.Width
            OrigHeight = tsiSizeCheck.Height
        End Using
        Resize()
        MyBase.AutoToolTip = False
        MyBase.AutoSize = False
    End Sub

    Public Sub Resize()
        MyBase.Size = MySize
    End Sub


#Region "Properties"

#Region "Custom"

    Dim mc_Persistent As Boolean = False
    <System.ComponentModel.DefaultValueAttribute(False)> _
    Public Property Persistent() As Boolean
        Get
            Return mc_Persistent
        End Get
        Set(ByVal value As Boolean)
            mc_Persistent = value
        End Set
    End Property

    Dim mc_SelectedColor As Color
    Public Property SelectedColor() As Color
        Get
            Return mc_SelectedColor
        End Get
        Set(ByVal value As Color)
            mc_SelectedColor = value
        End Set
    End Property


    Dim mc_Colors As List(Of Color)
    Public Property Colors() As List(Of Color)
        Get
            If mc_Colors Is Nothing Then
                FillWithDefaultColors()
            End If
            Return mc_Colors
        End Get
        Set(ByVal value As List(Of Color))
            mc_Colors = value
            'should really use a watched list and call the resize after item/s gets added / removed
            Resize()
        End Set
    End Property

    Dim mc_ShowColorToolTip As Boolean = True
    <System.ComponentModel.DefaultValueAttribute(True)> _
    Public Property ShowColorToolTip() As Boolean
        Get
            Return mc_ShowColorToolTip
        End Get
        Set(ByVal value As Boolean)
            mc_ShowColorToolTip = value
        End Set
    End Property

    Dim mc_BlocksAcross As Integer = 8
    <System.ComponentModel.DefaultValueAttribute(8)> _
    Public Property BlocksAcross() As Integer
        Get
            Return mc_BlocksAcross
        End Get
        Set(ByVal value As Integer)
            mc_BlocksAcross = value
            Resize()
        End Set
    End Property

#End Region

#Region "Private Read Only Properties"

    Private ReadOnly Property BlocksHigh() As Integer
        Get
            Return ((Colors.Count - 1) \ BlocksAcross) + 1
        End Get
    End Property

    Private ReadOnly Property TextSize() As SizeF
        Get
            If Text <> "" AndAlso Me.Parent IsNot Nothing Then
                TextSize = Me.Parent.CreateGraphics.MeasureString(Text, MyBase.Font)
            End If
        End Get
    End Property

    Private ReadOnly Property MySize() As Size
        Get
            Return New Size(CInt((OrigHeight * mc_BlocksAcross) + TextSize.Width), OrigHeight * BlocksHigh)
        End Get
    End Property

#End Region

#Region "Overridden Properties"

#Region "Hidden Properties"

    <System.ComponentModel.Browsable(False)> _
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property TextAlign() As System.Drawing.ContentAlignment
        Get
            Return MyBase.TextAlign
        End Get
        Set(ByVal value As System.Drawing.ContentAlignment)
            MyBase.TextAlign = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property TextDirection() As System.Windows.Forms.ToolStripTextDirection
        Get
            Return MyBase.TextDirection
        End Get
        Set(ByVal value As System.Windows.Forms.ToolStripTextDirection)
            MyBase.TextDirection = value
        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property Size() As System.Drawing.Size
        Get
            Return MySize
        End Get
        Set(ByVal value As System.Drawing.Size)

        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)> _
    Public Overrides Property Image() As System.Drawing.Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As System.Drawing.Image)

        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Public Overrides Property DisplayStyle() As System.Windows.Forms.ToolStripItemDisplayStyle
        Get
            Return ToolStripItemDisplayStyle.None
        End Get
        Set(ByVal value As System.Windows.Forms.ToolStripItemDisplayStyle)

        End Set
    End Property

    <System.ComponentModel.Browsable(False)> _
    Overrides ReadOnly Property CanSelect() As Boolean
        Get
            Return False
        End Get
    End Property

#End Region

    Public Overrides ReadOnly Property HasDropDownItems() As Boolean
        Get
            Return mc_Persistent
        End Get
    End Property

    Public Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Resize()
        End Set
    End Property

#End Region

#End Region

    Public Shared DefaultColors() As Color = New Color() {Color.Black, Color.FromArgb(153, 51, 0), Color.FromArgb(51, 51, 0), Color.FromArgb(0, 51, 0), Color.FromArgb(0, 51, 102), Color.Navy, Color.FromArgb(51, 51, 153), Color.FromArgb(51, 51, 51), _
                                             Color.Maroon, Color.FromArgb(255, 102, 0), Color.Olive, Color.Green, Color.Teal, Color.Blue, Color.FromArgb(102, 102, 153), Color.Gray, _
                                             Color.Red, Color.FromArgb(255, 153, 0), Color.FromArgb(153, 204, 0), Color.FromArgb(51, 153, 102), Color.FromArgb(51, 204, 204), Color.FromArgb(51, 102, 255), Color.Purple, Color.FromArgb(153, 153, 153), _
                                             Color.Fuchsia, Color.FromArgb(255, 204, 0), Color.Yellow, Color.Lime, Color.Aqua, Color.FromArgb(0, 204, 255), Color.FromArgb(153, 51, 102), Color.Silver, _
                                             Color.FromArgb(255, 153, 204), Color.FromArgb(255, 204, 153), Color.FromArgb(255, 255, 153), Color.FromArgb(204, 255, 204), Color.FromArgb(204, 255, 255), Color.FromArgb(153, 204, 255), Color.FromArgb(204, 153, 255), Color.White}

    Public Sub FillWithDefaultColors()
        mc_Colors = New List(Of Color)
        mc_Colors.AddRange(DefaultColors)
    End Sub

    Dim Tooltip As New ToolTip

    Private Sub tsiColorPickerItem_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Tooltip.Dispose()
    End Sub

    Private Sub tsiColorPickerItem_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
        Tooltip.Hide(Me.Parent)
        If PaintHoverColor <> -1 Then
            RaiseEvent HoverOverColor(Me, New ColorEventArgs)
            PaintHoverColor = -1
            Me.Invalidate()
        End If
    End Sub

    Public Class ColorEventArgs
        Inherits EventArgs
        Public Color As Color
    End Class

    Public Event HoverOverColor(ByVal sender As Object, ByVal e As ColorEventArgs)

    Private Sub tsiColorPickerItem_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Dim LastColorNo As Integer = PaintHoverColor
        PaintHoverColor = GetColorIndexAtLocation(e.Location)
        If LastColorNo <> PaintHoverColor Then
            'color has changed
            If PaintHoverColor = -1 Then
                RaiseEvent HoverOverColor(Me, New ColorEventArgs)
                Tooltip.Hide(Me.Parent)
            Else
                RaiseEvent HoverOverColor(Me, New ColorEventArgs With {.Color = Colors(PaintHoverColor)})
                If ShowColorToolTip Then
                    Dim TipLocation = GetColorRectFromIndex(PaintHoverColor).Location
                    Dim ts = TextSize
                    TipLocation.X += MyBase.Bounds.Left + CInt(ts.Width)
                    TipLocation.Y += MyBase.Bounds.Top + Cursors.Arrow.Size.Height
                    Dim NiceColor As Color = GetClosestKnownColor(Colors(PaintHoverColor))
                    If NiceColor.IsEmpty = False Then
                        Dim ColorName As String = NiceColor.Name
StartOver:
                        Dim Match = System.Text.RegularExpressions.Regex.Match(ColorName, "(?<!\s|^)[A-Z]")
                        If Match.Success Then
                            'do this letter
                            ColorName = Left(ColorName, Match.Index) & " " & Mid(ColorName, Match.Index + 1)
                            GoTo StartOver
                        End If
                        Tooltip.Show(ColorName, Me.Parent, TipLocation) 'New Point(Me.Bounds.Right, Me.Bounds.Y))
                    End If
                End If
            End If
            Me.Invalidate()
        End If
    End Sub

#Region "Cloest Color Matching"

    Public Shared Function GetClosestKnownColor(ByVal Color As Color) As Color
        'find the closest color...
        If Color.IsKnownColor Then
            Return Color
        Else
            Static ColorMatches() As Drawing.Color = (From xItem In (From xItem In [Enum].GetNames(GetType(KnownColor)) Select Color.FromKnownColor(DirectCast([Enum].Parse(GetType(KnownColor), xItem), KnownColor))) Where xItem.IsSystemColor = False).ToArray
            Dim ColorMatchesWithCloseness = (From xItem In ColorMatches Where (ColorCloseness(xItem, Color) < 25 OrElse IsRightColorTone(xItem, Color)) Select New With {.Color = xItem, .Closeness = ColorCloseness(xItem, Color)}).ToArray
            ColorMatchesWithCloseness = (From xItem In ColorMatchesWithCloseness Order By xItem.Closeness Ascending).ToArray
            If ColorMatchesWithCloseness.Count > 0 Then
                Return ColorMatchesWithCloseness.FirstOrDefault.Color
            Else
                Return Color.Empty
            End If
        End If
    End Function

    Private Class ObjectHolder
        Public theObject As Object
        Public Sub New(ByVal theObject As Object)
            Me.theObject = theObject
        End Sub
    End Class

    Private Shared Function ColorCloseness(ByVal Color1 As Color, ByVal Color2 As Color) As Integer
        Return Math.Abs(CInt(Color1.A) - CInt(Color2.A)) + Math.Abs(CInt(Color1.R) - CInt(Color2.R)) + Math.Abs(CInt(Color1.G) - CInt(Color2.G)) + Math.Abs(CInt(Color1.B) - CInt(Color2.B))
    End Function

    Private Shared Function IsRightColorTone(ByVal Color1 As Color, ByVal Color2 As Color) As Boolean
        Dim arrColor1 = New ObjectHolder() {New ObjectHolder(Color1.R), New ObjectHolder(Color1.G), New ObjectHolder(Color1.B)}
        Dim arrColor2 = New ObjectHolder() {New ObjectHolder(Color2.R), New ObjectHolder(Color2.G), New ObjectHolder(Color2.B)}
        'find the majors
        Dim MaxColors1 = Join((From xItem In arrColor1 Where Val(xItem.theObject) = arrColor1.Max(Function(b As ObjectHolder) Val(b.theObject)) Select CStr(Array.IndexOf(arrColor1, xItem))).ToArray, ";")
        Dim MaxColors2 = Join((From xItem In arrColor2 Where Val(xItem.theObject) = arrColor2.Max(Function(b As ObjectHolder) Val(b.theObject)) Select CStr(Array.IndexOf(arrColor2, xItem))).ToArray, ";")
        If MaxColors1 = MaxColors2 Then
            'good sofar...
        Else
            Return False
        End If
        'find the minors
        Dim MinColors1 = Join((From xItem In arrColor1 Where Val(xItem.theObject) = arrColor1.Min(Function(b As ObjectHolder) Val(b.theObject)) Select CStr(Array.IndexOf(arrColor1, xItem))).ToArray, ";")
        Dim MinColors2 = Join((From xItem In arrColor2 Where Val(xItem.theObject) = arrColor2.Min(Function(b As ObjectHolder) Val(b.theObject)) Select CStr(Array.IndexOf(arrColor2, xItem))).ToArray, ";")
        If MinColors1 = MinColors2 Then
            'good sofar...
        Else
            Return False
        End If
        Return True
    End Function

#End Region

    Dim PaintHoverColor As Integer = -1

    Private Function GetColorIndexAtLocation(ByVal Location As Point) As Integer
        GetColorIndexAtLocation = -1
        Dim ts = TextSize
        If Location.X < CInt(ts.Width) Then
            'over left margin
        Else
            GetColorIndexAtLocation = CInt((Int(Location.Y / OrigHeight) * BlocksAcross) + (Int((Location.X - CInt(ts.Width)) / OrigHeight)))
            If GetColorIndexAtLocation < Colors.Count Then
                'all good
            Else
                GetColorIndexAtLocation = -1
            End If
        End If
    End Function

    Private Function GetColorRectFromIndex(ByVal Index As Integer) As Rectangle
        Dim x = Index Mod BlocksAcross
        Dim y As Integer = CInt(Int((Index - x) / mc_BlocksAcross))
        Return New Rectangle((x * OrigHeight), (y * OrigHeight), OrigHeight, OrigHeight)
    End Function

    Public Event ColorChanged(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub tsiColorPicker_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        Dim ColorIndex = GetColorIndexAtLocation(e.Location)
        If ColorIndex = -1 Then
            'did not click a color
        Else
            Me.SelectedColor = Colors(ColorIndex)
            RaiseEvent ColorChanged(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub tsiColorPickerItem_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim ts = TextSize
        If ts.Width <> 0 Then
            'draw the text
            Using sb As New SolidBrush(Me.ForeColor)
                e.Graphics.DrawString(Me.Text, Me.Font, sb, 0, (Me.Height - ts.Height) / 2)
            End Using
            e.Graphics.TranslateTransform(CInt(ts.Width), 0)
        End If

        'draw each color 
        Dim ColorBorderSize As Integer = CInt(OrigHeight * 0.33)

        Dim MaxY = BlocksHigh - 1
        For y = 0 To MaxY
            For x = 0 To BlocksAcross - 1
                Dim ColorNo = (mc_BlocksAcross * y) + x
                If ColorNo < Colors.Count Then
                    Dim Color = Colors(ColorNo)
                    Dim Rect = New RectangleF(CSng((x * OrigHeight) + (ColorBorderSize / 2)), CSng((y * OrigHeight) + (ColorBorderSize / 2)), OrigHeight - ColorBorderSize, OrigHeight - ColorBorderSize)

                    If PaintHoverColor = ColorNo OrElse Color.ToArgb = mc_SelectedColor.ToArgb Then
                        'draw selection rectangle
                        'had to create a new object to call DrawItemCheck on as otherwise it draws the item at the full height
                        Using tsi As New ToolStripButton
                            tsi.AutoSize = False
                            tsi.Width = OrigHeight
                            tsi.Height = OrigHeight
                            tsi.Select()
                            Using b As New Bitmap(tsi.Width, tsi.Height)
                                Using g = Graphics.FromImage(b)
                                    Me.Parent.Renderer.DrawButtonBackground(New ToolStripItemRenderEventArgs(g, tsi))
                                    If Color <> mc_SelectedColor Then
                                        'alpha image
                                        b.Filters.Alpha()
                                    Else
                                        'just draw
                                    End If
                                    e.Graphics.DrawImage(b, New PointF(CInt(Rect.X - (ColorBorderSize / 2)), CInt(Rect.Y - (ColorBorderSize / 2))))
                                End Using
                            End Using
                        End Using
                    End If

                    Using sb As New SolidBrush(Color)
                        e.Graphics.FillRectangle(sb, Rect)
                    End Using

                    If PaintHoverColor <> ColorNo AndAlso Color <> mc_SelectedColor Then
                        'draw border
                        Using p As New Pen(AlphaColor(Color.FromKnownColor(KnownColor.MenuText), 63))
                            e.Graphics.DrawRectangle(p, Rect.X, Rect.Y, Rect.Width, Rect.Height)
                        End Using
                    End If
                End If
            Next
        Next
        e.Graphics.ResetTransform()
    End Sub

    Public Shared Function AlphaColor(ByVal theColor As Color, Optional ByVal AlphaLevel As Byte = 255) As Color
        AlphaColor = Color.FromArgb(AlphaLevel, theColor.R, theColor.G, theColor.B)
    End Function

End Class

