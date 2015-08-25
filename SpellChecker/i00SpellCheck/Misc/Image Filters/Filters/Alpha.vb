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

    Public Sub Alpha(Optional ByVal Alpha As Byte = 127)
        OnFilterStarted()
        Dim cm As New System.Drawing.Imaging.ColorMatrix
        cm.Matrix33 = CSng(Alpha / 255)
        Using ia As New System.Drawing.Imaging.ImageAttributes
            ia.SetColorMatrix(cm)
            Dim rc As New Rectangle(0, 0, b.Width, b.Height)
            Using bClone As Bitmap = DirectCast(b.Clone, Bitmap)
                Using g As Graphics = Graphics.FromImage(b)
                    g.Clear(Color.Transparent)
                    g.DrawImage(bClone, rc, 0, 0, bClone.Width, bClone.Height, GraphicsUnit.Pixel, ia)
                End Using
            End Using
        End Using
        OnFilterFinished()
    End Sub

End Class
