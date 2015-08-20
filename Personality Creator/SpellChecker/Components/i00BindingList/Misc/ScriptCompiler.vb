'i00 Script Compiler
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

Imports System.CodeDom.Compiler
Imports System.Reflection

Public Class ScriptCompiler

    Public MustInherit Class EvaluatorClass
        MustOverride Sub SetPram(ByVal Name As String, ByVal [Object] As Object)
        MustOverride Function Eval() As Object
    End Class

    Public CompilerResults As CompilerResults

    Public References As New List(Of String)

    Public Class EvalPram
        Public PramName As String
        Public PramData As Object
        Public Type As Type = Nothing
        Public Sub New(ByVal PramName As String, ByVal PramData As Object)
            Me.PramName = PramName
            Me.PramData = PramData
        End Sub
        Public Sub New(ByVal PramName As String, ByVal PramData As Object, ByVal Type As Type)
            Me.PramName = PramName
            Me.PramData = PramData
            Me.Type = Type
        End Sub

        Dim mc_TypeName As String
        Public Property TypeName() As String
            Get
                If mc_TypeName = "" Then
                    mc_TypeName = Type.Namespace & Type.Delimiter
                    Dim SubClasses As New List(Of String)
                    Dim TheType = Type
                    Do Until TheType Is Nothing
                        SubClasses.Add(TheType.Name)
                        TheType = TheType.ReflectedType()
                    Loop
                    SubClasses.Reverse()
                    mc_TypeName &= Join(SubClasses.ToArray, Type.Delimiter)
                End If
                Return mc_TypeName
            End Get
            Set(ByVal value As String)
                mc_TypeName = value
            End Set
        End Property

    End Class

    Public Sub AddDefaultReferences()
        'References.Add("Microsoft.VisualBasic, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a") 'microsoft.visualbasic.dll
        'References.Add("System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") 'system.dll
        'References.Add("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") 'system.core.dll
        'References.Add("System.Xml.Linq, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089") 'system.xml.linq.dll

        'add calling assembly
        References.Add(FileIO.FileSystem.GetFileInfo(Assembly.GetExecutingAssembly.Location).Name)
        'and its references...
        For Each RefItem In Assembly.GetExecutingAssembly.GetReferencedAssemblies()
            References.Add(RefItem.FullName)
        Next

    End Sub


    Public Function Eval(ByVal vbCode As String, Optional ByVal Prams As List(Of EvalPram) = Nothing, Optional ByVal CustomImports As List(Of String) = Nothing) As Object

        If Prams Is Nothing Then Prams = New List(Of EvalPram)
        If InStr(vbCode, vbCrLf) > 0 Then
            'we are more than one line so don't add Return
        Else
            'we are one line so lets see if we start with return
            If InStr(Trim(vbCode), "return", CompareMethod.Text) = 1 Then
                'we start with return so don't add it
            Else
                'add return as it does not return anything
                vbCode = "Return " & Trim(vbCode)
            End If
        End If

        Dim privateType = (From xItem In Prams Where xItem.Type.IsNotPublic = True).FirstOrDefault
        If privateType IsNot Nothing Then
            Throw New Exception(privateType.Type.FullName & " must be public to be an EvalPram")
        End If

        For Each item In Prams
            References.Add(item.Type.Assembly.FullName)
            For Each RefItem In item.Type.Assembly.GetReferencedAssemblies()
                References.Add(RefItem.FullName)
            Next
        Next

        Dim DimStr As String = Join((From xItem In Prams Select "    Dim " & xItem.PramName & If(xItem.Type Is Nothing, " As Object", " As " & xItem.TypeName)).ToArray, vbCrLf)
        'Dim DimStr As String = Join((From xItem In Prams Select "    Dim " & xItem.PramName & " As Object").ToArray, vbCrLf)
        '& vbCrLf & "    Public Sub Set" & xItem.PramName & "(Data" & IIf(xItem.Type Is Nothing, "", " As " & xItem.Type) & ")" & vbCrLf & "        " & xItem.PramName & " = Data" & vbCrLf & "    End Sub"

        Dim PramsSetStr = Join((From xItem In Prams Select "            Case """ & xItem.PramName & """" & vbCrLf & "                " & xItem.PramName & " = [Object]").ToArray, vbCrLf)
        PramsSetStr = "    Public Overrides Sub SetPram(ByVal Name As String, ByVal [Object] As Object)" & vbCrLf & "        Select Case Name" & vbCrLf & PramsSetStr & vbCrLf & "        End Select" & vbCrLf & "    End Sub"

        Dim a = Assembly.GetExecutingAssembly()
        Dim t As Type = GetType(EvaluatorClass)
        Dim Ass = (From xItem In a.GetTypes() Where t.IsAssignableFrom(xItem) AndAlso t.IsAbstract = True).FirstOrDefault

        Dim EvaluatorClassName = Ass.Namespace & Type.Delimiter

        Dim SubClasses As New List(Of String)
        Do Until Ass Is Nothing
            SubClasses.Add(Ass.Name)
            Ass = Ass.ReflectedType()
        Loop
        SubClasses.Reverse()

        EvaluatorClassName &= Join(SubClasses.ToArray, Type.Delimiter)

        Dim ImportsString = ""
        If CustomImports Is Nothing Then
            ImportsString = "Imports Microsoft.VisualBasic" & vbCrLf & _
                            "Imports System" & vbCrLf & _
                            "Imports System.Collections" & vbCrLf & _
                            "Imports System.Collections.Generic" & vbCrLf & _
                            "Imports System.Diagnostics" & vbCrLf & _
                            "Imports System.Linq"
        Else
            ImportsString = Join((From xItem In CustomImports Select "Imports " & xItem).ToArray, vbCrLf)
        End If

        Dim EvalString As String = ImportsString & vbCrLf & _
                                   "Public Class Main" & vbCrLf & _
                                   "    Inherits " & EvaluatorClassName & vbCrLf & _
                                   DimStr & vbCrLf & _
                                   PramsSetStr & vbCrLf & _
                                   "    Public Function Main() As Object" & vbCrLf & _
                                   "        Return Me" & vbCrLf & _
                                   "    End Function" & vbCrLf & _
                                   "    Public Overrides Function Eval() As Object" & vbCrLf & _
                                   "        " & vbCode & vbCrLf & _
                                   "    End Function" & vbCrLf & _
                                   "End Class"
        'Dim asd As Type
        'asd = GetType(IEnumerable)
        'Dim sw As New StreamWriter("c:\asd.txt")
        'sw.WriteLine(EvalString)
        'sw.Close()

        Dim OutClass = Build(EvalString)
        Dim EvaluatorClass = TryCast(OutClass, EvaluatorClass)
        If EvaluatorClass IsNot Nothing Then
            For Each item In Prams
                EvaluatorClass.SetPram(item.PramName, item.PramData)
            Next
            Return EvaluatorClass.Eval()
        Else
            Return Nothing
        End If
    End Function

    Public Function Build(ByVal vbCode As String, Optional ByVal CallFunction As String = "Main", Optional ByVal ScriptPath As String = "", Optional ByVal CompileOnly As Boolean = False) As Object

        CompilerResults = Nothing
        'Dim c As VBCodeProvider = New VBCodeProvider
        'Dim icc As ICodeCompiler = c.CreateCompiler()
        Dim cp As CompilerParameters = New CompilerParameters

        Dim CodeProviderOptions As New Dictionary(Of String, String)
        CodeProviderOptions.Add("CompilerVersion", "v3.5")
        Dim icc As CodeDomProvider = New Microsoft.VisualBasic.VBCodeProvider(CodeProviderOptions) 'CodeDomProvider.CreateProvider("VB")
        'Dim icc As CodeDomProvider = CodeDomProvider.CreateProvider("VB")
        CodeDomProvider.IsDefinedLanguage("VB")

        ' Sample code for adding your own referenced assemblies
        cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location)
        For Each item In References
            'Dim ItemName As String = item
            'Dim File As IO.FileInfo = FileIO.FileSystem.GetFileInfo(ItemName)
            'If LCase(File.Extension) = ".dll" Or LCase(File.Extension) = ".exe" Then
            '    'drop this
            '    ItemName = System.IO.Path.GetFileNameWithoutExtension(File.Name)
            'End If

            Dim aObject As Assembly = Nothing
            If FileIO.FileSystem.FileExists(item) Then
                aObject = Assembly.LoadFile(IO.Path.GetFullPath(item))
            Else
                aObject = Assembly.Load(item) 'ObsoleteFunctionCaller.CallFunction.Reflection_Assembly_LoadWithPartialName(ItemName)
            End If
            If aObject IsNot Nothing AndAlso aObject.Location <> "" Then
                cp.ReferencedAssemblies.Add(aObject.Location)
            Else
                'try to use the path of the plugin if we have one
                If ScriptPath <> "" Then
                    cp.ReferencedAssemblies.Add(ScriptPath & "\" & item)
                Else
                    cp.ReferencedAssemblies.Add(item)
                End If
            End If
        Next

        cp.CompilerOptions = "/t:library /optimize"
        cp.GenerateInMemory = True

        CompilerResults = icc.CompileAssemblyFromSource(cp, vbCode)
        Dim a As System.Reflection.Assembly
        Try
            a = CompilerResults.CompiledAssembly
        Catch ex As Exception
            Dim errors = (From xItem In CompilerResults.Errors.OfType(Of System.CodeDom.Compiler.CompilerError)() Where xItem.IsWarning = False).ToList
            Throw New ScripCompilerException(errors)
        End Try
        If CompileOnly Then Return Nothing

        Dim o = a.CreateInstance(CallFunction)

        Dim t As Type = o.GetType()

        Dim mi As MethodInfo = t.GetMethod(CallFunction)
        Dim s As Object
        s = mi.Invoke(o, Nothing)
        Return s

    End Function

    Public Class ScripCompilerException
        Inherits Exception

        Public errors As List(Of CompilerError)
        Public Sub New(ByVal errors As List(Of CompilerError))
            MyBase.New(errors.Count & " error" & If(errors.Count = 1, "", "s") & " occured when compilling: " & Join((From xItem In errors Select xItem.ErrorText).ToArray, vbCrLf))
            Me.errors = errors
        End Sub

    End Class

End Class
