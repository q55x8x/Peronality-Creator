'i00 .Net Control Extensions - OSControlRenderer
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

'Limitations:
'----------------------------------------------------------------------------------------------------
'LinkLabel
'   Does not provide support for a LinkArea that is not the full LinkLabel text
'   Does not support images
'CheckBox / RadioButton
'   FlatStyle is not implemented
'   Does not support images

'Improvements:
'----------------------------------------------------------------------------------------------------
'CheckBox / RadioButton
'   Disabled text color reflects control text color (would always be Gray on standard button base)

Imports i00SpellCheck

Public Class SelectedControlHighlight
    Inherits ControlExtension
    Implements iTestHarness

#Region "Plugin Info"

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(TextBoxBase), GetType(CheckBox), GetType(Button), GetType(ComboBox), GetType(DateTimePicker), GetType(LinkLabel), GetType(MonthCalendar), GetType(UpDownBase), GetType(RadioButton), GetType(ListBox), GetType(ListView), GetType(TreeView)}
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Overrides Sub Load()
        If TypeOf (Control) Is CheckBox OrElse TypeOf (Control) Is RadioButton Then
            ButtonBase = TryCast(Control, ButtonBase)
        End If
        Label = TryCast(Control, Label)
        ctl = Control
    End Sub

#End Region

#Region "Properties"

    Dim mc_Amount As Integer = 4
    <System.ComponentModel.DefaultValue(4)> _
    <System.ComponentModel.Description("The blur amount for the control highlight")> _
    Public Property Amount() As Integer
        Get
            Return mc_Amount
        End Get
        Set(ByVal value As Integer)
            mc_Amount = value
        End Set
    End Property

    Dim mc_Depth As Integer = 0
    <System.ComponentModel.DefaultValue(0)> _
    <System.ComponentModel.Description("The offset for the control highlight")> _
    Public Property Depth() As Integer
        Get
            Return mc_Depth
        End Get
        Set(ByVal value As Integer)
            mc_Depth = value
        End Set
    End Property

    Dim mc_Color As Color = Color.FromKnownColor(KnownColor.Highlight)
    <System.ComponentModel.DefaultValue(GetType(Color), "Highlight")> _
    <System.ComponentModel.Description("Sets the color of the control highlight")> _
    Public Property Color() As Color
        Get
            Return mc_Color
        End Get
        Set(ByVal value As Color)
            mc_Color = value
        End Set
    End Property

    Dim mc_Enabled As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.Description("Enables the highlighting of controls when they get focus")> _
    Public Property Enabled() As Boolean
        Get
            Return mc_Enabled
        End Get
        Set(ByVal value As Boolean)
            mc_Enabled = value
        End Set
    End Property

#End Region

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

