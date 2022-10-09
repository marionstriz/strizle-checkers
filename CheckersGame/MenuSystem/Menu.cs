using System.Collections.ObjectModel;

namespace MenuSystem;

public class Menu
{
    public EMenuLevel Level { get; }
    public string Title { get; }
    private readonly List<MenuItem> _menuItems = new();
    private readonly HashSet<string> _shortcuts = new();
    public ReadOnlyCollection<MenuItem> MenuItems => _menuItems.AsReadOnly();

    public Menu(EMenuLevel level, string title, List<MenuItem>? items)
    {
        Level = level;
        Title = title;
        try
        {
            AddAllMenuItems(items);
        }
        catch (ArgumentException e)
        {
            throw new ArgumentException($"Unable to create menu. {e.Message}");
        }
    }

    public string? ProcessInput(string input)
    {
        var menuItem = GetMenuItemWithShortcut(input);

        if (menuItem == null)
        {
            throw new ArgumentException($"No menu item with shortcut '{input}' in {Title}");
        }
        return !IsBaseMenuItem(menuItem) ? menuItem.RunMethod() : input;
    }

    private void AddAllMenuItems(List<MenuItem>? items)
    {
        if (items != null)
        {
            AddGivenMenuItems(items);
        }
        AddBaseMenuItems();
    }

    private void AddGivenMenuItems(List<MenuItem> items)
    {
        foreach (var item in items)
        {
            AddMenuItemAndShortcut(item);
        }
    }

    private void AddBaseMenuItems()
    {
        if (Level.Equals(EMenuLevel.MoreThanSecond))
        {
            AddMenuItemAndShortcut(new MenuItem("R", "Return", null));
        }
        if (!Level.Equals(EMenuLevel.Main))
        {
            AddMenuItemAndShortcut(new MenuItem("M", "Main Menu", null));
        }
        AddMenuItemAndShortcut(new MenuItem("X", "Exit", null));
    }

    private void AddMenuItemAndShortcut(MenuItem menuItem)
    {
        if (!_shortcuts.Add(menuItem.Shortcut))
        {
            throw new ArgumentException($"Menu item shortcut '{menuItem.Shortcut}' already in use.");
        }
        _menuItems.Add(menuItem);
    }

    private MenuItem? GetMenuItemWithShortcut(string shortcut)
    {
        return _menuItems.FirstOrDefault(menuItem => menuItem.Shortcut.Equals(shortcut));
    }

    private bool IsBaseMenuItem(MenuItem menuItem)
    {
        if (Level.Equals(EMenuLevel.MoreThanSecond) && menuItem.Shortcut.Equals("R")
            || !Level.Equals(EMenuLevel.Main) && menuItem.Shortcut.Equals("M")) {
            return true;
        }
        return menuItem.Shortcut.Equals("X");
    }
}