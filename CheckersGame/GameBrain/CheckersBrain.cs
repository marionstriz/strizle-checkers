using System.Text.Json.Serialization;

namespace GameBrain
{
    public class CheckersBrain
    {
        private int? Id { get; }
        public string? FileName { get; set; }
        public GameOptions GameOptions { get; }
        public Board Board { get; }
        private DateTime StartedAt { get; } = DateTime.Now;
        private DateTime? GameOverAt { get; set; }
        private string? GameWonByPlayer { get; set; }

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
            Id = dBrain.Id;
            FileName = dBrain.FileName;
            GameOptions = new GameOptions(dBrain.GameOptions);
            Board = new Board(dBrain.Board);
            StartedAt = dBrain.StartedAt;
            GameOverAt = dBrain.GameOverAt;
            GameWonByPlayer = dBrain.GameWonByPlayer;
        }
        
        public Domain.CheckersBrain ToDomainBrain(bool forJson)
        {
            var dBrain = new Domain.CheckersBrain
            {
                FileName = FileName,
                GameOptions = GameOptions.ToDomainOptions(),
                Board = Board.ToDomainBoard(forJson),
                StartedAt = StartedAt,
                GameOverAt = GameOverAt,
                GameWonByPlayer = GameWonByPlayer
            };
            if (Id != null)
            {
                dBrain.Id = (int) Id;
            }

            return dBrain;
        }
    }
}