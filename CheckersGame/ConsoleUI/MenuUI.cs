using MenuSystem;

namespace ConsoleUI;

public class MenuUI
{
    private readonly BaseUI _base;
    
    public MenuUI(BaseUI b)
    {
        _base = b;
    }
    
    public string RunMenuForUserInput(Menu menu)
    {
        var done = false;
        var menuOutput = "";
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

            Console.ForegroundColor = _base.FillerColor;
            Console.WriteLine("---------------------");

            Console.ForegroundColor = _base.MainColor;
            Console.Write("Your choice: ");
            var userInput = Console.ReadLine();
            try
            {
                menuOutput = menu.ProcessInput(userInput?.ToUpper().Trim() ?? "") ?? "";
                if (menuOutput.Equals("") 
                    || menu.Level.Equals(EMenuLevel.Main) && menuOutput.Equals("M"))
                {
                    Console.Clear();
                    continue;
                } 
                if (menu.Level.Equals(EMenuLevel.MoreThanSecond) && menuOutput.Equals("R"))
                {
                    return "";
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