'i00 .Net Control Extensions
'©i00 Productions All rights reserved
'Created by Kris Bennett
'----------------------------------------------------------------------------------------------------
'All property in this file is and remains the property of i00 Productions, regardless of its usage,
'unless stated otherwise in writing from i00 Productions.
'
'Anyone wishing to use this code in their projects may do so, however are required to leave a post on
'VBForums (under: http://www.vbforums.com/showthread.php?p=4075093) stating that they are doing so.
'A simple "I am using i00 Spell check in my project" will surffice.
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

<System.ComponentModel.DesignerCategory()> _
Public MustInherit Class ControlExtension
    Inherits NativeWindow
    Implements IMessageFilter

#Region "Update the WndProc Handle when the control gets a new handle - for when properties like RightToLeft are changed"

    Public Event ControlDisposed(ByVal sender As Object, ByRef e As EventArgs)
    Private Sub mc_Control_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mc_Control.Disposed
        RaiseEvent ControlDisposed(sender, e)
        'remove the meesage filter...
        Application.RemoveMessageFilter(Me)
    End Sub

    Private Sub mc_Control_HandleCreated(ByVal sender As Object, ByVal e As System.EventArgs) Handles mc_Control.HandleCreated
        AssignHandle(Control.Handle)
    End Sub

#End Region

#Region "IMessageFilter"

    Public Class PreFilterMessageEventArgs
        Inherits EventArgs
        Public m As System.Windows.Forms.Message
        Public Cancel As Boolean
    End Class

    Public Event PreFilterMessage(ByVal sender As Object, ByRef e As PreFilterMessageEventArgs)
    Private Function ePreFilterMessage(ByRef m As System.Windows.Forms.Message) As Boolean Implements System.Windows.Forms.IMessageFilter.PreFilterMessage
        Dim PreFilterMessageEventArgs As New PreFilterMessageEventArgs() With {.m = m}
        RaiseEvent PreFilterMessage(Me, PreFilterMessageEventArgs)
    End Function

#End Region

    Public Event Loaded(ByVal sender As Object, ByVal e As EventArgs)

    <System.ComponentModel.Description("Control Extensions that are required to be loaded prior to this ControlExtension")> _
    <System.ComponentModel.Browsable(False)> _
    <System.ComponentModel.DisplayName("Required Extensions")> _
    Public Overridable ReadOnly Property RequiredExtensions() As List(Of Type)
        Get
            Return Nothing
        End Get
    End Property

    <System.ComponentModel.Description("The type of control that will be automatically Extended by this class")> _
    <System.ComponentModel.Browsable(False)> _
    <System.ComponentModel.Category("Control")> _
    <System.ComponentModel.DisplayName("Control Type")> _
    Public MustOverride ReadOnly Property ControlTypes() As IEnumerable(Of Type)

    Private WithEvents mc_Control As Control
    <System.ComponentModel.Description("The Control associated with the ControlExtension object")> _
    <System.ComponentModel.Category("Control")> _
    <System.ComponentModel.DisplayName("Control")> _
    Public Overridable ReadOnly Property Control() As Control
        Get
            Return mc_Control
        End Get
    End Property

    Delegate Sub InstateHandle_cb()
    Private Sub InstateHandle()
        If Control.InvokeRequired Then
            Dim InstateHandle_cb As New InstateHandle_cb(AddressOf InstateHandle)
            Control.Invoke(InstateHandle_cb)
        Else
            If Control.IsHandleCreated = False Then
                Dim handle = Control.Handle
            Else
                AssignHandle(Control.Handle)
            End If
        End If
    End Sub

    Friend Sub DoLoad(ByVal control As Control)
        Me.mc_Control = control
    
        'AssignHandle allows the WndProc to be fired :)
        InstateHandle()

        'Try
        '    AssignHandle(Control.Handle)
        'Catch ex As Exception

        'End Try

        Application.AddMessageFilter(Me)
        Load()

        RaiseEvent Loaded(Me, EventArgs.Empty)

    End Sub

    MustOverride Sub Load()

End Class
