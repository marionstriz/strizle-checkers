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

    public string PrintBoard()
    {
        Console.WriteLine(GetHashCode());
        Console.Clear();
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
        
        Console.Write("Your choice: ");
        Console.ReadLine();
        return "";
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