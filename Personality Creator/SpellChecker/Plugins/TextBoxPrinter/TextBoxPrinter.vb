'i00 .Net Control Extensions - TextBoxPrinter
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

Imports i00SpellCheck

'The weight is the priority order of the plugins ... if there are multiple plugins that extend the same control ...
'in this case the plugin with the heighest weight gets used first...
'all built in plugins in i00SpellCheck have a weight of 0
<i00SpellCheck.PluginWeight(1)> _
Public Class TextBoxPrinter
    Inherits ControlExtension
    Implements iTestHarness

#Region "Ease of access"

    Private WithEvents parentTextBox As TextBoxBase

    Private WithEvents parentRichTextBox As RichTextBox

    Private WithEvents extTextBoxContextMenu As extTextBoxContextMenu

#End Region

#Region "Plugin Info"

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(TextBoxBase)}
        End Get
    End Property

    Public Overrides ReadOnly Property RequiredExtensions() As System.Collections.Generic.List(Of System.Type)
        Get
            RequiredExtensions = New List(Of System.Type)
            RequiredExtensions.Add(GetType(extTextBoxContextMenu))
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Overrides Sub Load()
        parentTextBox = DirectCast(Control, TextBoxBase)
        parentRichTextBox = TryCast(Control, RichTextBox)
        extTextBoxContextMenu = parentTextBox.ExtensionCast(Of extTextBoxContextMenu)()
    End Sub

#End Region

#Region "Properties"

    Dim mc_PrintScale As Double = 1.0
    <System.ComponentModel.DefaultValue(1.0)> _
    <System.ComponentModel.DisplayName("Print Scale")> _
    <System.ComponentModel.Description("The scale used when printing")> _
    Public Property PrintScale() As Double
        Get
            Return mc_PrintScale
        End Get
        Set(ByVal value As Double)
            mc_PrintScale = value
            'SetPaperPreviewMargin()
        End Set
    End Property


    Dim mc_AllowCtrlP As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow Ctrl+P Printing")> _
    <System.ComponentModel.Description("Enables the Ctrl+P shortcut to print")> _
    Public Property AllowCtrlP() As Boolean
        Get
            Return mc_AllowCtrlP
        End Get
        Set(ByVal value As Boolean)
            mc_AllowCtrlP = value
        End Set
    End Property

    Dim mc_AllowMenuPrinting As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow Menu Printing")> _
    <System.ComponentModel.Description("Enables the print menu item from the text boxes context menu")> _
    Public Property AllowMenuPrinting() As Boolean
        Get
            Return mc_AllowMenuPrinting
        End Get
        Set(ByVal value As Boolean)
            mc_AllowMenuPrinting = value
        End Set
    End Property

    ''Discontinued as the margins appear inaccurate???
    'Dim PreviousMargin As Integer
    'Dim mc_ShowPrintMargins As Boolean = False
    '<System.ComponentModel.DefaultValue(False)> _
    '<System.ComponentModel.DisplayName("Show Print Margins")> _
    '<System.ComponentModel.Description("Enables the print menu item from the text boxes context menu")> _
    'Public Property ShowPrintMargins() As Boolean
    '    Get
    '        Return mc_ShowPrintMargins
    '    End Get
    '    Set(ByVal value As Boolean)
    '        If mc_ShowPrintMargins <> value Then
    '            If parentRichTextBox IsNot Nothing Then
    '                mc_ShowPrintMargins = value
    '                SetPaperPreviewMargin()
    '            Else
    '                If value = True Then
    '                    Throw New NotSupportedException("ShowPrintMargins can only be set on RichTextBoxes")
    '                End If
    '            End If
    '        End If
    '    End Set
    'End Property

#End Region

#Region "Ctrl + P"

    Private Sub mc_TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles parentTextBox.KeyDown
        If e.KeyCode = Keys.P AndAlso My.Computer.Keyboard.CtrlKeyDown AndAlso AllowCtrlP Then
            e.Handled = True
            Print()
        End If
    End Sub

#End Region

#Region "Menu"

    Private Sub extTextBoxContextMenu_MenuOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles extTextBoxContextMenu.MenuOpening
        If AllowMenuPrinting Then

            Dim MenuItems As New List(Of ToolStripItem)

            Dim tsiPrint = New i00SpellCheck.extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripMenuItem("Print", My.Resources.PrintHS)
            AddHandler tsiPrint.Click, AddressOf tsiPrint_Click
            MenuItems.Add(tsiPrint)

            If MenuItems.Count > 0 Then
                If extTextBoxContextMenu.ContextMenuStrip.Items.Count > 0 Then
                    extTextBoxContextMenu.ContextMenuStrip.Items.Add(New extTextBoxContextMenu.StandardContextMenuStrip.StandardToolStripSeparator)
                End If
                For Each item In MenuItems
                    extTextBoxContextMenu.ContextMenuStrip.Items.AddRange(MenuItems.ToArray)
                Next
            End If

        End If
    End Sub

    Private Sub tsiPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Print()
    End Sub

