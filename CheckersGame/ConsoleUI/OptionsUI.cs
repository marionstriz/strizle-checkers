using GameBrain;

namespace ConsoleUI;

public class OptionsUI
{
    private readonly UIController _base;
    public GameOptions Options { get; set; }
    

    public OptionsUI(UIController c, GameOptions options)
    {
        _base = c;
        Options = options;
    }
    
    public string BoardSizePrompt()
    {
        bool done = false;
        
        Console.Clear();
        while (!done)
        {
            Console.ForegroundColor = _base.MainColor;
            Console.WriteLine("Note that a bigger board size might not fit your screen.");
            Console.WriteLine("'X' to exit.");
            Console.Write("Enter board height: ");
            
            string? heightString = Console.ReadLine()?.Trim();
            if (heightString != null && heightString.ToUpper().Equals("X"))
            {
                return "";
            }
            
            int? height = TryParseBoardDimension(heightString);
            if (height == null)
            {
                continue;
            }

            Console.Write("Enter board width: ");
            string? widthString = Console.ReadLine()?.Trim();
            if (widthString != null && widthString.ToUpper().Equals("X"))
            {
                return "";
            }

            int? width = TryParseBoardDimension(widthString);
            if (width == null)
            {
                continue;
            }
            Options.Height = height.Value;
            Options.Width = width.Value;
            _base.PrintSuccess($"Board size is now {Options.Height}x{Options.Width}");
            done = true;
        }

        return "";
    }

    public string SwitchStartingPlayer()
    {
        Options.PlayerOneStarts = !Options.PlayerOneStarts;
        _base.PrintSuccess($"Starting player changed to Player {(Options.PlayerOneStarts ? "1" : "2")}");
        return "";
    }
    
    public string SwitchCompulsoryJumps()
    {
        Options.CompulsoryJumps = !Options.CompulsoryJumps;
        _base.PrintSuccess($"Compulsory jumps {(Options.CompulsoryJumps ? "activated" : "deactivated")}");
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
            _base.PrintError($"Invalid input {input}, board dimensions must be integers");
            return null;
        }

        if (inputInt < 6 || inputInt > 26)
        {
            _base.PrintError($"Board height and width must be between 6 and 26, was {inputInt}");
            return null;
        }

        return inputInt;
    }
}