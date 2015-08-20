Imports i00SpellCheck

Public Class frmTest

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Hanks_Dic As New Hanks_Dic
        Hanks_Dic.LoadFromFile("dic.txt")
        i00SpellCheck.Dictionary.DefaultDictionary = Hanks_Dic

        Me.EnableControlExtensions()

        PropertyGrid1.SelectedObject = TextBox1.ExtensionCast(Of i00SpellCheck.SpellCheckTextBox)()

    End Sub
End Class
