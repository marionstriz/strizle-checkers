using System.Security.AccessControl;
using System.Text.Json;

namespace GameBrain
{
    public class CheckersGame
    {
        public SaveOptions? SaveOptions { get; set; }
        public GameOptions GameOptions { get; }
        public Board Board { get; }
        public Player PlayerOne { get; }
        public Player PlayerTwo { get; }
        public DateTime StartedAt { get; } = DateTime.Now.ToUniversalTime();
        public DateTime? GameOverAt { get; set; }
        public string? GameWonByPlayer { get; set; }

        public CheckersGame(GameOptions gameOptions)
        {
            GameOptions = gameOptions;
            Board = new Board(gameOptions.Width, gameOptions.Height);
            PlayerOne = new Player("p1", EColor.White);
            PlayerTwo = new Player("p2", EColor.Black);
            if (gameOptions.PlayerOneStarts)
            {
                PlayerOne.IsCurrent = true;
            }
            else
            {
                PlayerTwo.IsCurrent = true;
            }
        }

        public CheckersGame(DAL.DTO.CheckersGame dto, ESaveType saveType)
        {
            if (dto.PlayerOne == null || dto.PlayerTwo == null)
            {
                throw new ArgumentException(
                    "Unable to initialize game from DTO - please ensure all related entities are queried.");
            }
            SaveOptions = new SaveOptions(dto.Name, saveType);
            GameOptions = new GameOptions{
                Height = dto.BoardHeight, 
                Width = dto.BoardWidth, 
                PlayerOneStarts = dto.PlayerOneStarts, 
                CompulsoryJumps = dto.CompulsoryJumps};
            Board = new Board(dto.BoardWidth, dto.BoardHeight, dto.SerializedGameState);
            StartedAt = dto.StartedAt;
            GameOverAt = dto.GameOverAt;
            GameWonByPlayer = dto.GameWonByPlayer;
            PlayerOne = new Player(dto.PlayerOne);
            PlayerTwo = new Player(dto.PlayerTwo);
        }

        public DAL.DTO.CheckersGame ToDto(bool includePlayers = true)
        {
            if (SaveOptions == null)
            {
                throw new ApplicationException("Game must have a name when being saved!");
            }
            var dBrain = new DAL.DTO.CheckersGame
            {
                Name = SaveOptions.Name,
                BoardHeight = GameOptions.Height,
                BoardWidth = GameOptions.Width,
                PlayerOneStarts = GameOptions.PlayerOneStarts,
                CompulsoryJumps = GameOptions.CompulsoryJumps,
                StartedAt = StartedAt,
                GameOverAt = GameOverAt,
                GameWonByPlayer = GameWonByPlayer,
                SerializedGameState = JsonSerializer.Serialize(Board.Squares)
            };
            if (includePlayers)
            {
                dBrain.PlayerOne = PlayerOne.ToDto();
                dBrain.PlayerTwo = PlayerTwo.ToDto();
            }
            return dBrain;
        }
    }
}