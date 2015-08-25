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

Imports System.ComponentModel

Public Class LINQFilterException
    Inherits Exception
    Public Sub New(ByVal message As String)
        MyBase.New(Message)
    End Sub
End Class

Public Class BindingListView(Of T)
    Implements IBindingList
    Implements IBindingListView
    Implements ICancelAddNew
    Implements AdvancedBindingSource.iFilterChanged
    Private mc_IsSorted As Boolean = False
    Private mc_SortDirection As ListSortDirection
    Private mc_SortProperty As PropertyDescriptor
    Private mc_Filter As String
#Region "Binding Source"

    Dim mc_BindingSource As AdvancedBindingSource
    Public Function BindingSource() As AdvancedBindingSource
        If mc_BindingSource Is Nothing Then
            mc_BindingSource = New AdvancedBindingSource(GetType(T))
            mc_BindingSource.DataSource = Me
        End If
        Return mc_BindingSource
    End Function
#End Region

#Region "Filtering"

    Dim Compiler As New ScriptCompiler
    Public Sub New()
        Compiler.AddDefaultReferences()
    End Sub

    Private Sub ApplyFilter(ByVal filter As String)
        Dim prams As New List(Of ScriptCompiler.EvalPram)
        Dim pram = New ScriptCompiler.EvalPram("[BindingList]", FullList, GetType(T))
        pram.TypeName = "IEnumerable(Of " & pram.TypeName & ")"
        prams.Add(pram)
        filter = AddBracketsPrefix(filter, Me.BindingSource.BaseType.Name)
        filter = "From [" & Me.BindingSource.BaseType.Name & "] in [BindingList] Where " & filter
        Dim results = TryCast(Compiler.Eval(filter, prams), IEnumerable(Of T))
        If results IsNot Nothing Then
            'qwertyuiop
            Try
                FillList(results.ToList)
            Catch ex As Exception
                Throw New LINQFilterException(ex.Message)
            End Try
            'Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
        End If
    End Sub

    Private Function AddBracketsPrefix(ByVal text As String, ByVal ItemName As String) As String
        Return System.Text.RegularExpressions.Regex.Replace(text, _
                                                            "(?<!\.)\[(.+?)\]", _
                                                            New System.Text.RegularExpressions.MatchEvaluator(Function(m As System.Text.RegularExpressions.Match) If((Strings.Left(text, m.Index)).Count(Function(c As Char) c = """"c) Mod 2 = 0, If(LCase(m.Value) = "[me]", "[" & ItemName & "]", "[" & ItemName & "]." & m.Value), m.Value)))
    End Function

#End Region

    Public Sub ApplySort(ByVal sorts As System.ComponentModel.ListSortDescriptionCollection) Implements System.ComponentModel.IBindingListView.ApplySort
    End Sub

    Public Event FilterChanged(ByVal sender As Object, ByVal e As EventArgs) Implements AdvancedBindingSource.iFilterChanged.FilterChanged

    Protected Overridable Sub OnFilterChanged()
        RaiseEvent FilterChanged(Me, EventArgs.Empty)
    End Sub

    Public Property Filter() As String Implements System.ComponentModel.IBindingListView.Filter
        Get
            Return mc_Filter
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(Trim(value)) Then
                RemoveFilter()
            Else
                ApplyFilter(value)
            End If
            mc_Filter = value
            OnFilterChanged()
        End Set
    End Property

    Public Sub RemoveFilter() Implements System.ComponentModel.IBindingListView.RemoveFilter
        FillList(FullList)
    End Sub

    Public ReadOnly Property SortDescriptions() As System.ComponentModel.ListSortDescriptionCollection Implements System.ComponentModel.IBindingListView.SortDescriptions
        Get
            Return Nothing
        End Get
    End Property

    Public ReadOnly Property SupportsAdvancedSorting() As Boolean Implements System.ComponentModel.IBindingListView.SupportsAdvancedSorting
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property SupportsFiltering() As Boolean Implements System.ComponentModel.IBindingListView.SupportsFiltering
        Get
            Return True
        End Get
    End Property

    Private Sub FillList(ByVal lst As List(Of T))
        For z As Integer = FilteredList.Count - 1 To 0 Step -1
            FilteredList.RemoveAt(z)
        Next
        For Each i As T In lst
            FilteredList.Add(i)
        Next
        Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
    End Sub

    Public FilteredList As New List(Of T)
    Public FullList As New List(Of T)

    Protected Overridable Sub OnListChanged(ByVal ev As ListChangedEventArgs)
        RaiseEvent ListChanged(Me, ev)
    End Sub

    ReadOnly Property AllowEdit() As Boolean Implements IBindingList.AllowEdit
        Get
            Return True
        End Get
    End Property

    ReadOnly Property AllowNew() As Boolean Implements IBindingList.AllowNew
        Get
            Return True
        End Get
    End Property

    ReadOnly Property AllowRemove() As Boolean Implements IBindingList.AllowRemove
        Get
            Return True
        End Get
    End Property

    ReadOnly Property SupportsChangeNotification() As Boolean Implements IBindingList.SupportsChangeNotification
        Get
            Return True
        End Get
    End Property

    ReadOnly Property SupportsSearching() As Boolean Implements IBindingList.SupportsSearching
        Get
            Return False
        End Get
    End Property

    ReadOnly Property SupportsSorting() As Boolean Implements IBindingList.SupportsSorting
        Get
            Return True
        End Get
    End Property

    Public Event ListChanged As ListChangedEventHandler Implements IBindingList.ListChanged

    Public Sub AddRange(ByVal value As IEnumerable(Of T))
        'For Each theItem In value
        '    FilteredList.Add(theItem)
        '    FullList.Add(theItem)
        '    OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.ItemAdded, FilteredList.IndexOf(theItem)))
        'Next

        'qwertyuiop - hrm ... had to set it to a variable 1st otherwise it would "clone" the items so that the FilteredList item was not the same object in FullList???! WTF??
        Dim AddValue = value.ToArray
        FilteredList.AddRange(AddValue)
        FullList.AddRange(AddValue)
        Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
    End Sub

    ReadOnly Property IsSorted() As Boolean Implements IBindingList.IsSorted
        Get
            Return mc_IsSorted
        End Get
    End Property

    ReadOnly Property SortDirection() As ListSortDirection Implements IBindingList.SortDirection
        Get
            Return mc_SortDirection
        End Get
    End Property

    ReadOnly Property SortProperty() As PropertyDescriptor Implements IBindingList.SortProperty
        Get
            Return mc_SortProperty
        End Get
    End Property

    Sub AddIndex(ByVal prop As PropertyDescriptor) Implements IBindingList.AddIndex
        Throw New NotSupportedException()
    End Sub

    Public Sub ApplySort(ByVal prop As PropertyDescriptor, ByVal direction As ListSortDirection) Implements IBindingList.ApplySort
        Dim myitems As List(Of T) = TryCast(FullList, List(Of T))
        If myitems IsNot Nothing Then
            mc_SortDirection = direction
            mc_SortProperty = prop
            Dim pc As PropertyComparer(Of T) = _
              New PropertyComparer(Of T)(prop, direction)
            myitems.Sort(pc)
            mc_IsSorted = True
        Else
            mc_IsSorted = False
        End If
        If mc_Filter = "" Then
            RemoveFilter()
        Else
            ApplyFilter(Me.Filter)
        End If
        Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
    End Sub

    Function Find(ByVal prop As PropertyDescriptor, ByVal key As Object) As Integer Implements IBindingList.Find
        Throw New NotSupportedException()
    End Function

    Sub RemoveIndex(ByVal prop As PropertyDescriptor) Implements IBindingList.RemoveIndex
        Throw New NotSupportedException()
    End Sub

    Sub RemoveSort() Implements IBindingList.RemoveSort
        mc_IsSorted = False
    End Sub

    Public Sub CopyTo(ByVal array As System.Array, ByVal index As Integer) Implements System.Collections.ICollection.CopyTo
        FilteredList.CopyTo(array.OfType(Of T).ToArray, index)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.ICollection.Count
        Get
            Return FilteredList.Count
        End Get
    End Property

    Public ReadOnly Property IsSynchronized() As Boolean Implements System.Collections.ICollection.IsSynchronized
        Get
        End Get
    End Property

    Public ReadOnly Property SyncRoot() As Object Implements System.Collections.ICollection.SyncRoot
        Get
            Return Nothing
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return FilteredList.GetEnumerator
    End Function

    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Function AddObject(ByVal value As Object) As Integer Implements System.Collections.IList.Add
        Dim AddItem = DirectCast(value, T)
        FilteredList.Add(AddItem)
        FullList.Add(AddItem)
        OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.ItemAdded, FilteredList.IndexOf(AddItem)))
        Return FilteredList.IndexOf(AddItem)
    End Function
    Public Overloads Function Add(ByVal value As T) As Integer
        Return AddObject(value)
    End Function

    Public Sub Clear() Implements System.Collections.IList.Clear
        For Each theItem In FilteredList
            FullList.Remove(theItem)
        Next
        FilteredList.Clear()
        OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.Reset, -1))
    End Sub

    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)> _
    Public Function ContainsObject(ByVal value As Object) As Boolean Implements System.Collections.IList.Contains
        Return FilteredList.Contains(DirectCast(value, T))
    End Function
    Public Function Contains(ByVal value As T) As Boolean
        Return ContainsObject(value)
    End Function

    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)> _
    Public Function IndexOfObject(ByVal value As Object) As Integer Implements System.Collections.IList.IndexOf
        Return FilteredList.IndexOf(DirectCast(value, T))
    End Function
    Public Function IndexOf(ByVal value As T) As Integer
        Return IndexOfObject(value)
    End Function

    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)> _
    Public Sub InsertObject(ByVal index As Integer, ByVal value As Object) Implements System.Collections.IList.Insert
        FilteredList.Insert(index, DirectCast(value, T))
        Dim InsertAfterItem = (From xItem In FilteredList Where FilteredList.IndexOf(xItem) <= index Order By index Descending).FirstOrDefault
        If InsertAfterItem IsNot Nothing Then
            FullList.Insert(FullList.IndexOf(InsertAfterItem), DirectCast(value, T))
        Else
            FullList.Add(DirectCast(value, T))
        End If
        OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.ItemAdded, index))
    End Sub
    Public Sub Insert(ByVal index As Integer, ByVal value As T)
        InsertObject(index, value)
    End Sub

    Public ReadOnly Property IsFixedSize() As Boolean Implements System.Collections.IList.IsFixedSize
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.IList.IsReadOnly
        Get
            Return False
        End Get
    End Property

    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)> _
    Public Overloads Property ItemObject(ByVal index As Integer) As Object Implements System.Collections.IList.Item
        Get
            Return FilteredList(index)
        End Get
        Set(ByVal value As Object)
            FilteredList(index) = DirectCast(value, T)
        End Set
    End Property
    Default Public Property Item(ByVal Index As Integer) As T
        Get
            Return (DirectCast(Me.ItemObject(Index), T))
        End Get
        Set(ByVal value As T)
            ItemObject(Index) = value
        End Set
    End Property

    <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)> _
    Public Sub RemoveObject(ByVal value As Object) Implements System.Collections.IList.Remove
        Dim RemoveItem = DirectCast(value, T)
        If FilteredList.Contains(RemoveItem) Then
            Dim index = FilteredList.IndexOf(RemoveItem)
            FilteredList.Remove(RemoveItem)
            FullList.Remove(RemoveItem)
            OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.ItemDeleted, index))
        End If
    End Sub
    Public Sub Remove(ByVal value As T)
        RemoveObject(value)
    End Sub

    Public Sub RemoveAt(ByVal index As Integer) Implements System.Collections.IList.RemoveAt
        FullList.Remove(FilteredList(index))
        FilteredList.RemoveAt(index)
        OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.ItemDeleted, index))
    End Sub

    Function AddNew() As Object Implements IBindingList.AddNew
        Dim c = DirectCast(System.Activator.CreateInstance(GetType(T)), T)
        FilteredList.Add(c)
        FullList.Add(c)
        addNewPos = FilteredList.IndexOf(c)
        OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.ItemAdded, addNewPos))
        Return c
    End Function

    'Discards a pending new item from the collection.
    Public Sub CancelNew(ByVal itemIndex As Integer) Implements System.ComponentModel.ICancelAddNew.CancelNew
        If ((Me.addNewPos >= 0) AndAlso (Me.addNewPos = itemIndex)) Then
            FullList.Remove(FilteredList(itemIndex))
            FilteredList.RemoveAt(itemIndex)
            OnListChanged(New System.ComponentModel.ListChangedEventArgs(ListChangedType.ItemDeleted, itemIndex))
            Me.addNewPos = -1
        End If
    End Sub

    Dim addNewPos As Integer = -1

    'Commits a pending new item to the collection.
    Public Sub EndNew(ByVal itemIndex As Integer) Implements System.ComponentModel.ICancelAddNew.EndNew
        If ((Me.addNewPos >= 0) AndAlso (Me.addNewPos = itemIndex)) Then
            Me.addNewPos = -1
        End If
    End Sub
End Class
