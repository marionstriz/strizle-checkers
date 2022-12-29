using System.ComponentModel.DataAnnotations;
using GameBrain;

namespace DAL.DTO;

public class CheckersGame : IValidatableObject
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public int GameOptionsId { get; set; }
    public CheckersGameOptions? GameOptions { get; set; }
    
    public ICollection<CheckersGameState>? GameStates { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? GameOverAt { get; set; }
    [MaxLength(128)]
    public string? GameWonByPlayer { get; set; }
    
    public EColor PlayerOneColor { get; set; }
    
    public int PlayerOneId { get; set; }
    public Player? PlayerOne { get; set; }
    
    public int PlayerTwoId { get; set; }
    public Player? PlayerTwo { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PlayerOneId == PlayerTwoId)
        {
            yield return new ValidationResult(
                "Player 1 cannot be the same as Player 2",
                new[] { nameof(PlayerOneId), nameof(PlayerTwoId) });
        }
    }
}