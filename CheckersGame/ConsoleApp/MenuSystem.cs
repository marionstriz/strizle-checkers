using MenuSystem;
using ConsoleUI;
using GameBrain;

namespace ConsoleApp;

public class MenuSystem
{
    private readonly BaseUI _ui;
    
    private readonly Menu _mainMenu;

    public MenuSystem(BaseUI ui)
    {
        _ui = ui;

        var gameOptions = new GameOptions();
        var customOptionsMenu = new Menu(EMenuLevel.MoreThanSecond, "Custom Options",
            new List<MenuItem>
            {
                new("B", $"Set new board size", () => ui.BoardSizePrompt(gameOptions)),
                new("P", "Current starting player: Player 1. Select to change", null),
                new("A", "Compulsory jumps: on. Select to change", null),
            });
            
        var boardSelectMenu = new Menu(EMenuLevel.MoreThanSecond, "Select Board",
            new List<MenuItem>
            {
                new ("D", "Default Options", () => ui.StartNewGame(gameOptions)),
                new ("C", "Custom Options", () => ui.RunMenuForUserInput(customOptionsMenu))
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