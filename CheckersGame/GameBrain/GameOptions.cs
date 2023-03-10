using DAL.DTO;

namespace GameBrain;

public class GameOptions
{
    public int Width { get; set; } = 8;
    public int Height { get; set; } = 8;
    public bool PlayerOneStarts { get; set; } = true;
    public bool CompulsoryJumps { get; set; } = true;

    public CheckersGameOptions ToDto()
    {
        return new CheckersGameOptions {
            BoardHeight = Height,
            BoardWidth = Width,
            PlayerOneStarts = PlayerOneStarts,
            CompulsoryJumps = CompulsoryJumps
        };
    }
}