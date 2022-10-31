using System.Collections.ObjectModel;

namespace GameBrain;

public class Player
{
    public string Name { get; }
    public EColor Color { get; }
    
    private readonly List<Button> _buttons = new();
    
    public Player(string name, EColor color)
    {
        Name = name;
        Color = color;
    }

    public void AddButton(Button button)
    {
        _buttons.Add(button);
    }

    public bool HasButton(Button button)
    {
        return _buttons.Contains(button);
    }
}