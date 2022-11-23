using System.Text.Json.Serialization;

namespace GameBrain
{
    public class CheckersBrain
    {
        public SaveOptions? SaveOptions { get; set; }
        public GameOptions GameOptions { get; }
        public Board Board { get; }
        public DateTime StartedAt { get; } = DateTime.Now;
        public DateTime? GameOverAt { get; set; }
        public string? GameWonByPlayer { get; set; }

        public CheckersBrain(GameOptions gameOptions)
        {
            GameOptions = gameOptions;
            Board = new Board(gameOptions.Width, gameOptions.Height);
            if (gameOptions.PlayerOneStarts)
            {
                Board.PlayerOne.IsCurrent = true;
            }
            else
            {
                Board.PlayerTwo.IsCurrent = true;
            }
        }

        public CheckersBrain(Domain.CheckersBrain dBrain)
        {
            if (dBrain.GameOptions == null || dBrain.Board == null)
            {
                throw new ArgumentException(
                    "Unable to initialize game brain from Domain object - please ensure all related entities are queried.");
            }
            SaveOptions = new SaveOptions(dBrain.Name, ESaveType.Database);
            GameOptions = new GameOptions(dBrain.GameOptions);
            Board = new Board(dBrain.Board);
            StartedAt = dBrain.StartedAt;
            GameOverAt = dBrain.GameOverAt;
            GameWonByPlayer = dBrain.GameWonByPlayer;
        }

        [JsonConstructor]
        public CheckersBrain(SaveOptions? saveOptions, GameOptions gameOptions, Board board,
            DateTime startedAt, DateTime? gameOverAt, string? gameWonByPlayer)
        {
            SaveOptions = saveOptions;
            GameOptions = gameOptions;
            Board = board;
            StartedAt = startedAt;
            GameOverAt = gameOverAt;
            GameWonByPlayer = gameWonByPlayer;
        }

        public Domain.CheckersBrain ToDomainBrain()
        {
            if (SaveOptions == null)
            {
                throw new ApplicationException("Game brain must have a name when saved!");
            }
            var dBrain = new Domain.CheckersBrain
            {
                Name = SaveOptions.Name,
                GameOptions = GameOptions.ToDomainOptions(),
                Board = Board.ToDomainBoard(),
                StartedAt = StartedAt,
                GameOverAt = GameOverAt,
                GameWonByPlayer = GameWonByPlayer
            };
            return dBrain;
        }
    }
}