using System.Collections.ObjectModel;

namespace MenuSystem;

public class Menu
{
    public EMenuLevel Level { get; }
    public string Title { get; }
    
    private List<MenuItem> _menuItems = new();
    private HashSet<char> _shortcuts = new();
    private Menu? SubMenu { get; set; }
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

    public Menu(EMenuLevel level, string title)
    {
        Level = level;
        Title = title;
    }

    public char? ProcessInput(char c)
    {
        var menuItem = GetMenuItemByShortcut(c);
        if (menuItem == null)
        {
            throw new ArgumentException($"No menu item with shortcut '{c}' in {Title}");
        }
        return !IsBaseMenuItem(menuItem) ? menuItem.RunMethod() : c;
    }
    
    public bool IsBaseMenuItem(MenuItem menuItem)
    {
        if (!Level.Equals(EMenuLevel.Main) && !Level.Equals(EMenuLevel.Second)
                                           && menuItem.Equals(GetMenuItemByShortcut('R'))
            || !Level.Equals(EMenuLevel.Main) && menuItem.Equals(GetMenuItemByShortcut('M'))) {
            return true;
        }
        return menuItem.Equals(GetMenuItemByShortcut('X'));
    }

    public void NewMenuItems(List<MenuItem>? items)
    {
        ReInitializeFields();
        AddAllMenuItems(items);
    }

    public void AddListMenuItems(List<string> items, Func<string, char> itemFunc,
        Func<Menu, char>? loadMenuFunc = null, int pageNr = 1)
    {
        ReInitializeFields();

        var itemCount = items.Count;
        var nr = 0;
        
        if (itemCount > 10)
        {
            SubMenu = new Menu(EMenuLevel.List, Title);
            
            AddMenuItemAndShortcut(new MenuItem('N', "Next Page",
                loadMenuFunc != null ? () => loadMenuFunc(SubMenu) : null));
            
            SubMenu.AddListMenuItems(items.GetRange(10, itemCount-10),
                itemFunc, loadMenuFunc, pageNr+1);
            
            items = items.GetRange(0, 10);
        }
        if (pageNr > 1)
        {
            Console.WriteLine(pageNr);
            AddMenuItemAndShortcut(new MenuItem('P', "Previous Page", () => 'P'));
        }
        foreach (var item in items)
        {
            var shortcut = char.Parse(nr.ToString());
            AddMenuItemAndShortcut(new MenuItem(shortcut, item, () => itemFunc(item)));
            nr++;
        }
        AddBaseMenuItems();
    }

    public void RemoveMenuItemByTitle(string s)
    {
        var menuItem = GetMenuItemByTitle(s);
        var currMenu = this;
        while (menuItem == null && currMenu.SubMenu != null)
        {
            currMenu = currMenu.SubMenu;
            menuItem = currMenu.GetMenuItemByTitle(s);
        }
        if (menuItem == null)
        {
            throw new ArgumentException("Don't send in item that is not in list.");
        }
        currMenu._menuItems.Remove(menuItem);
        currMenu._shortcuts.Remove(menuItem.Shortcut);
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
        if (Level.Equals(EMenuLevel.MoreThanSecond) || Level.Equals(EMenuLevel.List))
        {
            AddMenuItemAndShortcut(new MenuItem('R', "Return", null));
        }
        if (!Level.Equals(EMenuLevel.Main))
        {
            AddMenuItemAndShortcut(new MenuItem('M', "Main Menu", null));
        }
        AddMenuItemAndShortcut(new MenuItem('X', "Exit", null));
    }

    private void AddMenuItemAndShortcut(MenuItem menuItem)
    {
        if (!_shortcuts.Add(menuItem.Shortcut))
        {
            throw new ArgumentException($"Menu item shortcut '{menuItem.Shortcut}' already in use.");
        }
        _menuItems.Add(menuItem);
    }

    private MenuItem? GetMenuItemByShortcut(char shortcut)
    {
        return _menuItems.FirstOrDefault(m => m.Shortcut.Equals(shortcut));
    }

    private MenuItem? GetMenuItemByTitle(string title)
    {
        return _menuItems.FirstOrDefault(m => m.Title.Equals(title));
    }

    private void ReInitializeFields()
    {
        _menuItems = new List<MenuItem>();
        _shortcuts = new HashSet<char>();
        SubMenu = null;
    }
}