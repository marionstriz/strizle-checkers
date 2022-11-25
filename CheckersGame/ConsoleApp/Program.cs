using DAL;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=/Users/marionstriz/Documents/dev/School/icd0008-2022f/CheckersGame/app.db")
    .Options;
using var dbContext = new AppDbContext(options);

var menuSystem = new ConsoleApp.MenuSystem(dbContext);

menuSystem.RunMainMenu();

/*
dotnet ef migrations add InitialCreate --project DAL.Db --startup-project ConsoleApp 
dotnet ef database update --project DAL.Db --startup-project ConsoleApp 
dotnet ef database drop --project DAL.Db --startup-project ConsoleApp 
*/