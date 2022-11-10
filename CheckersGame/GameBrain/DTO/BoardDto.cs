using System.Text.Json.Serialization;

namespace GameBrain.DTO;

public class BoardDto
{
    public Player PlayerOne { get; set; }
    public Player PlayerTwo { get; set; }
    public List<List<Square>> Squares { get; set; }

    [JsonConstructor]
    public BoardDto(Player playerOne, Player playerTwo, List<List<Square>> squares)
    {
        PlayerOne = playerOne;
        PlayerTwo = playerTwo;
        Squares = squares;
    }

    public BoardDto(Board board)
    {
        PlayerOne = board.PlayerOne;
        PlayerTwo = board.PlayerTwo;
        var squares = board.Squares;

        Squares = new List<List<Square>>();
        for (int j = 0; j < squares.GetLength(1); j++)
        {
            for (int i = 0; i < squares.GetLength(0); i++)
            {
                if (j == 0) Squares.Add(new List<Square>());
                Console.WriteLine(squares[i,j].Coordinates.X);
                Console.WriteLine(squares[i,j].Coordinates.Y);
                Squares[j].Add(squares[i,j]);
            }
        }
    }

    public Board GetBoard()
    {
        var squares = new Square[Squares.Count,Squares[0].Count];
        for (int j = 0; j < squares.GetLength(1); j++)
        {
            for (int i = 0; i < squares.GetLength(0); i++)
            {
                squares[i,j] = Squares[j][i];
                Console.WriteLine(squares[i,j].Coordinates.X);
                Console.WriteLine(squares[i,j].Coordinates.Y);
            }
        }
        return new Board(PlayerOne, PlayerTwo, squares);
    }
}