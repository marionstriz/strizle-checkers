using DAL;
using DAL.FileSystem;
using GameBrain;
using MenuSystem;

namespace ConsoleUI;

public class UIController
{
    public ConsoleColor MainColor => ConsoleColor.Blue;
    public ConsoleColor TitleColor => ConsoleColor.DarkCyan;
    public ConsoleColor FillerColor => ConsoleColor.DarkBlue;
    public ConsoleColor ErrorColor => ConsoleColor.Red;
    public ConsoleColor YaaasColor => ConsoleColor.Green;

    public MenuUI MenuUI { get; }
    public OptionsUI OptionsUI { get; }
    public GameUI GameUI { get; private set; }
    public RepositoryUI FileRepoUI { get; }
    public RepositoryUI DbRepoUI { get; }

    public UIController(AppDbContext dbContext)
    {
        var options = new GameOptions();
        var defaultBrain = new CheckersGame(options);
        var fileSystemRepo = new GameFileSystemRepository();
        var dbRepo = new GameDbRepository(dbContext);
        
        MenuUI = new MenuUI(this);
        OptionsUI = new OptionsUI(this, options);
        GameUI = new GameUI(this, defaultBrain);
        FileRepoUI = new RepositoryUI(this, fileSystemRepo);
        DbRepoUI = new RepositoryUI(this, dbRepo);
    }

    public void NewGame(GameOptions options)
    {
        GameUI = new GameUI(this, new CheckersGame(options));
    }

    public void LoadGame(CheckersGame game)
    {
        GameUI = new GameUI(this, game);
    }

    public char StartCustomOptions(Menu optionsMenu)
    {
        OptionsUI.Options = new GameOptions();
        return MenuUI.RunMenuForUserInput(optionsMenu);
    }

    public void PrintSuccess(string success)
    {
        Console.Clear();
        Console.ForegroundColor = YaaasColor;
        Console.WriteLine(success);
        MenuUI.ClearConsole = false;
    }

    public void PrintMenuError(string error) => PrintError(error, true);

    public void PrintError(string error, bool menuError = false)
    {
        Console.Clear();
        Console.ForegroundColor = ErrorColor;
        Console.WriteLine(error);
        if (menuError) MenuUI.ClearConsole = false;
    }

    public CheckersGame GetBrain() => GameUI.Game;

    public GameOptions GetOptions() => OptionsUI.Options;

    public char BrainPlayGame() => GameUI.PlayGame();
    
    public string? AskForInput(string prompt, bool clearConsole = true)
    {
        Console.CursorVisible = true;
        
        if (clearConsole) Console.Clear();
        Console.ForegroundColor = MainColor;
        
        Console.WriteLine("'X' to exit.");
        Console.Write(prompt);
        
        var input = Console.ReadLine();
        
        Console.CursorVisible = false;
        if (!"X".Equals(input?.ToUpper())) return input!;
        return null;
    }

    public char SaveExisting()
    {
        if (GameUI.Game.SaveOptions == null)
        {
            PrintMenuError("Current game has not been saved. Please select 'Save as...'.");
            return ' ';
        }
        return GameUI.Game.SaveOptions.SaveType == FileRepoUI.Repository.GetSaveType()
            ? FileRepoUI.SaveExistingGame() 
            : DbRepoUI.SaveExistingGame();
    }
}