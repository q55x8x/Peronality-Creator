'i00 .Net Spell Check - Crossword Generator
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

<Serializable()> _
Public Class CrossWordGenerator
    Public CrossWordCells As New List(Of CrossWordCell)

    Dim mc_CrossWordSize As New Size(10, 10)
    Public Property CrossWordSize(Optional ByVal DropUnusedCellInfo As Boolean = True) As Size
        Get
            Return mc_CrossWordSize
        End Get
        Set(ByVal value As Size)
            mc_CrossWordSize = value
            If DropUnusedCellInfo Then
                DropUnusedCells()
            End If
        End Set
    End Property

    Public Sub FromString(ByVal theString As String)
        CrossWordCells.Clear()

        Dim arrSplitString = New String() {vbCrLf}
        Dim Lines = theString.Split(arrSplitString, StringSplitOptions.None)
        Dim XWordSize As New Size(0, Lines.Count)
        For iY = 0 To Lines.Count - 1
            If XWordSize.Width < Lines(iY).Length Then XWordSize.Width = Lines(iY).Length
            For iX = 0 To Lines(iY).Length - 1
                If Lines(iY)(iX) = " "c Then
                    'blank
                    CrossWordCells.Add(New CrossWordCell(iX + 1, iY + 1))
                ElseIf Char.IsLetterOrDigit(Lines(iY)(iX)) Then
                    'filled 
                    CrossWordCells.Add(New CrossWordCell(iX + 1, iY + 1, Lines(iY)(iX)))
                End If
            Next
        Next

        mc_CrossWordSize = XWordSize
    End Sub

    Public Overrides Function ToString() As String
        ToString = ""
        For y = 1 To mc_CrossWordSize.Height
            For x = 1 To mc_CrossWordSize.Width
                Dim cell = CellAtPoint(x, y, False)
                If cell Is Nothing Then
                    ToString &= Chr(8)
                Else
                    If cell.isUsed = False Then
                        ToString &= Chr(8)
                    Else
                        If cell.Value = "" Then
                            ToString &= " "
                        Else
                            ToString &= cell.Value
                        End If
                    End If
                End If
            Next
            If y <> mc_CrossWordSize.Height Then
                ToString &= vbCrLf
            End If
        Next
    End Function

    Public Sub DropUnusedCells(Optional ByVal OutOfRange As Boolean = True, Optional ByVal BlackCells As Boolean = False)
        If OutOfRange Then
            'find all the cells outside the grid and kill them
            Dim KillCells = (From xItem In CrossWordCells Where xItem.x > mc_CrossWordSize.Width OrElse xItem.y > mc_CrossWordSize.Height).ToArray
            For Each item In KillCells
                CrossWordCells.Remove(item)
            Next
        End If
        If BlackCells Then
            Dim KillCells = (From xItem In CrossWordCells Where xItem.isUsed = False).ToArray
            For Each item In KillCells
                CrossWordCells.Remove(item)
            Next
        End If

    End Sub

    Public ReadOnly Property CellAtPoint(ByVal x As Integer, ByVal y As Integer, Optional ByVal AutoCreate As Boolean = True, Optional ByVal FlipXY As Boolean = False) As CrossWordCell
        Get
            If FlipXY Then
                Dim z = x
                x = y
                y = z
            End If
            'Find the cell at this location
            Dim Cell = (From xItem In CrossWordCells Where xItem.x = x AndAlso xItem.y = y).FirstOrDefault
            If Cell Is Nothing AndAlso AutoCreate Then
                'add it first
                Cell = New CrossWordCell(x, y)
                CrossWordCells.Add(Cell)
            End If
            Return Cell
        End Get
    End Property

