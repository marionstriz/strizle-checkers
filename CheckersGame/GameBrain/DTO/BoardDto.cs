using System.Text.Json.Serialization;

namespace GameBrain.DTO;

public class BoardDto
{
    public Player PlayerOne { get; }
    public Player PlayerTwo { get; }
    public List<List<Square>> Squares { get; }

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
        for (int i = 0; i < squares.GetLength(0); i++)
        {
            for (int j = 0; j < squares.GetLength(1); j++)
            {
                if (j == 0) Squares.Add(new List<Square>());
                Squares[i].Add(squares[i,j]);
            }
        }
    }

    public Board GetBoard()
    {
        var squares = new Square[Squares.Count,Squares[0].Count];
        for (int i = 0; i < squares.GetLength(0); i++)
        {
            for (int j = 0; j < squares.GetLength(1); j++)
            {
                squares[i,j] = Squares[i][j];
            }
        }
        return new Board(PlayerOne, PlayerTwo, squares);
    }
}