Imports FastColoredTextBoxNS
Imports i00SpellCheck

Public Class frmTest

    Private Sub frmTest_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load the extension
        ControlExtensions.LoadSingleControlExtension(FastColoredTextBox1, New SpellCheckFastColoredTextBox)

        'load the test data
        Dim AddControl = DirectCast(FastColoredTextBox1.SpellCheck(), iTestHarness).SetupControl(FastColoredTextBox1)
        AddControl.Dock = DockStyle.Fill
        Me.Controls.Add(AddControl)
        AddControl.BringToFront()

        ''The following will spell check strings not in html tags!
        'DirectCast(FastColoredTextBox1.SpellCheck(), FastColoredTextBoxPlugin.SpellCheckFastColoredTextBox).SpellCheckMatch = "(?<!<[^>]*)[^<^>]*"

        'set the property grid to the extension
        PropertyGrid1.SelectedObject = FastColoredTextBox1.SpellCheck

        'form icon
        Dim propToolBoxIcon As New ToolboxBitmapAttribute(GetType(FastColoredTextBox))
        Using b As Bitmap = DirectCast(propToolBoxIcon.GetImage(GetType(FastColoredTextBox), True), Bitmap)
            Me.Icon = Icon.FromHandle(b.GetHicon)
        End Using
    End Sub

End Class
