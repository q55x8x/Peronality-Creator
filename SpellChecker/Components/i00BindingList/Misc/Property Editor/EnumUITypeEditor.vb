'i00 EnumUITypeEditor
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

Friend Class EnumUITypeEditor
    Inherits System.Drawing.Design.UITypeEditor

    Private WithEvents PropertyEditorListView As PropertyEditorListView

    Dim editor As System.Drawing.Design.UITypeEditor

    Public Sub New(ByVal editor As System.Drawing.Design.UITypeEditor)
        Me.editor = editor
    End Sub

    Public ListOverride As List(Of Object)

    Public Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As System.IServiceProvider, ByVal value As Object) As Object
        If TypeOf value Is [Enum] Then

            Dim edSvc As System.Windows.Forms.Design.IWindowsFormsEditorService = DirectCast(provider.GetService(GetType(System.Windows.Forms.Design.IWindowsFormsEditorService)), System.Windows.Forms.Design.IWindowsFormsEditorService)
            If edSvc IsNot Nothing Then
                PropertyEditorListView = New PropertyEditorListView(edSvc, editor)

                PropertyEditorListView.EnumValue = DirectCast(value, [Enum])
                edSvc.DropDownControl(PropertyEditorListView)

                Return PropertyEditorListView.EnumValue
            End If
        ElseIf ListOverride IsNot Nothing Then
            Dim edSvc As System.Windows.Forms.Design.IWindowsFormsEditorService = DirectCast(provider.GetService(GetType(System.Windows.Forms.Design.IWindowsFormsEditorService)), System.Windows.Forms.Design.IWindowsFormsEditorService)
            If edSvc IsNot Nothing Then
                PropertyEditorListView = New PropertyEditorListView(edSvc, editor)

                PropertyEditorListView.Items.AddRange(ListOverride.ToArray)
                edSvc.DropDownControl(PropertyEditorListView)

                Return PropertyEditorListView.EnumValue
            End If
        End If
        Return Nothing
        'Return MyBase.EditValue(context, provider, value)
    End Function

    Public Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
        Return Drawing.Design.UITypeEditorEditStyle.DropDown
    End Function

    Public Overrides ReadOnly Property IsDropDownResizable() As Boolean
        Get
            Return True
        End Get
    End Property

End Class
