using System.Security.AccessControl;

namespace Domain;

public class Board
{
    public int Id { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    
    public ICollection<BoardPlayer>? BoardPlayers { get; set; }

    public string SerializedGameState { get; set; } = default!;
    
    public ICollection<CheckersBrain>? CheckersBrains { get; set; }
}