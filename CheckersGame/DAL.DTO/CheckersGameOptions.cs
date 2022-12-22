namespace DAL.DTO;

public class CheckersGameOptions
{
    public int Id { get; set; }
    
    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }
    
    public bool PlayerOneStarts { get; set; }
    public bool CompulsoryJumps { get; set; }
    
    public ICollection<CheckersGame>? CheckersGames { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }
        var other = (CheckersGameOptions) obj;
        return other.BoardWidth == BoardWidth &&
               other.BoardHeight == BoardHeight &&
               other.PlayerOneStarts == PlayerOneStarts &&
               other.CompulsoryJumps == CompulsoryJumps;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(BoardWidth, BoardHeight, PlayerOneStarts, CompulsoryJumps);
    }
}