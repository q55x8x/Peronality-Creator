Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim FileContents = My.Computer.FileSystem.ReadAllText("dic.dic").Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
        FileContents(0) = ""
        Dim DictWords = (From xItem In FileContents Where xItem <> "" Select xItem.ToLower).ToList

        Dim Meaning = ""

        Dim LastTick As Integer

        Dim ListOfMissingWords As New List(Of String)
        Dim ListOfTypes As New List(Of Integer)

        Dim asd As New Microsoft.Office.Interop.Word.Application
        Dim i As Integer
        For Each iWord In DictWords
            Try
                Dim lstMeanings As New List(Of String)
                Dim ThisSyn = asd.SynonymInfo(iWord)
                Dim UsedMeanings As New List(Of String)
                Dim iSpeechList As Integer = 1
                For Each item In ThisSyn.MeaningList
                    If UsedMeanings.Contains(item.ToString.ToLower) = False Then
                        UsedMeanings.Add(item.ToString.ToLower)
                        Dim ThisMeaning = item.ToString
                        Dim lstSyns As New List(Of String)
                        For Each iSynonym In ThisSyn.SynonymList(ThisMeaning)
                            Dim OK As Boolean = True
                            For Each iCheckWord In iSynonym.ToString.Split(" "c, "-"c)
                                If DictWords.Contains(iCheckWord.ToLower) = False Then
                                    'fail?
                                    Dim fail As Boolean = True
                                    If iCheckWord.ToLower.EndsWith("'s") Then
                                        If DictWords.Contains(Strings.Left(iCheckWord, Len(iCheckWord) - 2).ToLower) Then
                                            fail = False
                                        End If
                                    ElseIf iCheckWord.ToLower.EndsWith("ies") Then
                                        If DictWords.Contains(System.Text.RegularExpressions.Regex.Replace(iCheckWord, "ies$", "y").ToLower) Then
                                            fail = False
                                        End If
                                    ElseIf iCheckWord.ToLower.EndsWith("s") Then
                                        If DictWords.Contains(Strings.Left(iCheckWord, Len(iCheckWord) - 1).ToLower) Then
                                            fail = False
                                        End If
                                    End If
                                    If fail Then
                                        OK = False
                                        If ListOfMissingWords.Contains(iCheckWord.ToLower) = False Then
                                            ListOfMissingWords.Add(iCheckWord.ToLower)
                                        End If
                                        'MsgBox("Dict does not contain " & iCheckWord)
                                        Exit For
                                    End If
                                End If
                            Next
                            If OK = True Then
                                lstSyns.Add(iSynonym.ToString)
                            End If
                        Next
                        Dim ThisSpeechList As Integer = ThisSyn.PartOfSpeechList(iSpeechList)
                        If ListOfTypes.Contains(ThisSpeechList) = False Then ListOfTypes.Add(ThisSpeechList)
                        lstMeanings.Add(ThisMeaning & ";" & ThisSpeechList & ">" & Join(lstSyns.ToArray, ";"))
                        iSpeechList += 1
                    End If
                Next

                If lstMeanings.Count > 0 Then
                    Meaning &= iWord & "|" & Join(lstMeanings.ToArray, "|") & vbCrLf
                End If
            Catch ex As Exception
                MsgBox("Error matching " & iWord & " - " & ex.Message)
            End Try

            i += 1

            Dim ThisTick = Environment.TickCount
            If ThisTick - LastTick > 1000 Then
                Debug.Print((i / DictWords.Count) * 100)
                LastTick = ThisTick
            End If
        Next

        'MsgBox(asd.SynonymInfo("happy"))

        My.Computer.FileSystem.WriteAllText("WordMeaning.txt", Meaning, False)
        My.Computer.FileSystem.WriteAllText("MissingWords.txt", Join(ListOfMissingWords.ToArray, vbCrLf), False)

    End Sub
End Class
