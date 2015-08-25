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

<System.ComponentModel.TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))> _
Public Class SpellCheckSettings
    Implements ICloneable

    Public Overrides Function ToString() As String
        Return "SpellCheckSettings" 'MyBase.ToString()
    End Function

    Public Class RedrawArgs
        Inherits EventArgs
        Public ReloadDictionaryCache As Boolean
    End Class

    Public Event Redraw(ByVal sender As Object, ByVal e As RedrawArgs)

    Protected Overridable Sub OnRedraw(Optional ByVal RedrawArgs As RedrawArgs = Nothing)
        If RedrawArgs Is Nothing Then RedrawArgs = New RedrawArgs
        RaiseEvent Redraw(Me, RedrawArgs)
    End Sub

    Private mc_AllowAdditions As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow Additions")> _
    <System.ComponentModel.Description("Allows the user to add new words to the dictionary")> _
    Public Property AllowAdditions() As Boolean
        Get
            Return mc_AllowAdditions
        End Get
        Set(ByVal value As Boolean)
            mc_AllowAdditions = value
        End Set
    End Property

    Private mc_AllowRemovals As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow Removals")> _
    <System.ComponentModel.Description("Allows the user to remove existing words from the dictionary")> _
    Public Property AllowRemovals() As Boolean
        Get
            Return mc_AllowRemovals
        End Get
        Set(ByVal value As Boolean)
            mc_AllowRemovals = value
        End Set
    End Property

    Private mc_AllowInMenuDefs As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow In Menu Definitions")> _
    <System.ComponentModel.Description("Shows definitions for known words in the context menu")> _
    Public Property AllowInMenuDefs() As Boolean
        Get
            Return mc_AllowInMenuDefs
        End Get
        Set(ByVal value As Boolean)
            mc_AllowInMenuDefs = value
        End Set
    End Property

    Private mc_AllowChangeTo As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow Change To")> _
    <System.ComponentModel.Description("Adds the synonym lookup (""Change to"") to the right click menu")> _
    Public Property AllowChangeTo() As Boolean
        Get
            Return mc_AllowChangeTo
        End Get
        Set(ByVal value As Boolean)
            mc_AllowChangeTo = value
        End Set
    End Property

    Private mc_AllowF7 As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow F7")> _
    <System.ComponentModel.Description("Allows the user to press F7 to bring up the spell check dialog")> _
    Public Property AllowF7() As Boolean
        Get
            Return mc_AllowF7
        End Get
        Set(ByVal value As Boolean)
            mc_AllowF7 = value
        End Set
    End Property

    Private mc_AllowIgnore As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Allow Ignore")> _
    <System.ComponentModel.Description("Allows the user to ignore misspelled words")> _
    Public Property AllowIgnore() As Boolean
        Get
            Return mc_AllowIgnore
        End Get
        Set(ByVal value As Boolean)
            mc_AllowIgnore = value
        End Set
    End Property

    Public Enum ShowIgnoreState
        AlwaysShow
        AlwaysHide
        OnKeyDown
    End Enum
    Private mc_ShowIgnored As ShowIgnoreState = ShowIgnoreState.OnKeyDown
    <System.ComponentModel.DefaultValue(GetType(ShowIgnoreState), "OnKeyDown")> _
    <System.ComponentModel.DisplayName("Show Ignored")> _
    <System.ComponentModel.Description("Sets the draw state of ignored words")> _
    Public Property ShowIgnored() As ShowIgnoreState
        Get
            Return mc_ShowIgnored
        End Get
        Set(ByVal value As ShowIgnoreState)
            mc_ShowIgnored = value
            OnRedraw()
        End Set
    End Property

    Private mc_MistakeColor As Color = Color.Red
    <System.ComponentModel.DefaultValue(GetType(Color), "Red")> _
    <System.ComponentModel.DisplayName("Mistake Color")> _
    <System.ComponentModel.Description("Sets the base color for mistake highlighting")> _
    Public Property MistakeColor() As Color
        Get
            Return mc_MistakeColor
        End Get
        Set(ByVal value As Color)
            mc_MistakeColor = value
            OnRedraw()
        End Set
    End Property

    Private mc_CaseMistakeColor As Color = Color.Green
    <System.ComponentModel.DefaultValue(GetType(Color), "Green")> _
    <System.ComponentModel.DisplayName("Case Mistake Color")> _
    <System.ComponentModel.Description("Sets the base color for case mistake highlighting")> _
    Public Property CaseMistakeColor() As Color
        Get
            Return mc_CaseMistakeColor
        End Get
        Set(ByVal value As Color)
            mc_CaseMistakeColor = value
            OnRedraw()
        End Set
    End Property

    Private mc_IgnoreColor As Color = Color.Blue
    <System.ComponentModel.DefaultValue(GetType(Color), "Blue")> _
    <System.ComponentModel.DisplayName("Ignore Color")> _
    <System.ComponentModel.Description("Sets the base color for ignored word highlighting")> _
    Public Property IgnoreColor() As Color
        Get
            Return mc_IgnoreColor
        End Get
        Set(ByVal value As Color)
            mc_IgnoreColor = value
            OnRedraw()
        End Set
    End Property

    Private mc_ShowMistakes As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Show Mistakes")> _
    <System.ComponentModel.Description("Shows mistake highlights")> _
    Public Property ShowMistakes() As Boolean
        Get
            Return mc_ShowMistakes
        End Get
        Set(ByVal value As Boolean)
            mc_ShowMistakes = value
            OnRedraw()
        End Set
    End Property

    Private mc_IgnoreWordsInUpperCase As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Ignore Words In Upper Case")> _
    Public Property IgnoreWordsInUpperCase() As Boolean
        Get
            Return mc_IgnoreWordsInUpperCase
        End Get
        Set(ByVal value As Boolean)
            mc_IgnoreWordsInUpperCase = value
            OnRedraw(New RedrawArgs() With {.ReloadDictionaryCache = True})
        End Set
    End Property

    Private mc_IgnoreWordsWithNumbers As Boolean = True
    <System.ComponentModel.DefaultValue(True)> _
    <System.ComponentModel.DisplayName("Ignore Words With Numbers")> _
    Public Property IgnoreWordsWithNumbers() As Boolean
        Get
            Return mc_IgnoreWordsWithNumbers
        End Get
        Set(ByVal value As Boolean)
            mc_IgnoreWordsWithNumbers = value
            OnRedraw(New RedrawArgs() With {.ReloadDictionaryCache = True})
        End Set
    End Property

    Public Function IgnoreWordOverride(ByVal Word As String) As Boolean
        Word = Dictionary.Formatting.RemoveApoS(Word)
        Return (IgnoreWordsInUpperCase AndAlso Dictionary.Formatting.AllInCaps(Word)) OrElse _
               (IgnoreWordsWithNumbers AndAlso Dictionary.Formatting.ContainsNumbers(Word))
    End Function

    Public Sub New()

    End Sub

    Public Overloads Function Clone() As SpellCheckSettings
        Return DirectCast(CloneObject(), SpellCheckSettings)
    End Function

    <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
    Public Overloads Function CloneObject() As Object Implements System.ICloneable.Clone
        Dim SpellCheckSettings = New SpellCheckSettings
        For Each prop In Me.GetType.GetProperties
            If prop.CanWrite AndAlso prop.CanRead Then
                prop.SetValue(SpellCheckSettings, prop.GetValue(Me, Nothing), Nothing)
            End If
        Next
        Return SpellCheckSettings
    End Function

End Class