'i00 .Net Window Animation
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

Imports System.Collections.Generic
Imports System.Text
Imports System.Windows
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Controls
Imports System.Windows.Forms

Public Class WindowAnimation
    Inherits Form

    Dim mc_AnimationPoint As Rectangle
    Public Property AnimationPoint() As Rectangle
        Get
            Return mc_AnimationPoint
        End Get
        Set(ByVal value As Rectangle)
            mc_AnimationPoint = value
        End Set
    End Property

    Dim mc_AnimationSpeed As Integer = 250
    Public Property AnimationSpeed() As Integer
        Get
            Return mc_AnimationSpeed
        End Get
        Set(ByVal value As Integer)
            mc_AnimationSpeed = value
        End Set
    End Property

    Dim mc_AnimationEnabled As AnimationState = AnimationState.All
    Public Property AnimationEnabled() As AnimationState
        Get
            Return mc_AnimationEnabled
        End Get
        Set(ByVal value As AnimationState)
            mc_AnimationEnabled = value
        End Set
    End Property

    Dim ToOpacity As Double = 1

    Private Class AnimationObject
        Public DoubleAnimation As New DoubleAnimation
        Public Sub New(ByVal ObjectToAnimate As FrameworkElement, ByVal [Property] As DependencyProperty, ByVal From As Double?, ByVal [To] As Double?, ByVal DecelerationRatio As Double, ByVal DurationMS As Double, ByVal ReverseFromTo As Boolean)
            If ReverseFromTo Then
                DoubleAnimation.From = [To]
                DoubleAnimation.[To] = [From]
            Else
                DoubleAnimation.From = [From]
                DoubleAnimation.[To] = [To]
            End If
            DoubleAnimation.DecelerationRatio = DecelerationRatio
            DoubleAnimation.Duration = New Duration(TimeSpan.FromMilliseconds(DurationMS))
            Storyboard.SetTargetName(DoubleAnimation, ObjectToAnimate.Name)
            Storyboard.SetTargetProperty(DoubleAnimation, New PropertyPath([Property]))
        End Sub
    End Class

    Public ReadOnly Property WindowAnimationState() As AnimationState
        Get
            Return CurrentAnimationState
        End Get
    End Property

    Dim CurrentAnimationState As AnimationState
    Private Sub ShowAnimation(ByVal State As AnimationState)
        Dim AnimationSpeed = mc_AnimationSpeed
        If My.Computer.Keyboard.ShiftKeyDown Then AnimationSpeed *= 4

        CurrentAnimationState = State
        Dim ReverseFromTo As Boolean
        Select Case State
            Case AnimationState.Closing
                ReverseFromTo = True
        End Select

        AnimateWindow = New Window()
        AnimateWindow.Name = "AnimateWindow_" & Replace(Guid.NewGuid.ToString, "-", "")
        If State <> AnimationState.Closing Then
            Dim helper As New System.Windows.Interop.WindowInteropHelper(AnimateWindow)
            helper.Owner = Me.Handle
        End If
        AnimateWindow.AllowsTransparency = True
        AnimateWindow.Background = Brushes.Transparent
        AnimateWindow.WindowStyle = WindowStyle.None
        AnimateWindow.ShowInTaskbar = False

        Dim UIElementToAdd As FrameworkElement

        'Dim g As New Grid()
        Dim Animations As New List(Of AnimationObject)
        NameScope.SetNameScope(AnimateWindow, New NameScope())

        Dim AnimateDiff = 0.75

        Dim AnimationPoint As Rectangle
        If mc_AnimationPoint.IsEmpty = False Then
            AnimationPoint = mc_AnimationPoint
        Else
            AnimationPoint = New Rectangle(CInt(Me.Left + (Me.Size.Width * ((1 - AnimateDiff) / 2))), CInt(Me.Top + (Me.Size.Height * ((1 - AnimateDiff) / 2))), CInt(Me.Size.Width * AnimateDiff), CInt(Me.Size.Height * AnimateDiff))
        End If

        AnimateWindow.Left = Math.Min(AnimationPoint.Left, Me.Left) - 1
        AnimateWindow.Top = Math.Min(AnimationPoint.Top, Me.Top) - 1
        AnimateWindow.Width = (Math.Max(AnimationPoint.Right, Me.Right) - AnimateWindow.Left) + 1
        AnimateWindow.Height = (Math.Max(AnimationPoint.Bottom, Me.Bottom) - AnimateWindow.Top) + 1


        'AnimateWindow.Left = Screen.PrimaryScreen.Bounds.Left
        'AnimateWindow.Top = Screen.PrimaryScreen.Bounds.Top
        'AnimateWindow.Width = Screen.PrimaryScreen.Bounds.Width
        'AnimateWindow.Height = Screen.PrimaryScreen.Bounds.Height

        AnimationPoint.X -= CInt(AnimateWindow.Left)
        AnimationPoint.Y -= CInt(AnimateWindow.Top)

        Dim DWMEnabled As Boolean = False
        Try
            DWMEnabled = DWM.DwmIsCompositionEnabled()
        Catch ex As Exception

        End Try

        If DWMEnabled Then
            'qwertyuiop - dwm used to have an issue with this with multiple symaltanous animations ... still need more testing???

            Dim DWMThumb As New Thumbnail
            DWMThumb.Name = "DWMThumb_" & Replace(Guid.NewGuid.ToString, "-", "")
            DWMThumb.Source = Me.Handle

            UIElementToAdd = DWMThumb
        Else

            Dim ImageSource As New System.Windows.Media.Imaging.BitmapImage()
            Dim ms As New IO.MemoryStream()
            Dim b As New Bitmap(Me.Width, Me.Height)
            Me.DrawToBitmap(b, New Rectangle(0, 0, Me.Width, Me.Height))
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
            ImageSource.BeginInit()
            ImageSource.StreamSource = ms
            ImageSource.EndInit()
            'Controls for the form (Image)
            Dim FormImage As New Image()
            FormImage.Stretch = Stretch.Fill
            FormImage.Source = ImageSource
            FormImage.Name = "BThumb_" & Replace(Guid.NewGuid.ToString, "-", "")

            UIElementToAdd = FormImage
        End If

        Animations.Add(New AnimationObject(UIElementToAdd, System.Windows.FrameworkElement.OpacityProperty, 0, ToOpacity, 0.6, AnimationSpeed, ReverseFromTo))
        Animations.Add(New AnimationObject(UIElementToAdd, System.Windows.FrameworkElement.WidthProperty, AnimationPoint.Width, Me.Width, 0.6, AnimationSpeed, ReverseFromTo))
        Animations.Add(New AnimationObject(UIElementToAdd, System.Windows.FrameworkElement.HeightProperty, AnimationPoint.Height, Me.Height, 0.6, AnimationSpeed, ReverseFromTo))

        'DWMThumb.Margin = New Thickness(32, 100, 0, 0)
        AnimateWindow.RegisterName(UIElementToAdd.Name, UIElementToAdd)

        Dim g As New Grid()
        'qwertyuiop - need to do the below line as the animation window will be click-through otherwise in wpf if oargb could not be set
        Dim bgColor = System.Drawing.Color.FromArgb(4, 255, 255, 255)

        g.Background = New SolidColorBrush(Color.FromArgb(bgColor.A, bgColor.R, bgColor.G, bgColor.B))
        'g.Background = New SolidColorBrush(Color.FromArgb(127, 127, 0, 0))
        g.Children.Add(UIElementToAdd)
        g.Name = "Grid2_" & Replace(Guid.NewGuid.ToString, "-", "")
        AnimateWindow.RegisterName(g.Name, g)


        Dim c As New Canvas
        c.Name = "Canvs_" & Replace(Guid.NewGuid.ToString, "-", "")
        c.Children.Add(g)
        'c.Background = New SolidColorBrush(Color.FromArgb(127, 0, 0, 127))
        Animations.Add(New AnimationObject(g, Canvas.LeftProperty, AnimationPoint.X, Me.Left - AnimateWindow.Left, 0.6, AnimationSpeed, ReverseFromTo))
        Animations.Add(New AnimationObject(g, Canvas.TopProperty, AnimationPoint.Y, Me.Top - AnimateWindow.Top, 0.6, AnimationSpeed, ReverseFromTo))
        AnimateWindow.RegisterName(c.Name, c)

        'g.Children.Add(UIElementToAdd) 'FormImage)
        AnimateWindow.Content = c

        'STORY BOARD

        'Set the name for the story board
        AnimateWindow.RegisterName(AnimateWindow.Name, AnimateWindow)

        sb = New Storyboard
        'Add the animations
        For Each item In Animations
            sb.Children.Add(item.DoubleAnimation)
        Next
        'Timeline.SetDesiredFrameRate(sb, 60)
        Animation = Animations.First.DoubleAnimation

        'Show the window
        AnimateWindow.Show()

        'Start
        sb.Begin(AnimateWindow, True)

    End Sub

    Dim sb As Storyboard

    Private WithEvents AnimateWindow As Window

    Private WithEvents Animation As DoubleAnimation
    'Public Event Completed(ByVal sender As Object, ByVal e As System.EventArgs)

    Public Sub StopAnimation()
        If sb IsNot Nothing Then
            sb.Stop(AnimateWindow)
            Animation_Completed(Animation, EventArgs.Empty)
        End If
    End Sub

    Private Sub Animation_Completed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Animation.Completed
        Select Case CurrentAnimationState
            Case AnimationState.Opening
                Try
                    MyBase.Opacity = ToOpacity
                    Dim wasActive = AnimateWindow.IsActive
                    If wasActive Then
                        Me.Focus()
                    End If
                Catch ex As ObjectDisposedException

                End Try
            Case AnimationState.Closing
                MyBase.DialogResult = ReturnResult
                Me.Close()
            Case AnimationState.None
                'not animating don't do anything
                Return
        End Select
        AnimateWindow.Close()
        CurrentAnimationState = AnimationState.None
    End Sub

    Public Enum AnimationState
        None
        Opening = 1
        Closing = 2
        All = Opening + Closing
    End Enum

    Dim ReturnResult As DialogResult

    Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
        If (mc_AnimationEnabled And AnimationState.Closing) <> AnimationState.Closing Then Return
        Static AlreadyClosing As Boolean
        If AlreadyClosing = False Then
            MyBase.OnFormClosing(e)
            If e.Cancel = False Then
                'animate close...
                AlreadyClosing = True
                ReturnResult = MyBase.DialogResult
                e.Cancel = True
                ShowAnimation(AnimationState.Closing)
                MyBase.Opacity = 0
            End If
        End If
    End Sub

    Private Sub Form_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        If (mc_AnimationEnabled And AnimationState.Opening) <> AnimationState.Opening Then
            MyBase.Opacity = ToOpacity
            Return
        End If
        'qwertyuiop - have to doevents here otherwise the window doesn't show properly
        'Forms.Application.DoEvents()
        ShowAnimation(AnimationState.Opening)
    End Sub

    Shadows Property Opacity() As Double
        Get
            Return ToOpacity
        End Get
        Set(ByVal value As Double)
            ToOpacity = value
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        MyBase.Opacity = 0
    End Sub

End Class
