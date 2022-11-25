namespace GameBrain;

public class Button
{
    public EColor Color { get; }
    public EButtonState State { get; set; }

    public Button(EColor color, EButtonState state)
    {
        Color = color;
        State = state;
    }
}