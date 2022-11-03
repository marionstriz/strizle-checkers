using GameBrain;
using MenuSystem;

namespace ConsoleUI;

public class CheckersUIController
{
    public ConsoleColor MainColor => ConsoleColor.Blue;
    public ConsoleColor TitleColor => ConsoleColor.DarkCyan;
    public ConsoleColor FillerColor => ConsoleColor.DarkBlue;
    public ConsoleColor ErrorColor => ConsoleColor.Red;
    public ConsoleColor YaaasColor => ConsoleColor.Green;

    private readonly MenuUI _menuUI;
    private CheckersBrainUI? _brainUI;

    public CheckersUIController()
    {
        _menuUI = new MenuUI(this);
    }

    public string RunMenuForUserInput(Menu menu) => _menuUI.RunMenuForUserInput(menu);

    public string StartNewGame(GameOptions gameOptions)
    {
        _brainUI = new CheckersBrainUI(this, new CheckersBrain(gameOptions));
        return _brainUI.PrintBoard();
    }

    public string BoardSizePrompt(GameOptions options)
    {
        bool done = false;
        
        Console.Clear();
        while (!done)
        {
            Console.ForegroundColor = MainColor;
            Console.Write("Enter board height: ");
            string? heightString = Console.ReadLine()?.Trim();
            
            int? height = TryParseBoardDimension(heightString);
            if (height == null)
            {
                continue;
            }

            Console.Write("Enter board width: ");
            string? widthString = Console.ReadLine()?.Trim();
            
            int? width = TryParseBoardDimension(widthString);
            if (width == null)
            {
                continue;
            }
            options.Height = height.Value;
            options.Width = width.Value;
            Console.Clear();
            Console.ForegroundColor = YaaasColor;
            Console.WriteLine($"Board size is now {options.Height}x{options.Width}");
            _menuUI.ClearConsole = false;
            done = true;
        }

        return "";
    }

    public string SwitchStartingPlayer(GameOptions options)
    {
        Console.Clear();
        Console.ForegroundColor = YaaasColor;
        options.PlayerOneStarts = !options.PlayerOneStarts;
        Console.WriteLine($"Starting player changed to Player {(options.PlayerOneStarts ? "1" : "2")}");
        _menuUI.ClearConsole = false;
        return "";
    }
    
    public string SwitchCompulsoryJumps(GameOptions options)
    {
        Console.Clear();
        Console.ForegroundColor = YaaasColor;
        options.CompulsoryJumps = !options.CompulsoryJumps;
        Console.WriteLine($"Compulsory jumps {(options.CompulsoryJumps ? "activated" : "deactivated")}");
        _menuUI.ClearConsole = false;
        return "";
    }

    private int? TryParseBoardDimension(string? input)
    {
        if (input == null)
        {
            throw new NullReferenceException("Congrats, you broke the game! Entered value cannot be null");
        }
        
        bool inputParsed = Int32.TryParse(input!, out int inputInt);
        
        if (!inputParsed)
        {
            Console.Clear();
            Console.ForegroundColor = ErrorColor;
            Console.WriteLine($"Invalid input {input}, board dimensions must be integers");
            return null;
        }

        if (inputInt < 6 || inputInt > 26)
        {
            Console.Clear();
            Console.ForegroundColor = ErrorColor;
            Console.WriteLine($"Board height and width must be between 6 and 26, was {inputInt}");
            return null;
        }

        return inputInt;
    }

}