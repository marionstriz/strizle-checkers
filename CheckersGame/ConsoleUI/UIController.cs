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
    public GameUI? GameUI { get; private set; }
    public RepositoryUI FileRepoUI { get; }
    public RepositoryUI DbRepoUI { get; }

    public UIController(AppDbContext dbContext)
    {
        var options = new GameOptions();
        var fileSystemRepo = new GameFileSystemRepository();
        var dbRepo = new GameDbRepository(dbContext);
        
        MenuUI = new MenuUI(this);
        OptionsUI = new OptionsUI(this, options);
        FileRepoUI = new RepositoryUI(this, fileSystemRepo);
        DbRepoUI = new RepositoryUI(this, dbRepo);
    }

    public char AskForPlayerNames(GameOptions options, Menu gameMenu)
    {
        var p1 = AskForNonBlankInput("Enter name for player 1: ");
        if (p1 == null) return ' ';

        var p2 = p1;
        var clearConsole = true;
        while (p1.Equals(p2))
        {
            p2 = AskForNonBlankInput("Enter name for player 2: ", clearConsole);
            if (p1.Equals(p2)) PrintError("Player names cannot be the same");
            clearConsole = false;
        }

        return p2 == null ? ' ' : NewGame(options, p1, p2, gameMenu);
    }

    public char ContinueGame(Menu gameMenu)
    {
        if (GameUI == null)
        {
            PrintMenuError("No game ongoing. Please load or start a new game.");
            return ' ';
        }
        return MenuUI.RunMenuForUserInput(gameMenu);
    }

    private char NewGame(GameOptions options, string p1, string p2, Menu gameMenu)
    {
        GameUI = new GameUI(this, new CheckersGame(options, p1, p2));
        return MenuUI.RunMenuForUserInput(gameMenu);
    }
    
    public char LoadGame(CheckersGame game, Menu gameMenu){
        GameUI = new GameUI(this, game);
        return MenuUI.RunMenuForUserInput(gameMenu);
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

    public CheckersGame? GetBrain() => GameUI?.Game;

    public GameOptions GetOptions() => OptionsUI.Options;

    public char BrainPlayGame() => GameUI!.PlayGame();

    public string? AskForNonBlankInput(string prompt, bool clearConsole = true)
    {
        bool currClear = clearConsole;
        do
        {
            var input = AskForInput(prompt, currClear);
            if (input == null || input.Trim().Length > 0) return input;
            
            PrintError("Input cannot be empty.");
            currClear = false;
        } while (true);
    }
    
    public string? AskForInput(string prompt, bool clearConsole = true)
    {
        Console.CursorVisible = true;
        
        if (clearConsole) Console.Clear();
        Console.ForegroundColor = MainColor;
        
        Console.WriteLine("'X' to exit.");
        Console.Write(prompt);
        
        var input = Console.ReadLine();
        
        Console.CursorVisible = false;
        if (!"X".Equals(input?.Trim().ToUpper())) return input!;
        return null;
    }

    public char SaveExisting()
    {
        if (GameUI?.Game.SaveOptions == null)
        {
            PrintMenuError("Current game has not been saved. Please select 'Save as...'.");
            return ' ';
        }
        return GameUI.Game.SaveOptions.SaveType == FileRepoUI.Repository.GetSaveType()
            ? FileRepoUI.SaveExistingGame() 
            : DbRepoUI.SaveExistingGame();
    }
}