using System.Security.AccessControl;

namespace Domain;

public class Board
{
    public int Id { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public int PlayerOneId { get; set; }
    public Player? PlayerOne { get; set; }
    
    public int PlayerTwoId { get; set; }
    public Player? PlayerTwo { get; set; }

    public ICollection<Square>? Squares { get; set; }
}