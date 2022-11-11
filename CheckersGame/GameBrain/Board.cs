using System.Text;

namespace GameBrain;

public class Board
{
    public static readonly char[] AlphabetChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    
    public Player PlayerOne { get; }
    public Player PlayerTwo { get; }

    public Square[,] Squares { get; }

    public Board(int width, int height)
    {
        Squares = new Square[height, width];
        PlayerOne = new Player("p1", EColor.White);
        PlayerTwo = new Player("p2", EColor.Black);
        
        InitializeBoard();
    }

    public Board(Player one, Player two, Square[,] squares)
    {
        PlayerOne = one;
        PlayerTwo = two;
        Squares = squares;
    }

    private void InitializeBoard()
    {
        int height = Squares.GetLength(0);
        int width = Squares.GetLength(1);
        
        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                var coords = new SquareCoordinates(AlphabetChars[j], height - i);
                var square = new Square(coords);
                
                Squares[i, j] = square;
                if (IsButtonSquare(coords))
                {
                    if (i + 1 < height / 2)
                    {
                        var button = new Button(PlayerOne.Color, EButtonState.OnBoard);
                        square.Button = button;
                    } else if (i + 1 > height / 2 + 1)
                    {
                        var button = new Button(PlayerTwo.Color, EButtonState.OnBoard);
                        square.Button = button;
                    }
                }
            }
        }
    }

    public bool TryParseCoordinate(string? inCoordinates, out SquareCoordinates? outCoords)
    {
        outCoords = null;
        if (inCoordinates == null || inCoordinates.Trim().Length is < 2 or > 3 )
        {
            return false;
        }
        inCoordinates = inCoordinates.Trim();
        var x = ' ';
        for (var i = 0; i < Squares.GetLength(1); i++)
        {
            var alphaCoord = inCoordinates[0];
            if (AlphabetChars[i].CompareTo(alphaCoord) == 0)
            {
                x = alphaCoord;
                break;
            }
        }
        if (x == ' ')
        {
            return false;
        }
        var numCoord = inCoordinates[1..].Trim();
        var parsed = Int32.TryParse(numCoord, out var num);
        if (!parsed)
        {
            return false;
        }
        var y = 0;
        for (var i = 1; i <= Squares.GetLength(0); i++)
        {
            if (num == i)
            {
                y = num;
                break;
            }
        }
        outCoords = new SquareCoordinates(x, y);
        return y != 0;
    }
    
    //TODO: IsWithButton
    //TODO: PlayerCheck

    public bool IsButtonSquare(SquareCoordinates coords)
    {
        return coords.X % 2 == 0 && coords.Y % 2 == 1
               || coords.X % 2 == 1 && coords.Y % 2 == 0;
    }
}