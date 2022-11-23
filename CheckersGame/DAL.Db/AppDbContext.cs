using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<CheckersBrain> CheckersBrains { get; set; } = default!;
    public DbSet<GameOptions> GameOptions { get; set; } = default!;
    public DbSet<Player> Players { get; set; } = default!;
    public DbSet<Board> Boards { get; set; } = default!;
    public DbSet<BoardPlayer> PlayerBoards { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}