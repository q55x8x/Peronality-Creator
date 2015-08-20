Imports i00SpellCheck

Public Class frmTest

    Private Sub Control_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim control = TryCast(sender, Control)
        If control IsNot Nothing Then
            PropertyGrid1.SelectedObject = control.ExtensionCast(Of SelectedControlHighlight)()
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ''if you wish to apply this extension to a control that is not setup automatically, you can call:
        'ControlExtensions.LoadSingleControlExtension(YourControl, New SelectedControlHighlight)

        'load the controls - could be done simpler... but this is dynamic and should support adding future controls to the OSControlRenderer
        Dim extOSControlRenderer As New SelectedControlHighlight
        '...for each control that this extension supports...
        Dim AddControls As New List(Of Control)
        For Each item In extOSControlRenderer.ControlTypes
            If item.IsAbstract Then
                'cannot create instance for eg TextBoxBase... lets go through everything and try to create a control that comes from this
                AddControls.AddRange(i00SpellCheck.PluginManager(Of Control).GetAllPluginsInReferencedAssemblies(item.Assembly, item))
            Else
                'create control
                AddControls.Add(TryCast(System.Activator.CreateInstance(item), Control))
            End If
        Next

        For Each item In (From xItem In AddControls Order By xItem.GetType.Name).ToArray
            Dim ItemVisibility = item.GetType.GetCustomAttributes(True).OfType(Of System.ComponentModel.DesignTimeVisibleAttribute).FirstOrDefault()
            If item.GetType Is GetType(DataGridViewTextBoxEditingControl) Then Continue For
            If ItemVisibility IsNot Nothing AndAlso ItemVisibility.Visible = False Then Continue For

            Dim ControlPanel As New Panel
            ControlPanel.Dock = DockStyle.Top
            pnlControls.Controls.Add(ControlPanel)
            ControlPanel.BringToFront()

            item.Top = item.Margin.Top

            Dim pIcon As New PictureBox
            pIcon.Left = pIcon.Margin.Left
            Dim propToolBoxIcon As New ToolboxBitmapAttribute(item.GetType)
            pIcon.Image = DirectCast(propToolBoxIcon.GetImage(GetType(Label), True), Bitmap)
            ControlPanel.Controls.Add(pIcon)
            pIcon.SizeMode = PictureBoxSizeMode.StretchImage
            pIcon.Size = New Size(16, 16)
            pIcon.Top = item.Top

            item.Left = item.Margin.Left + pIcon.Right + pIcon.Margin.Right

            Try
                item.Text = item.GetType.Name
            Catch ex As Exception

            End Try

            '...add it to the panel 
            item.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
            item.Width = ControlPanel.ClientSize.Width - (item.Left + item.Margin.Right)

            Dim ext = New SelectedControlHighlight
            ext.SetupControl(item)

            ControlPanel.Height = item.Bottom + item.Margin.Bottom
            ControlExtensions.LoadSingleControlExtension(item, ext)

            AddHandler item.SizeChanged, AddressOf Control_SizeChanged
            AddHandler item.GotFocus, AddressOf Control_GotFocus

            ControlPanel.Controls.Add(item)
        Next

        Dim BottomControl = pnlControls.Controls.OfType(Of Control).Max(Function(x As Control) x.Bottom)
        BottomControl += Me.Height - pnlControls.Height
        Dim OldHeight = Me.Height
        Me.MinimumSize = New Size(Me.MinimumSize.Width, BottomControl)
        Me.Top -= CInt((Me.Height - OldHeight) / 2)

        'set the property grid to the extension
        UpdatePropertyGrid()
    End Sub

    Public Sub Control_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim control = DirectCast(sender, Control)
        control.Parent.Height = control.Bottom + control.Margin.Bottom
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        UpdatePropertyGrid()
    End Sub

    Private Sub UpdatePropertyGrid()
        'PropertyGrid1.SelectedObject = TabControl1.SelectedTab.Controls(0).ExtensionCast(Of OSControlRenderer)()
    End Sub

End Class
