using System.Text.Json.Serialization;

namespace DAL.DTO;

public class CheckersGameState
{
    public int Id { get; set; }
    public string SerializedBoardState { get; set; } = default!;
    public bool PlayerOneIsCurrent { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public int CheckersGameId { get; set; }
    [JsonIgnore]
    public CheckersGame? CheckersGame { get; set; }
    
    public string? SerializedNextMoves { get; set; }
}