#Region "Generate"

    <NonSerialized()> _
    Dim UsedWords As HashSet(Of String)


    Dim AllWords As List(Of String)
    Public Sub Generate(ByVal Dictionary As FlatFileDictionary)
        Randomize()
        CrossWordCells.Clear()
        UsedWords = New HashSet(Of String)
        'initial word:
        'Put a random word on the board
        AllWords = Dictionary.IndexedDictionary.GetFullList
        Dim Word = (From xItem In AllWords Where xItem.Length <= Math.Max(CrossWordSize.Width, CrossWordSize.Height) And xItem.Length > 1 Order By Rnd()).FirstOrDefault
        'pick a point for this word on the grid
        If Word Is Nothing Then Exit Sub ' no words fit in the grid or dict empty...
        Dim across As Boolean
        If Word.Length > CrossWordSize.Height Then
            'must be across
            across = True
        ElseIf Word.Length > CrossWordSize.Width Then
            'must be down
            across = False
        Else
            'random :)
            across = Int(Rnd() * 2) = 1
        End If
        Dim WordPos As New Point()
        If across Then
            WordPos.Y = CInt(Int(Rnd() * CrossWordSize.Height) + 1)
            WordPos.X = CInt(Int(Rnd() * (CrossWordSize.Width - Word.Length)) + 1)
        Else
            WordPos.X = CInt(Int(Rnd() * CrossWordSize.Width) + 1)
            WordPos.Y = CInt(Int(Rnd() * (CrossWordSize.Height - Word.Length)) + 1)
        End If

        FillCellsWithWord(WordPos, Word, across)
        UsedWords.Add(LCase(Word))

        'Do Until AddWordToGrid(Dictionary) = False
        'Loop

        AddAllWordsToGrid(Dictionary)
    End Sub

    Public Class ProgressEventArgs
        Inherits EventArgs
        Public Progress As Double
        Public Sub New(ByVal Progress As Double)
            Me.Progress = Progress
        End Sub
    End Class

    Public Event Progress(ByVal sender As Object, ByVal e As ProgressEventArgs)

    Private Sub AddAllWordsToGrid(ByVal Dictionary As i00SpellCheck.FlatFileDictionary)
        Randomize()

        Dim WordsAdded As Boolean
        Dim TriedCells As New List(Of CrossWordCell)

