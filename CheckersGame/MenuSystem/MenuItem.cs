namespace MenuSystem;

public class MenuItem
{
    public string Shortcut { get; }
    private readonly string _title;
    private readonly Func<string>? _methodToRun;

    public MenuItem(string shortcut, string title, Func<string>? methodToRun)
    {
        Shortcut = shortcut;
        _title = title;
        _methodToRun = methodToRun;
    }

    public string? RunMethod() => _methodToRun?.Invoke();

    public override string ToString() => $"({Shortcut}) {_title}";
}