'i00 .Net Control Extensions
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

Public Class extTextBoxContextMenu
    Inherits ControlExtension

#Region "Ease of access"

    Protected WithEvents mc_TextBox As TextBoxBase
    Protected Friend ReadOnly Property parentTextBox() As TextBoxBase
        Get
            Return mc_TextBox
        End Get
    End Property

    Protected WithEvents mc_RichTextBox As RichTextBox
    Protected Friend ReadOnly Property parentRichTextBox() As RichTextBox
        Get
            Return mc_RichTextBox
        End Get
    End Property

#End Region

#Region "Underlying Control"

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(TextBoxBase)}
        End Get
    End Property

#End Region

    Public Overrides Sub Load()
        mc_TextBox = DirectCast(Control, TextBoxBase)
        mc_RichTextBox = TryCast(Control, RichTextBox)

        parentTextBox_ContextMenuStripChanged(parentTextBox, EventArgs.Empty)
    End Sub

#Region "Key Press"

    Private Sub mc_TextBox_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles mc_TextBox.KeyDown
        'Ctrl+A
        If e.KeyCode = Keys.A AndAlso My.Computer.Keyboard.CtrlKeyDown Then
            e.Handled = True
            parentTextBox.SelectAll()
        End If
    End Sub

    Private Sub parentTextBox_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles mc_TextBox.KeyUp
        Select Case e.KeyCode
            Case Keys.Apps 'right click menu
                If ContextMenuStrip.Visible Then
                    'menu is open already ... so hide
                    ContextMenuStrip.Close()
                Else
                    MenuSpellClickReturn = New MenuSpellClickReturnItem(parentTextBox, False)
                End If
        End Select
    End Sub

#End Region

#Region "Class to get info on cursor/word location when menu is opened"

    'Used to get information on the location that was clicked on from a content menu
    Public Class MenuSpellClickReturnItem

        Public Word As String
        Public WordStart As Integer
        Public WordEnd As Integer
        Public UseMouseLocation As Boolean

        Public Sub New(ByVal TextBox As TextBoxBase, Optional ByVal UseMouseLocation As Boolean = False)
            Dim CharIndex As Integer
            Me.UseMouseLocation = UseMouseLocation
            If UseMouseLocation = True Then
                'Work out the location that was clicked on
                Dim Point0 As New Point(0, 0)
                Dim cmLocationRelToTxtLocationX = System.Windows.Forms.Control.MousePosition.X - TextBox.PointToScreen(Point0).X - (TextBox.ClientRectangle.X)
                Dim cmLocationRelToTxtLocationY = System.Windows.Forms.Control.MousePosition.Y - TextBox.PointToScreen(Point0).Y
                'Get the char index for the location we clicked on
                CharIndex = TextBox.GetCharIndexFromPosition(New Point(cmLocationRelToTxtLocationX, cmLocationRelToTxtLocationY))
            Else
                'Use the location that the caret is...
                If TextBox.SelectionLength = 0 Then
                    'ok
                    CharIndex = TextBox.SelectionStart
                Else
                    'multi text selected
                    Exit Sub
                End If
            End If

            'Get the word that is @ the CharIndex
            Dim theText As String = Dictionary.Formatting.RemoveWordBreaks(TextBox.Text)
            Dim LeftSide As String = Left(theText, CharIndex)
            Dim RightSide As String = Right(theText, Len(theText) - CharIndex)
            LeftSide = LeftSide.Split(" "c).Last
            RightSide = RightSide.Split(" "c).First
            Word = LeftSide & RightSide



            'and fill the chr indexes
            WordStart = CharIndex - Len(LeftSide)
            WordEnd = CharIndex + Len(RightSide)

        End Sub

    End Class

#End Region

#Region "Standard context menu"

    Public Class StandardContextMenuStrip

#Region "Buttons"

#Region "Button Class Objects"

        Public Interface StandardToolStripItem
            'Placeholder
        End Interface

        Public Class StandardToolStripSeparator
            Inherits ToolStripSeparator
            Implements StandardToolStripItem
        End Class

        Public Class StandardToolStripMenuItem
            Inherits ToolStripMenuItem
            Implements StandardToolStripItem

            Public Sub New(ByVal Text As String, ByVal Image As Image)
                MyBase.New(Text, Image)
            End Sub

            Public Sub New()

            End Sub
        End Class

