Public Class Misc

    Public Shared Sub ReferenceJSFile(ByVal Page As Web.UI.Page, ByVal ScriptFile As String)
        Dim ScriptKey = LCase(IO.Path.GetFileName(ScriptFile))
        If Page.ClientScript.IsClientScriptIncludeRegistered(ScriptKey) = False Then
            Page.ClientScript.RegisterClientScriptInclude(ScriptKey, ScriptFile)
            Page.Response.Write("<script type=""text/javascript"" src=""" & ScriptFile & """></script>")
        End If
    End Sub

    Public Shared Function JavascriptEscape(ByVal Data As String) As String
        'for: ' " \
        JavascriptEscape = System.Text.RegularExpressions.Regex.Replace(Data, "('|""|\\)", "\$1")

    End Function

End Class
