using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class PlayerDbRepository : BaseDbRepository, IPlayerDbRepository
{
    public PlayerDbRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<List<Player>> GetAllAsync()
    {
        return await _dbContext.Players.ToListAsync();
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        return await _dbContext.Players
            .Include(p => p.GamesAsPlayerOne)
            .Include(p => p.GamesAsPlayerTwo)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
    
    public async Task<Player?> GetByNameLazyAsync(string name)
    {
        return await _dbContext.Players
            .FirstOrDefaultAsync(p => p.Name.Equals(name));
    }

    public async Task DeleteByIdAsync(int id)
    {
        var player = await _dbContext.Players.FindAsync(id);

        if (player != null)
        {
            _dbContext.Players.Remove(player);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task AddAsync(Player player)
    {
        await _dbContext.Players.AddAsync(player);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveAsync(Player player)
    {
        _dbContext.Attach(player).State = EntityState.Modified; 
        await _dbContext.SaveChangesAsync();
    }
}