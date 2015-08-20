Imports i00SpellCheck

Public Class frmTest

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'load the controls - could be done simpler... but this is dynamic and should support adding future controls to the OSControlRenderer
        Dim extOSControlRenderer As New OSControlRenderer
        '...for each control that this extension supports...
        For Each item In extOSControlRenderer.ControlTypes
            If item.IsAbstract = False Then
                '...create the control
                Dim createObject = System.Activator.CreateInstance(item)
                Dim createControl = TryCast(createObject, Control)
                If createControl IsNot Nothing Then
                    '...add it to the tab page
                    Dim TabPage As New TabPage(createControl.GetType.Name)
                    createControl.Dock = DockStyle.Fill
                    TabPage.Controls.Add(createControl)
                    TabControl1.TabPages.Add(TabPage)
                    Dim ext = New OSControlRenderer
                    'load the test data
                    ext.SetupControl(createControl)
                    '...load the extension
                    ControlExtensions.LoadSingleControlExtension(createControl, ext)
                End If
            End If
        Next

        'set the property grid to the extension
        UpdatePropertyGrid()
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        UpdatePropertyGrid()
    End Sub

    Private Sub UpdatePropertyGrid()
        PropertyGrid1.SelectedObject = TabControl1.SelectedTab.Controls(0).ExtensionCast(Of OSControlRenderer)()
    End Sub

End Class
