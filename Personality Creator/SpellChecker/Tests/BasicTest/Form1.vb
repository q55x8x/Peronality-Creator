Imports i00SpellCheck

Public Class Form1

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TextBox1.SelectionStart = 0
        TextBox1.SelectionLength = 0

        'The following is all that needs to be called...
        Me.EnableControlExtensions()
        'All multi-line textboxes within the control specified will automatically be spell checked!

        'If the control specified is a form then all owned forms will AUTOMATICALLY have their textboxes 
        'spellchecked also! (An example can be seen in the "New Form" button)


    End Sub

#Region "About button"

    Private Sub tsiAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsiAbout.Click
        Using frmAbout As New i00SpellCheck.AboutScreen
            frmAbout.ShowInTaskbar = False
            frmAbout.StartPosition = FormStartPosition.CenterParent
            frmAbout.ShowDialog(Me)
        End Using
    End Sub

#End Region

#Region "New form button - to show spell check working in a form with NO user code"

    Private Sub tsiNewForm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsiNewForm.Click
        Dim frm As New Form2
        frm.TextBox1.SelectionStart = 0
        frm.TextBox1.SelectionLength = 0
        frm.Show(Me)
    End Sub

#End Region

End Class
