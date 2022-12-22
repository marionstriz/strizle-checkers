using System.ComponentModel.DataAnnotations;
using GameBrain;

namespace DAL.DTO;

public class CheckersGame
{
    public int Id { get; set; }
    
    [MaxLength(128)] 
    public string Name { get; set; } = default!;
    
    public int GameOptionsId { get; set; }
    public CheckersGameOptions? GameOptions { get; set; }
    
    public ICollection<CheckersGameState>? GameStates { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? GameOverAt { get; set; }
    [MaxLength(128)]
    public string? GameWonByPlayer { get; set; }
    
    public EColor PlayerOneColor { get; set; }

    public int PlayerOneId { get; set; }
    public Player? PlayerOne { get; set; }
    
    public int PlayerTwoId { get; set; }
    public Player? PlayerTwo { get; set; } 
}