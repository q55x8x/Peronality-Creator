<System.ComponentModel.DesignerCategory("")> _
Friend Class ExtendedListView
    Inherits ListView
    Public Event Scroll(ByVal sebder As Object, ByVal e As ScrollEventArgs)
    Protected Overridable Sub OnScroll(ByVal e As ScrollEventArgs)
        RaiseEvent Scroll(Me, e)
    End Sub

    Const WM_VSCROLL As Integer = &H115
    Const WM_HSCROLL As Integer = &H114
    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)
        Select Case m.Msg
            Case WM_HSCROLL, WM_VSCROLL
                OnScroll(New ScrollEventArgs(CType(m.WParam.ToInt32() And &HFFFF, ScrollEventType), 0))
        End Select
    End Sub
End Class