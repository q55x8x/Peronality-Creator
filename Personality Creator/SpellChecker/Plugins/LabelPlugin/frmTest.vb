Imports i00SpellCheck

Public Class frmTest

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'load the extension
        ControlExtensions.LoadSingleControlExtension(Label1, New SpellCheckLabel)

        'load the test data
        DirectCast(Label1.SpellCheck, iTestHarness).SetupControl(Label1)

        'set the property grid to the extension
        PropertyGrid1.SelectedObject = Label1.SpellCheck

        'form icon
        Dim propToolBoxIcon As New ToolboxBitmapAttribute(GetType(Label))
        Using b As Bitmap = DirectCast(propToolBoxIcon.GetImage(GetType(Label), True), Bitmap)
            Me.Icon = Icon.FromHandle(b.GetHicon)
        End Using
    End Sub

End Class
