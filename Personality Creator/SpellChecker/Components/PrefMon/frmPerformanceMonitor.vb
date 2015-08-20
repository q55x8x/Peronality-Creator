'i00 .Net Performance Monitor
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

Public Class frmPerformanceMonitor

#Region "Line Style tsi"

    Public Class ctlTsiLineStyle
        Inherits ToolStripDropDownItem

        Public AllowLineWidth0 As Boolean = False
        Public LineWidth As Single
        Public Type As clsGrid.GridSetProperty.DataStyles = clsGrid.GridSetProperty.DataStyles.None
        Public DashStyle As Drawing2D.DashStyle = Drawing.Drawing2D.DashStyle.Solid
        Public Color As Color

        Public Sub New()
            MyBase.AutoToolTip = False
            MyBase.AutoSize = False
            MyBase.Width = 100
        End Sub

        Public Checked As Boolean

        Private Sub ctlTsiLineStyle_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

            If Me.Selected OrElse Checked Then
                Using tsi As New ToolStripButton
                    tsi.AutoSize = False
                    tsi.Width = Me.ContentRectangle.Width
                    tsi.Height = Me.ContentRectangle.Height
                    tsi.Select()
                    Using b As New Bitmap(tsi.Width, tsi.Height)
                        Using g = Graphics.FromImage(b)
                            Me.Parent.Renderer.DrawButtonBackground(New ToolStripItemRenderEventArgs(g, tsi))
                            If Checked = False Then
                                'alpha image
                                b.Filters.Alpha()
                            Else
                                'just draw
                            End If
                            e.Graphics.DrawImageUnscaled(b, Me.ContentRectangle.Location)
                        End Using
                    End Using

                End Using
            End If

            'e.Graphics.SetClip(New Rectangle(Me.ContentRectangle.X + 1))
            Dim xOffset = 3
            If Text <> "" Then
                Dim TextSize = TextRenderer.MeasureText(Text, MyBase.Font)
                TextRenderer.DrawText(e.Graphics, Text, MyBase.Font, New Point(xOffset, 1 + CInt((Me.ContentRectangle.Height - TextSize.Height) / 2)), Me.ForeColor)
                xOffset += TextSize.Width + 3
            End If
            If Type = clsGrid.GridSetProperty.DataStyles.None Then
                If AllowLineWidth0 = True AndAlso LineWidth = 0 Then

                Else
                    Using p As New Pen(Color, LineWidth)
                        p.DashStyle = DashStyle
                        Dim y As Single = CSng(Me.ContentRectangle.X + ((Me.ContentRectangle.Height - 1) / 2))
                        'e.Graphics.DrawLine(p, Me.ContentRectangle.X + xOffset, y, Me.ContentRectangle.Right - 3, y)
                        e.Graphics.DrawLine(p, (Me.ContentRectangle.Right - 100) + 3, y, Me.ContentRectangle.Right - 3, y)
                    End Using
                End If
            Else
                Dim TestData() As Single = {0, 22, 80, 70, 20, 60, 0}
                Dim GridSetProperty As New clsGrid.GridSetProperty("Style", Color)
                GridSetProperty.DashStyle = DashStyle
                GridSetProperty.PenWidth = LineWidth
                If AllowLineWidth0 = False AndAlso GridSetProperty.PenWidth = 0 Then
                    GridSetProperty.PenWidth = 1
                End If
                GridSetProperty.DataStyle = Type
                GridSetProperty.TimeSpanMS = TestData.Count - 1
                Dim TheTime = Now
                Dim i As Integer = TestData.Count - 1
                For Each item In TestData
                    GridSetProperty.AddGridValue(New clsGrid.GridSetProperty.GridValueItem(item, TheTime.Subtract(New TimeSpan(i * TimeSpan.TicksPerMillisecond))))
                    i -= 1
                Next
                'Dim ContRect = New Rectangle(Me.ContentRectangle.X + xOffset, Me.ContentRectangle.Y, 1, Me.ContentRectangle.Height)
                'ContRect.Width = (Me.ContentRectangle.Right - 3) - ContRect.Left
                Dim ContRect = New Rectangle((Me.ContentRectangle.Right - 100) + 3, Me.ContentRectangle.Y, 100 - 6, Me.ContentRectangle.Height)
                GridSetProperty.DrawPath(ContRect, e.Graphics, TheTime)
            End If
        End Sub

        Public Function GetTextWidth() As Integer
            Return TextRenderer.MeasureText(Text, MyBase.Font).Width + 6
        End Function

        Private Sub ctlTsiLineStyle_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
            If Text = "" Then
                MyBase.Width = 100
            Else
                MyBase.Width = 100 + GetTextWidth()
            End If
        End Sub

    End Class

#End Region