#End Region

#Region "Print"

    Public Sub Print()
        Using PrintPreviewDialog As New PrintPreviewDialog
            Using PrintDialog As New PrintDialog
                If parentRichTextBox Is Nothing Then
                    'standard text box
                    Dim pd As New TextPrint(parentTextBox.Text)
                    pd.Font = New Font(parentTextBox.Font.FontFamily, CSng(parentTextBox.Font.Size * mc_PrintScale), parentTextBox.Font.Style)

                    PrintDialog.Document = pd
                Else
                    Dim pd As New Printing.PrintDocument
                    AddHandler pd.BeginPrint, AddressOf RTFpd_BeginPrint
                    AddHandler pd.PrintPage, AddressOf RTFpd_PrintPage
                    PrintDialog.Document = pd
                End If

                'the below is used as otherwise in win 7 64bit apps the dialog doesn't show
                PrintDialog.UseEXDialog = True

                If PrintDialog.ShowDialog(Control.FindForm) = DialogResult.OK Then
                    PrintPreviewDialog.Document = PrintDialog.Document
                    PrintPreviewDialog.UseAntiAlias = True
                    PrintPreviewDialog.WindowState = FormWindowState.Maximized
                    PrintPreviewDialog.ShowDialog(Control.FindForm)
                End If
            End Using
        End Using
    End Sub

#Region "RTF Printing"

    Dim PrintCharUpTo As Integer

    <System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    <System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
    Private Structure CHARRANGE
        'First character of range (0 for start of doc)
        Public cpMin As Integer
        'Last character of range (-1 for end of doc)
        Public cpMax As Integer
    End Structure

    <System.Runtime.InteropServices.StructLayout(Runtime.InteropServices.LayoutKind.Sequential)> _
    Private Structure FORMATRANGE
        Public hdc As IntPtr
        Public hdcTarget As IntPtr
        Public rc As RECT
        Public rcPage As RECT
        Public chrg As CHARRANGE
    End Structure

    Private Const anInch As Double = 14.4

    Private Const WM_USER As Integer = &H400
    Private Const EM_FORMATRANGE As Integer = WM_USER + 57

    <System.Runtime.InteropServices.DllImport("USER32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wp As IntPtr, ByVal lp As IntPtr) As IntPtr
    End Function

    Private Function PrintRTFPageBitToGraphics(ByVal g As Graphics, ByVal charFrom As Integer, ByVal charTo As Integer, ByVal rectToPrint As RECT, ByVal rectPage As RECT) As Integer
        Dim hdc As IntPtr = g.GetHdc()

        Dim fmtRange As FORMATRANGE

        'Indicate character from to character to 
        fmtRange.chrg.cpMax = charTo
        fmtRange.chrg.cpMin = charFrom

        'Use the same DC for measuring and rendering
        fmtRange.hdc = hdc
        fmtRange.hdcTarget = hdc

        'Point at printer hDC
        fmtRange.rc = rectToPrint
        'Indicate the area on page to print
        fmtRange.rcPage = rectPage
        'Indicate size of page
        Dim res As IntPtr = IntPtr.Zero

        Dim wparam As IntPtr = IntPtr.Zero
        wparam = New IntPtr(1)

        'Get the pointer to the FORMATRANGE structure in memory
        Dim lparam As IntPtr = IntPtr.Zero
        lparam = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(System.Runtime.InteropServices.Marshal.SizeOf(fmtRange))
        System.Runtime.InteropServices.Marshal.StructureToPtr(fmtRange, lparam, False)

        'Send the rendered data for printing 
        res = SendMessage(parentTextBox.Handle, EM_FORMATRANGE, wparam, lparam)

        'Free the block of memory allocated
        System.Runtime.InteropServices.Marshal.FreeCoTaskMem(lparam)

        'Release the device context handle obtained by a previous call
        g.ReleaseHdc(hdc)

        'Return last + 1 character printer
        Return res.ToInt32()

    End Function

    Private Function RTBPrintPage(ByVal charFrom As Integer, ByVal charTo As Integer, ByVal e As System.Drawing.Printing.PrintPageEventArgs) As Integer
        'Calculate the area to render and print
        Dim rectToPrint As RECT
        rectToPrint.Top = CInt(e.MarginBounds.Top * anInch)
        rectToPrint.Bottom = CInt(e.MarginBounds.Bottom * anInch)
        rectToPrint.Left = CInt(e.MarginBounds.Left * anInch)
        rectToPrint.Right = CInt(e.MarginBounds.Right * anInch)

        If mc_PrintScale <> 1 Then
            'for scaling ... this is more blurry than standard printing so don't do this if 1:1

            rectToPrint.Right -= rectToPrint.Left
            rectToPrint.Bottom -= rectToPrint.Top
            rectToPrint.Left = 0
            rectToPrint.Top = 0

            rectToPrint.Right = CInt(rectToPrint.Right * (1 / mc_PrintScale))
            rectToPrint.Bottom = CInt(rectToPrint.Bottom * (1 / mc_PrintScale))

            Using b As New Bitmap(CInt(rectToPrint.Right / 15), CInt(rectToPrint.Bottom / 15)) '15 as measurement in in twips
                Using g = Graphics.FromImage(b)
                    RTBPrintPage = PrintRTFPageBitToGraphics(g, charFrom, charTo, rectToPrint, rectToPrint)
                    g.InterpolationMode = Drawing2D.InterpolationMode.High
                    e.Graphics.DrawImage(b, e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height)
                End Using

            End Using

        Else

            'Calculate the size of the page
            Dim rectPage As RECT
            rectPage.Top = CInt(e.PageBounds.Top * anInch)
            rectPage.Bottom = CInt(e.PageBounds.Bottom * anInch)
            rectPage.Left = CInt(e.PageBounds.Left * anInch)
            rectPage.Right = CInt(e.PageBounds.Right * anInch)

            Return PrintRTFPageBitToGraphics(e.Graphics, charFrom, charTo, rectToPrint, rectPage)

        End If

    End Function

    Private Sub RTFpd_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs)
        PrintCharUpTo = 0
    End Sub

    Private Sub RTFpd_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        PrintCharUpTo = RTBPrintPage(PrintCharUpTo, parentTextBox.TextLength, e)
        If PrintCharUpTo < parentTextBox.TextLength Then
            e.HasMorePages = True
        End If
    End Sub

