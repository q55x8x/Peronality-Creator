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

#Region "Control.SpellCheck"

Public Module SpellCheckControlExtension

    'store a list of the SpellCheckControlBase items that are associated with each control
    'Public SpellCheckControls As New Dictionary(Of Control, SpellCheckControlBase)

    <System.Runtime.CompilerServices.Extension()> _
    Public Function SpellCheck(ByVal sender As Control, Optional ByVal AutoCreate As Boolean = True, Optional ByVal SpellCheckSettings As SpellCheckSettings = Nothing) As SpellCheckControlBase
        Dim Extension = sender.ExtensionCast(Of SpellCheckControlBase)()
        If Extension IsNot Nothing Then
            'exists

        Else
            If AutoCreate Then
                ''create SpellCheckControlBase object and send it back...

                Static plugins As List(Of SpellCheckControlBase) = PluginManager(Of SpellCheckControlBase).GetPlugins

                Dim AcceptedClass = (From xItem In plugins Where (From xItemTypes In xItem.ControlTypes Where xItemTypes.IsAssignableFrom(sender.GetType)).FirstOrDefault IsNot Nothing).FirstOrDefault  'AndAlso xItem.GetType.GetCustomAttributes(True).OfType(Of DoNotApplyControlExtensionAttribute)().FirstOrDefault Is Nothing)
                If AcceptedClass IsNot Nothing Then
                    'create a new instance of the plugin class
                    Dim o = DirectCast(System.Activator.CreateInstance(AcceptedClass.GetType), SpellCheckControlBase) 'TryCast(AcceptedClass.CreateObject, SpellCheckControlBase)
                    ControlExtensions.LoadSingleControlExtension(sender, o)
                Else
                    'no plugins for this control type
                    Return Nothing
                End If

            Else
                'we don't want to automatically this control to check spelling, and it is not enabled already so return nothing
                Return Nothing
            End If
        End If
        'could have been created
        If Extension Is Nothing Then Extension = sender.ExtensionCast(Of SpellCheckControlBase)()
        If SpellCheckSettings Is Nothing Then SpellCheckSettings = DefaultSpellCheckSettings
        If SpellCheckSettings IsNot Nothing AndAlso Extension IsNot Nothing Then
            Extension.Settings = SpellCheckSettings
        End If
        Return Extension
    End Function

    Public DefaultSpellCheckSettings As SpellCheckSettings

End Module

#End Region

#Region "Control.EnableSpellCheck"

Public Module SpellCheckFormExtension

#Region "Enable"

    Public Event DictionaryLoaded(ByVal sender As Object, ByVal e As EventArgs)

    Public Sub DictionaryLoaded_cb()
        RaiseEvent DictionaryLoaded(Nothing, EventArgs.Empty)

        Dim SpellCheckControls = ControlExtensions.GetControlsWithExtension(Of SpellCheckControlBase)()
        For Each item In SpellCheckControls
            item.SpellCheck.InvokeRepaint()
        Next
    End Sub

    Private Sub ControlExtensionRemoved(ByVal sender As Object, ByVal e As ControlExtensionAddedRemovedEventArgs)
        DisabledSpellCheckControls.Remove(e.Control)
    End Sub

    <System.Runtime.CompilerServices.Extension()> _
    Public Sub EnableSpellCheck(ByVal sender As Control)
        If sender.ExtensionCast(Of SpellCheckControlBase)() IsNot Nothing Then
            'this control has been spellchecked before...
        Else
            sender.SpellCheck()
        End If
        'Check that the spell check has been enabled for this control...
        '(single line text boxes for eg disable themselves by default when the extension is loaded)
        If DisabledSpellCheckControls.Contains(sender) Then
            DisabledSpellCheckControls.Remove(sender)
            sender.SpellCheck.RepaintControl()
        End If
    End Sub

#End Region

#Region "DisableSpellCheck"

    Friend DisabledSpellCheckControls As New List(Of Control)

    <System.Runtime.CompilerServices.Extension()> _
    Public Sub DisableSpellCheck(ByVal sender As Control)
        RemoveHandler ControlExtensions.ControlExtensionRemoved, AddressOf ControlExtensionRemoved
        AddHandler ControlExtensions.ControlExtensionRemoved, AddressOf ControlExtensionRemoved

        If sender.ExtensionCast(Of SpellCheckControlBase)() IsNot Nothing Then
            If DisabledSpellCheckControls.Contains(sender) Then
                DisabledSpellCheckControls.Remove(sender)
            Else
                DisabledSpellCheckControls.Add(sender)
            End If
            sender.SpellCheck.RepaintControl()
        End If
    End Sub

    <System.Runtime.CompilerServices.Extension()> _
    Public Function IsSpellCheckEnabled(ByVal sender As Control) As Boolean
        Return sender.ExtensionCast(Of SpellCheckControlBase)() IsNot Nothing AndAlso DisabledSpellCheckControls.Contains(sender) = False
    End Function

#End Region

End Module

#End Region