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

    public MenuUI Menu { get; }
    public OptionsUI Options { get; }
    public BrainUI Brain { get; private set; }
    public SaveGameUI SaveFile { get; }

    public UIController()
    {
        var options = new GameOptions();
        var defaultBrain = new CheckersBrain(options);
        var fileSystemRepo = new BrainFileSystemRepository();
        Menu = new MenuUI(this);
        Options = new OptionsUI(this, options);
        Brain = new BrainUI(this, defaultBrain);
        SaveFile = new SaveGameUI(this, fileSystemRepo);
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
}