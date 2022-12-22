using System.Text.Json.Serialization;

namespace GameBrain;

public class Move
{
    public int Source { get; }
    public int Destination { get; }
    public int? WithEdibleSquare { get; }
    public List<Move>? NextMoves { get; }

    public Move(int source, int dest, List<Move>? nextMoves = null)
    {
        Source = source;
        Destination = dest;
        NextMoves = nextMoves;
    }
    
    [JsonConstructor]
    public Move(int source, int destination, int? withEdibleSquare, List<Move>? nextMoves = null)
    {
        Source = source;
        Destination = destination;
        WithEdibleSquare = withEdibleSquare;
        NextMoves = nextMoves;
    }
}