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

Partial Public Class ImageFilter

    Public Sub GausianBlur(Optional ByVal Amount As Integer = 4)
        OnFilterStarted()
        Dim Blur As New System.Windows.Media.Effects.BlurEffect
        Blur.Radius = Amount
        b.Filters.ApplyWPFEffect(Blur)
        OnFilterFinished()

        ''THE OLD METHOD IS BELOW >>> THIS TOOK A WHILE SO I NOW USE WPF :)
        'Dim bData = BitmapData.LockBits(b)

        'Dim OutBytes As Byte()
        'ReDim OutBytes(bData.ByteData.Count - 1)

        'For ii = LBound(bData.ByteData) To UBound(bData.ByteData) Step 4
        '    'find the average color of the area around the pixel...
        '    Dim cR As Integer = 0
        '    Dim cG As Integer = 0
        '    Dim cB As Integer = 0
        '    Dim cA As Integer = 0
        '    Dim PixelCounter As Integer = 0

        '    Dim YLine = bData.YFromOffset(ii)

        '    For iY = Math.Max(YLine - Int(Amount / 2), 0) To Math.Min(YLine + Int(Amount / 2), bData.Bitmap.Height - 1)
        '        Dim StartOfLine As Integer = bData.StartOfLineFromOffset(CInt(iY * (bData.Bitmap.Width * 4)))
        '        Dim EndOfLine As Integer = StartOfLine + ((bData.Bitmap.Width * 4) - 1)
        '        Dim ThisX = ii + ((iY - YLine) * (bData.Bitmap.Width * 4))
        '        For iX As Integer = CInt(Math.Max(ThisX - (Int(Amount / 2) * 4), StartOfLine)) To CInt(Math.Min((ThisX + (Int((Amount / 2) + 1) * 4)) - 1, EndOfLine))
        '            Select Case iX Mod 4
        '                Case 0 'blue
        '                    cB += bData.ByteData(iX)
        '                Case 1 'green
        '                    cG += bData.ByteData(iX)
        '                Case 2 'red
        '                    cR += bData.ByteData(iX)
        '                Case 3 'alpha
        '                    cA += bData.ByteData(iX)
        '                    PixelCounter += 1
        '            End Select
        '        Next
        '    Next

        '    OutBytes(ii) = CByte(cB / PixelCounter) 'blue
        '    OutBytes(ii + 1) = CByte(cG / PixelCounter) 'green
        '    OutBytes(ii + 2) = CByte(cR / PixelCounter) 'red
        '    OutBytes(ii + 3) = CByte(cA / PixelCounter) 'alpha
        'Next

        'bData.ByteData = OutBytes

        'bData.UnlockBits()
    End Sub

End Class
