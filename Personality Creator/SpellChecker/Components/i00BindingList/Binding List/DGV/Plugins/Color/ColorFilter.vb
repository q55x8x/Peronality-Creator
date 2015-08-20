'i00 BindingList with DataGridView
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

Public Class ColorFilter
    Implements i00BindingList.Plugins.iFilterPlugin

    Private WithEvents ColorFilter As New tsiColorPicker With {.Persistent = True}

    Private WithEvents tsiShowFullColorList As New i00BindingList.PersistentToolStripMenuItem With {.Text = "Show All Known Colors"}

    Private WithEvents tsiIncludeSimilarColors As New i00BindingList.PersistentToolStripMenuItem With {.Text = "Include Similar Colors", .Checked = True}

    Public Class BasicColorFilter
        Inherits AdvancedBindingSource.BasicFilterBase
        Public Color As Color
        Public IncludeSimilarColors As Boolean = True

        Public Overrides ReadOnly Property GetLinq() As String
            Get
                If IncludeSimilarColors Then
                    Return GetType(tsiColorPicker).FullName & ".GetClosestKnownColor([" & Field & "]).ToArgb = " & tsiColorPicker.GetClosestKnownColor(Me.Color).ToArgb
                Else
                    Return "[" & Field & "].ToArgb = " & Me.Color.ToArgb
                End If
            End Get
        End Property
    End Class

    Public ReadOnly Property DataTypes() As System.Type() Implements i00BindingList.Plugins.iFilterPlugin.DataTypes
        Get
            Return New System.Type() {GetType(Color)}
        End Get
    End Property

    Public ReadOnly Property Dispaly() As i00BindingList.Plugins.DisplayMethods Implements i00BindingList.Plugins.iFilterPlugin.Dispaly
        Get
            Return i00BindingList.Plugins.DisplayMethods.DefaultShow
        End Get
    End Property

    Public Function MenuItems() As System.Collections.Generic.List(Of System.Windows.Forms.ToolStripItem) Implements i00BindingList.Plugins.iFilterPlugin.MenuItems
        MenuItems = New List(Of ToolStripItem)
        MenuItems.Add(ColorFilter)
        MenuItems.Add(tsiIncludeSimilarColors)
        MenuItems.Add(tsiShowFullColorList)
    End Function

    Public Event UpdateFilter(ByVal sender As Object, ByVal e As i00BindingList.Plugins.UpdateFilterPluginEventArgs) Implements i00BindingList.Plugins.iFilterPlugin.UpdateFilter

    Private Sub ColorFilter_ColorChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ColorFilter.ColorChanged
        If ColorFilter.SelectedColor.IsEmpty Then Exit Sub
        Dim FilterPlugin = New BasicColorFilter()
        FilterPlugin.Color = ColorFilter.SelectedColor
        FilterPlugin.IncludeSimilarColors = tsiIncludeSimilarColors.Checked
        RaiseEvent UpdateFilter(Me, New i00BindingList.Plugins.UpdateFilterPluginEventArgs() With {.FilterBase = FilterPlugin})
    End Sub

    Private Sub tsiIncludeSimilarColors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiIncludeSimilarColors.Click
        tsiIncludeSimilarColors.Checked = Not tsiIncludeSimilarColors.Checked
        ColorFilter_ColorChanged(ColorFilter, EventArgs.Empty)
    End Sub

    Public Sub LoadFromFilter(ByVal Filter As i00BindingList.AdvancedBindingSource.BasicFilterBase) Implements i00BindingList.Plugins.iFilterPlugin.LoadFromFilter
        Dim BasicColorFilter = TryCast(Filter, BasicColorFilter)
        If BasicColorFilter IsNot Nothing Then
            ColorFilter.SelectedColor = BasicColorFilter.Color
            tsiIncludeSimilarColors.Checked = BasicColorFilter.IncludeSimilarColors
        Else
            'tsiIncludeSimilarColors.Checked = True
            ColorFilter.SelectedColor = Nothing
        End If
        ColorFilter.Invalidate()
    End Sub

    Private Sub tsiShowFullColorList_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tsiShowFullColorList.Click
        tsiShowFullColorList.Checked = Not tsiShowFullColorList.Checked
        If tsiShowFullColorList.Checked Then
            'To add all known colors
            ColorFilter.Colors.Clear()
            Dim c As New List(Of Color)
            For Each item In [Enum].GetValues(GetType(KnownColor))
                Dim TheColor = Color.FromKnownColor(DirectCast([Enum].Parse(GetType(KnownColor), item.ToString), KnownColor))
                If TheColor.IsSystemColor = False AndAlso TheColor <> Color.Transparent Then
                    c.Add(TheColor)
                End If
            Next
            'order by lightness
            ColorFilter.Colors.AddRange(From xItem In c Order By CInt(xItem.R) + CInt(xItem.G) + CInt(xItem.B), xItem.ToArgb)

            ColorFilter.Invalidate()
            tmrResize.Enabled = True
        Else
            'default
            ColorFilter.Colors.Clear()
            ColorFilter.FillWithDefaultColors()
            ColorFilter.Invalidate()

            tmrResize.Enabled = True
        End If
    End Sub

    'this timer is used to resize the tsiColorPicker
    'this is due to an issue that when clicking on a percistant menu item,
    'if the menu is no longer under the menu after the click, the menu
    'will close
    Private WithEvents tmrResize As New Timer With {.Interval = 1}

    Private Sub tmrResize_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrResize.Tick
        tmrResize.Enabled = False
        ColorFilter.Resize()
    End Sub

End Class
