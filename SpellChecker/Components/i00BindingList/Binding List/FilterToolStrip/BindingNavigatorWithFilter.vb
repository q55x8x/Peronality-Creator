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

<System.ComponentModel.DesignerCategory("")> _
Public Class BindingNavigatorWithFilter
    Inherits BindingNavigator
    Public Sub New()
        Me.AddStandardItems()
    End Sub

#Region "Controls"

    Private Interface iFilterControl

    End Interface

    Public Class tsiFilterHeader
        Inherits ToolStripLabel
        Implements iFilterControl

        Public Sub New()
            Me.Text = "Filter:"
        End Sub
    End Class

    Public Class tsiFilterText
        Inherits ToolStripSpringTextBox
        Implements iFilterControl

        Public Sub New()

        End Sub
    End Class

    Public Class tsiRunFilter
        Inherits ToolStripButton
        Implements iFilterControl

        Public Sub New()
            Me.Text = "Run Filter"
            Me.DisplayStyle = ToolStripItemDisplayStyle.Image
            Me.Image = My.Resources.RunFilter
        End Sub
    End Class

    Public Class tsiClearFilter
        Inherits ToolStripButton
        Implements iFilterControl

        Public Sub New()
            Me.Text = "Clear Filter"
            Me.DisplayStyle = ToolStripItemDisplayStyle.Image
            Me.Image = My.Resources.ClearFilter
        End Sub
    End Class

#End Region

    Public Overrides Sub AddStandardItems()
        MyBase.AddStandardItems()

        Me.Items.Add(New ToolStripSeparator)

        Dim tsiFilterHeader As New tsiFilterHeader
        Me.Items.Add(tsiFilterHeader)

        Dim tsiFilterTextBox = New tsiFilterText
        Me.Items.Add(tsiFilterTextBox)

        Dim tsiRunFilter As New tsiRunFilter
        Me.Items.Add(tsiRunFilter)
        AddHandler tsiRunFilter.Click, AddressOf tsiRunFilter_Click

        Dim tsiClearFilter As New tsiClearFilter
        Me.Items.Add(tsiClearFilter)
        AddHandler tsiClearFilter.Click, AddressOf tsiClearFilter_Click

    End Sub


    Private Sub tsiClearFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If MyBase.BindingSource IsNot Nothing Then MyBase.BindingSource.Filter = ""
    End Sub


    Private Sub tsiRunFilter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'get the filter to the left... and run it...
        Dim FirstItemToLeft = (From xItem In Me.Items.OfType(Of tsiFilterText)() Where Me.Items.IndexOf(xItem) < Me.Items.IndexOf(DirectCast(sender, ToolStripItem))).LastOrDefault
        If FirstItemToLeft IsNot Nothing Then
            If MyBase.BindingSource IsNot Nothing Then
                Dim ErrorMessage = ""
                Try
                    MyBase.BindingSource.Filter = FirstItemToLeft.Text
                Catch ex As ScriptCompiler.ScripCompilerException
                    ttFilterErrors.ToolTipTitle = "Syntax Error:"
                    ErrorMessage = Join((From xItem In ex.errors Select xItem.ErrorText).ToArray, vbCrLf)
                Catch ex As LINQFilterException
                    ttFilterErrors.ToolTipTitle = "Error filtering data from linq statement:"
                    ErrorMessage = ex.Message
                Catch ex As Exception
                    ttFilterErrors.ToolTipTitle = "Error running filter:"
                    ErrorMessage = ex.Message
                End Try
                If ErrorMessage <> "" Then
                    ttFilterErrors.Show(ErrorMessage, Me, FirstItemToLeft.Bounds.Left, FirstItemToLeft.Bounds.Bottom, 4000)
                End If
            End If
        End If
    End Sub

    Dim ttFilterErrors As New ToolTip With {.ToolTipIcon = ToolTipIcon.Error}

    Private Property FilterControlsEnabled() As Boolean
        Get
            Dim FirstItem = (From xItem In Me.Items.OfType(Of tsiFilterHeader)()).FirstOrDefault
            If FirstItem IsNot Nothing Then
                Return FirstItem.Enabled
            End If
        End Get
        Set(ByVal value As Boolean)
            For Each item In (From xItem In Me.Items.OfType(Of ToolStripItem)() Where TypeOf xItem Is iFilterControl).ToArray
                'see if we enable the remove filters...
                If value = True AndAlso TypeOf item Is tsiClearFilter AndAlso MyBase.BindingSource IsNot Nothing AndAlso MyBase.BindingSource.Filter = "" Then
                    item.Enabled = False
                Else
                    item.Enabled = value
                End If
            Next
        End Set
    End Property

    Public Overloads Property BindingSource() As BindingSource
        Get
            Return MyBase.BindingSource
        End Get
        Set(ByVal value As BindingSource)
            'Dim AdvBindingSource = TryCast(value, i00BindingList.AdvancedBindingSource)
            Dim iFilterChanged = TryCast(MyBase.BindingSource, AdvancedBindingSource.iFilterChanged)
            If iFilterChanged IsNot Nothing Then
                RemoveHandler iFilterChanged.FilterChanged, AddressOf BindingSource_FilterChanged
            End If
            MyBase.BindingSource = value
            iFilterChanged = TryCast(MyBase.BindingSource, AdvancedBindingSource.iFilterChanged)
            If iFilterChanged IsNot Nothing Then
                AddHandler iFilterChanged.FilterChanged, AddressOf BindingSource_FilterChanged
            End If
            If MyBase.BindingSource.SupportsFiltering Then
                'use the filter :D
                FilterControlsEnabled = True
            Else
                FilterControlsEnabled = False
            End If
        End Set
    End Property

    Private Sub BindingSource_FilterChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        If MyBase.BindingSource Is Nothing Then Exit Sub
        Dim AllFilterTextFields = (From xItem In Me.Items.OfType(Of tsiFilterText)()).ToArray
        For Each item In AllFilterTextFields
            item.Text = MyBase.BindingSource.Filter
        Next
        For Each item In (From xItem In Me.Items.OfType(Of tsiClearFilter)()).ToArray
            If MyBase.BindingSource IsNot Nothing Then
                If MyBase.BindingSource.Filter = "" Then
                    item.Enabled = False
                Else
                    item.Enabled = True
                End If
            End If
        Next
    End Sub

End Class
