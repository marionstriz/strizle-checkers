using MenuSystem;

namespace ConsoleUI;

public static class ConsoleUI
{

    public static string RunMenuForUserInput(Menu menu)
    {
        bool done = false;
        string menuOutput = "";
        do
        {
            Console.WriteLine(menu.Title);
            Console.WriteLine("=====================");

            foreach (var item in menu.MenuItems)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("---------------------");

            Console.Write("Your choice: ");
            try
            {
                menuOutput = menu.ProcessInput(Console.ReadLine()?.ToUpper().Trim() ?? "") ?? "";
                if (menuOutput.Equals(""))
                {
                    continue;
                }
                if (menuOutput.Equals("R"))
                {
                    return "";
                }
                if (menu.Level.Equals(EMenuLevel.Main) && menuOutput.Equals("M"))
                {
                    continue;
                }
                done = true;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        } while (!done);
        
        return menuOutput;
    }

}