Public Class Autoexec
    Public Shared Sub Main()

        Application.EnableVisualStyles()

        FileIO.FileSystem.CreateDirectory("def")
        FileIO.FileSystem.CreateDirectory("def\not")
        Dim FileContents = My.Computer.FileSystem.ReadAllText("def.def")
        Dim mc = System.Text.RegularExpressions.Regex.Matches(FileContents, "(?<=\()[\w ]{1,}?(?=\))", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim wordlist = (From xItem In mc.OfType(Of System.Text.RegularExpressions.Match)() Order By xItem.Value Select xItem.Value.ToLower).ToArray

        'only pick up words that are used more than 1x
        Dim words = wordlist.Distinct.ToArray  '(From xItem In (From xItem In wordlist.Distinct Group Join xItem2 In wordlist On xItem2 Equals xItem Into Group Select Word = xItem, Count = Group.Count) Where xItem.Count > 1 Select xItem.Word).ToArray


        'Dim LastItemDone = (From xItem In words Where FileIO.FileSystem.FileExists("def\" & xItem.Word & ".png")).LastOrDefault
        'If LastItemDone IsNot Nothing Then
        '    words = (From xItem In words Where words.IndexOf(xItem) > words.IndexOf(LastItemDone)).ToList
        'End If

        Dim Files = FileIO.FileSystem.GetFiles("def").ToArray()

        Dim keepwords = (From xItem2 In words Select xItem2.ToLower).ToArray
        For Each item In Files
            If keepwords.Contains(IO.Path.GetFileNameWithoutExtension(item).ToLower) = False Then
                FileIO.FileSystem.MoveFile(item, "def\not\" & IO.Path.GetFileName(item))
            End If
        Next

        Dim wordarr = (From xItem In words Select xItem).ToArray
        If wordarr.Count > 0 Then
            Using frm As New frmSaveImage
                frm.ShowDialog(wordarr)
            End Using
        End If

        'For Each item In words
        '    If FileIO.FileSystem.FileExists("def\" & item.Word & ".png") = False Then
        '        Try
        '            Using frm As New frmSaveImage
        '                frm.ShowDialog(item.Word)
        '            End Using
        '        Catch ex As Exception

        '        End Try
        '    End If
        'Next
    End Sub

    Public Shared Function GetImageFromURL(ByVal url As String) As Image
        Dim myUri As New Uri(url)
        Dim myHttpWebRequest As System.Net.HttpWebRequest = DirectCast(System.Net.WebRequest.Create(myUri), System.Net.HttpWebRequest)
        myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"
        Using myHttpWebResponse As System.Net.HttpWebResponse = DirectCast(myHttpWebRequest.GetResponse(), System.Net.HttpWebResponse)
            Using stream = myHttpWebResponse.GetResponseStream()
                Return New Bitmap(stream)
            End Using
        End Using
    End Function

    Public Shared Function getHTMLSource(ByVal url As String) As String
        getHTMLSource = ""
        If url.Length > 0 Then
            Dim myUri As New Uri(url)
            Dim myHttpWebRequest As System.Net.HttpWebRequest = DirectCast(System.Net.WebRequest.Create(myUri), System.Net.HttpWebRequest)
            myHttpWebRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"
            Using myHttpWebResponse As System.Net.HttpWebResponse = DirectCast(myHttpWebRequest.GetResponse(), System.Net.HttpWebResponse)
                Using stream = myHttpWebResponse.GetResponseStream()
                    Using reader = New System.IO.StreamReader(stream)
                        Return reader.ReadToEnd
                    End Using
                End Using
            End Using
        End If
    End Function

End Class
