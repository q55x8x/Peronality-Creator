Imports System
Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Interop

Public Class Thumbnail
    Inherits FrameworkElement
  
    Public Shared SourceProperty As DependencyProperty
    Public Shared ClientAreaOnlyProperty As DependencyProperty

    Shared Sub SourceCallback(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        DirectCast(obj, Thumbnail).InitialiseThumbnail(DirectCast(args.NewValue, IntPtr))
    End Sub

    Shared Sub ClientAreaOnlyCallback(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        DirectCast(obj, Thumbnail).UpdateThumbnail()
    End Sub

    Shared Sub OpacityCallback(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
        DirectCast(obj, Thumbnail).UpdateThumbnail()
    End Sub

    Shared Sub New()
        SourceProperty = DependencyProperty.Register("Source", GetType(IntPtr), GetType(Thumbnail), New FrameworkPropertyMetadata(IntPtr.Zero, FrameworkPropertyMetadataOptions.AffectsMeasure, AddressOf SourceCallback))

        ClientAreaOnlyProperty = DependencyProperty.Register("ClientAreaOnly", GetType(Boolean), GetType(Thumbnail), New FrameworkPropertyMetadata(False, FrameworkPropertyMetadataOptions.AffectsMeasure, AddressOf ClientAreaOnlyCallback))

        OpacityProperty.OverrideMetadata(GetType(Thumbnail), New FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.[Inherits], AddressOf OpacityCallback))
    End Sub

    Public Property Source() As IntPtr
        Get
            Return DirectCast(Me.GetValue(SourceProperty), IntPtr)
        End Get
        Set(ByVal value As IntPtr)
            Me.SetValue(SourceProperty, value)
        End Set
    End Property

    Public Property ClientAreaOnly() As Boolean
        Get
            Return CBool(Me.GetValue(ClientAreaOnlyProperty))
        End Get
        Set(ByVal value As Boolean)
            Me.SetValue(ClientAreaOnlyProperty, value)
        End Set
    End Property

    Public Shadows Property Opacity() As Double
        Get
            Return CDbl(Me.GetValue(OpacityProperty))
        End Get
        Set(ByVal value As Double)
            Me.SetValue(OpacityProperty, value)
        End Set
    End Property

    Private target As HwndSource
    Private thumb As IntPtr

    Private Sub InitialiseThumbnail(ByVal source As IntPtr)
        If IntPtr.Zero <> thumb Then
            ' release the old thumbnail
            ReleaseThumbnail()
        End If

        If IntPtr.Zero <> source Then
            ' find our parent hwnd
            target = DirectCast(HwndSource.FromVisual(Me), HwndSource)

            ' if we have one, we can attempt to register the thumbnail
            If target IsNot Nothing AndAlso 0 = DWM.DwmRegisterThumbnail(target.Handle, source, Me.thumb) Then
                Dim props As New DWM.ThumbnailProperties()
                props.Visible = False
                props.SourceClientAreaOnly = Me.ClientAreaOnly
                props.Opacity = CByte(255 * Me.Opacity)
                props.Flags = DWM.ThumbnailFlags.Visible Or DWM.ThumbnailFlags.SourceClientAreaOnly Or DWM.ThumbnailFlags.Opacity
                DWM.DwmUpdateThumbnailProperties(thumb, props)
            End If
        End If
    End Sub

    Private Sub ReleaseThumbnail()
        DWM.DwmUnregisterThumbnail(thumb)
        Me.thumb = IntPtr.Zero
        Me.target = Nothing
    End Sub

    Private Sub UpdateThumbnail()
        If IntPtr.Zero <> thumb Then
            Dim props As New DWM.ThumbnailProperties()
            props.SourceClientAreaOnly = Me.ClientAreaOnly
            props.Opacity = CByte(255 * Me.Opacity)
            props.Flags = DWM.ThumbnailFlags.SourceClientAreaOnly Or DWM.ThumbnailFlags.Opacity
            DWM.DwmUpdateThumbnailProperties(thumb, props)
        End If
    End Sub

    Protected Overrides Function MeasureOverride(ByVal availableSize As Size) As Size
        Dim size As DWM.Size
        DWM.DwmQueryThumbnailSourceSize(Me.thumb, size)

        Dim scale As Double = 1

        ' our preferred size is the thumbnail source size
        ' if less space is available, we scale appropriately
        If size.Width > availableSize.Width Then
            scale = availableSize.Width / size.Width
        End If
        If size.Height > availableSize.Height Then
            scale = Math.Min(scale, availableSize.Height / size.Height)
        End If

        Return New Size(size.Width * scale, size.Height * scale)


    End Function

    Protected Overrides Function ArrangeOverride(ByVal finalSize As Size) As Size
        Dim size As DWM.Size
        DWM.DwmQueryThumbnailSourceSize(Me.thumb, size)

        ' scale to fit whatever size we were allocated
        Dim scale As Double = finalSize.Width / size.Width
        scale = Math.Min(scale, finalSize.Height / size.Height)

        Return New Size(size.Width * scale, size.Height * scale)
    End Function

    Private Sub Thumbnail_LayoutUpdated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LayoutUpdated
        If IntPtr.Zero = thumb Then
            InitialiseThumbnail(Me.Source)
        End If

        If IntPtr.Zero <> thumb Then
            'If target.RootVisual IsNot Nothing Then
            If target.RootVisual Is Nothing OrElse Not target.RootVisual.IsAncestorOf(Me) Then
                'we are no longer in the visual tree
                ReleaseThumbnail()
                Return
            End If

            Dim transform As GeneralTransform = TransformToAncestor(target.RootVisual)
            Dim a As Point = transform.Transform(New Point(0, 0))
            Dim b As Point = transform.Transform(New Point(Me.ActualWidth, Me.ActualHeight))

            Dim props As New DWM.ThumbnailProperties()
            props.Visible = True
            If Double.IsNaN(a.X) OrElse Double.IsNaN(a.Y) OrElse Double.IsNaN(b.X) OrElse Double.IsNaN(b.Y) Then
            Else
                props.Destination = New DWM.Rect(CInt(a.X), CInt(a.Y), CInt(b.X), CInt(b.Y))
                props.Flags = DWM.ThumbnailFlags.Visible Or DWM.ThumbnailFlags.RectDetination
                DWM.DwmUpdateThumbnailProperties(thumb, props)
            End If
            'End If
        End If
    End Sub

    Private Sub Thumbnail_Unloaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        ReleaseThumbnail()
    End Sub
End Class
