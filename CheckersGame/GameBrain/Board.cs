using System.Security.AccessControl;
using System.Text.Json;
using System.Text.Json.Serialization;
using DAL.DTO;

namespace GameBrain;

public class Board
{
    public static readonly char[] AlphabetChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    
    public Square[] Squares { get; }
    public int Width { get; }
    public int Height { get; }

    public Board(int width, int height)
    {
        Squares = new Square[height * width];
        Width = width;
        Height = height;
        InitializeBoard();
    }

    public Board(int width, int height, string serializedState)
    {
        Width = width;
        Height = height;
        var deserializedState = JsonSerializer.Deserialize<Square[]>(serializedState);
        Squares = deserializedState ?? throw new ArgumentException("Unable to deserialize game state.");
    }
    
    private void InitializeBoard()
    {
        for (var i = 0; i < Height; i++)
        {
            for (var j = 0; j < Width; j++)
            {
                var coords = new SquareCoordinates(AlphabetChars[j], Height - i);
                var square = new Square(coords);
                
                Squares[i*Width+j] = square;
                
                if (!IsButtonSquare(coords)) continue;
                
                if (i + 1 < Height / 2)
                {
                    var button = new Button(EColor.White, EButtonState.OnBoard);
                    square.Button = button;
                } else if (Height % 2 == 1 && i + 1 > Height / 2 + 2 || 
                           Height % 2 == 0 && i + 1 > Height / 2 + 1)
                {
                    var button = new Button(EColor.Black, EButtonState.OnBoard);
                    square.Button = button;
                }
            }
        }
    }

    public bool TryParseCoordinate(string? inCoordinates, out SquareCoordinates? outCoords)
    {
        outCoords = null;
        if (inCoordinates == null || inCoordinates.Trim().Length is < 2 or > 3)
        {
            return false;
        }
        inCoordinates = inCoordinates.Trim();
        var x = ' ';
        for (var i = 0; i < Width; i++)
        {
            var alphaCoord = char.ToUpper(inCoordinates[0]);
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
        var numCoord = inCoordinates[1..];
        var parsed = Int32.TryParse(numCoord, out var num);
        if (!parsed)
        {
            return false;
        }
        var y = 0;
        for (var i = 1; i <= Height; i++)
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

    public bool IsPlayerButtonSquare(SquareCoordinates coords, Player player)
    {
        var square = GetSquareWithCoords(coords);
        Console.WriteLine(player.Color);
        if (square?.Button == null)
        {
            return false;
        }
        return square.Button.Color.Equals(player.Color);
    }

    public bool IsButtonSquare(SquareCoordinates coords)
    {
        return coords.X % 2 == 0 && coords.Y % 2 == 1
               || coords.X % 2 == 1 && coords.Y % 2 == 0;
    }

    private Square? GetSquareWithCoords(SquareCoordinates coords)
    {
        foreach (var square in Squares)
        {
            if (square.Coordinates.Equals(coords))
            {
                return square;
            }
        }
        return null;
    }
}