'i00 PropertyEditorListView
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
Friend Class PropertyEditorListView
    Inherits ListBox

    Dim EnumType As Type
    Dim editor As System.Drawing.Design.UITypeEditor

    Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
        MyBase.OnDrawItem(e)
        e.DrawBackground()
        Dim Rect As New Rectangle(e.Bounds.X + 2, CInt(e.Bounds.Y + ((e.Bounds.Height - 15) / 2)), 19, 15)
        editor.PaintValue(DirectCast([Enum].Parse(EnumType, MyBase.Items(e.Index).ToString), [Enum]), e.Graphics, Rect)
        e.Graphics.DrawRectangle(System.Drawing.SystemPens.WindowText, Rect)
        Dim TextBounds = e.Bounds
        TextBounds.X += 22
        TextBounds.Width -= 22
        TextRenderer.DrawText(e.Graphics, MyBase.Items(e.Index).ToString, MyBase.Font, TextBounds, MyBase.ForeColor, TextFormatFlags.VerticalCenter Or TextFormatFlags.EndEllipsis Or TextFormatFlags.SingleLine Or TextFormatFlags.NoPrefix)
    End Sub

    Public Sub New(ByVal editorService As System.Windows.Forms.Design.IWindowsFormsEditorService, ByVal editor As System.Drawing.Design.UITypeEditor)
        Me.editorService = editorService
        Me.BorderStyle = Windows.Forms.BorderStyle.None
        Me.IntegralHeight = False

        Me.editor = editor
        If editor IsNot Nothing AndAlso editor.GetPaintValueSupported() Then
            MyBase.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
            MyBase.Height = CInt(MyBase.Height * (20 / MyBase.ItemHeight))
            MyBase.ItemHeight = 20
        End If
    End Sub

    Friend editorService As System.Windows.Forms.Design.IWindowsFormsEditorService

    Public Property EnumValue() As [Enum]
        Get
            If MyBase.SelectedItem Is Nothing Then
                Return Nothing
            Else
                Return DirectCast([Enum].Parse(EnumType, MyBase.SelectedItem.ToString), [Enum])
            End If
        End Get
        Set(ByVal value As [Enum])
            EnumType = value.GetType
            For Each item In [Enum].GetNames(EnumType)
                MyBase.Items.Add(item)
                If item.ToString = value.ToString Then
                    MyBase.SelectedItem = item
                End If
            Next
        End Set
    End Property

    Private Sub PropertyEditorListView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DoubleClick
        editorService.CloseDropDown()
    End Sub

    Private Sub PropertyEditorListView_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = vbCr Then
            editorService.CloseDropDown()
        End If
    End Sub
End Class
