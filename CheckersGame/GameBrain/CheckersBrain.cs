namespace GameBrain;

public class CheckersBrain
{
    private Board.Board _gameBoard;
    
    public CheckersBrain(int boardWidth = 8, int boardHeight = 8)
    {
        _gameBoard = new Board.Board(boardWidth, boardHeight);
    }
}