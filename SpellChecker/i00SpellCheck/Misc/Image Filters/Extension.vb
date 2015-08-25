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

Public Module ext_ImageFilters

    Public BitmapsWithFilters As New Dictionary(Of Bitmap, ImageFilter)

    <System.Runtime.CompilerServices.Extension()> _
    Public Function Filters(ByVal b As Bitmap) As ImageFilter
        If BitmapsWithFilters.ContainsKey(b) Then
            Return BitmapsWithFilters(b)
        Else
            Dim ImageFilter = New ImageFilter(b)
            BitmapsWithFilters.Add(b, ImageFilter)
            Return ImageFilter
        End If
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function Clone(ByVal b As Image, ByVal Width As Integer, ByVal Height As Integer) As Bitmap
        Clone = New Bitmap(Width, Height)
        Using g = Graphics.FromImage(Clone)
            g.InterpolationMode = Drawing2D.InterpolationMode.High
            g.DrawImage(b, New Rectangle(0, 0, Clone.Width, Clone.Height))
        End Using
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function Clone(ByVal b As Image, ByVal Size As Size) As Bitmap
        Return b.Clone(Size.Width, Size.Height)
    End Function

End Module