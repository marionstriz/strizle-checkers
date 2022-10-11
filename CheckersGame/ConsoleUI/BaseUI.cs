using GameBrain;
using MenuSystem;

namespace ConsoleUI;

public class BaseUI
{
    public ConsoleColor MainColor => ConsoleColor.Blue;
    public ConsoleColor TitleColor => ConsoleColor.DarkCyan;
    public ConsoleColor FillerColor => ConsoleColor.DarkBlue;
    public ConsoleColor ErrorColor => ConsoleColor.Red;

    private readonly MenuUI _menuUI;
    private BrainUI? _brainUI;

    public BaseUI()
    {
        _menuUI = new MenuUI(this);
    }

    public string RunMenuForUserInput(Menu menu) => _menuUI.RunMenuForUserInput(menu);

    public string StartNewGame(int boardWidth, int boardLength)
    {
        _brainUI = new BrainUI(this, new CheckersBrain(boardWidth, boardLength));
        return _brainUI.PrintBoard();
    }
}