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

    Public Sub ApplyWPFEffect(ByVal Effect As System.Windows.Media.Effects.Effect)
        OnFilterStarted()
        Dim ImageSource As New System.Windows.Media.Imaging.BitmapImage()
        Using ms As New IO.MemoryStream()
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            ImageSource.BeginInit()
            ImageSource.StreamSource = ms
            ImageSource.EndInit()
            Dim FormImage As New System.Windows.Controls.Image()
            FormImage.Effect = Effect
            FormImage.Source = ImageSource
            Dim ImageSize = New System.Windows.Size(b.Width, b.Height)
            FormImage.Measure(ImageSize)
            Dim renderingRectangle As New System.Windows.Rect(ImageSize)
            FormImage.Arrange(renderingRectangle)


            Dim enc As New System.Windows.Media.Imaging.PngBitmapEncoder
            Dim bmpSource As New System.Windows.Media.Imaging.RenderTargetBitmap(b.Width, b.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32)
            bmpSource.Render(FormImage)
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmpSource))
            Using msOut As New IO.MemoryStream
                enc.Save(msOut)
                Using bOut As New Bitmap(msOut)
                    Using g As Graphics = Graphics.FromImage(b)
                        g.Clear(Color.Transparent)
                        g.DrawImageUnscaled(bOut, New Point(0, 0))
                    End Using
                End Using
            End Using
        End Using
        OnFilterFinished()
    End Sub

End Class
