using MenuSystem;

namespace ConsoleUI;

public class MenuUI
{
    private readonly UIController _base;
    public bool ClearConsole { get; set; } = true;
    
    public MenuUI(UIController b)
    {
        _base = b;
    }
    
    public char RunMenuForUserInput(Menu menu)
    {
        Console.CursorVisible = false;
        var done = false;
        var menuOutput = ' ';
        Console.Clear();
        do
        {
            Console.ForegroundColor = _base.TitleColor;
            Console.WriteLine(menu.Title);
            Console.ForegroundColor = _base.FillerColor;
            Console.WriteLine("=====================");

            Console.ForegroundColor = _base.MainColor;
            var baseItemsSeparated = false;
            foreach (var item in menu.MenuItems)
            {
                if (menu.IsBaseMenuItem(item) && !baseItemsSeparated)
                {
                    Console.ForegroundColor = _base.FillerColor;
                    Console.WriteLine("---------------------");
                    Console.ForegroundColor = _base.MainColor;
                    baseItemsSeparated = true;
                }
                Console.WriteLine(item);
            }
            var userInput = Console.ReadKey().KeyChar;
            try
            {
                menuOutput = menu.ProcessInput(char.ToUpper(userInput)) ?? ' ';
                if (menuOutput.Equals(' ') 
                    || menu.Level.Equals(EMenuLevel.Main) && menuOutput.Equals('M'))
                {
                    if (ClearConsole)
                    {
                        Console.Clear();
                    }
                    else
                    {
                        ClearConsole = true;
                    }
                    continue;
                } 
                if (menu.Level.Equals(EMenuLevel.MoreThanSecond) && menuOutput.Equals('R') 
                    || menu.Level.Equals(EMenuLevel.List) && menuOutput.Equals('P'))
                {
                    return ' ';
                }
                done = true;
            }
            catch (ArgumentException)
            {
                Console.Clear();
                Console.ForegroundColor = _base.ErrorColor;
                Console.WriteLine($"Invalid choice '{userInput}'.");
            }
        } while (!done);
        
        return menuOutput;
    }
}