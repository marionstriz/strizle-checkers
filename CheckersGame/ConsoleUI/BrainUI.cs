using System.Security.AccessControl;
using System.Text;
using Domain;
using GameBrain;
using Board = GameBrain.Board;
using CheckersBrain = GameBrain.CheckersBrain;
using Player = GameBrain.Player;

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
        var firstTime = true;
        var board = Brain.Board;
        do
        {
            if (firstTime)
            {
                PrintBoard();
                firstTime = false;
            }
            else
            {
                PrintBoard(false);
            }
            var input = ButtonInput();
            if (input != null && input.Trim().ToUpper().Equals("X"))
            {
                return null;
            }
            var parsed = board.TryParseCoordinate(input, out var coords);
            if (!parsed)
            {
                if (input == null || input.Length.Equals(0))
                {
                    _base.PrintError($"Coordinates cannot be empty.");
                }
                else
                {
                    _base.PrintError($"{input} are not valid coordinates");
                }
            }
            else
            {
                Player currentPlayer = board.PlayerOne.IsCurrent ? board.PlayerOne : board.PlayerTwo;
                if (board.IsPlayerButtonSquare(coords!, currentPlayer))
                {
                    return coords;
                }
                _base.PrintError($"The chosen square does not hold your button.");
            }
        } while (true);
    }
    
    private string? ButtonInput()
    {
        Console.ForegroundColor = _base.MainColor;
        Console.WriteLine("'X' to exit.");
        Console.WriteLine("Please enter button coordinates in the format {letter}{number}, eg. A1");
        Console.Write("Choose button: ");
        return Console.ReadLine()?.Trim();
    }

    private void PrintBoard(bool clearConsole = true)
    {
        if (clearConsole)
        {
            Console.Clear();
        }
        WriteBoardAlphaCoordinates();

        var squares = Brain.Board.Squares;
        var rowCount = Brain.Board.Height;
        var columnCount = Brain.Board.Width;

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
                    WriteLeftNumericCoordinate(i);
                }
                for (var j = 0; j < columnCount; j++)
                {
                    var coords = new SquareCoordinates(Board.AlphabetChars[j], i);
                    var square = squares[(rowCount-i)*columnCount + j];
                    
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
                    WriteRightNumericCoordinate(i);
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

    private void WriteLeftNumericCoordinate(int currentIndex) =>
        WriteBoardNumericCoordinate(currentIndex, true);
    
    private void WriteRightNumericCoordinate(int currentIndex) =>
        WriteBoardNumericCoordinate(currentIndex, false);

    private void WriteBoardNumericCoordinate(int currentIndex, bool isLeft)
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
        
        for (var i = 0; i < Brain.Board.Width; i++)
        {
            sb.Append($"   {Board.AlphabetChars[i]}  ");
        }

        sb.Append("   ");
        Console.WriteLine(sb);
    }
}