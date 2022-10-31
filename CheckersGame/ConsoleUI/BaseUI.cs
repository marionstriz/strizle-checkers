using GameBrain;
using MenuSystem;

namespace ConsoleUI;

public class BaseUI
{
    public ConsoleColor MainColor => ConsoleColor.Blue;
    public ConsoleColor TitleColor => ConsoleColor.DarkCyan;
    public ConsoleColor FillerColor => ConsoleColor.DarkBlue;
    public ConsoleColor ErrorColor => ConsoleColor.Red;

    private readonly MenuUI _menuUI;
    private BrainUI? _brainUI;

    public BaseUI()
    {
        _menuUI = new MenuUI(this);
    }

    public string RunMenuForUserInput(Menu menu) => _menuUI.RunMenuForUserInput(menu);

    public string StartNewGame(GameOptions gameOptions)
    {
        _brainUI = new BrainUI(this, new CheckersBrain(gameOptions));
        return _brainUI.PrintBoard();
    }

    public string BoardSizePrompt(GameOptions gameOptions)
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
            gameOptions.Height = height.Value;
            gameOptions.Width = width.Value;
            done = true;
        }

        return "";
    }

    public int? TryParseBoardDimension(string? input)
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