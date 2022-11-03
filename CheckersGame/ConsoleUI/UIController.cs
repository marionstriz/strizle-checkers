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
    private BrainUI? _brainUI;

    public UIController()
    {
        Menu = new MenuUI(this);
        Options = new OptionsUI(this, new GameOptions());
    }

    public string StartGame(GameOptions options)
    {
        _brainUI = new BrainUI(this, new CheckersBrain(options));
        return _brainUI.PrintBoard();
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

    public void PrintError(string error)
    {
        Console.Clear();
        Console.ForegroundColor = ErrorColor;
        Console.WriteLine(error);
    }
}