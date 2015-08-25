'i00 .Net Spell Check
'©i00 Productions All rights reserved
'Created by Kris Bennett
'----------------------------------------------------------------------------------------------------
'All property in this file is and remains the property of i00 Productions, regardless of its usage,
'unless stated otherwise in writing from i00 Productions.
'
'Anyone wishing to use this code in their projects may do so, however are required to leave a post on
'VBForums (under: http://www.vbforums.com/showthread.php?p=4075093) stating that they are doing so.
'A simple "I am using i00 Spell check in my project" will surffice.
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Partial Class SpellCheckTextBox

#Region "Repaint events"

    Private Sub SpellCheckTextBox_SettingsChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SettingsChanged
        RepaintControl()
    End Sub

#End Region

#Region "Drawing"

#Region "Repaint"

    Public Overrides Sub RepaintControl()
        If parentTextBox IsNot Nothing Then
            If Me.RenderCompatibility Then
                If OKToDraw Then
                    parentTextBox.Invalidate()
                End If
            Else
                'only redraw if the overlay hasn't been created yet or if the overlay is visible
                If DrawOverlayForm Is Nothing Then
                    OpenOverlay()
                End If
                If OKToDraw Then
                    DrawOverlayFormVisible = parentTextBox.Visible
                Else
                    DrawOverlayFormVisible = False
                End If
                If DrawOverlayFormVisible Then
                    Me.CustomPaint()
                End If
            End If
        End If
    End Sub

#End Region

#Region "Painting"

    Private Sub CustomPaint()
        If OKToDraw = False OrElse parentTextBox.ClientSize.Width = 0 Then Exit Sub

        Dim TextHeight As Integer = System.Windows.Forms.TextRenderer.MeasureText("Ag", parentTextBox.Font).Height
        Dim BufferWidth As Integer = System.Windows.Forms.TextRenderer.MeasureText("--", parentTextBox.Font).Width

        'for drawing underlines below the textbox drawing bounds when on a single line text box
        Dim bHeight = parentTextBox.ClientSize.Height
        If DrawOverlayForm IsNot Nothing Then
            bHeight = DrawOverlayForm.Height
        End If

        Using b As New Bitmap(parentTextBox.ClientSize.Width, bHeight)
            Using g = Graphics.FromImage(b)
                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

                'Using sb As New SolidBrush(Color.FromArgb(127, Color.Blue))
                '    g.FillRectangle(sb, New RectangleF(0, 0, parentTextBox.Width, parentTextBox.Height))
                'End Using

                Dim FromChar = parentTextBox.GetCharIndexFromPosition(New Point(0, 0))
                Dim ToChar = parentTextBox.GetCharIndexFromPosition(New Point(parentTextBox.ClientRectangle.Width - 1, parentTextBox.ClientRectangle.Height - 1))

                Dim theText As String = Dictionary.Formatting.RemoveWordBreaks(parentTextBox.Text)

                Dim LetterIndex = FromChar
                Dim LeftSide = Left(theText, FromChar)
                LeftSide = LeftSide.Split(" "c).Last
                Dim RightSide = Right(theText, Len(theText) - ToChar)
                RightSide = RightSide.Split(" "c).First
                FromChar -= Len(LeftSide)
                ToChar += Len(RightSide)

                Dim VisibleText = Mid(theText, FromChar + 1, ToChar - FromChar)

                'If parentTextBox.Multiline = False Then
                'g.TranslateTransform(-System.Windows.Forms.TextRenderer.MeasureText(LeftSide, parentTextBox.Font).Width, -5)
                'End If

                Dim NewWords As New Dictionary(Of String, Dictionary.SpellCheckWordError)

                If Trim(VisibleText) <> "" Then
                    Dim words = Replace(Replace(VisibleText, vbCr, " "), vbLf, " ").Split(" "c)

                    For iWord = LBound(words) To UBound(words)
                        If words(iWord) <> "" Then
                            Dim P1 = parentTextBox.GetPositionFromCharIndex(LetterIndex)
                            Dim P1OffsetPlus As Integer = 0
                            If iWord = 0 AndAlso P1.X >= parentTextBox.ClientSize.Width Then
                                P1OffsetPlus = 1
                                P1.X = 0
                            End If
                            If P1.Y < parentTextBox.Height Then
                                Dim WordState As Dictionary.SpellCheckWordError = Dictionary.SpellCheckWordError.SpellError
                                If dictCache.ContainsKey(words(iWord)) Then
                                    'load from cache
                                    WordState = dictCache(words(iWord))
                                Else
                                    ''item is not in the dict cache
                                    If NewWords.ContainsKey(words(iWord)) = False Then
                                        NewWords.Add(words(iWord), Dictionary.SpellCheckWordError.OK)
                                    End If

                                    WordState = Dictionary.SpellCheckWordError.OK
                                End If
                                If WordState = Dictionary.SpellCheckWordError.OK Then

                                Else
                                    If WordState = Dictionary.SpellCheckWordError.Ignore Then
                                        If DrawIgnored() = False Then GoTo ContinueFor
                                    End If

                                    Dim P2 = parentTextBox.GetPositionFromCharIndex(LetterIndex + Len(words(iWord)))
                                    If LeftSide <> "" AndAlso iWord = 0 Then
                                        Dim NormalStringWidth = g.MeasureString(Mid(words(iWord), Len(LeftSide) + 1 + P1OffsetPlus), parentTextBox.Font).Width
                                        Dim XOffsetDiff = g.MeasureString("-" & Mid(words(iWord), Len(LeftSide) + 1 + P1OffsetPlus) & "-", parentTextBox.Font).Width - NormalStringWidth
                                        P2.X = CInt(parentTextBox.GetPositionFromCharIndex(LetterIndex + P1OffsetPlus).X + (NormalStringWidth - XOffsetDiff))
                                    End If
                                    If P2.X = 0 Then
                                        'we are the last char ... :(
                                        P2 = parentTextBox.GetPositionFromCharIndex(LetterIndex + Len(words(iWord)) - 1)
                                        P2.X += System.Windows.Forms.TextRenderer.MeasureText("-" & Right(words(iWord), 1) & "-", parentTextBox.Font).Width - BufferWidth
                                    End If
                                    Dim LineHeight As Integer = extTextBoxCommon.GetLineHeightFromCharPosition(parentTextBox, LetterIndex, RTBContents)
                                    'P1.Y += LineHeight
                                    P2.Y = P1.Y + LineHeight
                                    'P2.Y = P1.Y

                                    Dim e = New SpellCheckCustomPaintEventArgs With {.Graphics = g, .Word = words(iWord), .Bounds = New Rectangle(P1.X, P1.Y, P2.X - P1.X, P2.Y - P1.Y), .WordState = WordState}
                                    OnSpellCheckErrorPaint(e)
                                    If e.DrawDefault Then
                                        Select Case WordState
                                            Case Dictionary.SpellCheckWordError.Ignore
                                                Using p As New Pen(Settings.IgnoreColor)
                                                    g.DrawLine(p, P1.X, P2.Y + 1, P2.X, P2.Y + 1)
                                                End Using
                                            Case Dictionary.SpellCheckWordError.CaseError
                                                DrawingFunctions.DrawWave(g, P1, P2, Settings.CaseMistakeColor)
                                            Case Dictionary.SpellCheckWordError.SpellError
                                                DrawingFunctions.DrawWave(g, P1, P2, Settings.MistakeColor)
                                        End Select
                                    End If
                                End If
                            End If
                        End If
