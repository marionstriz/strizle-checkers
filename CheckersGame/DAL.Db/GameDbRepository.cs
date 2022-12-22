using System.Security.AccessControl;
using System.Text.Json;
using DAL.DTO;
using GameBrain;
using Microsoft.EntityFrameworkCore;
using CheckersGame = GameBrain.CheckersGame;

namespace DAL;

public class GameDbRepository : IGameRepository
{
    private readonly AppDbContext _dbContext;
    private const ESaveType SaveType = ESaveType.Database;
    
    public GameDbRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ESaveType GetSaveType() => SaveType;

    public List<string> GetGameFileNames()
    {
        return _dbContext.CheckersGames
            .OrderByDescending(g => g.StartedAt)
            .Select(b => b.Name)
            .ToList();
    }

    public List<string> GetGameFileNamesContaining(string substring)
    {
        return _dbContext.CheckersGames
            .OrderByDescending(g => g.StartedAt)
            .Select(g => g.Name)
            .Where(n => n.Contains(substring))
            .ToList();
    }

    public CheckersGame GetGameByName(string name)
    {
        var dto = GetDtoByName(name);
        if (dto == null)
        {
            throw new ArgumentException($"No game with name {name} found in database.");
        }
        return new CheckersGame(dto, SaveType);
    }

    public void SaveGame(CheckersGame game, string name)
    {
        var ogName = game.SaveOptions?.Name;
        var ogSaveType = game.SaveOptions?.SaveType;
        if (game.SaveOptions == null
            || !ogName!.Equals(name)
            || !ogSaveType.Equals(SaveType))
        {
            game.SaveOptions = new SaveOptions(name, SaveType);
        }
        
        var gameFromDb = GetDtoByNameWithoutIncludes(name);
        
        if (gameFromDb == null)
        {
            DAL.DTO.CheckersGame newGame = game.ToDto();
            var playerOne = GetPlayerDtoByName(game.PlayerOne.Name);
            var playerTwo = GetPlayerDtoByName(game.PlayerTwo.Name);
            var options = GetOptionsByOptions(game.GameOptions);

            if (playerOne != null) newGame.PlayerOneId = playerOne.Id;
            else newGame.PlayerOne = new DTO.Player {Name = game.PlayerOne.Name};
            
            if (playerTwo != null) newGame.PlayerTwoId = playerTwo.Id;
            else newGame.PlayerTwo = new DTO.Player {Name = game.PlayerTwo.Name};

            if (options != null) newGame.GameOptionsId = options.Id;
            else newGame.GameOptions = game.GameOptions.ToDto();
            
            _dbContext.CheckersGames.Add(newGame);
        }
        else
        {
            _dbContext.GameStates.Add(new CheckersGameState
            {
                CheckersGameId = gameFromDb.Id,
                PlayerOneIsCurrent = game.PlayerOne.IsCurrent,
                SerializedBoardState = JsonSerializer.Serialize(game.Board.Squares),
            });
            gameFromDb.GameOverAt = game.GameOverAt;
            gameFromDb.GameWonByPlayer = game.GameWonByPlayer;
        }
        _dbContext.SaveChanges();
    }

    public void DeleteGame(string name)
    {
        var gameFromDb = _dbContext.CheckersGames
            .FirstOrDefault(g => g.Name == name);
        if (gameFromDb == null)
        {
            throw new ArgumentException($"No game brain with name {name} found in database.");
        }
        _dbContext.CheckersGames.Remove(gameFromDb);
        _dbContext.SaveChanges();
    }

    private DAL.DTO.CheckersGame? GetDtoByName(string name)
    {
        return _dbContext.CheckersGames
            .Include(g => g.PlayerOne)
            .Include(g => g.PlayerTwo)
            .Include(g => g.GameOptions)
            .Include(g => g.GameStates)
            .FirstOrDefault(b => b.Name == name);
    }

    private DAL.DTO.CheckersGame? GetDtoByNameWithoutIncludes(string name)
    {
        return _dbContext.CheckersGames.FirstOrDefault(b => b.Name == name);
    }

    private DTO.Player? GetPlayerDtoByName(string name)
    {
        return _dbContext.Players.FirstOrDefault(p => p.Name.Equals(name));
    }
    
    private CheckersGameOptions? GetOptionsByOptions(GameOptions options)
    {
        return _dbContext.GameOptions
            .FirstOrDefault(o => o.BoardHeight == options.Height &&
                                 o.BoardWidth == options.Width &&
                                 o.CompulsoryJumps == options.CompulsoryJumps &&
                                 o.PlayerOneStarts == options.PlayerOneStarts);
    }
}