#Region "Control"

    Private WithEvents ctl As Control

    Dim DrawOverlayForm As i00SpellCheck.PerPixelAlphaForm

    Private Sub RenderControlForOverlay(ByVal g As Graphics, ByVal OffsetX As Integer, ByVal OffsetY As Integer)
        If (TypeOf (Control) Is CheckBox AndAlso DirectCast(Control, CheckBox).Appearance = Appearance.Normal) OrElse (TypeOf (Control) Is RadioButton AndAlso DirectCast(Control, RadioButton).Appearance = Appearance.Normal) Then
            RenderButtonBase(g, OffsetX, OffsetX, True)
        ElseIf TypeOf (Control) Is TextBoxBase AndAlso (Not TypeOf (Control) Is RichTextBox) Then
            TextBoxRenderer.DrawTextBox(g, New Rectangle(OffsetX, OffsetY, Control.Width, Control.Height), VisualStyles.TextBoxState.Selected)
        ElseIf TypeOf (Control) Is ComboBox AndAlso (DirectCast(Control, ComboBox).FlatStyle = FlatStyle.Standard OrElse DirectCast(Control, ComboBox).FlatStyle = FlatStyle.System) Then
            ComboBoxRenderer.DrawTextBox(g, New Rectangle(OffsetX, OffsetY, Control.Width, Control.Height), VisualStyles.ComboBoxState.Hot)
        ElseIf TypeOf (Control) Is ButtonBase AndAlso (DirectCast(Control, ButtonBase).FlatStyle = FlatStyle.Standard OrElse DirectCast(Control, ButtonBase).FlatStyle = FlatStyle.System) Then
            ButtonRenderer.DrawButton(g, New Rectangle(OffsetX, OffsetY, Control.Width, Control.Height), VisualStyles.PushButtonState.Default)
        ElseIf TypeOf (Control) Is Label AndAlso DirectCast(Control, Label).BorderStyle = BorderStyle.None Then
            RenderLabel(g, OffsetX, OffsetY, True)
        Else
            g.FillRectangle(Brushes.Black, New Rectangle(OffsetX, OffsetY, Control.Width, Control.Height))
        End If
    End Sub

    Private WithEvents OwnerForm As Form

    Dim IsSizing As Boolean
    Private Sub ctl_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctl.Resize
        If IsSizing = False Then
            ctl_GotFocus(ctl, EventArgs.Empty)
        End If
    End Sub

    Private Sub OwnerForm_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OwnerForm.SizeChanged
        'because ResizeEnd isn't called if the window is maximized
        If IsSizing = False Then
            ctl_GotFocus(ctl, EventArgs.Empty)
        End If
    End Sub

    Private Sub OwnerForm_ResizeBegin(ByVal sender As Object, ByVal e As System.EventArgs) Handles OwnerForm.ResizeBegin
        IsSizing = True
        ctl_LostFocus(ctl, EventArgs.Empty)
    End Sub

    Private Sub OwnerForm_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles OwnerForm.ResizeEnd
        IsSizing = False
        ctl_GotFocus(ctl, EventArgs.Empty)
    End Sub

    Private Sub ctl_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctl.GotFocus
        IsSizing = False

        If Control.Focused = False Then Return
        If Enabled = False Then Return
        If ctl.Parent Is Nothing Then Exit Sub
        Dim NewOverlay As Boolean
        If DrawOverlayForm Is Nothing Then
            NewOverlay = True
            DrawOverlayForm = New i00SpellCheck.PerPixelAlphaForm
            DrawOverlayForm.ShowInTaskbar = False
        End If
        Using bOverlay As New Bitmap(ctl.Width + (mc_Amount * 2), ctl.Height + (mc_Amount * 2))

            OwnerForm = Control.FindForm

            Using g = Graphics.FromImage(bOverlay)
                RenderControlForOverlay(g, mc_Amount, mc_Amount)
                bOverlay.Filters.AlphaMask(Color.Transparent, mc_Color)
                g.ResetTransform()

                bOverlay.Filters.GausianBlur(mc_Amount * 2)
                Using holeBitmap As New Bitmap(bOverlay.Width, bOverlay.Height)
                    Using gHole = Graphics.FromImage(holeBitmap)
                        RenderControlForOverlay(gHole, mc_Amount - mc_Depth, mc_Amount - mc_Depth)
                    End Using
                    CreateHoleInImage(bOverlay, holeBitmap)
                End Using
            End Using
            bOverlay.Filters.GausianBlur(2)

            DrawOverlayForm.SetBitmap(bOverlay)

        End Using

        Dim Location = ctl.Parent.PointToScreen(ctl.Location)
        Location.X -= mc_Amount
        Location.Y -= mc_Amount
        Location.X += mc_Depth
        Location.Y += mc_Depth

        DrawOverlayForm.Location = Location

        'DrawOverlayForm.ShowInTaskbar = False
        If NewOverlay Then
            DrawOverlayForm.ShowNoFocus(ctl.Parent)
        End If

        'make the overlay click-through
        Dim exstyle2 As Integer = GetWindowLong(DrawOverlayForm.Handle, GWL_EXSTYLE)
        exstyle2 = exstyle2 Or WS_EX_TRANSPARENT
        SetWindowLong(DrawOverlayForm.Handle, GWL_EXSTYLE, exstyle2)
    End Sub

    Private Sub ctl_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctl.LostFocus
        If DrawOverlayForm IsNot Nothing Then
            DrawOverlayForm.Close()
            DrawOverlayForm = Nothing
        End If
    End Sub

