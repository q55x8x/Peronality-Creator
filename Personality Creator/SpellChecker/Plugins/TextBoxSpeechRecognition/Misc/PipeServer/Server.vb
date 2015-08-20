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

Imports System.ComponentModel
Namespace Controls

    <System.Drawing.ToolboxBitmap(GetType(ImageRes), "PipeServerIcon.ico")> _
    <System.ComponentModel.DesignerCategory("")> _
    Public Class PipeServer
        Inherits System.ComponentModel.Component

#Region "Properties"

        'The name of the pipe that we will be communicating on
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

#Region "Event Form"

        Public Class EventForm
            Inherits Form

            Public Sub New()
                'generate a window handle...
                Dim asd = Me.Handle
            End Sub

            Public Event CommandRecieved(ByVal Command As String)

#Region "APIs"

            <System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
            Private Structure COPYDATASTRUCT
                Public dwData As IntPtr
                Public cdData As Integer
                Public lpData As IntPtr
            End Structure

#End Region

            Private Const WM_COPYDATA As Integer = &H4A
            Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
                Select Case m.Msg
                    Case WM_COPYDATA
                        Dim cds As COPYDATASTRUCT = DirectCast(System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, GetType(COPYDATASTRUCT)), COPYDATASTRUCT)
                        Dim b(0 To cds.cdData - 1) As Byte
                        System.Runtime.InteropServices.Marshal.Copy(cds.lpData, b, 0, cds.cdData)
                        Dim data = System.Text.UnicodeEncoding.ASCII.GetString(b)
                        RaiseEvent CommandRecieved(data)
                End Select
                MyBase.WndProc(m)
            End Sub

        End Class

#End Region

#Region "Pipe Server"

        Public Event CommandRecieved(ByVal Command As String)

        Private Sub frmEvent_CommandRecieved(ByVal Command As String) Handles frmEvent.CommandRecieved
            RaiseEvent CommandRecieved(Command)
        End Sub

        Public Sub New()

        End Sub

        Public Sub New(ByVal PipeName As String)
            Me.PipeName = PipeName
            Me.StartServer()
        End Sub

        Private WithEvents frmEvent As EventForm

        Public Sub StopServer()
            frmEvent.Close()
            frmEvent = Nothing
        End Sub

        Public Sub StartServer()
            frmEvent = New EventForm
            TagWindow(frmEvent.Handle, PipeName)
        End Sub

        <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)> _
        Private Shared Function SetProp(ByVal hWnd As IntPtr, ByVal lpString As String, ByVal hData As IntPtr) As Boolean
        End Function

        Public Sub TagWindow(ByVal hwnd As IntPtr, ByVal PipeID As String)
            ' Applies a window property to allow the window to
            ' be clearly identified.
            SetProp(hwnd, PipeID & "_i00PIPE", New IntPtr(1))
        End Sub

#End Region

    End Class

End Namespace