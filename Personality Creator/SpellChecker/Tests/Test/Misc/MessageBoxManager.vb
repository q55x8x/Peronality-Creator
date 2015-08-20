'©i00 Productions All rights reserved
'This article is derived from http://www.codeproject.com/Articles/18399/Localizing-System-MessageBox
'----------------------------------------------------------------------------------------------------
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Security.Permissions

Public Class MessageBoxManager
    Implements IDisposable

    Public Sub New()
        Register()
    End Sub

#Region "IDisposable"

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            Unregister()
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

    Private Delegate Function HookProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    Private Delegate Function EnumChildProc(ByVal hWnd As IntPtr, ByVal lParam As IntPtr) As Boolean

    Private Const WH_CALLWNDPROCRET As Integer = 12
    Private Const WM_DESTROY As Integer = &H2
    Private Const WM_INITDIALOG As Integer = &H110
    Private Const WM_TIMER As Integer = &H113
    Private Const WM_USER As Integer = &H400
    Private Const DM_GETDEFID As Integer = WM_USER + 0

    Private Const MBOK As Integer = 1
    Private Const MBCancel As Integer = 2
    Private Const MBAbort As Integer = 3
    Private Const MBRetry As Integer = 4
    Private Const MBIgnore As Integer = 5
    Private Const MBYes As Integer = 6
    Private Const MBNo As Integer = 7


    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal lpfn As HookProc, ByVal hInstance As IntPtr, ByVal threadId As Integer) As IntPtr
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function UnhookWindowsHookEx(ByVal idHook As IntPtr) As Integer
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function CallNextHookEx(ByVal idHook As IntPtr, ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="GetWindowTextLengthW", CharSet:=CharSet.Unicode)> _
    Private Shared Function GetWindowTextLength(ByVal hWnd As IntPtr) As Integer
    End Function

    <DllImport("user32.dll", EntryPoint:="GetWindowTextW", CharSet:=CharSet.Unicode)> _
    Private Shared Function GetWindowText(ByVal hWnd As IntPtr, ByVal text As StringBuilder, ByVal maxLength As Integer) As Integer
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function EndDialog(ByVal hDlg As IntPtr, ByVal nResult As IntPtr) As Integer
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function EnumChildWindows(ByVal hWndParent As IntPtr, ByVal lpEnumFunc As EnumChildProc, ByVal lParam As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", EntryPoint:="GetClassNameW", CharSet:=CharSet.Unicode)> _
    Private Shared Function GetClassName(ByVal hWnd As IntPtr, ByVal lpClassName As StringBuilder, ByVal nMaxCount As Integer) As Integer
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function GetDlgCtrlID(ByVal hwndCtl As IntPtr) As Integer
    End Function

    <DllImport("user32.dll")> _
    Private Shared Function GetDlgItem(ByVal hDlg As IntPtr, ByVal nIDDlgItem As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", EntryPoint:="SetWindowTextW", CharSet:=CharSet.Unicode)> _
    Private Shared Function SetWindowText(ByVal hWnd As IntPtr, ByVal lpString As String) As Boolean
    End Function


    <StructLayout(LayoutKind.Sequential)> _
    Public Structure CWPRETSTRUCT
        Public lResult As IntPtr
        Public lParam As IntPtr
        Public wParam As IntPtr
        Public message As UInteger
        Public hwnd As IntPtr
    End Structure

    Private Shared hookProcObj As HookProc
    Private Shared enumProc As EnumChildProc
    <ThreadStatic()> _
    Private Shared hHook As IntPtr
    <ThreadStatic()> _
    Private Shared nButton As Integer

    ''' <summary>
    ''' OK text
    ''' </summary>
    Public Shared OK As String = "&OK"
    ''' <summary>
    ''' Cancel text
    ''' </summary>
    Public Shared Cancel As String = "&Cancel"
    ''' <summary>
    ''' Abort text
    ''' </summary>
    Public Shared Abort As String = "&Abort"
    ''' <summary>
    ''' Retry text
    ''' </summary>
    Public Shared Retry As String = "&Retry"
    ''' <summary>
    ''' Ignore text
    ''' </summary>
    Public Shared Ignore As String = "&Ignore"
    ''' <summary>
    ''' Yes text
    ''' </summary>
    Public Shared Yes As String = "&Yes"
    ''' <summary>
    ''' No text
    ''' </summary>
    Public Shared No As String = "&No"

    Shared Sub New()
        hookProcObj = New HookProc(AddressOf MessageBoxHookProc)
        enumProc = New EnumChildProc(AddressOf MessageBoxEnumProc)
        hHook = IntPtr.Zero
    End Sub

    ''' <summary>
    ''' Enables MessageBoxManager functionality
    ''' </summary>
    ''' <remarks>
    ''' MessageBoxManager functionality is enabled on current thread only.
    ''' Each thread that needs MessageBoxManager functionality has to call this method.
    ''' </remarks>
    Public Shared Sub Register()
        If hHook <> IntPtr.Zero Then
            Throw New NotSupportedException("One hook per thread allowed.")
        End If
        hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, hookProcObj, IntPtr.Zero, GetCurrentThreadId)
    End Sub

    <DllImport("kernel32.dll")> _
    Public Shared Function GetCurrentThreadId() As Integer
    End Function

    ''' <summary>
    ''' Disables MessageBoxManager functionality
    ''' </summary>
    ''' <remarks>
    ''' Disables MessageBoxManager functionality on current thread only.
    ''' </remarks>
    Public Shared Sub Unregister()
        If hHook <> IntPtr.Zero Then
            UnhookWindowsHookEx(hHook)
            hHook = IntPtr.Zero
        End If
    End Sub

    Private Shared Function MessageBoxHookProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        If nCode < 0 Then
            Return CallNextHookEx(hHook, nCode, wParam, lParam)
        End If

        Dim msg As CWPRETSTRUCT = CType(Marshal.PtrToStructure(lParam, GetType(CWPRETSTRUCT)), CWPRETSTRUCT)
        Dim hook As IntPtr = hHook

        If msg.message = WM_INITDIALOG Then
            Dim nLength As Integer = GetWindowTextLength(msg.hwnd)
            Dim className As New StringBuilder(10)
            GetClassName(msg.hwnd, className, className.Capacity)
            If className.ToString() = "#32770" Then
                nButton = 0
                EnumChildWindows(msg.hwnd, enumProc, IntPtr.Zero)
                If nButton = 1 Then
                    Dim hButton As IntPtr = GetDlgItem(msg.hwnd, MBCancel)
                    If hButton <> IntPtr.Zero Then
                        SetWindowText(hButton, OK)
                    End If
                End If
            End If
        End If

        Return CallNextHookEx(hook, nCode, wParam, lParam)
    End Function

    Private Shared Function MessageBoxEnumProc(ByVal hWnd As IntPtr, ByVal lParam As IntPtr) As Boolean
        Dim className As New StringBuilder(10)
        GetClassName(hWnd, className, className.Capacity)
        If className.ToString() = "Button" Then
            Dim ctlId As Integer = GetDlgCtrlID(hWnd)
            Select Case ctlId
                Case MBOK
                    SetWindowText(hWnd, OK)
                    Exit Select
                Case MBCancel
                    SetWindowText(hWnd, Cancel)
                    Exit Select
                Case MBAbort
                    SetWindowText(hWnd, Abort)
                    Exit Select
                Case MBRetry
                    SetWindowText(hWnd, Retry)
                    Exit Select
                Case MBIgnore
                    SetWindowText(hWnd, Ignore)
                    Exit Select
                Case MBYes
                    SetWindowText(hWnd, Yes)
                    Exit Select
                Case MBNo
                    SetWindowText(hWnd, No)
                    Exit Select

            End Select
            nButton += 1
        End If

        Return True
    End Function

End Class