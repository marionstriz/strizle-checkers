namespace GameBrain;

public class Button
{
    public EColor Color { get; }
    public EButtonState State { get; private set; }
    public Square? Square { get; private set; }

    public Button(EColor color, EButtonState state)
    {
        Color = color;
        State = state;
    }
    
    public Button(EColor color, EButtonState state, Square square)
    {
        Color = color;
        State = state;
        SetSquare(square);
    }

    public void SetSquare(Square square)
    {
        Square = square;
        square.Button = this;
    }

    public void EatButton()
    {
        State = EButtonState.Eaten;
        if (Square?.Button != null)
        {
            Square.Button = null;
        }
        Square = null;
    }
}