#Region "Grid Setup"

    Private Sub frmPerformanceMonitor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim PrefMonFile = IO.Path.Combine(Environment.SystemDirectory, "perfmon.exe")
        If FileIO.FileSystem.FileExists(PrefMonFile) Then
            tsiOpen.Image = IconExtraction.GetDefaultIcon(PrefMonFile, IconExtraction.IconSize.SmallIcon).ToBitmap
            tsiOpen.Visible = True
        End If

        tsiGridBGColor.SelectedColor = Color.Black
        ClsGrid1.BackColor = tsiGridBGColor.SelectedColor

        tsiGridColor.SelectedColor = Color.Green
        ClsGrid1.ForeColor = tsiGridColor.SelectedColor

        If Me.Owner IsNot Nothing Then
            Me.StartPosition = FormStartPosition.Manual
            Me.Location = New Point(Me.Owner.Right - Me.Width, Me.Owner.Bottom - Me.Height)
        End If

        Dim UpdateSpeeds() As Integer = {250, 500, 1000, 2000, 3000, 4000}
        For Each itemMS In UpdateSpeeds
            Dim item = itemMS / 1000
            Dim tsi As New ToolStripMenuItem
            tsi.Tag = itemMS
            If item < 1 Then
                Dim ToBeOverVal = 1 / item
                If ToBeOverVal = CInt(ToBeOverVal) Then
                    'we can use this
                    tsi.Text = "1/" & ToBeOverVal & " second"
                Else
                    tsi.Text = item & " second"
                End If
            Else
                tsi.Text = item & " second" & If(item = 1, "", "s")
            End If
            AddHandler tsi.Click, AddressOf tsiUpdateSpeed_Click
            tsiUpdateSpeed.DropDownItems.Add(tsi)
        Next

        Dim RedrawSpeeds() As Integer = {1, 33, 100, 1000}
        For Each item In RedrawSpeeds
            Dim tsi As New ToolStripMenuItem
            tsi.Tag = item
            If item = 1 Then
                tsi.Text = "As quickly as possible"
            Else
                If item >= 1000 Then
                    Dim ToBeVal = item / 1000
                    tsi.Text = "Every " & ToBeVal & " second" & If(ToBeVal = 1, "", "s")
                Else
                    tsi.Text = CInt(1000 / item) & " per second"
                End If
            End If
            AddHandler tsi.Click, AddressOf tsiRedrawSpeed_Click
            tsiRedrawSpeed.DropDownItems.Add(tsi)
        Next

        If ClsGrid1.GridSets.Count = 0 Then
            'add cpu
            Dim PrefCounter = AddPerformanceCounter("Processor Information", "% Processor Time", "_Total", Color.Lime, 60000)
            PrefCounter.GridData.DataStyle = PrefMon.clsGrid.GridSetProperty.DataStyles.Filled_Smoothed_Line
            PrefCounter.GridData.PenWidth = 0
        End If

        If Me.Owner Is Nothing Then
            ShowInTaskbar = True
        End If

        'Dim b As New Bitmap(Me.Width, Me.Height)
        'Me.DrawToBitmap(b, New Rectangle(0, 0, Me.Width, Me.Height))
        ''b.Save("c:\asd.bmp")
        'WindowAnimation.ShowPopUp(Me.Width, Me.Height, b)

        'Dim DWMthumb As IntPtr

        'Dim frm As New Form
        'frm.Visible = True
        'Dim thumb As New IntPtr
        'Dim i As Integer = DWM.DwmRegisterThumbnail(frm.Handle, Me.Handle, DWMthumb)

        'Dim size As DWM.PSIZE
        'DWM.DwmQueryThumbnailSourceSize(DWMthumb, size)

        'Dim props As New DWM.DWM_THUMBNAIL_PROPERTIES

        'props.fVisible = True
        'props.dwFlags = DWM.DWM_TNP_VISIBLE Or DWM.DWM_TNP_RECTDESTINATION Or DWM.DWM_TNP_OPACITY 'Or DWM.DWM_TNP_CLIENT_AREA_ONLY
        'props.fSourceClientAreaOnly = True
        'props.opacity = CByte(255)
        'Dim rect As New Rectangle(0, 0, frm.ClientRectangle.Width, frm.ClientRectangle.Height)
        'props.rcDestination = New DWM.Rect(rect.X, rect.Y, rect.Right, rect.Bottom)
        'DWM.DwmUpdateThumbnailProperties(DWMthumb, props)

    End Sub

    Private Sub frmPerformanceMonitor_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If tt IsNot Nothing Then
            tt.Dispose()
        End If
    End Sub

    Dim ttInterval As Integer = 1000
    Private WithEvents tt As New System.Threading.Timer(AddressOf UpdateStatus, Nothing, 1000, 1000)
    Private Property UpdateSpeed() As Integer
        Get
            Return ttInterval
        End Get
        Set(ByVal value As Integer)
            Dim tt = Me.tt
            Me.tt = Nothing
            tt.Dispose()
            ttInterval = value
            Me.tt = New System.Threading.Timer(AddressOf UpdateStatus, Nothing, value, value)
        End Set
    End Property

    Private Sub tsiUpdateSpeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim oSender = TryCast(sender, ToolStripItem)
        If oSender IsNot Nothing Then
            UpdateSpeed = CInt(Val(oSender.Tag))
        End If
    End Sub

    Private Sub tsiUpdateSpeed_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiUpdateSpeed.DropDownOpening
        For Each item In tsiUpdateSpeed.DropDownItems.OfType(Of ToolStripMenuItem)()
            item.Checked = CInt(Val(item.Tag)) = ttInterval
        Next
    End Sub

    Private Sub tsiRedrawSpeed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim oSender = TryCast(sender, ToolStripItem)
        If oSender IsNot Nothing Then
            tmrRedraw.Interval = CInt(Val(oSender.Tag))
        End If
    End Sub

    Private Sub tsiRedrawSpeed_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiRedrawSpeed.DropDownOpening
        For Each item In tsiRedrawSpeed.DropDownItems.OfType(Of ToolStripMenuItem)()
            item.Checked = CInt(Val(item.Tag)) = tmrRedraw.Interval
        Next
    End Sub

    Private Sub tsiGridBGColor_ColorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiGridBGColor.ColorChanged
        ClsGrid1.BackColor = tsiGridBGColor.SelectedColor
    End Sub

    Private Sub tsiGridColor_ColorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiGridColor.ColorChanged
        ClsGrid1.ForeColor = tsiGridColor.SelectedColor
    End Sub

