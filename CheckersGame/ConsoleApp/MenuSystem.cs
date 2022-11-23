using MenuSystem;
using ConsoleUI;
using DAL;
using GameBrain;

namespace ConsoleApp
{
    public class MenuSystem
    {
        private readonly UIController _ui;
    
        private readonly Menu _loadMenu = new(EMenuLevel.MoreThanSecond, "Load Game");
        private readonly Menu _loadSourceMenu = new(EMenuLevel.MoreThanSecond, "Load Source");
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

        public MenuSystem(AppDbContext dbContext)
        {
            _ui = new UIController(dbContext);
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
                new("F", "Save As File", _ui.FileRepo.SaveNewGame),
                new("D", "Save To Database", _ui.DbRepo.SaveNewGame)
            });
            _gameMenu.NewMenuItems(new List<MenuItem>
            {
                new("P", "Play", _ui.BrainPlayGame),
                new("S", "Save", _ui.SaveExisting),
                new("A", "Save As...", () => _ui.Menu.RunMenuForUserInput(_saveMenu))
            });
            _customOptionsMenu.NewMenuItems(new List<MenuItem>
            {
                new("B", "Set new board size", _ui.Options.BoardSizePrompt),
                new("P", "Change current starting player", _ui.Options.SwitchStartingPlayer),
                new("A", "Change compulsory jump settings", _ui.Options.SwitchCompulsoryJumps),
                new("S", "Start Game", () => _ui.FileRepo.NewGame(_ui.GetOptions(), _gameMenu))
            });
        
            _boardSelectMenu.NewMenuItems(new List<MenuItem>
            {
                new ("D", "Default Options", () => _ui.FileRepo.NewGame(new GameOptions(), _gameMenu)),
                new ("C", "Custom Options", () => _ui.StartCustomOptions(_customOptionsMenu))
            });
            
            _loadSourceMenu.NewMenuItems(new List<MenuItem>
            {
                new ("F", "Load From File",() => _ui.FileRepo.LoadLoadMenu(_loadMenu, _gameMenu)),
                new ("D", "Load From Database", () => _ui.DbRepo.LoadLoadMenu(_loadMenu, _gameMenu))
            });

            _startMenu.NewMenuItems(new List<MenuItem>
            {
                new ("N", "New Game", () => _ui.Menu.RunMenuForUserInput(_boardSelectMenu)),
                new ("L", "Load Game", () => _ui.Menu.RunMenuForUserInput(_loadSourceMenu))
            });
        
            _mainMenu.NewMenuItems(new List<MenuItem>
            {
                new ("S", "Start", () => _ui.Menu.RunMenuForUserInput(_startMenu))
            });
        }
    }
}