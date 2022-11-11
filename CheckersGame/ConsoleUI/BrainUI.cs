using System.Text;
using GameBrain;

namespace ConsoleUI;

public class BrainUI
{
    public ConsoleColor BoardPrimarySquareColor => ConsoleColor.Gray;
    public ConsoleColor BoardSecondarySquareColor => ConsoleColor.White;
    public ConsoleColor PlayerOneButtonColor => ConsoleColor.White;
    public ConsoleColor PlayerTwoButtonColor => ConsoleColor.Black;
    
    private readonly UIController _base;
    public CheckersBrain Brain { get; }

    public BrainUI(UIController b, CheckersBrain brain)
    {
        _base = b;
        Brain = brain;
    }

    public string PlayGame()
    {
        var done = false;
        while (!done)
        {
            var coords = AskButtonScreen();
            if (coords == null)
            {
                return "";
            }
        }
        return "";
    }

    private SquareCoordinates? AskButtonScreen()
    {
        var clearConsole = true;
        do
        {
            PrintBoard(clearConsole);
            var input = ButtonInput();
            if (input != null && input.Trim().ToUpper().Equals("X"))
            {
                return null;
            }
            var parsed = Brain.Board.TryParseCoordinate(input, out var coords);
            if (!parsed)
            {
                if (input == null || input.Length.Equals(0))
                {
                    _base.PrintError($"Coordinates cannot be empty.", false);
                }
                else
                {
                    _base.PrintError($"{input} are not valid coordinates", false);
                }
                clearConsole = false;
            }
            else
            {
                return coords;
            }
        } while (true);
    }
    
    private string? ButtonInput()
    {
        Console.ForegroundColor = _base.MainColor;
        Console.WriteLine("'X' to exit.");
        Console.Write("Choose button: ");
        return Console.ReadLine()?.Trim();
    }

    private void PrintBoard(bool clearConsole)
    {
        if (clearConsole)
        {
            Console.Clear();
        }
        WriteBoardAlphaCoordinates();

        var squares = Brain.Board.Squares;
        var rowCount = squares.GetLength(0);
        var columnCount = squares.GetLength(1);

        for (var i = rowCount; i > 0; i--)
        {
            for (int k = 0; k < 3; k++)
            {
                if (k != 1)
                {
                    Console.Write("   ");
                }
                if (k == 1)
                {
                    WriteLeftNumericCoordinate(rowCount, i);
                }
                for (var j = 0; j < columnCount; j++)
                {
                    var coords = new SquareCoordinates(Board.AlphabetChars[j], i);
                    var square = squares[rowCount - i, j];
                    
                    Console.BackgroundColor = Brain.Board.IsButtonSquare(coords) ?
                        BoardPrimarySquareColor : BoardSecondarySquareColor;
                    if (k == 1 && square.Button != null)
                    {
                        Console.ForegroundColor = square.Button.Color.Equals(EColor.White)
                            ? PlayerOneButtonColor
                            : PlayerTwoButtonColor;
                        Console.Write("  ⬤   ");
                    }
                    else
                    {
                        Console.Write("      ");
                    }
                }
                if (k == 1)
                {
                    WriteRightNumericCoordinate(rowCount, i);
                }

                Console.WriteLine();
                Console.ResetColor();
            }
        }
        Console.ResetColor();
        Console.ForegroundColor = _base.MainColor;
        
        WriteBoardAlphaCoordinates();
        Console.WriteLine();
    }

    private void WriteLeftNumericCoordinate(int boardHeight, int currentIndex) =>
        WriteBoardNumericCoordinate(boardHeight, currentIndex, true);
    
    private void WriteRightNumericCoordinate(int boardHeight, int currentIndex) =>
        WriteBoardNumericCoordinate(boardHeight, currentIndex, false);

    private void WriteBoardNumericCoordinate(int boardHeight, int currentIndex, bool isLeft)
    {
        Console.ResetColor();
        Console.ForegroundColor = _base.MainColor;
        Console.Write((isLeft && currentIndex > 9 ? "" : " ") + currentIndex + " ");
    }

    private void WriteBoardAlphaCoordinates()
    {
        Console.ForegroundColor = _base.MainColor;
        
        var sb = new StringBuilder();
        sb.Append("   ");
        
        for (var i = 0; i < Brain.Board.Squares.GetLength(1); i++)
        {
            sb.Append($"   {Board.AlphabetChars[i]}  ");
        }

        sb.Append("   ");
        Console.WriteLine(sb);
    }
}