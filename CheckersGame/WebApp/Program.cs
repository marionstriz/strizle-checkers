using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<IGameDbRepository, GameDbRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

/*
dotnet aspnet-codegenerator razorpage -m DAL.DTO.CheckersGame -dc AppDbContext -udl -outDir Pages/CheckersGames --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m DAL.DTO.Player -dc AppDbContext -udl -outDir Pages/Players --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m DAL.DTO.CheckersGameOptions -dc AppDbContext -udl -outDir Pages/GameOptions --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m DAL.DTO.CheckersGameState -dc AppDbContext -udl -outDir Pages/GameStates --referenceScriptLibraries
*/