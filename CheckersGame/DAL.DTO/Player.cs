using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.DTO;

public class Player
{
    public int Id { get; set; }
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    public bool IsCurrent { get; set; }
    public int Color { get; set; }
    
    [InverseProperty("PlayerOne")]
    public ICollection<CheckersGame>? GamesAsPlayerOne { get; set; }
    [InverseProperty("PlayerTwo")]
    public ICollection<CheckersGame>? GamesAsPlayerTwo { get; set; }
}