#End Region

#Region "Bitmap manipulation for creating transparencies in custom control shapes"

    Public Sub CreateHoleInImage(ByVal Bitmap As Bitmap, ByVal holeBitmap As Bitmap)

        If Bitmap.Size <> holeBitmap.Size Then Throw New NotSupportedException("The images need to be the same size to use CreateHoleInImage")

        Using bData = ImageFilter.BitmapData.LockBits(Bitmap)
            Using bHoleData = ImageFilter.BitmapData.LockBits(holeBitmap)
                For ii = LBound(bData.ByteData) To UBound(bData.ByteData) Step 4
                    Dim AlphaByte = bData.ByteData(ii + 3)
                    bData.ByteData(ii + 3) = CByte(bData.ByteData(ii + 3) * ((255 - bHoleData.ByteData(ii + 3)) / 255)) 'alpha
                Next
            End Using
        End Using
    End Sub



#End Region

#Region "ButonBase - for CheckBox / RadioButton"

    Private WithEvents ButtonBase As ButtonBase

    Private Sub ButtonBase_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ButtonBase.Paint
        If Enabled = False Then Return
        If (TypeOf (Control) Is CheckBox AndAlso DirectCast(Control, CheckBox).Appearance = Appearance.Normal) OrElse (TypeOf (Control) Is RadioButton AndAlso DirectCast(Control, RadioButton).Appearance = Appearance.Normal) Then
            CheckBoxRenderer.DrawParentBackground(e.Graphics, ButtonBase.ClientRectangle, ButtonBase)
            DrawControlBackground(e.Graphics, Control)
            RenderButtonBase(e.Graphics, , , ButtonBase.Focused)
        Else
            'we are button style
        End If
    End Sub

    Private Sub RenderButtonBase(ByVal g As Graphics, Optional ByVal OffsetX As Integer = 0, Optional ByVal OffsetY As Integer = 0, Optional ByVal DrawFocused As Boolean = False)

        Dim isHot As Boolean
        Dim isPressed As Boolean
        If ButtonBase.RectangleToScreen(New Rectangle(0, 0, ButtonBase.Width, ButtonBase.Height)).Contains(Control.MousePosition) Then
            isHot = True
        End If
        If isHot AndAlso Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
            isPressed = True
        End If
        Dim CheckState = VisualStyles.CheckBoxState.UncheckedNormal
        Dim RadioCheckState = VisualStyles.RadioButtonState.UncheckedNormal
        Dim CA As ContentAlignment = ContentAlignment.MiddleLeft
        Dim TA As ContentAlignment = ContentAlignment.MiddleLeft

        If TypeOf (ButtonBase) Is CheckBox Then
            Dim CheckBox = DirectCast(ButtonBase, CheckBox)
            Select Case CheckBox.CheckState
                Case Windows.Forms.CheckState.Checked
                    CheckState = If(CheckBox.Enabled = False, VisualStyles.CheckBoxState.CheckedDisabled, If(isPressed, VisualStyles.CheckBoxState.CheckedPressed, If(isHot, VisualStyles.CheckBoxState.CheckedHot, VisualStyles.CheckBoxState.CheckedNormal)))
                Case Windows.Forms.CheckState.Indeterminate
                    CheckState = If(CheckBox.Enabled = False, VisualStyles.CheckBoxState.MixedDisabled, If(isPressed, VisualStyles.CheckBoxState.MixedPressed, If(isHot, VisualStyles.CheckBoxState.MixedHot, VisualStyles.CheckBoxState.MixedNormal)))
                Case Windows.Forms.CheckState.Unchecked
                    CheckState = If(CheckBox.Enabled = False, VisualStyles.CheckBoxState.UncheckedDisabled, If(isPressed, VisualStyles.CheckBoxState.UncheckedPressed, If(isHot, VisualStyles.CheckBoxState.UncheckedHot, VisualStyles.CheckBoxState.UncheckedNormal)))
            End Select
            CA = CheckBox.CheckAlign
            TA = CheckBox.TextAlign
        ElseIf TypeOf (ButtonBase) Is RadioButton Then
            Dim RadioButton = DirectCast(ButtonBase, RadioButton)
            If RadioButton.Checked Then
                RadioCheckState = If(RadioButton.Enabled = False, VisualStyles.RadioButtonState.CheckedDisabled, If(isPressed, VisualStyles.RadioButtonState.CheckedPressed, If(isHot, VisualStyles.RadioButtonState.CheckedHot, VisualStyles.RadioButtonState.CheckedNormal)))
            Else
                RadioCheckState = If(RadioButton.Enabled = False, VisualStyles.RadioButtonState.UncheckedDisabled, If(isPressed, VisualStyles.RadioButtonState.UncheckedPressed, If(isHot, VisualStyles.RadioButtonState.UncheckedHot, VisualStyles.RadioButtonState.UncheckedNormal)))
            End If
            CA = RadioButton.CheckAlign
            TA = RadioButton.TextAlign
        End If

        Dim GlyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckState)
       
        Dim CALeft As Boolean = CA = ContentAlignment.BottomLeft OrElse CA = ContentAlignment.MiddleLeft OrElse CA = ContentAlignment.TopLeft
        Dim CARight As Boolean = CA = ContentAlignment.BottomRight OrElse CA = ContentAlignment.MiddleRight OrElse CA = ContentAlignment.TopRight
        Dim CATop As Boolean = CA = ContentAlignment.TopCenter
        Dim CABottom As Boolean = CA = ContentAlignment.BottomCenter

        Dim TextRectangle As New Rectangle(ButtonBase.Padding.Left + If(CALeft, GlyphSize.Width + 3, 0), ButtonBase.Padding.Top + If(CATop, GlyphSize.Height + 3, 0), ButtonBase.Width - ButtonBase.Padding.Left - ButtonBase.Padding.Right - If(CALeft OrElse CARight, GlyphSize.Width + 3, 0), ButtonBase.Height - ButtonBase.Padding.Top - ButtonBase.Padding.Bottom - If(CATop OrElse CABottom, GlyphSize.Height + 3, 0))

        TextRectangle = GetTextRectangle(TA, TextRectangle, ButtonBase.Text, ButtonBase.Font, ButtonBase.AutoSize = False)

        CATop = CA = ContentAlignment.TopCenter OrElse CA = ContentAlignment.TopLeft OrElse CA = ContentAlignment.TopRight
        CABottom = CA = ContentAlignment.BottomCenter OrElse CA = ContentAlignment.BottomLeft OrElse CA = ContentAlignment.BottomRight

        Dim GlyphLocation As New Point
        If CALeft Then
            GlyphLocation.X = ButtonBase.Padding.Left
        ElseIf CARight Then
            GlyphLocation.X = ButtonBase.Width - ButtonBase.Padding.Right - GlyphSize.Width
        Else
            GlyphLocation.X = CInt(((ButtonBase.Width - ButtonBase.Padding.Left - ButtonBase.Padding.Right - GlyphSize.Width) / 2) + ButtonBase.Padding.Left)
        End If

        If CATop Then
            GlyphLocation.Y = ButtonBase.Padding.Top
        ElseIf CABottom Then
            GlyphLocation.Y = ButtonBase.Height - ButtonBase.Padding.Bottom - GlyphSize.Height
        Else
            GlyphLocation.Y = CInt(((ButtonBase.Height - ButtonBase.Padding.Top - ButtonBase.Padding.Bottom - GlyphSize.Height) / 2) + ButtonBase.Padding.Right)
        End If

        If GlyphLocation.Y < 0 Then GlyphLocation.Y = 0
        If GlyphLocation.X < 0 Then GlyphLocation.X = 0

        If TextRectangle.Y < 0 Then
            TextRectangle.Y = 0
        End If
        If TextRectangle.Top + TextRectangle.Height > Control.Height Then
            TextRectangle.Height = Control.Height - TextRectangle.Top
        End If

        GlyphLocation.X += OffsetX
        TextRectangle.X += OffsetX
        GlyphLocation.Y += OffsetY
        TextRectangle.Y += OffsetY

        Dim TextFormat = TextFormatFlags.Left
        If ButtonBase.AutoEllipsis Then
            TextFormat = TextFormat Or TextFormatFlags.EndEllipsis
        End If
        If ButtonBase.AutoSize = False Then
            TextFormat = TextFormat Or TextFormatFlags.WordBreak
        End If


        If DrawFocused Then
            System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, TextRectangle)
        End If
        TextRenderer.DrawText(g, ButtonBase.Text, ButtonBase.Font, TextRectangle, If(ButtonBase.Enabled, ButtonBase.ForeColor, i00SpellCheck.DrawingFunctions.BlendColor(ButtonBase.ForeColor, Drawing.Color.Transparent)), TextFormat)

        If TypeOf (ButtonBase) Is CheckBox Then
            Dim CheckBox = DirectCast(ButtonBase, CheckBox)
            Select Case ButtonBase.FlatStyle
                Case FlatStyle.Flat, FlatStyle.Popup
                    Dim FlatGlyphColor = ButtonBase.ForeColor
                    If ButtonBase.Enabled = False Then FlatGlyphColor = i00SpellCheck.DrawingFunctions.BlendColor(FlatGlyphColor, Drawing.Color.Transparent)
                    Dim FlatGlyphBGColor = Color.FromKnownColor(KnownColor.Window)
                    If isPressed = False Then FlatGlyphBGColor = i00SpellCheck.DrawingFunctions.BlendColor(FlatGlyphBGColor, Color.FromKnownColor(KnownColor.Control))

                    Dim FlatGlyphRect = New Rectangle(GlyphLocation, New Size(GlyphSize.Width - 1, GlyphSize.Height - 1))
                    Dim FlatGlyphRectFill = New Rectangle(GlyphLocation, New Size(GlyphSize.Width, GlyphSize.Height))
                    Using sb As New SolidBrush(FlatGlyphBGColor)
                        g.FillRectangle(sb, FlatGlyphRectFill)
                    End Using
                    Dim FlatCheckState = CheckBox.CheckState
                    If ButtonBase.FlatStyle = FlatStyle.Popup Then
                        Using p As New Pen(i00SpellCheck.DrawingFunctions.BlendColor(FlatGlyphColor, Drawing.Color.Transparent))
                            g.DrawRectangle(p, FlatGlyphRect)
                        End Using
                        If CheckBox.CheckState = Windows.Forms.CheckState.Indeterminate Then
                            FlatGlyphColor = i00SpellCheck.DrawingFunctions.BlendColor(FlatGlyphColor, Drawing.Color.Transparent)
                            FlatCheckState = Windows.Forms.CheckState.Checked
                        End If
                    Else
                        Using p As New Pen(FlatGlyphColor)
                            g.DrawRectangle(p, FlatGlyphRect)
                        End Using
                    End If
                    Select Case FlatCheckState
                        Case Windows.Forms.CheckState.Checked
                            g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                            For i = 0 To 2
                                Dim p1 = New Point(GlyphLocation.X + 3, GlyphLocation.Y + 5 + i)
                                Dim p2 As New Point(p1.X + 2, p1.Y + 2)
                                Dim p3 As New Point(p2.X + 4, p2.Y - 4)
                                Using p As New Pen(FlatGlyphColor)
                                    g.DrawLine(p, p1, p2)
                                    g.DrawLine(p, p2, p3)
                                End Using
                            Next
                        Case Windows.Forms.CheckState.Indeterminate
                            Using sb As New SolidBrush(FlatGlyphColor)
                                FlatGlyphRectFill.Inflate(-2, -2)
                                g.FillRectangle(sb, FlatGlyphRectFill)
                            End Using
                    End Select
                Case Else
                    CheckBoxRenderer.DrawCheckBox(g, GlyphLocation, CheckState)
            End Select

            ''used to do it like this:
            'CheckBoxRenderer.DrawCheckBox(g, GlyphLocation, TextRectangle, ButtonBase.Text, ButtonBase.Font, TextFormat, DrawFocused, CheckState)
            ''... but doesn't support text color
        ElseIf TypeOf (ButtonBase) Is RadioButton Then
            RadioButtonRenderer.DrawRadioButton(g, GlyphLocation, RadioCheckState)
            ''used to do it like this:
            'RadioButtonRenderer.DrawRadioButton(g, GlyphLocation, TextRectangle, ButtonBase.Text, ButtonBase.Font, TextFormat, DrawFocused, RadioCheckState)
            ''... but doesn't support text color
        End If

    End Sub

