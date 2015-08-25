'i00 Plugins Module
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

    Public Class Setup

        Public Class Types(Of T)
            Public Shared Function GetClassesOfType(ByVal a As System.Reflection.Assembly) As List(Of Types(Of T))
                Return (From xItem In a.GetTypes() Where GetType(T).IsAssignableFrom(xItem) AndAlso xItem.IsAbstract = False AndAlso xItem.IsPublic = True Select New Types(Of T) With {.Type = xItem}).ToList
            End Function

            Public Type As Type
            Public Function CreateObject() As T
                Return DirectCast(System.Activator.CreateInstance(Type), T)
            End Function
        End Class

        Private Shared mc_FilterPlugins As List(Of iFilterPlugin)
        Public Shared ReadOnly Property FilterPlugins() As List(Of iFilterPlugin)
            Get
                If mc_FilterPlugins Is Nothing Then
                    mc_FilterPlugins = New List(Of iFilterPlugin)
                    For Each file In FileIO.FileSystem.GetFiles(IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location()), FileIO.SearchOption.SearchTopLevelOnly, "*.dll", "*.exe")
                        Try
                            Dim a = System.Reflection.Assembly.LoadFile(file)

                            Dim Classes = Types(Of iFilterPlugin).GetClassesOfType(a)
                            For Each item In Classes
                                Try
                                    mc_FilterPlugins.Add(item.CreateObject)
                                Catch ex As Exception

                                End Try
                            Next
                        Catch ex As Exception

                        End Try
                    Next
                End If
                Return mc_FilterPlugins
            End Get
        End Property

    End Class
End Class
