'i00 .Net ToolStripSpringHelper
'©i00 Productions All rights reserved
'This article is derived from http://msdn.microsoft.com/en-us/library/ms404304.aspx
'----------------------------------------------------------------------------------------------------
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Public Class ToolStripSpringHelper

    Friend Interface ToolStripSpringItem

    End Interface

    Public Shared Function GetPreferredSize(ByVal Control As ToolStripItem, ByVal constrainingSize As Size, ByVal DefaultSize As Size, ByVal BaseGetPreferedSize As Func(Of Size, Size)) As Size
        ' Use the default size if the text box is on the overflow menu
        ' or is on a vertical ToolStrip.
        If Control.IsOnOverflow Or Control.Owner.Orientation = Orientation.Vertical Then
            Return DefaultSize
        End If

        ' Declare a variable to store the total available width as 
        ' it is calculated, starting with the display width of the 
        ' owning ToolStrip.
        Dim width As Int32 = Control.Owner.DisplayRectangle.Width

        ' Subtract the width of the overflow button if it is displayed. 
        If Control.Owner.OverflowButton.Visible Then
            width = width - Control.Owner.OverflowButton.Width - _
                Control.Owner.OverflowButton.Margin.Horizontal()
        End If

        ' Declare a variable to maintain a count of ToolStripSpringTextBox 
        ' items currently displayed in the owning ToolStrip. 
        Dim springBoxCount As Int32 = 0

        For Each item As ToolStripItem In Control.Owner.Items
            ' Ignore items on the overflow menu.
            If item.IsOnOverflow Then Continue For

            If TypeOf (item) Is ToolStripSpringItem Then
                ' For ToolStripSpringTextBox items, increment the count and 
                ' subtract the margin width from the total available width.
                springBoxCount += 1
                width -= item.Margin.Horizontal
            Else
                ' For all other items, subtract the full width from the total
                ' available width.
                width = width - item.Width - item.Margin.Horizontal
            End If
        Next

        ' If there are multiple ToolStripSpringTextBox items in the owning
        ' ToolStrip, divide the total available width between them. 
        If springBoxCount > 1 Then width = CInt(width / springBoxCount)

        ' If the available width is less than the default width, use the
        ' default width, forcing one or more items onto the overflow menu.
        If width < DefaultSize.Width Then width = DefaultSize.Width

        ' Retrieve the preferred size from the base class, but change the
        ' width to the calculated width. 
        Dim preferredSize As Size = BaseGetPreferedSize(constrainingSize)
        preferredSize.Width = width
        Return preferredSize
    End Function
End Class
