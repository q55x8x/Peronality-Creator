<System.ComponentModel.DesignerCategory("UserControl")> _
Public Class RTBHighlight
    Inherits RichTextBox

#Region "Scrollbar location"

    Private Overloads Declare Auto Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As ScrollPoint) As IntPtr

    <Runtime.InteropServices.DllImport("user32.dll")> _
    Private Shared Function GetScrollPos(ByVal hwnd As IntPtr, ByVal nBar As Integer) As Integer
    End Function

    <Runtime.InteropServices.DllImport("user32.dll")> _
    Public Shared Function LockWindowUpdate(ByVal hWndLock As IntPtr) As Boolean
    End Function

    Private Const SB_HORZ As Integer = 0
    Private Const SB_VERT As Integer = 1
    Private Const SB_TOP As Integer = 6
    Private Const EM_SCROLL As Integer = &HB5
    Private Const EM_LINESCROLL As Integer = &HB6

    Private Enum EMFlags
        EM_SETSCROLLPOS = &H400 + 222
    End Enum

    <System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
    Private Class ScrollPoint
        Public x As Integer
        Public y As Integer
        Public Sub New(ByVal ScrollPos As Point)
            Me.x = ScrollPos.X
            Me.y = ScrollPos.Y
        End Sub
    End Class

#End Region

#Region "Background Color"

    <System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
    Private Structure RTBFormat
        Public cbSize As Int32
        Public dwMask As Int32
        Public dwEffects As Int32
        Public yHeight As Int32
        Public yOffset As Int32
        Public crTextColor As Int32
        Public bCharSet As Byte
        Public bPitchAndFamily As Byte
        <System.Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=32)> _
        Public szFaceName As String
        Public wWeight As Int16
        Public sSpacing As Int16
        Public crBackColor As Int32
        Public lcid As Int32
        Public dwReserved As Int32
        Public sStyle As Int16
        Public wKerning As Int16
        Public bUnderlineType As Byte
        Public bAnimation As Byte
        Public bRevAuthor As Byte
        Public bReserved1 As Byte
    End Structure

    Private Const CFM_BACKCOLOR As Integer = &H4000000
    Private Const WM_USER As Integer = &H400
    Private Const EM_SETCHARFORMAT As Integer = (WM_USER + 68)
    Private Const SCF_SELECTION As Integer = &H1&

    Private Overloads Declare Auto Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As Integer, ByRef lParam As RTBFormat) As Boolean

#End Region

    Public HighlightWords As New List(Of String)

    Public Sub ClearHighlights(Optional ByVal LockWindowUpdates As Boolean = True)
        If LockWindowUpdates Then
            LockWindowUpdate(Me.Handle)
            Me.SuspendLayout()
        End If

        FireTextChanged = False

        Dim OldScrollPos = New Point(GetScrollPos(Me.Handle, SB_HORZ), GetScrollPos(Me.Handle, SB_VERT))
        Dim SelectionStart = Me.SelectionStart
        Dim SelectionLength = Me.SelectionLength

        Me.SelectAll()
        Dim Format As New RTBFormat
        Format.crBackColor = -1
        Format.dwMask = CFM_BACKCOLOR
        Format.dwEffects = CFM_BACKCOLOR
        Format.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(Format)
        SendMessage(Me.Handle, EM_SETCHARFORMAT, SCF_SELECTION, Format)
        Me.SelectionColor = Me.ForeColor

        Me.SelectionStart = SelectionStart
        Me.SelectionLength = SelectionLength
        SendMessage(Me.Handle, EMFlags.EM_SETSCROLLPOS, 0, New ScrollPoint(OldScrollPos))

        FireTextChanged = True

        If LockWindowUpdates Then
            Me.ResumeLayout()
            LockWindowUpdate(IntPtr.Zero)
        End If
    End Sub

    Dim FireTextChanged As Boolean = True
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        If FireTextChanged Then
            MyBase.OnTextChanged(e)
        End If
    End Sub

    Public Function UpdateHighlights() As Integer
        LockWindowUpdate(Me.Handle)
        Me.SuspendLayout()

        Dim OldScrollPos = New Point(GetScrollPos(Me.Handle, SB_HORZ), GetScrollPos(Me.Handle, SB_VERT))
        Dim SelectionStart = Me.SelectionStart
        Dim SelectionLength = Me.SelectionLength

        ClearHighlights(False)

        FireTextChanged = False

        Dim SearchIndex As Integer
        For Each item In HighlightWords
            SearchIndex = 0
            While Me.Find(item, SearchIndex, RichTextBoxFinds.WholeWord) > -1
                UpdateHighlights += 1
                Me.SelectionColor = Color.FromKnownColor(KnownColor.HighlightText)
                Me.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                SearchIndex = Me.SelectionStart + Me.SelectionLength
                If SearchIndex >= Me.TextLength Then
                    Exit While
                End If
            End While
        Next

        Me.SelectionStart = SelectionStart
        Me.SelectionLength = SelectionLength
        SendMessage(Me.Handle, EMFlags.EM_SETSCROLLPOS, 0, New ScrollPoint(OldScrollPos))

        FireTextChanged = True

        Me.ResumeLayout()
        LockWindowUpdate(IntPtr.Zero)
    End Function

End Class
