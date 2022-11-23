namespace GameBrain;

public class Button
{
    private int? Id { get; }
    public EColor Color { get; }
    public EButtonState State { get; set; }

    public Button(EColor color, EButtonState state)
    {
        Color = color;
        State = state;
    }

    public Button(Domain.Button dButton)
    {
        if (dButton.Color is not (0 or 1) || dButton.State is not (0 or 1))
        {
            throw new ArgumentException(
                "Unable to initialize button from Domain object - invalid enum values.");
        }

        Id = dButton.Id;
        Color = (EColor) dButton.Color;
        State = (EButtonState) dButton.State;
    }

    public Domain.Button ToDomainButton()
    {
        var dButton = new Domain.Button
        {
            Color = (int) Color,
            State = (int) State
        };
        if (Id != null)
        {
            dButton.Id = (int) Id;
        }

        return dButton;
    }
}