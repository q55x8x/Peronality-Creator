Public Class ControlExtPropGrid
    Inherits Panel
    Private WithEvents cboControlExts As New ComboBox With {.DropDownStyle = ComboBoxStyle.DropDownList, .Dock = DockStyle.Top}
    Private WithEvents PropGrid As New PropertyGrid With {.Dock = DockStyle.Fill}

    Public Overrides Sub Refresh()
        PropGrid.Refresh()
        'MyBase.Refresh()
    End Sub

    Public Sub New()
        Me.Controls.Add(PropGrid)
        Me.Controls.Add(cboControlExts)
    End Sub

    Dim mc_SelectedObject As Control
    Public Property SelectedObject() As Control
        Get
            Return mc_SelectedObject
        End Get
        Set(ByVal value As Control)
            mc_SelectedObject = value
            cboControlExts.Items.Clear()
            If i00SpellCheck.ControlExtensions.LoadedControlExtensions.ContainsKey(value) Then
                For Each item In i00SpellCheck.ControlExtensions.LoadedControlExtensions.Item(value)
                    cboControlExts.Items.Add(item)
                Next
            End If
            cboControlExts.SelectedItem = cboControlExts.Items(0)
        End Set
    End Property

    Private Sub cboControlExts_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboControlExts.SelectedValueChanged
        PropGrid.SelectedObject = cboControlExts.SelectedItem
    End Sub
End Class
