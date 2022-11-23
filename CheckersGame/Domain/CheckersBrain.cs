using System.ComponentModel.DataAnnotations;

namespace Domain;

public class CheckersBrain
{
    public int Id { get; set; }
    
    [MaxLength(128)]
    public string? FileName { get; set; }
    
    public int GameOptionsId { get; set; }
    public GameOptions? GameOptions { get; set; } 
    
    public int BoardId { get; set; }
    public Board? Board { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime? GameOverAt { get; set; }
    [MaxLength(128)]
    public string? GameWonByPlayer { get; set; }
}