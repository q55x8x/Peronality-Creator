Imports i00SpellCheck

Public Class frmTest

    Private Sub frmTest_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load the extension
        ControlExtensions.LoadSingleControlExtension(RichTextBox1, New KonamiCode)
        'load the test data
        RichTextBox1.ExtensionCast(Of KonamiCode)().SetupControl(RichTextBox1)

        'set the property grid to the extension
        PropertyGrid1.SelectedObject = RichTextBox1.ExtensionCast(Of KonamiCode)()

    End Sub
End Class
