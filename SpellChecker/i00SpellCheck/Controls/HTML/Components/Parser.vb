'©i00 Productions All rights reserved
'This article is derived from http://www.codeproject.com/Articles/33842/HTMLLabel-An-HTML-Label-for-the-NET-CF
'----------------------------------------------------------------------------------------------------
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

Imports System.Text
Imports System.Collections
Imports System.Collections.Generic

Public Class HTMLParser
    Friend NotInheritable Class HTMLColors
        Private Sub New()
        End Sub
        Shared _colors As New Dictionary(Of String, String)()

        Private Shared Sub setColors()
            _colors.Clear()

            _colors.Add("aliceblue", "#F0F8FF")
            _colors.Add("antiquewhite", "#FAEBD7")
            _colors.Add("aqua", "#00FFFF")
            _colors.Add("aquamarine", "#7FFFD4")
            _colors.Add("azure", "#F0FFFF")
            _colors.Add("beige", "#F5F5DC")
            _colors.Add("bisque", "#FFE4C4")
            _colors.Add("black", "#000000")
            _colors.Add("blanchedalmond", "#FFEBCD")
            _colors.Add("blue", "#0000FF")
            _colors.Add("blueviolet", "#8A2BE2")
            _colors.Add("brown", "#A52A2A")
            _colors.Add("burlywood", "#DEB887")
            _colors.Add("cadetblue", "#5F9EA0")
            _colors.Add("chartreuse", "#7FFF00")
            _colors.Add("chocolate", "#D2691E")
            _colors.Add("coral", "#FF7F50")
            _colors.Add("cornflowerblue", "#6495ED")
            _colors.Add("cornsilk", "#FFF8DC")
            _colors.Add("crimson", "#DC143C")
            _colors.Add("cyan", "#00FFFF")
            _colors.Add("darkblue", "#00008B")
            _colors.Add("darkcyan", "#008B8B")
            _colors.Add("darkgoldenrod", "#B8860B")
            _colors.Add("darkgray", "#A9A9A9")
            _colors.Add("darkgreen", "#006400")
            _colors.Add("darkkhaki", "#BDB76B")
            _colors.Add("darkmagenta", "#8B008B")
            _colors.Add("darkolivegreen", "#556B2F")
            _colors.Add("darkorange", "#FF8C00")
            _colors.Add("darkorchid", "#9932CC")
            _colors.Add("darkred", "#8B0000")
            _colors.Add("darksalmon", "#E9967A")
            _colors.Add("darkseagreen", "#8FBC8F")
            _colors.Add("darkslateblue", "#483D8B")
            _colors.Add("darkslategray", "#2F4F4F")
            _colors.Add("darkturquoise", "#00CED1")
            _colors.Add("darkviolet", "#9400D3")
            _colors.Add("deeppink", "#FF1493")
            _colors.Add("deepskyblue", "#00BFFF")
            _colors.Add("dimgray", "#696969")
            _colors.Add("dodgerblue", "#1E90FF")
            _colors.Add("firebrick", "#B22222")
            _colors.Add("floralwhite", "#FFFAF0")
            _colors.Add("forestgreen", "#228B22")
            _colors.Add("fuchsia", "#FF00FF")
            _colors.Add("gainsboro", "#DCDCDC")
            _colors.Add("ghostwhite", "#F8F8FF")
            _colors.Add("gold", "#FFD700")
            _colors.Add("goldenrod", "#DAA520")
            _colors.Add("gray", "#808080")
            _colors.Add("green", "#008000")
            _colors.Add("greenyellow", "#ADFF2F")
            _colors.Add("honeydew", "#F0FFF0")
            _colors.Add("hotpink", "#FF69B4")
            _colors.Add("indianred", "#CD5C5C")
            _colors.Add("indigo", "#4B0082")
            _colors.Add("ivory", "#FFFFF0")
            _colors.Add("khaki", "#F0E68C")
            _colors.Add("lavender", "#E6E6FA")
            _colors.Add("lavenderblush", "#FFF0F5")
            _colors.Add("lawngreen", "#7CFC00")
            _colors.Add("lemonchiffon", "#FFFACD")
            _colors.Add("lightblue", "#ADD8E6")
            _colors.Add("lightcoral", "#F08080")
            _colors.Add("lightcyan", "#E0FFFF")
            _colors.Add("lightgoldenrodyellow", "#FAFAD2")
            _colors.Add("lightgrey", "#D3D3D3")
            _colors.Add("lightgreen", "#90EE90")
            _colors.Add("lightpink", "#FFB6C1")
            _colors.Add("lightsalmon", "#FFA07A")
            _colors.Add("lightseagreen", "#20B2AA")
            _colors.Add("lightskyblue", "#87CEFA")
            _colors.Add("lightslategray", "#778899")
            _colors.Add("lightsteelblue", "#B0C4DE")
            _colors.Add("lightyellow", "#FFFFE0")
            _colors.Add("lime", "#00FF00")
            _colors.Add("limegreen", "#32CD32")
            _colors.Add("linen", "#FAF0E6")
            _colors.Add("magenta", "#FF00FF")
            _colors.Add("maroon", "#800000")
            _colors.Add("mediumaquamarine", "#66CDAA")
            _colors.Add("mediumblue", "#0000CD")
            _colors.Add("mediumorchid", "#BA55D3")
            _colors.Add("mediumpurple", "#9370D8")
            _colors.Add("mediumseagreen", "#3CB371")
            _colors.Add("mediumslateblue", "#7B68EE")
            _colors.Add("mediumspringgreen", "#00FA9A")
            _colors.Add("mediumturquoise", "#48D1CC")
            _colors.Add("mediumvioletred", "#C71585")
            _colors.Add("midnightblue", "#191970")
            _colors.Add("mintcream", "#F5FFFA")
            _colors.Add("mistyrose", "#FFE4E1")
            _colors.Add("moccasin", "#FFE4B5")
            _colors.Add("navajowhite", "#FFDEAD")
            _colors.Add("navy", "#000080")
            _colors.Add("oldlace", "#FDF5E6")
            _colors.Add("olive", "#808000")
            _colors.Add("olivedrab", "#6B8E23")
            _colors.Add("orange", "#FFA500")
            _colors.Add("orangered", "#FF4500")
            _colors.Add("orchid", "#DA70D6")
            _colors.Add("palegoldenrod", "#EEE8AA")
            _colors.Add("palegreen", "#98FB98")
            _colors.Add("paleturquoise", "#AFEEEE")
            _colors.Add("palevioletred", "#D87093")
            _colors.Add("papayawhip", "#FFEFD5")
            _colors.Add("peachpuff", "#FFDAB9")
            _colors.Add("peru", "#CD853F")
            _colors.Add("pink", "#FFC0CB")
            _colors.Add("plum", "#DDA0DD")
            _colors.Add("powderblue", "#B0E0E6")
            _colors.Add("purple", "#800080")
            _colors.Add("red", "#FF0000")
            _colors.Add("rosybrown", "#BC8F8F")
            _colors.Add("royalblue", "#4169E1")
            _colors.Add("saddlebrown", "#8B4513")
            _colors.Add("salmon", "#FA8072")
            _colors.Add("sandybrown", "#F4A460")
            _colors.Add("seagreen", "#2E8B57")
            _colors.Add("seashell", "#FFF5EE")
            _colors.Add("sienna", "#A0522D")
            _colors.Add("silver", "#C0C0C0")
            _colors.Add("skyblue", "#87CEEB")
            _colors.Add("slateblue", "#6A5ACD")
            _colors.Add("slategray", "#708090")
            _colors.Add("snow", "#FFFAFA")
            _colors.Add("springgreen", "#00FF7F")
            _colors.Add("steelblue", "#4682B4")
            _colors.Add("tan", "#D2B48C")
            _colors.Add("teal", "#008080")
            _colors.Add("thistle", "#D8BFD8")
            _colors.Add("tomato", "#FF6347")
            _colors.Add("turquoise", "#40E0D0")
            _colors.Add("violet", "#EE82EE")
            _colors.Add("wheat", "#F5DEB3")
            _colors.Add("white", "#FFFFFF")
            _colors.Add("whitesmoke", "#F5F5F5")
            _colors.Add("yellow", "#FFFF00")
            _colors.Add("yellowgreen", "#9ACD32")
        End Sub

        Public Shared Function GetColor(ByVal hexValue As String) As Color
            If (hexValue.StartsWith("#")) AndAlso (hexValue.Length = 7) Then
                Dim rgb As Integer = Integer.Parse("FF" & hexValue.Substring(1, 6), System.Globalization.NumberStyles.HexNumber)
                Dim c As Color = Color.FromArgb(rgb)
                Return Color.FromArgb(rgb)
            End If

            Return Color.Black
        End Function

        Public Shared Function GetColorByName(ByVal colorName As String) As Color
            colorName = colorName.Replace(" ", "").ToLower()
            If _colors.Count = 0 Then
                setColors()
            End If
            If _colors.ContainsKey(colorName) Then
                Return GetColor(_colors(colorName))
            End If

            Return Color.Black
        End Function
    End Class

    Friend Enum ElementType
        Status
        HTML
    End Enum

    Friend Class Element
        Private _type As ElementType
        Private _status As Status
        Private _html As HTMLParser.SimplePart
        Private _size As SizeF
        Private _dispRect As Rectangle

        Public Property DisplayedRect() As Rectangle
            Get
                Return _dispRect
            End Get
            Set(ByVal value As Rectangle)
                _dispRect = value
            End Set
        End Property

        Public Property HTML() As HTMLParser.SimplePart
            Get
                Return _html
            End Get
            Set(ByVal value As HTMLParser.SimplePart)
                _html = value
            End Set
        End Property

        Public Property Type() As ElementType
            Get
                Return _type
            End Get
            Set(ByVal value As ElementType)
                _type = value
            End Set
        End Property

        Public Property Status() As Status
            Get
                Return _status
            End Get
            Set(ByVal value As Status)
                _status = value
            End Set
        End Property

        Public Property Size() As SizeF
            Get
                Return _size
            End Get
            Set(ByVal value As SizeF)
                _size = value
            End Set
        End Property

        Public Sub New(ByVal status As Status)
            _type = ElementType.Status
            _status = New Status(status)
            _size = New SizeF(0, 0)
            _html = Nothing
        End Sub

        Public Sub New(ByVal html As HTMLParser.SimplePart)
            _type = ElementType.HTML
            _status = Nothing
            _html = html

            If TypeOf html Is HTMLParser.Text Then
                _size = New SizeF(0, 0)
                Return
            End If

            Throw New Exception("Unknown HTML element!")
        End Sub

        Public Overrides Function ToString() As String
            Select Case _type
                Case ElementType.Status
                    Return String.Format("STAT Element: stat={0};sz={1}", _status, _size.ToString())
                Case ElementType.HTML
                    Return String.Format("HTML Element: type={0};sz={1}", _html.Type.ToString(), _size.ToString())
            End Select

            Return "NULL Element"
        End Function
    End Class

    Friend Class TextLine
        Private _width As Single
        Private _height As Single
        Private _lastElement As Integer

        Public Property LastElement() As Integer
            Get
                Return _lastElement
            End Get
            Set(ByVal value As Integer)
                _lastElement = value
            End Set
        End Property

        Public Property Height() As Single
            Get
                Return _height
            End Get
            Set(ByVal value As Single)
                _height = value
            End Set
        End Property

        Public Property Width() As Single
            Get
                Return _width
            End Get
            Set(ByVal value As Single)
                _width = value
            End Set
        End Property

        Public Sub New(ByVal width As Single, ByVal height As Single, ByVal lastElement As Integer)
            _width = width
            _height = height
            _lastElement = lastElement
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("TextLine: w={0};h={1};le={2}", _width, _height, _lastElement)
        End Function
    End Class


    Friend Class Elements
        Private _elements As List(Of Element) = Nothing

        Public Sub New()
            _elements = New List(Of Element)()
        End Sub

        Public Function Parse(ByVal lineOfText As String, ByVal status As Status) As Status
            _elements.Clear()
            lineOfText = lineOfText.Replace(vbLf, "")
            lineOfText = lineOfText.Replace(vbCr, "")

            Dim brushes As New Stack(Of STRBrush)()
            Dim fonts As New Stack(Of STRFont)()

            brushes.Push(New STRBrush(status.Brush))
            fonts.Push(New STRFont(status.Font))

            _elements.Add(New Element(status))
            Dim parts As List(Of HTMLParser.Part) = HTMLParser.Parse.ParseAll(lineOfText)
            For Each part As HTMLParser.Part In parts
                If TypeOf part Is HTMLParser.Text Then
                    Dim text As HTMLParser.Text = DirectCast(part, HTMLParser.Text)
                    Dim words As String() = text.Value.Trim().Split(" "c)
                    For Each word As String In words
                        _elements.Add(New Element(New HTMLParser.Text(word)))
                    Next
                End If


                If TypeOf part Is HTMLParser.Tag Then
                    Dim tag As HTMLParser.Tag = DirectCast(part, HTMLParser.Tag)
                    Dim oldStatus As New Status(status)

                    Select Case tag.LName
                        Case "img"
                            Dim Src = tag.AttrList.Find("src")
                            If Src IsNot Nothing Then
                                If FileIO.FileSystem.FileExists(Src.Value) Then
                                    status.Image = New STRImage() With {.Image = Image.FromFile(Src.Value)}
                                End If
                            End If
                            GoTo AddElement
                        Case "br"
                            status.NewLine = True
                        Case "pre"
                            status.WordWrap = tag.[End]
                        Case Else
                            If tag.[End] Then
                                Select Case tag.LName
                                    Case "b"
                                        status.Font.Style = status.Font.Style And Not FontStyle.Bold
                                    Case "i"
                                        status.Font.Style = status.Font.Style And Not FontStyle.Italic
                                    Case "u"
                                        status.Font.Style = status.Font.Style And Not FontStyle.Underline
                                    Case "p"
                                        status.NewLine = True
                                        status.Alignment = ContentAlignment.TopLeft
                                    Case "font"
                                        Dim oldFS As FontStyle = status.Font.Style
                                        status.Brush = New STRBrush(If((brushes.Count > 1), brushes.Pop(), brushes.Peek()))
                                        status.Font = New STRFont(If((fonts.Count > 1), fonts.Pop(), fonts.Peek()))
                                        status.Font.Style = oldFS
                                End Select
                            Else
                                Select Case tag.LName
                                    Case "b"
                                        status.Font.Style = status.Font.Style Or FontStyle.Bold
                                    Case "i"
                                        status.Font.Style = status.Font.Style Or FontStyle.Italic
                                    Case "u"
                                        status.Font.Style = status.Font.Style Or FontStyle.Underline
                                    Case "p"
                                        status.NewLine = True
                                        status.Alignment = ContentAlignment.TopLeft
                                        Dim attr As HTMLParser.Attribute = tag.AttrList.Find("align")
                                        If attr IsNot Nothing Then
                                            If attr.LValue = "center" Then
                                                status.Alignment = ContentAlignment.TopCenter
                                            End If
                                            If attr.LValue = "right" Then
                                                status.Alignment = ContentAlignment.TopRight
                                            End If
                                        End If
                                    Case "font"
                                        brushes.Push(New STRBrush(status.Brush))
                                        fonts.Push(New STRFont(status.Font))

                                        Dim attr As HTMLParser.Attribute = tag.AttrList.Find("color")
                                        If attr IsNot Nothing Then
                                            If (attr.Value.Length = 7) AndAlso (attr.Value(0) = "#"c) Then
                                                status.Brush.Color = HTMLColors.GetColor(attr.Value)
                                            Else
                                                status.Brush.Color = HTMLColors.GetColorByName(attr.Value)
                                            End If
                                        End If

                                        attr = tag.AttrList.Find("size")
                                        If attr IsNot Nothing Then
                                            status.Font.Size = Convert.ToInt16(attr.Value)
                                        End If

                                        attr = tag.AttrList.Find("name")
                                        If attr IsNot Nothing Then
                                            status.Font.Name = attr.Value
                                        End If
                                End Select
                            End If
                    End Select

                    If oldStatus IsNot status Then
