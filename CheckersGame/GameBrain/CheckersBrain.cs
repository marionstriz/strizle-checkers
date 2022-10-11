namespace GameBrain;

public class CheckersBrain
{
    public Board.Board Board { get; }
    
    public CheckersBrain(int boardWidth = 8, int boardHeight = 8)
    {
        Board = new Board.Board(boardWidth, boardHeight);
    }
}