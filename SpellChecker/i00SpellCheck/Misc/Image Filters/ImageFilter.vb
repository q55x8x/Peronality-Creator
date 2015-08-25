'i00 .Net Image Filters
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

Public Class ImageFilter
    Dim b As Bitmap
    Public Sub New(ByVal b As Bitmap)
        Me.b = b
    End Sub

    Public Shared Sub ConvertToSupportLockBits(ByRef b As Bitmap)
        Dim bClone As New Bitmap(b.Width, b.Height, Imaging.PixelFormat.Format32bppArgb)
        Using g = Graphics.FromImage(bClone)
            b.SetResolution(96, 96)
            g.DrawImage(b, Point.Empty)
        End Using
        b.Dispose()
        b = bClone
    End Sub

    Public Shared Function SupportsLockBits(ByVal b As Bitmap) As Boolean
        Return Bitmap.GetPixelFormatSize(b.PixelFormat) = 32 AndAlso b.PixelFormat <> Imaging.PixelFormat.Indexed
    End Function

#Region "Bitmap Data"

    Public Class BitmapData
        Implements IDisposable

#Region "IDisposable"

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    UnlockBits()
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

        Public Bitmap As Bitmap
        Public ByteData As Byte()
        Public BitmapData As Imaging.BitmapData

        Private Sub New()

        End Sub

        Public Shared Function LockBits(ByVal b As Bitmap) As BitmapData
            If SupportsLockBits(b) = False Then
                Throw New Exception("Your bitmap must be 32bit non-indexed in order to apply filters")
            End If
            LockBits = New BitmapData

            LockBits.Bitmap = b

            Dim bmpRect As New Rectangle(0, 0, b.Width, b.Height)
            ReDim LockBits.ByteData(b.Width * b.Height * 4 - 1)

            LockBits.BitmapData = b.LockBits(bmpRect, Imaging.ImageLockMode.ReadWrite, b.PixelFormat)
            System.Runtime.InteropServices.Marshal.Copy(LockBits.BitmapData.Scan0, LockBits.ByteData, 0, LockBits.ByteData.Length)
        End Function

        Public Sub UnlockBits()
            System.Runtime.InteropServices.Marshal.Copy(Me.ByteData, 0, Me.BitmapData.Scan0, Me.ByteData.Length)
            Me.Bitmap.UnlockBits(Me.BitmapData)
        End Sub

        Public Function YFromOffset(ByVal Offset As Integer) As Integer
            Return CInt(Int(Offset / (Bitmap.Width * 4)))
        End Function

        Public Function StartOfLineFromOffset(ByVal Offset As Integer) As Integer
            Return Offset - (Offset Mod (Bitmap.Width * 4))
        End Function

    End Class

#End Region

    Private Sub Clear()
        Using bData = BitmapData.LockBits(b)

            Dim OutBytes As Byte()
            ReDim OutBytes(bData.ByteData.Count - 1)
            bData.ByteData = OutBytes

        End Using
    End Sub

    Private Function BlendByte(ByVal From As Byte, ByVal [To] As Byte, ByVal Amount As Byte) As Byte
        Return CByte(((CInt([To]) - [From]) * (Amount / 255)) + [From])
    End Function

#Region "Filters"

    Public Event FilterStarted(ByVal sender As Object, ByVal e As EventArgs)
    Public Event FilterFinished(ByVal sender As Object, ByVal e As EventArgs)

    Protected Overridable Sub OnFilterStarted()
        RaiseEvent FilterStarted(Me, EventArgs.Empty)
    End Sub

    Protected Overridable Sub OnFilterFinished()
        RaiseEvent FilterFinished(Me, EventArgs.Empty)
    End Sub

#End Region

End Class