ContinueFor:
                        If LeftSide <> "" AndAlso iWord = 0 Then
                            LetterIndex -= Len(LeftSide)
                        End If
                        LetterIndex += 1 + Len(words(iWord))
                    Next
                End If

Draw:
                If DrawOverlayForm IsNot Nothing Then
                    DrawOverlayForm.SetBitmap(b, 255)
                Else
                    Dim textBoxGraphics = Graphics.FromHwnd(parentTextBox.Handle)
                    textBoxGraphics.DrawImageUnscaled(b, 0, 0)
                End If

                If NewWords.Count > 0 Then
                    AddWordsToCache(NewWords)
                End If

            End Using
        End Using

    End Sub

#End Region

#End Region

#Region "Overlay"

    Dim DrawOverlayForm As PerPixelAlphaForm

#Region "Create"

#Region "APIs for click through"

    <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function GetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer) As Integer
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll", SetLastError:=True)> _
    Private Shared Function SetWindowLong(ByVal hWnd As IntPtr, ByVal nIndex As Integer, ByVal dwNewLong As Integer) As Integer
    End Function

    Private Const GWL_EXSTYLE As Integer = -20
    Private Const WS_EX_TRANSPARENT As Integer = &H20

#End Region

    Private Sub SetOverlayBounds()
        If DrawOverlayForm IsNot Nothing Then
            'for drawing underlines below the textbox drawing bounds when on a single line text box
            Dim toBeHeight = Me.parentTextBox.ClientSize.Height
            If parentTextBox.Multiline = False Then
                'add wave height
                toBeHeight += 3
            End If
            DrawOverlayForm.Bounds = Me.parentTextBox.RectangleToScreen(New Rectangle(0, 0, Me.parentTextBox.ClientSize.Width, toBeHeight))
        End If
    End Sub

    Private Sub OpenOverlay()
        If DrawOverlayForm Is Nothing Then
            DrawOverlayForm = New PerPixelAlphaForm

            DrawOverlayForm.ShowInTaskbar = False
            DrawOverlayForm.ShowNoFocus(Me.parentTextBox.Parent)

            'make the overlay click-through
            Dim exstyle2 As Integer = GetWindowLong(DrawOverlayForm.Handle, GWL_EXSTYLE)
            exstyle2 = exstyle2 Or WS_EX_TRANSPARENT
            SetWindowLong(DrawOverlayForm.Handle, GWL_EXSTYLE, exstyle2)

            SetOverlayBounds()
            mc_parentTextBox_ParentChanged(Me.parentTextBox, EventArgs.Empty)
        End If
    End Sub

