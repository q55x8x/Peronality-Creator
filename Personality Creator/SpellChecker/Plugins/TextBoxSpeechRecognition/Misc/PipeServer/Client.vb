''i00 .Net Pipe Server
''©i00 Productions All rights reserved
''Created by Kris Bennett
''----------------------------------------------------------------------------------------------------
''All property in this file is and remains the property of i00 Productions, regardless of its usage,
''unless stated otherwise in writing from i00 Productions.
''
''i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
''use or miss-use of this product.  This product is only a component and thus is intended to be used 
''as part of other software, it is not a complete software package, thus i00 Productions is not 
''responsible for any legal ramifications that software using this product breaches.

Namespace Controls

    <System.Drawing.ToolboxBitmap(GetType(ImageRes), "PipeServerIcon.ico")> _
    <System.ComponentModel.DesignerCategory("")> _
    Public Class PipeClient
        Inherits System.ComponentModel.Component

        Public Sub New()

        End Sub

        Public Sub New(ByVal PipeName As String)
            Me.PipeName = PipeName
        End Sub

#Region "Properties"

        Dim mc_PipeName As String
        Public Property PipeName() As String
            Get
                Return mc_PipeName
            End Get
            Set(ByVal value As String)
                mc_PipeName = value
            End Set
        End Property

#End Region

#Region "Send message"

#Region "APIs"

        <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)> _
        Private Shared Function SendMessageTimeout(ByVal windowHandle As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr, ByVal flags As SendMessageTimeoutFlags, ByVal timeout As Integer, ByRef result As IntPtr) As IntPtr
        End Function

        <System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
        Private Structure COPYDATASTRUCT
            Public dwData As IntPtr
            Public cdData As Integer
            Public lpData As IntPtr
        End Structure

        <Flags()> _
        Private Enum SendMessageTimeoutFlags
            SMTO_NORMAL = 0
            SMTO_BLOCK = 1
            SMTO_ABORTIFHUNG = 2
            SMTO_NOTIMEOUTIFNOTHUNG = 8
        End Enum

        Private Const WM_COPYDATA As Integer = &H4A

        '<System.Runtime.InteropServices.DllImport("user32.dll")> _
        'Private Shared Function GetProp(ByVal hWnd As IntPtr, ByVal lpString As String) As IntPtr
        'End Function

#End Region

        'To convert an object to a memory pointer
        Private Function VarPtr(ByVal e As Object) As IntPtr
            Dim GC As System.Runtime.InteropServices.GCHandle = System.Runtime.InteropServices.GCHandle.Alloc(e, Runtime.InteropServices.GCHandleType.Pinned)
            Dim GC2 = GC.AddrOfPinnedObject
            GC.Free()
            Return GC2
        End Function

        'To Send a message
        Public Function SendCommand(ByVal Command As String, Optional ByVal IncludeClientsInThisApp As Boolean = False, Optional ByVal BringServerToTop As Boolean = False) As List(Of Windows.Window)

            Dim Servers As New List(Of Windows.Window)

            For Each item In (From xItem In Windows.EnumerateWindows)
                If item.GetProperty(mc_PipeName & "_i00PIPE").ToInt32 = 1 Then
                    'pipe server window found...
                    If IncludeClientsInThisApp Then
                        'do all windows
                        Servers.Add(item)
                    Else
                        'only do the other windows that are not part of this application
                        If Process.GetCurrentProcess.Id <> item.PID.ToInt32 Then
                            Servers.Add(item)
                        End If
                    End If
                End If
            Next


            If Servers.Count = 0 Then
                'no server to send to :(
            Else
                'send message

                Dim b = System.Text.UnicodeEncoding.ASCII.GetBytes(Command)
                Dim Data As New COPYDATASTRUCT
                Data.dwData = New IntPtr
                Data.cdData = UBound(b) + 1
                Data.lpData = VarPtr(b)
                Dim DataPointer = VarPtr(Data)
                Dim r As IntPtr

                For Each client In Servers
                    If BringServerToTop = True Then
                        'Try to activate the existing window:
                        client.Restore()
                        client.ActivateApp()
                    End If

                    Dim lr = SendMessageTimeout(client.Handle, WM_COPYDATA, IntPtr.Zero, DataPointer, SendMessageTimeoutFlags.SMTO_NORMAL, 5000, r)
                Next
            End If
            Return Servers
        End Function

#End Region

    End Class
End Namespace