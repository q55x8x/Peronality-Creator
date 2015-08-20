Public Class frmSaveImage


    Overloads Sub ShowDialog(ByVal Words As String())
        Me.Words = Words
        MyBase.ShowDialog()
    End Sub

    Dim Words As String()

    Private Sub frmSaveImage_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListBox2.Items.AddRange(Words)
    End Sub

    Public Class ImageListObject
        Public Image As Image
        Public url As String
        Public Overrides Function ToString() As String
            Return IO.Path.GetFileNameWithoutExtension(url)
        End Function
    End Class

    Public Sub FindImage(ByVal ImageListObject As ImageListObject)
        Try
            Dim b As Bitmap = Autoexec.GetImageFromURL(ImageListObject.url)
            If ImageFilter.SupportsLockBits(b) = False Then Exit Sub
            If b Is Nothing Then Throw New Exception("Image error")
            If b.Width <> 16 OrElse b.Height <> 16 OrElse b.HorizontalResolution <> 96 OrElse b.VerticalResolution <> 96 Then
                Throw New Exception("Image size error")
            End If
            If b.Filters.ContainsAlpha = False Then
                Throw New Exception("Image contains no alpha")
            End If
            ImageListObject.Image = b
            AddItemToList(ImageListObject)
            'RefreshItems()
        Catch ex As Exception
            'error ... remove
            Debug.Print(ImageListObject.url & " - " & ex.Message)
            'RemoveItem(ImageListObject)
        End Try
    End Sub

    Delegate Sub RefreshItems_cb()
    Public Sub RefreshItems()
        If ListBox1.InvokeRequired Then
            Dim RefreshItems_cb As New RefreshItems_cb(AddressOf RefreshItems)
            ListBox1.Invoke(RefreshItems_cb)
        Else
            ListBox1.Invalidate()
        End If
    End Sub

    Delegate Sub RemoveItem_cb(ByVal Item As Object)
    Public Sub RemoveItem(ByVal Item As Object)
        Try
            If ListBox1.InvokeRequired Then
                Dim RemoveItem_cb As New RemoveItem_cb(AddressOf RemoveItem)
                ListBox1.Invoke(RemoveItem_cb, Item)
            Else
                ListBox1.Items.Remove(Item)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Dim SearchStart = 1

    Public Class LoadingListItem
        Public LoadStarted As Boolean
        Public Overrides Function ToString() As String
            Return "Loading..."
        End Function
    End Class
    Private Sub frmSaveImage_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

    End Sub

    Dim threads As New List(Of Threading.Thread)

    Private Sub AbortAllThreads()
        'For Each item In threads.ToArray
        '    Try
        '        item.Abort()
        '    Catch ex As Exception

        '    End Try
        '    threads.Remove(item)
        'Next
    End Sub

    Dim KeyWord As String

    Public Sub StartSearch(ByVal SearchStart As Integer)
        Dim url = "http://www.google.com.au/search?q=" & Replace(KeyWord, " ", "+") & "+type:png&hl=en&rls=en&tbs=isz:ex,iszw:16,iszh:16&tbm=isch&safe=off&start=" & SearchStart
        Dim html = Autoexec.getHTMLSource(url)
        Dim ImageUrlMatches = System.Text.RegularExpressions.Regex.Matches(html, "(?<=imgurl=).+?(?=&)")

        For Each match In ImageUrlMatches.OfType(Of System.Text.RegularExpressions.Match)()
            Dim ImageListObject As New ImageListObject With {.url = match.Value}
            Dim t As New System.Threading.Thread(AddressOf FindImage)
            t.IsBackground = True
            threads.Add(t)
            t.Start(ImageListObject)
        Next
        UpdateLoadingItem()
    End Sub

    Delegate Sub UpdateLoadingItem_cb()
    Private Sub UpdateLoadingItem()
        Try
            If ListBox1.InvokeRequired Then
                Dim UpdateLoadingItem_cb As New UpdateLoadingItem_cb(AddressOf UpdateLoadingItem)
                ListBox1.Invoke(UpdateLoadingItem_cb)
            Else
                'For Each item In ListBox1.Items.OfType(Of LoadingListItem)().ToArray
                '    ListBox1.Items.Remove(item)
                'Next
                ListBox1.Items.Add(New LoadingListItem)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Delegate Sub AddItemToList_cb(ByVal Item As Object)
    Private Sub AddItemToList(ByVal Item As Object)
        Try
            If ListBox1.InvokeRequired Then
                Dim AddItemToList_cb As New AddItemToList_cb(AddressOf AddItemToList)
                ListBox1.Invoke(AddItemToList_cb, Item)
            Else
                ListBox1.Items.Add(Item)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListBox1.DoubleClick

    End Sub

    Private Sub ListBox1_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles ListBox1.DrawItem
        If e.Index <> -1 Then
            Dim item = DirectCast(sender, ListBox).Items(e.Index)
            e.DrawBackground()
            e.DrawFocusRectangle()
            Dim ImageListObject = TryCast(item, ImageListObject)
            If ImageListObject IsNot Nothing Then
                If ImageListObject.Image IsNot Nothing Then
                    e.Graphics.DrawImage(ImageListObject.Image, New Rectangle(0, e.Bounds.Y, ListBox1.ItemHeight, ListBox1.ItemHeight))
                End If
            End If
            Dim LoadingListItem = TryCast(item, LoadingListItem)
            If LoadingListItem IsNot Nothing AndAlso LoadingListItem.LoadStarted = False Then
                'load the next set?
                LoadingListItem.LoadStarted = True
                Dim t As New System.Threading.Thread(AddressOf StartSearch)
                t.IsBackground = True
                threads.Add(t)
                t.Start(SearchStart)
                SearchStart += 20
            End If
            e.Graphics.DrawString(item.ToString, System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.ControlText, DirectCast(sender, ListBox).ItemHeight, e.Bounds.Y + (ListBox1.ItemHeight - e.Graphics.MeasureString("Ag", System.Drawing.SystemFonts.DefaultFont).Height))
        End If
    End Sub

    Private Sub ListBox1_MeasureItem(ByVal sender As Object, ByVal e As System.Windows.Forms.MeasureItemEventArgs) Handles ListBox1.MeasureItem
        If e.Index <> -1 Then
            Dim item = ListBox1.Items(e.Index)
            Dim LoadingListItem = TryCast(item, LoadingListItem)
            If LoadingListItem IsNot Nothing Then
                e.ItemHeight = 1
            End If
        End If
    End Sub

    Private Sub ListBox1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListBox1.MouseDoubleClick
        Dim index = ListBox1.IndexFromPoint(e.Location)
        If index <> -1 Then
            Dim ImageListObject = TryCast(ListBox1.Items(index), ImageListObject)
            If ImageListObject IsNot Nothing AndAlso ImageListObject.Image IsNot Nothing AndAlso ListBox2.SelectedItem IsNot Nothing Then
                ImageListObject.Image.Save("def\" & ListBox2.SelectedItem.ToString & ".png", System.Drawing.Imaging.ImageFormat.Png)
                ListBox2.Invalidate()
            End If
        End If
    End Sub

    Private Sub ListBox2_DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles ListBox2.DrawItem
        If e.Index <> -1 Then
            Dim item = DirectCast(sender, ListBox).Items(e.Index)
            e.DrawBackground()
            e.DrawFocusRectangle()


            If FileIO.FileSystem.FileExists("def\" & item.ToString & ".png") Then
                Using b As New Bitmap("def\" & item.ToString & ".png")
                    e.Graphics.DrawImage(b, New Rectangle(0, e.Bounds.Y, ListBox1.ItemHeight, ListBox1.ItemHeight))
                End Using
            End If

            e.Graphics.DrawString(item.ToString, System.Drawing.SystemFonts.DefaultFont, System.Drawing.SystemBrushes.ControlText, DirectCast(sender, ListBox).ItemHeight, e.Bounds.Y + (ListBox1.ItemHeight - e.Graphics.MeasureString("Ag", System.Drawing.SystemFonts.DefaultFont).Height))
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox2.SelectedIndexChanged
        Static LastSelectedItem As Object
        If ListBox2.SelectedItem Is Nothing Then
            LastSelectedItem = Nothing
        End If
        If My.Computer.Keyboard.ShiftKeyDown AndAlso LastSelectedItem IsNot Nothing Then
            'copy...
            FileCopy("def\" & LastSelectedItem.ToString & ".png", "def\" & ListBox2.SelectedItem.ToString & ".png")
            ListBox2.Invalidate()
        End If
        LastSelectedItem = ListBox2.SelectedItem.ToString
        If My.Computer.Keyboard.CtrlKeyDown = False Then
            AbortAllThreads()
            ListBox1.Items.Clear()
            KeyWord = ListBox2.SelectedItem.ToString
            SearchStart = 1
            UpdateLoadingItem()
        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = vbCr Then
            AbortAllThreads()
            ListBox1.Items.Clear()
            KeyWord = TextBox1.Text
            SearchStart = 1
            UpdateLoadingItem()
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub
End Class