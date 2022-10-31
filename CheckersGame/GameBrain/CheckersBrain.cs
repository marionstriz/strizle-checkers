namespace GameBrain;

public class CheckersBrain
{
    public GameOptions GameOptions { get; }
    public Board Board { get; }

    public CheckersBrain(GameOptions gameOptions)
    {
        GameOptions = gameOptions;
        Board = new Board(gameOptions.Width, gameOptions.Height);
    }
}