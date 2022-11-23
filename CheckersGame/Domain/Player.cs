using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain;

public class Player
{
    public int Id { get; set; }
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    public bool IsCurrent { get; set; }

    public EColor Color { get; set; }
    
    public ICollection<BoardPlayer>? BoardPlayers { get; set; }
}