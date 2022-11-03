using MenuSystem;
using ConsoleUI;
using GameBrain;

namespace ConsoleApp;

public class MenuSystem
{
    private readonly UIController _ui;
    
    private readonly Menu _customOptionsMenu = new(EMenuLevel.MoreThanSecond, "Custom Options");
    private readonly Menu _boardSelectMenu = new(EMenuLevel.MoreThanSecond, "Select Board");
    private readonly Menu _startMenu = new(EMenuLevel.Second, "Start Menu");

    private readonly Menu _mainMenu = new(EMenuLevel.Main, "   _____ _               _                 \n" +
                                                                "  / ____| |             | |                \n" +
                                                                " | |    | |__   ___  ___| | _____ _ __ ___ \n" +
                                                                " | |    | '_ \\ / _ \\/ __| |/ / _ \\ '__/ __|\n" +
                                                                " | |____| | | |  __/ (__|   <  __/ |  \\__ \\\n" +
                                                                "  \\_____|_| |_|\\___|\\___|_|\\_\\___|_|  |___/");

    public MenuSystem(UIController ui)
    {
        _ui = ui;
        AddMenuItems();
    }

    public void RunMainMenu()
    {
        _ui.Menu.RunMenuForUserInput(_mainMenu);
    }

    private void AddMenuItems()
    {
        _customOptionsMenu.NewMenuItems(new List<MenuItem>
        {
            new("B", $"Set new board size", _ui.Options.BoardSizePrompt),
            new("P", "Change current starting player", _ui.Options.SwitchStartingPlayer),
            new("A", "Change compulsory jump settings", _ui.Options.SwitchCompulsoryJumps),
            new("S", "Start Game", () => _ui.StartGame(_ui.Options.Options))
        });
        
        _boardSelectMenu.NewMenuItems(new List<MenuItem>
        {
            new ("D", "Default Options", () => _ui.StartGame(new GameOptions())),
            new ("C", "Custom Options", () => _ui.StartCustomOptions(_customOptionsMenu))
        });

        _startMenu.NewMenuItems(new List<MenuItem>
        {
            new ("N", "New Game", () => _ui.Menu.RunMenuForUserInput(_boardSelectMenu))
        });
        
        _mainMenu.NewMenuItems(new List<MenuItem>
        {
            new ("S", "Start", () => _ui.Menu.RunMenuForUserInput(_startMenu))
        });
    }
}