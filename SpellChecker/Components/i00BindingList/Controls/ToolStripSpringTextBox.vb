Public Class ToolStripSpringTextBox
    Inherits ToolStripTextBox
    Implements ToolStripSpringHelper.ToolStripSpringItem

    Public Overrides Function GetPreferredSize(ByVal constrainingSize As Size) As Size
        Return ToolStripSpringHelper.GetPreferredSize(Me, constrainingSize, DefaultSize, AddressOf MyBase.GetPreferredSize)
    End Function

End Class