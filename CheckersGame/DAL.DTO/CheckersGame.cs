using System.ComponentModel.DataAnnotations;

namespace DAL.DTO;

public class CheckersGame
{
    public int Id { get; set; }

    [MaxLength(128)] 
    public string Name { get; set; } = default!;
    
    public int BoardWidth { get; set; }
    public int BoardHeight { get; set; }
    
    public bool PlayerOneStarts { get; set; }
    public bool CompulsoryJumps { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? GameOverAt { get; set; }
    [MaxLength(128)]
    public string? GameWonByPlayer { get; set; }

    public string SerializedGameState { get; set; } = default!;
    
    public int PlayerOneId { get; set; }
    public Player? PlayerOne { get; set; }
    
    public int PlayerTwoId { get; set; }
    public Player? PlayerTwo { get; set; } 
}