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
Public Class AdvancedBindingSource
    Inherits BindingSource
    Implements iFilterChanged

    Public Interface iFilterChanged
        Event FilterChanged(ByVal sender As Object, ByVal e As EventArgs)
    End Interface

    Public BasicFilters As New List(Of BasicFilterBase)

    Public MustInherit Class BasicFilterBase
        Public Field As String
        Public MustOverride ReadOnly Property GetLinq() As String
    End Class

    Public NotInheritable Class BasicFilter
        Inherits BasicFilterBase
        Public Filter As String

        Public Sub New(ByVal Field As String, ByVal Filter As String)
            MyBase.Field = Field
            Me.Filter = Filter
        End Sub

        Public Overrides ReadOnly Property GetLinq() As String
            Get
                Return "IsNothing([" & Field & "]) = False AndAlso LCase(System.ComponentModel.TypeDescriptor.GetConverter([" & Field & "]).ConvertToString([" & Field & "])) Like ""*" & Replace(LCase(Filter), """", """""") & "*"""
            End Get
        End Property
    End Class

    Public Sub RunBasicFilter()

        If BasicFilters.Count = 0 Then
            MyBase.Filter = ""
        Else
            Dim WhereClause = (From xItem In BasicFilters Select "(" & xItem.GetLinq & ")").ToArray
            MyBase.Filter = Join(WhereClause, " AndAlso ")
        End If
        OnFilterChanged()
    End Sub

    Friend BaseType As Type

    Friend Sub New(ByVal BaseType As Type)
        Me.BaseType = BaseType
    End Sub

    Public Overrides Property Filter() As String
        Get
            Return MyBase.Filter
        End Get
        Set(ByVal value As String)
            BasicFilters.Clear()
            MyBase.Filter = value
            OnFilterChanged()
        End Set
    End Property

    Protected Overridable Sub OnFilterChanged()
        RaiseEvent FilterChanged(Me, EventArgs.Empty)
    End Sub

    Public Event FilterChanged(ByVal sender As Object, ByVal e As System.EventArgs) Implements iFilterChanged.FilterChanged
End Class