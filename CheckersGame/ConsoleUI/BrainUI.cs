using System.Text;
using GameBrain;
using GameBrain.Board;

namespace ConsoleUI;

public class BrainUI
{
    public ConsoleColor BoardPrimarySquareColor => ConsoleColor.Gray;
    public ConsoleColor BoardSecondarySquareColor => ConsoleColor.White;
    public ConsoleColor PlayerOneButtonColor => ConsoleColor.White;
    public ConsoleColor PlayerTwoButtonColor => ConsoleColor.Black;
    
    private readonly BaseUI _base;
    private readonly CheckersBrain _brain;

    public BrainUI(BaseUI b, CheckersBrain brain)
    {
        _base = b;
        _brain = brain;
    }

    public string PrintBoard()
    {
        Console.Clear();
        WriteBoardAlphaCoordinates();
        var squares = _brain.Board.Squares;
        var rowCount = squares.GetLength(0);
        for (var i = rowCount; i > 0; i--)
        {
            Console.ResetColor();
            Console.ForegroundColor = _base.MainColor;
            Console.Write((i == rowCount && i > 9 ? "" : " ") +
                          (i == rowCount ? i : i + 1) + 
                          (i != rowCount && i > 8 ? "" : " "));
            if (i < rowCount)
            {
                Console.Write($"\n{(i > 9 ? "" : " ")}{i} ");
            } 
            for (var j = 0; j < squares.GetLength(1); j++)
            {
                var square = squares[rowCount - i, j];
                if (i % 2 == 0 && j % 2 == 1 ||
                    i % 2 == 1 && j % 2 == 0)
                {
                    Console.BackgroundColor = BoardPrimarySquareColor;
                }
                else
                {
                    Console.BackgroundColor = BoardSecondarySquareColor;
                }

                Console.ForegroundColor = square.State.Equals(ESquareState.White) ?
                    PlayerOneButtonColor : PlayerTwoButtonColor;
                Console.Write(square.State.Equals(ESquareState.Empty) ? "  " : "()");
            }
        }
        Console.ResetColor();
        Console.ForegroundColor = _base.MainColor;
        Console.Write($" 1 \n");
        WriteBoardAlphaCoordinates();
        
        Console.Write("\nYour choice: ");
        Console.ReadLine();
        return "";
    }

    private void WriteBoardAlphaCoordinates()
    {
        Console.ForegroundColor = _base.MainColor;
        StringBuilder sb = new StringBuilder();
        Board board = _brain.Board;

        sb.Append("   ");
        for (int i = 0; i < board.Squares.GetLength(1); i++)
        {
            sb.Append($"{board.AlphabetChars[i]} ");
        }

        sb.Append("   ");
        Console.WriteLine(sb.ToString());
    }
}