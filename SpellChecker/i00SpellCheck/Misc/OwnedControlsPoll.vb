'i00 Owned Controls Poll
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

Public Class OwnedControlsPoll
    Implements IDisposable

#Region "IDisposable"

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
                If tt IsNot Nothing Then
                    tt.Dispose()
                End If
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

    Public Event ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs)

    Private ControlPolls As New List(Of Control)
    Private tt As System.Threading.Timer

    Public Sub New()

    End Sub

    Public Sub New(ByVal MasterControl As Control)
        MonitorControl(MasterControl)
    End Sub

    Public Sub MonitorControl(ByVal MasterControl As Control)
        'go through all of the controls on this control...
        If ControlPolls.Contains(MasterControl) = False Then
            Dim control As Control = MasterControl
            Do Until control Is Nothing
                MonitorControl2(control)
                control = MasterControl.GetNextControl(control, True)
            Loop
        End If
    End Sub

    Public Sub MonitorControl2(ByVal MasterControl As Control)
        If TypeOf MasterControl Is Form AndAlso tt Is Nothing Then
            Dim cb As System.Threading.TimerCallback = AddressOf CheckNewForms
            tt = New System.Threading.Timer(cb, Nothing, 250, 250)
        End If
        If ControlPolls.Contains(MasterControl) = False Then
            ControlPolls.Add(MasterControl)

            Dim SplitContainer = TryCast(MasterControl, SplitContainer)
            If SplitContainer IsNot Nothing Then
                MonitorControl(SplitContainer.Panel1)
                MonitorControl(SplitContainer.Panel2)
            End If

            AddHandler MasterControl.ControlAdded, AddressOf Control_ControlAdded
            AddHandler MasterControl.Disposed, AddressOf Control_Disposed

            RaiseEvent ControlAdded(Me, New ControlEventArgs(MasterControl))

            Dim form = TryCast(MasterControl, Form)
            If form IsNot Nothing Then
                For Each frm In form.OwnedForms
                    MonitorControl(frm)
                Next
            End If
        End If
    End Sub

    Private Delegate Sub cb_SetupNewForm(ByVal NewForm As Form)
    Private Sub SetupNewForm(ByVal NewForm As Form)
        If NewForm.IsDisposed Then
            Exit Sub
        End If
        If NewForm.InvokeRequired Then
            Try
                'form could have been disposed between above ^ and now
                Dim cb_SetupNewControl As New cb_SetupNewForm(AddressOf SetupNewForm)
                NewForm.Invoke(cb_SetupNewControl, NewForm)
            Catch ex As Exception

            End Try
        Else
            'add form
            MonitorControl(NewForm)
        End If
    End Sub

    Private Sub CheckNewForms(ByVal state As Object)
        Dim AllOwnedForms()() As Form
        SyncLock ControlPolls
            AllOwnedForms = (From xItem In ControlPolls.OfType(Of Form)() Select xItem.OwnedForms).ToArray
        End SyncLock

        Dim NewForms As New List(Of Form)
        For Each iForms In AllOwnedForms
            For Each iForm In iForms
                If ControlPolls.Contains(iForm) = False Then
                    NewForms.Add(iForm)
                End If
            Next
        Next
        If NewForms.Count > 0 Then
            For Each item In NewForms
                SetupNewForm(item)
            Next
        End If
    End Sub

    Private Sub Control_Disposed(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim control = TryCast(sender, Control)
        If control IsNot Nothing Then
            If ControlPolls.Contains(control) Then
                ControlPolls.Remove(control)
            End If
        End If
    End Sub

    Private Sub Control_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs)
        MonitorControl(e.Control)
    End Sub

End Class
