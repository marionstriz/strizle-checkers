using MenuSystem;
using ConsoleUI;
using GameBrain;

namespace ConsoleApp;

public class MenuSystem
{
    private readonly CheckersUIController _ui;
    
    private Menu CustomOptionsMenu = new Menu(EMenuLevel.MoreThanSecond, "Custom Options");
    
    private readonly Menu _mainMenu;

    public MenuSystem(CheckersUIController ui)
    {
        _ui = ui;

        var gameOptions = new GameOptions();
        CustomOptionsMenu.NewMenuItems(new List<MenuItem>
        {
            new("B", $"Set new board size", () => ui.BoardSizePrompt(gameOptions)),
            new("P", "Change current starting player", () => ui.SwitchStartingPlayer(gameOptions)),
            new("A", "Change compulsory jump settings", () => ui.SwitchCompulsoryJumps(gameOptions)),
            new("S", "Start Game", () => ui.StartNewGame(gameOptions))
        });

        var boardSelectMenu = new Menu(EMenuLevel.MoreThanSecond, "Select Board",
            new List<MenuItem>
            {
                new ("D", "Default Options", () => ui.StartNewGame(gameOptions)),
                new ("C", "Custom Options", () => ui.RunMenuForUserInput(CustomOptionsMenu))
            });
        var startMenu = new Menu(EMenuLevel.Second, "Start Menu",
            new List<MenuItem>
            {
                new ("N", "New Game", () => ui.RunMenuForUserInput(boardSelectMenu))
            });
        _mainMenu = new Menu(EMenuLevel.Main, "   _____ _               _                 \n" +
                                              "  / ____| |             | |                \n" +
                                              " | |    | |__   ___  ___| | _____ _ __ ___ \n" +
                                              " | |    | '_ \\ / _ \\/ __| |/ / _ \\ '__/ __|\n" +
                                              " | |____| | | |  __/ (__|   <  __/ |  \\__ \\\n" +
                                              "  \\_____|_| |_|\\___|\\___|_|\\_\\___|_|  |___/", 
            new List<MenuItem>
            {
                new ("S", "Start", () => _ui.RunMenuForUserInput(startMenu))
            });
    }

    public void RunMainMenu()
    {
        _ui.RunMenuForUserInput(_mainMenu);
    }
}