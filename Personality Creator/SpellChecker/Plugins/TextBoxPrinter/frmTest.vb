Imports i00SpellCheck

Public Class frmTest

    Private Sub frmTest_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'load the extension
        ControlExtensions.LoadSingleControlExtension(RichTextBox1, New TextBoxPrinter)
        'load the test data
        RichTextBox1.ExtensionCast(Of TextBoxPrinter)().SetupControl(RichTextBox1)

        'set the property grid to the extension
        PropertyGrid1.SelectedObject = RichTextBox1.ExtensionCast(Of TextBoxPrinter)()

        'icon
        Try
            Me.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tsbPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPrint.Click
        RichTextBox1.ExtensionCast(Of TextBoxPrinter)().Print()
    End Sub
End Class
