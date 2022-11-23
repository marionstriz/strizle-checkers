using DAL;
using Microsoft.EntityFrameworkCore;

var menuSystem = new ConsoleApp.MenuSystem();

menuSystem.RunMainMenu();

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=app.db")
    .Options;

using var ctx = new AppDbContext(options);

//dotnet ef migrations add InitialCreate --project DAL.Db --startup-project ConsoleApp 
