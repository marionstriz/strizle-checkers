using System.Text.Json;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class BrainDbRepository : IBrainRepository
{
    private readonly AppDbContext _dbContext;
    private const ESaveType SaveType = ESaveType.Database;
    
    public BrainDbRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ESaveType GetSaveType() => SaveType;

    public List<string> GetBrainFileNames()
    {
        return _dbContext.CheckersBrains
            .OrderBy(b => b.Name)
            .Select(b => b.Name)
            .ToList();
    }

    public CheckersBrain GetBrain(string name) => new (GetDomainBrain(name));

    private Domain.CheckersBrain GetDomainBrain(string name)
    {
        return _dbContext.CheckersBrains
            .Include(b => b.Board)
            .ThenInclude(b => b!.BoardPlayers)!
            .ThenInclude(b => b.Player)
            .Include(b => b.GameOptions)
            .First(b => b.Name == name);
    }

    public void SaveBrain(CheckersBrain brain, string name)
    {
        if (brain.SaveOptions == null
            || !brain.SaveOptions.Name.Equals(name)
            || !brain.SaveOptions.SaveType.Equals(SaveType))
        {
            brain.SaveOptions = new SaveOptions(name, SaveType);
        }
        var brainFromDb = _dbContext.CheckersBrains
            .Include(b => b.Board)
            .FirstOrDefault(b => b.Name == name);
        
        if (brainFromDb == null)
        {
            _dbContext.CheckersBrains.Add(brain.ToDomainBrain());
        }
        else
        {
            brainFromDb.Board!.SerializedGameState = JsonSerializer.Serialize(brain.Board.Squares);
            brainFromDb.GameOverAt = brain.GameOverAt;
            brainFromDb.GameWonByPlayer = brain.GameWonByPlayer;
        }
        _dbContext.SaveChanges();
    }

    public void DeleteBrain(string name)
    {
        var brainFromDb = GetDomainBrain(name);
        _dbContext.Players.RemoveRange(
            brainFromDb.Board!.BoardPlayers!.Select(b => b.Player)!);
        _dbContext.Boards.Remove(brainFromDb.Board!);
        _dbContext.GameOptions.Remove(brainFromDb.GameOptions!);
        _dbContext.CheckersBrains.Remove(brainFromDb);
        _dbContext.SaveChanges();
    }
}