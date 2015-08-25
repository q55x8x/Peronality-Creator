Imports System.Runtime.InteropServices

Public NotInheritable Class DWM
    Private Sub New()
    End Sub

    <DllImport("dwmapi.dll", PreserveSig:=False)> _
    Public Shared Function DwmIsCompositionEnabled() As Boolean
    End Function

    <DllImport("dwmapi.dll")> _
    Public Shared Function DwmRegisterThumbnail(ByVal dest As IntPtr, ByVal source As IntPtr, <System.Runtime.InteropServices.Out()> ByRef hthumbnail As IntPtr) As Integer
    End Function

    <DllImport("dwmapi.dll")> _
    Public Shared Function DwmUnregisterThumbnail(ByVal HThumbnail As IntPtr) As Integer
    End Function

    <DllImport("dwmapi.dll")> _
    Public Shared Function DwmUpdateThumbnailProperties(ByVal HThumbnail As IntPtr, ByRef props As ThumbnailProperties) As Integer
    End Function

    <DllImport("dwmapi.dll")> _
    Public Shared Function DwmQueryThumbnailSourceSize(ByVal HThumbnail As IntPtr, <System.Runtime.InteropServices.Out()> ByRef size As Size) As Integer
    End Function

    Public Structure Point
        Public x As Integer
        Public y As Integer
    End Structure

    Public Structure Size
        Public Width As Integer, Height As Integer
    End Structure

    Public Structure WindowPlacement
        Public Length As Integer
        Public Flags As Integer
        Public ShowCmd As Integer
        Public MinPosition As Point
        Public MaxPosition As Point
        Public NormalPosition As Rect
    End Structure

    Public Structure ThumbnailProperties
        Public Flags As ThumbnailFlags
        Public Destination As Rect
        Public Source As Rect
        Public Opacity As [Byte]
        Public Visible As Boolean
        Public SourceClientAreaOnly As Boolean
    End Structure

    Public Structure Rect
        Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal x1 As Integer, ByVal y1 As Integer)
            Me.Left = x
            Me.Top = y
            Me.Right = x1
            Me.Bottom = y1
        End Sub

        Public Left As Integer, Top As Integer, Right As Integer, Bottom As Integer
    End Structure

    <Flags()> _
    Public Enum ThumbnailFlags As Integer
        RectDetination = 1
        RectSource = 2
        Opacity = 4
        Visible = 8
        SourceClientAreaOnly = 16
    End Enum

    Public Enum GetWindowCmd As UInteger
        First = 0
        Last = 1
        [Next] = 2
        Prev = 3
        Owner = 4
        Child = 5
        EnabledPopup = 6
    End Enum
End Class
