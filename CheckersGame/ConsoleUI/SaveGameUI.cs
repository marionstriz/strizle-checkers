using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleUI;

public class SaveGameUI
{
    private readonly UIController _base;
    private IBrainRepository Repository { get; set; }

    public SaveGameUI(UIController c, IBrainRepository r)
    {
        _base = c;
        Repository = r;
    }
    
    public string SaveNewGame()
    {
        Console.Clear();
        Console.ForegroundColor = _base.MainColor;
        Console.WriteLine("'X' to exit.");
        Console.Write("Save game as: ");
        var input = Console.ReadLine();
        if (input == null || input.Trim().Length == 0)
        {
            _base.PrintMenuError("File name cannot be empty. >:(");
            return "";
        }
        if (input.ToUpper().Equals("X"))
        {
            return "";
        }
        return SaveGame(input, true);
    }
    
    public string SaveExistingGame(string? name) => SaveGame(name, false);

    private string SaveGame(string? name, bool newGame)
    {
        if (name == null)
        {
            _base.PrintMenuError("Current game has not been saved. Please select 'Save as...'.");
            return "";
        }
        if (newGame && Repository.GetBrainFileNames().Contains(name))
        {
            _base.PrintMenuError($"Save with name '{name}' already exists.");
            return "";
        }
        Repository.SaveBrain(_base.GetBrain(), name);
        _base.PrintSuccess($"Game saved with name '{name}'");
        return newGame ? "R" : "";
    }

    public string NewGame(GameOptions options, Menu gameMenu)
    {
        _base.NewGame(options);
        return _base.Menu.RunMenuForUserInput(gameMenu);
    }
    
    public string LoadLoadMenu(Menu loadMenu, Menu gameMenu)
    {
        var menuItems = new List<MenuItem>();
        var nr = 1;
        foreach (var fileName in Repository.GetBrainFileNames())
        {
            var shortcut = nr.ToString();
            menuItems.Add(new MenuItem(shortcut, fileName, 
                () => GetFileOptionsMenu(shortcut, fileName, loadMenu, gameMenu)));
            nr++;
        }
        loadMenu.NewMenuItems(menuItems);

        return _base.Menu.RunMenuForUserInput(loadMenu);
    }
    
    private string DeleteSave(string fileName, string menuShortcut, Menu loadMenu)
    {
        Repository.DeleteBrain(fileName);
        _base.PrintSuccess($"Game '{fileName}' deleted.");
        loadMenu.RemoveMenuItem(loadMenu.GetMenuItemWithShortcut(menuShortcut)!);
        return "R";
    }

    private string GetFileOptionsMenu(string menuShortcut, string fileName, Menu loadMenu, Menu gameMenu)
    {
        var fileActionsMenu = new Menu(EMenuLevel.MoreThanSecond, "File Options", new List<MenuItem>
        {
            new("S", "Start", () => LoadGame(Repository.GetBrain(fileName), gameMenu)),
            new ("D", "Delete", () => DeleteSave(fileName, menuShortcut, loadMenu))
        });
        return _base.Menu.RunMenuForUserInput(fileActionsMenu);
    }
    
    private string LoadGame(CheckersBrain brain, Menu gameMenu){
        _base.LoadGame(brain);
        return _base.Menu.RunMenuForUserInput(gameMenu);
    }
}