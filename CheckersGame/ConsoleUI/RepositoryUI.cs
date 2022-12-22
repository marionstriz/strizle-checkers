using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleUI;

public class RepositoryUI
{
    private readonly UIController _base;
    public IGameRepository Repository { get; }

    public RepositoryUI(UIController c, IGameRepository r)
    {
        _base = c;
        Repository = r;
    }
    
    public char SaveNewGame()
    {
        var input = _base.AskForNonBlankInput("Save game as: ");
        return input == null ? ' ' : SaveGame(input, true);
    }

    public char LoadGameWithInputName(Menu loadMenu, Menu gameMenu)
    {
        var input = _base.AskForInput("Enter partial or full game save name: ");
        return input == null ? ' ' : LoadLoadMenu(loadMenu, gameMenu, input);
    }
    
    public char SaveExistingGame() => SaveGame(_base.GetBrain()!.SaveOptions?.Name, false);

    private char SaveGame(string? name, bool newGame)
    {
        if (name == null || name.Trim().Length == 0)
        {
            _base.PrintMenuError("Game save name cannot be empty.");
            return ' ';
        }
        if (newGame && Repository.GetGameFileNames().Contains(name))
        {
            _base.PrintMenuError($"Save with name '{name}' already exists.");
            return ' ';
        }
        Repository.SaveGame(_base.GetBrain()!, name);
        _base.PrintSuccess($"Game saved to {Repository.GetSaveType().ToString()} with name '{name}'");
        return newGame ? 'R' : ' ';
    }

    public char LoadLoadMenu(Menu loadMenu, Menu gameMenu, string nameContains = "")
    {
        var fileNames = nameContains != ""
            ? Repository.GetGameFileNamesContaining(nameContains)
            : Repository.GetGameFileNames();
        
        loadMenu.AddListMenuItems(fileNames,
            t => GetFileOptionsMenu(t, loadMenu, gameMenu),
            m => _base.MenuUI.RunMenuForUserInput(m));

        return _base.MenuUI.RunMenuForUserInput(loadMenu);
    }
    
    private char DeleteSave(string fileName, Menu loadMenu)
    {
        Repository.DeleteGame(fileName);
        loadMenu.RemoveMenuItemByTitle(fileName);
        _base.PrintSuccess($"Game '{fileName}' deleted.");
        return 'R';
    }

    private char GetFileOptionsMenu(string fileName, Menu loadMenu, Menu gameMenu)
    {
        Console.WriteLine("in file options menu");
        var fileActionsMenu = new Menu(EMenuLevel.MoreThanSecond, "File Options", new List<MenuItem>
        {
            new('S', "Start", () => _base.LoadGame(Repository.GetGameByName(fileName), gameMenu)),
            new ('D', "Delete", () => DeleteSave(fileName, loadMenu))
        });
        return _base.MenuUI.RunMenuForUserInput(fileActionsMenu);
    }
}