#End Region

#Region "Actions"

        Private WithEvents Redo As ToolStripMenuItem
        Private Sub Redo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Redo.Click
            Dim RichTextBox = TryCast(TextBox, RichTextBox)
            If RichTextBox IsNot Nothing Then
                RichTextBox.Redo()
            End If
        End Sub

        Private WithEvents Undo As ToolStripMenuItem
        Private Sub Undo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Undo.Click
            TextBox.Undo()
        End Sub

        Private WithEvents Cut As ToolStripMenuItem
        Private Sub Cut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cut.Click
            TextBox.Cut()
        End Sub

        Private WithEvents Copy As ToolStripMenuItem
        Private Sub Copy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Copy.Click
            TextBox.Copy()
        End Sub

        Private WithEvents Paste As ToolStripMenuItem
        Private Sub Paste_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Paste.Click
            TextBox.Paste()
        End Sub

        Private WithEvents Delete As ToolStripMenuItem
        Private Sub Delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Delete.Click
            'lock window from updating
            extTextBoxCommon.LockWindowUpdate(TextBox.Handle)
            TextBox.SuspendLayout()

            Dim SelStart = TextBox.SelectionStart

            Dim mc_parentRichTextBox = TryCast(TextBox, RichTextBox)
            If mc_parentRichTextBox IsNot Nothing Then
                'rich text box :(... use alternate method ... this will ensure that the formatting isn't lost
                mc_parentRichTextBox.Select(SelStart, TextBox.SelectionLength)
                mc_parentRichTextBox.SelectedText = ""
            Else
                Dim OldVertPos = extTextBoxCommon.GetScrollBarLocation(TextBox)

                'replace the text
                TextBox.Text = Strings.Left(TextBox.Text, SelStart) & _
                                  Strings.Right(TextBox.Text, Len(TextBox.Text) - (TextBox.SelectionStart + TextBox.SelectionLength))
                '... and select the replaced text
                TextBox.SelectionStart = SelStart

                'Set scroll bars to what they were
                extTextBoxCommon.SetScrollBarLocation(TextBox, OldVertPos) ' set Vscroll to last saved pos.
            End If

            'unlock window updates
            TextBox.ResumeLayout()
            extTextBoxCommon.LockWindowUpdate(IntPtr.Zero)
        End Sub

        Private WithEvents SelectAll As ToolStripMenuItem
        Private Sub SelectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SelectAll.Click
            TextBox.SelectAll()
            TextBox.Focus()
        End Sub

#End Region

#End Region

        Public Event PreAddingMenuItems(ByVal sender As Object, ByVal e As EventArgs)
        Public Event PostAddingMenuItems(ByVal sender As Object, ByVal e As EventArgs)

        Private TextBox As TextBoxBase

        Public Sub New(ByVal TextBox As TextBoxBase)
            Me.TextBox = TextBox
        End Sub

        Public Sub AddStandardItems(ByVal ContextMenuStrip As ContextMenuStrip)

            RaiseEvent PreAddingMenuItems(Me, EventArgs.Empty)

            If ContextMenuStrip.Items.Count > 0 Then
                ContextMenuStrip.Items.Add(New StandardToolStripSeparator)
            End If

            Undo = New StandardToolStripMenuItem("&Undo", My.Resources.Undo)
            ContextMenuStrip.Items.Add(Undo)

            Dim RichTextBox = TryCast(TextBox, RichTextBox)
            If RichTextBox IsNot Nothing Then
                Redo = New StandardToolStripMenuItem("&Redo", My.Resources.Redo)
                ContextMenuStrip.Items.Add(Redo)
                Redo.Enabled = RichTextBox.CanRedo
            End If

            ContextMenuStrip.Items.Add(New StandardToolStripSeparator)

            If System.Threading.Thread.CurrentThread.GetApartmentState = Threading.ApartmentState.STA Then
                'allow cut/copy/paste - doesn't work unless STA thread
                Cut = New StandardToolStripMenuItem("Cu&t", My.Resources.Cut)
                ContextMenuStrip.Items.Add(Cut)
                Copy = New StandardToolStripMenuItem("&Copy", My.Resources.Copy)
                ContextMenuStrip.Items.Add(Copy)
                Paste = New StandardToolStripMenuItem("&Paste", My.Resources.Paste)
                ContextMenuStrip.Items.Add(Paste)

                Cut.Enabled = TextBox.SelectionLength > 0
                Copy.Enabled = TextBox.SelectionLength > 0
                Paste.Enabled = Clipboard.GetText <> ""
            End If

            Delete = New StandardToolStripMenuItem("&Delete", My.Resources.Delete)
            ContextMenuStrip.Items.Add(Delete)

            ContextMenuStrip.Items.Add(New StandardToolStripSeparator)

            SelectAll = New StandardToolStripMenuItem("Select &All", My.Resources.SelectAll)
            ContextMenuStrip.Items.Add(SelectAll)

            'enable / disable

            Undo.Enabled = TextBox.CanUndo

            Delete.Enabled = TextBox.SelectionLength > 0

            SelectAll.Enabled = TextBox.SelectionLength <> Len(TextBox.Text)

            RaiseEvent PostAddingMenuItems(Me, EventArgs.Empty)

        End Sub

        Friend Sub RemoveStandardItems(ByVal ContextMenuStrip As ContextMenuStrip)
            For Each item In ContextMenuStrip.Items.OfType(Of StandardToolStripItem)().OfType(Of ToolStripItem).ToArray
                ContextMenuStrip.Items.Remove(item)
                item.Dispose()
            Next
        End Sub

    End Class

