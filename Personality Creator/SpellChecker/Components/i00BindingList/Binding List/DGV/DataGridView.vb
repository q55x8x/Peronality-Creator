'i00 BindingList with DataGridView
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

<System.ComponentModel.DesignerCategory("")> _
Public Class DataGridView
    Inherits System.Windows.Forms.DataGridView

#Region "Properties"

    Dim mc_DrawUIEditorCells As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    Public Property DrawUIEditorCells() As Boolean
        Get
            Return mc_DrawUIEditorCells
        End Get
        Set(ByVal value As Boolean)
            If mc_DrawUIEditorCells <> value Then
                mc_DrawUIEditorCells = value
                MyBase.Invalidate()
            End If
        End Set
    End Property

    Dim mc_AllowUIEditorCells As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    Public Property AllowUIEditorCells() As Boolean
        Get
            Return mc_AllowUIEditorCells
        End Get
        Set(ByVal value As Boolean)
            mc_AllowUIEditorCells = value
        End Set
    End Property

    Dim mc_NoImageText As String = "No Image"
    <System.ComponentModel.DefaultValue("No Image")> _
    Public Property NoImageText() As String
        Get
            Return mc_NoImageText
        End Get
        Set(ByVal value As String)
            mc_NoImageText = value
        End Set
    End Property

    Dim mc_NoImageImage As Image = Nothing
    <System.ComponentModel.DefaultValue(GetType(Image), Nothing)> _
    Public Property NoImageImage() As Image
        Get
            Return mc_NoImageImage
        End Get
        Set(ByVal value As Image)
            mc_NoImageImage = value
        End Set
    End Property

#End Region

#Region "Events"

    Public Event AddingFilterPlugin(ByVal sender As Object, ByVal e As FilterEventArgs)

    Public Class FilterEventArgs
        Inherits EventArgs
        Public Column As DataGridViewColumn
        Public Plugin As Plugins.iFilterPlugin
        Public Cancel As Boolean
    End Class

    Protected Overridable Sub OnAddingFilterPlugin(ByVal e As FilterEventArgs)
        RaiseEvent AddingFilterPlugin(Me, e)
    End Sub

#End Region

