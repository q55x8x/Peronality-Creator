'i00 Key toolbar helper
'©i00 Productions All rights reserved
'Created by Kris Bennett
'----------------------------------------------------------------------------------------------------
'All property in this file is and remains the property of i00 Productions, regardless of its usage,
'unless stated otherwise in writing from i00 Productions.
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Public Class KeyToolbarHelper
    Inherits List(Of KeyItems)

    Public Class KeyItems
        Public KeyName As String
        Public Color As Color
        Public Description As String
        Public Filter As String
        Public Sub New(ByVal KeyName As String, ByVal Color As Color, ByVal Description As String, ByVal Filter As String)
            Me.KeyName = KeyName
            Me.Color = Color
            Me.Description = Description
            Me.Filter = Filter
        End Sub

        Public KeyButton As ToolStripButton
        Public FilterButton As ToolStripButton
    End Class

    Private WithEvents ToolStrip As ToolStrip

    Public Sub FillToolBarWithKeys(ByVal ToolStrip As ToolStrip)
        Me.ToolStrip = ToolStrip
        For Each Key In Me
            Key.KeyButton = New ToolStripButton
            Key.KeyButton.AutoToolTip = False
            Key.KeyButton.Text = Key.KeyName
            Key.KeyButton.Tag = Key.Description
            AddHandler Key.KeyButton.Click, AddressOf tsbKey_Click
            AddHandler Key.KeyButton.MouseEnter, AddressOf tsbKey_MouseEnter
            Dim b As New Bitmap(14, 14)
            Using g = Graphics.FromImage(b)
                g.Clear(Key.Color)
                Using p As New Pen(DrawingFunctions.AlphaColor(Color.FromKnownColor(KnownColor.MenuText), 63))
                    g.DrawRectangle(p, New Rectangle(0, 0, b.Width - 1, b.Height - 1))
                End Using
            End Using
            Key.KeyButton.Image = b
            ToolStrip.Items.Add(Key.KeyButton)

            Key.FilterButton = New ToolStripButton
            Key.FilterButton.DisplayStyle = ToolStripItemDisplayStyle.Image
            Key.FilterButton.Image = My.Resources.Filter
            Key.FilterButton.Text = "Filter by " & Key.KeyName
            Key.FilterButton.Visible = False
            AddHandler Key.FilterButton.Click, AddressOf tsbKeyFilter_Click
            AddHandler Key.FilterButton.MouseEnter, AddressOf tsbKey_MouseEnter
            ToolStrip.Items.Add(Key.FilterButton)

            If Key IsNot Me.Last Then
                Dim Seperator = New ToolStripSeparator
                Seperators.Add(Seperator)
                ToolStrip.Items.Add(Seperator)
            End If

        Next
    End Sub

    Public Seperators As New List(Of ToolStripSeparator)

    Public WithEvents KeyTooltip As New HTMLToolTip

    Private Sub tsbKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim thisItem = (From xItem In Me Where xItem.KeyButton Is sender).FirstOrDefault
        If thisItem IsNot Nothing Then
            If thisItem.Description <> "" Then
                KeyTooltip.Dispose()
                KeyTooltip = New HTMLToolTip With {.IsBalloon = True, .ToolTipTitle = thisItem.KeyName, .ToolTipIcon = ToolTipIcon.Info}
                KeyTooltip.ShowHTML(thisItem.Description, thisItem.KeyButton.Owner, New Point(CInt(thisItem.KeyButton.Bounds.Left + (thisItem.KeyButton.Width / 2)), CInt(thisItem.KeyButton.Bounds.Top + (thisItem.KeyButton.Height / 2))), 5000)
            End If
        End If
    End Sub

    Public Class KeyListEventArgs
        Inherits EventArgs
        Public KeyItem As KeyItems
    End Class

    Public Event FilterClicked(ByVal sender As Object, ByVal e As KeyListEventArgs)
    Private Sub tsbKeyFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim thisItem = (From xItem In Me Where xItem.FilterButton Is sender).FirstOrDefault
        If thisItem IsNot Nothing Then
            RaiseEvent FilterClicked(Me, New KeyListEventArgs() With {.KeyItem = thisItem})
        End If
    End Sub

    Public Sub HideAllFilters()
        For Each KeyItem In Me
            If KeyItem.FilterButton.Checked = False Then
                KeyItem.FilterButton.Visible = False
            End If
        Next
    End Sub

    Private Sub tsKeys_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStrip.MouseLeave
        HideAllFilters()
    End Sub

    Private Sub tsbKey_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim thisItem = (From xItem In Me Where xItem.KeyButton Is sender OrElse xItem.FilterButton Is sender).FirstOrDefault
        If thisItem IsNot Nothing Then
            HideAllFilters()
            If thisItem.Filter <> "" Then
                thisItem.FilterButton.Visible = True
            End If
            'Dict.BindingList.BindingSource.Filter = FilterButtons.Item(ToolStripButton)
        End If
    End Sub

    Public Sub FilterSet(ByVal Filter As String)
        For Each KeyItem In Me
            If KeyItem.Filter = Filter Then
                KeyItem.FilterButton.Checked = True
                KeyItem.FilterButton.Visible = True
            Else
                KeyItem.FilterButton.Checked = False
                KeyItem.FilterButton.Visible = False
            End If
        Next
        HideAllFilters()
    End Sub

End Class
