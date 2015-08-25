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

Partial Public Class Plugins

    Public Enum DisplayMethods
        DefaultHide
        Always
        DefaultShow
    End Enum

    Public Class UpdateFilterPluginEventArgs
        Inherits EventArgs
        Public FilterBase As AdvancedBindingSource.BasicFilterBase
    End Class

    Public Interface iFilterPlugin
        ReadOnly Property DataTypes() As Type()
        ReadOnly Property Dispaly() As DisplayMethods

        Function MenuItems() As List(Of ToolStripItem)

        Event UpdateFilter(ByVal sender As Object, ByVal e As UpdateFilterPluginEventArgs)

        Sub LoadFromFilter(ByVal Filter As AdvancedBindingSource.BasicFilterBase)
    End Interface

    <AttributeUsage(AttributeTargets.Property Or AttributeTargets.Field)> _
    Public Class ActiveFilterPluginsAttribute
        Inherits System.Attribute

        Dim mc_ActivePlugins As List(Of Type)
        Public ReadOnly Property ActivePlugins() As List(Of Type)
            Get
                Return mc_ActivePlugins
            End Get
        End Property


        Public Sub New(ByVal ParamArray Plugins() As Type)
            ''validation
            mc_ActivePlugins = Plugins.ToList
        End Sub
    End Class

End Class