#End Region

#Region "Actual Menu"

    Public MenuSpellClickReturn As MenuSpellClickReturnItem
    Public LastMenuSpellClickReturn As MenuSpellClickReturnItem

    Public WithEvents ContextMenuStrip As ContextMenuStrip

    Dim tsiPlaceHolder As New ToolStripMenuItem() 'this is used as you cannot have a context menu with no items in it :(

    Private Sub parentTextBox_ContextMenuStripChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mc_TextBox.ContextMenuStripChanged
        ContextMenuStrip = parentTextBox.ContextMenuStrip
        If ContextMenuStrip Is Nothing Then
            ContextMenuStrip = New ContextMenuStrip
            ContextMenuStrip.Items.Add(tsiPlaceHolder)
            parentTextBox.ContextMenuStrip = ContextMenuStrip
        End If
    End Sub

    Protected WithEvents StandardCMS As StandardContextMenuStrip

    Protected Event PreAddingStandardMenuItems(ByVal sender As Object, ByVal e As EventArgs)
    Protected Event PostAddingStandardMenuItems(ByVal sender As Object, ByVal e As EventArgs)

    Private Sub StandardCMS_PostAddingMenuItems(ByVal sender As Object, ByVal e As System.EventArgs) Handles StandardCMS.PostAddingMenuItems
        RaiseEvent PostAddingStandardMenuItems(Me, e)
    End Sub

    Private Sub StandardCMS_PreAddingMenuItems(ByVal sender As Object, ByVal e As System.EventArgs) Handles StandardCMS.PreAddingMenuItems
        RaiseEvent PreAddingStandardMenuItems(Me, e)
    End Sub

    Public Event MenuClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs)

    Private Sub ContextMenuStrip_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles ContextMenuStrip.Closed
        'qwertyuiop - can't check for SourceControl here unfortunantly ...
        'as if the ContextMenuStrip menu was open and user right-clicks somewhere else by the time the ContextMenuStrip.Closed event is
        'called the .SourceControl property has already been changed even though it was opened on this text box :(
        'If DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl IsNot MyBase.Control Then Return

        LastMenuSpellClickReturn = MenuSpellClickReturn
        MenuSpellClickReturn = Nothing

        RaiseEvent MenuClosed(Me, e)

        'If ContextMenuStrip.Items.Count = 0 Then
        '    ContextMenuStrip.Items.Add(tsiPlaceHolder)
        'End If
    End Sub

    Private Sub ContextMenuStrip_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenuStrip.LocationChanged
        If DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl IsNot MyBase.Control Then Return

        Static LocationChanging As Boolean
        If LocationChanging Then Exit Sub
        LocationChanging = True
        If ContextMenuStrip.SourceControl Is parentTextBox Then
            If MenuSpellClickReturn IsNot Nothing AndAlso MenuSpellClickReturn.UseMouseLocation = False Then 'If keyboard context menu was pressed ...
                Dim txtBoxScreenPos = Me.parentTextBox.PointToScreen(Point.Empty)
                Dim AddX = txtBoxScreenPos.X
                Dim AddY = txtBoxScreenPos.Y

                extTextBoxCommon.ScrollToCaret(parentTextBox)

                Dim ChrPos = Me.parentTextBox.GetPositionFromCharIndex(parentTextBox.SelectionStart)
                Dim LineHeight As Integer = extTextBoxCommon.GetLineHeightFromCharPosition(parentTextBox, parentTextBox.SelectionStart, RTBContents)

                ContextMenuStrip.Top = AddY + ChrPos.Y + LineHeight
                ContextMenuStrip.Left = AddX + ChrPos.X
            End If
        End If

        CheckContextMenuLocation()
        LocationChanging = False
    End Sub

    Dim RTBContents As Rectangle
    Private Sub mc_RichTextBox_ContentsResized(ByVal sender As Object, ByVal e As System.Windows.Forms.ContentsResizedEventArgs) Handles mc_RichTextBox.ContentsResized
        RTBContents = e.NewRectangle
    End Sub

    Private Sub ContextMenuStrip_ItemAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemEventArgs) Handles ContextMenuStrip.ItemAdded
        If DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl IsNot MyBase.Control Then Return

        CheckContextMenuLocation()
    End Sub

    Private Sub CheckContextMenuLocation()
        Static isWorking As Boolean = False
        If isWorking Then Exit Sub
        isWorking = True
        Dim ScreenAtPoint = Screen.FromPoint(New Point(ContextMenuStrip.Left, ContextMenuStrip.Top))
        Dim PopupLocation = Control.MousePosition
        If MenuSpellClickReturn IsNot Nothing AndAlso MenuSpellClickReturn.UseMouseLocation = False Then
            'If keyboard context menu was pressed ... use carrot location instead
            'qwertyuiop hrm doesn't seem to work atm ... 
            'PopupLocation = Me.parentTextBox.GetPositionFromCharIndex(parentTextBox.SelectionStart)
            'PopupLocation = Me.parentTextBox.PointToScreen(PopupLocation)
            GoTo AllDone
        End If
        If ContextMenuStrip.Right >= ScreenAtPoint.WorkingArea.Right - 16 Then
            ContextMenuStrip.Left = PopupLocation.X - ContextMenuStrip.Width
        End If
        If ContextMenuStrip.Bottom >= ScreenAtPoint.WorkingArea.Bottom - 16 Then
            ContextMenuStrip.Top = PopupLocation.Y - ContextMenuStrip.Height
        End If
