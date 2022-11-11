using MenuSystem;
using ConsoleUI;
using DAL.FileSystem;
using GameBrain;

namespace ConsoleApp;

public class MenuSystem
{
    private readonly UIController _ui;
    private readonly BrainFileSystemRepository _fileRepo;
    
    private readonly Menu _loadGameFileMenu = new(EMenuLevel.MoreThanSecond, "Load Game");
    private readonly Menu _saveMenu = new(EMenuLevel.MoreThanSecond, "Save Game");
    private readonly Menu _gameMenu = new(EMenuLevel.Second, "Game Menu");
    private readonly Menu _customOptionsMenu = new(EMenuLevel.MoreThanSecond, "Custom Options");
    private readonly Menu _boardSelectMenu = new(EMenuLevel.MoreThanSecond, "Select Board");
    private readonly Menu _startMenu = new(EMenuLevel.Second, "Start Menu");

    private readonly Menu _mainMenu = new(EMenuLevel.Main, "   _____ _               _                 \n" +
                                                                "  / ____| |             | |                \n" +
                                                                " | |    | |__   ___  ___| | _____ _ __ ___ \n" +
                                                                " | |    | '_ \\ / _ \\/ __| |/ / _ \\ '__/ __|\n" +
                                                                " | |____| | | |  __/ (__|   <  __/ |  \\__ \\\n" +
                                                                "  \\_____|_| |_|\\___|\\___|_|\\_\\___|_|  |___/");

    public MenuSystem()
    {
        _ui = new UIController();
        _fileRepo = new BrainFileSystemRepository();
        AddMenuItems();
    }

    public void RunMainMenu()
    {
        _ui.Menu.RunMenuForUserInput(_mainMenu);
    }

    private void AddMenuItems()
    {
        _saveMenu.NewMenuItems(new List<MenuItem>
        {
            new("F", "Save As File", SaveNewGame),
            new("D", "Save To Database", null)
        });
        _gameMenu.NewMenuItems(new List<MenuItem>
        {
            new("P", "Play", _ui.BrainPlayGame),
            new("S", "Save", () => SaveExistingGame(_ui.GetBrain().FileName)),
            new("F", "Save As...", () => _ui.Menu.RunMenuForUserInput(_saveMenu))
        });
        _customOptionsMenu.NewMenuItems(new List<MenuItem>
        {
            new("B", "Set new board size", _ui.Options.BoardSizePrompt),
            new("P", "Change current starting player", _ui.Options.SwitchStartingPlayer),
            new("A", "Change compulsory jump settings", _ui.Options.SwitchCompulsoryJumps),
            new("S", "Start Game", () => NewGame(_ui.GetOptions()))
        });
        
        _boardSelectMenu.NewMenuItems(new List<MenuItem>
        {
            new ("D", "Default Options", () => NewGame(new GameOptions())),
            new ("C", "Custom Options", () => _ui.StartCustomOptions(_customOptionsMenu))
        });

        _startMenu.NewMenuItems(new List<MenuItem>
        {
            new ("N", "New Game", () => _ui.Menu.RunMenuForUserInput(_boardSelectMenu)),
            new ("L", "Load Game", LoadGamesMenu)
        });
        
        _mainMenu.NewMenuItems(new List<MenuItem>
        {
            new ("S", "Start", () => _ui.Menu.RunMenuForUserInput(_startMenu))
        });
    }

    private string SaveNewGame()
    {
        Console.Clear();
        Console.ForegroundColor = _ui.MainColor;
        Console.WriteLine("'X' to exit.");
        Console.Write("Save game as: ");
        var input = Console.ReadLine();
        if (input == null || input.Trim().Length == 0)
        {
            _ui.PrintMenuError("File name cannot be empty. >:(");
            return "";
        }
        if (input.ToUpper().Equals("X"))
        {
            return "";
        }
        return SaveGame(input, true);
    }

    private string SaveExistingGame(string? name) => SaveGame(name, false);

    private string SaveGame(string? name, bool newGame)
    {
        if (name == null)
        {
            _ui.PrintMenuError("Current game has not been saved. Please select 'Save as...'.");
            return "";
        }
        _fileRepo.SaveNewBrain(_ui.GetBrain(), name);
        _ui.PrintSuccess($"Game saved with name '{name}'");
        return newGame ? "R" : "";
    }

    private string NewGame(GameOptions options)
    {
        _ui.NewGame(options);
        return _ui.Menu.RunMenuForUserInput(_gameMenu);
    }

    private string FileOptionsMenu(string menuShortcut, string fileName)
    {
        var fileActionsMenu = new Menu(EMenuLevel.MoreThanSecond, "File Options", new List<MenuItem>
        {
            new("S", "Start", () => LoadGame(_fileRepo.GetBrain(fileName))),
            new ("D", "Delete", () => DeleteSave(fileName, menuShortcut))
        });
        return _ui.Menu.RunMenuForUserInput(fileActionsMenu);
    }
    
    private string LoadGame(CheckersBrain brain){
        _ui.LoadGame(brain);
        return _ui.Menu.RunMenuForUserInput(_gameMenu);
    }

    private string DeleteSave(string fileName, string menuShortcut)
    {
        _fileRepo.DeleteBrain(fileName);
        _ui.PrintSuccess($"Game '{fileName}' deleted.");
        _loadGameFileMenu.RemoveMenuItem(_loadGameFileMenu.GetMenuItemWithShortcut(menuShortcut)!);
        return "R";
    }

    private string LoadGamesMenu()
    {
        var menuItems = new List<MenuItem>();
        var nr = 1;
        foreach (var fileName in _fileRepo.GetBrainFileNames())
        {
            var shortcut = nr.ToString();
            menuItems.Add(new MenuItem(shortcut, fileName, () => FileOptionsMenu(shortcut, fileName)));
            nr++;
        }
        _loadGameFileMenu.NewMenuItems(menuItems);

        return _ui.Menu.RunMenuForUserInput(_loadGameFileMenu);
    }
}