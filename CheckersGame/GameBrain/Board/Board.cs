namespace GameBrain.Board;

public class Board
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private Square[,] _squares;

    public Board(int width, int height)
    {
        _squares = new Square[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                _squares[j, i] = new Square(new SquareCoordinates('A', 5));
            }
        }
    }
}