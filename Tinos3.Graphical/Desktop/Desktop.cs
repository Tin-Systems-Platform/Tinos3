using System.Collections.Generic;

namespace Tinos3.Graphical.Desktop;

public sealed class Desktop
{
    private readonly List<DesktopWindow> windows = new();
    private int nextWindowId;

    public Desktop(DesktopSize size)
    {
        Size = size;
    }

    public DesktopSize Size { get; private set; }

    public DesktopWindow? FocusedWindow { get; private set; }

    public IReadOnlyList<DesktopWindow> Windows => windows;

    public DesktopWindow CreateWindow(string title, DesktopPoint position, DesktopSize size)
    {
        var window = new DesktopWindow(nextWindowId++, title, position, size);
        windows.Add(window);
        Focus(window);
        return window;
    }

    public bool RemoveWindow(DesktopWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (!windows.Remove(window))
        {
            return false;
        }

        if (ReferenceEquals(FocusedWindow, window))
        {
            FocusedWindow = null;
            if (windows.Count > 0)
            {
                Focus(windows[windows.Count - 1]);
            }
        }

        return true;
    }

    public bool Focus(DesktopWindow window)
    {
        ArgumentNullException.ThrowIfNull(window);

        if (!windows.Contains(window) || window.State == DesktopWindowState.Minimized)
        {
            return false;
        }

        if (FocusedWindow is not null)
        {
            FocusedWindow.IsFocused = false;
        }

        windows.Remove(window);
        windows.Add(window);
        window.IsFocused = true;
        FocusedWindow = window;
        return true;
    }

    public void Resize(DesktopSize size)
    {
        Size = size;
    }

    public void Render(IDesktopRenderer renderer)
    {
        ArgumentNullException.ThrowIfNull(renderer);

        renderer.BeginFrame(Size);
        foreach (var window in windows)
        {
            if (window.State != DesktopWindowState.Minimized)
            {
                renderer.RenderWindow(window);
            }
        }

        renderer.EndFrame();
    }
}