#End Region

#Region "Open Perfmon"

    <System.Runtime.InteropServices.DllImport("User32.dll")> _
    Private Shared Function SetForegroundWindow(ByVal handle As IntPtr) As Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("User32.dll")> _
    Private Shared Function ShowWindow(ByVal handle As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function
    Private Const SW_RESTORE As Integer = 9
    <System.Runtime.InteropServices.DllImport("User32.dll")> _
    Private Shared Function IsIconic(ByVal handle As IntPtr) As Boolean
    End Function

    Dim PerfmonProcess As Process
    Private Sub tsiOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsiOpen.Click
        Dim ShellFile = IO.Path.Combine(Environment.SystemDirectory, "perfmon.exe")
        'qwertyuiop - wanted to bring the existing process to the foreground ... but it just opens mmc so it does not stay open :(
        If PerfmonProcess Is Nothing OrElse PerfmonProcess.HasExited Then
            If FileIO.FileSystem.FileExists(ShellFile) Then
                PerfmonProcess = Process.Start(ShellFile)
            End If
        Else
            Dim hdl = PerfmonProcess.MainWindowHandle
            If IsIconic(hdl) Then
                ShowWindow(hdl, SW_RESTORE)
            End If
            SetForegroundWindow(hdl)
        End If
    End Sub

#End Region

#Region "Performance counters"

    Dim tListCounters As System.Threading.Thread

    Private Sub tsiAdd_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiAdd.DropDownOpening
        If tsiAdd.DropDownItems.Count = 0 Then
            ListCounters()
        End If
    End Sub

    Dim tsiLoading As New ToolStripMenuItem("Loading...") With {.Enabled = False}
    Private Sub ListCounters()
        tsiAdd.DropDownItems.Clear()
        tsiAdd.DropDownItems.Add(tsiLoading)

        If tListCounters IsNot Nothing AndAlso tListCounters.IsAlive Then
            tListCounters.Abort()
        End If
        tListCounters = New System.Threading.Thread(AddressOf LoadPerformanceCounters)
        tListCounters.IsBackground = True
        tListCounters.Start()
    End Sub

    Private Class PerformanceListItem
        Public Name As String
        Public Counters As New List(Of CounterListItem)
        Public Class CounterListItem
            Public Name As String
            Public Counters As New List(Of String)
        End Class
    End Class

    Private Sub LoadPerformanceCounters()
        Dim PerformanceListItems As New List(Of PerformanceListItem)
        For Each cat In (From xItem In PerformanceCounterCategory.GetCategories() Order By xItem.CategoryName).ToArray
            Try
                Dim PerformanceListItem As New PerformanceListItem With {.Name = cat.CategoryName}
                If cat.CategoryType = PerformanceCounterCategoryType.SingleInstance Then
                    For Each counter In (From xItem In cat.GetCounters Order By xItem.CounterName).ToArray
                        Try
                            PerformanceListItem.Counters.Add(New PerformanceListItem.CounterListItem() With {.Name = counter.CounterName})
                        Catch ex As Exception

                        End Try
                    Next
                ElseIf cat.CategoryType = PerformanceCounterCategoryType.MultiInstance Then
                    For Each instance In (From xItem In cat.GetInstanceNames Order By xItem).ToArray
                        Try
                            Dim cli = New PerformanceListItem.CounterListItem() With {.Name = instance}
                            Try
                                cli.Counters.AddRange((From xItem In cat.GetCounters(instance) Order By xItem.CounterName Select xItem.CounterName).ToArray)
                            Catch ex As Exception

                            End Try
                            PerformanceListItem.Counters.Add(cli)
                        Catch ex As Exception

                        End Try
                    Next
                End If

                PerformanceListItems.Add(PerformanceListItem)
            Catch ex As Exception

            End Try
        Next
        PerformanceCountersLoaded(PerformanceListItems)
    End Sub

    Private Class tsiCounterAdd
        Inherits ToolStripMenuItem
        Public Category As String
        Public Counter As String
        Public Instance As String
        Public Sub New(ByVal Category As String, ByVal Counter As String, Optional ByVal Instance As String = "")
            Me.Category = Category
            Me.Counter = Counter
            Me.Text = Counter
            Me.Instance = Instance
        End Sub
    End Class

    Private Delegate Sub PerformanceCountersLoaded_cb(ByVal PerformanceListItems As List(Of PerformanceListItem))
    Private Sub PerformanceCountersLoaded(ByVal PerformanceListItems As List(Of PerformanceListItem))
        If Me.InvokeRequired Then
            Dim PerformanceCountersLoaded_cb As New PerformanceCountersLoaded_cb(AddressOf PerformanceCountersLoaded)
            Me.Invoke(PerformanceCountersLoaded_cb, PerformanceListItems)
        Else
            For Each cat In PerformanceListItems
                Dim tsi As New ToolStripMenuItem(cat.Name)
                For Each counter In cat.Counters
                    If counter.Counters.Count = 0 Then
                        Dim tsiSub As New tsiCounterAdd(cat.Name, counter.Name)
                        AddHandler tsiSub.Click, AddressOf AddToolStripMenuItem_Click
                        tsi.DropDownItems.Add(tsiSub)
                    Else
                        Dim tsiSub As New ToolStripMenuItem(counter.Name)
                        For Each item In counter.Counters
                            Dim tsiSub2 As New tsiCounterAdd(cat.Name, item, counter.Name)
                            AddHandler tsiSub2.Click, AddressOf AddToolStripMenuItem_Click
                            tsiSub.DropDownItems.Add(tsiSub2)
                        Next
                        tsi.DropDownItems.Add(tsiSub)

                    End If
                Next
                tsiAdd.DropDownItems.Add(tsi)
            Next
            tsiAdd.DropDownItems.Remove(tsiLoading)
        End If
    End Sub

    Private Sub AddToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim tsiCounterAdd = TryCast(sender, tsiCounterAdd)
        If tsiCounterAdd IsNot Nothing Then
            AddPerformanceCounter(tsiCounterAdd.Category, tsiCounterAdd.Counter, tsiCounterAdd.Instance, 60000)
        End If
    End Sub

    Public Class tsiCounter
        Inherits ToolStripDropDownButton

#Region "Shared controls"

#Region "Common"

#Region "Setup"

        Public Shared tsiCounterOwner As tsiCounter

        Private Sub tsiCounter_DropDownOpening(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDownOpening
            tsiCounterOwner = Me

            Me.DropDownItems.Add(tsiHeader)
            tsiHeader.Text = Replace(Me.Text, vbCrLf, " - ")

            Me.DropDownItems.Add(tsiStatus)
            UpdateTsiStatus(True)

            Me.DropDownItems.Add(tsiSeparator)

            Me.DropDownItems.Add(tsiColor)
            tsiColor.SelectedColor = Me.ItemColor

            Me.DropDownItems.Add(tsiGraphType)
            For Each item In tsiGraphType.DropDownItems.OfType(Of ctlTsiLineStyle)()
                item.Checked = item.Type = Me.GridData.DataStyle
                item.LineWidth = tsiCounterOwner.GridData.PenWidth
                item.Color = tsiCounterOwner.GridData.Color
                item.DashStyle = tsiCounterOwner.GridData.DashStyle
            Next

            Me.DropDownItems.Add(tsiLineWidth)
            For Each item In tsiLineWidth.DropDownItems.OfType(Of ctlTsiLineStyle)()
                item.Checked = item.LineWidth = Me.GridData.PenWidth
                item.Color = tsiCounterOwner.GridData.Color
                item.DashStyle = tsiCounterOwner.GridData.DashStyle
            Next

            Me.DropDownItems.Add(tsiLineStyle)
            For Each item In tsiLineStyle.DropDownItems.OfType(Of ctlTsiLineStyle)()
                item.Checked = item.DashStyle = Me.GridData.DashStyle
                item.Color = tsiCounterOwner.GridData.Color
                item.LineWidth = tsiCounterOwner.GridData.PenWidth
            Next

            Me.DropDownItems.Add(tsiUpdateInterval)
            For Each item In tsiUpdateInterval.DropDownItems.OfType(Of ToolStripMenuItem)()
                item.Checked = Val(item.Tag) = Me.GridData.TimeSpanMS
            Next

            Me.DropDownItems.Add(tsiRemove)

        End Sub

#End Region

#End Region

#Region "Header"

        Private Shared ReadOnly Property tsiHeader() As i00SpellCheck.MenuTextSeperator
            Get
                Static mc_tsiHeader As i00SpellCheck.MenuTextSeperator
                If mc_tsiHeader Is Nothing OrElse mc_tsiHeader.IsDisposed Then
                    mc_tsiHeader = New i00SpellCheck.MenuTextSeperator
                End If

                Return mc_tsiHeader
            End Get
        End Property

#End Region

#Region "Status"

        Public Shared ReadOnly Property tsiStatus() As i00SpellCheck.HTMLMenuItem
            Get
                Static mc_tsiStatus As i00SpellCheck.HTMLMenuItem
                If mc_tsiStatus Is Nothing OrElse mc_tsiStatus.IsDisposed Then
                    mc_tsiStatus = New i00SpellCheck.HTMLMenuItem(" ")
                    mc_tsiStatus.SmallHeight = 1000
                End If

                Return mc_tsiStatus
            End Get
        End Property


        Public Shared Sub UpdateTsiStatus(Optional ByVal ForceUpdate As Boolean = False)
            If tsiCounterOwner IsNot Nothing AndAlso (tsiStatus.Visible OrElse ForceUpdate) Then
                Dim HTML As String = ""
                If tsiCounterOwner.GridData.GridValues.Count >= 1 Then
                    Dim Average As String = tsiCounterOwner.GridData.GetAverage.ToString
                    Dim AverageExcluding0 As String
                    Dim ItemsNot0 = (From xItem In tsiCounterOwner.GridData.GridValues Where xItem.Value <> 0 Select xItem.Value).ToArray
                    If ItemsNot0.Count > 0 Then
                        AverageExcluding0 = ItemsNot0.Average.ToString
                    Else
                        Dim ShortcutKeyColor = System.Drawing.ColorTranslator.ToHtml(i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.ControlText), Color.FromKnownColor(KnownColor.Control)))
                        AverageExcluding0 = "<i><font color=" & ShortcutKeyColor & ">All recorded data is 0</font></i>"
                    End If
                    Dim WatchingSince As String = (From xItem In tsiCounterOwner.GridData.GridValues Order By xItem.Time Select (xItem.Time.ToString)).FirstOrDefault
                    Dim RecordValue As String = tsiCounterOwner.GridData.GridValues.Max(Function(x As clsGrid.GridSetProperty.GridValueItem) x.Value).ToString
                    HTML = "<b>Current Value:</b> " & tsiCounterOwner.GridData.GridValues.Last.Value & vbCrLf & _
                                         "<b>Highest Recorded Value:</b> " & RecordValue & vbCrLf & _
                                         "<b>Graph Max:</b> " & tsiCounterOwner.GridData.MaxValue & vbCrLf & _
                                         "<b>Average:</b> " & Average & vbCrLf & _
                                         "<b>Average Excluding 0 Values:</b> " & AverageExcluding0 & vbCrLf & _
                                         "<b>Since:</b> " & WatchingSince & vbCrLf & _
                                         "<b>Total Data Recorded:</b> " & tsiCounterOwner.GridData.GridValues.Count
                    tsiStatus.Invalidate()
                Else
                    Dim ShortcutKeyColor = System.Drawing.ColorTranslator.ToHtml(i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.ControlText), Color.FromKnownColor(KnownColor.Control)))
                    HTML = "<i><font color=" & ShortcutKeyColor & ">No Data</font></i>"
                End If
                If tsiCounterOwner.inError <> "" Then
                    Dim ShortcutKeyColor = System.Drawing.ColorTranslator.ToHtml(i00SpellCheck.DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.ControlText), Color.FromKnownColor(KnownColor.Red)))
                    HTML &= "<BR><B><font color=" & ShortcutKeyColor & ">Error:</font></B> " & tsiCounterOwner.inError
                End If
                tsiStatus.HTMLText = HTML
            End If
        End Sub

