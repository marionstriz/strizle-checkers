using System.Text.Json;
using GameBrain;
using Microsoft.EntityFrameworkCore;

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
        
        var gameFromDb = GetDtoByName(name);
        
        if (gameFromDb == null)
        {
            DAL.DTO.CheckersGame newGame;
            if (ESaveType.Database.Equals(ogSaveType))
            {
                var ogGame = _dbContext.CheckersGames
                    .First(g => g.Name == ogName);
                
                newGame = game.ToDto(false);
                newGame.PlayerOneId = ogGame.PlayerOneId;
                newGame.PlayerTwoId = ogGame.PlayerTwoId;
                
            } else newGame = game.ToDto();
            
            _dbContext.CheckersGames.Add(newGame);
        }
        else
        {
            gameFromDb.PlayerOne!.IsCurrent = game.PlayerOne.IsCurrent;
            gameFromDb.PlayerTwo!.IsCurrent = game.PlayerTwo.IsCurrent;
            gameFromDb.SerializedGameState = JsonSerializer.Serialize(game.Board.Squares);
            gameFromDb.UpdatedAt = DateTime.UtcNow;
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
            .FirstOrDefault(b => b.Name == name);
    }
}