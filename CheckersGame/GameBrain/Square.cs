namespace GameBrain;

public class Square
{
    private int? Id { get; }
    public SquareCoordinates Coordinates { get; }
    public Button? Button { get; set; }

    public Square(SquareCoordinates coordinates)
    {
        Coordinates = coordinates;
    }

    public Square(Domain.Square dSquare)
    {
        Coordinates = new SquareCoordinates(dSquare.X, dSquare.Y);
        Id = dSquare.Id;
        if (dSquare.Button != null)
        {
            Button = new Button(dSquare.Button);
        }
    }

    public Domain.Square ToDomainSquare(Domain.Board board, bool forJson)
    {
        var dSquare = new Domain.Square
        {
            X = Coordinates.X,
            Y = Coordinates.Y,
            Button = Button?.ToDomainButton()
        };
        if (!forJson)
        {
            dSquare.Board = board;
        }
        if (Id != null)
        {
            dSquare.Id = (int) Id;
        }

        return dSquare;
    }
}