#End Region

#Region "Separator"

        Private Shared ReadOnly Property tsiSeparator() As ToolStripSeparator
            Get
                Static mc_tsiSeparator As ToolStripSeparator
                If mc_tsiSeparator Is Nothing OrElse mc_tsiSeparator.IsDisposed Then
                    mc_tsiSeparator = New ToolStripSeparator
                End If

                Return mc_tsiSeparator
            End Get
        End Property

#End Region

#Region "Color"

        Private Shared WithEvents mc_tsiColor As i00SpellCheck.tsiColorPicker
        Private Shared ReadOnly Property tsiColor() As i00SpellCheck.tsiColorPicker
            Get
                If mc_tsiColor Is Nothing OrElse mc_tsiColor.IsDisposed Then
                    mc_tsiColor = New i00SpellCheck.tsiColorPicker With {.Persistent = True}
                End If

                Return mc_tsiColor
            End Get
        End Property

        Private Shared Sub tsiColor_ColorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mc_tsiColor.ColorChanged
            tsiCounterOwner.ItemColor = tsiColor.SelectedColor
        End Sub

#End Region

#Region "Line Width"

        Private Shared Sub tsiLineWidth_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsi = TryCast(sender, ctlTsiLineStyle)
            If tsi IsNot Nothing Then
                tsiCounterOwner.GridData.PenWidth = tsi.LineWidth
            End If
        End Sub

        Private Shared ReadOnly Property tsiLineWidth() As ToolStripMenuItem
            Get
                Static mc_tsiLineWidth As ToolStripMenuItem
                If mc_tsiLineWidth Is Nothing OrElse mc_tsiLineWidth.IsDisposed Then
                    mc_tsiLineWidth = New ToolStripMenuItem("Line Width")
                End If

                If mc_tsiLineWidth.DropDownItems.Count = 0 Then
                    For i = 0 To 4
                        Dim tsi As New ctlTsiLineStyle() With {.LineWidth = i, .AllowLineWidth0 = True}
                        If i = 0 Then
                            tsi.Text = "None"
                            tsi.Width = 100
                        End If
                        AddHandler tsi.Click, AddressOf tsiLineWidth_Click
                        mc_tsiLineWidth.DropDownItems.Add(tsi)
                    Next
                End If
                Return mc_tsiLineWidth
            End Get
        End Property

