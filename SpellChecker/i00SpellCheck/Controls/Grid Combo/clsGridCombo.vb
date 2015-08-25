'i00 .Net Grid Combo
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
Public Class clsGridCombo
    Inherits ComboBox

    Dim mc_MaxGridSize As New Size(10, 15)
    Public Property MaxGridSize() As Size
        Get
            Return mc_MaxGridSize
        End Get
        Set(ByVal value As Size)
            mc_MaxGridSize = value
            FillGridItems()
        End Set
    End Property

    Public Class GridComboItem
        Public Row As Integer = 1
        Public Col As Integer = 1
        Public Sub New(ByVal Row As Integer)
            Me.Row = Row
        End Sub
        Public Overrides Function ToString() As String
            Return Col & "x" & Row
        End Function
    End Class

    <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    End Function

    Private Const GWL_EXSTYLE As Integer = -20
    Private Const WS_EX_TRANSPARENT As Integer = &H20

    Dim mc_CellSize As Size = New Size(16, 16)
    Public Property CellSize() As Size
        Get
            Return mc_CellSize
        End Get
        Set(ByVal value As Size)
            mc_CellSize = value
        End Set
    End Property

    Dim mc_ShadowOffset As Integer = 3
    Public Property ShadowOffset() As Integer
        Get
            Return mc_ShadowOffset
        End Get
        Set(ByVal value As Integer)
            mc_ShadowOffset = value
        End Set
    End Property

    Public Sub New()
        MyBase.DropDownStyle = ComboBoxStyle.DropDownList
        MyBase.DrawMode = Windows.Forms.DrawMode.OwnerDrawVariable
        FillGridItems()

        'make the shadow click-through
        Dim exstyle2 As Integer = GetWindowLong(frmDropShadow.Handle, GWL_EXSTYLE)
        exstyle2 = exstyle2 Or WS_EX_TRANSPARENT
        SetWindowLong(frmDropShadow.Handle, GWL_EXSTYLE, exstyle2)
    End Sub

    Public Sub FillGridItems()
        MyBase.Items.Clear()
        For i = 1 To mc_MaxGridSize.Height
            MyBase.Items.Add(New GridComboItem(i))
        Next
        MyBase.DropDownHeight = (mc_MaxGridSize.Height * CellSize.Height) + 2
        MyBase.DropDownWidth = (mc_MaxGridSize.Width * CellSize.Width) + 3
        SelectSelectedCellInCombo()
        'need to set this again ... otherwise items will not re-measure
        MyBase.DrawMode = Windows.Forms.DrawMode.OwnerDrawVariable
    End Sub

    Public Event SelectedCellChanged(ByVal sender As Object, ByVal e As System.EventArgs)

    Dim mc_SelectedCell As New Point(1, 1)
    Public Property SelectedCell() As Point
        Get
            Return mc_SelectedCell
        End Get
        Set(ByVal value As Point)
            mc_SelectedCell = value
            SelectSelectedCellInCombo()
            RaiseEvent SelectedCellChanged(Me, New System.EventArgs())
        End Set
    End Property

    Private Sub SelectSelectedCellInCombo()
        Dim SelectedRow = From xItem In MyBase.Items.OfType(Of GridComboItem)() Where xItem.Row = mc_SelectedCell.Y
        If SelectedRow.Count > 0 AndAlso mc_SelectedCell.X <= mc_MaxGridSize.Width Then
            SelectedRow(0).Col = mc_SelectedCell.X
            MyBase.SelectedItem = SelectedRow(0)
        End If
    End Sub

    Dim clsGridComboDropped As Boolean

    Private Sub clsGridCombo_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        If e.Index = -1 Then Exit Sub
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        'draw the text 
        e.Graphics.FillRectangle(New SolidBrush(MyBase.BackColor), e.Bounds)
        If (e.State And DrawItemState.ComboBoxEdit) = DrawItemState.ComboBoxEdit Then
            DrawingFunctions.DrawElipseText(e.Graphics, New PointF(e.Bounds.X, e.Bounds.Y), MyBase.Items(e.Index).ToString, MyBase.Font, e.Bounds.Width, MyBase.ForeColor)
        Else
            For i = 1 To mc_MaxGridSize.Width
                Dim CellRect As Rectangle = New Rectangle(((i - 1) * CellSize.Width) + 1, e.Bounds.Y + 1, CellSize.Width - 2, CellSize.Height - 2)
                Dim GridComboItemSelected As GridComboItem = DirectCast(MyBase.SelectedItem, GridComboItem)
                Dim GridComboItemI As GridComboItem = DirectCast(MyBase.Items(e.Index), GridComboItem)
                If GridComboItemSelected IsNot Nothing Then
                    'highlight where needed
                    If i <= GridComboItemSelected.Col AndAlso GridComboItemI.Row <= GridComboItemSelected.Row Then
                        e.Graphics.FillRectangle(New SolidBrush(DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.Highlight), 127)), CellRect)
                    End If
                End If
                e.Graphics.DrawRectangle(New Pen(DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.ControlText), 127)), CellRect)
            Next
        End If
    End Sub