#End Region

#Region "Destory"

    Private Sub CloseOverlay()
        If DrawOverlayForm IsNot Nothing Then
            DrawOverlayForm.Close()
            DrawOverlayForm.Dispose()
            DrawOverlayForm = Nothing
            RemoveAllOverlayHandlers()
        End If
    End Sub

    Private Sub parentTextBox_ForOverlay_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentTextBox.Disposed
        CloseOverlay()
    End Sub

#End Region

#Region "Stuff for overlay"

    Private Sub parentTextBox_ForOverlay_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentTextBox.SizeChanged
        If DrawOverlayForm IsNot Nothing Then
            SetOverlayBounds()
            RepaintControl()
        End If
    End Sub

    Private Sub mc_parentTextBox_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentTextBox.LocationChanged
        SetOverlayBounds()
    End Sub

    Private Sub mc_parentTextBox_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentTextBox.VisibleChanged
        If DrawOverlayForm IsNot Nothing AndAlso DrawOverlayForm.IsDisposed = False Then
            If DrawOverlayFormVisible <> parentTextBox.Visible Then
                DrawOverlayFormVisible = parentTextBox.Visible
                If parentTextBox.Visible Then
                    SetOverlayBounds()
                    RepaintControl()
                End If
            End If
        End If
    End Sub

    Private Sub RemoveAllOverlayHandlers()
        For Each item In OverlayHandlerControls
            RemoveHandler item.LocationChanged, AddressOf mc_parentTextBox_LocationChanged
            RemoveHandler item.VisibleChanged, AddressOf mc_parentTextBox_VisibleChanged
            RemoveHandler item.ControlRemoved, AddressOf parents_ControlRemoved
        Next
        OverlayHandlerControls = New List(Of Control)
    End Sub

    Public OverlayHandlerControls As New List(Of Control)

    Private Sub parents_ControlRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs)
        If OverlayHandlerControls.Contains(e.Control) Then
            mc_parentTextBox_ParentChanged(Me.parentTextBox, EventArgs.Empty)
        End If
    End Sub

    Private Property DrawOverlayFormVisible() As Boolean
        Get
            Return DrawOverlayForm IsNot Nothing AndAlso DrawOverlayForm.Visible
        End Get
        Set(ByVal value As Boolean)
            'need to do it this way as the control may be visible with no parent
            If DrawOverlayForm IsNot Nothing Then
                If value = True Then
                    If TypeOf parentTextBox.TopLevelControl Is Form Then
                        DrawOverlayForm.Visible = True
                    Else
                        DrawOverlayForm.Visible = False
                    End If
                Else
                    DrawOverlayForm.Visible = False
                End If
            End If
        End Set
    End Property

    Private Sub mc_parentTextBox_ParentChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles parentTextBox.ParentChanged
        If DrawOverlayForm IsNot Nothing Then
            If DrawOverlayForm.Owner IsNot Me.parentTextBox.Parent Then
                CloseOverlay()
                OpenOverlay()
            Else
                SetOverlayBounds()

                Dim parentControl As Control = Me.parentTextBox.Parent
                RemoveAllOverlayHandlers()
                Do Until parentControl Is Nothing
                    OverlayHandlerControls.Add(parentControl)
                    AddHandler parentControl.LocationChanged, AddressOf mc_parentTextBox_LocationChanged
                    AddHandler parentControl.VisibleChanged, AddressOf mc_parentTextBox_VisibleChanged
                    AddHandler parentControl.ControlRemoved, AddressOf parents_ControlRemoved
                    parentControl = parentControl.Parent
                Loop
                Try
                    DrawOverlayFormVisible = parentTextBox.Visible
                Catch ex As ObjectDisposedException

                End Try
            End If
        End If
    End Sub

#End Region

#End Region

End Class