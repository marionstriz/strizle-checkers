namespace GameBrain.Board;

public class Square
{
    public readonly SquareCoordinates Coordinates;
    public ESquareState State { get; set; }
    
    public Square(SquareCoordinates coords, ESquareState state = ESquareState.Empty)
    {
        Coordinates = coords;
        State = state;
    }
}