#Region "Over ride the dropdown"

    Private WithEvents Timer As New Timer

    Private WithEvents frmDrop As New frmNonFocusStealable
    Private WithEvents frmDropShadow As New PerPixelAlphaForm

    Private WithEvents BPDropDown As New BufferedPanel

    'this is used so that the drop down doesn't depress
    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Public Shared Function LockWindowUpdate(ByVal hWndLock As IntPtr) As Boolean
    End Function

    Private Sub clsGridCombo_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDown
        frmDrop.MasterForm = Me.FindForm
        frmDrop.Controls.Add(BPDropDown)
        BPDropDown.Dock = DockStyle.Fill
        frmDrop.ShowInTaskbar = False
        frmDrop.FormBorderStyle = FormBorderStyle.None
        frmDrop.StartPosition = FormStartPosition.Manual
        Dim DropDownPoint As Point = MyBase.PointToScreen(New Point(0, MyBase.Height))
        frmDrop.Bounds = New Rectangle(DropDownPoint.X, DropDownPoint.Y, MyBase.DropDownWidth, MyBase.DropDownHeight + MyBase.Height)

        frmDropShadow.Bounds = frmDrop.Bounds
        frmDropShadow.ShowInTaskbar = False
        frmDropShadow.Location = New Point(frmDropShadow.Bounds.X + mc_ShadowOffset, frmDropShadow.Bounds.Y + mc_ShadowOffset)
        Dim b As New Bitmap(frmDropShadow.Bounds.Width, frmDropShadow.Bounds.Height)
        Dim g As Graphics = Graphics.FromImage(b)
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        For i = 0 To mc_ShadowOffset
            'top
            g.DrawLine(New Pen(DrawingFunctions.AlphaColor(Color.Black, CByte((i / mc_ShadowOffset) * 255))), i, i, b.Width - i, i)
            'left
            g.DrawLine(New Pen(DrawingFunctions.AlphaColor(Color.Black, CByte((i / mc_ShadowOffset) * 255))), i, i, i, b.Height - i)
            'right
            g.DrawLine(New Pen(DrawingFunctions.AlphaColor(Color.Black, CByte((i / mc_ShadowOffset) * 255))), b.Width - i, i, b.Width - i, b.Height - i)
            'bottom
            g.DrawLine(New Pen(DrawingFunctions.AlphaColor(Color.Black, CByte((i / mc_ShadowOffset) * 255))), i, b.Height - i, b.Width - i, b.Height - i)
        Next
        frmDropShadow.SetBitmap(b, 127)

        frmDrop.BackColor = MyBase.BackColor
        Timer.Interval = 1
        Timer.Enabled = True

        If MyBase.SelectedItem IsNot Nothing Then
            MouseOverPoint = New Point(CType(MyBase.SelectedItem, GridComboItem).Col, CType(MyBase.SelectedItem, GridComboItem).Row)
            ValidateDropDownPoint()
        End If
        clsGridComboDropped = False
    End Sub

    Private Sub Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer.Tick
        Timer.Enabled = False
        LockWindowUpdate(Me.FindForm.Handle)
        Me.FindForm.SuspendLayout()
        'frmDrop.Visible = False
        'frmDropShadow.Visible = False
        frmDropShadow.Show(Me)
        frmDrop.Show(frmDropShadow)
        'frmDrop.Visible = True
        'frmDropShadow.Visible = True
    End Sub

    Private Sub frmDrop_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles frmDrop.LostFocus
        frmDropShadow.Visible = False
        frmDrop.Visible = False
        Me.FindForm.ResumeLayout()
        LockWindowUpdate(IntPtr.Zero)
    End Sub

    Dim MouseOverPoint As New Point

    Private Sub ValidateDropDownPoint()
        If MouseOverPoint.X > MaxGridSize.Width Then MouseOverPoint.X = MaxGridSize.Width
        If MouseOverPoint.X < 1 Then MouseOverPoint.X = 1

        If MouseOverPoint.Y > MaxGridSize.Height Then MouseOverPoint.Y = MaxGridSize.Height
        If MouseOverPoint.Y < 1 Then MouseOverPoint.Y = 1
    End Sub

    Private Sub BPDropDown_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles BPDropDown.MouseLeave
        Dim OldMousePos As String = MouseOverPoint.ToString()
        MouseOverPoint = mc_SelectedCell
        ValidateDropDownPoint()
        If OldMousePos <> MouseOverPoint.ToString Then
            BPDropDown.Refresh()
        End If
    End Sub

    Private Sub BPDropDown_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BPDropDown.MouseMove
        Dim OldMousePos As String = MouseOverPoint.ToString
        MouseOverPoint.X = CInt(Math.Truncate((e.X - 2) / CellSize.Width) + 1)
        MouseOverPoint.Y = CInt(Math.Truncate((e.Y - 2) / CellSize.Height) + 1)
        ValidateDropDownPoint()
        If OldMousePos <> MouseOverPoint.ToString Then
            BPDropDown.Refresh()
        End If
    End Sub

    Public Event CellPrePaint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
    Public Event CellPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    Private Sub BPDropDown_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BPDropDown.MouseUp
        SelectedCell = MouseOverPoint
        frmDrop.Visible = False
    End Sub

    Private Sub BPDropDown_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles BPDropDown.Paint
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        e.Graphics.DrawRectangle(New Pen(Color.FromKnownColor(KnownColor.ControlDark)), New Rectangle(0, 0, BPDropDown.Width - 1, BPDropDown.Height - 1))
        e.Graphics.TranslateTransform(1, 1)
        For x = 1 To mc_MaxGridSize.Width
            For y = 1 To mc_MaxGridSize.Height
                Dim CellRect As New Rectangle(((x - 1) * CellSize.Width) + 1, ((y - 1) * CellSize.Height) + 1, CellSize.Width - 2, CellSize.Height - 2)
                RaiseEvent CellPrePaint(Me, New PaintEventArgs(e.Graphics, CellRect))
                If x <= MouseOverPoint.X AndAlso y <= MouseOverPoint.Y Then
                    'make this cell blue
                    e.Graphics.FillRectangle(New SolidBrush(DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.Highlight), 127)), CellRect)
                End If
                e.Graphics.DrawRectangle(New Pen(DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.ControlText), 127)), CellRect)
                RaiseEvent CellPostPaint(Me, New PaintEventArgs(e.Graphics, CellRect))
            Next
        Next
        Dim BotText As String = MouseOverPoint.X & "x" & MouseOverPoint.Y
        Dim BotTextSize As SizeF = e.Graphics.MeasureString(BotText, MyBase.Font)
        Dim BotTextOffset As Integer = CInt((MyBase.Height - BotTextSize.Height) / 2)
        BotTextOffset = BotTextOffset - 1
        e.Graphics.DrawLine(New Pen(Color.FromKnownColor(KnownColor.ControlDark)), 0, BPDropDown.Height - MyBase.Height - 1, BPDropDown.Width, BPDropDown.Height - MyBase.Height - 1)
        DrawingFunctions.DrawElipseText(e.Graphics, New Point(BotTextOffset, (BPDropDown.Height - MyBase.Height) + BotTextOffset), BotText, MyBase.Font, BPDropDown.Width, MyBase.ForeColor)
    End Sub