#End Region

#Region "Line Draw Style"

        Private Shared Sub tsiGraphType_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsi = TryCast(sender, ctlTsiLineStyle)
            If tsi IsNot Nothing Then
                tsiCounterOwner.GridData.DataStyle = tsi.Type
            End If
        End Sub

        Private Shared ReadOnly Property tsiGraphType() As ToolStripMenuItem
            Get
                Static mc_GraphType As ToolStripMenuItem
                If mc_GraphType Is Nothing OrElse mc_GraphType.IsDisposed Then
                    mc_GraphType = New ToolStripMenuItem("Type")
                End If

                If mc_GraphType.DropDownItems.Count = 0 Then
                    Dim AddedTSIs As New List(Of ctlTsiLineStyle)
                    For Each item In [Enum].GetValues(GetType(clsGrid.GridSetProperty.DataStyles))
                        If CInt(item) <> ClsGrid.GridSetProperty.DataStyles.None Then
                            Dim tsi As New ctlTsiLineStyle()
                            Select Case DirectCast(item, clsGrid.GridSetProperty.DataStyles)
                                Case ClsGrid.GridSetProperty.DataStyles.Smoothed_Line, ClsGrid.GridSetProperty.DataStyles.Line
                                Case Else
                                    tsi.AllowLineWidth0 = True
                            End Select

                            tsi.Text = Replace(item.ToString, "_", " ")
                            tsi.Type = DirectCast(item, clsGrid.GridSetProperty.DataStyles)
                            AddHandler tsi.Click, AddressOf tsiGraphType_Click
                            mc_GraphType.DropDownItems.Add(tsi)
                            AddedTSIs.Add(tsi)
                        End If
                    Next
                    Dim MaxWidth = AddedTSIs.Max(Function(x As ctlTsiLineStyle) x.Width)
                    For Each item In AddedTSIs
                        item.Width = MaxWidth
                    Next
                End If
                Return mc_GraphType
            End Get
        End Property