#Region "Drawing"

    'give some space for the UIEditors..
    Protected Overrides Sub OnColumnAdded(ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs)
        If mc_DrawUIEditorCells AndAlso e.Column.ValueType IsNot Nothing Then
            If e.Column.ValueType IsNot GetType(System.Drawing.Image) AndAlso PropertyEditor.PaintProperty(e.Column.ValueType, Nothing, Nothing, GetColumnOverrideTypeEditor(e.Column)) = True Then
                If e.Column.CellTemplate.Style.Padding.Left < 22 Then
                    e.Column.CellTemplate.Style.Padding = New Padding(22, e.Column.CellTemplate.Style.Padding.Top, e.Column.CellTemplate.Style.Padding.Right, e.Column.CellTemplate.Style.Padding.Bottom)
                End If
            End If
        End If
        MyBase.OnColumnAdded(e)
    End Sub

    'redraw the column header when the mouse leaves the control
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)

        If HeaderMenu.Visible Then Exit Sub 'don't redraw if we have the menu open :)
        If Me.OverColumnIndex <> -1 Then
            Me.InvalidateCell(Me.OverColumnIndex, -1)
        End If
        OverColumnIndex = -1
    End Sub

    'redraw the column header when the menu is closed
    Private Sub HeaderMenu_Closed(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs) Handles HeaderMenu.Closed
        OnMouseLeave(EventArgs.Empty)
    End Sub

    Public Sub New()
        MyBase.DoubleBuffered = True
    End Sub

    'draw the column headers
    Protected Overrides Sub OnCellPainting(ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs)
        MyBase.OnCellPainting(e)
        If e.Handled = False Then
            e.Graphics.SetClip(Me.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True))
            If e.RowIndex = -1 Then
                Static FilterImage As Image = My.Resources.Filter 'cache for filter image.. faster than looking up the image each from Resources time...
                If e.ColumnIndex = -1 Then
                    If Me.GetFilter <> "" Then
                        'draw filter
                        e.PaintBackground(e.CellBounds, True)
                        e.Graphics.DrawImage(FilterImage, New Rectangle(e.CellBounds.Left + (e.CellBounds.Width - FilterImage.Width - 2), e.CellBounds.Top + CInt((e.CellBounds.Height - FilterImage.Height) / 2), FilterImage.Width, FilterImage.Height))
                        e.Handled = True
                    End If
                Else
                    Dim Column = Me.Columns(e.ColumnIndex)

                    Dim HStyle = Column.HeaderCell.InheritedStyle
                    If Column IsNot Nothing Then

                        Dim CellBounds = New Rectangle(e.CellBounds.X, e.CellBounds.Y + 1, e.CellBounds.Width, e.CellBounds.Height - 2)
                        Dim WidgetButtonBounds = New Rectangle(CellBounds.Right - 16, CellBounds.Y, 16, CellBounds.Height)

                        If OverColumnIndex = e.ColumnIndex Then
                            e.PaintBackground(e.CellBounds, True)
                            If System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsSupported Then
                                Dim r = New System.Windows.Forms.VisualStyles.VisualStyleRenderer("Header", 1, 1)
                                r.DrawBackground(e.Graphics, CellBounds)
                            End If
                            'e.Graphics.FillRectangle(Brushes.White, e.CellBounds)

                            Dim HighlightBounds As Rectangle
                            If isOverWidget Then
                                HighlightBounds = WidgetButtonBounds
                            Else
                                HighlightBounds = CellBounds
                            End If
                            If System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsSupported Then
                                Dim r = New System.Windows.Forms.VisualStyles.VisualStyleRenderer("Header", 1, If((Control.MouseButtons And Windows.Forms.MouseButtons.Left) = Windows.Forms.MouseButtons.Left, 3, 2))
                                r.DrawBackground(e.Graphics, HighlightBounds)
                            Else
                                If isOverWidget Then
                                    Using p As New Pen(Color.FromKnownColor(KnownColor.ControlLightLight))
                                        e.Graphics.DrawRectangle(p, HighlightBounds)
                                    End Using
                                    Using p As New Pen(Color.FromKnownColor(KnownColor.ControlDark))
                                        e.Graphics.DrawLine(p, HighlightBounds.Left, HighlightBounds.Bottom, HighlightBounds.Right, HighlightBounds.Bottom)
                                        e.Graphics.DrawLine(p, HighlightBounds.Right, HighlightBounds.Top, HighlightBounds.Right, HighlightBounds.Bottom)
                                    End Using
                                End If
                            End If
                            'e.Graphics.FillRectangle(Brushes.Blue, HighlightBounds)
                            'e.PaintBackground(HighlightBounds, True)
                            If Column.IsDataBound AndAlso (ColumnSupportsSorting(Column) OrElse Me.SupportsFiltering <> FilteringMethods.None) AndAlso Column.ValueType IsNot GetType(System.Drawing.Image) Then
                                Dim ShowDropDownArrow = True
                                Select Case Me.SupportsFiltering()
                                    Case FilteringMethods.Basic
                                        'qwertyuiop
                                        'NOT IMPLEMENTED
                                    Case FilteringMethods.Advanced
                                        Dim bs = DirectCast(Me.DataSource, AdvancedBindingSource)
                                        Dim ThisFilterItem = (From xItem In bs.BasicFilters Where xItem.Field = Column.DataPropertyName).FirstOrDefault
                                        If ThisFilterItem IsNot Nothing Then
                                            e.Graphics.DrawImage(FilterImage, New Rectangle(e.CellBounds.Left + (e.CellBounds.Width - FilterImage.Width), e.CellBounds.Top + CInt((e.CellBounds.Height - FilterImage.Height) / 2), FilterImage.Width, FilterImage.Height))
                                            ShowDropDownArrow = False
                                        End If
                                    Case Else
                                End Select

                                If ShowDropDownArrow Then
                                    'show the widget dropdown
                                    If System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsSupported Then
                                        Dim r = New System.Windows.Forms.VisualStyles.VisualStyleRenderer("Toolbar", 4, If((Control.MouseButtons And Windows.Forms.MouseButtons.Left) = Windows.Forms.MouseButtons.Left, 1, 4))
                                        r.DrawBackground(e.Graphics, WidgetButtonBounds)
                                    Else
                                        'drop down arrow
                                        Dim tsr As New ToolStripProfessionalRenderer
                                        Using tsmi As New ToolStripMenuItem
                                            tsr.DrawArrow(New ToolStripArrowRenderEventArgs(e.Graphics, tsmi, WidgetButtonBounds, HStyle.ForeColor, ArrowDirection.Down))
                                        End Using
                                    End If
                                End If
                                WidgetButtonBounds.X -= WidgetButtonBounds.Width

                            End If
                        Else
                            e.PaintBackground(e.CellBounds, True)
                        End If

                        If Column.SortMode <> DataGridViewColumnSortMode.NotSortable Then
                            Select Case Column.HeaderCell.SortGlyphDirection
                                Case Windows.Forms.SortOrder.Ascending
                                    If System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsSupported Then
                                        Dim r = New System.Windows.Forms.VisualStyles.VisualStyleRenderer("Header", 4, 1)
                                        r.DrawBackground(e.Graphics, WidgetButtonBounds)
                                    Else
                                        'drop down arrow
                                        Dim tsr As New ToolStripProfessionalRenderer
                                        Using tsmi As New ToolStripMenuItem
                                            tsr.DrawArrow(New ToolStripArrowRenderEventArgs(e.Graphics, tsmi, WidgetButtonBounds, HStyle.ForeColor, ArrowDirection.Up))
                                        End Using
                                    End If
                                Case Windows.Forms.SortOrder.Descending
                                    If System.Windows.Forms.VisualStyles.VisualStyleRenderer.IsSupported Then
                                        Dim r = New System.Windows.Forms.VisualStyles.VisualStyleRenderer("Header", 4, 2)
                                        r.DrawBackground(e.Graphics, WidgetButtonBounds)
                                    Else
                                        'drop down arrow
                                        Dim tsr As New ToolStripProfessionalRenderer
                                        Using tsmi As New ToolStripMenuItem
                                            tsr.DrawArrow(New ToolStripArrowRenderEventArgs(e.Graphics, tsmi, WidgetButtonBounds, HStyle.ForeColor, ArrowDirection.Down))
                                        End Using
                                    End If
                                Case Else
                                    WidgetButtonBounds.X += WidgetButtonBounds.Width
                            End Select
                        Else
                            WidgetButtonBounds.X += WidgetButtonBounds.Width
                        End If

                        Dim Padding = HStyle.Padding
                        If Padding = Nothing Then
                            Padding = New Padding(4, 0, 0, 0)
                        End If

                        Dim ContentBounds = e.CellBounds 'Me.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
                        ContentBounds = New Rectangle(ContentBounds.X + Padding.Left, ContentBounds.Y + Padding.Top, ContentBounds.Width - (Padding.Left + Padding.Right), ContentBounds.Height - (Padding.Top + Padding.Bottom))
                        If ContentBounds.Right > WidgetButtonBounds.X Then
                            ContentBounds.Width = WidgetButtonBounds.X - ContentBounds.Left
                        End If
                        Dim TextStyle = DrawingText.TextRendererMeasure.TextFlagsFrom(HStyle)
                        If (TextStyle And TextFormatFlags.WordBreak) = TextFormatFlags.WordBreak Then
                            TextStyle = TextStyle Xor TextFormatFlags.WordBreak
                        End If
                        TextRenderer.DrawText(e.Graphics, Column.HeaderText, HStyle.Font, ContentBounds, HStyle.ForeColor, TextStyle)
                        e.Handled = True
                    End If
                    End If
            Else

                    'data
                    '
                    If e.ColumnIndex = -1 Then

                    Else
                        Dim Column = Me.Columns(e.ColumnIndex)
                        If Column IsNot Nothing Then

                            e.Handled = True
                            If Column.ValueType Is GetType(System.Drawing.Image) Then
                                e.PaintBackground(e.ClipBounds, True)
                                If e.Value Is Nothing Then
                                    If mc_NoImageImage IsNot Nothing Then
                                        'draw the image bg... for no image...
                                        Dim Image = NoImageImage
                                        Dim ImageBounds = New Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1)
                                        Dim BestFitRect = DrawingFunctions.GetBestFitRect(ImageBounds, New RectangleF(0, 0, Image.Width, Image.Height), DrawingFunctions.BestFitStyle.Stretch Or DrawingFunctions.BestFitStyle.DoNotAllowEnlarge)
                                        Using b As New Bitmap(CInt(BestFitRect.Width), CInt(BestFitRect.Height))
                                            Using g = Graphics.FromImage(b)
                                                g.InterpolationMode = Drawing2D.InterpolationMode.High
                                                g.TranslateTransform(-BestFitRect.X, -BestFitRect.Y)
                                                g.DrawImage(Image, BestFitRect)
                                            End Using
                                            b.Filters.Alpha() 'with alpha
                                            e.Graphics.DrawImageUnscaled(b, New Point(CInt(BestFitRect.Location.X), CInt(BestFitRect.Location.Y)))
                                        End Using
                                    End If
                                    TextRenderer.DrawText(e.Graphics, NoImageText, e.CellStyle.Font, e.CellBounds, DrawingFunctions.BlendColor(e.CellStyle.ForeColor, e.CellStyle.BackColor), TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or TextFormatFlags.NoPrefix Or TextFormatFlags.PreserveGraphicsClipping Or TextFormatFlags.PreserveGraphicsTranslateTransform)
                                Else
                                    Dim Image = DirectCast(e.Value, Image)
                                    e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.High
                                    Dim ImageBounds = New Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1)
                                    DrawingFunctions.DrawImageBestFit(e.Graphics, ImageBounds, Image, DrawingFunctions.BestFitStyle.Stretch Or DrawingFunctions.BestFitStyle.DoNotAllowEnlarge)
                                End If
                            Else

                                e.PaintBackground(e.ClipBounds, True)

                                If mc_DrawUIEditorCells Then

                                    If e.Value IsNot Nothing Then
                                        Dim PaintPropertyRect = New Rectangle(e.CellBounds.X + 2, CInt(Int(e.CellBounds.Y + ((e.CellBounds.Height - 15) / 2))), 19, 15)
                                        'Dim PaintPropertyRect = New Rectangle(e.CellBounds.X + 2, e.CellBounds.Y + 2, 19, e.CellBounds.Height - 6)
                                        If PropertyEditor.PaintProperty(e.Value, e.Graphics, PaintPropertyRect, GetColumnOverrideTypeEditor(Column)) = True Then
                                            'Dim NiceText = System.ComponentModel.TypeDescriptor.GetConverter(e.Value).ConvertToString(e.Value)
                                            e.Graphics.DrawRectangle(System.Drawing.SystemPens.WindowText, PaintPropertyRect)
                                        End If
                                    End If
                                End If

                                Select Case Me.SupportsFiltering()
                                    Case FilteringMethods.Basic
                                        'qwertyuiop
                                        'NOT IMPLEMENTED
                                    Case FilteringMethods.Advanced
                                        Dim bs = DirectCast(Me.DataSource, AdvancedBindingSource)
                                        Dim ThisFilterItem = (From xItem In bs.BasicFilters.OfType(Of AdvancedBindingSource.BasicFilter)() Where xItem.Field = Column.DataPropertyName).FirstOrDefault
                                        If ThisFilterItem IsNot Nothing Then
                                            'filtered...

                                            Dim RegExString = System.Text.RegularExpressions.Regex.Escape(ThisFilterItem.Filter)
                                            RegExString = Replace(RegExString, "\?", ".")
                                            RegExString = Replace(RegExString, "\*", ".{0,}")
                                            Dim FindResult = System.Text.RegularExpressions.Regex.Match(e.FormattedValue.ToString, RegExString, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                                            If FindResult.Success Then
                                                Dim CellBounds = New Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 1, e.CellBounds.Height - 1)

                                                Dim CellSize = CellBounds.Size
                                                CellSize.Width -= e.CellStyle.Padding.Left + e.CellStyle.Padding.Right
                                                CellSize.Height -= e.CellStyle.Padding.Top + e.CellStyle.Padding.Bottom

                                                Dim WordBounds = DrawingText.TextRendererMeasure.Measure(e.FormattedValue.ToString, e.CellStyle.Font, CellSize, DrawingText.TextRendererMeasure.TextFlagsFrom(e.CellStyle), , False)

                                                Dim StartIndex = (From xItem In WordBounds Where xItem.LetterIndex = FindResult.Index).FirstOrDefault
                                                Dim EndIndex = (From xItem In WordBounds Where xItem.LetterIndex = FindResult.Index + (FindResult.Length - 1)).FirstOrDefault

                                                If StartIndex IsNot Nothing AndAlso EndIndex IsNot Nothing Then
                                                    Dim HighlightRect = New Rectangle(StartIndex.Bounds.X + CellBounds.X + e.CellStyle.Padding.Left, StartIndex.Bounds.Y + CellBounds.Y + e.CellStyle.Padding.Top, EndIndex.Bounds.Right - StartIndex.Bounds.X, EndIndex.Bounds.Bottom - StartIndex.Bounds.Y)

                                                    Using sb As New SolidBrush(Color.FromArgb(127, e.CellStyle.BackColor))
                                                        e.Graphics.FillRectangle(sb, HighlightRect)
                                                    End Using

                                                    Using sb As New SolidBrush(Color.FromArgb(63, Color.FromKnownColor(KnownColor.Highlight)))
                                                        e.Graphics.FillRectangle(sb, HighlightRect)
                                                        'Using p As New Pen(sb)
                                                        '    e.Graphics.DrawRectangle(p, HighlightRect)
                                                        'End Using
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    Case Else
                                End Select
                                e.PaintContent(e.ClipBounds)
                            End If
                        End If
                    End If
            End If
            e.Graphics.ResetClip()
        End If
    End Sub

