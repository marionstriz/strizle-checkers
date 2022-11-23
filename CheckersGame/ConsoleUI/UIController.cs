using DAL;
using DAL.FileSystem;
using GameBrain;
using MenuSystem;
using Microsoft.EntityFrameworkCore.Storage;

namespace ConsoleUI;

public class UIController
{
    public ConsoleColor MainColor => ConsoleColor.Blue;
    public ConsoleColor TitleColor => ConsoleColor.DarkCyan;
    public ConsoleColor FillerColor => ConsoleColor.DarkBlue;
    public ConsoleColor ErrorColor => ConsoleColor.Red;
    public ConsoleColor YaaasColor => ConsoleColor.Green;

    public MenuUI Menu { get; }
    public OptionsUI Options { get; }
    public BrainUI Brain { get; private set; }
    public RepositoryUI FileRepo { get; }
    public RepositoryUI DbRepo { get; }

    public UIController(AppDbContext dbContext)
    {
        var options = new GameOptions();
        var defaultBrain = new CheckersBrain(options);
        var fileSystemRepo = new BrainFileSystemRepository();
        var dbRepo = new BrainDbRepository(dbContext);
        
        Menu = new MenuUI(this);
        Options = new OptionsUI(this, options);
        Brain = new BrainUI(this, defaultBrain);
        FileRepo = new RepositoryUI(this, fileSystemRepo);
        DbRepo = new RepositoryUI(this, dbRepo);
    }

    public void NewGame(GameOptions options)
    {
        Brain = new BrainUI(this, new CheckersBrain(options));
    }

    public void LoadGame(CheckersBrain brain)
    {
        Brain = new BrainUI(this, brain);
    }

    public string StartCustomOptions(Menu optionsMenu)
    {
        Options.Options = new GameOptions();
        return Menu.RunMenuForUserInput(optionsMenu);
    }

    public void PrintSuccess(string success)
    {
        Console.Clear();
        Console.ForegroundColor = YaaasColor;
        Console.WriteLine(success);
        Menu.ClearConsole = false;
    }

    public void PrintMenuError(string error) => PrintError(error, true);

    public void PrintError(string error, bool menuError = false)
    {
        Console.Clear();
        Console.ForegroundColor = ErrorColor;
        Console.WriteLine(error);
        if (menuError)
        {
            Menu.ClearConsole = false;
        }
    }

    public CheckersBrain GetBrain() => Brain.Brain;

    public GameOptions GetOptions() => Options.Options;

    public string BrainPlayGame() => Brain.PlayGame();

    public string SaveExisting()
    {
        if (Brain.Brain.SaveOptions == null)
        {
            PrintMenuError("Current game has not been saved. Please select 'Save as...'.");
            return "";
        }
        if (Brain.Brain.SaveOptions.SaveType == FileRepo.Repository.GetSaveType())
        {
            return FileRepo.SaveExistingGame();
        }
        return DbRepo.SaveExistingGame();
    }
}