#End Region

#Region "Link Label"

    Private WithEvents Label As Label

    Private Sub Label_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Label.MouseLeave
        Label.Invalidate()
    End Sub

    Private Sub Label_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Label.MouseMove
        If Enabled = False Then Return
        'should use mouse leave but it doesn't work correctly when the mouse is down
        Static LastMouseOver As Boolean
        Dim ThisMouseOver = Label.RectangleToScreen(LabelTextRectangle).Contains(Control.MousePosition)
        If LastMouseOver <> ThisMouseOver Then
            Label.Invalidate()
        End If
        LastMouseOver = ThisMouseOver
    End Sub

    Private Sub Label_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Label.Paint
        If Enabled = False Then Return
        CheckBoxRenderer.DrawParentBackground(e.Graphics, Label.ClientRectangle, Label)
        DrawControlBackground(e.Graphics, Control)
        RenderLabel(e.Graphics, , , Label.Focused, True)
    End Sub

    Dim LabelTextRectangle As Rectangle

    Private Sub RenderLabel(ByVal g As Graphics, Optional ByVal OffsetX As Integer = 0, Optional ByVal OffsetY As Integer = 0, Optional ByVal DrawFocused As Boolean = False, Optional ByVal RealLabel As Boolean = False)
        Dim LinkLabel = TryCast(Label, LinkLabel)
        Dim LinkFont = Label.Font
        If LinkLabel IsNot Nothing Then
            If LinkFont.Underline = False Then
                Dim FontStyle = LinkFont.Style Or Drawing.FontStyle.Underline
                If LinkFont.FontFamily.IsStyleAvailable(FontStyle) Then
                    LinkFont = New Font(LinkFont, FontStyle)
                End If
            End If
        End If

        Dim TextRectangle As New Rectangle(0, 0, Label.ClientRectangle.Width, Label.ClientRectangle.Height)
        TextRectangle = GetTextRectangle(Label.TextAlign, TextRectangle, Label.Text, LinkFont, Label.AutoSize = False)

        If TextRectangle.Y < 0 Then
            TextRectangle.Y = 0
        End If
        If TextRectangle.Top + TextRectangle.Height > Control.Height Then
            TextRectangle.Height = Control.Height - TextRectangle.Top
        End If

        If RealLabel Then LabelTextRectangle = TextRectangle

        Dim IsHot As Boolean
        Dim LinkColor As Color = Label.ForeColor
        If LinkLabel IsNot Nothing Then
            If LinkLabel.Enabled = False Then
                LinkColor = LinkLabel.DisabledLinkColor
            Else
                LinkColor = LinkLabel.LinkColor
                If LinkLabel.LinkVisited Then
                    LinkColor = LinkLabel.VisitedLinkColor
                End If
                If Label.RectangleToScreen(TextRectangle).Contains(Control.MousePosition) Then
                    IsHot = True
                    If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
                        'isHot and isPressed
                        LinkColor = LinkLabel.ActiveLinkColor
                    End If
                End If
            End If
        End If

        TextRectangle.X += OffsetX
        TextRectangle.Y += OffsetY

        If DrawFocused Then
            System.Windows.Forms.ControlPaint.DrawFocusRectangle(g, TextRectangle)
        End If


        Dim TextFormat = TextFormatFlags.Left
        If Label.AutoEllipsis Then
            TextFormat = TextFormat Or TextFormatFlags.EndEllipsis
        End If
        If Label.AutoSize = False Then
            TextFormat = TextFormat Or TextFormatFlags.WordBreak
        End If

        Dim Underline As Boolean
        If LinkLabel IsNot Nothing Then
            Dim LinkBehavior = LinkLabel.LinkBehavior
            If LinkBehavior = Windows.Forms.LinkBehavior.SystemDefault Then
                'look it up from the registry
                Dim regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Microsoft\Internet Explorer\Main")
                If regKey Is Nothing OrElse regKey.GetValue("Anchor Underline") Is Nothing Then
                    regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Microsoft\Internet Explorer\Main")
                End If
                If regKey IsNot Nothing AndAlso regKey.GetValue("Anchor Underline") IsNot Nothing Then
                    Select Case LCase(regKey.GetValue("Anchor Underline").ToString)
                        Case "no"
                            LinkBehavior = Windows.Forms.LinkBehavior.NeverUnderline
                        Case "hover"
                            LinkBehavior = Windows.Forms.LinkBehavior.HoverUnderline
                        Case Else
                            LinkBehavior = Windows.Forms.LinkBehavior.AlwaysUnderline
                    End Select
                End If
            End If
            Select Case LinkBehavior
                Case LinkBehavior.AlwaysUnderline
                    Underline = True
                Case LinkBehavior.HoverUnderline
                    Underline = IsHot
                Case LinkBehavior.NeverUnderline
                    Underline = False
            End Select
        End If

        TextRenderer.DrawText(g, Label.Text, If(Underline, LinkFont, Label.Font), TextRectangle, LinkColor, TextFormat)

    End Sub

