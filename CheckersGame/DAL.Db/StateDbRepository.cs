using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class StateDbRepository : BaseDbRepository, IStateDbRepository
{
    public StateDbRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
    public async Task<List<CheckersGameState>> GetAllAsync()
    {
        return await _dbContext.GameStates
                    .Include(s => s.CheckersGame)
                    .ToListAsync();
    }

    public async Task<CheckersGameState?> GetByIdAsync(int id)
    {
        return await _dbContext.GameStates
            .Include(s => s.CheckersGame)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task DeleteByIdAsync(int id)
    {
        var state = await _dbContext.GameStates.FindAsync(id);

        if (state != null)
        {
            _dbContext.GameStates.Remove(state);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task AddAsync(CheckersGameState state)
    {
        await _dbContext.GameStates.AddAsync(state);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveAsync(CheckersGameState state)
    {
        _dbContext.Attach(state).State = EntityState.Modified; 
        await _dbContext.SaveChangesAsync();
    }
}