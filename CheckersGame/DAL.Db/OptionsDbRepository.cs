using DAL.DTO;
using GameBrain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class OptionsDbRepository : BaseDbRepository, IOptionsDbRepository
{
    public OptionsDbRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<List<CheckersGameOptions>> GetAllAsync()
    {
        return await _dbContext.GameOptions.ToListAsync();
    }

    public async Task<CheckersGameOptions?> GetByIdAsync(int id)
    {
        return await _dbContext.GameOptions
            .FirstOrDefaultAsync(b => b.Id == id);
    }
    
    public async Task<CheckersGameOptions?> GetByOptionsAsync(GameOptions options)
    {
        return await _dbContext.GameOptions
            .FirstOrDefaultAsync(o => o.BoardHeight == options.Height &&
                                 o.BoardWidth == options.Width &&
                                 o.CompulsoryJumps == options.CompulsoryJumps &&
                                 o.PlayerOneStarts == options.PlayerOneStarts);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var options = await _dbContext.GameOptions.FindAsync(id);

        if (options != null)
        {
            _dbContext.GameOptions.Remove(options);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task AddAsync(CheckersGameOptions options)
    {
        await _dbContext.GameOptions.AddAsync(options);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task SaveAsync(CheckersGameOptions options)
    {
        _dbContext.Attach(options).State = EntityState.Modified; 
        await _dbContext.SaveChangesAsync();
    }
    
}