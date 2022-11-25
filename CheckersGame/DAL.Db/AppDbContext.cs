using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<CheckersGame> CheckersGames { get; set; } = default!;
    public DbSet<Player> Players { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}