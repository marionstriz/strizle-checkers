namespace MenuSystem;

public class MenuItem
{
    public char Shortcut { get; }
    public string Title { get; set; }
    private readonly Func<char>? _methodToRun;

    public MenuItem(char shortcut, string title, Func<char>? methodToRun)
    {
        Shortcut = shortcut;
        Title = title;
        _methodToRun = methodToRun;
    }

    public char? RunMethod() => _methodToRun?.Invoke();

    public override string ToString() => $"({Shortcut}) {Title}";
}