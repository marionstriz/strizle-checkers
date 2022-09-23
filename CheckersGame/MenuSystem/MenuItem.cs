using System.Security.AccessControl;

namespace MenuSystem;

public class MenuItem
{
    private string Shortcut { get; }
    private string Title { get; }
    private Action? MethodToRun { get; }

    public MenuItem(string shortcut, string title, Action? methodToRun)
    {
        Shortcut = shortcut;
        Title = title;
        MethodToRun = methodToRun;
    }

    public void RunAction() => MethodToRun?.Invoke();

    public override string ToString() => $"({Shortcut}) {Title}";
}