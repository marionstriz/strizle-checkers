using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameBrain.DTO;

public class CheckersBrainDto
{
    public GameOptions Options { get; set; }
    public BoardDto Board { get; set; }

    [JsonConstructor]
    public CheckersBrainDto(GameOptions options, BoardDto board)
    {
        Options = options;
        Board = board;
    }

    public CheckersBrainDto(CheckersBrain brain)
    {
        Options = brain.GameOptions;
        Board = new BoardDto(brain.Board);
    }

    public CheckersBrain GetBrain()
    {
        return new CheckersBrain(Options, Board.GetBoard());
    }
}