Imports System.Reflection
Imports System.Diagnostics
Imports System.Drawing.Drawing2D

Public Class AboutScreen

    Private Sub bpLogo_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles bpLogo.Paint
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality

        Using gb As New LinearGradientBrush(New Point(0, 0), New Point(0, bpLogo.ClientSize.Height), Color.Transparent, DrawingFunctions.AlphaColor(Color.Black, 31))
            e.Graphics.FillRectangle(gb, bpLogo.ClientRectangle)
        End Using
        'e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.High
        'e.Graphics.DrawImage(My.Resources.Icon, New Point(CInt((bpLogo.ClientSize.Width - My.Resources.Icon.Width) / 2), CInt((bpLogo.ClientSize.Height - My.Resources.Icon.Height) / 2)))
        Using b As New SolidBrush(Color.FromArgb(255, 120, 154, 255))
            'Dim LogoSize As Single = Me.ClientSize.Width
            Dim LogoSize = CSng(Me.ClientSize.Height * 1.25)
            Dim LeftPos = CSng(Me.ClientSize.Width - (LogoSize * 0.75))
            If LeftPos < -(LogoSize * 0.25) Then LeftPos = CSng(-(LogoSize * 0.25))
            DrawingFunctions.DrawLogo(e.Graphics, b, New RectangleF(LeftPos, CSng((bpLogo.ClientSize.Height - LogoSize) / 2), LogoSize, LogoSize))
        End Using

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Close()
    End Sub

    Public Sub New()
        InitializeComponent()
        Me.Size = New Size(400, 400)
    End Sub

    Private Sub AboutScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim a As Assembly = System.Reflection.Assembly.GetExecutingAssembly

        lblReferences.Width = pnlReferences.ClientSize.Width - lblReferences.Left - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth
        lblReferences.Text = Join((From xItem In a.GetReferencedAssemblies Order By xItem.Name Select xItem.Name).ToArray, vbCrLf)

        'Dim fi As FileVersionInfo = FileVersionInfo.GetVersionInfo(a.Location)

        lblVersion.Text = "Version: " & a.GetName.Version.ToString

        SelectedTab = New TabItem("About")
        Tabs.Add(SelectedTab)
        Tabs.Add(New TabItem("Legal"))

        Dim ControlColor As Color = DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.Control), 127)
        pnlAbout.BackColor = ControlColor
        For Each tab As Control In pnlTabHolders.Controls
            tab.Dock = DockStyle.Fill
            For Each Control As Control In tab.Controls
                If TypeOf Control Is Panel Then
                    AddHandler CType(Control, Panel).Scroll, AddressOf pnlContents_Scroll
                End If
                If LCase(TryCast(Control.Tag, String)) <> "false" Then
                    Control.BackColor = ControlColor
                    For Each subctl In Control.Controls.OfType(Of Control)()
                        subctl.BackColor = Color.Transparent
                    Next
                ElseIf tab.BackColor = ControlColor Then
                    Control.BackColor = Color.Transparent
                End If
            Next
            tab.Dock = DockStyle.Fill
        Next
        UpdateTabs()

        SiteLink.Text = "http://i00Productions.org"
        SiteLinkProduct.Visible = False
        'SiteLinkProduct.Text = "http://www.vbforums.com/showthread.php?p=4075093"

    End Sub

    Private Sub pnlContents_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs)
        Dim sPanel = TryCast(sender, Panel)
        If sPanel IsNot Nothing Then sPanel.Refresh()
    End Sub

    Private Sub SiteLink_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles SiteLink.LinkClicked, SiteLinkProduct.LinkClicked
        Dim sLinkLabel = TryCast(sender, LinkLabel)
        If sLinkLabel IsNot Nothing Then System.Diagnostics.Process.Start(sLinkLabel.Text)
    End Sub

    Dim SelectedTab As TabItem = Nothing

    Dim Tabs As New List(Of TabItem)

    Public Class TabItem
        Public Text As String
        Public Location As Rectangle
        Public Sub New(ByVal Text As String)
            Me.Text = Text
        End Sub
    End Class

    Private Sub UpdateTabs()
        For Each tab As Control In pnlTabHolders.Controls
            If LCase(TryCast(tab.Tag, String)) = LCase(SelectedTab.Text) Then
                tab.Visible = True
            Else
                tab.Visible = False
            End If
        Next
    End Sub

    Private Sub bpTabs_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bpTabs.MouseClick
        Dim TabsClicked = From xItem In Tabs Where xItem.Location.Contains(e.Location)
        If TabsClicked.Count > 0 Then
            SelectedTab = TabsClicked(0)
            UpdateTabs()
            bpTabs.Refresh()
        End If
    End Sub

    Private Sub bpTabs_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles bpTabs.Paint
        e.Graphics.SmoothingMode = SmoothingMode.HighQuality
        Dim FontSize As Single = DrawingFunctions.Text.DetermineFontSizeForBounds("", bpTabs.Font, , bpTabs.Height)
        Dim Font As Font = DrawingFunctions.Text.FontSetNewSize(bpTabs.Font, FontSize)

        Dim TabOffset As Integer = 0
        Dim LastGp As GraphicsPath = Nothing
        For i = 0 To Tabs.Count - 1
            Using TabBrush As New LinearGradientBrush(New Point(0, 0), New Point(0, bpTabs.ClientSize.Height), CType(IIf(Tabs(i) Is SelectedTab, DrawingFunctions.BlendColor(Color.FromKnownColor(KnownColor.Highlight), bpLogo.BackColor, 127), DrawingFunctions.BlendColor(Color.Black, bpLogo.BackColor, 31)), Color), bpLogo.BackColor)
                Dim gp As New GraphicsPath
                Dim TextWidth As Integer = CInt(e.Graphics.MeasureString(Tabs(i).Text, Font).Width)
                Dim TextRight As Integer = TabOffset + TextWidth
                gp.AddLine(New Point(TabOffset, (bpTabs.ClientSize.Height - If(Tabs(i) Is SelectedTab, 0, 1))), New Point(TabOffset, 0))
                gp.AddLine(gp.GetLastPoint, New Point(TextRight, 0))
                gp.AddLine(gp.GetLastPoint, New Point(TextRight + bpTabs.ClientSize.Height, bpTabs.ClientSize.Height - If(Tabs(i) Is SelectedTab, 0, 1)))
                If LastGp IsNot Nothing Then
                    If Tabs(i) IsNot SelectedTab Then
                        e.Graphics.SetClip(LastGp, CombineMode.Exclude)
                    End If
                End If
                If Tabs(i) IsNot SelectedTab Then
                    gp.CloseFigure()
                End If
                e.Graphics.FillPath(TabBrush, gp)
                DrawingFunctions.Text.DrawString(e.Graphics, Tabs(i).Text, Font, New SolidBrush(Color.FromKnownColor(KnownColor.ControlText)), TabOffset, 1, DrawingFunctions.Text.TextRenderMode.Windows)
                e.Graphics.DrawPath(New Pen(Color.FromKnownColor(KnownColor.ControlDark)), gp)
                Tabs(i).Location = New Rectangle(TabOffset, 0, CInt(TextWidth + (bpTabs.ClientSize.Height / 2)), bpTabs.ClientSize.Height)
                TabOffset = CInt(TextRight + (bpTabs.ClientSize.Height / 2))
                LastGp = gp
            End Using
        Next
        e.Graphics.DrawLine(New Pen(Color.FromKnownColor(KnownColor.ControlDark)), CSng(TabOffset + (bpTabs.ClientSize.Height / 2)), bpTabs.ClientSize.Height - 1, bpTabs.ClientSize.Width, bpTabs.ClientSize.Height - 1)
    End Sub

    Private Sub pnlTabHolders_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlTabHolders.Paint
        e.Graphics.DrawRectangle(New Pen(Color.FromKnownColor(KnownColor.ControlDark)), New Rectangle(0, -1, pnlTabHolders.ClientSize.Width - 1, pnlTabHolders.ClientSize.Height))
    End Sub

    Private Sub TransParentControls_BackColorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Panel1.BackColorChanged
        Dim sControl = TryCast(sender, Control)
        If sControl IsNot Nothing AndAlso sControl.BackColor <> Color.Transparent Then sControl.BackColor = Color.Transparent
    End Sub

End Class