#End Region

#End Region

#Region "iTestHarness"

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As System.Windows.Forms.Control Implements i00SpellCheck.iTestHarness.SetupControl

        If Control.GetType Is GetType(TextBox) Then
            Dim TextBox = DirectCast(Control, TextBox)

            TextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12)
            TextBox.Multiline = True
            TextBox.ScrollBars = ScrollBars.Vertical
            TextBox.AppendText(If(TextBox.Text = "", "", vbCrLf & vbCrLf) & "The TextBoxPrinter project adds the ability to print the contents of any TextBox, simply press Ctrl+P, or right click and press Print!")

            TextBox.SelectionStart = 0
            TextBox.SelectionLength = 0

            Return TextBox
        ElseIf Control.GetType Is GetType(RichTextBox) Then
            Dim RichTextBox = DirectCast(Control, RichTextBox)

            RichTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!)

            RichTextBox.AppendText(If(RichTextBox.Text = "", "", vbCrLf & vbCrLf) & "The TextBoxPrinter project adds the ability to print the contents of any TextBox, simply press Ctrl+P, or right click and press Print!")

            Dim HighlightKeyWordFormat As New extTextBoxCommon.HighlightKeyWordFormat
            HighlightKeyWordFormat.Color = Color.FromKnownColor(KnownColor.HotTrack)

            extTextBoxCommon.HighlightKeyWord(RichTextBox, "TextBoxPrinter", HighlightKeyWordFormat)

            RichTextBox.Select(0, 0)
            RichTextBox.ClearUndo()

            Return RichTextBox
        Else
            Return Nothing
        End If

    End Function

#End Region

