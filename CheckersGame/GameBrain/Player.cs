using System.Collections.ObjectModel;

namespace GameBrain;

public class Player
{
    public string Name { get; }
    public EColor Color { get; }
    
    public Player(string name, EColor color)
    {
        Name = name;
        Color = color;
    }
}