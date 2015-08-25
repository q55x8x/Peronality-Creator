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

    Public Sub AlphaMask(ByVal AlphaColor As Color, ByVal SolidColor As Color)
        OnFilterStarted()
        Using bData = BitmapData.LockBits(b)
            For ii = LBound(bData.ByteData) To UBound(bData.ByteData) Step 4
                Dim AlphaByte = bData.ByteData(ii + 3)
                bData.ByteData(ii) = BlendByte(AlphaColor.B, SolidColor.B, AlphaByte) 'blue
                bData.ByteData(ii + 1) = BlendByte(AlphaColor.G, SolidColor.G, AlphaByte)  'green
                bData.ByteData(ii + 2) = BlendByte(AlphaColor.R, SolidColor.R, AlphaByte)  'red
                bData.ByteData(ii + 3) = BlendByte(AlphaColor.A, SolidColor.A, AlphaByte)  'alpha
            Next
        End Using
        OnFilterFinished()
    End Sub

End Class