AddElement:
                        Dim element = New Element(status)
                        If status IsNot Nothing AndAlso status.Image IsNot Nothing Then
                            element.Status.Image = status.Image
                            status.Image = Nothing
                        End If
                        _elements.Add(element)
                        status.NewLine = False
                    End If
                End If
            Next

            Return New Status(status)
        End Function

        Public ReadOnly Property Value() As IList(Of Element)
            Get
                Return _elements
            End Get
        End Property

        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder(1000)
            For Each elem As Element In _elements
                sb.Append(elem.ToString())
                sb.Append(vbLf)
            Next
            Return sb.ToString()
        End Function
    End Class


    Public Class STRFont
        Public Name As String
        Public Size As Integer
        Public Style As FontStyle

        Public Overrides Function ToString() As String
            Return String.Format("Font: {0}/{1} - {2}", Name, Size.ToString(), Style.ToString())
        End Function

        Public Function GetRealFont() As Font
            Return New Font(Name, Size, Style)
        End Function

        Public Sub New()
            Name = "Tahoma"
            Size = 10
            Style = FontStyle.Regular
        End Sub

        Public Sub New(ByVal oldFont As STRFont)
            Name = oldFont.Name
            Size = oldFont.Size
            Style = oldFont.Style
        End Sub

        Public Sub New(ByVal fnt As Font)
            Name = fnt.Name
            Size = CInt(Math.Truncate(fnt.Size))
            Style = fnt.Style
        End Sub

    End Class

    Public Class STRBrush
        Public Color As Color

        Public Overrides Function ToString() As String
            Return Color.ToString()
        End Function

        Public Function GetRealBrush() As Brush
            Return New SolidBrush(Color)
        End Function

        Public Sub New()
            Color = Color.Black
        End Sub

        Public Sub New(ByVal oldBrush As STRBrush)
            Color = oldBrush.Color
        End Sub

        Public Sub New(ByVal color As Color)
            Me.Color = color
        End Sub

    End Class

    Public Class STRImage
        Public Image As Image
        Public Size As Size
    End Class

    Public Class Status
        'Implements IComparable
        Public Image As STRImage
        Public NewLine As Boolean
        Public WordWrap As Boolean
        Public Alignment As ContentAlignment
        Public Font As STRFont
        Public Brush As STRBrush

        Public Sub New()
            Font = New STRFont()
            NewLine = False
            WordWrap = True
            Alignment = ContentAlignment.TopLeft
            Brush = New STRBrush()
        End Sub

        Public Sub New(ByVal oldSatus As Status)
            Font = New STRFont(oldSatus.Font)
            NewLine = oldSatus.NewLine
            WordWrap = oldSatus.WordWrap
            Alignment = oldSatus.Alignment
            Brush = New STRBrush(oldSatus.Brush)
        End Sub

    End Class










    Public Enum PartType
        Unknown
        Text
        ProcessInstruction
        Comment
        SpecialAnchore
        Tag
    End Enum

    Public Class Attribute
        Private _name As String
        Private _value As String
        Private _lname As String
        Private _lvalue As String

        Private Sub [set](ByVal name As String, ByVal value As String)
            _name = name
            _lname = name.ToLower()
            _value = value
            _lvalue = value.ToLower()
        End Sub

        Public Sub New(ByVal name As String, ByVal value As String)
            [set](name, value)
        End Sub

        Public Sub New(ByVal oldAttr As Attribute)
            [set](oldAttr.Name, oldAttr.Value)
        End Sub

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
                _lname = value.ToLower()
            End Set
        End Property

        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
                _lvalue = value.ToLower()
            End Set
        End Property

        Public ReadOnly Property LName() As String
            Get
                Return _lname
            End Get
        End Property

        Public ReadOnly Property LValue() As String
            Get
                Return _lvalue
            End Get
        End Property

        Public Function GetValueAsFloat(ByVal defaultValue As Single) As Single
            Try
                Return CSng(Convert.ToDouble(_value))
            Catch generatedExceptionName As Exception
            End Try
            Return defaultValue
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("Attribute: name={0};value={1}", _name, _value)
        End Function
    End Class

    Public Class AttributeList
        Protected _list As List(Of Attribute) = Nothing

        Public Sub New()
            _list = New List(Of Attribute)()
        End Sub

        Public Sub New(ByVal oldAttrList As AttributeList)
            _list = New List(Of Attribute)()
            For i As Integer = 0 To oldAttrList._list.Count - 1
                Add(New Attribute(oldAttrList._list(i)))
            Next
        End Sub

        Public Function Find(ByVal name As String) As Attribute
            name = name.ToLower()
            For Each attr As Attribute In _list
                If attr.LName = name Then
                    Return attr
                End If
            Next

            Return Nothing
        End Function

        Public Sub Add(ByVal a As Attribute)
            _list.Add(a)
        End Sub


        Public Overrides Function ToString() As String
            Dim sb As New StringBuilder(1024)
            For Each att As Attribute In _list
                sb.Append(att.ToString())
                sb.Append(vbLf)
            Next
            Return sb.ToString()
        End Function
    End Class

    Public Class Part
        Private _type As PartType

        Public Sub New(ByVal type As PartType)
            _type = type
        End Sub

        Public ReadOnly Property Type() As PartType
            Get
                Return _type
            End Get
        End Property
    End Class

    Public Class SimplePart
        Inherits Part
        Private _value As String

        Public Property Value() As String
            Get
                Return _value
            End Get
            Set(ByVal value As String)
                _value = value
            End Set
        End Property

        Public Sub New(ByVal type As PartType, ByVal text As String)
            MyBase.New(type)
            _value = text
        End Sub
    End Class

    Public Class Text
        Inherits SimplePart
        Public Sub New(ByVal value As String)
            MyBase.New(PartType.Text, value)
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("TEXT: {0}", Me.Value)
        End Function
    End Class

    Public Class ProcessInstruction
        Inherits SimplePart
        Public Sub New(ByVal value As String)
            MyBase.New(PartType.ProcessInstruction, value)
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("ProcInstr: {0}", Me.Value)
        End Function
    End Class

    Public Class Comment
        Inherits SimplePart
        Public Sub New(ByVal value As String)
            MyBase.New(PartType.Comment, value)
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("Comment: {0}", Me.Value)
        End Function
    End Class

    Public Class Tag
        Inherits Part
        Private _name As String
        Private _lname As String
        Private _end As Boolean
        Private _attrList As AttributeList = Nothing

        Private Sub [set](ByVal name__1 As String, ByVal [end] As Boolean, ByVal attrList As AttributeList)
            _end = [end]
            Name = name__1
            If attrList IsNot Nothing Then
                _attrList = New AttributeList(attrList)
            Else
                _attrList = Nothing
            End If
        End Sub

        Public Sub New(ByVal name As String, ByVal attrList As AttributeList)
            MyBase.New(PartType.Tag)
            [set](name, False, attrList)
        End Sub

        Public Property [End]() As Boolean
            Get
                Return _end
            End Get
            Set(ByVal value As Boolean)
                _end = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                If value.Length > 0 Then
                    If (value(0) = "/"c) OrElse (value(value.Length - 1) = "/"c) Then
                        Dim start As Integer = If((value(0) = "/"c), 1, 0)
                        _end = True
                        _name = value.Substring(start, value.Length - 1)
                        _lname = _name.ToLower()
                        Return
                    End If
                End If

                _name = value
                _lname = _name.ToLower()
            End Set
        End Property

        Public ReadOnly Property LName() As String
            Get
                Return _lname
            End Get
        End Property

        Public Property AttrList() As AttributeList
            Get
                Return _attrList
            End Get
            Set(ByVal value As AttributeList)
                _attrList = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("TAG: name={0};end={1}" & vbLf & "{2}", _name, _end, _attrList)
        End Function
    End Class

    Public Class Parse
        Private _source As String
        Private _source_with_guards As String
        Private _source_len As Integer
        Private _idx As Integer

        Private Sub eatWhiteSpace()
            While _idx < _source_len
                Dim ch As Char = _source_with_guards(_idx)
                If ch <> " "c AndAlso ch <> ControlChars.Tab AndAlso ch <> ControlChars.Lf AndAlso ch <> ControlChars.Cr Then
                    Return
                End If
                _idx += 1
            End While
        End Sub

        Private Function parseAttributeName() As String
            eatWhiteSpace()

            Dim start As Integer = _idx
            While _idx < _source_len
                Dim ch As Char = _source_with_guards(_idx)
                If ch = " "c OrElse ch = ControlChars.Tab OrElse ch = ControlChars.Lf OrElse ch = ControlChars.Cr OrElse (ch = "="c) OrElse (ch = ">"c) Then
                    Exit While
                End If

                _idx += 1
            End While

            Dim name As String = _source_with_guards.Substring(start, _idx - start)
            eatWhiteSpace()
            Return name
        End Function

        Private Function parseAttributeValue() As String
            If _source_with_guards(_idx) <> "="c Then
                Return ""
            End If

            _idx += 1
            eatWhiteSpace()

            Dim value As String = ""
            Dim ch As Char = _source_with_guards(_idx)

            If (ch = "'"c) OrElse (ch = """"c) Then
                Dim valueDelimeter As Char = ch
                _idx += 1
                Dim start As Integer = _idx
                While _source_with_guards(_idx) <> valueDelimeter
                    _idx += 1
                End While
                value = _source_with_guards.Substring(start, _idx - start)
                _idx += 1
            Else
                Dim start As Integer = _idx
                While (_idx < _source_len) AndAlso (ch <> " "c AndAlso ch <> ControlChars.Tab AndAlso ch <> ControlChars.Lf AndAlso ch <> ControlChars.Cr) AndAlso (_source_with_guards(_idx) <> ">"c)
                    _idx += 1
                End While

                value = _source_with_guards.Substring(start, _idx - start)
            End If
            eatWhiteSpace()

            Return value
        End Function

        Private Function parseTag() As Part
            If _source_with_guards(_idx) = "!"c Then
                If (_source_with_guards(_idx + 1) = "-"c) AndAlso (_source_with_guards(_idx + 2) = "-"c) Then
                    _idx += 3
                    Dim start As Integer = _idx
                    While _idx < _source_len
                        If (_source_with_guards(_idx) = "-"c) AndAlso (_source_with_guards(_idx + 1) = "-"c) AndAlso (_source_with_guards(_idx + 2) = ">"c) Then
                            Exit While
                        End If
                        _idx += 1
                    End While
                    Dim value As String = _source_with_guards.Substring(start, _idx - start)
                    If _idx < _source_len Then
                        _idx += 3
                    End If
                    Return New Comment(value)
                Else
                    _idx += 1
                    Dim start As Integer = _idx
                    While (_idx < _source_len) AndAlso (_source_with_guards(_idx) <> ">"c)
                        _idx += 1
                    End While

                    Dim [end] As Integer = _idx
                    If _idx < _source_len Then
                        [end] = _idx - 1
                        _idx += 1
                    End If
                    Return New ProcessInstruction(_source_with_guards.Substring(start, [end] - start))
                End If
            End If

            Dim start1 As Integer = _idx
            While _idx < _source_len
                Dim ch As Char = _source_with_guards(_idx)
                If ch = " "c OrElse ch = ControlChars.Tab OrElse ch = ControlChars.Lf OrElse ch = ControlChars.Cr OrElse ch = ">"c Then
                    Exit While
                End If
                _idx += 1
            End While
            Dim name As String = _source_with_guards.Substring(start1, _idx - start1)

            eatWhiteSpace()

            Dim attrList As New AttributeList()
            While _source_with_guards(_idx) <> ">"c
                Dim ParseName As String = parseAttributeName()

                If _source_with_guards(_idx) = ">"c Then
                    attrList.Add(New Attribute(ParseName, ""))
                    Exit While
                End If

                Dim ParseValue As String = parseAttributeValue()
                attrList.Add(New Attribute(ParseName, ParseValue))
            End While
            _idx += 1

            Return New Tag(name, attrList)
        End Function

        Private Function isEof() As Boolean
            Return (_idx >= _source_len)
        End Function

        Private Function parseNext() As Part
            Dim start As Integer = _idx

            If _source_with_guards(_idx) = "<"c Then
                Dim ch As Char = Char.ToUpper(_source_with_guards(_idx + 1))
                If (ch >= "A"c) AndAlso (ch <= "Z"c) OrElse (ch = "!"c) OrElse (ch = "/"c) Then
                    _idx += 1
                    Return parseTag()
                End If

                _idx += 1
            End If

            While _idx < _source_len
                Dim ch As Char = _source_with_guards(_idx)
                If _source_with_guards(_idx) = "<"c Then
                    Exit While
                End If
                _idx += 1
            End While

            Dim value As String = _source_with_guards.Substring(start, _idx - start)
            Return New Text(value)
        End Function

        Private Property source() As String
            Get
                Return _source
            End Get

            Set(ByVal value As String)
                _source = value
                _source_with_guards = _source & ChrW(0) & ChrW(0) & ChrW(0)
                _source_len = _source.Length
            End Set
        End Property

        Public Shared Function ParseAll(ByVal HTMLString As String) As List(Of Part)
            Dim retValue As New List(Of Part)()

            Dim parse As New Parse()
            parse.source = HTMLString
            While Not parse.isEof()
                retValue.Add(parse.parseNext())
            End While

            Return retValue
        End Function
    End Class

End Class

