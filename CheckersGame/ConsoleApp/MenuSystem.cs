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
        private readonly Menu _loadByNameSourceMenu = new(EMenuLevel.MoreThanSecond, "By Name:");
        private readonly Menu _loadAllSourceMenu = new(EMenuLevel.MoreThanSecond, "All:");
        private readonly Menu _saveListOptionMenu = new(EMenuLevel.Second, "Show Games:");
        private readonly Menu _saveMenu = new(EMenuLevel.MoreThanSecond, "Save To:");
        private readonly Menu _gameMenu = new(EMenuLevel.Second, "Game Menu");
        private readonly Menu _customOptionsMenu = new(EMenuLevel.MoreThanSecond, "Custom Options");
        private readonly Menu _boardSelectMenu = new(EMenuLevel.Second, "Select Board");

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
            _ui.MenuUI.RunMenuForUserInput(_mainMenu);
        }

        private void AddMenuItems()
        {
            _saveMenu.NewMenuItems(new List<MenuItem>
            {
                new('F', "File", _ui.FileRepoUI.SaveNewGame),
                new('D', "Database", _ui.DbRepoUI.SaveNewGame)
            });
            _gameMenu.NewMenuItems(new List<MenuItem>
            {
                new('P', "Play", _ui.BrainPlayGame),
                new('S', "Save", _ui.SaveExisting),
                new('A', "Save As...", () => _ui.MenuUI.RunMenuForUserInput(_saveMenu))
            });
            _customOptionsMenu.NewMenuItems(new List<MenuItem>
            {
                new('B', "Set new board size", _ui.OptionsUI.BoardSizePrompt),
                new('P', "Change current starting player", _ui.OptionsUI.SwitchStartingPlayer),
                new('A', "Change compulsory jump settings", _ui.OptionsUI.SwitchCompulsoryJumps),
                new('S', "Start Game", () => _ui.FileRepoUI.NewGame(_ui.GetOptions(), _gameMenu))
            });
        
            _boardSelectMenu.NewMenuItems(new List<MenuItem>
            {
                new ('D', "Default Options", () => _ui.FileRepoUI.NewGame(new GameOptions(), _gameMenu)),
                new ('C', "Custom Options", () => _ui.StartCustomOptions(_customOptionsMenu))
            });
            
            _loadByNameSourceMenu.NewMenuItems(new List<MenuItem>
            {
                new ('F', "Files",() => _ui.FileRepoUI.LoadGameWithInputName(_loadMenu, _gameMenu)),
                new ('D', "Database Entries", () => _ui.DbRepoUI.LoadGameWithInputName(_loadMenu, _gameMenu))
            });
            
            _loadAllSourceMenu.NewMenuItems(new List<MenuItem>
            {
                new ('F', "Files",() => _ui.FileRepoUI.LoadLoadMenu(_loadMenu, _gameMenu)),
                new ('D', "Database Entries", () => _ui.DbRepoUI.LoadLoadMenu(_loadMenu, _gameMenu))
            });
            
            _saveListOptionMenu.NewMenuItems(new List<MenuItem>
            {
                new('N', "By Name", () => _ui.MenuUI.RunMenuForUserInput(_loadByNameSourceMenu)),
                new('A', "All", () => _ui.MenuUI.RunMenuForUserInput(_loadAllSourceMenu))
            });

            _mainMenu.NewMenuItems(new List<MenuItem>
            {
                new ('N', "New Game", () => _ui.MenuUI.RunMenuForUserInput(_boardSelectMenu)),
                new ('L', "Load Game", () => _ui.MenuUI.RunMenuForUserInput(_saveListOptionMenu))
            });
        }
    }
}