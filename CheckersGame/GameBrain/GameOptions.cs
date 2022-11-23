namespace GameBrain;

public class GameOptions
{
    private int? Id { get; }
    public int Width { get; set; } = 8;
    public int Height { get; set; } = 8;
    public bool PlayerOneStarts { get; set; } = true;
    public bool CompulsoryJumps { get; set; } = true;
    
    public GameOptions() {}

    public GameOptions(Domain.GameOptions dOptions)
    {
        Id = dOptions.Id;
        Width = dOptions.Width;
        Height = dOptions.Height;
        PlayerOneStarts = dOptions.PlayerOneStarts;
        CompulsoryJumps = dOptions.CompulsoryJumps;
    }

    public Domain.GameOptions ToDomainOptions()
    {
        var dOptions = new Domain.GameOptions
        {
            Width = Width,
            Height = Height,
            PlayerOneStarts = PlayerOneStarts,
            CompulsoryJumps = CompulsoryJumps
        };
        if (Id != null)
        {
            dOptions.Id = (int) Id;
        }

        return dOptions;
    }
}