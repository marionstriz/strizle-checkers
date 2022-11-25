using System.Security.AccessControl;
using System.Text;
using GameBrain;
using Board = GameBrain.Board;
using CheckersGame = GameBrain.CheckersGame;

namespace ConsoleUI;

public class GameUI
{
    public ConsoleColor BoardPrimarySquareColor => ConsoleColor.Gray;
    public ConsoleColor BoardSecondarySquareColor => ConsoleColor.White;
    public ConsoleColor PlayerOneButtonColor => ConsoleColor.White;
    public ConsoleColor PlayerTwoButtonColor => ConsoleColor.Black;
    
    private readonly UIController _base;
    public CheckersGame Game { get; }

    public GameUI(UIController b, CheckersGame game)
    {
        _base = b;
        Game = game;
    }

    public char PlayGame()
    {
        Console.CursorVisible = true;
        var done = false;
        while (!done)
        {
            var coords = AskButtonScreen();
            if (coords == null)
            {
                return ' ';
            }
        }
        return ' ';
    }

    private SquareCoordinates? AskButtonScreen()
    {
        var firstTime = true;
        do
        {
            if (firstTime)
            {
                PrintBoard();
                firstTime = false;
            }
            else PrintBoard(false);
            
            var input = ButtonInput();
            if (input == null) return null;
            
            var parsed = Game.Board.TryParseCoordinate(input, out var coords);
            
            if (!parsed) {
                _base.PrintError(input.Length.Equals(0)
                    ? $"Coordinates cannot be empty."
                    : $"{input} are not valid coordinates");
                continue;
            }
            var currentPlayer = Game.PlayerOne.IsCurrent ? Game.PlayerOne : Game.PlayerTwo;
                
            if (Game.Board.IsPlayerButtonSquare(coords!, currentPlayer)) return coords;
                
            _base.PrintError($"The chosen square does not hold your button.");
        } while (true);
    }
    
    private string? ButtonInput()
    {
        return _base.AskForInput("Please enter button coordinates in the format" +
                          " {letter}{number}, eg. A1\nChoose button: ", false);
    }

    private void PrintBoard(bool clearConsole = true)
    {
        if (clearConsole) Console.Clear();
        WriteBoardAlphaCoordinates();

        var squares = Game.Board.Squares;
        var rowCount = Game.Board.Height;
        var columnCount = Game.Board.Width;

        for (var i = rowCount; i > 0; i--)
        {
            for (int k = 0; k < 3; k++)
            {
                if (k != 1) Console.Write("   ");
                if (k == 1) WriteLeftNumericCoordinate(i);
                
                for (var j = 0; j < columnCount; j++)
                {
                    var coords = new SquareCoordinates(Board.AlphabetChars[j], i);
                    var square = squares[(rowCount-i)*columnCount + j];
                    
                    Console.BackgroundColor = Game.Board.IsButtonSquare(coords)
                        ? BoardPrimarySquareColor 
                        : BoardSecondarySquareColor;
                    
                    if (k == 1 && square.Button != null)
                    {
                        Console.ForegroundColor = square.Button.Color.Equals(EColor.White)
                            ? PlayerOneButtonColor
                            : PlayerTwoButtonColor;
                        Console.Write("  ⬤   ");
                    }
                    else Console.Write("      ");
                }
                if (k == 1) WriteRightNumericCoordinate(i);
                
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
        
        for (var i = 0; i < Game.Board.Width; i++)
        {
            sb.Append($"   {Board.AlphabetChars[i]}  ");
        }

        sb.Append("   ");
        Console.WriteLine(sb);
    }
}