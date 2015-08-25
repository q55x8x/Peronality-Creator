'i00 PropertyEditor
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
Public Class PropertyEditor
    Implements IServiceProvider
    Implements System.Windows.Forms.Design.IWindowsFormsEditorService
    Implements IDisposable

    <AttributeUsage(AttributeTargets.Property Or AttributeTargets.Field)> _
    Public MustInherit Class CustomDropdownListAttribute
        Inherits System.Attribute

        Public MustOverride Function List() As List(Of Object)

    End Class

    Public Shared Function PaintProperty(ByVal theObject As Object, ByVal g As Graphics, ByVal Bounds As Rectangle, Optional ByVal OverrideTypeEditor As System.Reflection.MemberInfo = Nothing) As Boolean
StartOver:
        Dim ObjectType As System.Reflection.MemberInfo = If(TypeOf theObject Is Type, DirectCast(theObject, Type), theObject.GetType)
        If OverrideTypeEditor IsNot Nothing Then ObjectType = OverrideTypeEditor
        Dim Attrib = ObjectType.GetCustomAttributes(True).OfType(Of System.ComponentModel.EditorAttribute).FirstOrDefault()
        If Attrib IsNot Nothing Then
            Dim SpecificTypeEditor = Type.GetType(Attrib.EditorTypeName)
            If SpecificTypeEditor IsNot Nothing Then
                Dim Editor = DirectCast(System.Activator.CreateInstance(SpecificTypeEditor), System.Drawing.Design.UITypeEditor)
                If Editor.GetPaintValueSupported() Then
                    If g IsNot Nothing Then
                        Editor.PaintValue(theObject, g, Bounds)
                    End If
                    Return True
                End If
            End If
        End If
        If OverrideTypeEditor IsNot Nothing Then
            OverrideTypeEditor = Nothing
            GoTo StartOver
        End If
    End Function

    Public Function ShowPropertyEditor(ByVal theObject As Object, ByVal Location As Point, Optional ByVal OverrideTypeEditor As System.Reflection.MemberInfo = Nothing) As Object
StartOver:
        Dim ObjectType As System.Reflection.MemberInfo = If(TypeOf theObject Is Type, DirectCast(theObject, Type), theObject.GetType)
        If OverrideTypeEditor IsNot Nothing Then ObjectType = OverrideTypeEditor
        Dim Attrib = ObjectType.GetCustomAttributes(True).OfType(Of System.ComponentModel.EditorAttribute).FirstOrDefault()
        Dim Editor As System.Drawing.Design.UITypeEditor = Nothing
        If Attrib IsNot Nothing Then
            Dim SpecificTypeEditor = Type.GetType(Attrib.EditorTypeName)
            If SpecificTypeEditor IsNot Nothing Then
                Editor = DirectCast(System.Activator.CreateInstance(SpecificTypeEditor), System.Drawing.Design.UITypeEditor)
ReloadEditor:
                If Editor.GetEditStyle <> Drawing.Design.UITypeEditorEditStyle.None Then
                    Resizable = Editor.IsDropDownResizable
                    Me.Location = Location
                    Return Editor.EditValue(Me, theObject)
                ElseIf theObject IsNot Nothing AndAlso TypeOf theObject Is [Enum] Then
                    'show list...
