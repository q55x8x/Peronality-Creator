Public Class Word

    Friend Shared ReadOnly Property WordApp() As Microsoft.Office.Interop.Word.Application
        Get
            Static mc_WordApp As Microsoft.Office.Interop.Word.Application = Nothing
            If mc_WordApp Is Nothing Then
                mc_WordApp = New Microsoft.Office.Interop.Word.Application
                mc_WordApp.Documents.Add()
                AddHandler Application.ApplicationExit, AddressOf ApplicationExit
                mc_WordApp.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsAll
                'mc_WordApp.Visible = True
            End If
            Return mc_WordApp
        End Get
    End Property

    Private Shared Sub ApplicationExit(ByVal sender As Object, ByVal e As System.EventArgs)
        WordApp.Quit()
    End Sub

End Class
