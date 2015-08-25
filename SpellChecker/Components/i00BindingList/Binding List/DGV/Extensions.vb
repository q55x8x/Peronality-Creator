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

Module Extensions

    <System.Runtime.CompilerServices.Extension()> _
    Friend Function GetFilter(ByVal DataGridView As DataGridView) As String
        If DataGridView.DataSource IsNot Nothing Then
            Dim bs = TryCast(DataGridView.DataSource, BindingSource)
            If bs IsNot Nothing Then
                Return bs.Filter
            End If
        End If
        Return Nothing
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Friend Sub SetFilter(ByVal DataGridView As DataGridView, ByVal value As String)
        If DataGridView.DataSource IsNot Nothing Then
            Dim bs = TryCast(DataGridView.DataSource, BindingSource)
            If bs IsNot Nothing Then
                bs.Filter = value
            End If
        End If
    End Sub

    Public Enum FilteringMethods
        None
        Basic
        Advanced
    End Enum

    <System.Runtime.CompilerServices.Extension()> _
    Friend Function SupportsFiltering(ByVal DataGridView As DataGridView) As FilteringMethods
        SupportsFiltering = FilteringMethods.None
        If DataGridView.DataSource IsNot Nothing Then
            Dim bs = TryCast(DataGridView.DataSource, BindingSource)
            If bs IsNot Nothing Then
                SupportsFiltering = FilteringMethods.Basic
                bs = TryCast(DataGridView.DataSource, AdvancedBindingSource)
                If bs IsNot Nothing Then
                    SupportsFiltering = FilteringMethods.Advanced
                End If
            End If
        End If
    End Function

End Module
