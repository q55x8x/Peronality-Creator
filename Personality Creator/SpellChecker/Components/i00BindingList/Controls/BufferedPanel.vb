<System.ComponentModel.DesignerCategory("")> _
Public Class BufferedPanel
    Inherits Panel

    Public Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.DoubleBuffer Or _
                 ControlStyles.ResizeRedraw Or _
                 ControlStyles.UserPaint, _
                 True)
    End Sub
End Class