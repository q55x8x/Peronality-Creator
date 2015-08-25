'i00 .Net Spell Check
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

Partial Public Class Dictionary

#Region "UI Editor"

    Public Class dicFile_UITypeEditor
        Inherits System.Drawing.Design.UITypeEditor

        Public Overloads Overrides Function EditValue(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            Dim Dict = TryCast(value, Dictionary)
            Return (Dict.ShowUIEditor())
        End Function

        Public Overloads Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As System.Drawing.Design.UITypeEditorEditStyle
            Return System.Drawing.Design.UITypeEditorEditStyle.Modal
        End Function
    End Class

#End Region

    Public Overridable Function ShowUIEditor() As Dictionary
        Using ofd As New OpenFileDialog()
            ofd.FileName = Me.Filename
            ofd.Filter = "Dictionary Files (" & Me.DicFileFilter & ")|" & Me.DicFileFilter & "|All Files (*.*)|*.*"
            ofd.FilterIndex = 0
            If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim ReturnObj = Me
                Try
                    'we need to create a new instance of the class for the property changed event to be fired :(...
                    ReturnObj = DirectCast(System.Activator.CreateInstance(Me.GetType), Dictionary)
                Catch ex As Exception
                    'failed ... maybe the class requires constructor prams?
                    '... oh well just load the new dictionary ... interface may not redraw for existing words

                End Try
                ReturnObj.LoadFromFile(ofd.FileName)
                Return ReturnObj
            Else
                Return Me 'no changes
            End If
        End Using
    End Function


End Class
