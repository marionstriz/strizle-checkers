using MenuSystem;
using ConsoleUI;

namespace ConsoleApp;

public class MenuSystem
{
    private readonly BaseUI _ui;
    
    private readonly Menu _mainMenu;

    public MenuSystem(BaseUI ui)
    {
        _ui = ui;

        var boardSelectMenu = new Menu(EMenuLevel.MoreThanSecond, "Select Board",
            new List<MenuItem>
            {
                new ("8", "8 x 8", () => ui.StartNewGame(8, 8)),
                new ("10", "10 x 10", () => ui.StartNewGame(10, 10))
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