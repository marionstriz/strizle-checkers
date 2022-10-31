namespace GameBrain;

public class Square
{
    public SquareCoordinates Coordinates { get; }
    public Button? Button { get; set; }

    public Square(SquareCoordinates coords)
    {
        Coordinates = coords;
    }
}