#End Region

#Region "Text Rendering"

    Private Function GetTextRectangle(ByVal TA As ContentAlignment, ByVal TextRectangle As Rectangle, ByVal Text As String, ByVal Font As Font, ByVal WordWrap As Boolean) As Rectangle

        Dim TALeft As Boolean = TA = ContentAlignment.BottomLeft OrElse TA = ContentAlignment.MiddleLeft OrElse TA = ContentAlignment.TopLeft
        Dim TARight As Boolean = TA = ContentAlignment.BottomRight OrElse TA = ContentAlignment.MiddleRight OrElse TA = ContentAlignment.TopRight
        Dim TATop As Boolean = TA = ContentAlignment.TopCenter OrElse TA = ContentAlignment.TopLeft OrElse TA = ContentAlignment.TopRight
        Dim TABottom As Boolean = TA = ContentAlignment.BottomCenter OrElse TA = ContentAlignment.BottomLeft OrElse TA = ContentAlignment.BottomRight

        Dim TextSize = TextRenderer.MeasureText(Text, Font, TextRectangle.Size, If(WordWrap, TextFormatFlags.WordBreak, Nothing))
        If TATop Then
            'already set
        ElseIf TABottom Then
            TextRectangle.Y = TextRectangle.Bottom - TextSize.Height
        Else
            TextRectangle.Y = CInt(TextRectangle.Top + ((TextRectangle.Height - TextSize.Height) / 2))
        End If
        If TALeft Then
            'already set
        ElseIf TARight Then
            TextRectangle.X = TextRectangle.Right - TextSize.Width
        Else
            TextRectangle.X = CInt(TextRectangle.Left + ((TextRectangle.Width - TextSize.Width) / 2))
        End If

        Dim OrigSize = TextRectangle.Size
        TextRectangle.Size = TextSize
        If TextRectangle.Width > OrigSize.Width Then
            TextRectangle.Width = OrigSize.Width
        End If

        Return TextRectangle
    End Function