AllDone:
        isWorking = False
    End Sub

    Private Sub ContextMenuStrip_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenuStrip.SizeChanged
        If DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl IsNot MyBase.Control Then Return

        CheckContextMenuLocation()
    End Sub

    Public Event MenuOpening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)

    Private Sub ContextMenuStrip_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip.Opening
        If DirectCast(sender, System.Windows.Forms.ContextMenuStrip).SourceControl IsNot MyBase.Control Then Return

        If ContextMenuStrip.Visible = True Then
            'already showing ... no need to run this...
            Exit Sub
        End If

        ContextMenuStrip.Items.Remove(tsiPlaceHolder)

        If StandardCMS Is Nothing Then
            StandardCMS = New StandardContextMenuStrip(parentTextBox)
        End If
        StandardCMS.RemoveStandardItems(ContextMenuStrip)

        StandardCMS.AddStandardItems(ContextMenuStrip)

        'MenuSpellClickReturn = Nothing
        If ContextMenuStrip.SourceControl Is parentTextBox Then
            If MenuSpellClickReturn IsNot Nothing Then
                'use the existing MenuSpellClickReturn - right click button was pressed on keyboard
                'show it at location of the cursor instead of the center?
                'qwertyuiop - DOESN'T WORK HERE?
                'moved to ContextMenuStrip_LocationChanged
            Else
                'use mouse location as this was fired with a right click
                MenuSpellClickReturn = New MenuSpellClickReturnItem(parentTextBox, True)
            End If
        End If

        RaiseEvent MenuOpening(sender, e)

    End Sub

    Private WithEvents SpellMenuItems As New Menu.AddSpellItemsToMenu()

#End Region

End Class
