using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace DAL;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public static readonly ILoggerFactory MyLoggerFactory =
        LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information)
                .AddConsole();
        });
    
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder
            .UseLoggerFactory(MyLoggerFactory)
            .UseSqlite("Data Source=/Users/marionstriz/Documents/dev/School/icd0008-2022f/CheckersGame/app.db");

        return new AppDbContext(optionsBuilder.Options);
    }
}