'©i00 Productions All rights reserved
'This article is derived from http://www.developer.com/net/asp/article.php/3102381/A-NET-Text-Printing-Class-That-Works.htm
'----------------------------------------------------------------------------------------------------
'
'i00 is not and shall not be held accountable for any damages directly or indirectly caused by the
'use or miss-use of this product.  This product is only a component and thus is intended to be used 
'as part of other software, it is not a complete software package, thus i00 Productions is not 
'responsible for any legal ramifications that software using this product breaches.

<System.ComponentModel.DesignerCategory("")> _
Friend Class TextPrint
    ' Inherits all the functionality of a PrintDocument
    Inherits Printing.PrintDocument
    ' Private variables to hold default font and text
    Private fntPrintFont As Font
    Private strText As String
    Public Sub New(ByVal Text As String)
        ' Sets the file stream
        MyBase.New()
        strText = Text
    End Sub
    Public Property Text() As String
        Get
            Return strText
        End Get
        Set(ByVal Value As String)
            strText = Value
        End Set
    End Property
    Protected Overrides Sub OnBeginPrint(ByVal ev _
                                As Printing.PrintEventArgs)
        ' Run base code
        MyBase.OnBeginPrint(ev)
        ' Sets the default font
        If fntPrintFont Is Nothing Then
            fntPrintFont = New Font("Times New Roman", 12)
        End If
    End Sub
    Public Property Font() As Font
        ' Allows the user to override the default font
        Get
            Return fntPrintFont
        End Get
        Set(ByVal Value As Font)
            fntPrintFont = Value
        End Set
    End Property
    Protected Overrides Sub OnPrintPage(ByVal ev _
       As Printing.PrintPageEventArgs)
        ' Provides the print logic for our document

        ' Run base code
        MyBase.OnPrintPage(ev)
        ' Variables
        Static intCurrentChar As Integer
        Dim intPrintAreaHeight, intPrintAreaWidth, _
            intMarginLeft, intMarginTop As Integer
        ' Set printing area boundaries and margin coordinates
        With MyBase.DefaultPageSettings
            intPrintAreaHeight = .PaperSize.Height - _
                               .Margins.Top - .Margins.Bottom
            intPrintAreaWidth = .PaperSize.Width - _
                              .Margins.Left - .Margins.Right
            intMarginLeft = .Margins.Left 'X
            intMarginTop = .Margins.Top   'Y
        End With
        ' If Landscape set, swap printing height/width
        If MyBase.DefaultPageSettings.Landscape Then
            Dim intTemp As Integer
            intTemp = intPrintAreaHeight
            intPrintAreaHeight = intPrintAreaWidth
            intPrintAreaWidth = intTemp
        End If
        ' Calculate total number of lines
        Dim intLineCount As Int32 = _
                CInt(intPrintAreaHeight / Font.Height)
        ' Initialise rectangle printing area
        Dim rectPrintingArea As New RectangleF(intMarginLeft, _
            intMarginTop, intPrintAreaWidth, intPrintAreaHeight)
        ' Initialise StringFormat class, for text layout
        Dim objSF As New StringFormat(StringFormatFlags.LineLimit)
        ' Figure out how many lines will fit into rectangle
        Dim intLinesFilled, intCharsFitted As Int32
        ev.Graphics.MeasureString(Mid(strText, _
                    Math.Max(1, intCurrentChar)), Font, _
                    New SizeF(intPrintAreaWidth, _
                    intPrintAreaHeight), objSF, _
                    intCharsFitted, intLinesFilled)

        'go back to the last space...
        Dim PrintedText = Mid(strText, Math.Max(1, intCurrentChar), intCharsFitted + 1)
        If intCurrentChar + intCharsFitted < strText.Length Then
            Dim LastBreakChar = PrintedText.LastIndexOfAny(New Char() {CChar(vbCr), CChar(vbLf), " "c, "-"c, "\"c, "/"c})
            If LastBreakChar <> -1 Then
                intCharsFitted -= (Len(PrintedText) - (LastBreakChar + 1)) - 1
                PrintedText = Left(PrintedText, LastBreakChar + 1)
            End If
        End If

        ' Print the text to the page
        ev.Graphics.DrawString(PrintedText, Font, _
            Brushes.Black, rectPrintingArea, objSF)
        ' Increase current char count
        intCurrentChar += intCharsFitted + 1
        ' Check whether we need to print more
        If intCurrentChar < strText.Length + 1 Then
            ev.HasMorePages = True
        Else
            ev.HasMorePages = False
            intCurrentChar = 0
        End If
    End Sub
    Public Function UpgradeZeros(ByVal Input As Integer) As Integer
        ' Upgrades all zeros to ones
        ' - used as opposed to defunct IIF or messy If statements

        If Input = 0 Then
            Return 1
        Else
            Return Input
        End If
    End Function
End Class