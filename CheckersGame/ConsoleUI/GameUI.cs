using System.Security.AccessControl;
using System.Text;
using DAL;
using GameBrain;
using Board = GameBrain.Board;
using CheckersGame = GameBrain.CheckersGame;

namespace ConsoleUI;

public class GameUI
{
    public ConsoleColor BoardPrimarySquareColor => ConsoleColor.Gray;
    public ConsoleColor BoardSecondarySquareColor => ConsoleColor.White;
    public ConsoleColor BoardHighlightSquareColor => ConsoleColor.Green;
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
            if (Game.GameWonByPlayer != null)
            {
                done = true;
                CelebrateGoodTimesComeOn();
            }
            var currentPlayer = Game.PlayerOne.IsCurrent ? Game.PlayerOne : Game.PlayerTwo;
            var movesMap = Game.Board.GetCurrentPossibleMoves(currentPlayer, Game.GameOptions.CompulsoryJumps);

            int? moveSquare;
            List<Move>? movesList;
            if (Game.NextMoves == null)
            {
                var buttonSquare = AskForAnyValidSquare(movesMap.Keys.ToArray());
                if (buttonSquare == null) return ' ';

                var gotValue = movesMap.TryGetValue(buttonSquare.Value, out movesList);
                
                if (gotValue) moveSquare = AskForAnyValidSquare(
                    movesList!.ConvertAll(m => m.Destination).ToArray(), buttonSquare);
                else throw new ApplicationException(
                        "Cannot find chosen square with valid moves, please contact administrator.");
            }
            else
            {
                moveSquare = AskForAnyValidSquare(
                    Game.NextMoves.Select(m => m.Destination).ToArray(), Game.NextMoves[0].Source);
                movesList = Game.NextMoves;
            } 
            if (moveSquare == null) return ' ';
            foreach (var move in movesList.Where(move => moveSquare.Equals(move.Destination)))
            {
                Game.MakeMove(move);
                break;
            }
        }
        return ' ';
    }

    private void CelebrateGoodTimesComeOn()
    {
        _base.AskForInput($"Congratulations, {Game.GameWonByPlayer} has won the game!");
    }

    private int? AskForAnyValidSquare(int[] validSquares, int? moveButtonSquare = null)
    {
        var firstTime = true;
        do
        {
            if (firstTime)
            {
                PrintBoard(validSquares, highlightButtonSquare:moveButtonSquare);
                firstTime = false;
            }
            else PrintBoard(validSquares, moveButtonSquare, false);
            
            var input = SquareInput();
            if (input == null) return null;
            
            var parsed = Game.Board.TryParseCoordinate(input, out var coords);
            
            if (!parsed) {
                _base.PrintError(input.Length.Equals(0)
                    ? $"Coordinates cannot be empty."
                    : $"{input} are not valid coordinates");
                continue;
            }
            foreach (var squareIndex in validSquares)
            {
                if (Game.Board.Squares[squareIndex].Coordinates.Equals(coords)) return squareIndex;
            }
            _base.PrintError($"Please choose a valid square.");
        } while (true);
    }
    
    private string? SquareInput()
    {
        return _base.AskForInput("Please enter square coordinates in the format" +
                          " {letter}{number}, eg. A1\nChoose a highlighted square: ", false);
    }

    private void PrintBoard(int[] highlightSquares, 
        int? highlightButtonSquare = null, bool clearConsole = true)
    {
        if (clearConsole) Console.Clear();
        
        var currentPlayer = Game.PlayerOne.IsCurrent ? Game.PlayerOne : Game.PlayerTwo;

        Console.ForegroundColor = _base.MainColor;
        Console.WriteLine($"Current turn: {currentPlayer.Name} ({currentPlayer.Color})\n");
        
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
                    var squareIndex = (rowCount - i) * columnCount + j;
                    var square = squares[squareIndex];
                    var squareHighlighted = highlightSquares.Any(s => s.Equals((rowCount-i)*columnCount + j));
                    
                    Console.BackgroundColor = square.IsMovableSquare()
                        ? squareHighlighted ? BoardHighlightSquareColor : BoardPrimarySquareColor
                        : BoardSecondarySquareColor;
                    
                    if (k == 1 && square.Button != null)
                    {
                        Console.ForegroundColor = squareIndex.Equals(highlightButtonSquare) ?
                                BoardHighlightSquareColor : square.Button.Color.Equals(EColor.White)
                                ? PlayerOneButtonColor
                                : PlayerTwoButtonColor;
                        var buttonString = square.Button.State.Equals(EButtonState.Supermario) ? " 𒁎    " : "  ⬤   ";
                        Console.Write(buttonString);
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