#End Region

#Region "Background Rendering"

    Private Sub DrawControlBackground(ByVal g As Graphics, ByVal Control As Control)
        'draw bg
        Dim BGRect = New Rectangle(0, 0, Control.ClientSize.Width, Control.ClientSize.Height)
        Using sb As New SolidBrush(Control.BackColor)
            g.FillRectangle(sb, BGRect)
        End Using
        If control.BackgroundImage IsNot Nothing Then
            Select Case Control.BackgroundImageLayout
                Case ImageLayout.Tile
                    Using tb As New TextureBrush(Control.BackgroundImage)
                        g.FillRectangle(tb, BGRect)
                    End Using
                Case ImageLayout.Center
                    g.DrawImage(Control.BackgroundImage, CSng(Math.Max((Control.ClientSize.Width - Control.BackgroundImage.Width) / 2, 0)), CSng(Math.Max((Control.ClientSize.Height - Control.BackgroundImage.Height) / 2, 0)))
                Case ImageLayout.Stretch
                    g.DrawImage(Control.BackgroundImage, BGRect)
                Case ImageLayout.Zoom
                    Dim ZoomRect = i00SpellCheck.DrawingFunctions.GetBestFitRect(BGRect, New Rectangle(0, 0, Control.BackgroundImage.Width, Control.BackgroundImage.Height), DrawingFunctions.BestFitStyle.Stretch)
                    g.DrawImage(Control.BackgroundImage, Rectangle.Round(ZoomRect))
                Case ImageLayout.None
                    g.DrawImage(Control.BackgroundImage, New Point(0, 0))
            End Select
        End If
    End Sub

