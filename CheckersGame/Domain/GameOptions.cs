namespace Domain;

public class GameOptions
{
    public int Id { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool PlayerOneStarts { get; set; }
    public bool CompulsoryJumps { get; set; }
    
    public ICollection<CheckersBrain>? CheckersBrains { get; set; }
}