TryAgain:
        WordsAdded = False
        Dim SearchCells = (From xItem In CrossWordCells Where TriedCells.Contains(xItem) = False AndAlso xItem.isUsed Order By Rnd()).ToArray
        Dim Progress = 1 - (SearchCells.Count / (TriedCells.Count + SearchCells.Count))
        RaiseEvent Progress(Me, New ProgressEventArgs(Progress))

        For Each item In SearchCells
            Dim ExtendDirection = CanExtend(item)
            If ExtendDirection <> ExtendDirections.None Then
                'this word can extend :D
                If AddSingleWordAt(Dictionary, item, ExtendDirection) = True Then
                    'word added
                    WordsAdded = True
                End If
            End If
        Next
        TriedCells.AddRange(SearchCells)

        If WordsAdded Then
            'new cells to check :D
            GoTo TryAgain
        End If
    End Sub

    Private Function AddSingleWordAt(ByVal Dictionary As i00SpellCheck.FlatFileDictionary, ByVal Cell As CrossWordCell, ByVal ExtendDirection As ExtendDirections) As Boolean
        'find the word bounds...
        If ExtendDirection = ExtendDirections.None Then Exit Function

        Dim IsX As Boolean = ExtendDirection = ExtendDirections.xBoth OrElse ExtendDirection = ExtendDirections.xLeft OrElse ExtendDirection = ExtendDirections.xRight

        'see how far we can extend left ...
        Dim LeftMostCell As Integer = If(IsX, Cell.x, Cell.y)
        Dim RightMostCell As Integer = LeftMostCell

        Dim CellConstant = If(IsX, Cell.y, Cell.x)
        Dim CellVariable = If(IsX, Cell.x, Cell.y)


        For x = If(IsX, Cell.x, Cell.y) - 1 To 1 Step -1
            'ok to use this if the next two cells to the left are not used and the cell directly above and below are empty
            If CellContent(x - 1, CellConstant, Not IsX) = eCellContent.Used AndAlso CellContent(x - 2, CellConstant, Not IsX) = eCellContent.Used Then
                'no good
                Exit For
            Else
                If CellContent(x, CellConstant, Not IsX) = eCellContent.Used Then
                    'we are part of a vertical word
                    LeftMostCell = x
                Else
                    If CellContent(x, CellConstant - 1, Not IsX) = eCellContent.Used OrElse CellContent(x, CellConstant + 1, Not IsX) = eCellContent.Used Then
                        'no good
                        Exit For
                    Else
                        LeftMostCell = x
                    End If
                End If
            End If
        Next

        For x = If(IsX, Cell.x, Cell.y) + 1 To If(IsX, mc_CrossWordSize.Width, mc_CrossWordSize.Height)
            'ok to use this if the next two cells to the right are not used and the cell directly above and below are empty
            If CellContent(x + 1, CellConstant, Not IsX) = eCellContent.Used AndAlso CellContent(x + 2, CellConstant, Not IsX) = eCellContent.Used Then
                'no good
                Exit For
            Else
                If CellContent(x, CellConstant, Not IsX) = eCellContent.Used Then
                    'we are part of a vertical word
                    RightMostCell = x
                Else
                    If CellContent(x, CellConstant - 1, Not IsX) = eCellContent.Used OrElse CellContent(x, CellConstant + 1, Not IsX) = eCellContent.Used Then
                        'no good
                        Exit For
                    Else
                        RightMostCell = x
                    End If
                End If
            End If
        Next

        'find all words that we can add
        Dim P1 As Point
        Dim P2 As Point
        If IsX Then
            P1 = New Point(LeftMostCell, CellConstant)
            P2 = New Point(RightMostCell, CellConstant)
        Else
            P1 = New Point(CellConstant, LeftMostCell)
            P2 = New Point(CellConstant, RightMostCell)
        End If

        Dim pattern As String = ""
        Dim Words = GetAllMatchingWords(Dictionary, P1, P2, Cell, pattern)
        Dim AddWordMatchInput As New AddWordMatchInput With {.Words = Words, .Cell = Cell, .CellVariable = CellVariable, .CellConstant = CellConstant, .LeftMostCell = LeftMostCell, .RightMostCell = RightMostCell, .IsX = IsX}

        '...I do the matches in a thread so i can abort the thread if it goes overtime :)
        AddWordMatchResult = Nothing
        Dim MT_AddWordMatch As New System.Threading.Thread(AddressOf AddWordMatch)
        Dim OrigWordCount = UsedWords.Count
        MT_AddWordMatch.Name = "Crossword generator AddWordMatch"
        MT_AddWordMatch.IsBackground = True
        MT_AddWordMatch.Start(AddWordMatchInput)
        Dim StartTime = Environment.TickCount
        Do Until MT_AddWordMatch.IsAlive = False
            If Environment.TickCount - StartTime > 1000 Then
                Debug.Print("Could not find a suitable match for " & Replace(Replace(pattern, "\w?", "*", , , CompareMethod.Text), "?", "").Trim("^"c, "$"c) & " in the given time (1 second)")
                MT_AddWordMatch.Abort()
                Exit Function
                'Exit Do
            End If
            Threading.Thread.Sleep(1)
        Loop

        If AddWordMatchResult IsNot Nothing Then
            'add to the crossword
            FillCellsWithWord(AddWordMatchResult.Point, AddWordMatchResult.Word, AddWordMatchResult.IsX)
            UsedWords.Add(LCase(AddWordMatchResult.Word))
            Return True
        End If

    End Function

    Dim AddWordMatchResult As AddWordMatchOutput
    Private Class AddWordMatchOutput
        Public Point As Point
        Public Word As String
        Public IsX As Boolean
    End Class

    Private Class AddWordMatchInput
        Public Words As IEnumerable(Of String)
        Public Cell As CrossWordCell
        Public CellVariable As Integer
        Public CellConstant As Integer
        Public LeftMostCell As Integer
        Public RightMostCell As Integer
        Public IsX As Boolean
    End Class

    Private Sub AddWordMatch(ByVal AddWordMatchInput As Object)
        Randomize()
        Dim awm = TryCast(AddWordMatchInput, AddWordMatchInput)
        If awm IsNot Nothing Then

            For Each word In awm.Words
                'we have our word... lets see where we can add it :D
                For i = 0 To word.Length - 1
                    If word(i) = awm.Cell.Value Then
                        'this is a matching letter... lets check the position...
                        If i <= (awm.CellVariable - awm.LeftMostCell) AndAlso (word.Length - i - 1) + awm.CellVariable <= awm.RightMostCell Then
                            Dim StartX = awm.CellVariable - i
                            Dim EndX = (awm.CellVariable + (word.Length - i)) - 1

                            'this is it?
                            For x = StartX To EndX
                                'check the other letters 1st :)
                                Dim xCellCheck = CellAtPoint(x, awm.CellConstant, False, Not awm.IsX)
                                If xCellCheck IsNot Nothing AndAlso xCellCheck.isUsed Then
                                    'check the letter
                                    If LCase(xCellCheck.Value) <> LCase(word(x - (awm.CellVariable - i))) Then
                                        'we have a filled in cell and it doesn't match :(
                                        GoTo NextPositionX
                                    End If
                                End If
                            Next
                            'and finially that the cells above and below are blank :)
                            If CellContent(StartX - 1, awm.CellConstant, Not awm.IsX) = eCellContent.Used OrElse CellContent(EndX + 1, awm.CellConstant, Not awm.IsX) = eCellContent.Used Then
                                'not this word
                                GoTo NextPositionX
                            End If

                            '...add the word :)
                            Dim P1 As New Point(0, 0)
                            If awm.IsX Then
                                P1 = New Point(StartX, awm.CellConstant)
                            Else
                                P1 = New Point(awm.CellConstant, StartX)
                            End If
                            Dim AddWordMatchOutput As New AddWordMatchOutput With {.Point = P1, .IsX = awm.IsX, .Word = word}
                            AddWordMatchResult = AddWordMatchOutput

                            Exit Sub
                        End If
                    End If
NextPositionX:
                Next

            Next
        End If
    End Sub

    Private Function GetAllMatchingWords(ByVal Dictionary As i00SpellCheck.FlatFileDictionary, ByVal WordStartCell As Point, ByVal WordEndCell As Point, ByVal RequiredCell As CrossWordCell, Optional ByRef Pattern As String = "") As IEnumerable(Of String)
        Randomize()

        Dim IsX As Boolean
        Dim StartCellVariable As Integer
        Dim EndCellVariable As Integer
        Dim CellConstent As Integer
        If WordStartCell.Y = WordEndCell.Y Then
            IsX = True
            StartCellVariable = WordStartCell.X
            EndCellVariable = WordEndCell.X
            CellConstent = WordStartCell.Y
        ElseIf WordStartCell.X = WordEndCell.X Then
            IsX = False
            StartCellVariable = WordStartCell.Y
            EndCellVariable = WordEndCell.Y
            CellConstent = WordStartCell.X
        Else
            Throw New Exception("GetAllMatchingWords failed, start and end cells must be horizontally or vertically aligned")
        End If

        'trim the dictionary down to the length that we want...
        Dim Dict = (From xItem In AllWords Where InStr(xItem, RequiredCell.Value, CompareMethod.Text) > 0 AndAlso xItem.Length < ((EndCellVariable - StartCellVariable) + 1) AndAlso xItem.Length > 1 AndAlso UsedWords.Contains(LCase(xItem)) = False Order By Rnd()).ToArray
        Pattern = "^"
        For x = StartCellVariable To EndCellVariable
            Dim cell = CellAtPoint(x, CellConstent, False, Not IsX)
            If cell Is Nothing OrElse cell.isUsed = False Then
                'blank
                Pattern &= "\w"
            ElseIf cell.isUsed = True Then
                'white cell
                If cell.Value <> "" AndAlso Char.IsLetter(cell.Value) Then
                    If cell Is RequiredCell Then
                        Pattern &= UCase(cell.Value)
                    Else
                        Pattern &= LCase(cell.Value)
                    End If
                Else
                    Pattern &= "\w"
                End If
            End If
            If cell Is RequiredCell Then
                'this is required
            Else
                Pattern &= "?"
            End If
        Next
        Pattern &= "$"
        Dim thePattern = Pattern
        Return (From xItem In Dict Where System.Text.RegularExpressions.Regex.IsMatch(xItem, thePattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))

    End Function

    Private Enum ExtendDirections
        None
        xBoth
        yBoth
        xLeft
        xRight
        yUp
        yDown
    End Enum
    Private Function CanExtend(ByVal CrossWordCell As CrossWordCell) As ExtendDirections
        'Search to see if we can build words off a cell :)
        'search for up down...
        Dim SurroundingCells() As eCellContent = {CellContent(CrossWordCell.x - 1, CrossWordCell.y - 1), CellContent(CrossWordCell.x, CrossWordCell.y - 1), CellContent(CrossWordCell.x + 1, CrossWordCell.y - 1), _
                                                  CellContent(CrossWordCell.x - 1, CrossWordCell.y), CellContent(CrossWordCell.x, CrossWordCell.y), CellContent(CrossWordCell.x + 1, CrossWordCell.y), _
                                                  CellContent(CrossWordCell.x - 1, CrossWordCell.y + 1), CellContent(CrossWordCell.x, CrossWordCell.y + 1), CellContent(CrossWordCell.x + 1, CrossWordCell.y + 1)}
        If SurroundingCells(3) = eCellContent.Used OrElse SurroundingCells(5) = eCellContent.Used Then
            'can only extend up or down... maybe.. y
            If SurroundingCells(1) = eCellContent.Used OrElse SurroundingCells(7) = eCellContent.Used Then
                'we are used in updown word
                Return ExtendDirections.None
            End If
            CanExtend = ExtendDirections.yBoth
            If SurroundingCells(1) = eCellContent.OffBoard OrElse (SurroundingCells(0) = eCellContent.Used OrElse SurroundingCells(2) = eCellContent.Used) Then
                'can't extend up
                CanExtend = ExtendDirections.yDown
            End If
            If SurroundingCells(7) = eCellContent.OffBoard OrElse (SurroundingCells(6) = eCellContent.Used OrElse SurroundingCells(8) = eCellContent.Used) Then
                'can't extend down
                If CanExtend = ExtendDirections.yDown Then
                    'can't extend at all
                    CanExtend = ExtendDirections.None
                Else
                    'can extend up
                    CanExtend = ExtendDirections.yUp
                End If
            End If
        ElseIf SurroundingCells(1) = eCellContent.Used OrElse SurroundingCells(7) = eCellContent.Used Then
            'can only extend left or right... maybe.. x
            'Debug.Print(Me.ToString & CrossWordCell.Value)
            CanExtend = ExtendDirections.xBoth
            If SurroundingCells(3) = eCellContent.OffBoard OrElse (SurroundingCells(0) = eCellContent.Used OrElse SurroundingCells(6) = eCellContent.Used) Then
                'can't extend left
                CanExtend = ExtendDirections.xRight
            End If
            If SurroundingCells(5) = eCellContent.Used OrElse (SurroundingCells(2) = eCellContent.Used OrElse SurroundingCells(8) = eCellContent.Used) Then
                'can't extend right
                If CanExtend = ExtendDirections.xRight Then
                    'can't extend at all
                    CanExtend = ExtendDirections.None
                Else
                    'can extend left
                    CanExtend = ExtendDirections.xLeft
                End If
            End If
        End If

    End Function

    Private Enum eCellContent
        OffBoard
        Used
        Black
    End Enum

    Private Function CellContent(ByVal x As Integer, ByVal y As Integer, Optional ByVal FlipXY As Boolean = False) As eCellContent
        If FlipXY Then
            Dim z = x
            x = y
            y = z
        End If
        Dim cell = CellAtPoint(x, y, False)
        If x < 1 OrElse y < 1 Then Return eCellContent.OffBoard
        If x > CrossWordSize.Width OrElse y > CrossWordSize.Height Then Return eCellContent.OffBoard

        If cell Is Nothing OrElse cell.isUsed = False Then
            Return eCellContent.Black
        Else
            Return eCellContent.Used
        End If
    End Function

    Public Class WordLocationEventArgs
        Inherits EventArgs
        Public Word As String
        Public CellStart As Point
        Public Across As Boolean
        Public Sub New(ByVal Word As String, ByVal CellStart As Point, ByVal Across As Boolean)
            Me.Word = Word
            Me.CellStart = CellStart
            Me.Across = Across
        End Sub
    End Class

    Public Event FilledCellsWithWord(ByVal sender As Object, ByVal e As WordLocationEventArgs)

    Private Sub FillCellsWithWord(ByVal p As Point, ByVal word As String, ByVal across As Boolean)
        RaiseEvent FilledCellsWithWord(Me, New WordLocationEventArgs(word, p, across))
        Dim pAt = New Point(p.X, p.Y)
        For Each iLetter In word
            Dim cell = CellAtPoint(pAt.X, pAt.Y)
            cell.isUsed = True
            cell.Value = iLetter
            If across Then
                pAt.X += 1
            Else
                pAt.Y += 1
            End If
        Next
    End Sub

#End Region

    <Serializable()> _
    Public Class CrossWordCell
        Public x As Integer
        Public y As Integer
        Public isUsed As Boolean
        Public Value As Char
        Public Sub New(ByVal x As Integer, ByVal y As Integer, ByVal Value As Char)
            Me.x = x
            Me.y = y
            Me.Value = Value
            isUsed = True
        End Sub
        Public Sub New(ByVal x As Integer, ByVal y As Integer)
            Me.x = x
            Me.y = y
        End Sub
    End Class

    Public Class CrossWordDrawingFunctions
        Dim CrossWordGenerator As CrossWordGenerator
        Public Sub New(ByVal CrossWordGenerator As CrossWordGenerator)
            Me.CrossWordGenerator = CrossWordGenerator
        End Sub

        Public CellSize As Integer = 32

        Public ReadOnly Property BoardSize() As Size
            Get
                Return New Size(((CellSize + 1) * CrossWordGenerator.CrossWordSize.Width) + 1, ((CellSize + 1) * CrossWordGenerator.CrossWordSize.Height) + 1)
            End Get
        End Property

        Public ReadOnly Property CellRect(ByVal CrossWordCell As CrossWordCell) As Rectangle
            Get
                Return New Rectangle(1 + ((CellSize + 1) * (CrossWordCell.x - 1)), 1 + ((CellSize + 1) * (CrossWordCell.y - 1)), CellSize, CellSize)
            End Get
        End Property

        Public Sub Draw(ByVal g As Graphics)
            'exclude the cells
            Dim XWordSize = BoardSize
            Dim origClip = g.Clip.Clone
            Using pCells As New Drawing2D.GraphicsPath
                Dim UsedCells = (From xItem In CrossWordGenerator.CrossWordCells Where xItem.isUsed = True Select New With {.Rect = CellRect(xItem), .Item = xItem}).ToArray
                For Each item In UsedCells
                    Dim CellRect = item.Rect
                    pCells.AddRectangle(CellRect)
                    'e.Graphics.FillRectangle(Brushes.White, CellRect)
                Next
                Using rClip As New Region(pCells)
                    g.Clip = rClip
                    'paint black bg
                    Dim CellColor = i00SpellCheck.DrawingFunctions.AlphaColor(Color.White, 223)
                    Using b As New SolidBrush(CellColor)
                        g.FillRectangle(b, New Rectangle(New Point(0, 0), XWordSize))
                    End Using
                    g.Clip = origClip
                    g.ExcludeClip(rClip)

                    'paint black bg
                    Dim BlackColor = i00SpellCheck.DrawingFunctions.AlphaColor(Color.Black, 131)
                    Using b As New SolidBrush(BlackColor)
                        g.FillRectangle(b, New Rectangle(New Point(0, 0), XWordSize))
                    End Using

                    g.Clip = origClip

                    Dim OldTransform = g.Transform.Clone
                    'paint letters
                    'g.TranslateTransform(1 / FontSizeRatio, 1 / FontSizeRatio)
                    'g.ScaleTransform(FontSizeRatio, FontSizeRatio)
                    For Each item In UsedCells
                        Dim FontSize = g.MeasureString(UCase(item.Item.Value), Drawing.SystemFonts.DefaultFont)
                        Dim Ratio = CellSize / FontSize.Height
                        DrawingFunctions.DrawString(g, UCase(item.Item.Value), Drawing.SystemFonts.DefaultFont, Brushes.Black, item.Rect.X + ((CellSize - (FontSize.Width * Ratio)) / 2), item.Rect.Y, Ratio)
                    Next
                    'g.Transform = OldTransform
                End Using
            End Using
        End Sub

    End Class

End Class