#End Region

#Region "Line Draw Style"

        Private Shared Sub tsiLineStyle_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsi = TryCast(sender, ctlTsiLineStyle)
            If tsi IsNot Nothing Then
                tsiCounterOwner.GridData.DashStyle = tsi.DashStyle
            End If
        End Sub

        Private Shared ReadOnly Property tsiLineStyle() As ToolStripMenuItem
            Get
                Static mc_LineStyle As ToolStripMenuItem
                If mc_LineStyle Is Nothing OrElse mc_LineStyle.IsDisposed Then
                    mc_LineStyle = New ToolStripMenuItem("Line Style")
                End If

                If mc_LineStyle.DropDownItems.Count = 0 Then
                    For Each item In [Enum].GetValues(GetType(Drawing2D.DashStyle))
                        If CInt(item) <> Drawing2D.DashStyle.Custom Then
                            Dim tsi As New ctlTsiLineStyle()
                            tsi.DashStyle = DirectCast(item, Drawing2D.DashStyle)
                            AddHandler tsi.Click, AddressOf tsiLineStyle_Click
                            mc_LineStyle.DropDownItems.Add(tsi)
                        End If
                    Next
                End If
                Return mc_LineStyle
            End Get
        End Property

#End Region

#Region "Update Interval"

        Private Shared Sub tsiUpdateInterval_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim tsi = TryCast(sender, ToolStripItem)
            If tsi IsNot Nothing Then
                tsiCounterOwner.GridData.TimeSpanMS = CLng(tsi.Tag)
            End If
        End Sub

        Private Shared ReadOnly Property tsiUpdateInterval() As ToolStripMenuItem
            Get
                Static mc_tsiUpdateInterval As ToolStripMenuItem
                If mc_tsiUpdateInterval Is Nothing OrElse mc_tsiUpdateInterval.IsDisposed Then
                    mc_tsiUpdateInterval = New ToolStripMenuItem("Time Range")
                End If

                If mc_tsiUpdateInterval.DropDownItems.Count = 0 Then
                    mc_tsiUpdateInterval.DropDownItems.Add("1 minute", Nothing, AddressOf tsiUpdateInterval_Click).Tag = 60000
                    mc_tsiUpdateInterval.DropDownItems.Add("2 minutes", Nothing, AddressOf tsiUpdateInterval_Click).Tag = 120000
                    mc_tsiUpdateInterval.DropDownItems.Add("5 minutes", Nothing, AddressOf tsiUpdateInterval_Click).Tag = 300000
                    mc_tsiUpdateInterval.DropDownItems.Add("10 minutes", Nothing, AddressOf tsiUpdateInterval_Click).Tag = 600000
                End If
                Return mc_tsiUpdateInterval
            End Get
        End Property

