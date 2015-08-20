'i00 .Net Control Extensions - OSControlRenderer
'©i00 Productions All rights reserved
'Created by Kris Bennett
'----------------------------------------------------------------------------------------------------
'All property in this file is and remains the property of i00 Productions, regardless of its usage,
'unless stated otherwise in writing from i00 Productions.
'
'Anyone wishing to use this code in their projects may do so, however are required to leave a post on
'VBForums (under: http://www.vbforums.com/showthread.php?p=4075093) stating that they are doing so.
'A simple "I am using i00 Spell check in my project" will surffice.
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Imports i00SpellCheck

Public Class OSControlRenderer
    Inherits ControlExtension
    Implements iTestHarness

#Region "Plugin Info"

    Public Overrides ReadOnly Property ControlTypes() As IEnumerable(Of System.Type)
        Get
            Return New System.Type() {GetType(ListView), GetType(TreeView)}
        End Get
    End Property

#End Region

#Region "Constructor"

    Public Overrides Sub Load()
        Enabled = True
    End Sub

#End Region

#Region "Properties"

    Dim mc_Enabled As Boolean
    Public Property Enabled() As Boolean
        Get
            Return mc_Enabled
        End Get
        Set(ByVal value As Boolean)
            If value = False Then
                'for the label selection bg, detail column lines, etc
                SetWindowTheme(Control.Handle, "", Nothing)
                'for the selection rectangle
                SendMessage(Control.Handle, &H1000 + 54, IntPtr.Zero, IntPtr.Zero)
            Else
                'for the label selection bg, detail column lines, etc
                SetWindowTheme(Control.Handle, "explorer", Nothing)
                'for the selection rectangle
                SendMessage(Control.Handle, &H1000 + 54, New IntPtr(LVS_EX_DOUBLEBUFFER), New IntPtr(LVS_EX_DOUBLEBUFFER))
            End If
            mc_Enabled = value
        End Set
    End Property

#End Region

#Region "API"

    Const LVS_EX_DOUBLEBUFFER As Integer = &H10000

    <System.Runtime.InteropServices.DllImport("USER32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wp As IntPtr, ByVal lp As IntPtr) As IntPtr
    End Function


    'Imports the UXTheme DLL
    <System.Runtime.InteropServices.DllImport("uxtheme", CharSet:=Runtime.InteropServices.CharSet.Unicode)> _
    Public Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal textSubAppName As [String], ByVal textSubIdList As [String]) As Int32
    End Function

#End Region

#Region "iTestHarness"

    Public Function SetupControl(ByVal Control As System.Windows.Forms.Control) As System.Windows.Forms.Control Implements i00SpellCheck.iTestHarness.SetupControl

        If Control.GetType Is GetType(TreeView) Then
            Dim TreeView = DirectCast(Control, TreeView)
            TreeView.HideSelection = False
            For iNode = 1 To 3
                Dim Node = TreeView.Nodes.Add("Node " & iNode)
                For iSubNode = 1 To 3
                    Node.Nodes.Add("SubNode " & iSubNode)
                Next
            Next
        ElseIf Control.GetType Is GetType(ListView) Then
            Dim ListView = DirectCast(Control, ListView)
            ListView.HideSelection = False
            ListView.FullRowSelect = True
            ListView.View = View.Details
            For i = 1 To 2
                ListView.Columns.Add("Column " & i)
            Next
            For i = 1 To 3
                ListView.Items.Add("List View Item " & i).SubItems.Add("List View SubItem " & i)
            Next
            For Each item In ListView.Columns.OfType(Of ColumnHeader)()
                item.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            Next
        End If

        Return Control
    End Function

#End Region

End Class
