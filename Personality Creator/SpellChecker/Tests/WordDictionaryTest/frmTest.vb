Imports i00SpellCheck

Public Class frmTest

    Private Sub frmTest_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Me.Icon = My.Resources.Icon

        i00SpellCheck.Dictionary.DefaultDictionary = New Word.Word_Dic

        i00SpellCheck.Synonyms.DefaultSynonyms = New Word.Word_Syn

        Me.EnableControlExtensions()

        PropertyGrid1.SelectedObject = TextBox1.ExtensionCast(Of i00SpellCheck.SpellCheckTextBox)()

    End Sub

 End Class
