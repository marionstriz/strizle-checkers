using DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<CheckersGame> CheckersGames { get; set; } = default!;
    public DbSet<Player> Players { get; set; } = default!;
    public DbSet<CheckersGameOptions> GameOptions { get; set; } = default!;
    public DbSet<CheckersGameState> GameStates { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CheckersGame>().HasIndex(g => g.Name).IsUnique();
        modelBuilder.Entity<Player>().HasIndex(g => g.Name).IsUnique();
    }
}