#Region "ShowPrintMargins"

    '    Private Const WM_PAINT As Integer = &HF

    '    Protected Overrides Sub WndProc(ByRef m As Message)
    '        Select Case m.Msg
    '            Case WM_PAINT
    '                If parentRichTextBox IsNot Nothing AndAlso mc_ShowPrintMargins Then
    '                    parentTextBox.Invalidate()
    '                    'parentTextBox.SuspendLayout()
    '                    MyBase.WndProc(m)
    '                    Me.CustomPaint()
    '                    'parentRichTextBox.ResumeLayout()
    '                    'MyBase.WndProc(m)
    '                Else
    '                    MyBase.WndProc(m)
    '                End If
    '            Case Else
    '                MyBase.WndProc(m)
    '        End Select
    '    End Sub

    '    Dim PageHeight As Integer
    '    Dim DefaultPageSettings As Printing.PageSettings = (New Printing.PrintDocument).DefaultPageSettings
    '    Private Sub SetPaperPreviewMargin()
    '        If parentRichTextBox IsNot Nothing Then
    '            If mc_ShowPrintMargins = True Then
    '                PreviousMargin = parentRichTextBox.RightMargin
    '                parentRichTextBox.RightMargin = CInt(((DefaultPageSettings.PaperSize.Width - (DefaultPageSettings.Margins.Right + DefaultPageSettings.Margins.Left)) * (1 / mc_PrintScale)) * parentRichTextBox.ZoomFactor)
    '                PageHeight = CInt(((DefaultPageSettings.PaperSize.Height - (DefaultPageSettings.Margins.Top + DefaultPageSettings.Margins.Bottom)) * (1 / mc_PrintScale)) * parentRichTextBox.ZoomFactor)
    '            Else
    '                parentRichTextBox.RightMargin = PreviousMargin
    '            End If
    '            parentRichTextBox.Invalidate()
    '        End If
    '    End Sub

    '#Region "ScrollInfo API"

    '    Private Structure SCROLLINFO
    '        Public Declare Function GetScrollInfo Lib "user32" (ByVal hWnd As IntPtr, ByVal fnBar As Bar, ByRef lpScrollInfo As SCROLLINFO) As Boolean
    '        Public Declare Function SetScrollInfo Lib "user32" (ByVal hWnd As IntPtr, ByVal fnBar As Bar, ByRef lpScrollInfo As SCROLLINFO, ByVal redraw As Boolean) As Integer
    '        Sub New(ByVal mask As Mask)
    '            Me.cbSize = CUInt(Runtime.InteropServices.Marshal.SizeOf(Me))
    '            Me.fMask = mask
    '        End Sub
    '        Private cbSize As UInteger
    '        Private fMask As Mask
    '        Public nMin As Integer
    '        Public nMax As Integer
    '        Public nPage As UInteger
    '        Public nPos As Integer
    '        Public nTrackPos As Integer
    '        Public Enum Bar As Integer
    '            Horizontal = 0
    '            Vertical = 1
    '            Client = 2
    '            Both = 3
    '        End Enum
    '        Public Enum Mask As UInteger
    '            Range = &H1
    '            Page = &H2
    '            Position = &H4
    '            DisableNoScroll = &H8
    '            TrackPosition = &H10
    '            ALL = Range Or Page Or Position Or TrackPosition
    '        End Enum
    '    End Structure

    '#End Region

    '    Private Sub CustomPaint()
    '        If parentTextBox.ClientSize.Width = 0 Then Exit Sub
    '        'for drawing underlines below the textbox drawing bounds when on a single line text box
    '        Using b As New Bitmap(parentTextBox.ClientSize.Width, parentTextBox.ClientSize.Height)
    '            Using g = Graphics.FromImage(b)
    '                g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
    '                Dim MarginColor = i00SpellCheck.DrawingFunctions.BlendColor(parentRichTextBox.ForeColor, parentRichTextBox.BackColor, 63)
    '                Using p As New Pen(MarginColor)
    '                    'right margin...
    '                    g.DrawLine(p, parentRichTextBox.RightMargin, 0, parentRichTextBox.RightMargin, b.Height)

    '                    'page break margins
    '                    If PageHeight <> 0 Then
    '                        Dim ScrollInfo As New SCROLLINFO(ScrollInfo.Mask.TrackPosition)
    '                        ScrollInfo.GetScrollInfo(parentRichTextBox.Handle, ScrollInfo.Bar.Vertical, ScrollInfo)
    '                        Dim PageTop = ScrollInfo.nTrackPos
    '                        Dim StartPage = PageTop \ PageHeight
    '                        For i = StartPage * PageHeight To PageTop + parentRichTextBox.Height Step PageHeight
    '                            Dim yLine As Integer = i - ScrollInfo.nTrackPos
    '                            g.DrawLine(p, 0, yLine, parentRichTextBox.RightMargin, yLine)
    '                        Next
    '                    End If
    '                End Using
    '                'g.DrawString(extTextBoxCommon.GetScrollBarLocation(parentTextBox).ToString, SystemFonts.DefaultFont, Brushes.Black, 0, 0)
    '            End Using
    '            Dim textBoxGraphics = Graphics.FromHwnd(parentTextBox.Handle)
    '            textBoxGraphics.DrawImageUnscaled(b, 0, 0)
    '        End Using
    '    End Sub

#End Region

End Class
