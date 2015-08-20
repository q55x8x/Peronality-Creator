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


Public Class clsGrid
    Inherits i00SpellCheck.BufferedPanel

    Public Class GridSetProperty
        Public Color As Color
        Public SetName As String
        Public TimeSpanMS As Long = 300000 '5 mins

        Public Sub New(ByVal SetName As String, ByVal Color As Color)
            Me.SetName = SetName
            Me.Color = Color
        End Sub

        Public DashStyle As System.Drawing.Drawing2D.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid

        Public PenWidth As Single = 2

        Public Sub New()

        End Sub

        Public MaxValue As Single = 100

        Public AutoMax As Boolean = True

        Public GridValues As New List(Of GridValueItem)
        Public Sub AddGridValue(ByVal GridValueItem As GridValueItem)
            GridValues.Add(GridValueItem)
        End Sub

        Public Enum DataStyles
            Smoothed_Line
            Line
            Bar
            Filled_Line
            Filled_Smoothed_Line
            None
        End Enum

#Region "Overlay"

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

        Private Shared frmOverlay As i00SpellCheck.HTMLToolTip.ToolTipPopup

        Private Sub frmOverlay_TipClosed(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim frm = TryCast(sender, i00SpellCheck.HTMLToolTip.ToolTipPopup)
            If frm IsNot Nothing Then
                'dispose...
                frm.Close()
            End If
        End Sub

        Public Shared Sub HideOverlay()
            If frmOverlay IsNot Nothing AndAlso frmOverlay.IsDisposed = False Then
                Dim gsp = TryCast(frmOverlay.Tag, GridSetProperty)
                If gsp IsNot Nothing Then
                    gsp.OverlayShowing = False
                End If
                frmOverlay.hide()
            End If
        End Sub

        Public Function GetAverage() As Single
            SyncLock GridValues
                If GridValues.Count = 0 Then Return 0
                Return GridValues.ToArray.Average(Function(x As clsGrid.GridSetProperty.GridValueItem) x.Value)
            End SyncLock
        End Function

        Private OverlayShowing As Boolean
        Private LastOverlayGrid As clsGrid
        Private LastDrawTime As Date
        Public Sub ShowGridOverlay(ByVal ctl As clsGrid)
            LastDrawTime = Now
            LastOverlayGrid = ctl
            Const ShadowExtraWidth As Single = 2
            Const ShadowBlur As Integer = 4

            HideOverlay()
            OverlayShowing = True

            frmOverlay = New i00SpellCheck.HTMLToolTip.ToolTipPopup With {.Tag = Me}
            AddHandler frmOverlay.TipClosed, AddressOf frmOverlay_TipClosed

            Dim ShadowOffset = CInt(ShadowExtraWidth + ShadowBlur)
            Dim ctlScreenLocation = ctl.PointToScreen(Point.Empty)

            frmOverlay.ShowInTaskbar = False
            'frmOverlay.ShowNoFocus(ctl.Parent)

            'make the overlay click-through
            Dim exstyle2 As Integer = GetWindowLong(frmOverlay.Handle, GWL_EXSTYLE)
            exstyle2 = exstyle2 Or WS_EX_TRANSPARENT
            SetWindowLong(frmOverlay.Handle, GWL_EXSTYLE, exstyle2)

            Using f = New Font(System.Drawing.SystemFonts.DefaultFont.FontFamily, 12, FontStyle.Regular, GraphicsUnit.Point)
                'Dim GridMaxPath = i00SpellCheck.DrawingFunctions.GetStringPath(GridMaxString, f, 0, 0)
                'Dim GridMaxBounds = GridMaxPath.GetBounds
                'ctlScreenLocation.X -= CInt(GridMaxBounds.Right) + 16
                'ctlScreenLocation.Y -= CInt((GridMaxBounds.Bottom + GridMaxBounds.Y) / 2)

                Using b As New Bitmap(ctl.Width, ctl.Height)
                    Using g = Graphics.FromImage(b)
                        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

                        Using p As New Pen(Me.Color, PenWidth)
                            p.DashStyle = Me.DashStyle
                            Dim yAve = b.Height - ((GetAverage() / MaxValue) * b.Height)
                            g.DrawLine(p, 0, yAve, b.Width, yAve)
                            g.DrawRectangle(p, New Rectangle(0, 0, b.Width - 1, b.Height - 1))
                        End Using
                        'g.TranslateTransform(ShadowOffset, ShadowOffset)
                        'Using sb As New SolidBrush(Color.FromArgb(127, Color.Black))
                        '    g.FillRectangle(sb, New Rectangle(0, 0, ctl.Width, ctl.Height))
                        'End Using

                        'Using gp As New System.Drawing.Drawing2D.GraphicsPath
                        '    gp.AddLine(0, 0, ctl.Width, ctl.Height)
                        '    'gp.AddLine(gp.GetLastPoint.X, gp.GetLastPoint.Y, ctl.Width, ctl.Height)

                        '    'Dim ps = GetSetPoints(New Rectangle(0, 0, ctl.Width, ctl.Height))
                        '    'If ps.Count > 1 Then
                        '    '    Using gpLine As New Drawing2D.GraphicsPath
                        '    '        gpLine.AddCurve(ps.ToArray)
                        '    '        gpLine.Flatten()
                        '    '        'remove the points to the left of the data area
                        '    '        Dim NewPoints As New List(Of PointF)
                        '    '        Dim NewTypes As New List(Of Byte)
                        '    '        For i = 0 To gpLine.PathData.Points.Count - 1
                        '    '            If gpLine.PathData.Points(i).X > 0 Then
                        '    '                NewPoints.Add(gpLine.PathData.Points(i))
                        '    '                NewTypes.Add(gpLine.PathData.Types(i))
                        '    '            End If
                        '    '        Next
                        '    '        gpLine.PathData.Points = NewPoints.ToArray
                        '    '        gpLine.PathData.Types = NewTypes.ToArray
                        '    '        gp.AddPath(gpLine, False)
                        '    '    End Using
                        '    'End If

                        '    Using p = New Pen(Drawing.Color.Black, PenWidth)
                        '        gp.Widen(p)
                        '    End Using

                        '    'gp.AddPath(GridMaxPath, False)
                        '    gp.AddPath(i00SpellCheck.DrawingFunctions.GetStringPath("200", f, 100, 0, , g), False)

                        '    'Using gpShadow = DirectCast(gp.Clone, Drawing2D.GraphicsPath)
                        '    '    Using p = New Pen(Drawing.Color.Black, ShadowExtraWidth)
                        '    '        gpShadow.Widen(p)
                        '    '    End Using
                        '    '    Using sbShadow As New SolidBrush(Drawing.Color.Black)
                        '    '        g.FillPath(sbShadow, gpShadow)
                        '    '    End Using
                        '    '    b.Filters.GausianBlur(ShadowBlur)
                        '    'End Using

                        '    Using sb As New SolidBrush(Drawing.Color.White)
                        '        g.FillPath(sb, gp)
                        '    End Using

                        'End Using

                    End Using

                    frmOverlay.Location = ctlScreenLocation
                    frmOverlay.Show(ctl, True, Integer.MaxValue, b)

                End Using
            End Using

        End Sub

#End Region

        Public DataStyle As DataStyles = DataStyles.Smoothed_Line

        Public Function GetSetPoints(ByVal rect As Rectangle, ByVal TheTime As Date) As List(Of System.Drawing.PointF)
            GetSetPoints = New List(Of System.Drawing.PointF)
            Dim FirstDate = (From xItem In GridValues Order By xItem.Time Descending Where TheTime.Subtract(xItem.Time).TotalMilliseconds > TimeSpanMS).FirstOrDefault  'first date before our timespan...
            Dim GraphValues As List(Of GridValueItem)

            If FirstDate Is Nothing Then
                'we have no entries older than the timespan ... use all items..
                GraphValues = (From xItem In GridValues Order By xItem.Time).ToList
            Else
                'only select entries newer or equal to the firstDate
                GraphValues = (From xItem In GridValues Where xItem.Time >= FirstDate.Time Order By xItem.Time).ToList
            End If
            GridValues = GraphValues

            If GraphValues.Count = 0 Then Exit Function

            If AutoMax = True Then
                'set the maxvalue to the highest value recorded...
                'but only if it is over the current max value...
                Dim Max = GraphValues.Max(Function(x As GridValueItem) x.Value)
                If Max > MaxValue Then
                    MaxValue = Max
                End If
            End If

            For Each item In GraphValues
                'find out the x location
                Dim x As Single = ((1 - (CSng(TheTime.Subtract(item.Time).TotalMilliseconds) / TimeSpanMS)) * rect.Width) + rect.X
                Dim y As Single = (1 - (item.Value / MaxValue)) * (rect.Height - 1)
                If DataStyle = DataStyles.Bar Then
                    If GetSetPoints.Count > 0 Then
                        GetSetPoints.Add(New System.Drawing.PointF(x, GetSetPoints.Last.Y))
                    End If
                    GetSetPoints.Add(New System.Drawing.PointF(x, y))
                Else
                    GetSetPoints.Add(New System.Drawing.PointF(x, y))
                End If
            Next
        End Function

        Public Sub DrawPath(ByVal rect As Rectangle, ByVal g As Graphics, Optional ByVal TheTime As Date = Nothing)

            If OverlayShowing Then
                'update the overlay...
                If Now.Subtract(LastDrawTime).TotalMilliseconds > 1000 Then
                    ShowGridOverlay(LastOverlayGrid)
                End If
            End If

            If TheTime = Nothing Then TheTime = Now
            Try
                If DataStyle = DataStyles.None Then Exit Sub
                Dim ps = GetSetPoints(rect, TheTime)
                If ps.Count > 1 Then
                    Using gp As New System.Drawing.Drawing2D.GraphicsPath
                        Select Case DataStyle
                            Case DataStyles.Bar, DataStyles.Line, DataStyles.Filled_Line
                                gp.AddLines(ps.ToArray)
                            Case Else
                                gp.AddCurve(ps.ToArray)
                        End Select
                        If PenWidth > 0 Then
                            Using p As New Pen(Me.Color, PenWidth)
                                p.DashStyle = DashStyle
                                g.DrawPath(p, gp)
                            End Using
                        End If

                        Select Case DataStyle
                            Case DataStyles.Line, DataStyles.Smoothed_Line

                            Case DataStyles.Filled_Smoothed_Line, DataStyles.Filled_Line, DataStyles.Bar
                                'finish the path
                                gp.AddLine(gp.GetLastPoint, New PointF(gp.GetLastPoint.X, rect.Bottom))
                                gp.AddLine(gp.GetLastPoint, New PointF(gp.PathPoints.First.X, rect.Bottom))
                                gp.CloseFigure()
                                Using sb As New SolidBrush(Color.FromArgb(127, Me.Color))
                                    g.FillPath(sb, gp)
                                End Using
                        End Select
                    End Using
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Class GridValueItem
            Public Time As DateTime
            Public Value As Single

            Public Sub New(ByVal Value As Single, Optional ByVal Time As DateTime = Nothing)
                If Time = New DateTime Then Time = Now

                Me.Value = Value
                Me.Time = Time
            End Sub
        End Class
    End Class

    Private mc_GridSets As New List(Of GridSetProperty)
    Public ReadOnly Property GridSets() As List(Of GridSetProperty)
        Get
            Return mc_GridSets
        End Get
    End Property

    Private Sub clsGrid_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Using b As New Bitmap(12, 12)
            Using g = Graphics.FromImage(b)
                Using p As New Pen(Me.ForeColor)
                    g.DrawLine(p, 0, 0, b.Width, 0)
                    g.DrawLine(p, 0, 0, 0, b.Height)
                End Using
            End Using
            Using tb As New TextureBrush(b)
                e.Graphics.FillRectangle(tb, e.ClipRectangle)
            End Using
        End Using

        For Each item In mc_GridSets
            item.DrawPath(e.ClipRectangle, e.Graphics)
        Next
    End Sub
End Class
