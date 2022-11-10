using System.Security.AccessControl;
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
    private readonly Menu _gameMenu = new(EMenuLevel.MoreThanSecond, "Game Menu");
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
            new("F", "Save To File", SaveGame),
            new("D", "Save To Database", null)
        });
        _gameMenu.NewMenuItems(new List<MenuItem>
        {
            new("P", "Play", _ui.PrintBrainBoard),
            new("S", "Save", () => _ui.Menu.RunMenuForUserInput(_saveMenu))
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
            new ("D", "Default Options", () => _ui.Menu.RunMenuForUserInput(_gameMenu)),
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

    private string SaveGame()
    {
        _fileRepo.SaveBrain(_ui.GetBrain(), "123");
        return "";
    }

    private string NewGame(GameOptions options)
    {
        _ui.NewGame(options);
        return _ui.Menu.RunMenuForUserInput(_gameMenu);
    }

    private string LoadGame(CheckersBrain brain)
    {
        _ui.LoadGame(brain);
        return _ui.Menu.RunMenuForUserInput(_gameMenu);
    }

    private string LoadGamesMenu()
    {
        var menuItems = new List<MenuItem>();
        var nr = 1;
        foreach (var fileName in _fileRepo.GetBrainFileNames())
        {
            menuItems.Add(new MenuItem(nr.ToString(), fileName, () => LoadGame(_fileRepo.GetBrain(fileName))));
            nr++;
        }
        _loadGameFileMenu.NewMenuItems(menuItems);

        return _ui.Menu.RunMenuForUserInput(_loadGameFileMenu);
    }
}