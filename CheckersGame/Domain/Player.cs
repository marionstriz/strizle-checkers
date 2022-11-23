using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Player
{
    public int Id { get; set; }
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    public bool IsCurrent { get; set; }

    public int Color { get; set; }
}