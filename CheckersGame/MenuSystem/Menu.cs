using System.Security.AccessControl;

namespace MenuSystem;

public class Menu
{
    private readonly EMenuLevel _level;
    private readonly string _title;
    private readonly List<MenuItem> _menuItems = new();

    public Menu(EMenuLevel level, string title, List<MenuItem>? items)
    {
        _level = level;
        _title = title;
        AddAllMenuItems(items);
    }

    public string RunForUserInput()
    {
        string userInput;
        
        Console.WriteLine(_title);
        Console.WriteLine("=====================");

        foreach (var item in _menuItems)
        {
            Console.WriteLine(item);
        }
        
        Console.WriteLine("---------------------");
        
        Console.Write("Your choice: ");
        userInput = Console.ReadLine()?.ToUpper().Trim() ?? "";
        
        return userInput;
    }

    private void AddAllMenuItems(List<MenuItem>? items)
    {
        if (items != null)
        {
            AddGivenMenuItems(items);
        }
        AddBaseMenuItems();
    }

    private void AddGivenMenuItems(List<MenuItem> items)
    {
        foreach (var item in items)
        {
            _menuItems.Add(item);
        }
    }

    private void AddBaseMenuItems()
    {
        if (_level.Equals(EMenuLevel.MoreThanSecond))
        {
            _menuItems.Add(new MenuItem("R", "Return", null));
        }
        if (!_level.Equals(EMenuLevel.Main))
        {
            _menuItems.Add(new MenuItem("M", "Main Menu", null));
        }
        _menuItems.Add(new MenuItem("X", "Exit", null));
    }
}