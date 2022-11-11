namespace GameBrain;

public class CheckersBrain
{
    public string? FileName { get; set; }
    public GameOptions GameOptions { get; }
    public Board Board { get; }

    public CheckersBrain(GameOptions gameOptions)
    {
        GameOptions = gameOptions;
        Board = new Board(gameOptions.Width, gameOptions.Height);
    }

    public CheckersBrain(GameOptions options, Board board)
    {
        GameOptions = options;
        Board = board;
    }
}