namespace Tinos3.Graphical.Desktop;

public sealed class DesktopWindow
{
    public DesktopWindow(int id, string title, DesktopPoint position, DesktopSize size)
    {
        if (id < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A window title is required.", nameof(title));
        }

        Id = id;
        Title = title;
        Position = position;
        Size = size;
    }

    public int Id { get; }

    public string Title { get; set; }

    public DesktopPoint Position { get; set; }

    public DesktopSize Size { get; set; }

    public DesktopWindowState State { get; internal set; }

    public bool IsFocused { get; internal set; }
}
