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

Public Module MiscControlExtension

    'Forces a ControlExtension not to get automatically picked up in EnableControlExtensions
    <AttributeUsage(AttributeTargets.Class)> _
    Public Class DoNotApplyControlExtensionAttribute
        Inherits System.Attribute
    End Class

    Public Class ControlExtensionAddedRemovedEventArgs
        Inherits EventArgs
        Public Control As Control
        Public Extension As ControlExtension
    End Class

    Public Class ControlExtensionAddingEventArgs
        Inherits EventArgs
        Public Cancel As Boolean
        Public Extension As ControlExtension
        Public Control As Control
    End Class

    <System.Runtime.CompilerServices.Extension()> _
    Public Sub EnableControlExtensions(ByVal sender As Control)
        ControlExtensions.Poll.MonitorControl(sender)
    End Sub

    <System.Runtime.CompilerServices.Extension()> _
    Public Function ExtensionCast(Of T)(ByVal Control As Control) As T
        If ControlExtensions.LoadedControlExtensions.ContainsKey(Control) Then
            'can't use trycast for T so use a try catch instead
            Try
                Return DirectCast(DirectCast((From xItem In ControlExtensions.LoadedControlExtensions.Item(Control) Where GetType(T).IsAssignableFrom(xItem.GetType)).FirstOrDefault, Object), T)
            Catch ex As Exception
                Return Nothing
            End Try
        Else
            Return Nothing
        End If
    End Function

End Module

Public Class ControlExtensions
    Friend Shared WithEvents Poll As New OwnedControlsPoll

    Public Shared Event ControlExtensionAdded(ByVal sender As Object, ByVal e As ControlExtensionAddedRemovedEventArgs)
    Public Shared Event ControlExtensionAdding(ByVal sender As Object, ByVal e As ControlExtensionAddingEventArgs)
    Public Shared Event ControlExtensionRemoved(ByVal sender As Object, ByVal e As ControlExtensionAddedRemovedEventArgs)

    Public Shared LoadedControlExtensions As New Dictionary(Of Control, List(Of ControlExtension))

    Public Shared Function GetControlsWithExtension(Of T)() As List(Of Control)
        'can't use trycast for T so use a try catch instead
        Try
            Return (From xItem In ControlExtensions.LoadedControlExtensions Where xItem.Key.ExtensionCast(Of T)() IsNot Nothing Select xItem.Key).ToList
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Sub LoadSingleControlExtension(ByVal control As Control, ByVal Extension As ControlExtension, Optional ByVal AllowCancel As Boolean = True)
        'allow user to cancel
        Dim ControlExtensionAddingEventArgs As New ControlExtensionAddingEventArgs With {.Control = control, .Extension = Extension}
        RaiseEvent ControlExtensionAdding(Nothing, ControlExtensionAddingEventArgs)
        If AllowCancel AndAlso ControlExtensionAddingEventArgs.Cancel = True Then
            Return
        End If

        If LoadedControlExtensions.ContainsKey(control) Then
            'control already in the collection
        Else
            LoadedControlExtensions.Add(control, New List(Of ControlExtension))
            AddHandler control.Disposed, AddressOf Control_Disposed
        End If

        If (From xItem In LoadedControlExtensions.Item(control) Where Extension.GetType.IsAssignableFrom(xItem.GetType) = True).FirstOrDefault Is Nothing Then
            'this extension has not been added yet so add it

            'add the actual extension to the list of loaded extensions
            LoadedControlExtensions(control).Add(Extension)

            'add any required extensions
            Dim RequiredExtensions = Extension.RequiredExtensions
            If RequiredExtensions IsNot Nothing Then
                For Each item In RequiredExtensions
                    'create a new instance of the plugin class required
                    Dim o = TryCast(System.Activator.CreateInstance(item), ControlExtension)
                    If o Is Nothing Then
                        Throw New Exception(Extension.GetType.Name & " could not load required plugin " & item.Name)
                    Else
                        LoadSingleControlExtension(control, o, False)
                    End If
                Next
            End If

            'setup the control extension ...
            Extension.DoLoad(control)

            'tell the user it has been added
            RaiseEvent ControlExtensionAdded(Nothing, New ControlExtensionAddedRemovedEventArgs With {.Control = control, .Extension = Extension})
        End If
    End Sub

    Private Shared Sub Poll_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles Poll.ControlAdded
        Static plugins As List(Of ControlExtension) = PluginManager(Of ControlExtension).GetPlugins

        Dim AcceptedClasses = (From xItem In plugins Where (From xItemTypes In xItem.ControlTypes Where xItemTypes.IsAssignableFrom(e.Control.GetType)).FirstOrDefault IsNot Nothing) 'AndAlso xItem.GetType.GetCustomAttributes(True).OfType(Of DoNotApplyControlExtensionAttribute)().FirstOrDefault Is Nothing)

        For Each AcceptedClass In AcceptedClasses
            'create a new instance of the plugin class
            Dim o = DirectCast(System.Activator.CreateInstance(AcceptedClass.GetType), ControlExtension)
            'load the extension
            LoadSingleControlExtension(e.Control, o)
        Next

    End Sub

    Private Shared Sub Control_Disposed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ctlSender = TryCast(sender, Control)
        If ctlSender IsNot Nothing Then
            RaiseEvent ControlExtensionRemoved(Nothing, New ControlExtensionAddedRemovedEventArgs With {.Control = ctlSender})
            LoadedControlExtensions.Remove(ctlSender)
        End If
    End Sub

End Class
