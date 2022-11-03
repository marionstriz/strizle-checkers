namespace MenuSystem;

public class MenuItem
{
    public string Shortcut { get; }
    public string Title { get; set; }
    private readonly Func<string>? _methodToRun;

    public MenuItem(string shortcut, string title, Func<string>? methodToRun)
    {
        Shortcut = shortcut;
        Title = title;
        _methodToRun = methodToRun;
    }

    public string? RunMethod() => _methodToRun?.Invoke();

    public override string ToString() => $"({Shortcut}) {Title}";
}