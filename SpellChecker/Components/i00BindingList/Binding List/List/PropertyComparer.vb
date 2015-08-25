'©i00 Productions All rights reserved
'This article is derived from http://social.msdn.microsoft.com/forums/en-US/winformsdatacontrols/thread/43069cef-9f0a-4854-a5e7-42b701abc75e/
'----------------------------------------------------------------------------------------------------
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Imports System.Reflection
Imports System.ComponentModel

<System.ComponentModel.DesignerCategory("")> _
Public Class PropertyComparer(Of T)
    Inherits System.Collections.Generic.Comparer(Of T)
    ' The following code contains code implemented by Rockford Lhotka:
    ' http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnadvnet/html/vbnet01272004.asp
    ' The following code was then converted into VB from Michael Weinhardt's C# implementation at:
    ' http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnforms/html/winforms02182005.asp
    Private _property As PropertyDescriptor
    Private _direction As ListSortDirection
    Public Sub New(ByVal [property] As PropertyDescriptor, ByVal direction As ListSortDirection)
        _property = [property]
        _direction = direction
    End Sub
#Region "IComparer<T>"
    Public Overrides Function Compare(ByVal xWord As T, ByVal yWord As T) As Integer
        ' Get property values
        Dim xValue As Object = GetPropertyValue(xWord, _property.Name)
        Dim yValue As Object = GetPropertyValue(yWord, _property.Name)
        ' Determine sort order
        If _direction = ListSortDirection.Ascending Then
            Return CompareAscending(xValue, yValue)
        Else
            Return CompareDescending(xValue, yValue)
        End If
    End Function
    Public Overloads Function Equals(ByVal xWord As T, ByVal yWord As T) As Boolean
        Return xWord.Equals(yWord)
    End Function
    Public Overloads Function GetHashCode(ByVal obj As T) As Integer
        Return obj.GetHashCode()
    End Function
#End Region
    ' Compare two property values of any type
    Private Function CompareAscending(ByVal xValue As Object, ByVal yValue As Object) As Integer
        Dim result As Integer
        ' If values implement IComparer
        If TypeOf xValue Is IComparable Then
            result = (DirectCast(xValue, IComparable)).CompareTo(yValue)
        Else
            If xValue Is Nothing Then
                If yValue Is Nothing Then
                    result = 0
                Else
                    result = -1
                End If
            ElseIf xValue.Equals(yValue) Then
                ' If values don't implement IComparer but are equivalent
                result = 0
            Else
                result = xValue.ToString().CompareTo(yValue.ToString())
            End If
            ' Values don't implement IComparer and are not equivalent, so compare as string values
        End If
        ' Return result
        Return result
    End Function
    Private Function CompareDescending(ByVal xValue As Object, ByVal yValue As Object) As Integer
        ' Return result adjusted for ascending or descending sort order ie
        ' multiplied by 1 for ascending or -1 for descending
        Return CompareAscending(xValue, yValue) * -1
    End Function
    Private Function GetPropertyValue(ByVal value As T, ByVal [property] As String) As Object
        ' Get property
        Dim propertyInfo As PropertyInfo = value.[GetType]().GetProperty([property])
        ' Return value
        Return propertyInfo.GetValue(value, Nothing)
    End Function
End Class