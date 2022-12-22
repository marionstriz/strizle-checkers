using System.Text.Json;

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
                
                if (!square.IsMovableSquare()) continue;
                
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

    public int CountButtonsWithColor(EColor color)
    {
        return Squares.Count(square => square.HasButton() && square.Button!.Color.Equals(color));
    }

    public bool TryParseCoordinate(string? inCoordinates, out SquareCoordinates? outCoords)
    {
        outCoords = null;
        if (inCoordinates == null || inCoordinates.Trim().Length is < 2 or > 3) return false;
        
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
        if (x == ' ') return false;
        
        var numCoord = inCoordinates[1..];
        var parsed = Int32.TryParse(numCoord, out var num);
        if (!parsed) return false;
        
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

    public Dictionary<int, List<Move>> GetCurrentPossibleMoves(Player player, bool jumpsCompulsory)
    {
        var jumpsDict = new Dictionary<int, List<Move>>();
        var allDict = new Dictionary<int, List<Move>>();
        var onlyJumps = false;
        var color = player.Color;
        
        for (var i = 0; i < Squares.Length; i++)
        {
            var curr = Squares[i];
            if (!curr.IsColorButtonSquare(color)) continue;

            var supermario = curr.Button!.IsSupermario();
            var movesList = GetJumpMovesFromSquare(i, color, supermario);
            if (movesList.Count > 0 && jumpsCompulsory && !onlyJumps) onlyJumps = true;

            if (movesList.Count > 0 && onlyJumps)
            {
                jumpsDict.Add(i, movesList);
                continue;
            }
            var regsList = GetRegularMovesFromSquare(i, color, supermario);
            if (regsList.Count == 0 && movesList.Count == 0) continue;

            movesList.AddRange(regsList);
            allDict.Add(i, movesList);
        }
        return jumpsCompulsory && onlyJumps ? jumpsDict : allDict;
    }

    private List<Move> GetJumpMovesFromSquare(int sqIndex, EColor color, bool supermario, 
        int initMultiplier = 1, List<Move>? moves = null, int[]? adds = null, List<int>? wouldBeEaten = null)
    {
        moves ??= new List<Move>();
        adds ??= GetSquareIndexAdds(color, supermario);

        foreach (var add in adds)
        {
            var dest = sqIndex + add * initMultiplier;
            if (IsOutsideOfArray(dest)) continue;
            if (!Squares[dest].IsOtherColorButtonSquare(color))
            {
                if (supermario && !Squares[dest].IsColorButtonSquare(color)
                               && dest % Width > 1 && dest % Width < Width - 2)
                {
                    moves = GetJumpMovesFromSquare(sqIndex, color, supermario, 
                        initMultiplier + 1, moves, adds:new[]{add}, wouldBeEaten);
                }
                continue;
            }
            var jump = dest + add;
            if (wouldBeEaten != null && wouldBeEaten.Contains(dest)) continue;
            if (ButtonAndDiagonalInvalid(dest, jump) ||
                IsUnmovable(sqIndex, jump, color) ||
                Squares[jump].HasButton()) continue;

            wouldBeEaten ??= new List<int>();
            wouldBeEaten.Add(dest);
            var nextMoves = GetJumpMovesFromSquare(jump, color, supermario, wouldBeEaten:wouldBeEaten);
            
            moves.Add(new Move(sqIndex, jump, dest, nextMoves.Count == 0 ? null : nextMoves));

            if (supermario) GetRegularMovesFromSquare(sqIndex, color, supermario, 
                initMultiplier + 1, moves, adds:new[]{add});
        }
        return moves;
    }

    private List<Move> GetRegularMovesFromSquare(int sqIndex, EColor color, bool supermario, 
        int initMultiplier = 1, List<Move>? moves = null, int[]? adds = null)
    {
        moves ??= new List<Move>();
        adds ??= GetSquareIndexAdds(color, supermario);

        foreach (var add in adds)
        {
            var dest = sqIndex + add * initMultiplier;
            if (IsUnmovable(sqIndex, dest, color) || Squares[dest].HasButton()) continue;
            
            moves.Add(new Move(sqIndex, dest));

            if (supermario) moves = GetRegularMovesFromSquare(sqIndex, color, supermario, 
                initMultiplier + 1, moves, adds:new[]{add});
        }
        return moves;
    } 

    private int[] GetSquareIndexAdds(EColor color, bool supermario)
    {
        var adds = new int[supermario ? 4 : 2];
        var count = 0;

        if (color.Equals(EColor.White) || supermario)
        {
            adds[count++] = Width - 1;
            adds[count++] = Width + 1;
        }
        if (color.Equals(EColor.Black) || supermario)
        {
            adds[count++] = -Width - 1;
            adds[count] = 1 - Width;
        }
        return adds;
    }

    private bool IsUnmovable(int sourceIndex, int destIndex, EColor color)
    {
        return IsOutsideOfArray(destIndex) || IsOutsideOfArray(destIndex) ||
               ButtonAndDiagonalInvalid(sourceIndex, destIndex) ||
               Squares[destIndex].IsColorButtonSquare(color);
    }

    private bool IsOutsideOfArray(int index)
    {
        return index < 0 || index >= Squares.Length;
    }

    private bool ButtonAndDiagonalInvalid(int buttonIndex, int diagonalIndex)
    {
        return (buttonIndex % Width == Width - 1 || buttonIndex % Width == 0) && 
            (diagonalIndex % Width == Width - 1 || diagonalIndex % Width == 0);
    }
}