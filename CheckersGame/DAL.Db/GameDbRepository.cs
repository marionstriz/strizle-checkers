using GameBrain;
using Microsoft.EntityFrameworkCore;
using CheckersGame = GameBrain.CheckersGame;

namespace DAL;

public class GameDbRepository : BaseDbRepository, IGameDbRepository
{
    private const ESaveType SaveType = ESaveType.Database;
    private IStateDbRepository StateRepository { get; }
    private IPlayerDbRepository PlayerRepository { get; }
    private IOptionsDbRepository OptionsRepository { get; }
    
    public GameDbRepository(AppDbContext dbContext) : base(dbContext)
    {
        StateRepository = new StateDbRepository(dbContext);
        PlayerRepository = new PlayerDbRepository(dbContext);
        OptionsRepository = new OptionsDbRepository(dbContext);
    }

    public IStateDbRepository GetStateRepository() => StateRepository;
    public IPlayerDbRepository GetPlayerRepository() => PlayerRepository;
    public IOptionsDbRepository GetOptionsRepository() => OptionsRepository;

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

    public async Task<List<DTO.CheckersGame>> GetAllAsync()
    {
        return await _dbContext.CheckersGames
            .Include(c => c.GameOptions)
            .Include(c => c.PlayerOne)
            .Include(c => c.PlayerTwo)
            .ToListAsync();
    }

    public async Task<DTO.CheckersGame?> GetByIdAsync(int id)
    {
        return await _dbContext.CheckersGames
            .Include(g => g.PlayerOne)
            .Include(g => g.PlayerTwo)
            .Include(g => g.GameOptions)
            .Include(g => g.GameStates)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<DTO.CheckersGame?> GetByIdLazyAsync(int id)
    {
        return await _dbContext.CheckersGames
            .FirstOrDefaultAsync(b => b.Id == id);
    }
 
    public async Task DeleteByIdAsync(int id)
    {
        var checkersGame = await _dbContext.CheckersGames.FindAsync(id);

        if (checkersGame != null)
        {
            _dbContext.CheckersGames.Remove(checkersGame);
            await _dbContext.SaveChangesAsync();
        }
    }

    public CheckersGame? GetBrainGameByName(string name)
    {
        var dto = GetByName(name);
        if (dto == null) return null;
        return new CheckersGame(dto, SaveType);
    }

    public void SaveBrainGameByName(CheckersGame game, string name)
    {
        SaveBrainGameByNameAsync(game, name).Wait();
    }

    public async Task AddAsync(DTO.CheckersGame game)
    {
        await _dbContext.CheckersGames.AddAsync(game);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveBrainGameAsync(CheckersGame game)
    {
        if (game.SaveOptions == null || !game.SaveOptions.SaveType.Equals(ESaveType.Database))
        {
            throw new ArgumentException(
                "Cannot save game with incorrect save options to database. Please use Name method.");
        }
        await SaveBrainGameByNameAsync(game, game.SaveOptions!.Name);
    }

    public async Task SaveBrainGameByNameAsync(CheckersGame game, string name)
    {
        var ogName = game.SaveOptions?.Name;
        var ogSaveType = game.SaveOptions?.SaveType;
        if (game.SaveOptions == null
            || !ogName!.Equals(name)
            || !ogSaveType.Equals(SaveType))
        {
            game.SaveOptions = new SaveOptions(name, SaveType);
        }
        
        var gameFromDb = GetByNameLazyAsync(name).Result;
        
        if (gameFromDb == null)
        {
            DAL.DTO.CheckersGame newGame = game.ToDto();
            var playerOne = await PlayerRepository.GetByNameLazyAsync(game.PlayerOne.Name);
            var playerTwo = await PlayerRepository.GetByNameLazyAsync(game.PlayerTwo.Name);
            var options = await OptionsRepository.GetByOptionsAsync(game.GameOptions);

            if (playerOne != null) newGame.PlayerOneId = playerOne.Id;
            else newGame.PlayerOne = new DTO.Player {Name = game.PlayerOne.Name};
            
            if (playerTwo != null) newGame.PlayerTwoId = playerTwo.Id;
            else newGame.PlayerTwo = new DTO.Player {Name = game.PlayerTwo.Name};

            if (options != null) newGame.GameOptionsId = options.Id;
            else newGame.GameOptions = game.GameOptions.ToDto();

            await AddAsync(newGame);
            game.GameStates.ForEach(s => s.CheckersGameId = newGame.Id);
            game.Id = newGame.Id;
        }
        else
        {
            game.Id ??= gameFromDb.Id;
            gameFromDb.GameStates = game.GameStates;
            gameFromDb.GameOverAt = game.GameOverAt;
            gameFromDb.GameWonByPlayer = game.GameWonByPlayer;
        }
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task SaveAsync(DTO.CheckersGame game)
    {
        _dbContext.Attach(game).State = EntityState.Modified; 
        await _dbContext.SaveChangesAsync();
    }

    public void DeleteByName(string name)
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
    
    public async Task<DTO.CheckersGame?> GetByNameLazyAsync(string name)
    {
        return await _dbContext.CheckersGames
            .FirstOrDefaultAsync(p => p.Name.Equals(name));
    }

    private DAL.DTO.CheckersGame? GetByName(string name)
    {
        return _dbContext.CheckersGames
            .Include(g => g.PlayerOne)
            .Include(g => g.PlayerTwo)
            .Include(g => g.GameOptions)
            .Include(g => g.GameStates)
            .FirstOrDefault(b => b.Name == name);
    }
}