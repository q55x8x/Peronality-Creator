Public Class SpellCheck

    Public Shared DictionaryPath As String = IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "bin\dic.dic")
    Public Shared dictionary = New i00SpellCheck.FlatFileDictionary(DictionaryPath)

    Public Shared Sub CheckSpellingResponse(ByVal page As Page)
        page.Response.Clear()

        Dim words = Split(page.Request.QueryString("words"), ";")

        Dim WordCheck As New Dictionary(Of String, Boolean)(System.StringComparer.OrdinalIgnoreCase)
        For Each item In words
            If WordCheck.ContainsKey(item) = False Then
                WordCheck.Add(item, dictionary.SpellCheckWord(item) <> i00SpellCheck.Dictionary.SpellCheckWordError.SpellError)
            End If
        Next
        page.Response.Write("UpdateCorrections(['" & Join((From xItem In WordCheck Where xItem.Value = True Select xItem.Key).ToArray, "','") & "'], ['" & Join((From xItem In WordCheck Where xItem.Value = False Select xItem.Key).ToArray, "','") & "']);" & vbCrLf)
        page.Response.End()
    End Sub

End Class