#End Region

#Region "Property Editor"

    Private WithEvents OwnerForm As Form

    Dim DropDownCancelled As Boolean

    Private Sub CancelDropDown()
        DropDownCancelled = True
        PropertyEditor.CloseDropDown()
    End Sub

    Private Sub OwnerForm_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles OwnerForm.Deactivate
        CancelDropDown()
    End Sub

    Dim AllowOnCellBeginEdit As Boolean = True

    Protected Overrides Sub OnCellBeginEdit(ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs)
        If e.ColumnIndex <> -1 AndAlso AllowUIEditorCells AndAlso AllowOnCellBeginEdit Then
            Dim Column = Me.Columns(e.ColumnIndex)
            Dim CellBounds = Me.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, True)
            OwnerForm = Me.FindForm
            DropDownCancelled = False

            Dim t As New System.Threading.Thread(AddressOf OpenPropWindow)
            t.IsBackground = True
            t.Name = "Property Editor"
            t.Start(New PropEditPrams With {.Value = Me.CurrentCell.Value, .DataType = Me.CurrentCell.ValueType, .Location = Me.PointToScreen(New Point(CellBounds.X, CellBounds.Bottom)), .OverrideTypeEditor = GetColumnOverrideTypeEditor(Column)})

        End If

        MyBase.OnCellBeginEdit(e)
    End Sub

    Private WithEvents PropertyEditor As New PropertyEditor

    Private Class PropEditPrams
        Public Value As Object
        Public Location As Point
        Public DataType As Type
        Public OverrideTypeEditor As System.Reflection.MemberInfo
    End Class

    Private Sub OpenPropWindow(ByVal oPropEditPrams As Object)
        Dim PropEditPrams = TryCast(oPropEditPrams, PropEditPrams)
        If PropEditPrams IsNot Nothing Then
            'PropertyEditor.TopMost = True
            Try
                Dim Result = PropertyEditor.ShowPropertyEditor(If(PropEditPrams.Value Is Nothing, PropEditPrams.DataType, PropEditPrams.Value), PropEditPrams.Location, PropEditPrams.OverrideTypeEditor)
                If Me.IsCurrentCellInEditMode AndAlso DropDownCancelled = False Then
                    'send the result back to the cell.. we selected something :)
                    SetCurrentCellData(Result)
                End If
            Catch ex As NotSupportedException

            End Try
        End If
        OwnerForm = Nothing
    End Sub

    Delegate Sub SetCurrentCellData_cb(ByVal Data As Object)
    Private Sub SetCurrentCellData(ByVal Data As Object)
        If Me.InvokeRequired Then
            Dim SetCurrentCellData_cb As New SetCurrentCellData_cb(AddressOf SetCurrentCellData)
            Me.Invoke(SetCurrentCellData_cb, Data)
        Else
            'should just be able to call:
            'Me.EndEdit()
            'Me.CurrentCell.Value = Data
            '...but the above doesn't work properly ... so had to use this dodge method:
            AllowOnCellBeginEdit = False
            Me.EndEdit()
            Me.CurrentCell.Value = Data
            Me.BeginEdit(False)
            Me.NotifyCurrentCellDirty(True)
            Me.EndEdit()
            AllowOnCellBeginEdit = True
        End If
    End Sub

    Protected Overrides Sub OnCellDoubleClick(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        MyBase.OnCellDoubleClick(e)
        If e.ColumnIndex <> -1 AndAlso e.RowIndex <> -1 Then
            Dim Column = Me.Columns(e.ColumnIndex)
            If Column.ValueType Is GetType(System.Drawing.Image) AndAlso AllowUIEditorCells Then
                'show editor...
                Try
                    Dim toBeValue = PropertyEditor.ShowPropertyEditor(If(Me.CurrentCell.Value Is Nothing, Column.ValueType, Me.CurrentCell.Value), Point.Empty, GetColumnOverrideTypeEditor(Column))
                    If TypeOf toBeValue Is Image Then
                        Me.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = toBeValue
                    End If
                Catch ex As Exception
                    MsgBox("Error setting image: " & ex.Message, MsgBoxStyle.Exclamation)
                End Try
            End If
        End If
    End Sub

    Protected Overrides Sub OnCellEndEdit(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        CancelDropDown()
        MyBase.OnCellEndEdit(e)
    End Sub

    Private Function GetColumnOverrideTypeEditor(ByVal Column As DataGridViewColumn) As System.Reflection.MemberInfo
        'for generics ... eg... list(of T) ... all of this is to use the datasources UIEditor instead of the default one...
        Dim BaseType = If(DataSource IsNot Nothing, Me.DataSource.GetType.GetGenericArguments.FirstOrDefault, Nothing)
        Dim OverrideTypeEditor As System.Reflection.MemberInfo = Nothing
        If BaseType Is Nothing Then
            'we could also be an AdvancedBindingSource :)
            If Me.SupportsFiltering = FilteringMethods.Advanced Then
                Dim AdvancedBindingSource = DirectCast(Me.DataSource, AdvancedBindingSource)
                BaseType = AdvancedBindingSource.BaseType
            End If
        End If
        If BaseType IsNot Nothing Then
            OverrideTypeEditor = BaseType.GetProperty(Column.DataPropertyName)
        End If
        Return OverrideTypeEditor
    End Function

#End Region

#Region "Nice Data Errors"

    Protected Overrides Sub OnDataError(ByVal displayErrorDialogIfNoHandler As Boolean, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)
        e.ThrowException = True
        MyBase.OnDataError(displayErrorDialogIfNoHandler, e)
        If e.ThrowException Then
            If e.Exception IsNot Nothing Then
                Dim ErrorText As String = e.Exception.Message

                If e.Exception.InnerException IsNot Nothing Then
                    If TypeOf (e.Exception.InnerException) Is FormatException Then
                        ErrorText = "The value you have entered is not valid valid for this field."
                    End If
                End If

                'If MsgBox(ErrorText & vbCrLf & vbCrLf & "Do you want to revert the changes?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                '    Me.CancelEdit()
                'End If
                MsgBox(ErrorText, MsgBoxStyle.Exclamation)

            End If
        End If
        e.ThrowException = False
    End Sub

    Private Sub DataGridView_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Me.DataError
        'required for nice errors :)
    End Sub

#End Region

#Region "Cell sizing"

    ''These are to make cells size nicely to the image size...

    'Protected Overrides Sub OnColumnDividerDoubleClick(ByVal e As System.Windows.Forms.DataGridViewColumnDividerDoubleClickEventArgs)

    '    Dim ImageCells = (From xItem In MyBase.Rows.OfType(Of DataGridViewRow)() Where xItem.Cells(e.ColumnIndex).ValueType Is GetType(System.Drawing.Image) Select xItem.Cells(e.ColumnIndex)).ToList
    '    Dim MaxCellWidth = 22
    '    Dim CellWidths = (From xItem In MyBase.Rows.OfType(Of DataGridViewRow)() Where xItem.Cells(e.ColumnIndex).RowIndex <> -1 AndAlso ImageCells.Contains(xItem.Cells(e.ColumnIndex)) = False Select xItem.Cells(e.ColumnIndex).ContentBounds.Width + xItem.Cells(e.ColumnIndex).InheritedStyle.Padding.Left + xItem.Cells(e.ColumnIndex).InheritedStyle.Padding.Right)
    '    If CellWidths.Count > 0 Then MaxCellWidth = CellWidths.Max
    '    'work out the ideal image width..
    '    For Each iItem In ImageCells
    '        Dim image = TryCast(iItem.Value, Image)
    '        If image IsNot Nothing Then
    '            Dim BestFitRect = DrawingFunctions.GetBestFitRect(New RectangleF(0, 0, Single.MaxValue, iItem.Size.Height), New RectangleF(0, 0, image.Width, image.Height), DrawingFunctions.BestFitStyle.Stretch Or DrawingFunctions.BestFitStyle.DoNotAllowEnlarge)
    '            If MaxCellWidth <= CInt(BestFitRect.Width) Then MaxCellWidth = CInt(BestFitRect.Width)
    '        End If
    '    Next

    '    MyBase.Columns(e.ColumnIndex).Width = MaxCellWidth + 4
    '    'MyBase.OnColumnDividerDoubleClick(e)
    'End Sub

    'Protected Overrides Sub OnRowDividerDoubleClick(ByVal e As System.Windows.Forms.DataGridViewRowDividerDoubleClickEventArgs)

    '    Dim ImageCells = (From xItem In MyBase.Rows(e.RowIndex).Cells.OfType(Of DataGridViewCell)() Where xItem.ValueType Is GetType(System.Drawing.Image)).ToList
    '    Dim MaxCellHeight = 16
    '    Dim CellHeights = (From xItem In MyBase.Rows(e.RowIndex).Cells.OfType(Of DataGridViewCell)() Where xItem.ColumnIndex <> -1 AndAlso ImageCells.Contains(xItem) = False Select xItem.ContentBounds.Height + xItem.InheritedStyle.Padding.Top + xItem.InheritedStyle.Padding.Bottom)
    '    If CellHeights.Count > 0 Then MaxCellHeight = CellHeights.Max
    '    'work out the ideal image height..
    '    For Each iItem In ImageCells
    '        Dim image = TryCast(iItem.Value, Image)
    '        If image IsNot Nothing Then
    '            Dim BestFitRect = DrawingFunctions.GetBestFitRect(New RectangleF(0, 0, iItem.Size.Width, Single.MaxValue), New RectangleF(0, 0, image.Width, image.Height), DrawingFunctions.BestFitStyle.Stretch Or DrawingFunctions.BestFitStyle.DoNotAllowEnlarge)
    '            If MaxCellHeight <= CInt(BestFitRect.Height) Then MaxCellHeight = CInt(BestFitRect.Height)
    '        End If
    '    Next

    '    MyBase.Rows(e.RowIndex).Height = MaxCellHeight + 4
    '    'MyBase.OnRowDividerDoubleClick(e)
    'End Sub

#End Region

#Region "Widget Hit Test"

    Dim isOverWidget As Boolean = False
    Dim OverColumnIndex As Integer = -1

    Private Function ColumnSupportsSorting(ByVal Column As DataGridViewColumn) As Boolean
        If Column.SortMode = DataGridViewColumnSortMode.NotSortable Then Return False
        If Me.DataSource Is Nothing Then Return False
        If Not TypeOf Me.DataSource Is System.ComponentModel.IBindingList Then Return False
        Return True
    End Function

    'find out if we are over the widget?
    Protected Overrides Sub OnCellMouseMove(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
        MyBase.OnCellMouseMove(e)
        If e.RowIndex = -1 AndAlso e.ColumnIndex <> -1 Then
            Dim Column = Me.Columns(e.ColumnIndex)
            If Column IsNot Nothing Then
                If Column.IsDataBound AndAlso (ColumnSupportsSorting(Column) OrElse Me.SupportsFiltering <> FilteringMethods.None) AndAlso Column.ValueType IsNot GetType(System.Drawing.Image) Then
                    'show the widget dropdown highlight
                    Dim isOverWidget = False
                    If e.X >= Column.Width - 16 Then
                        isOverWidget = True
                    End If
                    If Me.isOverWidget <> isOverWidget Then
                        Me.isOverWidget = isOverWidget
                        Me.InvalidateCell(Column.Index, -1)
                    End If
                Else
                    Me.isOverWidget = False
                End If
            End If
        End If
        If e.RowIndex <> -1 Then
            If Me.OverColumnIndex <> -1 Then
                Me.InvalidateCell(Me.OverColumnIndex, -1)
            End If
            OverColumnIndex = -1
        Else
            If OverColumnIndex <> e.ColumnIndex Then
                If Me.OverColumnIndex <> -1 Then
                    Me.InvalidateCell(Me.OverColumnIndex, -1)
                End If
                OverColumnIndex = e.ColumnIndex
            End If
        End If
    End Sub

#End Region

#Region "Widget Menu"

#Region "Menu"

    Private WithEvents HeaderMenu As New WidgetContextMenuStrip(Me)

    Private Class WidgetContextMenuStrip
        Inherits ContextMenuStrip

#Region "Constructor"

        Private mc_DataGridView As DataGridView
        Private mc_Column As DataGridViewColumn

        Public Sub New(ByVal DataGridView As DataGridView)
            Me.mc_DataGridView = DataGridView
            Me.Items.Add(SortHeader)
            Me.Items.Add(SortAscending)
            Me.Items.Add(SortDescending)
            Me.Items.Add(FilterHeader)
            Me.Items.Add(FilterBasic)
            Me.Items.Add(tsiFilterRemoveSeperator)
            Me.Items.Add(FilteredFields)
            Me.Items.Add(FilterRemove)
        End Sub

#End Region

#Region "Built In Items"

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                tmrRemoveFilterRemove.Dispose()
                SortHeader.Dispose()
                SortAscending.Dispose()
                SortDescending.Dispose()
                FilterHeader.Dispose()
                FilterBasic.Dispose()
                FilterRemove.Dispose()
            End If
        End Sub

        Dim SortHeader As New MenuTextSeperator("Sort")

        Private WithEvents SortAscending As New ToolStripMenuItem("Ascending")
        Private Sub SortAscending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SortAscending.Click
            mc_DataGridView.Sort(mc_Column, System.ComponentModel.ListSortDirection.Ascending)
        End Sub

        Private WithEvents SortDescending As New ToolStripMenuItem("Descending")
        Private Sub SortDescending_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SortDescending.Click
            mc_DataGridView.Sort(mc_Column, System.ComponentModel.ListSortDirection.Descending)
        End Sub

        Dim FilterHeader As New MenuTextSeperator("Filter")

        Private WithEvents FilterBasic As New ToolStripTextBox()
        Dim RaiseFilterBasic_TextChanged As Boolean = True
        Private Sub FilterBasic_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterBasic.TextChanged
            If RaiseFilterBasic_TextChanged Then
                Select Case mc_DataGridView.SupportsFiltering()
                    Case FilteringMethods.Basic
                        'qwertyuiop
                        'NOT IMPLEMENTED
                    Case FilteringMethods.Advanced
                        Dim bs = DirectCast(mc_DataGridView.DataSource, AdvancedBindingSource)

                        'remove existing filtering on the column...
                        Dim ThisFilterItems = (From xItem In bs.BasicFilters Where xItem.Field = mc_Column.DataPropertyName).ToList
                        For Each FilterToRemove In ThisFilterItems
                            bs.BasicFilters.Remove(FilterToRemove)
                        Next

                        'add new filtering...
                        If FilterBasic.Text <> "" Then
                            bs.BasicFilters.Add(New AdvancedBindingSource.BasicFilter(mc_Column.DataPropertyName, FilterBasic.Text))
                            FilterRemove.Visible = True
                        Else
                            FilterRemove.Visible = False
                        End If
                        bs.RunBasicFilter()
                End Select
                For Each Plugin In LoadedPlugins
                    Plugin.LoadFromFilter(Nothing)
                Next
            End If
        End Sub

        Dim tsiFilterRemoveSeperator As New ToolStripSeparator

        Dim FilteredFields As New ToolStripMenuItem("") With {.Enabled = False}

        Private WithEvents FilterRemove As New PersistentToolStripMenuItem With {.Text = "Remove Filter"}
        Private Sub FilterRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterRemove.Click
            If RaiseFilterBasic_TextChanged Then
                Select Case mc_DataGridView.SupportsFiltering()
                    Case FilteringMethods.Basic
                        'qwertyuiop
                        'NOT IMPLEMENTED
                    Case FilteringMethods.Advanced
                        Dim bs = DirectCast(mc_DataGridView.DataSource, AdvancedBindingSource)
                        'remove existing filtering on the column...
                        If mc_Column Is Nothing Then
                            'remove all
                            bs.BasicFilters.Clear()
                        Else
                            'remove existing filtering on the column...
                            Dim ThisFilterItems = (From xItem In bs.BasicFilters Where xItem.Field = mc_Column.DataPropertyName).ToList
                            For Each FilterToRemove In ThisFilterItems
                                bs.BasicFilters.Remove(FilterToRemove)
                            Next
                        End If
                        bs.RunBasicFilter()
                End Select
                For Each Plugin In LoadedPlugins
                    Plugin.LoadFromFilter(Nothing)
                Next
                RaiseFilterBasic_TextChanged = False
                FilterBasic.Text = ""
                RaiseFilterBasic_TextChanged = True
                tmrRemoveFilterRemove.Enabled = True
            End If
        End Sub

        'this timer is used to remove the FilterRemove item
        'this is due to an issue that when clicking on a percistant menu item,
        'if the menu is no longer under the menu after the click, the menu
        'will close
        Private WithEvents tmrRemoveFilterRemove As New Timer With {.Interval = 1}

        Private Sub tmrRemoveFilterRemove_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrRemoveFilterRemove.Tick
            tmrRemoveFilterRemove.Enabled = False
            FilterRemove.Visible = False
        End Sub

#End Region

        Protected Overrides Sub OnClosed(ByVal e As System.Windows.Forms.ToolStripDropDownClosedEventArgs)
            For Each CustomMenuItem In CustomMenuItems.ToArray
                Me.Items.Remove(CustomMenuItem)
            Next
            CustomMenuItems.Clear()
            MyBase.OnClosed(e)
        End Sub

        Public LoadedPlugins As New List(Of Plugins.iFilterPlugin)

        Public Overloads Sub Show(ByVal ColIndex As Integer)
            FilterRemove.Persistent = True
            FilteredFields.Visible = False
            If ColIndex = -1 Then
                For Each MenuItem In Me.Items.OfType(Of ToolStripItem)()
                    MenuItem.Visible = False
                Next
                FilterRemove.Visible = True
                mc_Column = Nothing

                FilterHeader.Visible = True

                'RaiseFilterBasic_TextChanged = False
                'If mc_DataGridView.SupportsFiltering <> FilteringMethods.None Then
                '    FilterBasic.Text = mc_DataGridView.GetFilter
                '    FilterBasic.Visible = True
                'End If
                'RaiseFilterBasic_TextChanged = True
                Select Case mc_DataGridView.SupportsFiltering
                    Case FilteringMethods.Basic
                        'qwertyuiop
                        'NOT IMPLEMENTED
                    Case FilteringMethods.Advanced
                        Dim bs = DirectCast(mc_DataGridView.DataSource, AdvancedBindingSource)

                        Dim Fields As New List(Of String)
                        For Each BasicFilter In bs.BasicFilters
                            Dim Display = BasicFilter.Field
                            Dim Prop = bs.BaseType.GetProperty(BasicFilter.Field)
                            Dim ActivePluginAttrib = Prop.GetCustomAttributes(False).OfType(Of System.ComponentModel.DisplayNameAttribute)().FirstOrDefault
                            If ActivePluginAttrib IsNot Nothing Then
                                Display = ActivePluginAttrib.DisplayName
                            End If
                            Fields.Add(Display)
                        Next
                        If Fields.Count > 0 Then
                            FilteredFields.Text = "Filtered on: " & Join(Fields.ToArray, ", ")
                            FilteredFields.Visible = True
                        End If
                    Case Else

                End Select
                FilterRemove.Persistent = False
                FilterRemove.Text = "Remove All Filters"
            Else
                mc_Column = mc_DataGridView.Columns(ColIndex)
                If mc_Column.SortMode <> DataGridViewColumnSortMode.NotSortable Then
                    SortHeader.Visible = True
                    Select Case mc_Column.HeaderCell.SortGlyphDirection
                        Case Windows.Forms.SortOrder.Ascending
                            SortAscending.Visible = False
                            SortDescending.Visible = True
                        Case Windows.Forms.SortOrder.Descending
                            SortAscending.Visible = True
                            SortDescending.Visible = False
                        Case Windows.Forms.SortOrder.None
                            SortAscending.Visible = True
                            SortDescending.Visible = True
                    End Select
                Else
                    SortHeader.Visible = False
                    SortAscending.Visible = False
                    SortDescending.Visible = False
                End If

                RaiseFilterBasic_TextChanged = False
                FilterRemove.Visible = False
                Select Case mc_DataGridView.SupportsFiltering
                    Case FilteringMethods.Basic
                        'qwertyuiop
                        'NOT IMPLEMENTED
                    Case FilteringMethods.Advanced
                        FilterHeader.Visible = True
                        FilterBasic.Visible = True
                        Dim bs = DirectCast(mc_DataGridView.DataSource, AdvancedBindingSource)
                        Dim ThisFilterItem = (From xItem In bs.BasicFilters Where xItem.Field = mc_Column.DataPropertyName).FirstOrDefault
                        If ThisFilterItem IsNot Nothing Then
                            Dim ThisBasicFilterItem = TryCast(ThisFilterItem, AdvancedBindingSource.BasicFilter)
                            If ThisBasicFilterItem IsNot Nothing Then
                                FilterBasic.Text = ThisBasicFilterItem.Filter
                            Else
                                FilterBasic.Text = ""
                            End If
                            FilterRemove.Text = "Remove Filter"
                            FilterRemove.Visible = True
                            tsiFilterRemoveSeperator.Visible = True 'need to set this as event doesn't get thrown to show it when the menu has yet to be loaded
                        Else
                            FilterBasic.Text = ""
                        End If
                        Dim UnderlyingListObjectType = bs.DataSource.GetType.GetGenericArguments().First()
                        Dim Prop = UnderlyingListObjectType.GetProperty(mc_DataGridView.Columns(ColIndex).DataPropertyName)
                        Dim Datatype = Prop.PropertyType
                        Dim AvaliablePlugins = (From xItem In Plugins.Setup.FilterPlugins Where xItem.DataTypes.Contains(Datatype))


                        Dim ActivePluginAttrib = Prop.GetCustomAttributes(False).OfType(Of i00BindingList.Plugins.ActiveFilterPluginsAttribute)()
                        Dim ActivePlugins = If(ActivePluginAttrib.Count > 0, New List(Of Type), Nothing)
                        For Each ActivePlugin In ActivePluginAttrib
                            ActivePlugins.AddRange(ActivePlugin.ActivePlugins)
                        Next

                        LoadedPlugins.Clear()
                        For Each plugin In (From xItem In AvaliablePlugins Where xItem.Dispaly = Plugins.DisplayMethods.Always OrElse _
                                                                                 (xItem.Dispaly = Plugins.DisplayMethods.DefaultShow AndAlso (ActivePlugins Is Nothing OrElse ActivePlugins.Contains(xItem.GetType))) OrElse _
                                                                                 (xItem.Dispaly = Plugins.DisplayMethods.DefaultHide AndAlso (ActivePlugins IsNot Nothing AndAlso ActivePlugins.Contains(xItem.GetType))))
                            plugin.LoadFromFilter(ThisFilterItem)
                            Dim FilterEventArgs As New FilterEventArgs With {.Cancel = False, .Column = mc_Column, .Plugin = plugin}
                            mc_DataGridView.OnAddingFilterPlugin(FilterEventArgs)
                            If FilterEventArgs.Cancel = False Then
                                RemoveHandler plugin.UpdateFilter, AddressOf Plugin_UpdateFilter
                                AddHandler plugin.UpdateFilter, AddressOf Plugin_UpdateFilter
                                LoadedPlugins.Add(plugin)
                                CustomMenuItems.Add(New ToolStripSeparator)
                                For Each MenuItemToAdd In plugin.MenuItems
                                    CustomMenuItems.Add(MenuItemToAdd)
                                Next
                            End If
                        Next
                        Me.Items.AddRange(CustomMenuItems.ToArray)

                        Me.Items.Remove(tsiFilterRemoveSeperator)
                        Me.Items.Add(tsiFilterRemoveSeperator)
                        Me.Items.Remove(FilterRemove)
                        Me.Items.Add(FilterRemove)

                    Case Else
                        FilterHeader.Visible = False
                        FilterBasic.Visible = False
                End Select
                RaiseFilterBasic_TextChanged = True


            End If
            Me.Show(mc_DataGridView, New Point(mc_DataGridView.GetCellDisplayRectangle(ColIndex, -1, True).Right - 16, mc_DataGridView.ColumnHeadersHeight))
        End Sub

        Private Sub Plugin_UpdateFilter(ByVal sender As Object, ByVal e As Plugins.UpdateFilterPluginEventArgs)
            If RaiseFilterBasic_TextChanged Then
                'clear any other plugins 1st
                For Each plugin In LoadedPlugins
                    If plugin IsNot sender Then
                        plugin.LoadFromFilter(Nothing)
                    End If
                Next
                Select Case mc_DataGridView.SupportsFiltering()
                    Case FilteringMethods.Basic
                        'qwertyuiop
                        'NOT IMPLEMENTED
                    Case FilteringMethods.Advanced
                        Dim bs = DirectCast(mc_DataGridView.DataSource, AdvancedBindingSource)

                        'remove existing filtering on the column...
                        Dim ThisFilterItems = (From xItem In bs.BasicFilters Where xItem.Field = mc_Column.DataPropertyName).ToList
                        For Each FilterToRemove In ThisFilterItems
                            bs.BasicFilters.Remove(FilterToRemove)
                        Next

                        'add new filtering...
                        If e.FilterBase IsNot Nothing Then
                            bs.BasicFilters.Add(e.FilterBase)
                            e.FilterBase.Field = mc_Column.DataPropertyName
                            FilterRemove.Visible = True
                        Else
                            FilterRemove.Visible = False
                        End If
                        bs.RunBasicFilter()
                End Select
                RaiseFilterBasic_TextChanged = False
                FilterBasic.Text = ""
                RaiseFilterBasic_TextChanged = True
            End If

        End Sub

        Dim CustomMenuItems As New List(Of ToolStripItem)

        Private Sub FilterRemove_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FilterRemove.VisibleChanged
            tsiFilterRemoveSeperator.Visible = FilterRemove.Visible
        End Sub
    End Class

#End Region

    Protected Overrides Sub OnColumnHeaderMouseClick(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
        If isOverWidget AndAlso e.ColumnIndex <> -1 Then
            HeaderMenu.Show(e.ColumnIndex)
        Else
            MyBase.OnColumnHeaderMouseClick(e)
        End If
    End Sub


    Protected Overrides Sub OnCellMouseClick(ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs)
        If e.ColumnIndex = -1 AndAlso e.RowIndex = -1 AndAlso e.Button = Windows.Forms.MouseButtons.Right AndAlso Me.GetFilter <> "" Then
            'remove filter
            HeaderMenu.Show(e.ColumnIndex)
        Else
            MyBase.OnCellMouseClick(e)
        End If
    End Sub

#End Region

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            HeaderMenu.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub DataGridView_RowValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Me.RowValidating
        If Me.IsCurrentRowDirty Then
            'e.Cancel = True
        End If
    End Sub

End Class
