using System.Security.AccessControl;

namespace GameBrain;

public class Board
{
    public static readonly char[] AlphabetChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    private int? Id { get; }
    
    public Player PlayerOne { get; }
    public Player PlayerTwo { get; }

    public Square[] Squares { get; }
    public int Width { get; }
    public int Height { get; }

    public Board(int width, int height)
    {
        Squares = new Square[height * width];
        PlayerOne = new Player("p1", EColor.White);
        PlayerTwo = new Player("p2", EColor.Black);
        Width = width;
        Height = height;
        InitializeBoard();
    }

    public Board(Domain.Board dBoard)
    {
        if (dBoard.PlayerOne == null || dBoard.PlayerTwo == null || dBoard.Squares == null)
        {
            throw new ArgumentException(
                "Unable to initialize board from Domain object - please ensure all related entities are queried.");
        }
        Id = dBoard.Id;
        PlayerOne = new Player(dBoard.PlayerOne);
        PlayerTwo = new Player(dBoard.PlayerTwo);
        Squares = dBoard.Squares.Select(s => new Square(s)).ToArray();
        Width = dBoard.Width;
        Height = dBoard.Height;
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
                    var button = new Button(PlayerOne.Color, EButtonState.OnBoard);
                    square.Button = button;
                } else if (Height % 2 == 1 && i + 1 > Height / 2 + 2 || 
                           Height % 2 == 0 && i + 1 > Height / 2 + 1)
                {
                    var button = new Button(PlayerTwo.Color, EButtonState.OnBoard);
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

    public Domain.Board ToDomainBoard(bool forJson)
    {
        var dBoard = new Domain.Board();
        if (Id != null)
        {
            dBoard.Id = (int) Id;
        }
        dBoard.Height = Height;
        dBoard.Width = Width;
        ICollection<Domain.Square> squaresCollection = Squares.Select(s => s.ToDomainSquare(dBoard, forJson)).ToArray();
        dBoard.Squares = squaresCollection;
        dBoard.PlayerOne = PlayerOne.ToDomainPlayer();
        dBoard.PlayerTwo = PlayerTwo.ToDomainPlayer();
        return dBoard;
    }
}