EnumList:
                    Editor = New EnumUITypeEditor(Editor)
                    GoTo ReloadEditor
                End If
            End If
        ElseIf theObject IsNot Nothing AndAlso TypeOf theObject Is [Enum] Then
            'show list
            GoTo EnumList
        Else
            'check custom list...
            Dim DropListAttrib = ObjectType.GetCustomAttributes(True).OfType(Of CustomDropdownListAttribute).FirstOrDefault()
            If DropListAttrib IsNot Nothing Then
                Dim DropList = DirectCast(System.Activator.CreateInstance(DropListAttrib.GetType), CustomDropdownListAttribute).List()
                If DropList IsNot Nothing Then
                    Editor = New EnumUITypeEditor(Editor)
                    DirectCast(Editor, EnumUITypeEditor).ListOverride = DropList
                    GoTo ReloadEditor
                End If
            End If
        End If
        If OverrideTypeEditor IsNot Nothing Then
            OverrideTypeEditor = Nothing
            GoTo StartOver
        End If
        Throw New NotSupportedException(theObject.GetType.Name & " does not have a Property Editor")
    End Function

    Dim Resizable As Boolean
    Public TopMost As Boolean
    Dim Location As Point

    Public Function GetService(ByVal serviceType As Type) As Object Implements IServiceProvider.GetService
        If serviceType Is GetType(System.Windows.Forms.Design.IWindowsFormsEditorService) Then
            Return Me
        End If
        Return Nothing
    End Function

    Public Class DropDownForm
        Inherits Form

        '<System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
        'Public Structure WINDOWPOS
        '    Public hwnd As IntPtr
        '    Public hwndInsertAfter As IntPtr
        '    Public x As Integer
        '    Public y As Integer
        '    Public cx As Integer
        '    Public cy As Integer
        '    Public flags As Integer
        'End Structure

        'Const SWP_NOSIZE As Integer = &H1
        'Const SWP_NOMOVE As Integer = &H2
        'Const SWP_NOACTIVATE As Integer = &H10
        'Const WM_WINDOWPOSCHANGING As Integer = &H46

        Protected Overrides ReadOnly Property CreateParams() As System.Windows.Forms.CreateParams
            Get
                Dim baseParams As CreateParams = MyBase.CreateParams
                Const WS_EX_NOACTIVATE As Integer = &H8000000
                Const WS_EX_TOOLWINDOW As Integer = &H80
                baseParams.ExStyle = baseParams.ExStyle Or CInt(WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW)
                Return baseParams
            End Get
        End Property


        'Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        '    MyBase.WndProc(m)
        '    Debug.Print(m.ToString)
        '    Select Case m.Msg
        '        Case WM_WINDOWPOSCHANGING
        '            Dim WinPos As New WINDOWPOS
        '            WinPos = CType(Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(WINDOWPOS)), WINDOWPOS)
        '            If (WinPos.flags And SWP_NOSIZE) = SWP_NOSIZE And (WinPos.flags And SWP_NOMOVE) = SWP_NOMOVE And (WinPos.flags And SWP_NOACTIVATE) = SWP_NOACTIVATE Then
        '                If Me.Visible Then
        '                    Close()
        '                End If
        '            End If
        '    End Select
        'End Sub

        Public Sub New(ByVal Control As Control)
            Me.ShowInTaskbar = False
            Me.Text = ""
            Me.FormBorderStyle = FormBorderStyle.Sizable
            Me.MinimizeBox = False
            Me.MaximizeBox = False
            Me.ControlBox = False

            Me.ClientSize = Control.Size
            Me.MinimumSize = Me.Size

            Me.Controls.Add(Control)

            Control.Dock = DockStyle.Fill

            'If OwnerControl IsNot Nothing Then
            '    Me.StartPosition = FormStartPosition.Manual
            '    Me.Location = OwnerControl.PointToScreen(New Point(0, OwnerControl.Height))
            'End If
        End Sub

    End Class

    Public DropDown As DropDownForm

    Public Sub DropDownControl(ByVal ctl As Control) Implements System.Windows.Forms.Design.IWindowsFormsEditorService.DropDownControl
        DropDown = New DropDownForm(ctl)
        If Location <> Point.Empty Then
            DropDown.Location = Location
            DropDown.StartPosition = FormStartPosition.Manual
        End If
        If TopMost Then
            DropDown.TopMost = True
        End If
        DropDown.ShowDialog()
    End Sub

    Delegate Sub CloseDropDown_cb()
    Public Sub CloseDropDown() Implements System.Windows.Forms.Design.IWindowsFormsEditorService.CloseDropDown
        If DropDown IsNot Nothing Then
            If DropDown.InvokeRequired Then
                Dim CloseDropDown_cb As New CloseDropDown_cb(AddressOf CloseDropDown)
                DropDown.Invoke(CloseDropDown_cb)
            Else
                DropDown.Close()
                DropDown.Dispose()
                DropDown = Nothing
            End If
        End If
    End Sub

    Public Function ShowDialog(ByVal dialog As System.Windows.Forms.Form) As System.Windows.Forms.DialogResult Implements System.Windows.Forms.Design.IWindowsFormsEditorService.ShowDialog
        dialog.ShowDialog()
    End Function

#Region "IDisposable"

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
                CloseDropDown()
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#End Region



End Class