#End Region

#Region "Remove"

        Private Shared WithEvents mc_tsiRemove As ToolStripMenuItem
        Private Shared ReadOnly Property tsiRemove() As ToolStripMenuItem
            Get
                If mc_tsiRemove Is Nothing OrElse mc_tsiRemove.IsDisposed Then
                    mc_tsiRemove = New ToolStripMenuItem("Remove counter", My.Resources.Delete)
                End If

                Return mc_tsiRemove
            End Get
        End Property

        Private Shared Sub tsiRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mc_tsiRemove.Click
            'qwertyuiop - had to show before remove otherwise would get an error about the toolstrip items being read only!
            tsiCounterOwner.Overflow = ToolStripItemOverflow.Never
            tsiCounterOwner.Parent.Items.Remove(tsiCounterOwner)
        End Sub

#End Region

#End Region

        Dim mc_ItemColor As Color
        Public Property ItemColor() As Color
            Get
                Return mc_ItemColor
            End Get
            Set(ByVal value As Color)
                mc_ItemColor = value
                GridData.Color = value
                RepaintItem()
            End Set
        End Property

        Public Performance As String
        Public Counter As String
        Public Instance As String

        Dim mc_inError As String
        Private Property inError() As String
            Get
                Return mc_inError
            End Get
            Set(ByVal value As String)
                If mc_inError <> value Then
                    mc_inError = value
                    RepaintItem()
                End If
            End Set
        End Property


        Delegate Sub RepaintItem_cb()
        Public Sub RepaintItem()
            If Me.Parent IsNot Nothing Then
                If Me.Parent.InvokeRequired Then
                    Dim RepaintItem_cb As New RepaintItem_cb(AddressOf RepaintItem)
                    Me.Parent.Invoke(RepaintItem_cb)
                Else
                    Me.Invalidate()
                End If
            End If
        End Sub

        Public Sub CommitValue()
            Try
                Dim ThisValue As Single = PerformanceCounter.NextValue
                GridData.GridValues.Add(New clsGrid.GridSetProperty.GridValueItem(ThisValue))
                inError = ""
            Catch ex As Exception
                'performance counter error...
                inError = ex.Message
                GridData.GridValues.Add(New clsGrid.GridSetProperty.GridValueItem(0))
            End Try
        End Sub

        Public ReadOnly Property PerformanceCounter() As PerformanceCounter
            Get
                Static mc_PerformanceCounter As PerformanceCounter
                If mc_PerformanceCounter Is Nothing Then
                    mc_PerformanceCounter = New PerformanceCounter(Performance, Counter, Instance, True)
                End If
                Return mc_PerformanceCounter
            End Get
        End Property

        Public Sub New(ByVal Performance As String, ByVal Counter As String, ByVal Instance As String, ByVal Color As Color, ByVal Interval As Long, ByVal ClsGrid As clsGrid)
            Me.ItemColor = Color
            NewInternal(Performance, Counter, Instance, Interval, ClsGrid)
        End Sub

        Public Sub New(ByVal Performance As String, ByVal Counter As String, ByVal Instance As String, ByVal Interval As Long, ByVal ClsGrid As clsGrid)
            NewInternal(Performance, Counter, Instance, Interval, ClsGrid)
        End Sub

        Private Shared ReadOnly Property BlankImage() As Bitmap
            Get
                Static mc_BlankImage As New Bitmap(16, 16)
                Return mc_BlankImage
            End Get
        End Property

        Private Sub NewInternal(ByVal Performance As String, ByVal Counter As String, ByVal Instance As String, ByVal Interval As Long, ByVal ClsGrid As clsGrid)
            Me.Performance = Performance
            Me.Counter = Counter
            Me.Instance = Instance

            Me.Image = BlankImage
            Me.DisplayStyle = ToolStripItemDisplayStyle.Image
            Me.Text = Performance & vbCrLf & If(Instance <> "", Instance & vbCrLf, "") & Counter

            If Me.ItemColor.IsEmpty Then
                Randomize()
                Me.ItemColor = (From xItem In tsiColor.Colors Order By Rnd()).First
            End If

            Me.ClsGrid = ClsGrid
            GridData.Color = Me.ItemColor
            GridData.TimeSpanMS = Interval
        End Sub

        Public GridData As New clsGrid.GridSetProperty

        Private ClsGrid As clsGrid

        Private Sub tsiCounter_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
            Me.GridData.ShowGridOverlay(ClsGrid)
        End Sub

        Private Sub tsiCounter_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseLeave
            ClsGrid.GridSetProperty.HideOverlay()
        End Sub

        Private Sub tsiCounter_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
            Dim rect = Me.ContentRectangle

            'we only want the rect 13 x 13
            Dim RectSizeDiff = New Size(rect.Width - 13, rect.Height - 13)
            rect.X += CInt(RectSizeDiff.Width / 2)
            rect.Width = 13
            rect.Y += CInt(RectSizeDiff.Height / 2)
            rect.Height = 13

            Using sb As New SolidBrush(mc_ItemColor)
                e.Graphics.FillRectangle(sb, rect)
            End Using
            'draw border
            Using p As New Pen(i00SpellCheck.DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.MenuText), 63))
                e.Graphics.DrawRectangle(p, rect.X, rect.Y, rect.Width, rect.Height)
            End Using

            If mc_inError <> "" Then
                rect = Me.ContentRectangle
                e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.High
                rect.Width = CInt(rect.Width / 2)
                rect.Height = CInt(rect.Height / 2)
                rect.X += rect.Width
                rect.Y += rect.Height
                e.Graphics.DrawImage(My.Resources.Delete, rect)
            End If
        End Sub
    End Class

    Public Function AddPerformanceCounter(ByVal Performance As String, ByVal Counter As String, ByVal Instance As String, ByVal interval As Long) As tsiCounter
        AddPerformanceCounter = New tsiCounter(Performance, Counter, Instance, interval, ClsGrid1)
        ToolStrip1.Items.Add(AddPerformanceCounter)
        ClsGrid1.GridSets.Add(AddPerformanceCounter.GridData)
    End Function

    Public Function AddPerformanceCounter(ByVal Performance As String, ByVal Counter As String, ByVal Instance As String, ByVal Color As Color, ByVal interval As Long) As tsiCounter
        AddPerformanceCounter = New tsiCounter(Performance, Counter, Instance, Color, interval, ClsGrid1)
        ToolStrip1.Items.Add(AddPerformanceCounter)
        ClsGrid1.GridSets.Add(AddPerformanceCounter.GridData)
    End Function

    Public Sub UpdateStatus(ByVal state As Object)
        If ToolStrip1 IsNot Nothing Then
            SyncLock ToolStrip1.Items
                Dim items = ToolStrip1.Items.OfType(Of tsiCounter)().ToArray
                For Each item In items
                    item.CommitValue()
                Next
            End SyncLock
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrRedraw.Tick
        Static lastTime As Integer
        Dim ThisTime = CInt(Now.TimeOfDay.TotalSeconds)
        If lastTime <> ThisTime Then
            'For Each item In ToolStrip1.Items.OfType(Of tsiCounter)().ToArray
            '    item.CommitValue()
            'Next
            'lastTime = ThisTime

            'update the status menu item
            tsiCounter.UpdateTsiStatus()
        End If

        ClsGrid1.Invalidate()
    End Sub

#End Region

    'Private WithEvents WindowAnimation As New WindowAnimation

    'Private Sub frmPerformanceMonitor_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

    'End Sub

    'Private Sub WindowAnimation_Completed(ByVal sender As Object, ByVal e As System.EventArgs) Handles WindowAnimation.Completed
    '    Me.Visible = True
    'End Sub

End Class