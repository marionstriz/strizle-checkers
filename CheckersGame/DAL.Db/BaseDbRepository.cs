using Microsoft.EntityFrameworkCore;

namespace DAL;

public abstract class BaseDbRepository
{
    protected readonly AppDbContext _dbContext;

    protected BaseDbRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
}