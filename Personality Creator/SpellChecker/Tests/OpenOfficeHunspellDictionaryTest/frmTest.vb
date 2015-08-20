Imports i00SpellCheck

Public Class frmTest

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Icon = My.Resources.Icon

        Dim Hunspell_Dic As New Hunspell_Dic
        Hunspell_Dic.LoadFromFile("en_US.dic")
        i00SpellCheck.Dictionary.DefaultDictionary = Hunspell_Dic

        Using MessageBoxManager As New MessageBoxManager
            MessageBoxManager.Yes = "Hunspell"
            MessageBoxManager.No = "i00"
            Select Case MsgBox("The Hunspell thesaurus uses alot of memory." & vbCrLf & "This is used for the ""Change to"" suggestions." & vbCrLf & vbCrLf & "Do you want to load the Hunspell thesaurus or use the i00 Spell Check synonyms instead?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes
                Case MsgBoxResult.Yes 'Hunspell thesaurus
                    Dim Hunspell_Syn As New Hunspell_Syn
                    Hunspell_Syn.File = "th_en_US.dat"
                    i00SpellCheck.Synonyms.DefaultSynonyms = Hunspell_Syn
                Case Else 'i00 Spell Check synonyms

            End Select
        End Using

        Me.EnableControlExtensions()

        PropertyGrid1.SelectedObject = TextBox1.ExtensionCast(Of i00SpellCheck.SpellCheckTextBox)()

    End Sub
End Class
