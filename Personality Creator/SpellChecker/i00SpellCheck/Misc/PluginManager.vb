'i00 Plugin Manager
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

Public Class PluginManager(Of T)

    Private Class Types
        Public Shared Function GetClassesOfType(ByVal a As System.Reflection.Assembly) As List(Of Types)
            Return (From xItem In a.GetTypes() Where GetType(T).IsAssignableFrom(xItem) AndAlso xItem.IsAbstract = False AndAlso xItem.IsPublic = True Select New Types With {.Type = xItem}).ToList
        End Function

        Public Type As Type
        Public Function CreateObject() As T
            Return DirectCast(System.Activator.CreateInstance(Type), T)
        End Function
    End Class

    Public Class PluginsWithWeight
        Public PluginClass As T
        Public Weight As Double
        Public Sub New(ByVal PluginClass As T, Optional ByVal Weight As Double = 0)
            Me.PluginClass = PluginClass
            Me.Weight = Weight
        End Sub
    End Class

#If VBC_VER <= 9.0 Then
    Const DebuggerShadowCopyOK As Boolean = False
#Else
    Const DebuggerShadowCopyOK as Boolean =True 
#End If

    Shared ShadowCopyDebuggerOverride As Boolean

    Public Shared ReadOnly Property GetPlugins(Optional ByVal PluginLocation As String = "", Optional ByVal ShadowCopy As Boolean = False) As List(Of T)
        Get
            If PluginLocation = "" Then
                PluginLocation = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location())
            End If

            Dim PluginsWithWeight = New List(Of PluginsWithWeight)

            For Each file In FileIO.FileSystem.GetFiles(PluginLocation, FileIO.SearchOption.SearchTopLevelOnly, "*.dll", "*.exe")
                Try
                    Dim a As System.Reflection.Assembly


                    If DebuggerShadowCopyOK = False AndAlso ShadowCopy AndAlso Debugger.IsAttached Then
                        'debugger issues with vs<=9.0
                        Static AskedToShadowCopy As Boolean = False
                        If AskedToShadowCopy = False Then
                            AskedToShadowCopy = True
                            If MsgBox("The VS9 or prior debugger is attached, this debugger is not fully compatable with loading shadow copy plugins." & vbCrLf & vbCrLf & "Plugins will still work, but any breakpoints located within the calling function, within its call stack, or within plugins will not be able to locate the line for the break point." & vbCrLf & vbCrLf & "This will not effect the deployed product." & vbCrLf & vbCrLf & "Do you wish to disable shadow copying for plugins in this session?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                                'enable shadow copy
                                ShadowCopyDebuggerOverride = True
                            End If
                        End If

                        ShadowCopy = ShadowCopyDebuggerOverride
                    End If

                    If ShadowCopy Then
                        a = System.Reflection.Assembly.Load(My.Computer.FileSystem.ReadAllBytes(file))
                    Else
                        a = System.Reflection.Assembly.LoadFile(file)
                    End If
                    PluginsWithWeight.AddRange(GetPluginsFromAssembly(a))
                Catch ex As Exception

                End Try
            Next


            Return (From xItem In PluginsWithWeight Order By xItem.Weight Descending Select xItem.PluginClass).ToList

        End Get
    End Property

    Public Shared ReadOnly Property GetPluginsFromAssembly(ByVal a As System.Reflection.Assembly) As List(Of PluginsWithWeight)
        Get
            GetPluginsFromAssembly = New List(Of PluginsWithWeight)
            Dim Classes = Types.GetClassesOfType(a)
            For Each item In Classes
                Dim weight = 0.0
                Dim PluginWeightAttribute = item.Type.GetCustomAttributes(False).OfType(Of PluginWeightAttribute)().FirstOrDefault
                If PluginWeightAttribute IsNot Nothing Then
                    weight = PluginWeightAttribute.Weight
                End If

                Try
                    GetPluginsFromAssembly.Add(New PluginsWithWeight(item.CreateObject, weight))
                Catch ex As Exception

                End Try
            Next
        End Get
    End Property

    Public Shared Function GetAllPluginsInReferencedAssemblies(ByVal a As System.Reflection.Assembly, ByVal BaseType As Type) As List(Of T)
        GetAllPluginsInReferencedAssemblies = New List(Of T)
        Dim AssembliesToCheck = a.GetReferencedAssemblies.ToList
        AssembliesToCheck.Add(a.GetName)

        For Each item In AssembliesToCheck
            Try
                Dim AssemblyToCheck = System.Reflection.Assembly.Load(item.FullName)

                Dim CreatableObjects = (From xItem In AssemblyToCheck.GetTypes() Where BaseType.IsAssignableFrom(xItem) AndAlso xItem.IsAbstract = False AndAlso xItem.IsPublic = True).ToList
                For Each CreateItem In CreatableObjects
                    Try
                        GetAllPluginsInReferencedAssemblies.Add(DirectCast(System.Activator.CreateInstance(CreateItem), T))
                    Catch ex As Exception

                    End Try
                Next
            Catch ex As Exception

            End Try
        Next
    End Function

End Class

<AttributeUsage(AttributeTargets.Class)> _
    Public Class PluginWeightAttribute
    Inherits System.Attribute

    Dim mc_Weight As Double
    Public ReadOnly Property Weight() As Double
        Get
            Return mc_Weight
        End Get
    End Property

    Public Sub New(ByVal Weight As Double)
        mc_Weight = Weight
    End Sub
End Class