#End Region

#Region "iTestHarness"

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As System.Windows.Forms.Control Implements i00SpellCheck.iTestHarness.SetupControl
        If TypeOf (Control) Is Label Then
            Dim Label = DirectCast(Control, Label)
            Label.AutoSize = True
        ElseIf TypeOf (Control) Is CheckBox Then
            Dim CheckBox = DirectCast(Control, CheckBox)
            CheckBox.AutoSize = True
        ElseIf TypeOf (Control) Is RadioButton Then
            Dim RadioButton = DirectCast(Control, RadioButton)
            RadioButton.AutoSize = True
            RadioButton.TabStop = True
        ElseIf TypeOf (Control) Is MonthCalendar Then
            Dim MonthCalendar = DirectCast(Control, MonthCalendar)
            MonthCalendar.MaxSelectionCount = Integer.MaxValue
            MonthCalendar.Anchor = AnchorStyles.Top Or AnchorStyles.Left
            MonthCalendar.Size = New Size(0, 0)
        ElseIf TypeOf (Control) Is RichTextBox Then
            Control.Height = 21
        ElseIf TypeOf (Control) Is TreeView Then
            Dim TreeView = DirectCast(Control, TreeView)
            TreeView.HideSelection = False
            For iNode = 1 To 3
                Dim Node = TreeView.Nodes.Add("Node " & iNode)
                For iSubNode = 1 To 3
                    Node.Nodes.Add("Sub Node " & iSubNode)
                Next
            Next
            TreeView.Height = TreeView.Nodes(TreeView.Nodes.Count - 1).Bounds.Bottom + (TreeView.Height - TreeView.ClientRectangle.Height)
        ElseIf TypeOf (Control) Is ListView Then
            Dim ListView = DirectCast(Control, ListView)
            ListView.FullRowSelect = True
            ListView.HideSelection = False
            For i = 1 To 2
                ListView.Columns.Add("Column " & i)
            Next
            ListView.View = View.Details
            For i = 1 To 3
                ListView.Items.Add("Item " & i).SubItems.Add("Sub Item " & i)
            Next
            For Each item In ListView.Columns.OfType(Of ColumnHeader)()
                item.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            Next
            ListView.Height = ListView.Items(ListView.Items.Count - 1).GetBounds(ItemBoundsPortion.Entire).Bottom + (ListView.Height - ListView.ClientRectangle.Height)
        ElseIf TypeOf (Control) Is ListBox Then
            Dim ListBox = DirectCast(Control, ListBox)
            For i = 1 To 3
                ListBox.Items.Add("Item " & i)
            Next
            ListBox.Height = ListBox.ItemHeight * ListBox.Items.Count + (ListBox.Height - ListBox.ClientRectangle.Height)
        End If
        Return Control
    End Function

#End Region

End Class
