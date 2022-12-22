namespace GameBrain;

public class Square
{
    public SquareCoordinates Coordinates { get; }
    public Button? Button { get; set; }

    public Square(SquareCoordinates coordinates)
    {
        Coordinates = coordinates;
    }

    public bool IsColorButtonSquare(EColor color)
    {
        return HasButton() && Button!.Color.Equals(color);
    }

    public bool IsOtherColorButtonSquare(EColor color)
    {
        return HasButton() && !IsColorButtonSquare(color);
    }

    public bool HasButton()
    {
        return Button != null;
    }
    
    public bool IsMovableSquare()
    {
        return Coordinates.X % 2 == 0 && Coordinates.Y % 2 == 1
               || Coordinates.X % 2 == 1 && Coordinates.Y % 2 == 0;
    }
}