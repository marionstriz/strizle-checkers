using System.Text.Json;
using DAL.DTO;

namespace GameBrain
{
    public class CheckersGame
    {
        public int? Id { get; set; }
        public SaveOptions? SaveOptions { get; set; }
        public GameOptions GameOptions { get; }
        public Board Board { get; }
        public Player PlayerOne { get; }
        public Player PlayerTwo { get; }
        private DateTime StartedAt { get; } = DateTime.UtcNow;
        public DateTime? GameOverAt { get; set; }
        public string? GameWonByPlayer { get; set; }
        public List<Move>? NextMoves { get; set; }
        public List<CheckersGameState> GameStates { get; } = new();

        public CheckersGame(GameOptions gameOptions, string p1Name, string p2Name)
        {
            if (p1Name.Equals(p2Name)) throw new ArgumentException("Player names cannot be the same");
            GameOptions = gameOptions;
            Board = new Board(gameOptions.Width, gameOptions.Height);
            
            PlayerOne = new Player(p1Name, EColor.White, gameOptions.PlayerOneStarts, 
                Board.CountButtonsWithColor(EColor.White));
            PlayerTwo = new Player(p2Name, EColor.Black, !gameOptions.PlayerOneStarts, 
                Board.CountButtonsWithColor(EColor.Black));
            AddCurrentGameStateToList();
        }

        public CheckersGame(DAL.DTO.CheckersGame dto, ESaveType saveType)
        {
            if (dto.PlayerOne == null || dto.PlayerTwo == null || dto.GameOptions == null || dto.GameStates == null)
            {
                throw new ArgumentException(
                    "Unable to initialize game from DTO - please ensure all related entities are queried.");
            }
            Id = dto.Id;
            GameStates = dto.GameStates.OrderBy(s => s.UpdatedAt).ToList();
            var latestState = GameStates.Last();
                
            SaveOptions = new SaveOptions(dto.Name, saveType);
            
            GameOptions = new GameOptions{
                Height = dto.GameOptions.BoardHeight, 
                Width = dto.GameOptions.BoardWidth, 
                PlayerOneStarts = dto.GameOptions.PlayerOneStarts, 
                CompulsoryJumps = dto.GameOptions.CompulsoryJumps};
            
            Board = new Board(dto.GameOptions.BoardWidth, dto.GameOptions.BoardHeight, 
                latestState.SerializedBoardState);
            
            StartedAt = dto.StartedAt;
            GameOverAt = dto.GameOverAt;
            GameWonByPlayer = dto.GameWonByPlayer;
            
            PlayerOne = new Player(dto.PlayerOne.Name, dto.PlayerOneColor, latestState.PlayerOneIsCurrent,
                Board.CountButtonsWithColor(dto.PlayerOneColor));
            
            var playerTwoColor = dto.PlayerOneColor == EColor.Black ? EColor.White : EColor.Black;
            PlayerTwo = new Player(dto.PlayerTwo.Name, playerTwoColor, !latestState.PlayerOneIsCurrent,
                Board.CountButtonsWithColor(playerTwoColor));

            if (latestState.SerializedNextMoves != null) 
                NextMoves = JsonSerializer.Deserialize<List<Move>>(latestState.SerializedNextMoves);
        }

        public Dictionary<int, List<Move>> GetCurrentPlayerMoves()
        {
            return Board.GetCurrentPossibleMoves(GetCurrentTurnPlayer(), GameOptions.CompulsoryJumps);
        }

        public Player GetCurrentTurnPlayer()
        {
            return PlayerOne.IsCurrent ? PlayerOne : PlayerTwo;
        }

        private void SwapPlayerTurn()
        {
            PlayerOne.IsCurrent = !PlayerOne.IsCurrent;
            PlayerTwo.IsCurrent = !PlayerTwo.IsCurrent;
        }
        
        public void MakeMove(Move move)
        {
            if (NextMoves != null && !NextMoves.Contains(move))
            {
                throw new ApplicationException("Please carry out compulsory move first");
            }
            var sourceSq = Board.Squares[move.Source];
            var destSq = Board.Squares[move.Destination];
            if (!sourceSq.HasButton()) 
                throw new ApplicationException("Don't try to move a nonexistent button please");
            var movingButton = sourceSq.Button;
            destSq.Button = movingButton;
            sourceSq.Button = null;

            if (movingButton!.Color.Equals(EColor.Black) && destSq.Coordinates.Y == Board.Height ||
                movingButton.Color.Equals(EColor.White) && destSq.Coordinates.Y == 1)
            {
                movingButton.State = EButtonState.Supermario;
            }
            if (move.NextMoves != null) NextMoves = move.NextMoves;
            else if (NextMoves != null) NextMoves = null;
            if (move.WithEdibleSquare != null) EatPlayerButton(Board.Squares[move.WithEdibleSquare.Value]);
            if (NextMoves == null) SwapPlayerTurn();
            AddCurrentGameStateToList();
        }

        private void AddCurrentGameStateToList()
        {
            var state = new CheckersGameState
            {
                SerializedBoardState = JsonSerializer.Serialize(Board.Squares),
                SerializedNextMoves = NextMoves != null ? JsonSerializer.Serialize(NextMoves) : null,
                PlayerOneIsCurrent = PlayerOne.IsCurrent,
            };
            if (Id != null) state.CheckersGameId = Id.Value;
            GameStates.Add(state);
        }

        private void EatPlayerButton(Square edible)
        {
            var player = edible.Button!.Color.Equals(PlayerOne.Color) ? PlayerOne : PlayerTwo;
            
            player.ButtonCount--;
            if (player.ButtonCount == 0)
            {
                GameOverAt = DateTime.UtcNow;
                GameWonByPlayer = player.Equals(PlayerOne) ? PlayerTwo.Name : PlayerOne.Name;
            }
            edible.Button = null;
        }

        public DAL.DTO.CheckersGame ToDto()
        {
            if (SaveOptions == null)
            {
                throw new ApplicationException("Game must have a name when being saved!");
            }
            var dBrain = new DAL.DTO.CheckersGame
            {
                Name = SaveOptions.Name,
                StartedAt = StartedAt,
                GameOverAt = GameOverAt,
                GameWonByPlayer = GameWonByPlayer,
                GameStates = GameStates,
                PlayerOneColor = PlayerOne.Color
            };
            if (Id != null) dBrain.Id = Id.Value;
            return dBrain;
        }

        public DAL.DTO.CheckersGame ToDtoWithAllRelatedEntities()
        {
            var dto = ToDto();
            dto.PlayerOne = new DAL.DTO.Player {Name = PlayerOne.Name};
            dto.PlayerTwo = new DAL.DTO.Player {Name = PlayerTwo.Name};
            dto.GameOptions = GameOptions.ToDto();

            return dto;
        }
    }
}