#End Region

#Region "To make the drop down form not steal focus"

    Public Class frmNonFocusStealable
        Inherits Form

        Public active As Boolean = False
        <Runtime.InteropServices.DllImport("user32")> _
        Private Shared Function SendMessage(ByVal handle As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As IntPtr) As Integer
        End Function
        <Runtime.InteropServices.DllImport("user32.dll")> _
        Private Shared Function GetForegroundWindow() As IntPtr
        End Function
        Private Const WM_ACTIVATE As Integer = &H6
        Private Const WM_ACTIVATEAPP As Integer = &H1C
        Private Const WM_NCACTIVATE As Integer = &H86

        Public MasterForm As Form

        Public Sub New(ByVal MasterForm As Form)
            Me.MasterForm = MasterForm
        End Sub

        Public Sub New()

        End Sub

        Protected Overloads Overrides Sub WndProc(ByRef m As Message)
            MyBase.WndProc(m)
            If MasterForm Is Nothing Then Exit Sub
            If m.Msg = WM_NCACTIVATE Then
                If CInt(m.WParam) = 0 Then
                    Dim handle As IntPtr = GetForegroundWindow()
                    Try
                        If handle = Me.Handle OrElse handle = MasterForm.Handle Then
                            If Me.active = False Then
                                Me.active = True
                                SendMessage(Me.Handle, WM_NCACTIVATE, 1, IntPtr.Zero)
                            End If
                            'If cboGrid.frmDrop.active = False Then
                            'form2.active = True
                            SendMessage(MasterForm.Handle, WM_NCACTIVATE, 1, IntPtr.Zero)
                            'End If
                        Else
                            If Me.active = True Then
                                Me.active = False
                                SendMessage(Me.Handle, WM_NCACTIVATE, 0, IntPtr.Zero)
                            End If
                            'If form2.active = True Then
                            '    form2.active = False
                            SendMessage(MasterForm.Handle, WM_NCACTIVATE, 0, IntPtr.Zero)
                            'End If
                        End If
                    Catch ex As Exception
                    End Try
                End If
                If CInt(m.WParam) = 1 Then
                    If Me.active = False Then
                        Me.active = True
                        SendMessage(Me.Handle, WM_NCACTIVATE, 1, IntPtr.Zero)
                    End If
                    'If form2.active = False Then
                    'form2.active = True
                    SendMessage(MasterForm.Handle, WM_NCACTIVATE, 1, IntPtr.Zero)
                    'End If
                End If
            End If
        End Sub

    End Class

#End Region

    Private Sub clsGridCombo_MeasureItem(ByVal sender As Object, ByVal e As System.Windows.Forms.MeasureItemEventArgs) Handles Me.MeasureItem
        e.ItemHeight = CellSize.Height
    End Sub
End Class
