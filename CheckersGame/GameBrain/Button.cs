namespace GameBrain;

public class Button
{
    public EColor Color { get; }
    public EButtonState State { get; private set; }

    public Button(EColor color, EButtonState state)
    {
        Color = color;
        State = state;
    }

    public void EatButton()
    {
        State = EButtonState.Eaten;
    }
}