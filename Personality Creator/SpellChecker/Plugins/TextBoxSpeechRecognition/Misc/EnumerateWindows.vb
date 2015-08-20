''i00 .Net Window Enumeration
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

Public Class Windows
    Public Shared Function EnumerateWindows() As List(Of Window)
        Dim Windows As New Windows
        Return Windows.AllWindows
    End Function

#Region "APIs"

    <System.Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function EnumWindows(ByVal ewp As EnumWindows_cb, ByVal lParam As Integer) As Integer
    End Function

#End Region

    Public Delegate Function EnumWindows_cb(ByVal hWnd As Integer, ByVal lParam As Integer) As Boolean
    Private AllWindows As New List(Of Window)

    Private Sub New()
        EnumWindows(New EnumWindows_cb(AddressOf EnumWindow), 0)
    End Sub

    Private Function EnumWindow(ByVal hWnd As Integer, ByVal lParam As Integer) As Boolean
        AllWindows.Add(New Window(New IntPtr(hWnd)))
        Return True
    End Function

#Region "Window Return Object"

    Public Class Window

#Region "APIs"

        <System.Runtime.InteropServices.DllImport("user32.dll")> _
        Private Shared Function GetWindowText(ByVal hWnd As IntPtr, ByVal title As System.Text.StringBuilder, ByVal size As Integer) As Integer
        End Function
        <System.Runtime.InteropServices.DllImport("user32.dll")> _
        Private Shared Function GetWindowModuleFileName(ByVal hWnd As IntPtr, ByVal title As System.Text.StringBuilder, ByVal size As Integer) As Integer
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll")> _
        Private Shared Function IsWindowVisible(ByVal hWnd As IntPtr) As Boolean
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)> _
        Private Shared Function GetWindowThreadProcessId(ByVal hwnd As IntPtr, ByRef lpdwProcessId As IntPtr) As Integer
        End Function

        <System.Runtime.InteropServices.DllImport("user32.dll")> _
        Private Shared Function GetProp(ByVal hWnd As IntPtr, ByVal lpString As String) As IntPtr
        End Function

        Private Const WM_SYSCOMMAND As Integer = &H112
        Private Const SC_RESTORE As Integer = &HF120&

        <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True, CharSet:=Runtime.InteropServices.CharSet.Auto)> _
       Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function

        Private Declare Auto Function IsIconic Lib "user32.dll" (ByVal hwnd As IntPtr) As Boolean

#End Region

        Friend Sub New(ByVal Handle As IntPtr)
            mc_Handle = Handle
        End Sub

#Region "Window Attributes"

        Dim mc_Handle As IntPtr
        Public ReadOnly Property Handle() As IntPtr
            Get
                Return mc_Handle
            End Get
        End Property

        Public Enum WindowStates
            Normal
            Minimised
            'Maximised - not implemented
        End Enum
        Public ReadOnly Property WindowState() As WindowStates
            Get
                If IsIconic(mc_Handle) Then
                    Return WindowStates.Minimised
                Else
                    Return WindowStates.Normal
                End If
            End Get
        End Property

        Public ReadOnly Property Title() As String
            Get
                Dim sb_title As New System.Text.StringBuilder(256)
                GetWindowText(mc_Handle, sb_title, 256)
                Return sb_title.ToString
            End Get
        End Property

        Public ReadOnly Property ModuleName() As String
            Get
                Dim sb_ModuleName As New System.Text.StringBuilder(256)
                GetWindowModuleFileName(mc_Handle, sb_ModuleName, 256)
                Return sb_ModuleName.ToString()
            End Get
        End Property

        Public ReadOnly Property Visible() As Boolean
            Get
                Return IsWindowVisible(mc_Handle)
            End Get
        End Property

        Public ReadOnly Property PID() As IntPtr
            Get
                Dim procID As IntPtr
                GetWindowThreadProcessId(mc_Handle, procID)
                Return procID
            End Get
        End Property

        Public Function GetProperty(ByVal PropertyName As String) As IntPtr
            Return GetProp(mc_Handle, PropertyName)
        End Function

#End Region

#Region "Window Actions"

        Public Sub Restore()
            If WindowState = WindowStates.Minimised Then
                SendMessage(mc_Handle, WM_SYSCOMMAND, New IntPtr(SC_RESTORE), IntPtr.Zero)
            End If
        End Sub

        Public Sub ActivateApp()
            Dim PID = Me.PID
            If PID.ToInt32 <> 0 Then AppActivate(PID.ToInt32)
        End Sub

#End Region

    End Class

#End Region

End Class
